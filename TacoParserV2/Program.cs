using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;
using LoggingKata.API;
using LoggingKata.Models;
using LoggingKata.Logger;
using System.Collections.Generic;
using DotNetEnv;

namespace LoggingKata;

class Program
{
    static readonly ILog logger = new TacoLogger();
    //const string csvPath1 = "TacoBell-US-AL.csv";
    const string csvPath2 = "TacoBellCanada.csv";
    const string csvPath3 = "TacoBellAlabamaLocations.csv";
    static void Main(string[] args)
    {
        /*
        // Load the .env file
        string filepath = "secret.env";
        Env.Load(@"../../../" + filepath);

        try
        {
            WebScraper_ALLStateCities.RunWebScraper().Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }*/

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

        Console.WriteLine($"The two tacobells with the furthest distance are:\nName A: {locA.Name}\nLatitude: {locA.Location.Latitude}\tLongitude: {locA.Location.Longitude}\nName B: {locB.Name}\tLatitude: {locB.Location.Latitude}\tLongitude: {locB.Location.Longitude}\n");
    }
}
