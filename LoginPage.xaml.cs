using Microsoft.Maui.Controls;

namespace ChildGuard
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var selectedRole = RolePicker.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedRole))
            {
                await DisplayAlert("Error", "Please select a role to proceed.", "OK");
                return;
            }

            if (selectedRole == "End User")
            {
                await Shell.Current.GoToAsync("//MainPage"); // Navigate to End User's Home
            }
            else if (selectedRole == "Authority User")
            {
                await Shell.Current.GoToAsync("//IncidentListPage"); // Navigate to Authority Dashboard
            }
        }
    }
}

