using Microsoft.Maui.Devices.Sensors;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChildGuard.Services
{
    public class LocationAndWeatherService
    {
        private const string OpenWeatherApiKey = "52855755e942bc47ccc168fcf35dd2a0"; // Replace with your OpenWeatherMap API key
        private const string OpenWeatherApiUrl = "https://api.openweathermap.org/data/2.5/weather";

        public async Task<(string Address, string Temperature)> GetLocationAndTemperatureAsync()
        {
            try
            {
                // Fetch Location
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    throw new Exception("Unable to fetch location.");
                }

                // Fetch Address (Reverse Geocoding)
                string address = await FetchAddressAsync(location.Latitude, location.Longitude);

                // Fetch Temperature
                string temperature = await FetchTemperatureAsync(location.Latitude, location.Longitude);

                return (address, temperature);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return ("Address not available", "Temperature not available");
            }
        }

        private async Task<string> FetchAddressAsync(double latitude, double longitude)
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    return $"{placemark.SubLocality}, {placemark.Locality}, {placemark.AdminArea}, {placemark.CountryName},{placemark.PostalCode}";
                }

                return "Address not found";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching address: {ex.Message}");
                return " ";
            }
        }

        private async Task<string> FetchTemperatureAsync(double latitude, double longitude)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = $"{OpenWeatherApiUrl}?lat={latitude}&lon={longitude}&units=metric&appid={OpenWeatherApiKey}";
                var response = await httpClient.GetStringAsync(url);

                var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(response);
                return $"{weatherData.Main.Temp:F1}°C";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching temperature: {ex.Message}");
                return "Error fetching temperature";
            }
        }
    }

    // Classes for JSON deserialization
    public class WeatherResponse
    {
        public Main Main { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
    }
}
