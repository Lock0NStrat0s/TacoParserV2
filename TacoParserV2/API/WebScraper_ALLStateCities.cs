using CsvHelper;
using CsvHelper.Configuration;
using HtmlAgilityPack;
using System.Globalization;
using System.Net.Http.Headers;
using TacoParserV2.Models;

namespace TacoParserV2.API;

public static class WebScraper_ALLStateCities
{
    public static async Task RunWebScraper()
    {
        // MODIFY THIS URL TO THE STATE YOU WANT TO EXTRACT LOCATIONS FROM
        string url = "https://locations.tacobell.com/al.html";

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

        // THIS WILL CONVERT THE FIRST X LOCATIONS IN THE STATE YOU SELECT (FOR DEMO PURPOSES WE ARE USING ALABAMA)
        // TO VIEW ALL, CHANGE THE VALUE OF recordsToExtract TO locations.Count
        int recordsToExtract = 4;
        for (int i = 0; i < recordsToExtract; i++)
        {
            var locModel = await API_AddressToCoords.RunAPI(locations[i]);
            tbList.Add(locModel);
        }

        // WRITE THE DATA TO A CSV FILE IN A LOCATION OF YOUR CHOICE
        WriteToCsv(@"../../../CSV_Files/TempTacoBellLocations.csv", tbList);
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

            HtmlDocument document = await CallStringASyncResponse(url, client);

            // Select the nodes that contain the location information
            HtmlNodeCollection locationNodesOuter = null;
            try
            {
                locationNodesOuter = document.DocumentNode.SelectNodes("//a[contains(@class, 'Directory-listLink')]");

                foreach (var node in locationNodesOuter)
                {
                    string urlInner = "https://locations.tacobell.com/" + node.GetAttributeValue("href", string.Empty);

                    HtmlDocument documentInner = await CallStringASyncResponse(urlInner, client);

                    HtmlNodeCollection locationNodesInner = null;
                    try
                    {
                        //ALL US STATE LOCATIONS
                        locationNodesInner = documentInner.DocumentNode.SelectNodes("//div[contains(@class, 'AddressRow')]");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: {e.Message}");
                    }

                    if (locationNodesInner != null)
                    {
                        int count = 0;
                        string temp = "";

                        // THIS WILL EXTRACT ALL LOCATIONS IN THE STATE YOU SELECT
                        // LIMITED TO 1 REQUEST PER SECOND USING THE FREE VERSION OF THE API
                        foreach (var locNode in locationNodesInner)
                        {
                            temp += locNode.InnerText.Trim() + " ";
                            count++;
                            if (count % 3 == 0)
                            {
                                temp += locNode.InnerText.Trim();
                                locations.Add(temp);
                                temp = "";
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No inner location nodes found.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        return locations;
    }

    private static async Task<HtmlDocument> CallStringASyncResponse(string url, HttpClient client)
    {
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

        return document;
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