using CsvHelper;
using CsvHelper.Configuration;
using HtmlAgilityPack;
using System.Globalization;
using System.Net.Http.Headers;
using TacoParserV2.Logger;
using TacoParserV2.Models;

namespace TacoParserV2;

public abstract class WebScraper_Base
{
    // Url of:  any city in the US
    //          any Canadian city
    //          all Canadian cities
    private string _url { get; set; } = "https://www.tacobell.ca/en/store-locator.html";
    public static readonly ILog logger = new TacoLogger();

    protected WebScraper_Base()
    {
    }

    protected WebScraper_Base(string url)
    {
        _url = url;
    }

    // This method will run the web scraper
    public virtual async Task RunWebScraper()
    {
        List<string> locations = new List<string>();
        try
        {
            locations = await GetTacoBellLocations(_url);
        }
        catch (Exception e)
        {
            logger.LogError($"Error: {e.Message}");
        }

        await ConvertToGeoLocationUsingAPI(locations);
    }

    // This method will convert the location to geo coordinates using an API
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

    // This method will select the location and write to a CSV file
    protected virtual void SelectLocationAndWriteToCSV(List<TacoBellLocation> tbList)
    {
        // Write the data to a CSV file to a location of your choice
        WriteToCsv(@"../../../CSV_Files/TempTacoBellLocations.csv", tbList);
    }

    // This method will extract the Taco Bell locations from the page
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

    // This method will extract the locations from the HTML content
    protected abstract void ExtractLocations(List<string> locations, HtmlNodeCollection locationNodes);

    // This method will select the HTML nodes that contain the location information
    protected abstract HtmlNodeCollection SelectHTMLNodes(HtmlDocument document);

    // This method will load the HTML content into HtmlAgilityPack
    protected virtual HtmlDocument LoadHTMLContent(string response)
    {
        HtmlDocument document = new HtmlDocument();
        try
        {
            document.LoadHtml(response);
        }
        catch (Exception e)
        {
            logger.LogError($"Error: {e.Message}");
        }

        return document;
    }

    // This method will fetch the HTML content of the page
    protected virtual async Task<string> FetchHTMLContent(string url, HttpClient client)
    {
        try
        {
            return await client.GetStringAsync(url);
        }
        catch (HttpRequestException e)
        {
            logger.LogError($"Error: {e.Message}");
        }
        catch (TaskCanceledException e)
        {
            logger.LogError($"Error: {e.Message}");
        }
        catch (Exception e)
        {
            logger.LogError($"Error: {e.Message}");
        }

        return null;
    }

    // This method will set up the HttpClient headers
    protected virtual void SetupHttpClientDefaults(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
    }

    // This method will write the data to a CSV file
    protected virtual void WriteToCsv(string filePath, List<TacoBellLocation> data)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(data);
        }
    }
}
