using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using TacoParserV2.Models;
using TacoParserV2.Logger;
using System.Collections.Generic;
using DotNetEnv;

namespace TacoParserV2;

class Program
{
    static readonly ILog logger = new TacoLogger();
    const string csvPath1 = "TacoBellCanada.csv";
    const string csvPath2 = "TacoBellAlabamaLocations.csv";
    static void Main(string[] args)
    {
        // Load the .env file
        string filepath = "secret.env";
        Env.Load(@"../../../" + filepath);

        //string urlCanadianCity = "https://locations.tacobell.ca/en/ab/edmonton";
        string urlCanadianCity = "https://locations.tacobell.ca/en/bc/surrey";
        WebScraper_SingleCanadianCity scraperSingleCan = new WebScraper_SingleCanadianCity(urlCanadianCity);

        string urlAllCanadianCities = "https://www.tacobell.ca/en/store-locator.html";
        WebScraper_ALLCanadianCities scraperAllCan = new WebScraper_ALLCanadianCities(urlAllCanadianCities);

        //string urlUSCity = "https://locations.tacobell.com/al/oxford.html";
        string urlUSCity = "https://locations.tacobell.com/ms/flowood.html";
        WebScraper_SingleUSCity scraperSingleUS = new WebScraper_SingleUSCity(urlUSCity);

        //string urlAllStateCities = "https://locations.tacobell.com/al.html";
        //WebScraper_ALLStateCities scraperAllState = new WebScraper_ALLStateCities(urlAllStateCities);

        try
        {
            scraperSingleUS.RunWebScraper().Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        // Objective: Find the two Taco Bells that are the farthest apart from one another. 

        string[] lines = File.ReadAllLines(@"../../../CSV_Files/" + csvPath2).Skip(1).ToArray();

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
