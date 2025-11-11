using System;
using Newtonsoft.Json;

namespace MunicipalServicesApp
{
    public class ServiceRequest
    {
        public string IssueID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime DateReported { get; set; }
        public string Description { get; set; }
        public string Reporter { get; set; }
        public string Email { get; set; }
        public string StreetAddress { get; set; }

        [JsonIgnore]
        public int DaysOpen => (DateTime.Now - DateReported).Days;

        [JsonIgnore]
        public DateTime SLADeadline
        {
            get
            {
                int daysToAdd = Priority switch
                {
                    "Critical" => 1,
                    "High" => 3,
                    "Medium" => 7,
                    "Low" => 14,
                    _ => 7
                };
                return DateReported.AddDays(daysToAdd);
            }
        }

        public ServiceRequest()
        {
            IssueID = string.Empty;
            Title = string.Empty;
            Category = string.Empty;
            Status = "Pending";
            Priority = "Medium";
            DateReported = DateTime.Now;
            Description = string.Empty;
            Reporter = "Anonymous";
            Email = string.Empty;
            StreetAddress = string.Empty;
        }
    }
}