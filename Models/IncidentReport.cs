using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeReportingApp.Models
{
    public class IncidentReport
    {
        public string ReportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IncidentType { get; set; }
        public string Location { get; set; }
        public string Status { get; set; } // E.g., "Pending", "Resolved"
        public DateTime DateSubmitted { get; set; }
    }
}
