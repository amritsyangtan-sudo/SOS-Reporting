using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;

namespace ChildGuard.Services
{
    public class GeocodingService
    {
        public async Task<string> GetAddressFromLocationAsync(double latitude, double longitude)
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    return $"{placemark.Thoroughfare}, {placemark.Locality}, {placemark.AdminArea}, {placemark.CountryName}, {placemark.PostalCode}";
                }

                return "Address not found";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
