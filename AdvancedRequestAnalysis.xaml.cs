using System;
using System.Linq;
using System.Windows;

namespace MunicipalServicesApp
{
    public partial class AdvancedRequestAnalysis : Window
    {
        public AdvancedRequestAnalysis()
        {
            InitializeComponent();
        }

        private void btnSearchByID_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtSearchID.Text, out int issueId))
            {
                MessageBox.Show("Please enter a valid numeric ID", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var request = RequestManager.Instance.SearchByID(issueId);

            dgResults.Visibility = Visibility.Collapsed;
            txtResultTitle.Text = "🔍 BST Search Result:";

            if (request != null)
            {
                txtResults.Text = $@"
✅ Request Found (using Binary Search Tree - O(log n)):

📋 ID: {request.IssueID}
📌 Title: {request.Title}
📂 Category: {request.Category}
🔴 Status: {request.Status}
⚡ Priority: {request.Priority}
📅 Date Reported: {request.DateReported:MMMM dd, yyyy}
📧 Reporter: {request.Reporter}
📍 Location: {request.StreetAddress}
📝 Description: {request.Description}

⏱️ Time Complexity: O(log n) - Efficient logarithmic search!";
            }
            else
            {
                txtResults.Text = $"❌ No request found with ID: {issueId}";
            }
        }

        private void btnGetHighestPriority_Click(object sender, RoutedEventArgs e)
        {
            var request = RequestManager.Instance.GetHighestPriorityRequest();

            dgResults.Visibility = Visibility.Collapsed;
            txtResultTitle.Text = "⚡ Heap - Highest Priority Request:";

            if (request != null)
            {
                txtResults.Text = $@"
🔥 HIGHEST PRIORITY REQUEST (extracted from Max Heap):

📋 ID: {request.IssueID}
📌 Title: {request.Title}
📂 Category: {request.Category}
🔴 Status: {request.Status}
⚡ Priority: {request.Priority} (Value: {request.GetPriorityValue()})
📅 Date Reported: {request.DateReported:MMMM dd, yyyy}
⏰ Days Open: {request.DaysOpen} days
🚨 SLA Deadline: {request.SLADeadline:MMMM dd, yyyy}

⏱️ Time Complexity: O(log n) - Heap extract operation!

Note: This request has been removed from the heap.
Refresh to rebuild the heap with all requests.";
            }
            else
            {
                txtResults.Text = "❌ Heap is empty - no requests available.";
            }
        }

        private void btnViewCategories_Click(object sender, RoutedEventArgs e)
        {
            var categories = RequestManager.Instance.GetAllCategories();
            var height = RequestManager.Instance.GetCategoryTreeHeight();

            dgResults.Visibility = Visibility.Collapsed;
            txtResultTitle.Text = "🌳 Category Tree Structure:";

            txtResults.Text = $@"
📂 HIERARCHICAL CATEGORY TREE:

Tree Height: {height} levels
Total Nodes: {categories.Count}

Categories (Pre-order Traversal):
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
{string.Join("\n", categories.Select((c, i) => $"{i + 1}. {c}"))}
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

💡 The N-ary tree organizes categories hierarchically,
   allowing for easy navigation and classification!

⏱️ Time Complexity: O(n) - Tree traversal";
        }

        private void btnFindRelated_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtRelatedID.Text, out int issueId))
            {
                MessageBox.Show("Please enter a valid numeric ID", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var relatedRequests = RequestManager.Instance.FindRelatedRequests(issueId);

            txtResults.Text = "";
            txtResultTitle.Text = $"🔗 Graph - Related Requests to ID {issueId}:";

            if (relatedRequests.Count > 0)
            {
                dgResults.ItemsSource = relatedRequests;
                dgResults.Visibility = Visibility.Visible;

                txtResults.Text = $@"
✅ Found {relatedRequests.Count} related requests using BFS (Breadth-First Search):

Relationships are based on:
- Same category (+5 weight)
- Same priority (+3 weight)
- Same location (+10 weight)
- Same reporter (+2 weight)

⏱️ Time Complexity: O(V + E) - Graph traversal
   V = vertices (requests), E = edges (relationships)";
            }
            else
            {
                dgResults.Visibility = Visibility.Collapsed;
                txtResults.Text = $"❌ No related requests found for ID: {issueId}";
            }
        }

        private void btnGetGrouped_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtGroupID.Text, out int issueId))
            {
                MessageBox.Show("Please enter a valid numeric ID", "Invalid Input",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var groupedRequests = RequestManager.Instance.GetGroupedRequests(issueId);

            txtResults.Text = "";
            txtResultTitle.Text = $"👥 Union-Find - Grouped Requests for ID {issueId}:";

            if (groupedRequests.Count > 0)
            {
                dgResults.ItemsSource = groupedRequests;
                dgResults.Visibility = Visibility.Visible;

                txtResults.Text = $@"
✅ Found {groupedRequests.Count} requests in the same group:

Union-Find groups requests by:
- Same category AND priority
- Same location

These requests can be handled together for efficiency!

⏱️ Time Complexity: O(α(n)) ≈ O(1) - Nearly constant time!
   (α = inverse Ackermann function, grows VERY slowly)";
            }
            else
            {
                dgResults.Visibility = Visibility.Collapsed;
                txtResults.Text = $"❌ No grouped requests found for ID: {issueId}";
            }
        }

        private void btnViewStats_Click(object sender, RoutedEventArgs e)
        {
            var stats = RequestManager.Instance.GetDataStructureStats();

            dgResults.Visibility = Visibility.Collapsed;
            txtResultTitle.Text = "📊 Data Structure Statistics:";
            txtResults.Text = stats + @"

🎯 ALL DATA STRUCTURES ARE ACTIVELY BEING USED:

1️⃣ LIST - Basic storage and iteration
2️⃣ BST (Binary Search Tree) - Fast O(log n) search by ID
3️⃣ HEAP (Max Heap) - Priority-based retrieval
4️⃣ TREE (N-ary) - Hierarchical category organization
5️⃣ GRAPH - Relationship mapping between requests
6️⃣ UNION-FIND - Efficient grouping of related requests

Each structure serves a specific purpose for optimal performance! 🚀";
        }

        private void btnViewSorted_Click(object sender, RoutedEventArgs e)
        {
            var sortedRequests = RequestManager.Instance.GetRequestsSortedByID();

            txtResults.Text = "";
            txtResultTitle.Text = "📑 BST In-Order Traversal (Sorted by ID):";

            if (sortedRequests.Count > 0)
            {
                dgResults.ItemsSource = sortedRequests;
                dgResults.Visibility = Visibility.Visible;

                txtResults.Text = $@"
✅ Showing {sortedRequests.Count} requests sorted by ID:

Using BST in-order traversal produces a sorted list!

⏱️ Time Complexity: O(n) - Must visit all nodes
   But the tree maintains sorted order automatically!";
            }
            else
            {
                dgResults.Visibility = Visibility.Collapsed;
                txtResults.Text = "❌ No requests to display.";
            }
        }
    }
}