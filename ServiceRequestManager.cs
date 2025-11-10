using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MunicipalServicesApp
{
  
    // Singleton class that manages all service requests across the application.
    public class ServiceRequestManager : INotifyPropertyChanged
    {
        // Singleton instance
        private static ServiceRequestManager? _instance;
        private static readonly object _lock = new object();

        public static ServiceRequestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ServiceRequestManager();
                        }
                    }
                }
                return _instance;
            }
        }

        // All service requests stored here
        private ObservableCollection<ServiceRequest> _allRequests;
        public ObservableCollection<ServiceRequest> AllRequests
        {
            get => _allRequests;
            private set
            {
                _allRequests = value;
                OnPropertyChanged(nameof(AllRequests));
            }
        }

        // Event raised when a new request is added
        public event EventHandler<ServiceRequest>? RequestAdded;

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Private constructor (Singleton pattern)
        private ServiceRequestManager()
        {
            _allRequests = new ObservableCollection<ServiceRequest>();
            LoadSampleData();
        }

     
        /// Add a new service request to the system
        public void AddRequest(ServiceRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Check for duplicate IDs
            if (AllRequests.Any(r => r.IssueID == request.IssueID))
            {
                throw new InvalidOperationException($"Request with ID {request.IssueID} already exists.");
            }

            AllRequests.Add(request);

            // Raise event so other windows can respond
            RequestAdded?.Invoke(this, request);
        }

    
        // Get a request by ID
        public ServiceRequest? GetRequestById(int id)
        {
            return AllRequests.FirstOrDefault(r => r.IssueID == id);
        }


        // Update an existing request
        public void UpdateRequest(ServiceRequest request)
        {
            var existing = GetRequestById(request.IssueID);
            if (existing != null)
            {
                int index = AllRequests.IndexOf(existing);
                AllRequests[index] = request;
            }
        }


        // Delete a request
        public void DeleteRequest(int id)
        {
            var request = GetRequestById(id);
            if (request != null)
            {
                AllRequests.Remove(request);
            }
        }


        // Get the next available ID
        public int GetNextId()
        {
            if (AllRequests.Count == 0)
            {
                return 1001; // Start from 1001
            }

            int maxId = AllRequests.Max(r => r.IssueID);
            return maxId + 1;
        }


        // Load sample data (called once during initialization)
        private void LoadSampleData()
        {
            var sampleRequests = new List<ServiceRequest>
            {
                new ServiceRequest(1001, "Pothole Repair", "Road Maintenance", "Pending", "High",
                    new DateTime(2025, 11, 1), "Large pothole on Main Street causing traffic issues"),
                new ServiceRequest(1002, "Street Light Out", "Utilities", "In Progress", "Medium",
                    new DateTime(2025, 11, 3), "Street light not working on Oak Avenue"),
                new ServiceRequest(1003, "Water Leak", "Water Services", "Resolved", "Critical",
                    new DateTime(2025, 10, 28), "Major water leak affecting multiple homes"),
                new ServiceRequest(1004, "Illegal Dumping", "Sanitation", "Pending", "Medium",
                    new DateTime(2025, 11, 5), "Illegal dumping site near residential area"),
                new ServiceRequest(1005, "Traffic Signal Malfunction", "Traffic Management", "In Progress", "Critical",
                    new DateTime(2025, 11, 2), "Traffic light stuck on red at intersection"),
                new ServiceRequest(1006, "Park Maintenance", "Parks & Recreation", "Pending", "Low",
                    new DateTime(2025, 11, 6), "Broken playground equipment needs repair"),
                new ServiceRequest(1007, "Sewer Blockage", "Sanitation", "Resolved", "High",
                    new DateTime(2025, 10, 25), "Sewer blockage causing overflow"),
                new ServiceRequest(1008, "Noise Complaint", "Public Safety", "Closed", "Low",
                    new DateTime(2025, 10, 20), "Excessive noise from construction site"),
                new ServiceRequest(1009, "Graffiti Removal", "Public Works", "In Progress", "Medium",
                    new DateTime(2025, 11, 4), "Graffiti on public building walls"),
                new ServiceRequest(1010, "Emergency Bridge Repair", "Infrastructure", "Pending", "Critical",
                    new DateTime(2025, 11, 7), "Structural damage to pedestrian bridge")
            };

            foreach (var request in sampleRequests)
            {
                AllRequests.Add(request);
            }
        }


        // Clear all requests (for testing)
        public void ClearAll()
        {
            AllRequests.Clear();
        }


        // Get requests by status
        public IEnumerable<ServiceRequest> GetRequestsByStatus(string status)
        {
            return AllRequests.Where(r => r.Status == status);
        }


        // Get requests by priority
        public IEnumerable<ServiceRequest> GetRequestsByPriority(string priority)
        {
            return AllRequests.Where(r => r.Priority == priority);
        }


        // Get requests by category
        public IEnumerable<ServiceRequest> GetRequestsByCategory(string category)
        {
            return AllRequests.Where(r => r.Category == category);
        }
    }
}