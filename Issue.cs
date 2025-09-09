namespace MunicipalServicesApp
{
    public class Issue
    {
        public int IssueID { get; set; }   // ✅ Unique identifier for each issue
        public string Title { get; set; } // Short title of the issue
        public string Description { get; set; } // Detailed description of the issue
        public string Reporter { get; set; } // Name of the person reporting the issue
        public string Email { get; set; } // Email of the reporter
        public string Province { get; set; } // Province where the issue is located
        public string City { get; set; } // City where the issue is located
        public string StreetAddress { get; set; } // Street address of the issue
        public string Category { get; set; } // Category of the issue (e.g., Pothole, Streetlight)
        public string FilePath { get; set; } // Path to the associated file (e.g., image)
        public string Feedback { get; set; } // Feedback or comments on the issue
        public string Status { get; set; } // Current status of the issue (e.g., Open, In Progress, Resolved)
        public DateTime DateReported { get; set; } // Date when the issue was reported
        public DateTime SLADeadline { get; set; } // Service Level Agreement deadline for resolving the issue
    }
}
