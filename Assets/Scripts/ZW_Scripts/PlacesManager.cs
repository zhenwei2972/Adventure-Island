using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

public class PlacesManager
{
    private static readonly string API_KEY = "AIzaSyAzphIBAueM3WnETHq6Vn6Ai5HGG1SiIeU";

    public static async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestNear(double latitude, double longitude, string type, int distance)
    {
        // Create a new HttpClient object.
        HttpClient client = new HttpClient();

        // Create a new HttpRequestMessage object.
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latitude},{longitude}&radius={distance}&type={type}&key={API_KEY}");

        // Send the request and get the response.
        HttpResponseMessage response = await client.SendAsync(request);

        // Check if the response was successful.
        if (response.IsSuccessStatusCode)
        {
            // Read the response body.
            string responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a list of PointOfInterest objects.
            List<PointOfInterest> pointsOfInterest = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PointOfInterest>>(responseBody);

            // Return an IEnumerable of PointOfInterest objects.
            return pointsOfInterest;
        }
        else
        {
            // Throw an exception if the response was not successful.
            throw new Exception($"Failed to get points of interest: {response.StatusCode}");
        }
    }
}
public class PointOfInterest
{
    public string placeId;
    public string name;
    public string type;
    public double latitude;
    public double longitude;
}