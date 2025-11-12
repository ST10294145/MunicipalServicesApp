# 🏛️ Municipal Services Application

## 📋 Table of Contents
1. [Project Overview](#project-overview)
2. [System Requirements](#system-requirements)
3. [Installation & Setup](#installation--setup)
4. [How to Compile and Run](#how-to-compile-and-run)
5. [Application Features by Part](#application-features-by-part)
6. [User Guide](#user-guide)
7. [Admin Guide](#admin-guide)
8. [Part 3: Data Structures Implementation](#part-3-data-structures-implementation)
9. [Technical Architecture](#technical-architecture)
10. [Troubleshooting](#troubleshooting)
11. [Video Demonstration](#video-demonstration)
12. [Credits](#credits)

---

## 📘 Project Overview

The **Municipal Services Application** is a comprehensive Windows Presentation Foundation (WPF) desktop application developed in C# .NET for South African municipalities. The system enables residents to report issues, track service requests, and stay informed about community events.

### Development Timeline

**Part 1: Report Issues (Completed)**
- Citizens can report municipal issues (potholes, broken streetlights, water leaks, etc.)
- File/image attachment support for visual documentation
- Issue tracking with unique IDs
- Data stored in-memory using linked list data structure

**Part 2: Local Events and Announcements (Completed)**
- Community event calendar with search and filter capabilities
- Category-based event organization
- Date range filtering using SortedDictionary for efficient lookups
- Queue-based event prioritization
- Admin event management (add, edit, delete)

**Part 3: Service Request Status (Current)**
- **Advanced data structures implementation** for efficient request management
- Real-time request tracking with status updates
- Priority-based request handling using Max Heap
- Graph-based relationship discovery between requests
- Union-Find for intelligent request grouping
- Binary Search Tree for fast ID-based lookups
- Hierarchical category organization using N-ary Tree

### Key Objectives
- Streamline municipal service delivery
- Improve citizen engagement
- Demonstrate practical application of computer science data structures
- Provide efficient request analysis and management tools for administrators

---

## ⚙️ System Requirements

### Minimum Requirements
- **Operating System**: Windows 10 or higher
- **Framework**: .NET 6.0 or later
- **RAM**: 4 GB minimum (8 GB recommended)
- **Storage**: 200 MB free space
- **Display**: 1024x768 resolution minimum (1920x1080 recommended)
- **Input**: Keyboard and mouse

### Development Requirements
- **IDE**: Visual Studio 2022 (Community, Professional, or Enterprise)
- **SDK**: .NET 6.0 SDK or later
- **NuGet Packages**: 
  - `Newtonsoft.Json` (v13.0.0 or later) - for JSON serialization

### Optional Requirements
- **Git**: For version control and cloning repository
- **GitHub Desktop**: Alternative to command-line Git

---

## 🔧 Installation & Setup

### Clone from GitHub (Recommended)

1. **Install Git** (if not already installed):
   - Download from: https://git-scm.com/downloads
   - Run installer with default settings

2. **Clone the repository**:
```bash
git clone https://github.com/ST10294145/MunicipalServicesApp.git
cd MunicipalServicesApp
```

3. **Open in Visual Studio**:
   - Launch Visual Studio 2022
   - Click `File → Open → Project/Solution`
   - Navigate to the cloned folder
   - Select `MunicipalServicesApp.sln`

---

## 🚀 How to Compile and Run

### Step 1: Restore NuGet Packages

After opening the solution:

1. In **Solution Explorer**, right-click on the solution
2. Select **Restore NuGet Packages**
3. Wait for the Output window to show "Restore completed"
4. Verify `Newtonsoft.Json` package is installed:
   - Right-click project → **Manage NuGet Packages**
   - Check **Installed** tab for Newtonsoft.Json

### Step 2: Build the Solution

**Option A: Using Keyboard Shortcut**
```
Press: Ctrl + Shift + B
```

**Option B: Using Menu**
1. Click `Build → Build Solution`
2. Monitor **Output** window (View → Output)
3. Look for: **"Build succeeded"**
4. Ensure **0 errors** before proceeding

**Expected Output**:
```
Build started...
1>------ Build started: Project: MunicipalServicesApp, Configuration: Debug Any CPU ------
1>MunicipalServicesApp -> C:\...\bin\Debug\net6.0-windows\MunicipalServicesApp.dll
========== Build: 1 succeeded, 0 failed, 0 up-to-date, 0 skipped ==========
```

### Step 3: Run the Application

**Debug Mode (For Development)**:
```
Press: F5
```
- Enables breakpoints and debugging
- Console output visible
- Slower performance

**Release Mode (For Testing)**:
1. Change dropdown from **Debug** to **Release**
2. Press: `Ctrl + F5`
3. Optimized performance
4. No debugging overhead

### Step 4: First Launch Initialization

On first launch, the application will:

1. **Create data file**: `requests.json` in application directory
```
Location: C:\...\bin\Debug\net6.0-windows\requests.json
```

2. **Initialize data structures**:
   - Binary Search Tree (empty)
   - Max Heap (empty)
   - N-ary Category Tree (pre-populated with municipal categories)
   - Graph (empty)
   - Union-Find (capacity: 1000 requests)

3. **Display Main Window** with all navigation options ready

### Verification Steps

**Check Installation Success**:
1. Main window appears with navigation header
2. Six service cards visible (3 user, 3 admin)
3. No error messages in Output window
4. File `requests.json` created in bin folder

**Test Basic Functionality**:
1. Click "Submit Service Request"
2. Fill in form and submit
3. Click "View My Requests"
4. Verify request appears in list

---

## 🎯 Application Features by Part

### Part 1: Report Issues ✅

**Implemented Features**:
- **Issue Reporting Form**
  - Title and detailed description fields
  - Reporter name and contact email
  - Province, city, and street address selection
  - Category dropdown (Roads, Utilities, Public Safety, etc.)
  - File/image attachment support (PNG, JPG, PDF)
  
- **Data Structure**: Custom Linked List
  - In-memory storage during application runtime
  - O(n) time complexity for search operations
  - Simple sequential access

- **Admin Features**:
  - View all submitted issues in DataGrid
  - Add feedback to specific issues
  - Open attached files directly
  - Track submission timestamps

**Access**: Click **"📌 Report Issues"** button

### Part 2: Local Events & Announcements ✅

**Implemented Features**:
- **Event Calendar**
  - Display community events and announcements
  - Grid layout with event cards
  - Date, title, category, and description
  
- **Search & Filter System**
  - **Search by keyword**: Real-time text filtering
  - **Filter by category**: Dropdown selection (Community, Sports, Government, etc.)
  - **Filter by date range**: Start and end date pickers
  
- **Data Structures**:
  - **SortedDictionary<DateTime, Event>**: O(log n) date-based lookups
  - **Queue<Event>**: FIFO processing for event notifications
  - **Dictionary<string, List<Event>>**: Category-based indexing

- **Admin Event Management**:
  - Add new events with validation
  - Edit existing events
  - Delete events
  - Automatic sorting by date

**Access**: Click **"📅 Local Events & Announcements"** button

### Part 3: Service Request Status ⭐ (Current Focus)

This section implements **six advanced data structures** to demonstrate efficient algorithms and real-world computer science applications.

**Implemented Features**:

#### For All Users:
1. **Submit Service Requests**
   - Submit municipal service needs (repairs, permits, maintenance)
   - Auto-generated unique Request ID
   - Priority selection (Low, Medium, High, Critical)
   - Category selection from hierarchical tree
   - Location and contact information

2. **View Request Status**
   - Personal request dashboard
   - Real-time status tracking (Pending → In Progress → Completed)
   - Statistics summary (total, pending, in progress, completed)
   - Color-coded priority and status indicators
   - Days open and SLA deadline calculation
   - Detailed request information panel

#### For Administrators:
3. **Request Management Dashboard**
   - View ALL submitted requests in sortable DataGrid
   - Update request status (4 states available)
   - Update request priority (4 levels available)
   - Real-time data refresh
   - Bulk view and filtering

4. **Advanced Request Analysis** (Demonstrates All Data Structures)
   - **BST Search**: Find request by ID in O(log n) time
   - **Heap Operations**: Extract highest priority request
   - **Tree Traversal**: View hierarchical category structure
   - **Graph BFS**: Find related requests using relationship mapping
   - **Union-Find**: Get grouped requests for batch processing
   - **Sorted View**: Display requests ordered by ID

**Data Structures Implemented**:

| Structure | File Location | Purpose | Operations |
|-----------|---------------|---------|------------|
| **Binary Search Tree** | `DataStructures.cs` | Fast ID lookup | Insert, Search, In-order |
| **Max Heap** | `DataStructures.cs` | Priority queue | Insert, ExtractMax |
| **N-ary Tree** | `DataStructures.cs` | Category hierarchy | Traverse, Find, Height |
| **Graph** | `DataStructures.cs` | Relationships | AddEdge, BFS, DFS |
| **Union-Find** | `DataStructures.cs` | Grouping | Find, Union |
| **List** | Built-in | Basic storage | Add, Remove, Iterate |

**Access**:
- User: **"➕ Submit Service Request"** and **"📋 View My Requests"**
- Admin: **"🔧 Manage Service Requests"** and **"🔬 Advanced Request Analysis"**

---

## 📖 User Guide

### Getting Started

1. **Launch Application**
   - Double-click `MunicipalServicesApp.exe` or press F5 in Visual Studio
   - Main window opens with navigation dashboard

2. **Main Dashboard Overview**
```
┌─────────────────────────────────────────────────────┐
│  🏛️ Municipal Service Portal                        │
│  Your Community, Your Voice                         │
├─────────────────────────────────────────────────────┤
│                                                      │
│  👤 USER SERVICES                                    │
│  ┌──────────────────────────────────────────────┐   │
│  │ 📌 Report Issues                             │   │
│  │ Submit problems in your community            │   │
│  └──────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────┐   │
│  │ ➕ Submit Service Request                    │   │
│  │ Request municipal services                   │   │
│  └──────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────┐   │
│  │ 📋 View My Requests                          │   │
│  │ Track status of your submissions             │   │
│  └──────────────────────────────────────────────┘   │
│  ┌──────────────────────────────────────────────┐   │
│  │ 📅 Local Events & Announcements              │   │
│  │ Stay updated with community news             │   │
│  └──────────────────────────────────────────────┘   │
│                                                     │
│  ⚙️ ADMIN SERVICES (Login Required)                 │
│  ┌──────────────────────────────────────────────┐   │
│  │ 🔧 Manage Service Requests                   │   │
│  │ 📊 View All Reports                          │   │
│  │ 🔬 Advanced Request Analysis                 │   │
│  └──────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────┘
```

### Submitting a Service Request (Part 3 Feature)

**Step 1: Open Request Form**
1. Click **"➕ Submit Service Request"** button
2. Request form window opens

**Step 2: Fill Required Fields**

| Field | Description | Example | Required |
|-------|-------------|---------|----------|
| **Title** | Brief summary | "Pothole on Main Street" | ✅ Yes |
| **Category** | Issue type | Select from dropdown | ✅ Yes |
| **Priority** | Urgency level | Low/Medium/High/Critical | ✅ Yes |
| **Description** | Detailed explanation | "Large pothole causing damage..." | ✅ Yes |
| **Street Address** | Location | "123 Oak Avenue" | ❌ Optional |

**Step 3: Select Priority**

Priority levels determine Service Level Agreement (SLA) deadlines:

| Priority | SLA Deadline | Use When |
|----------|--------------|----------|
| 🔴 **Critical** | 1 day | Emergency (gas leak, major hazard) |
| 🟠 **High** | 3 days | Urgent (no water, major damage) |
| 🟡 **Medium** | 7 days | Standard issues (pothole, streetlight) |
| ⚪ **Low** | 14 days | Non-urgent (cosmetic, minor issues) |

**Step 4: Submit**
1. Review all fields
2. Click **"Submit"** button
3. Confirmation message displays with **Request ID**
4. Request automatically saved to `requests.json`
5. Request added to all data structures

### Viewing Your Requests (Part 3 Feature)

**Step 1: Open Request Viewer**
1. Click **"📋 View My Requests"** button
2. Request status window opens

**Step 2: View Statistics Dashboard**

Top section shows real-time statistics with color-coded cards

**Step 3: Browse Request List**

DataGrid columns:
- **ID**: Unique identifier
- **Title**: Request summary
- **Category**: Infrastructure, Utilities, etc.
- **Status**: Current state (color-coded)
- **Priority**: Urgency level (color-coded)
- **Date Reported**: Submission date
- **Days Open**: Time since submission

**Status Color Codes**:
- 🟡 Pending (Yellow) - Submitted, awaiting assignment
- 🔵 In Progress (Blue) - Work in progress
- 🟢 Completed (Green) - Request fulfilled
- 🔴 Cancelled (Red) - Request cancelled/invalid

**Priority Color Codes**:
- 🔴 Critical (Red) - Emergency
- 🟠 High (Orange) - Urgent
- 🟡 Medium (Yellow) - Standard
- ⚪ Low (Gray) - Non-urgent

**Step 4: View Request Details**
1. Click on any row in the list
2. Details panel appears showing full information

**Step 5: Refresh Data**
- Click **🔄 Refresh** button to reload from file
- Updates any status changes made by admin

### Reporting Issues (Part 1 Feature)

1. Click **"📌 Report Issues"**
2. Fill in issue details
3. Optionally attach photo/document
4. Click **Submit**
5. Issue saved to linked list

### Viewing Local Events (Part 2 Feature)

**Browse Events**:
1. Click **"📅 Local Events & Announcements"**
2. View event cards in grid layout

**Search Events**:
1. Type keyword in search box
2. Events filter in real-time

**Filter by Category**:
1. Click category dropdown
2. Select category
3. Only matching events display

**Filter by Date**:
1. Select start and end dates
2. Click "Apply Filter"
3. Events within range display

---

## 🔐 Admin Guide

### Admin Login

**Step 1: Access Login**
1. Click **"🔐 Admin Login"** button (top-right of main window)
2. Login dialog appears

**Step 2: Enter Credentials**
```
Username: admin
Password: password123
```
⚠️ **Important**: Credentials are case-sensitive

**Step 3: Verify Access**
- Success message: "Admin login successful!"
- Admin features now unlocked

### Managing Service Requests (Part 3)

**Access**: Click **"🔧 Manage Service Requests"** (requires admin login)

#### View All Requests

DataGrid displays all submitted requests with sortable columns

#### Update Request Status

**Procedure**:
1. Select request row
2. Click **"Update Status"** button
3. Select new status from dropdown
4. Click **"Update"**
5. Changes saved immediately

#### Update Request Priority

**Procedure**:
1. Select request row
2. Click **"Update Priority"** button
3. Select new priority level
4. Click **"Update"**
5. Priority updated, SLA recalculated

#### Refresh Data

Click **"Refresh"** to reload from `requests.json`

### Advanced Request Analysis (Part 3 Showcase)

**Access**: Click **"🔬 Advanced Request Analysis"** (requires admin login)

This window demonstrates all six data structures with real-world use cases.

#### 1. Search by ID (Binary Search Tree)

**Purpose**: Fast lookup of specific request

**Procedure**:
1. Enter Request ID
2. Click **"Search"**
3. BST searches in O(log n) time
4. Result displays complete request details

**Algorithm Complexity**: O(log n) - Logarithmic search time

#### 2. Get Highest Priority Request (Max Heap)

**Purpose**: Always extract most urgent request first

**Procedure**:
1. Click **"Get Highest Priority Request"**
2. Heap extracts root (maximum priority) in O(log n)
3. Displays request details with priority score
4. Request removed from heap

**Algorithm Complexity**: O(log n) - Heap extract operation

**Real-World Use Case**: Emergency dispatcher gets next critical call

#### 3. View Category Tree (N-ary Tree)

**Purpose**: Display hierarchical category organization

**Procedure**:
1. Click **"View All Categories"**
2. Tree traversal displays structure
3. Shows tree height and total nodes

**Algorithm Complexity**: O(n) - Tree traversal

**Tree Structure**:
```
Municipal Services (root)
├── Infrastructure
│   ├── Roads
│   ├── Bridges
│   └── Sidewalks
├── Utilities
│   ├── Water
│   ├── Electricity
│   └── Streetlights
└── Public Safety
    ├── Traffic Signals
    └── Street Signs
```

#### 4. Find Related Requests (Graph BFS)

**Purpose**: Discover connected requests based on relationships

**Procedure**:
1. Enter Request ID
2. Click **"Find Related Requests"**
3. BFS traversal finds all connected requests
4. Results display in DataGrid

**Relationship Criteria**:
- Same category (+5 weight)
- Same priority (+3 weight)
- Same location (+10 weight)
- Same reporter (+2 weight)

**Algorithm Complexity**: O(V + E) - Graph traversal

**Real-World Use Cases**:
- Route optimization: "All potholes on Main Street"
- Resource allocation: "Multiple water issues in Area B"
- Pattern detection: "Several streetlight failures"

#### 5. Get Grouped Requests (Union-Find)

**Purpose**: Find requests that should be handled together

**Procedure**:
1. Enter Request ID
2. Click **"Get Grouped Requests"**
3. Union-Find finds all requests in same disjoint set
4. Results display in DataGrid

**Output Example**:
```
✅ Found 5 requests in the same group (Union-Find Disjoint Set):

These requests are grouped because they share:
- Similar categories
- Same geographic area
- Related issues

Group Representative: Request #15

⏱️ Time Complexity: O(α(n)) - Nearly constant with path compression!
   α(n) is the inverse Ackermann function (practically < 5)
```

**Algorithm**:
```csharp
public int Find(int x)
{
    if (parent[x] != x)
        parent[x] = Find(parent[x]);  // Path compression
    return parent[x];
}

public void Union(int x, int y)
{
    int rootX = Find(x);
    int rootY = Find(y);
    
    if (rootX != rootY)
    {
        if (rank[rootX] < rank[rootY])
            parent[rootX] = rootY;
        else if (rank[rootX] > rank[rootY])
            parent[rootY] = rootX;
        else
        {
            parent[rootY] = rootX;
            rank[rootX]++;
        }
    }
}
```

**Real-World Use Cases**:
- **Batch Processing**: Handle all grouped requests together
- **Resource Planning**: "Assign single team to handle group"
- **Cost Optimization**: Reduce duplicate visits to same area
- **Efficiency**: Process related requests in one operation

#### 6. View Sorted Requests (Binary Search Tree)

**Purpose**: Display all requests in sorted order by ID

**Procedure**:
1. Click **"View All Requests (Sorted by ID)"**
2. BST in-order traversal displays sorted list
3. Results show in DataGrid

**Algorithm**:
```csharp
public List<ServiceRequest> InOrderTraversal()
{
    List<ServiceRequest> result = new List<ServiceRequest>();
    InOrderTraversalRecursive(root, result);
    return result;
}

private void InOrderTraversalRecursive(BSTNode node, List<ServiceRequest> result)
{
    if (node != null)
    {
        InOrderTraversalRecursive(node.Left, result);   // Left subtree
        result.Add(node.Request);                       // Current node
        InOrderTraversalRecursive(node.Right, result);  // Right subtree
    }
}
```

**Algorithm Complexity**: O(n) - Visit every node once

**Real-World Use Case**: Generate ordered reports, verify data integrity

---

## 🔬 Part 3: Data Structures Implementation

This section provides deep technical details for each data structure.

### 1. Binary Search Tree (BST)

**File**: `DataStructures.cs` → Class `BinarySearchTree`

**Purpose**: Fast request lookup by ID

**Properties**:
- Self-balancing not implemented (assumes random insertion order)
- Average case: O(log n) search, insert, delete
- Worst case: O(n) if tree becomes skewed

**Implementation Details**:

```csharp
public class BSTNode
{
    public int Key { get; set; }              // Request ID
    public ServiceRequest Request { get; set; }
    public BSTNode Left { get; set; }
    public BSTNode Right { get; set; }
}

public class BinarySearchTree
{
    private BSTNode root;
    
    public void Insert(ServiceRequest request)
    {
        root = InsertRecursive(root, request);
    }
    
    private BSTNode InsertRecursive(BSTNode node, ServiceRequest request)
    {
        if (node == null)
            return new BSTNode 
            { 
                Key = request.Id, 
                Request = request 
            };
        
        if (request.Id < node.Key)
            node.Left = InsertRecursive(node.Left, request);
        else if (request.Id > node.Key)
            node.Right = InsertRecursive(node.Right, request);
        
        return node;
    }
    
    public BSTNode Search(int key)
    {
        return SearchRecursive(root, key);
    }
    
    private BSTNode SearchRecursive(BSTNode node, int key)
    {
        if (node == null || node.Key == key)
            return node;
        
        return key < node.Key 
            ? SearchRecursive(node.Left, key)
            : SearchRecursive(node.Right, key);
    }
}
```

**Time Complexities**:
- Insert: O(log n) average, O(n) worst
- Search: O(log n) average, O(n) worst
- Delete: O(log n) average, O(n) worst
- In-order Traversal: O(n)

**Space Complexity**: O(n)

**Real-World Performance**:
```
1,000 requests: ~10 comparisons
10,000 requests: ~14 comparisons
100,000 requests: ~17 comparisons
```

### 2. Max Heap (Priority Queue)

**File**: `DataStructures.cs` → Class `MaxHeap`

**Purpose**: Always retrieve highest priority request first

**Properties**:
- Complete binary tree stored in array
- Parent always greater than children
- Heap property: `heap[parent] >= heap[child]`

**Implementation Details**:

```csharp
public class MaxHeap
{
    private List<ServiceRequest> heap;
    
    public void Insert(ServiceRequest request)
    {
        heap.Add(request);
        HeapifyUp(heap.Count - 1);
    }
    
    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            
            if (GetPriorityValue(heap[index]) <= 
                GetPriorityValue(heap[parentIndex]))
                break;
            
            Swap(index, parentIndex);
            index = parentIndex;
        }
    }
    
    public ServiceRequest ExtractMax()
    {
        if (heap.Count == 0) return null;
        
        ServiceRequest max = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        
        if (heap.Count > 0)
            HeapifyDown(0);
        
        return max;
    }
    
    private void HeapifyDown(int index)
    {
        int maxIndex = index;
        int left = 2 * index + 1;
        int right = 2 * index + 2;
        
        if (left < heap.Count && 
            GetPriorityValue(heap[left]) > GetPriorityValue(heap[maxIndex]))
            maxIndex = left;
        
        if (right < heap.Count && 
            GetPriorityValue(heap[right]) > GetPriorityValue(heap[maxIndex]))
            maxIndex = right;
        
        if (maxIndex != index)
        {
            Swap(index, maxIndex);
            HeapifyDown(maxIndex);
        }
    }
    
    private int GetPriorityValue(ServiceRequest request)
    {
        return request.Priority switch
        {
            "Critical" => 4,
            "High" => 3,
            "Medium" => 2,
            "Low" => 1,
            _ => 0
        };
    }
}
```

**Time Complexities**:
- Insert: O(log n)
- ExtractMax: O(log n)
- Peek: O(1)
- BuildHeap: O(n)

**Space Complexity**: O(n)

**Heap Visualization**:
```
Array: [Critical, High, High, Medium, Medium, Low, Low]
Index:    0       1     2      3       4      5    6

Tree Structure:
           Critical (0)
          /            \
       High (1)       High (2)
       /     \         /     \
  Medium(3) Medium(4) Low(5) Low(6)
```

### 3. N-ary Tree (Category Hierarchy)

**File**: `DataStructures.cs` → Class `BasicNaryTree<T>`

**Purpose**: Organize categories hierarchically

**Properties**:
- Each node can have multiple children
- Pre-order traversal for display
- Height calculation for depth analysis

**Implementation Details**:

```csharp
public class BasicTreeNode<T>
{
    public T Data { get; set; }
    public List<BasicTreeNode<T>> Children { get; set; }
    
    public BasicTreeNode(T data)
    {
        Data = data;
        Children = new List<BasicTreeNode<T>>();
    }
}

public class BasicNaryTree<T>
{
    public BasicTreeNode<T> Root { get; private set; }
    
    public void AddChild(BasicTreeNode<T> parent, T childData)
    {
        var child = new BasicTreeNode<T>(childData);
        parent.Children.Add(child);
    }
    
    public List<T> Traverse()
    {
        List<T> result = new List<T>();
        TraverseRecursive(Root, result);
        return result;
    }
    
    private void TraverseRecursive(BasicTreeNode<T> node, List<T> result)
    {
        if (node != null)
        {
            result.Add(node.Data);
            foreach (var child in node.Children)
                TraverseRecursive(child, result);
        }
    }
    
    public int GetHeight(BasicTreeNode<T> node)
    {
        if (node == null) return 0;
        
        int maxHeight = 0;
        foreach (var child in node.Children)
        {
            int childHeight = GetHeight(child);
            maxHeight = Math.Max(maxHeight, childHeight);
        }
        
        return maxHeight + 1;
    }
}
```

**Time Complexities**:
- Insert: O(1)
- Traverse: O(n)
- Height: O(n)
- Search: O(n)

**Space Complexity**: O(n)

**Category Tree Structure**:
```
Municipal Services (root)
├── Infrastructure
│   ├── Roads
│   ├── Bridges
│   └── Sidewalks
├── Utilities
│   ├── Water
│   ├── Electricity
│   └── Streetlights
├── Public Safety
│   ├── Traffic Signals
│   ├── Street Signs
│   └── Hazards
└── Environment
    ├── Parks
    ├── Waste Management
    └── Pollution
```

### 4. Graph (Request Relationships)

**File**: `DataStructures.cs` → Class `RequestGraph`

**Purpose**: Map relationships between requests

**Properties**:
- Directed weighted graph
- Adjacency list representation
- Supports BFS and DFS traversal

**Implementation Details**:

```csharp
public class GraphEdge
{
    public int To { get; set; }
    public int Weight { get; set; }
}

public class RequestGraph
{
    private Dictionary<int, List<GraphEdge>> adjacencyList;
    
    public void AddEdge(int from, int to, int weight)
    {
        if (!adjacencyList.ContainsKey(from))
            adjacencyList[from] = new List<GraphEdge>();
        
        adjacencyList[from].Add(new GraphEdge { To = to, Weight = weight });
    }
    
    public List<int> BreadthFirstSearch(int start)
    {
        var visited = new List<int>();
        var queue = new Queue<int>();
        queue.Enqueue(start);
        visited.Add(start);
        
        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            
            if (adjacencyList.ContainsKey(current))
            {
                foreach (var edge in adjacencyList[current])
                {
                    if (!visited.Contains(edge.To))
                    {
                        visited.Add(edge.To);
                        queue.Enqueue(edge.To);
                    }
                }
            }
        }
        return visited;
    }
    
    public List<int> DepthFirstSearch(int start)
    {
        var visited = new List<int>();
        DFSRecursive(start, visited);
        return visited;
    }
    
    private void DFSRecursive(int node, List<int> visited)
    {
        visited.Add(node);
        
        if (adjacencyList.ContainsKey(node))
        {
            foreach (var edge in adjacencyList[node])
            {
                if (!visited.Contains(edge.To))
                    DFSRecursive(edge.To, visited);
            }
        }
    }
}
```

**Edge Weight Calculation**:
```csharp
private int CalculateRelationshipWeight(ServiceRequest req1, ServiceRequest req2)
{
    int weight = 0;
    
    if (req1.Category == req2.Category)
        weight += 5;      // Same issue type
    
    if (req1.Priority == req2.Priority)
        weight += 3;      // Same urgency
    
    if (req1.StreetAddress == req2.StreetAddress)
        weight += 10;     // Same location (strongest relationship)
    
    if (req1.Email == req2.Email)
        weight += 2;      // Same reporter
    
    return weight;
}
```

**Time Complexities**:
- AddNode: O(1)
- AddEdge: O(1)
- BFS: O(V + E) where V = vertices, E = edges
- DFS: O(V + E)

**Space Complexity**: O(V + E)

**Graph Visualization Example**:
```
Request 1 (Main St Pothole)
    │
    ├──[weight=10]──> Request 5 (Main St Pothole #2)
    │                      │
    │                      └──[weight=5]──> Request 12 (Oak Ave Pothole)
    │
    └──[weight=8]───> Request 8 (Main St Sidewalk)
                           │
                           └──[weight=5]──> Request 15 (Main St Curb)
```

### 5. Union-Find (Disjoint Set Union)

**File**: `DataStructures.cs` → Class `UnionFind`

**Purpose**: Group related requests efficiently

**Properties**:
- Path compression for optimization
- Union by rank for balance
- Near-constant time operations

**Implementation Details**:

```csharp
public class UnionFind
{
    private int[] parent;
    private int[] rank;
    
    public UnionFind(int size)
    {
        parent = new int[size];
        rank = new int[size];
        
        for (int i = 0; i < size; i++)
        {
            parent[i] = i;  // Each element is its own parent initially
            rank[i] = 0;
        }
    }
    
    public int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]);  // Path compression
        return parent[x];
    }
    
    public void Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);
        
        if (rootX != rootY)
        {
            // Union by rank
            if (rank[rootX] < rank[rootY])
                parent[rootX] = rootY;
            else if (rank[rootX] > rank[rootY])
                parent[rootY] = rootX;
            else
            {
                parent[rootY] = rootX;
                rank[rootX]++;
            }
        }
    }
    
    public bool Connected(int x, int y)
    {
        return Find(x) == Find(y);
    }
}
```

**Time Complexities**:
- Find: O(α(n)) - inverse Ackermann function (practically constant)
- Union: O(α(n))
- Connected: O(α(n))

**Space Complexity**: O(n)

**Path Compression Visualization**:
```
Before Path Compression:
    5 → 4 → 3 → 2 → 1 (root)

After Find(5):
    5 → 1 (root)
    4 → 1 (root)
    3 → 1 (root)
    2 → 1 (root)
```

**Union by Rank Example**:
```
Initial: Each node is its own set
[0] [1] [2] [3] [4]

Union(0, 1):
[0,1] [2] [3] [4]
  0
  └─1

Union(2, 3):
[0,1] [2,3] [4]
  0     2
  └─1   └─3

Union(0, 2):
[0,1,2,3] [4]
    0
   / \
  1   2
      └─3
```

**Grouping Logic**:
```csharp
public void GroupRequests(List<ServiceRequest> requests)
{
    for (int i = 0; i < requests.Count; i++)
    {
        for (int j = i + 1; j < requests.Count; j++)
        {
            int weight = CalculateRelationshipWeight(
                requests[i], 
                requests[j]
            );
            
            if (weight >= 8)  // Threshold for grouping
                UnionFind.Union(requests[i].Id, requests[j].Id);
        }
    }
}
```

### 6. List (Basic Storage)

**Built-in .NET Collection**: `List<ServiceRequest>`

**Purpose**: Primary storage for all requests

**Properties**:
- Dynamic array implementation
- Fast indexed access
- Automatic resizing

**Operations Used**:
```csharp
// Add request
Requests.Add(request);              // O(1) amortized

// Find by ID
var request = Requests.FirstOrDefault(r => r.Id == id);  // O(n)

// Update request
var existing = Requests.FirstOrDefault(r => r.Id == id);
if (existing != null)
{
    existing.Status = newStatus;    // O(n) to find, O(1) to update
}

// Remove request
Requests.RemoveAll(r => r.Id == id);  // O(n)

// Count
int total = Requests.Count;         // O(1)

// Filter
var pending = Requests.Where(r => r.Status == "Pending").ToList();  // O(n)
```

**Time Complexities**:
- Add: O(1) amortized
- Access by index: O(1)
- Search: O(n)
- Remove: O(n)

**Space Complexity**: O(n)

---

## 🏗️ Technical Architecture

### Project Structure

```
MunicipalServicesApp/
│
├── MunicipalServicesApp.csproj    # Project configuration
├── App.xaml                        # Application entry point
├── App.xaml.cs                     # Application logic
│
├── Models/                         # Data models
│   ├── ServiceRequest.cs          # Part 3: Request model
│   ├── Issue.cs                   # Part 1: Issue model
│   └── Event.cs                   # Part 2: Event model
│
├── DataStructures/                 # Part 3: Advanced data structures
│   └── DataStructures.cs          # All 6 data structures
│
├── Services/                       # Business logic
│   ├── RequestManager.cs          # Part 3: Request operations
│   ├── IssueManager.cs            # Part 1: Issue operations
│   └── EventManager.cs            # Part 2: Event operations
│
├── Views/                          # UI Windows
│   ├── MainWindow.xaml            # Navigation dashboard
│   ├── ReportIssuesWindow.xaml    # Part 1: Issue reporting
│   ├── ViewIssuesWindow.xaml      # Part 1: Admin issue viewer
│   ├── LocalEventsWindow.xaml     # Part 2: Event calendar
│   ├── ManageEventsWindow.xaml    # Part 2: Admin event manager
│   ├── ServiceRequestWindow.xaml  # Part 3: Submit request
│   ├── ViewRequestsWindow.xaml    # Part 3: User request status
│   ├── ManageRequestsWindow.xaml  # Part 3: Admin management
│   └── AnalysisWindow.xaml        # Part 3: Data structure demo
│
├── Data/                           # Persistent storage
│   └── requests.json              # JSON file (auto-created)
│
└── Resources/                      # Assets
    ├── Images/
    └── Styles/
```

### Class Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                       MainWindow                             │
│  - Navigation hub                                            │
│  - Button handlers for all windows                          │
└─────────────────────────────────────────────────────────────┘
                           │
        ┌──────────────────┼──────────────────┐
        │                  │                  │
        ▼                  ▼                  ▼
┌───────────────┐  ┌────────────────┐  ┌────────────────┐
│  Part 1       │  │  Part 2        │  │  Part 3        │
│  Windows      │  │  Windows       │  │  Windows       │
├───────────────┤  ├────────────────┤  ├────────────────┤
│ - Report      │  │ - Local Events │  │ - Submit       │
│   Issues      │  │ - Manage       │  │   Request      │
│ - View        │  │   Events       │  │ - View         │
│   Issues      │  │                │  │   Requests     │
│               │  │                │  │ - Manage       │
│               │  │                │  │   Requests     │
│               │  │                │  │ - Analysis     │
└───────┬───────┘  └────────┬───────┘  └────────┬───────┘
        │                   │                   │
        ▼                   ▼                   ▼
┌───────────────┐  ┌────────────────┐  ┌────────────────┐
│ IssueManager  │  │ EventManager   │  │ RequestManager │
├───────────────┤  ├────────────────┤  ├────────────────┤
│ - LinkedList  │  │ - SortedDict   │  │ - List         │
│               │  │ - Queue        │  │ - BST          │
│               │  │ - Dictionary   │  │ - Max Heap     │
│               │  │                │  │ - N-ary Tree   │
│               │  │                │  │ - Graph        │
│               │  │                │  │ - Union-Find   │
└───────┬───────┘  └────────┬───────┘  └────────┬───────┘
        │                   │                   │
        ▼                   ▼                   ▼
┌───────────────┐  ┌────────────────┐  ┌────────────────┐
│  Issue.cs     │  │  Event.cs      │  │ ServiceRequest │
│  (Model)      │  │  (Model)       │  │  .cs (Model)   │
└───────────────┘  └────────────────┘  └────────────────┘
```

### Data Flow Diagram (Part 3 Focus)

```
┌─────────────────────────────────────────────────────────────┐
│                         USER                                │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│              ServiceRequestWindow.xaml                      │
│  - Form inputs (Title, Category, Priority, etc.)            │
│  - Validation logic                                         │
└───────────────────────┬─────────────────────────────────────┘
                        │ Submit
                        ▼
┌─────────────────────────────────────────────────────────────┐
│              RequestManager.AddRequest()                    │
│  1. Create ServiceRequest object                            │
│  2. Assign unique ID                                        │
│  3. Insert into all data structures:                        │
│     - List.Add()            [O(1)]                          │
│     - BST.Insert()          [O(log n)]                      │
│     - Heap.Insert()         [O(log n)]                      │
│     - Graph.AddNode()       [O(1)]                          │
│     - Graph.AddEdges()      [O(E)]                          │
│     - UnionFind.Union()     [O(α(n))]                       │
│  4. SaveToFile()                                            │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│              requests.json (Persistent Storage)             │
│  [                                                          │
│    {                                                        │
│      "Id": 1,                                               │
│      "Title": "Pothole on Main Street",                     │
│      "Category": "Roads",                                   │
│      "Status": "Pending",                                   │
│      "Priority": "High",                                    │
│      ...                                                    │
│    }                                                        │
│  ]                                                          │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│              RequestManager.LoadFromFile()                  │
│  - Called on application startup                            │
│  - Deserialize JSON                                         │
│  - Rebuild all data structures                              │
└───────────────────────┬─────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────┐
│              ViewRequestsWindow.xaml                        │
│  - Display requests in DataGrid                             │
│  - Statistics dashboard                                     │
│  - Details panel                                            │
└─────────────────────────────────────────────────────────────┘
```

### File I/O and Serialization

**Saving Requests** (`RequestManager.cs`):
```csharp
public void SaveToFile(string filename = "requests.json")
{
    try
    {
        string json = JsonConvert.SerializeObject(
            Requests, 
            Formatting.Indented
        );
        File.WriteAllText(filename, json);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error saving: {ex.Message}");
    }
}
```

**Loading Requests** (`RequestManager.cs`):
```csharp
public void LoadFromFile(string filename = "requests.json")
{
    try
    {
        if (File.Exists(filename))
        {
            string json = File.ReadAllText(filename);
            var loadedRequests = JsonConvert.DeserializeObject<List<ServiceRequest>>(json);
            
            if (loadedRequests != null)
            {
                Requests.Clear();
                RequestBST = new BinarySearchTree();
                RequestHeap = new MaxHeap();
                RequestGraph = new RequestGraph();
                UnionFind = new UnionFind(1000);
                
                foreach (var request in loadedRequests)
                {
                    Requests.Add(request);
                    RequestBST.Insert(request);
                    RequestHeap.Insert(request);
                    RequestGraph.AddNode(request.Id);
                }
                
                BuildGraphEdges();
                BuildUnionFindGroups();
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error loading: {ex.Message}");
    }
}
```

**JSON Structure Example**:
```json
[
  {
    "Id": 1,
    "Title": "Pothole on Main Street",
    "Category": "Roads",
    "Status": "Pending",
    "Priority": "High",
    "Description": "Large pothole causing vehicle damage",
    "Name": "John Doe",
    "Email": "john@example.com",
    "StreetAddress": "123 Main Street",
    "DateReported": "2024-11-12T10:30:00"
  },
  {
    "Id": 2,
    "Title": "Water leak on Oak Avenue",
    "Category": "Utilities",
    "Status": "In Progress",
    "Priority": "Critical",
    "Description": "Major water leak flooding street",
    "Name": "Jane Smith",
    "Email": "jane@example.com",
    "StreetAddress": "456 Oak Avenue",
    "DateReported": "2024-11-13T08:15:00"
  }
]
```

### Performance Characteristics

**Comparative Analysis**:

| Operation | List | BST | Heap | Tree | Graph | Union-Find |
|-----------|------|-----|------|------|-------|------------|
| Insert | O(1) | O(log n) | O(log n) | O(1) | O(1) | O(α(n)) |
| Search | O(n) | O(log n) | N/A | O(n) | O(V+E) | O(α(n)) |
| Delete | O(n) | O(log n) | O(log n) | O(1) | O(1) | N/A |
| Traverse | O(n) | O(n) | O(n) | O(n) | O(V+E) | N/A |
| Extract Max | O(n) | O(n) | O(log n) | N/A | N/A | N/A |

**Memory Usage** (1000 requests):
```
List:          ~80 KB (base data)
BST:           ~120 KB (+ node overhead)
Max Heap:      ~80 KB (array-based)
N-ary Tree:    ~20 KB (category structure)
Graph:         ~200 KB (adjacency list + edges)
Union-Find:    ~8 KB (two integer arrays)
────────────────────────────────────────
Total:         ~508 KB for 1000 requests
```

---



## 🎥 Video Demonstration

