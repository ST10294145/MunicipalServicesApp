using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunicipalServicesApp
{
    public class Issue
    {
        public int IssueID { get; set; }   // 🔑 Needed to fix the error
        public string Title { get; set; } // Short issue title
        public string Description { get; set; } // Detailed description
        public string Reporter { get; set; } // Name of the person reporting
        public string Email { get; set; } // Contact email
        public string Province { get; set; } // Province of the issue
        public string Category { get; set; } // Category of the issue
        public string FilePath { get; set; } // Path to the image file
        public string Feedback { get; set; } // Feedback from the municipality
        public string Status { get; set; } // Pending, Resolved, etc.
        public DateTime DateReported { get; set; } // When it was created
        public DateTime SLADeadline { get; set; } // SLA deadline

    }
}