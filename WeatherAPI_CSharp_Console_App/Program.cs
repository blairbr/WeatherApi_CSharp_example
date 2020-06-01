using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;

namespace WeatherAPI_CSharp_Console_App
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/forecast?zip=48104&appid=3182650f72b53ec159a2efe5b65b5413&units=imperial");

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            // HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.

            if (response.IsSuccessStatusCode)
            {

                var responseContent = response.Content;
                // Parse the response body.
                string responseBody = response.Content.ReadAsStringAsync().Result;

                //var oMycustomclassname = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);
                var weatherData = JsonConvert.DeserializeObject<WeatherObject>(responseBody);

                Console.WriteLine($"It is currently {weatherData.weatherDetails[0].mainWeatherData.temp} degrees in {weatherData.city.Name} with {weatherData.weatherDetails[0].secondaryWeatherData[0].description}.");
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }

    public class WeatherObject
    {
        [JsonProperty("cod")]
        public string ResponseCode { get; set; }

        [JsonProperty("list")]
        public List<WeatherDetails> weatherDetails { get; set; }

        public City city { get; set; }

    }
    public class WeatherDetails
    {
        public string dt { get; set; }

        [JsonProperty("main")]
        public MainWeatherData mainWeatherData { get; set; }

        [JsonProperty("weather")]
        public List<SecondaryWeatherData> secondaryWeatherData { get; set; }

    }

    public class MainWeatherData
    {
        public double temp { get; set; }
        public double humidity { get; set; }
    }

    public class SecondaryWeatherData 
    { 
        public string description { get; set; }
    }

    public class City
    {
        public string Name { get; set; }

        [JsonProperty("coord")]
        public Coordinates Coordinates { get; set; }
        public string Country { get; set; }
        public double Timezone { get; set; }
        public double Sunrise { get; set; }
        public double Sunset { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }
        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}
