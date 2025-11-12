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

        // ALL DATA STRUCTURES

        // 1. Binary Search Tree - Fast lookup by IssueID
        public ServiceRequestBST RequestBST { get; private set; }

        // 2. Heap - Priority-based ordering (highest priority first)
        public ServiceRequestHeap RequestHeap { get; private set; }

        // 3. Basic Tree - Category hierarchy
        public BasicTree<string> CategoryTree { get; private set; }

        // 4. Graph - Request dependencies and relationships
        public ServiceRequestGraph RequestGraph { get; private set; }

        // 5. Union-Find - Group related requests
        private UnionFind requestGroups;
        private Dictionary<int, int> issueIdToIndex; // Map IssueID to UnionFind index

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
            RequestGraph = new ServiceRequestGraph();
            issueIdToIndex = new Dictionary<int, int>();

            // Initialize UnionFind with capacity for 1000 requests
            requestGroups = new UnionFind(1000);

            // Initialize Category Tree with hierarchical categories
            InitializeCategoryTree();
        }

        private void InitializeCategoryTree()
        {
            // Root: All Municipal Services
            CategoryTree = new BasicTree<string>("Municipal Services");

            // Level 1: Main categories
            var infrastructureNode = new BasicTreeNode<string>("Infrastructure");
            var utilitiesNode = new BasicTreeNode<string>("Utilities");
            var publicSafetyNode = new BasicTreeNode<string>("Public Safety");
            var environmentNode = new BasicTreeNode<string>("Environment");

            CategoryTree.Root.AddChild(infrastructureNode);
            CategoryTree.Root.AddChild(utilitiesNode);
            CategoryTree.Root.AddChild(publicSafetyNode);
            CategoryTree.Root.AddChild(environmentNode);

            // Level 2: Subcategories for Infrastructure
            infrastructureNode.AddChild(new BasicTreeNode<string>("Roads"));
            infrastructureNode.AddChild(new BasicTreeNode<string>("Bridges"));
            infrastructureNode.AddChild(new BasicTreeNode<string>("Sidewalks"));

            // Level 2: Subcategories for Utilities
            utilitiesNode.AddChild(new BasicTreeNode<string>("Water"));
            utilitiesNode.AddChild(new BasicTreeNode<string>("Electricity"));
            utilitiesNode.AddChild(new BasicTreeNode<string>("Streetlights"));

            // Level 2: Subcategories for Public Safety
            publicSafetyNode.AddChild(new BasicTreeNode<string>("Traffic Signals"));
            publicSafetyNode.AddChild(new BasicTreeNode<string>("Street Signs"));
            publicSafetyNode.AddChild(new BasicTreeNode<string>("Hazards"));

            // Level 2: Subcategories for Environment
            environmentNode.AddChild(new BasicTreeNode<string>("Parks"));
            environmentNode.AddChild(new BasicTreeNode<string>("Waste Management"));
            environmentNode.AddChild(new BasicTreeNode<string>("Pollution"));
        }

        public void AddRequest(ServiceRequest request)
        {
            // Generate numeric IssueID if not set
            if (string.IsNullOrEmpty(request.IssueID))
            {
                request.IssueID = (Requests.Count + 1).ToString();
            }

            int issueId = int.Parse(request.IssueID);

            // 1. Add to List
            Requests.Add(request);

            // 2. Add to BST (for fast searching by ID)
            RequestBST.Insert(request);

            // 3. Add to Heap (for priority-based retrieval)
            RequestHeap.Insert(request);

            // 4. Add to Graph (as a node)
            RequestGraph.AddNode(issueId, request);

            // 5. Track in UnionFind (for grouping)
            int requestIndex = Requests.Count - 1;
            issueIdToIndex[issueId] = requestIndex;

            // 6. Create relationships in graph based on location proximity
            CreateGraphRelationships(request, issueId);

            // 7. Group related requests using UnionFind
            GroupRelatedRequests(request, requestIndex);

            SaveToFile();
        }

        private void CreateGraphRelationships(ServiceRequest newRequest, int newIssueId)
        {
            // Create edges between requests in the same category or nearby locations
            foreach (var existingRequest in Requests.Where(r => r.IssueID != newRequest.IssueID))
            {
                if (!int.TryParse(existingRequest.IssueID, out int existingId))
                    continue;

                int weight = CalculateRelationshipWeight(newRequest, existingRequest);

                if (weight > 0)
                {
                    // Add bidirectional edges
                    RequestGraph.AddEdge(newIssueId, existingId, weight);
                    RequestGraph.AddEdge(existingId, newIssueId, weight);
                }
            }
        }

        private int CalculateRelationshipWeight(ServiceRequest req1, ServiceRequest req2)
        {
            int weight = 0;

            // Same category = strong relationship
            if (req1.Category == req2.Category)
                weight += 5;

            // Same priority = moderate relationship
            if (req1.Priority == req2.Priority)
                weight += 3;

            // Same street address = very strong relationship
            if (!string.IsNullOrEmpty(req1.StreetAddress) &&
                !string.IsNullOrEmpty(req2.StreetAddress) &&
                req1.StreetAddress.Equals(req2.StreetAddress, StringComparison.OrdinalIgnoreCase))
                weight += 10;

            // Similar reporter = weak relationship
            if (req1.Email == req2.Email)
                weight += 2;

            return weight;
        }

        private void GroupRelatedRequests(ServiceRequest newRequest, int newIndex)
        {
            // Use UnionFind to group requests with similar characteristics
            for (int i = 0; i < Requests.Count - 1; i++)
            {
                var existingRequest = Requests[i];

                // Group by same category and priority
                if (existingRequest.Category == newRequest.Category &&
                    existingRequest.Priority == newRequest.Priority)
                {
                    requestGroups.Union(i, newIndex);
                }

                // Group by same location
                if (!string.IsNullOrEmpty(existingRequest.StreetAddress) &&
                    !string.IsNullOrEmpty(newRequest.StreetAddress) &&
                    existingRequest.StreetAddress.Equals(newRequest.StreetAddress, StringComparison.OrdinalIgnoreCase))
                {
                    requestGroups.Union(i, newIndex);
                }
            }
        }

        public List<ServiceRequest> GetAllRequests()
        {
            return Requests.ToList();
        }

        // NEW: Search request by ID using BST (O(log n) time complexity)
        public ServiceRequest? SearchByID(int issueId)
        {
            var node = RequestBST.Search(issueId);
            return node?.Data;
        }

        // NEW: Get requests sorted by ID using BST in-order traversal
        public List<ServiceRequest> GetRequestsSortedByID()
        {
            return RequestBST.InOrderTraversal();
        }

        // NEW: Get highest priority request using Heap
        public ServiceRequest? GetHighestPriorityRequest()
        {
            return RequestHeap.ExtractMax();
        }

        // NEW: Get all categories from tree
        public List<string> GetAllCategories()
        {
            return CategoryTree.Traverse();
        }

        // NEW: Find category in tree
        public bool CategoryExists(string category)
        {
            return CategoryTree.Find(category) != null;
        }

        // NEW: Get category tree height
        public int GetCategoryTreeHeight()
        {
            return CategoryTree.GetHeight();
        }

        // NEW: Find related requests using Graph BFS
        public List<ServiceRequest> FindRelatedRequests(int issueId)
        {
            var relatedIds = RequestGraph.BreadthFirstSearch(issueId);
            var relatedRequests = new List<ServiceRequest>();

            foreach (var id in relatedIds)
            {
                if (id != issueId) // Exclude the original request
                {
                    var request = SearchByID(id);
                    if (request != null)
                        relatedRequests.Add(request);
                }
            }

            return relatedRequests;
        }

        // NEW: Get requests in the same group using UnionFind
        public List<ServiceRequest> GetGroupedRequests(int issueId)
        {
            if (!issueIdToIndex.ContainsKey(issueId))
                return new List<ServiceRequest>();

            int requestIndex = issueIdToIndex[issueId];
            int groupRoot = requestGroups.Find(requestIndex);

            var groupedRequests = new List<ServiceRequest>();
            for (int i = 0; i < Requests.Count; i++)
            {
                if (requestGroups.Find(i) == groupRoot)
                {
                    groupedRequests.Add(Requests[i]);
                }
            }

            return groupedRequests;
        }

        // NEW: Get requests by category using tree
        public List<ServiceRequest> GetRequestsByCategory(string category)
        {
            return Requests.Where(r => r.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // NEW: Get statistics about the data structures
        public string GetDataStructureStats()
        {
            return $@"Data Structure Statistics:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
📊 Total Requests: {Requests.Count}
🌳 BST In-Order Count: {RequestBST.InOrderTraversal().Count}
⚡ Heap Count: {RequestHeap.Count}
📂 Category Tree Height: {CategoryTree.GetHeight()}
🗂️  Total Categories: {CategoryTree.Traverse().Count}
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━";
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

            // Rebuild all data structures
            RequestBST = new ServiceRequestBST();
            RequestHeap = new ServiceRequestHeap();
            RequestGraph = new ServiceRequestGraph();
            issueIdToIndex = new Dictionary<int, int>();
            requestGroups = new UnionFind(1000);

            for (int i = 0; i < Requests.Count; i++)
            {
                var r = Requests[i];
                if (int.TryParse(r.IssueID, out int issueId))
                {
                    RequestBST.Insert(r);
                    RequestHeap.Insert(r);
                    RequestGraph.AddNode(issueId, r);
                    issueIdToIndex[issueId] = i;

                    // Recreate relationships
                    CreateGraphRelationships(r, issueId);
                    GroupRelatedRequests(r, i);
                }
            }
        }
    }
}