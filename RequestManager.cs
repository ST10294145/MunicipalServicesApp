using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MunicipalServicesApp
{
    public class RequestManager
    {
        private static RequestManager? _instance;
        private static readonly object _lock = new object();

        public List<ServiceRequest> Requests { get; private set; }

        // Trees/Heaps using your existing data structures
        public ServiceRequestBST RequestBST { get; private set; }
        public ServiceRequestHeap RequestHeap { get; private set; }

        public static RequestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new RequestManager();
                    }
                }
                return _instance;
            }
        }

        private RequestManager()
        {
            Requests = new List<ServiceRequest>();
            RequestBST = new ServiceRequestBST();
            RequestHeap = new ServiceRequestHeap();
        }

        public void AddRequest(ServiceRequest request)
        {
            // Generate numeric IssueID if not set
            if (string.IsNullOrEmpty(request.IssueID))
            {
                request.IssueID = (Requests.Count + 1).ToString();
            }

            Requests.Add(request);
            RequestBST.Insert(request);
            RequestHeap.Insert(request);
            SaveToFile();
        }

        // Returns all requests
        public List<ServiceRequest> GetAllRequests()
        {
            return Requests.ToList();
        }

        public void UpdateRequest(ServiceRequest request, string newStatus, string newPriority)
        {
            request.Status = newStatus;
            request.Priority = newPriority;

            // Rebuild heap after priority change
            RequestHeap = new ServiceRequestHeap();
            foreach (var r in Requests)
                RequestHeap.Insert(r);

            SaveToFile();
        }

        public void SaveToFile()
        {
            string json = JsonConvert.SerializeObject(Requests, Formatting.Indented);
            File.WriteAllText("requests.json", json);
        }

        public void LoadFromFile()
        {
            if (!File.Exists("requests.json"))
                return;

            string json = File.ReadAllText("requests.json");
            Requests = JsonConvert.DeserializeObject<List<ServiceRequest>>(json) ?? new List<ServiceRequest>();

            RequestBST = new ServiceRequestBST();
            RequestHeap = new ServiceRequestHeap();

            foreach (var r in Requests)
            {
                RequestBST.Insert(r);
                RequestHeap.Insert(r);
            }
        }
    }
}