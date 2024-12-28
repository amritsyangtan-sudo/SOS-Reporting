using SQLite;
using System;
using System.IO;
using Microsoft.Maui.Storage;

namespace CrimeReportingApp.Models
{
    public static class Database
    {
        public static SQLiteConnection GetConnection()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "CrimeReports.db");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Report>();  // Create the table if it doesn't exist
            return db;
        }

        public static void SaveReport(Report report)
        {
            var db = GetConnection();
            db.Insert(report);  // Insert the report into the database
        }

        public static List<Report> GetReports()
        {
            var db = GetConnection();
            return db.Table<Report>().ToList();  // Retrieve all reports from the database
        }
    }
}
