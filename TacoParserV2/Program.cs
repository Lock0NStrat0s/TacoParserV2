using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using TacoParserV2.Models;
using TacoParserV2.Logger;
using System.Collections.Generic;
using DotNetEnv;
using TacoParserV2.API;

namespace TacoParserV2;

class Program
{
    static readonly ILog logger = new TacoLogger();
    const string csvPath = "TempTacoBellLocations.csv";     // Generated locations of temporary Taco Bell locations using this program
    const string csvPath1 = "TacoBellCanada.csv";           // Generated locations of all Canadian Taco Bell locations
    const string csvPath2 = "TacoBellAlabamaLocations.csv"; // Generated locations of all Alabama Taco Bell locations
    static void Main(string[] args)
    {
        // Load the .env file
        LoadEnv();
        string path = UserSelectionGetRecords();
        FindLocationsFurthestApart(path);
    }

    private static string UserSelectionGetRecords()
    {
        //string urlCanadianCity = "https://locations.tacobell.ca/en/ab/edmonton";
        string urlCanadianCity = "https://locations.tacobell.ca/en/bc/surrey";
        WebScraper_SingleCanadianCity scraperSingleCan = new WebScraper_SingleCanadianCity(urlCanadianCity);

        WebScraper_ALLCanadianCities scraperAllCan = new WebScraper_ALLCanadianCities();

        //string urlUSCity = "https://locations.tacobell.com/al/oxford.html";
        string urlUSCity = "https://locations.tacobell.com/al/decatur.html";
        WebScraper_SingleUSCity scraperSingleUS = new WebScraper_SingleUSCity(urlUSCity);

        try
        {
            //scraperSingleUS.RunWebScraper().Wait();
            scraperAllCan.RunWebScraper().Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return csvPath1;
    }

    private static void LoadEnv()
    {
        string filepath = "secret.env";
        try
        {
            Env.Load(@"../../../" + filepath);
        }
        catch (FileNotFoundException)
        {
            logger.LogError("File not found.");
        }
    }

    private static void FindLocationsFurthestApart(string csvPath)
    {
        // Objective: Find the two Taco Bells that are the farthest apart from one another. 

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
        double locDistance = 0;

        for (int i = 0; i < locations.Length; i++)
        {
            var corA = new GeoCoordinate(locations[i].Location.Latitude, locations[i].Location.Longitude);
            for (int j = i + 1; j < locations.Length; j++)
            {
                var corB = new GeoCoordinate(locations[j].Location.Latitude, locations[j].Location.Longitude);
                if (locDistance < corA.GetDistanceTo(corB))
                {
                    locDistance = corA.GetDistanceTo(corB);
                    locA = locations[i];
                    locB = locations[j];
                }
            }
        }

        //Console.WriteLine($"The two tacobells with the furthest distance are:\nName A: {locA.Name}\nLatitude: {locA.Location.Latitude}\tLongitude: {locA.Location.Longitude}\n\nName B: {locB.Name}\nLatitude: {locB.Location.Latitude}\tLongitude: {locB.Location.Longitude}");
    }
}
