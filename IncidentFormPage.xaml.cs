using Microsoft.Maui.Devices.Sensors;
using ChildGuard.Services;
using ChildGuard.Models;

namespace ChildGuard
{
    public partial class IncidentFormPage : ContentPage
    {
        private readonly LocationAndWeatherService _locationService;
        private readonly DatabaseService _databaseService;
        private readonly EmailService _emailService = new EmailService();

        public IncidentFormPage()
        {
            InitializeComponent();
            _locationService = new LocationAndWeatherService();
            _databaseService = new DatabaseService();
            FetchData();
        }

        private async void FetchData()
        {
            try
            {
                // Get current date and time
                DateTimeEntry.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Use the service to fetch location and address
                var (address, _) = await _locationService.GetLocationAndTemperatureAsync();

                if (!string.IsNullOrWhiteSpace(address))
                {
                    AddressEntry.Text = address;
                }
                else
                {
                    AddressEntry.Text = "Unable to fetch address.";
                }

                // Also update latitude and longitude
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    LatitudeEntry.Text = location.Latitude.ToString("F6");
                    LongitudeEntry.Text = location.Longitude.ToString("F6");
                }
                else
                {
                    LatitudeEntry.Text = "N/A";
                    LongitudeEntry.Text = "N/A";
                }
            }
            catch (Exception ex)
            {
                AddressEntry.Text = "Error fetching location.";
                LatitudeEntry.Text = "Error";
                LongitudeEntry.Text = "Error";
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void OnUploadFileClicked(object sender, EventArgs e)
        {
            // Handle file upload logic
            DisplayAlert("Upload", "File upload functionality is not implemented yet.", "OK");
        }

        private async void OnSubmitIncidentClicked(object sender, EventArgs e)
        {
            try
            {
                Loader.IsVisible = true;
                Loader.IsRunning = true;
                var incident = new IncidentReport
                {
                    Name = NameEntry.Text,
                    Description = DescriptionEditor.Text,
                    Type = IncidentTypePicker.SelectedItem?.ToString(),
                    Latitude = LatitudeEntry.Text,
                    Longitude = LongitudeEntry.Text,
                    Address = AddressEntry.Text,
                    DateTime = DateTimeEntry.Text,
                    UploadedFilePath = "AttachmentPathPlaceholder" // Replace with actual path if implemented
                };

                await _databaseService.AddIncidentAsync(incident);

                string recipientEmail = _emailService.GetRecipientEmail(incident.Type);
                if (!string.IsNullOrEmpty(recipientEmail))
                {
                    string subject = $"New Incident Report: {incident.Type}";
                    string body = $@"
                        <p><strong>Incident Report</strong></p>
                        <p><strong>Name:</strong> {incident.Name}</p>
                        <p><strong>Description:</strong> {incident.Description}</p>
                        <p><strong>Type:</strong> {incident.Type}</p>
                        <p><strong>Address:</strong> {incident.Address}</p>
                        <p><strong>Latitude:</strong> {incident.Latitude}</p>
                        <p><strong>Longitude:</strong> {incident.Longitude}</p>
                        <p><strong>Date and Time:</strong> {incident.DateTime}</p>";

                    bool isEmailSent = await _emailService.SendEmailAsync(recipientEmail, subject, body);

                    if (!isEmailSent)
                    {
                        await DisplayAlert("Error", "Failed to send email to the authority.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Warning", "No email address found for the selected incident type.", "OK");
                }
                await DisplayAlert("Incident Submitted",
            $"ID: {incident.Id}\nName: {incident.Name}\nDescription: {incident.Description}\nType: {incident.Type}\nAddress: {incident.Address}",
            "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
              
                await DisplayAlert("Error", "Failed to submit incident.", "OK");
            }
            finally
            {
                // Hide loader
                Loader.IsVisible = false;
                Loader.IsRunning = false;
            }
        }
    }

}
