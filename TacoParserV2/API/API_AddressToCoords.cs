using TacoParserV2.Models;
using TacoParserV2.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TacoParserV2.Logger;

public static class API_AddressToCoords
{
    static readonly ILog logger = new TacoLogger();

    // This method will call the API and return the coordinates of the address
    public static async Task<TacoBellLocation> RunAPI(string address)
    {
        // SIGN UP FOR AN ACCOUNT AT https://opencagedata.com/users/sign_up TO GET YOUR API KEY FOR TESTING
        string apiKey = Environment.GetEnvironmentVariable("API_KEY");
        
        TacoBellLocation coordinates = new TacoBellLocation();
        try
        {
            coordinates = await GetCoordinatesFromAddress(address, apiKey);

            logger.LogResults(coordinates);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.ForegroundColor = ConsoleColor.Green;

        return coordinates;
    }

    // This method will call the API and return the coordinates of the address
    private static async Task<TacoBellLocation> GetCoordinatesFromAddress(string address, string apiKey)
    {
        string url = $"https://api.opencagedata.com/geocode/v1/json?q={Uri.EscapeDataString(address)}&key={apiKey}";

        using (HttpClient client = new HttpClient())
        {
            var results = await client.GetStringAsync(url);
            return GetDeserializedModel(results);
        }
    }

    // This method will deserialize the JSON response from the API
    private static TacoBellLocation GetDeserializedModel(string results)
    {
        var response = JsonConvert.DeserializeObject<SingleResponseTacoBellLocationResponse>(results);
        return MapTacoBellLocationModel(response);
    }

    // This method will map the JSON response to the TacoBellLocation model
    private static TacoBellLocation MapTacoBellLocationModel(SingleResponseTacoBellLocationResponse record)
    {
        TacoBellLocation tacoBellLocation = new TacoBellLocation();

        if (record.status.code == 200)
        {
            tacoBellLocation.Name = record.results[0].formatted.Replace(",", "");
            tacoBellLocation.Location = new Point() { Latitude = record.results[0].geometry.lat, Longitude = record.results[0].geometry.lng };
        }
        else
        {
            Console.WriteLine("Geocoding failed: " + record.status.message);
            return null;
        }

        return tacoBellLocation;
    }
}