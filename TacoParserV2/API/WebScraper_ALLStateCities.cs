using CsvHelper.Configuration;
using CsvHelper;
using HtmlAgilityPack;
using TacoParserV2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;

namespace TacoParserV2;

public class WebScraper_ALLStateCities : WebScraper_Base
{
    Location location = new Location();

    public WebScraper_ALLStateCities(string url) : base(url)
    {
    }

    protected override async Task<List<string>> GetTacoBellLocations(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            SetupHttpClientDefaults(client);

            string response = await FetchHTMLContent(url, client);

            HtmlDocument document = LoadHTMLContent(response);

            SelectHTMLNodesOuter(document, client);
        }

        return location.Loc;
    }

    protected async Task SelectHTMLNodesOuter(HtmlDocument document, HttpClient client)
    {
        try
        {
            //ALL US STATE LOCATIONS
            HtmlNodeCollection locationNodesOuter = document.DocumentNode.SelectNodes("//a[contains(@class, 'Directory-listLink')]");

            foreach (HtmlNode node in locationNodesOuter)
            {
                string urlInner = "https://locations.tacobell.com/" + node.GetAttributeValue("href", string.Empty);

                string response2 = await FetchHTMLContent(urlInner, client);

                HtmlDocument documentInner = LoadHTMLContent(response2);

                HtmlNodeCollection locationNodesInner = SelectHTMLNodes(documentInner);

                List<string> locations = new List<string>();
                ExtractLocations(locations, locationNodesInner);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    protected override void ExtractLocations(List<string> locations, HtmlNodeCollection locationNodes)
    {
        if (locationNodes != null)
        {
            int count = 0;
            string temp = "";

            // THIS WILL DO ALL LOCATIONS
            //foreach (var locNode in locationNodes)    
            //{
            //    temp += locNode.InnerText.Trim() + " ";
            //    count++;
            //    if (count % 3 == 0)
            //    {
            //        temp += locNode.InnerText.Trim();
            //        locations.Add(temp);
            //        temp = "";
            //    }
            //}

            // FOR TESTING PURPOSES, WE WILL ONLY DO THE FIRST 10 LOCATIONS
            for (int i = 0; i < 10; i++)
            {
                temp += locationNodes[i].InnerText.Trim() + " ";
                count++;
                if (count % 3 == 0)
                {
                    temp += locationNodes[i].InnerText.Trim();
                    locations.Add(temp);
                    temp = "";
                }
            }
        }
        else
        {
            Console.WriteLine("No inner location nodes found.");
        }

        Location location = new Location();
        location.Loc = locations;
    }

    protected override HtmlNodeCollection SelectHTMLNodes(HtmlDocument documentInner)
    {
        try
        {
            //ALL STATE CITY LOCATIONS
            return documentInner.DocumentNode.SelectNodes("//div[contains(@class, 'AddressRow')]");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        return null;
    }

    //private static async Task<HtmlDocument> CallStringASyncResponse(string url, HttpClient client)
    //{
    //    // Fetch the HTML content of the page
    //    string response = null;
    //    try
    //    {
    //        response = await client.GetStringAsync(url);
    //    }
    //    catch (HttpRequestException e)
    //    {
    //        Console.WriteLine($"Error: {e.Message}");
    //    }
    //    catch (TaskCanceledException e)
    //    {
    //        Console.WriteLine($"Error: {e.Message}");
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine($"Error: {e.Message}");
    //    }

    //    // Load the HTML content into HtmlAgilityPack
    //    HtmlDocument document = new HtmlDocument();
    //    try
    //    {
    //        document.LoadHtml(response);
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine($"Error: {e.Message}");
    //    }

    //    return document;
    //}
}

public class Location
{
    public List<string> Loc { get; set; }
}