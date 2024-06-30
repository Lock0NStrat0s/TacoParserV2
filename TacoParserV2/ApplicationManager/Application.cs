using DotNetEnv;
using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacoParserV2.API;
using TacoParserV2.Logger;
using TacoParserV2.Models;

namespace TacoParserV2.ApplicationManager;

class Application
{
    static readonly ILog logger = new TacoLogger();
    const string csvPath = "TempTacoBellLocations.csv";     // Generated locations of temporary Taco Bell locations using the web scraper and API
    const string csvPath1 = "TacoBellCanada.csv";           // Generated locations of all Canadian Taco Bell locations
    const string csvPath2 = "TacoBellAlabamaLocations.csv"; // Generated locations of all Alabama Taco Bell locations
    public static bool RunApplication(bool isRunning)
    {
        string userInput = Helper.GetResponse("Welcome to the Taco Bell Location Finder!\n\nOptions:\n1. Find the two Taco Bells that are the farthest apart from one another\n2. Run Web Scraper and Geo Coder API (THIS REQUIRES YOU TO ENTER YOUR OWN API_KEY FOR OPENCAGE GEOCODER - REFER TO README)\n\nYour selection: ");

        if (userInput == "1")
        {
            // Select from CSV files
            SelectFile();
        }
        else if (userInput == "2")
        {
            // Get the records from the user
            GetRecords();
        }
        else
        {
            isRunning = false;
        }

        return isRunning;
    }

    private static void SelectFile()
    {
        string userInput = Helper.GetResponse("Select a file!\n\nOptions:\n1. Temporary List (This can be repopulated if you choose to do so from the previous menu)\n2. All of Canada\n3. All of Alabama (and surrounding area)[DEFAULT]\n\nYour selection: ");

        string csvpath = userInput switch
        {
            "1" => csvPath,
            "2" => csvPath1,
            "3" => csvPath2,
            _ => csvPath2
        };

        FindLocationsFurthestApart(csvpath);
    }

    private static void GetRecords()
    {

        string userInput = Helper.GetResponse("Select Web Scraper to begin geocoding!\n\n1. Single Canadian City\n2. All Canadian Cities (Top 4 will be displayed)\n3. Single US City\n4. All US State Cities (Top 4 will be displayed)\n\nYour selection: ");

        string urlCity = "";
        WebScraper_Base scraper = null;
        bool isAllState = false;

        switch (userInput)
        {
            case "1":
                urlCity = "https://locations.tacobell.ca/en/ab/edmonton";
                scraper = new WebScraper_SingleCanadianCity(urlCity);
                break;
            case "2":
                scraper = new WebScraper_ALLCanadianCities();
                break;
            case "3":
                urlCity = "https://locations.tacobell.com/al/decatur.html";
                scraper = new WebScraper_SingleUSCity(urlCity);
                break;
            case "4":
                isAllState = true;
                break;
            default:
                logger.LogError("Invalid selection. Returning to main menu.");
                return;
        }

        if (!isAllState)
        {
            BeginWebScrapingAndGeocoding(scraper);
        }
        else
        {
            BeginWebScrapingAndGeocoding();
        }
    }

    // Overloaded method for running the web scraper and geocoding for Canadian cities and single US city
    private static async Task BeginWebScrapingAndGeocoding(WebScraper_Base scraper)
    {
        try
        {
            scraper.RunWebScraper().Wait();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    // Overloaded method for running the web scraper and geocoding for all US state cities
    private static async Task BeginWebScrapingAndGeocoding()
    {
        try
        {
            WebScraper_ALLStateCities.RunWebScraper().Wait();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    // Load the .env file
    public static void LoadEnv()
    {
        string filepath = "secret.env";
        try
        {
            Env.Load(@"../../../" + filepath);
        }
        catch (FileNotFoundException)
        {
            logger.LogError("ENV file not found.");
        }
    }

    // Objective: Find the two Taco Bells that are the farthest apart from one another
    private static void FindLocationsFurthestApart(string csvPath)
    {
        string location = csvPath switch
        {
            "TempTacoBellLocations.csv" => "the temporary list",
            "TacoBellCanada.csv" => "Canada",
            "TacoBellAlabamaLocations.csv" => "Alabama",
            _ => "the list"
        };

        string[] lines = File.ReadAllLines(@"../../../CSV_Files/" + csvPath).Skip(1).ToArray();

        if (lines.Length == 0)
        {
            logger.LogError("No lines found in csv file.");
        }
        if (lines.Length == 1)
        {
            logger.LogWarning("Only one line found in csv file.");
        }

        TacoParser parser = new TacoParser();

        ITrackable[] locations = lines.Select(parser.Parse).ToArray();
        ITrackable locA = null;
        ITrackable locB = null;
        GeoCoordinate corA = null;
        GeoCoordinate corB = null;
        double locDistance = 0;

        for (int i = 0; i < locations.Length; i++)
        {
            corA = new GeoCoordinate(locations[i].Location.Latitude, locations[i].Location.Longitude);
            for (int j = i + 1; j < locations.Length; j++)
            {
                corB = new GeoCoordinate(locations[j].Location.Latitude, locations[j].Location.Longitude);
                if (locDistance < corA.GetDistanceTo(corB))
                {
                    locDistance = corA.GetDistanceTo(corB);
                    locA = locations[i];
                    locB = locations[j];
                }
            }
        }

        logger.LogResults($"\nResults:\nThe two tacobells with the furthest distance in {location} are:\n");
        logger.LogResults($"\nName A: {locA.Name}\nLatitude: {locA.Location.Latitude}\tLongitude: {locA.Location.Longitude}\n");
        logger.LogResults($"\nName B: {locB.Name}\nLatitude: {locB.Location.Latitude}\tLongitude: {locB.Location.Longitude}\n");
        logger.LogResults($"\nTheir distance is {locDistance:F2}m");
        Console.ForegroundColor = ConsoleColor.Green;
    }
}
