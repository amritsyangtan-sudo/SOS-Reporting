using ChildGuard.Services;
using System;

namespace ChildGuard
{
    public partial class IncidentListPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public IncidentListPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            LoadIncidents();
        }

        private async void LoadIncidents()
        {
            try
            {
                var incidents = await _databaseService.GetAllIncidentsAsync();

                if (incidents == null || incidents.Count == 0)
                {
                    await DisplayAlert("Incident List", "No incidents found.", "OK");
                }
                else
                {                    
                    IncidentListView.ItemsSource = incidents; // Bind incidents to the ListView
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load incidents: {ex.Message}", "OK");
            }
        }

    }


}

