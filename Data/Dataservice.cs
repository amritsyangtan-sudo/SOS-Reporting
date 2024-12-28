using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChildGuard.Models;
using SQLite;
using System.IO;


namespace ChildGuard.Data
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "childguard.db");
            _database = new SQLiteAsyncConnection(dbPath);

            // Ensure the tables exist
            _database.CreateTableAsync<IncidentReport>().Wait();
            _database.CreateTableAsync<EmergencyContact>().Wait(); // Create EmergencyContact table
        }


        // Add a new contact
        public Task<int> AddContactAsync(EmergencyContact contact)
        {
            return _database.InsertAsync(contact);
        }

        // Get all contacts
        public Task<List<EmergencyContact>> GetContactsAsync()
        {
            return _database.Table<EmergencyContact>().ToListAsync();
        }

        // Delete a contact
        public Task<int> DeleteContactAsync(EmergencyContact contact)
        {
            return _database.DeleteAsync(contact);
        }
    }
}
