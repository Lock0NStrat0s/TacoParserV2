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

public class WebScraper_SingleUSCity : WebScraper_Base
{
    public WebScraper_SingleUSCity(string url) : base(url)
    {
    }

    protected override void ExtractLocations(List<string> locations, HtmlNodeCollection locationNodes)
    {
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
            logger.LogError("No location nodes found.");
        }
    }

    protected override HtmlNodeCollection SelectHTMLNodes(HtmlDocument document)
    {
        HtmlNodeCollection locationNodes = null;
        try
        {
            //SPECIFIC CITY IN US
            locationNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'AddressRow')]");

        }
        catch (Exception e)
        {
            logger.LogError($"Error: {e.Message}");
        }

        return locationNodes;
    }
}