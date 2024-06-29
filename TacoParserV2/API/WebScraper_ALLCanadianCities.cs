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

namespace TacoParserV2;

public class WebScraper_ALLCanadianCities : WebScraper_Base
{
    protected override async Task ConvertToGeoLocationUsingAPI(List<string> locations)
    {
        // THIS WILL CONVERT THE FIRST X LOCATIONS IN THE STATE YOU SELECT FOR DEMO PURPOSES
        // TO VIEW ALL, CHANGE THE VALUE OF recordsToExtract TO locations.Count
        List<TacoBellLocation> tbList = new List<TacoBellLocation>();
        int recordsToExtract = 4;
        for (int i = 0; i < recordsToExtract; i++)
        {
            var locModel = await API_AddressToCoords.RunAPI(locations[i]);
            tbList.Add(locModel);
        }
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
            logger.LogError($"Error: {e.Message}");
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
            logger.LogError("No location nodes found.");
        }
    }
}