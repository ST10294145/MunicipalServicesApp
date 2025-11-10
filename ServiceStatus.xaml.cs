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

        private List<ServiceRequest> allRequests = null!;
        private ServiceRequest? selectedRequest;

        public ServiceStatus()
        {
            InitializeComponent();
            Loaded += ServiceStatus_Loaded;
        }

        private void ServiceStatus_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeDataStructures();
            LoadSampleData();
            DisplayAllRequests();
            UpdateStatistics();
            GenerateDependencyGraphVisualization();
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
            allRequests = new List<ServiceRequest>();

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

        private void LoadSampleData()
        {
            var requests = new List<ServiceRequest>
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

            // Add to ALL data structures
            foreach (var request in requests)
            {
                allRequests.Add(request);
                requestBST.Insert(request);
                priorityHeap.Insert(request);
                avlTree.Insert(request.IssueID, request);
                rbTree.Insert(request.IssueID, request);

                // Add to Binary Tree for classification
                classificationTree.Insert($"{request.Priority}:{request.IssueID}");
            }

            BuildDependencyGraph();
        }

        private void BuildDependencyGraph()
        {
            foreach (var request in allRequests)
            {
                dependencyGraph.AddNode(request.IssueID, request);
            }

            dependencyGraph.AddEdge(1003, 1007, 2);
            dependencyGraph.AddEdge(1005, 1001, 3);
            dependencyGraph.AddEdge(1001, 1009, 1);
            dependencyGraph.AddEdge(1007, 1004, 1);
            dependencyGraph.AddEdge(1010, 1006, 4);
        }

        private void DisplayAllRequests()
        {
            dgServiceRequests.ItemsSource = allRequests.OrderBy(r => r.IssueID).ToList();
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
            if (allRequests == null || dgServiceRequests == null ||
                cmbStatusFilter == null || cmbPriorityFilter == null)
            {
                return;
            }

            var filtered = allRequests.AsEnumerable();

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
            dgServiceRequests.ItemsSource = result;

            if (txtStatusBar != null)
            {
                txtStatusBar.Text = $"Displaying {result.Count} of {allRequests.Count} requests";
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
            details.AppendLine($"Date Reported: {request.DateReported:yyyy-MM-dd}");
            details.AppendLine($"Days Open: {request.DaysOpen}");
            details.AppendLine($"SLA Deadline: {request.SLADeadline:yyyy-MM-dd}");
            details.AppendLine();

            var dependencies = dependencyGraph.GetDependencies(request.IssueID);
            if (dependencies.Count > 0)
            {
                details.AppendLine("Dependencies:");
                foreach (var depId in dependencies)
                {
                    var depRequest = allRequests.FirstOrDefault(r => r.IssueID == depId);
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

            foreach (var request in allRequests.OrderBy(r => r.IssueID))
            {
                var dependencies = dependencyGraph.GetDependencies(request.IssueID);

                graph.AppendLine($"[{request.IssueID}] {request.Title}");

                if (dependencies.Count > 0)
                {
                    foreach (var depId in dependencies)
                    {
                        var depRequest = allRequests.FirstOrDefault(r => r.IssueID == depId);
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
                    var req = allRequests.FirstOrDefault(r => r.IssueID == id);
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
                    var req = allRequests.FirstOrDefault(r => r.IssueID == id);
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
                    var from = allRequests.FirstOrDefault(r => r.IssueID == edge.From);
                    var to = allRequests.FirstOrDefault(r => r.IssueID == edge.To);
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

            var avgDaysOpen = allRequests.Average(r => r.DaysOpen);
            stats.AppendLine($"Average Days Open: {avgDaysOpen:F1} days");

            var overdueCount = allRequests.Count(r => DateTime.Now > r.SLADeadline && r.Status != "Resolved" && r.Status != "Closed");
            stats.AppendLine($"Overdue Requests: {overdueCount}");
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

                // Add to ALL data structures
                allRequests.Add(newRequest);
                requestBST.Insert(newRequest);
                priorityHeap.Insert(newRequest);
                avlTree.Insert(newRequest.IssueID, newRequest);
                rbTree.Insert(newRequest.IssueID, newRequest);
                dependencyGraph.AddNode(newRequest.IssueID, newRequest);
                classificationTree.Insert($"{newRequest.Priority}:{newRequest.IssueID}");

                // Refresh all displays
                DisplayAllRequests();
                UpdateStatistics();
                GenerateDependencyGraphVisualization();

                txtStatusBar.Text = $"New request #{newRequest.IssueID} added to all data structures successfully!";
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
            this.Close();
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