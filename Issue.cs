using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServicesApp
{
    public class Issue
    {
        public int Id { get; set; }          // Auto-generated ID
        public string Title { get; set; }    // Short issue title
        public string Description { get; set; } // Detailed description
        public string Status { get; set; }   // Pending, Resolved, etc.
        public DateTime CreatedAt { get; set; } // When it was created
    }
}
