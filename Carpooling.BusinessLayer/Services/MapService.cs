using Carpooling.BusinessLayer.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace Carpooling.BusinessLayer.Services
{
    public class MapService : IMapService
    {

        private readonly HttpClient client;

        public MapService(HttpClient httpClient)
        {
            this.client = httpClient;
        }

        public async Task<(double travelDistance, double travelDuration)> GetDirection(string originCity, string destinationCity, string country, DateTime departureTime)
        {
            //var startLocaionUrl = GetLocationUrl("Bulgaria", "Sofia");
            //var endLocationUrl = GetLocationUrl("Bulgaria", "Varna");
            var startLocaionUrl = GetLocationUrl(country, originCity);
            var endLocationUrl = GetLocationUrl(country, destinationCity);

            var startLocationCoordinates = await GetLocationCoordinates(startLocaionUrl);
            var endLocationCoordinates = await GetLocationCoordinates(endLocationUrl);

            var matrixRequest = GetDistanceMatrixRequest(startLocationCoordinates, endLocationCoordinates, departureTime);

            var result = await GetTravelInfo(matrixRequest);
            return result;
            //return Distance and Duration of the trip?
        }

        private async Task<CoordinatesResult> GetLocationCoordinates(string url)
        {
            using (HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult())
            {
                using (HttpContent content = response.Content)
                {
                    var json = content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var result = JsonConvert.DeserializeObject<CoordinatesResult>(json);

                    return result;
                }
            }
        }

        private async Task<(double travelDistance, double travelDuration)> GetTravelInfo(string request)
        {
            var url = GetDistanceMatrixUrl();

            HttpContent reqContent = new StringContent(request, Encoding.UTF8, "application/json"); // Adjust the content type as needed

            using (HttpResponseMessage response = client.PostAsync(url, reqContent).GetAwaiter().GetResult())
            {
                using (HttpContent content = response.Content)
                {
                    var json = content.ReadAsStringAsync().GetAwaiter().GetResult();

                    var result = JsonConvert.DeserializeObject<DistanceMatrixResult>(json);

                    var travelDistance = result.ResourceSets[0].Resources[0].Results[0].TravelDistance;
                    var travelDuration = result.ResourceSets[0].Resources[0].Results[0].TravelDuration;

                    return (travelDistance, travelDuration);
                }
            }
        }

        private string GetDistanceMatrixRequest(CoordinatesResult startLocationCoordinates, CoordinatesResult endLocationCoordinates, DateTime startTime)
        {
            var startCoordinates = startLocationCoordinates.ResourceSets[0].Resources[0].Point.Coordinates;
            var endCoordinates = endLocationCoordinates.ResourceSets[0].Resources[0].Point.Coordinates;
            var reqStartTime = startTime.ToString("yyyy-MM-dd"); //Formatting? 

            var req = new DistanceMatrix
            {
                Origins = new List<Origin>
                {
                    //Sofia
                    new Origin
                    {
                        Latitude = startCoordinates[0],
                        Longitude = startCoordinates[1]
                    },
                    //Varna
                    new Origin
                    {
                        Latitude = endCoordinates[0],
                        Longitude = endCoordinates[1]
                    }
                },
                TravelMode = "driving",
                StartTime = reqStartTime
            };

            var requestyAsString = JsonConvert.SerializeObject(req);

            return requestyAsString;
        }

        private string GetLocationUrl(string country, string city)
        {
            return $"http://dev.virtualearth.net/REST/v1/Locations/{country}/{city}?key=AujsdXfNFvcoJsjCMOfZL8341QzAccJT0BD4-jkVUIuY20434G4x0ncSiKl_wWyB";
        }

        private string GetDistanceMatrixUrl()
        {
            return "https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?&key=AujsdXfNFvcoJsjCMOfZL8341QzAccJT0BD4-jkVUIuY20434G4x0ncSiKl_wWyB";
        }

    }
    #region Coordinates
    public class CoordinatesResult
    {
        public ResourceSets[] ResourceSets { get; set; }
    }

    public class ResourceSets
    {
        public Resources[] Resources { get; set; }
    }

    public class Resources
    {
        public string Name { get; set; }

        public Point Point { get; set; }
    }

    public class Point
    {
        public string Type { get; set; }

        public double[] Coordinates { get; set; }
    }
    #endregion

    #region DistanceMatrtix
    public class DistanceMatrix
    {
        public List<Origin> Origins { get; set; }

        public string TravelMode { get; set; }

        public string StartTime { get; set; }
    }

    public class Origin
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class DistanceMatrixResult
    {
        public List<DistanceResourceSet> ResourceSets { get; set; }
    }

    public class DistanceResourceSet
    {
        public List<DistanceResources> Resources { get; set; }
    }

    public class DistanceResources
    {
        public List<DistanceResult> Results { get; set; }
    }

    public class DistanceResult
    {
        public double TravelDistance { get; set; }

        public double TravelDuration { get; set; }
    }



    #endregion



}
