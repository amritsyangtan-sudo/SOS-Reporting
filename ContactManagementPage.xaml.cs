using ChildGuard.Models;
using System.Collections.ObjectModel;
using ChildGuard.Data;

namespace ChildGuard
{
    public partial class ContactManagementPage : ContentPage
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<EmergencyContact> Contacts { get; set; }

        public ContactManagementPage()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
            Contacts = new ObservableCollection<EmergencyContact>();
            LoadContacts();
            ContactsCollection.ItemsSource = Contacts;
        }

        private async void LoadContacts()
        {
            var contacts = await _databaseService.GetContactsAsync();
            Contacts.Clear();
            foreach (var contact in contacts)
            {
                Contacts.Add(contact);
            }
        }

        private async void OnAddContactClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NameEntry.Text) && !string.IsNullOrWhiteSpace(PhoneEntry.Text))
            {
                var newContact = new EmergencyContact
                {
                    Name = NameEntry.Text,
                    PhoneNumber = PhoneEntry.Text
                };

                await _databaseService.AddContactAsync(newContact);
                Contacts.Add(newContact);

                NameEntry.Text = string.Empty;
                PhoneEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Please enter both name and phone number.", "OK");
            }
        }

        private async void OnRemoveContactClicked(object sender, EventArgs e)
        {
            var contact = (EmergencyContact)((Button)sender).CommandParameter;
            if (contact != null)
            {
                await _databaseService.DeleteContactAsync(contact);
                Contacts.Remove(contact);
            }
        }
    }
}
