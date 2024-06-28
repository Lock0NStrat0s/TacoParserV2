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

public class WebScraper_SingleCanadianCity : WebScraper_Base
{
    public WebScraper_SingleCanadianCity(string url) : base(url)
    {
    }

    protected override HtmlNodeCollection SelectHTMLNodes(HtmlDocument document)
    {
        HtmlNodeCollection locationNodes = null;
        try
        {
            //SPECIFIC CITY IN CANADA
            locationNodes = document.DocumentNode.SelectNodes("//div[contains(@class, 'Address-line')]");

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
            bool flag = false;
            string temp = "";
            foreach (var node in locationNodes)
            {
                //Extract the text content of each address node
                var address = node.InnerText.Trim();

                if (!flag)
                {
                    temp += address;
                }
                else
                {
                    temp += ", " + address;
                    locations.Add(temp);
                    temp = "";
                }
                flag = !flag;
            }
        }
        else
        {
            Console.WriteLine("No location nodes found.");
        }
    }
}