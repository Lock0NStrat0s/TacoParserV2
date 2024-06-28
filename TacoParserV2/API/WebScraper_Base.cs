﻿using CsvHelper.Configuration;
using CsvHelper;
using HtmlAgilityPack;
using TacoParserV2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TacoParserV2;

public abstract class WebScraper_Base
{
    // Url of any city in the US
    // Url of any Canadian city
    // Url of all Canadian cities
    private string _url { get; set; } = "https://www.tacobell.ca/en/store-locator.html";

    protected WebScraper_Base()
    {
    }

    protected WebScraper_Base(string url)
    {
        _url = url;
    }

    public virtual async Task RunWebScraper()
    {
        List<string> locations = new List<string>();
        try
        {
            locations = await GetTacoBellLocations(_url);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        await ConvertToGeoLocationUsingAPI(locations);
    }

    protected virtual async Task ConvertToGeoLocationUsingAPI(List<string> locations)
    {
        List<TacoBellLocation> tbList = new List<TacoBellLocation>();
        foreach (var location in locations)
        {
            var locModel = await API_AddressToCoords.RunAPI(location);

            tbList.Add(locModel);
        }

        SelectLocationAndWriteToCSV(tbList);
    }

    protected virtual void SelectLocationAndWriteToCSV(List<TacoBellLocation> tbList)
    {
        // Write the data to a CSV file to a location of your choice
        WriteToCsv(@"../../../CSV_Files/TempTacoBellLocations.csv", tbList);
    }

    protected virtual async Task<List<string>> GetTacoBellLocations(string url)
    {
        var locations = new List<string>();

        using (HttpClient client = new HttpClient())
        {
            // Set up HttpClient headers
            SetupHttpClientDefaults(client);

            // Fetch the HTML content of the page
            string response = await FetchHTMLContent(url, client);

            // Load the HTML content into HtmlAgilityPack
            HtmlDocument document = LoadHTMLContent(response);

            // Select the nodes that contain the location information
            HtmlNodeCollection locationNodes = SelectHTMLNodes(document);
            
            ExtractLocations(locations, locationNodes);
        }

        return locations;
    }

    protected abstract void ExtractLocations(List<string> locations, HtmlNodeCollection locationNodes);

    protected abstract HtmlNodeCollection SelectHTMLNodes(HtmlDocument document);

    protected virtual HtmlDocument LoadHTMLContent(string response)
    {
        HtmlDocument document = new HtmlDocument();
        try
        {
            document.LoadHtml(response);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        return document;
    }

    protected virtual async Task<string> FetchHTMLContent(string url, HttpClient client)
    {
        try
        {
            return await client.GetStringAsync(url);
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

        return null;
    }

    protected virtual void SetupHttpClientDefaults(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
    }

    protected virtual void WriteToCsv(string filePath, List<TacoBellLocation> data)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(data);
        }
    }
}
