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
        // Data Structures for Part 3
        private ServiceRequestBST requestBST;
        private ServiceRequestHeap priorityHeap;
        private ServiceRequestGraph dependencyGraph;
        private AVLTree<int, ServiceRequest> avlTree;
        private RedBlackTree<int, ServiceRequest> rbTree;

        private List<ServiceRequest> allRequests;
        private ServiceRequest selectedRequest;

        public ServiceStatus()
        {
            InitializeComponent();
            InitializeDataStructures();
            LoadSampleData();
            DisplayAllRequests();
            UpdateStatistics();
            GenerateDependencyGraphVisualization();
        }

        private void InitializeDataStructures()
        {
            requestBST = new ServiceRequestBST();
            priorityHeap = new ServiceRequestHeap();
            dependencyGraph = new ServiceRequestGraph();
            avlTree = new AVLTree<int, ServiceRequest>();
            rbTree = new RedBlackTree<int, ServiceRequest>();
            allRequests = new List<ServiceRequest>();
        }

        private void LoadSampleData()
        {
            // Create sample service requests with various statuses and priorities
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

            // Add to all data structures
            foreach (var request in requests)
            {
                allRequests.Add(request);
                requestBST.Insert(request);
                priorityHeap.Insert(request);
                avlTree.Insert(request.IssueID, request);
                rbTree.Insert(request.IssueID, request);
            }

            // Build dependency graph
            BuildDependencyGraph();
        }

        private void BuildDependencyGraph()
        {
            // Add all requests as nodes
            foreach (var request in allRequests)
            {
                dependencyGraph.AddNode(request.IssueID, request);
            }

            // Define dependencies (some requests depend on others being completed first)
            dependencyGraph.AddEdge(1003, 1007, 2); // Water leak must be fixed before sewer
            dependencyGraph.AddEdge(1005, 1001, 3); // Traffic signal depends on pothole repair
            dependencyGraph.AddEdge(1001, 1009, 1); // Pothole repair before graffiti removal
            dependencyGraph.AddEdge(1007, 1004, 1); // Sewer before illegal dumping cleanup
            dependencyGraph.AddEdge(1010, 1006, 4); // Bridge repair before park maintenance
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
                // Demonstrate BST search
                var foundNode = requestBST.Search(requestId);

                if (foundNode != null)
                {
                    selectedRequest = foundNode.Data;
                    DisplayRequestDetails(selectedRequest);
                    txtStatusBar.Text = $"Found Request ID: {requestId} using Binary Search Tree (O(log n) complexity)";

                    // Switch to details tab
                    ((TabControl)((Grid)dgServiceRequests.Parent).Parent).SelectedIndex = 1;
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
            var filtered = allRequests.AsEnumerable();

            // Filter by status
            var selectedStatus = (cmbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedStatus != "All Statuses")
            {
                filtered = filtered.Where(r => r.Status == selectedStatus);
            }

            // Filter by priority
            var selectedPriority = (cmbPriorityFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedPriority != "All Priorities")
            {
                filtered = filtered.Where(r => r.Priority == selectedPriority);
            }

            var result = filtered.ToList();
            dgServiceRequests.ItemsSource = result;
            txtStatusBar.Text = $"Displaying {result.Count} of {allRequests.Count} requests";
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

            // Show dependencies
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
            graph.AppendLine("Service Request Dependency Graph");
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

            // Show Minimum Spanning Tree
            graph.AppendLine();
            graph.AppendLine("════════════════════════════════════════════════════════════");
            graph.AppendLine("Minimum Spanning Tree (MST) - Optimal Service Order:");
            graph.AppendLine("════════════════════════════════════════════════════════════");

            var mst = dependencyGraph.GetMinimumSpanningTree();
            if (mst.Count > 0)
            {
                foreach (var edge in mst)
                {
                    var from = allRequests.FirstOrDefault(r => r.IssueID == edge.From);
                    var to = allRequests.FirstOrDefault(r => r.IssueID == edge.To);
                    graph.AppendLine($"[{edge.From}] {from?.Title} → [{edge.To}] {to?.Title} (Priority: {edge.Weight})");
                }
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

            // Status breakdown
            stats.AppendLine("Status Breakdown:");
            var statusGroups = allRequests.GroupBy(r => r.Status);
            foreach (var group in statusGroups)
            {
                stats.AppendLine($"  • {group.Key}: {group.Count()} requests");
            }
            stats.AppendLine();

            // Priority breakdown
            stats.AppendLine("Priority Breakdown:");
            var priorityGroups = allRequests.GroupBy(r => r.Priority);
            foreach (var group in priorityGroups)
            {
                stats.AppendLine($"  • {group.Key}: {group.Count()} requests");
            }
            stats.AppendLine();

            // Category breakdown
            stats.AppendLine("Category Breakdown:");
            var categoryGroups = allRequests.GroupBy(r => r.Category);
            foreach (var group in categoryGroups)
            {
                stats.AppendLine($"  • {group.Key}: {group.Count()} requests");
            }
            stats.AppendLine();

            // High priority queue
            stats.AppendLine("═══════════════════════════════════════════════");
            stats.AppendLine("HIGH PRIORITY QUEUE (Using Heap):");
            stats.AppendLine("═══════════════════════════════════════════════");

            var highPriorityRequests = priorityHeap.GetTopPriority(5);
            foreach (var request in highPriorityRequests)
            {
                stats.AppendLine($"  {request.Priority,-10} | [{request.IssueID}] {request.Title}");
            }
            stats.AppendLine();

            // Average days open
            var avgDaysOpen = allRequests.Average(r => r.DaysOpen);
            stats.AppendLine($"Average Days Open: {avgDaysOpen:F1} days");

            // Overdue requests
            var overdueCount = allRequests.Count(r => DateTime.Now > r.SLADeadline && r.Status != "Resolved" && r.Status != "Closed");
            stats.AppendLine($"Overdue Requests: {overdueCount}");

            txtStatistics.Text = stats.ToString();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DisplayAllRequests();
            UpdateStatistics();
            GenerateDependencyGraphVisualization();
            txtStatusBar.Text = "Data refreshed successfully";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

    // Service Request class extending Issue
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

            // Calculate SLA deadline based on priority
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