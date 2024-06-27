using CsvHelper.Configuration;
using CsvHelper;
using HtmlAgilityPack;
using LoggingKata.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http.Headers;

namespace LoggingKata.API;

public static class WebScraper_SingleUSCity
{
    public static async Task RunWebScraper()
    {
        //url of any US city
        string url = "https://locations.tacobell.com/al/oxford.html";

        List<string> locations = new List<string>();
        try
        {
            locations = await GetTacoBellLocations(url);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        List<TacoBellLocation> tbList = new List<TacoBellLocation>();
        foreach (var location in locations)
        {
            var locModel = await API_AddressToCoords.RunAPI(location);

            tbList.Add(locModel);
        }
    }

    private static async Task<List<string>> GetTacoBellLocations(string url)
    {
        var locations = new List<string>();

        using (HttpClient client = new HttpClient())
        {
            // Set up HttpClient headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

            // Fetch the HTML content of the page
            string response = null;
            try
            {
                response = await client.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            // Load the HTML content into HtmlAgilityPack
            HtmlDocument document = new HtmlDocument();
            try
            {
                document.LoadHtml(response);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            // Select the nodes that contain the location information
            HtmlNodeCollection locationNodes = null;
            try
            {
                //SPECIFIC CITY IN US
                locationNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'AddressRow')]");

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            if (locationNodes != null)
            {
                int count = 0;
                string temp = "";
                foreach (var node in locationNodes)
                {
                    temp += node.InnerText.Trim() + " ";
                    count++;
                    if (count % 3 == 0)
                    {
                        temp += node.InnerText.Trim();
                        locations.Add(temp);
                        temp = "";
                    }
                }
            }
            else
            {
                Console.WriteLine("No location nodes found.");
            }
        }

        return locations;
    }

    private static void WriteToCsv(string filePath, List<TacoBellLocation> data)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(data);
        }
    }
}