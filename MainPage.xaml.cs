using Microsoft.Maui.Devices.Sensors;
using ChildGuard.Services;
using ChildGuard.Models;
using ChildGuard.Data;


namespace ChildGuard
{
    public partial class MainPage : ContentPage
    {
        private readonly LocationAndWeatherService _locationAndWeatherService;
        public MainPage()
        {
            InitializeComponent();
            _locationAndWeatherService = new LocationAndWeatherService();
            UpdateLocationAndTemperature();
        }
        private async void UpdateLocationAndTemperature()
        {
            try
            {
                // Use your location service to get the address
                var (address,temperature) = await _locationAndWeatherService.GetLocationAndTemperatureAsync();

                if (!string.IsNullOrEmpty(address))
                {
                    LocationLabel.Text = address;
                }
                else
                {
                    LocationLabel.Text = "Unable to fetch address.";
                }

                // Update the temperature label
                TemperatureLabel.Text = $"{temperature}";
               
            }
            catch (Exception ex)
            {
                //LocationLabel.Text = "";
                Console.WriteLine($"Error fetching location and temperature: {ex.Message}");
            }
        }

        private async void OnSOSButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    var geocodingService = new GeocodingService();
                    var address = await geocodingService.GetAddressFromLocationAsync(location.Latitude, location.Longitude);

                    var smsService = new SmsService();
                    string sosMessage = $"SOS Alert!\nLocation:\nAddress: {address}\nLatitude: {location.Latitude}, Longitude: {location.Longitude}";

                    var databaseService = new ChildGuard.Data.DatabaseService();
                    var contacts = await databaseService.GetContactsAsync();

                    // Collect all phone numbers into an array
                    var phoneNumbers = contacts.Select(contact => contact.PhoneNumber).ToArray();

                    // Send SMS in bulk                
                    bool isSent = await smsService.SendSmsInBulkAsync(sosMessage, phoneNumbers);

                    if (isSent)
                    {
                        await DisplayAlert("SOS Alert", "Proceed to Send", "OK");
                    }
                    else
                    {
                        await DisplayAlert("SOS Alert", "Failed to send SOS alerts.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("SOS Alert", "Unable to fetch location. Please check your device settings.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");

            }

        }
        private async void OnManageContactsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactManagementPage());
        }

        private async void OnReportIncidentClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IncidentFormPage());
        }

        private async void OnViewReportsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("IncidentListPage");
        }
        private  void OnHelplineClicked(object sender, EventArgs e)
        {
            DisplayAlert("HelpLine", "You clicked Helpline Button", "OK");
        }
        private void OnInformationClicked(object sender, EventArgs e)
        {
            DisplayAlert("Information", "You clicked Information Button", "OK");
        }

    }
}

