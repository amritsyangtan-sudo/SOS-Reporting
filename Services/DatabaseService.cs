using SQLite;
using System.IO;
using System.Threading.Tasks;
using ChildGuard.Models;

namespace ChildGuard.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "childguard.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<IncidentReport>().Wait();
        }
        public Task<int> AddIncidentAsync(IncidentReport incident)
        {
            return _database.InsertAsync(incident);
        }

        public Task<List<IncidentReport>> GetAllIncidentsAsync()
        {
            return _database.Table<IncidentReport>().OrderByDescending(i => i.Id).ToListAsync();
        }
 
    }
}
