namespace ChildGuard
{
    public partial class AppShell : Shell
    {
        public Command LogoutCommand { get; }

        public AppShell()
        {
            InitializeComponent();

            // Initialize the Logout Command
            LogoutCommand = new Command(Logout);

            // Set Binding Context to AppShell
            BindingContext = this;

            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("IncidentListPage", typeof(IncidentListPage));
        }

        private async void Logout()
        {
            try
            {
                // Clear navigation stack and navigate to LoginPage
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout failed: {ex.Message}");
            }
        }
    }
}
