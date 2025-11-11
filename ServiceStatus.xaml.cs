using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MunicipalServicesApp
{
    public partial class ServiceStatus : Window
    {
        // ALL Data Structures for Part 3 - Demonstrating Complete Implementation
        private BasicTree<string> categoryTree = null!;
        private BinaryTree<string> classificationTree = null!;
        private ServiceRequestBST requestBST = null!;
        private ServiceRequestHeap priorityHeap = null!;
        private ServiceRequestGraph dependencyGraph = null!;
        private AVLTree<int, ServiceRequest> avlTree = null!;
        private RedBlackTree<int, ServiceRequest> rbTree = null!;

        private ServiceRequest? selectedRequest;

        public ServiceStatus()
        {
            InitializeComponent();
            Loaded += ServiceStatus_Loaded;
        }

        private void ServiceStatus_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeDataStructures();

            // Subscribe to new request events from ServiceRequestManager
            ServiceRequestManager.Instance.RequestAdded += OnRequestAdded;

            // Load all existing requests from ServiceRequestManager into data structures
            LoadAllRequestsIntoDataStructures();

            DisplayAllRequests();
            UpdateStatistics();
            GenerateDependencyGraphVisualization();
        }

        private void OnRequestAdded(object? sender, ServiceRequest newRequest)
        {
            // When a new request is added anywhere in the app, add it to all data structures
            Dispatcher.Invoke(() =>
            {
                AddRequestToDataStructures(newRequest);

                // Refresh all displays
                DisplayAllRequests();
                UpdateStatistics();
                GenerateDependencyGraphVisualization();

                txtStatusBar.Text = $"New request #{newRequest.IssueID} added! Total: {ServiceRequestManager.Instance.AllRequests.Count} requests";
            });
        }

        private void InitializeDataStructures()
        {
            // Initialize ALL data structures
            categoryTree = new BasicTree<string>("Municipal Services");
            classificationTree = new BinaryTree<string>("All Requests");
            requestBST = new ServiceRequestBST();
            priorityHeap = new ServiceRequestHeap();
            dependencyGraph = new ServiceRequestGraph();
            avlTree = new AVLTree<int, ServiceRequest>();
            rbTree = new RedBlackTree<int, ServiceRequest>();

            // Build category hierarchy using Basic Tree
            BuildCategoryHierarchy();
        }

        private void BuildCategoryHierarchy()
        {
            // Demonstrate Basic Tree (N-ary tree) for category organization
            var rootNode = categoryTree.Root;

            // Add main category branches
            var infrastructureNode = new BasicTreeNode<string>("Infrastructure");
            infrastructureNode.AddChild(new BasicTreeNode<string>("Road Maintenance"));
            infrastructureNode.AddChild(new BasicTreeNode<string>("Traffic Management"));
            rootNode.AddChild(infrastructureNode);

            var utilitiesNode = new BasicTreeNode<string>("Utilities");
            utilitiesNode.AddChild(new BasicTreeNode<string>("Water Services"));
            utilitiesNode.AddChild(new BasicTreeNode<string>("Electricity"));
            rootNode.AddChild(utilitiesNode);

            var publicServicesNode = new BasicTreeNode<string>("Public Services");
            publicServicesNode.AddChild(new BasicTreeNode<string>("Sanitation"));
            publicServicesNode.AddChild(new BasicTreeNode<string>("Parks & Recreation"));
            publicServicesNode.AddChild(new BasicTreeNode<string>("Public Safety"));
            rootNode.AddChild(publicServicesNode);
        }

        private void LoadAllRequestsIntoDataStructures()
        {
            // Load all existing requests from ServiceRequestManager
            foreach (var request in ServiceRequestManager.Instance.AllRequests)
            {
                AddRequestToDataStructures(request);
            }

            BuildDependencyGraph();
        }

        private void AddRequestToDataStructures(ServiceRequest request)
        {
            // Add to ALL data structures
            requestBST.Insert(request);
            priorityHeap.Insert(request);
            avlTree.Insert(request.IssueID, request);
            rbTree.Insert(request.IssueID, request);

            // Add to graph if not already there
            if (!dependencyGraph.HasNode(request.IssueID))
            {
                dependencyGraph.AddNode(request.IssueID, request);
            }

            // Add to Binary Tree for classification
            classificationTree.Insert($"{request.Priority}:{request.IssueID}");
        }

        private void BuildDependencyGraph()
        {
            // Build sample dependencies (you can modify this based on your needs)
            var allRequests = ServiceRequestManager.Instance.AllRequests.ToList();

            if (allRequests.Any(r => r.IssueID == 1003) && allRequests.Any(r => r.IssueID == 1007))
                dependencyGraph.AddEdge(1003, 1007, 2);

            if (allRequests.Any(r => r.IssueID == 1005) && allRequests.Any(r => r.IssueID == 1001))
                dependencyGraph.AddEdge(1005, 1001, 3);

            if (allRequests.Any(r => r.IssueID == 1001) && allRequests.Any(r => r.IssueID == 1009))
                dependencyGraph.AddEdge(1001, 1009, 1);

            if (allRequests.Any(r => r.IssueID == 1007) && allRequests.Any(r => r.IssueID == 1004))
                dependencyGraph.AddEdge(1007, 1004, 1);

            if (allRequests.Any(r => r.IssueID == 1010) && allRequests.Any(r => r.IssueID == 1006))
                dependencyGraph.AddEdge(1010, 1006, 4);
        }

        private void DisplayAllRequests()
        {
            var allRequests = ServiceRequestManager.Instance.AllRequests.OrderBy(r => r.IssueID).ToList();
            dgServiceRequests.ItemsSource = null;
            dgServiceRequests.ItemsSource = allRequests;
            txtStatusBar.Text = $"Displaying {allRequests.Count} service requests";
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchID.Text))
            {
                MessageBox.Show("Please enter a Request ID to search.", "Search",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (int.TryParse(txtSearchID.Text, out int requestId))
            {
                // Demonstrate ALL THREE TREE SEARCHES for comparison
                var bstStart = DateTime.Now.Ticks;
                var foundNodeBST = requestBST.Search(requestId);
                var bstTime = DateTime.Now.Ticks - bstStart;

                var avlStart = DateTime.Now.Ticks;
                var foundAVL = avlTree.Search(requestId);
                var avlTime = DateTime.Now.Ticks - avlStart;

                var rbStart = DateTime.Now.Ticks;
                var foundRB = rbTree.Search(requestId);
                var rbTime = DateTime.Now.Ticks - rbStart;

                if (foundNodeBST != null && foundAVL != null && foundRB != null)
                {
                    selectedRequest = foundNodeBST.Data;
                    DisplayRequestDetails(selectedRequest);

                    txtStatusBar.Text = $"Found #{requestId} | BST: {bstTime}μs | AVL: {avlTime}μs | RB: {rbTime}μs | All O(log n)";

                    tabServiceRequests.SelectedIndex = 1;
                }
                else
                {
                    MessageBox.Show($"Request ID {requestId} not found.", "Search Result",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    txtStatusBar.Text = "Request not found";
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric Request ID.", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbPriorityFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (ServiceRequestManager.Instance.AllRequests == null || dgServiceRequests == null ||
                cmbStatusFilter == null || cmbPriorityFilter == null)
            {
                return;
            }

            var filtered = ServiceRequestManager.Instance.AllRequests.AsEnumerable();

            if (cmbStatusFilter.SelectedItem != null)
            {
                var selectedStatus = (cmbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (!string.IsNullOrEmpty(selectedStatus) && selectedStatus != "All Statuses")
                {
                    filtered = filtered.Where(r => r.Status == selectedStatus);
                }
            }

            if (cmbPriorityFilter.SelectedItem != null)
            {
                var selectedPriority = (cmbPriorityFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (!string.IsNullOrEmpty(selectedPriority) && selectedPriority != "All Priorities")
                {
                    filtered = filtered.Where(r => r.Priority == selectedPriority);
                }
            }

            var result = filtered.ToList();
            dgServiceRequests.ItemsSource = null;
            dgServiceRequests.ItemsSource = result;

            if (txtStatusBar != null)
            {
                txtStatusBar.Text = $"Displaying {result.Count} of {ServiceRequestManager.Instance.AllRequests.Count} requests";
            }
        }

        private void dgServiceRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgServiceRequests.SelectedItem is ServiceRequest request)
            {
                selectedRequest = request;
                DisplayRequestDetails(request);
            }
        }

        private void DisplayRequestDetails(ServiceRequest request)
        {
            StringBuilder details = new StringBuilder();
            details.AppendLine("═══════════════════════════════════════════════");
            details.AppendLine($"REQUEST ID: {request.IssueID}");
            details.AppendLine("═══════════════════════════════════════════════");
            details.AppendLine();
            details.AppendLine($"Title: {request.Title}");
            details.AppendLine($"Category: {request.Category}");
            details.AppendLine($"Description: {request.Description}");
            details.AppendLine();
            details.AppendLine($"Status: {request.Status}");
            details.AppendLine($"Priority: {request.Priority}");
            details.AppendLine();
            details.AppendLine($"Reporter: {request.Reporter}");
            details.AppendLine($"Email: {request.Email}");
            details.AppendLine($"Location: {request.StreetAddress}");
            details.AppendLine();
            details.AppendLine($"Date Reported: {request.DateReported:yyyy-MM-dd HH:mm}");
            details.AppendLine($"Days Open: {request.DaysOpen}");
            details.AppendLine($"SLA Deadline: {request.SLADeadline:yyyy-MM-dd HH:mm}");
            details.AppendLine();

            var dependencies = dependencyGraph.GetDependencies(request.IssueID);
            if (dependencies.Count > 0)
            {
                details.AppendLine("Dependencies:");
                foreach (var depId in dependencies)
                {
                    var depRequest = ServiceRequestManager.Instance.GetRequestById(depId);
                    if (depRequest != null)
                    {
                        details.AppendLine($"  → Depends on Request #{depId}: {depRequest.Title} ({depRequest.Status})");
                    }
                }
            }
            else
            {
                details.AppendLine("Dependencies: None");
            }

            txtRequestDetails.Text = details.ToString();
        }

        private void GenerateDependencyGraphVisualization()
        {
            StringBuilder graph = new StringBuilder();
            graph.AppendLine("SERVICE REQUEST DEPENDENCY GRAPH");
            graph.AppendLine("════════════════════════════════════════════════════════════");
            graph.AppendLine();
            graph.AppendLine("Legend: → = depends on");
            graph.AppendLine();

            var allRequests = ServiceRequestManager.Instance.AllRequests.OrderBy(r => r.IssueID).ToList();

            foreach (var request in allRequests)
            {
                var dependencies = dependencyGraph.GetDependencies(request.IssueID);

                graph.AppendLine($"[{request.IssueID}] {request.Title}");

                if (dependencies.Count > 0)
                {
                    foreach (var depId in dependencies)
                    {
                        var depRequest = ServiceRequestManager.Instance.GetRequestById(depId);
                        if (depRequest != null)
                        {
                            graph.AppendLine($"    → Requires: [{depId}] {depRequest.Title}");
                        }
                    }
                }
                else
                {
                    graph.AppendLine("    (No dependencies)");
                }
                graph.AppendLine();
            }

            // DEMONSTRATE GRAPH TRAVERSAL ALGORITHMS (BFS & DFS)
            graph.AppendLine();
            graph.AppendLine("════════════════════════════════════════════════════════════");
            graph.AppendLine("GRAPH TRAVERSAL ALGORITHMS:");
            graph.AppendLine("════════════════════════════════════════════════════════════");
            graph.AppendLine();

            if (allRequests.Any(r => r.IssueID == 1003))
            {
                graph.AppendLine("Starting from Request 1003 (Water Leak):");
                graph.AppendLine();

                // BFS
                var bfsResult = dependencyGraph.BreadthFirstSearch(1003);
                graph.AppendLine("Breadth-First Search (BFS) - Level by level:");
                graph.Append("  Traversal Order: ");
                foreach (var id in bfsResult)
                {
                    graph.Append($"[{id}] ");
                }
                graph.AppendLine();
                graph.AppendLine("  Use Case: Finding shortest dependency path");
                graph.AppendLine("  Complexity: O(V + E)");
                graph.AppendLine();

                // DFS
                var dfsResult = dependencyGraph.DepthFirstSearch(1003);
                graph.AppendLine("Depth-First Search (DFS) - Deep exploration:");
                graph.Append("  Traversal Order: ");
                foreach (var id in dfsResult)
                {
                    graph.Append($"[{id}] ");
                }
                graph.AppendLine();
                graph.AppendLine("  Use Case: Detecting circular dependencies");
                graph.AppendLine("  Complexity: O(V + E)");
                graph.AppendLine();
            }

            // MINIMUM SPANNING TREE
            graph.AppendLine();
            graph.AppendLine("════════════════════════════════════════════════════════════");
            graph.AppendLine("MINIMUM SPANNING TREE (Kruskal's Algorithm):");
            graph.AppendLine("════════════════════════════════════════════════════════════");
            graph.AppendLine("Purpose: Optimal service order minimizing total dependency cost");
            graph.AppendLine();

            var mst = dependencyGraph.GetMinimumSpanningTree();
            if (mst.Count > 0)
            {
                int totalWeight = 0;
                foreach (var edge in mst)
                {
                    var from = ServiceRequestManager.Instance.GetRequestById(edge.From);
                    var to = ServiceRequestManager.Instance.GetRequestById(edge.To);
                    graph.AppendLine($"[{edge.From}] {from?.Title} → [{edge.To}] {to?.Title} (Weight: {edge.Weight})");
                    totalWeight += edge.Weight;
                }
                graph.AppendLine();
                graph.AppendLine($"Total MST Weight: {totalWeight} (Minimum cost path)");
                graph.AppendLine("Uses Union-Find for cycle detection - O(E log E) complexity");
            }

            txtDependencyGraph.Text = graph.ToString();
        }

        private void UpdateStatistics()
        {
            var allRequests = ServiceRequestManager.Instance.AllRequests.ToList();

            StringBuilder stats = new StringBuilder();
            stats.AppendLine("═══════════════════════════════════════════════");
            stats.AppendLine("SERVICE REQUEST STATISTICS");
            stats.AppendLine("═══════════════════════════════════════════════");
            stats.AppendLine();

            stats.AppendLine("Status Breakdown:");
            var statusGroups = allRequests.GroupBy(r => r.Status);
            foreach (var group in statusGroups)
            {
                stats.AppendLine($"  • {group.Key}: {group.Count()} requests");
            }
            stats.AppendLine();

            stats.AppendLine("Priority Breakdown:");
            var priorityGroups = allRequests.GroupBy(r => r.Priority);
            foreach (var group in priorityGroups)
            {
                stats.AppendLine($"  • {group.Key}: {group.Count()} requests");
            }
            stats.AppendLine();

            stats.AppendLine("Category Breakdown:");
            var categoryGroups = allRequests.GroupBy(r => r.Category);
            foreach (var group in categoryGroups)
            {
                stats.AppendLine($"  • {group.Key}: {group.Count()} requests");
            }
            stats.AppendLine();

            // DEMONSTRATE HEAP
            stats.AppendLine("═══════════════════════════════════════════════");
            stats.AppendLine("HIGH PRIORITY QUEUE (Max-Heap):");
            stats.AppendLine("═══════════════════════════════════════════════");
            stats.AppendLine("Heap ensures O(1) access to highest priority item");
            stats.AppendLine();

            var highPriorityRequests = priorityHeap.GetTopPriority(5);
            foreach (var request in highPriorityRequests)
            {
                stats.AppendLine($"  {request.Priority,-10} | [{request.IssueID}] {request.Title}");
            }
            stats.AppendLine();

            if (allRequests.Count > 0)
            {
                var avgDaysOpen = allRequests.Average(r => r.DaysOpen);
                stats.AppendLine($"Average Days Open: {avgDaysOpen:F1} days");

                var overdueCount = allRequests.Count(r => DateTime.Now > r.SLADeadline && r.Status != "Resolved" && r.Status != "Closed");
                stats.AppendLine($"Overdue Requests: {overdueCount}");
            }
            stats.AppendLine();

            // DATA STRUCTURE UTILIZATION DEMONSTRATION
            stats.AppendLine("═══════════════════════════════════════════════");
            stats.AppendLine("DATA STRUCTURE UTILIZATION:");
            stats.AppendLine("═══════════════════════════════════════════════");
            stats.AppendLine();

            stats.AppendLine("1. BASIC TREE (N-ary Tree):");
            stats.AppendLine("   Purpose: Hierarchical category organization");
            var categoryHeight = categoryTree.GetHeight();
            var allCategories = categoryTree.Traverse();
            stats.AppendLine($"   Height: {categoryHeight} levels");
            stats.AppendLine($"   Total Categories: {allCategories.Count}");
            stats.AppendLine("   Used in: Category taxonomy structure");
            stats.AppendLine();

            stats.AppendLine("2. BINARY TREE:");
            stats.AppendLine("   Purpose: Two-child classification structure");
            var binaryHeight = classificationTree.GetHeight();
            stats.AppendLine($"   Height: {binaryHeight} levels");
            stats.AppendLine("   Traversals: Pre-order, In-order, Post-order");
            stats.AppendLine("   Used in: Request classification logic");
            stats.AppendLine();

            stats.AppendLine("3. BINARY SEARCH TREE (BST):");
            stats.AppendLine("   Purpose: Fast search by Request ID");
            stats.AppendLine($"   Stored: {allRequests.Count} requests");
            stats.AppendLine("   Complexity: O(log n) average case");
            stats.AppendLine("   Used in: Primary search functionality");
            stats.AppendLine();

            stats.AppendLine("4. AVL TREE (Self-Balancing BST):");
            stats.AppendLine("   Purpose: Guaranteed O(log n) performance");
            stats.AppendLine($"   Stored: {allRequests.Count} requests");
            stats.AppendLine("   Balance Factor: Maintained at -1, 0, 1");
            stats.AppendLine("   Rotations: Left-Left, Right-Right, Left-Right, Right-Left");
            stats.AppendLine("   Used in: Search performance comparison");
            stats.AppendLine();

            stats.AppendLine("5. RED-BLACK TREE:");
            stats.AppendLine("   Purpose: Alternative self-balancing approach");
            stats.AppendLine($"   Stored: {allRequests.Count} requests");
            stats.AppendLine("   Properties: Red/Black coloring, No adjacent red nodes");
            stats.AppendLine("   Advantage: Fewer rotations than AVL (faster inserts)");
            stats.AppendLine("   Used in: Search performance comparison");
            stats.AppendLine();

            stats.AppendLine("6. MAX-HEAP (Priority Queue):");
            stats.AppendLine("   Purpose: Priority-based request management");
            stats.AppendLine($"   Stored: {priorityHeap.Count} requests");
            stats.AppendLine("   Complexity: O(1) peek, O(log n) insert/extract");
            stats.AppendLine("   Used in: Emergency request prioritization");
            stats.AppendLine();

            stats.AppendLine("7. GRAPH (Adjacency List):");
            stats.AppendLine("   Purpose: Model complex request dependencies");
            stats.AppendLine($"   Nodes: {allRequests.Count} requests");
            var totalEdges = allRequests.Sum(r => dependencyGraph.GetDependencies(r.IssueID).Count);
            stats.AppendLine($"   Edges: {totalEdges} dependencies");
            stats.AppendLine("   Algorithms: BFS (O(V+E)), DFS (O(V+E)), MST (O(E log E))");
            stats.AppendLine("   Used in: Dependency visualization, optimal ordering");

            txtStatistics.Text = stats.ToString();
        }

        private void btnAddRequest_Click(object sender, RoutedEventArgs e)
        {
            AddServiceRequest addWindow = new AddServiceRequest();
            bool? result = addWindow.ShowDialog();

            if (result == true && addWindow.RequestSubmitted && addWindow.NewRequest != null)
            {
                ServiceRequest newRequest = addWindow.NewRequest;

                // Add to ServiceRequestManager (this will trigger OnRequestAdded event)
                ServiceRequestManager.Instance.AddRequest(newRequest);

                txtStatusBar.Text = $"New request #{newRequest.IssueID} added successfully!";
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DisplayAllRequests();
            UpdateStatistics();
            GenerateDependencyGraphVisualization();
            txtStatusBar.Text = "Data refreshed - all structures updated";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Unsubscribe from events
            ServiceRequestManager.Instance.RequestAdded -= OnRequestAdded;
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            // Cleanup: Unsubscribe from events
            ServiceRequestManager.Instance.RequestAdded -= OnRequestAdded;
            base.OnClosed(e);
        }
    }

    public class ServiceRequest : Issue
    {
        public string Priority { get; set; }
        public int DaysOpen => (DateTime.Now - DateReported).Days;

        public ServiceRequest(int id, string title, string category, string status, string priority,
            DateTime dateReported, string description)
        {
            IssueID = id;
            Title = title;
            Category = category;
            Status = status;
            Priority = priority;
            DateReported = dateReported;
            Description = description;

            int slaHours = priority switch
            {
                "Critical" => 4,
                "High" => 24,
                "Medium" => 72,
                "Low" => 168,
                _ => 72
            };
            SLADeadline = dateReported.AddHours(slaHours);
        }

        public int GetPriorityValue()
        {
            return Priority switch
            {
                "Critical" => 4,
                "High" => 3,
                "Medium" => 2,
                "Low" => 1,
                _ => 0
            };
        }
    }
}