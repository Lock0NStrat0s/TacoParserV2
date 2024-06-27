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
using TacoParserV2.API;

namespace LoggingKata.API;

public class WebScraper_ALLCanadianCities : WebScraper_Base
{
    public WebScraper_ALLCanadianCities(string url) : base(url)
    {
    }

    protected override HtmlNodeCollection SelectHTMLNodes(HtmlDocument document)
    {
        HtmlNodeCollection locationNodes = null;
        try
        {
            //ALL CANADIAN LOCATIONS
            locationNodes = document.DocumentNode.SelectNodes("//p[contains(@data-testid, 'store-address')]");

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }

        return locationNodes;
    }

    protected override void ExtractLocations(List<string> locations, HtmlNodeCollection locationNodes)
    {
        if (locationNodes != null)
        {
            foreach (var node in locationNodes)
            {
                var address = node.InnerText.Trim();
                locations.Add(address);
            }
        }
        else
        {
            Console.WriteLine("No location nodes found.");
        }
    }
}