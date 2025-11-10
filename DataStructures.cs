using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp
{
    // BASIC TREE (N-ary Tree) - Hierarchical Category Organization
    // Purpose: Organize service categories in parent-child relationships
    // Time Complexity: O(n) traversal
    public class BasicTreeNode<T>
    {
        public T Data { get; set; }
        public List<BasicTreeNode<T>> Children { get; set; }

        public BasicTreeNode(T data)
        {
            Data = data;
            Children = new List<BasicTreeNode<T>>();
        }

        public void AddChild(BasicTreeNode<T> child)
        {
            Children.Add(child);
        }
    }

    public class BasicTree<T>
    {
        public BasicTreeNode<T> Root { get; set; }

        public BasicTree(T rootData)
        {
            Root = new BasicTreeNode<T>(rootData);
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
                {
                    TraverseRecursive(child, result);
                }
            }
        }

        public BasicTreeNode<T> Find(T data)
        {
            return FindRecursive(Root, data);
        }

        private BasicTreeNode<T> FindRecursive(BasicTreeNode<T> node, T data)
        {
            if (node == null) return null;
            if (EqualityComparer<T>.Default.Equals(node.Data, data)) return node;

            foreach (var child in node.Children)
            {
                var result = FindRecursive(child, data);
                if (result != null) return result;
            }
            return null;
        }

        public int GetHeight()
        {
            return GetHeightRecursive(Root);
        }

        private int GetHeightRecursive(BasicTreeNode<T> node)
        {
            if (node == null || node.Children.Count == 0) return 1;
            return 1 + node.Children.Max(c => GetHeightRecursive(c));
        }
    }


    // BINARY TREE - Simple Two-Child Structure
    // Purpose: Binary structure for classification logic
    // Time Complexity: O(n) search, O(1) insert
    public class BinaryTreeNode<T>
    {
        public T Data { get; set; }
        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; }

        public BinaryTreeNode(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }

    public class BinaryTree<T>
    {
        public BinaryTreeNode<T> Root { get; set; }

        public BinaryTree(T rootData)
        {
            Root = new BinaryTreeNode<T>(rootData);
        }

        public void Insert(T data)
        {
            if (Root == null)
            {
                Root = new BinaryTreeNode<T>(data);
                return;
            }

            Queue<BinaryTreeNode<T>> queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(Root);

            while (queue.Count > 0)
            {
                BinaryTreeNode<T> current = queue.Dequeue();

                if (current.Left == null)
                {
                    current.Left = new BinaryTreeNode<T>(data);
                    return;
                }
                else
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right == null)
                {
                    current.Right = new BinaryTreeNode<T>(data);
                    return;
                }
                else
                {
                    queue.Enqueue(current.Right);
                }
            }
        }

        public List<T> InOrder()
        {
            List<T> result = new List<T>();
            InOrderRecursive(Root, result);
            return result;
        }

        private void InOrderRecursive(BinaryTreeNode<T> node, List<T> result)
        {
            if (node != null)
            {
                InOrderRecursive(node.Left, result);
                result.Add(node.Data);
                InOrderRecursive(node.Right, result);
            }
        }

        public int GetHeight()
        {
            return GetHeightRecursive(Root);
        }

        private int GetHeightRecursive(BinaryTreeNode<T> node)
        {
            if (node == null) return 0;
            return 1 + Math.Max(GetHeightRecursive(node.Left), GetHeightRecursive(node.Right));
        }
    }

    // BINARY SEARCH TREE (BST)
    // Purpose: Fast O(log n) search by Request ID
    public class BSTNode
    {
        public int Key { get; set; }
        public ServiceRequest Data { get; set; }
        public BSTNode Left { get; set; }
        public BSTNode Right { get; set; }

        public BSTNode(int key, ServiceRequest data)
        {
            Key = key;
            Data = data;
            Left = null;
            Right = null;
        }
    }

    public class ServiceRequestBST
    {
        private BSTNode root;

        public ServiceRequestBST()
        {
            root = null;
        }

        public void Insert(ServiceRequest request)
        {
            root = InsertRecursive(root, request.IssueID, request);
        }

        private BSTNode InsertRecursive(BSTNode node, int key, ServiceRequest data)
        {
            if (node == null)
            {
                return new BSTNode(key, data);
            }

            if (key < node.Key)
            {
                node.Left = InsertRecursive(node.Left, key, data);
            }
            else if (key > node.Key)
            {
                node.Right = InsertRecursive(node.Right, key, data);
            }

            return node;
        }

        public BSTNode Search(int key)
        {
            return SearchRecursive(root, key);
        }

        private BSTNode SearchRecursive(BSTNode node, int key)
        {
            if (node == null || node.Key == key)
            {
                return node;
            }

            if (key < node.Key)
            {
                return SearchRecursive(node.Left, key);
            }
            else
            {
                return SearchRecursive(node.Right, key);
            }
        }

        public List<ServiceRequest> InOrderTraversal()
        {
            List<ServiceRequest> result = new List<ServiceRequest>();
            InOrderRecursive(root, result);
            return result;
        }

        private void InOrderRecursive(BSTNode node, List<ServiceRequest> result)
        {
            if (node != null)
            {
                InOrderRecursive(node.Left, result);
                result.Add(node.Data);
                InOrderRecursive(node.Right, result);
            }
        }
    }

 
    // AVL TREE - Self-Balancing BST
    // Purpose: Guaranteed O(log n) with automatic balancing
    public class AVLNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public AVLNode<TKey, TValue> Left { get; set; }
        public AVLNode<TKey, TValue> Right { get; set; }
        public int Height { get; set; }

        public AVLNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Height = 1;
        }
    }

    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private AVLNode<TKey, TValue> root;

        public void Insert(TKey key, TValue value)
        {
            root = InsertRecursive(root, key, value);
        }

        private AVLNode<TKey, TValue> InsertRecursive(AVLNode<TKey, TValue> node, TKey key, TValue value)
        {
            if (node == null)
            {
                return new AVLNode<TKey, TValue>(key, value);
            }

            if (key.CompareTo(node.Key) < 0)
            {
                node.Left = InsertRecursive(node.Left, key, value);
            }
            else if (key.CompareTo(node.Key) > 0)
            {
                node.Right = InsertRecursive(node.Right, key, value);
            }
            else
            {
                return node;
            }

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            if (balance > 1 && key.CompareTo(node.Left.Key) < 0)
            {
                return RotateRight(node);
            }

            if (balance < -1 && key.CompareTo(node.Right.Key) > 0)
            {
                return RotateLeft(node);
            }

            if (balance > 1 && key.CompareTo(node.Left.Key) > 0)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            if (balance < -1 && key.CompareTo(node.Right.Key) < 0)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        private int GetHeight(AVLNode<TKey, TValue> node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(AVLNode<TKey, TValue> node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private AVLNode<TKey, TValue> RotateRight(AVLNode<TKey, TValue> y)
        {
            AVLNode<TKey, TValue> x = y.Left;
            AVLNode<TKey, TValue> T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        private AVLNode<TKey, TValue> RotateLeft(AVLNode<TKey, TValue> x)
        {
            AVLNode<TKey, TValue> y = x.Right;
            AVLNode<TKey, TValue> T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public TValue Search(TKey key)
        {
            var node = SearchRecursive(root, key);
            return node != null ? node.Value : default(TValue);
        }

        private AVLNode<TKey, TValue> SearchRecursive(AVLNode<TKey, TValue> node, TKey key)
        {
            if (node == null)
            {
                return null;
            }

            if (key.CompareTo(node.Key) == 0)
            {
                return node;
            }
            else if (key.CompareTo(node.Key) < 0)
            {
                return SearchRecursive(node.Left, key);
            }
            else
            {
                return SearchRecursive(node.Right, key);
            }
        }
    }

    // RED-BLACK TREE - Alternative Self-Balancing BST
    // Purpose: Fewer rotations than AVL, faster insertions
    public enum RBColor { Red, Black }

    public class RBNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public RBNode<TKey, TValue> Left { get; set; }
        public RBNode<TKey, TValue> Right { get; set; }
        public RBNode<TKey, TValue> Parent { get; set; }
        public RBColor Color { get; set; }

        public RBNode(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Color = RBColor.Red;
        }
    }

    public class RedBlackTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private RBNode<TKey, TValue> root;
        private RBNode<TKey, TValue> nil;

        public RedBlackTree()
        {
            nil = new RBNode<TKey, TValue>(default(TKey), default(TValue));
            nil.Color = RBColor.Black;
            root = nil;
        }

        public void Insert(TKey key, TValue value)
        {
            RBNode<TKey, TValue> newNode = new RBNode<TKey, TValue>(key, value);
            newNode.Left = nil;
            newNode.Right = nil;
            newNode.Parent = nil;

            RBNode<TKey, TValue> y = nil;
            RBNode<TKey, TValue> x = root;

            while (x != nil)
            {
                y = x;
                if (newNode.Key.CompareTo(x.Key) < 0)
                {
                    x = x.Left;
                }
                else
                {
                    x = x.Right;
                }
            }

            newNode.Parent = y;
            if (y == nil)
            {
                root = newNode;
            }
            else if (newNode.Key.CompareTo(y.Key) < 0)
            {
                y.Left = newNode;
            }
            else
            {
                y.Right = newNode;
            }

            newNode.Color = RBColor.Red;
            InsertFixup(newNode);
        }

        private void InsertFixup(RBNode<TKey, TValue> z)
        {
            while (z.Parent.Color == RBColor.Red)
            {
                if (z.Parent == z.Parent.Parent.Left)
                {
                    RBNode<TKey, TValue> y = z.Parent.Parent.Right;
                    if (y.Color == RBColor.Red)
                    {
                        z.Parent.Color = RBColor.Black;
                        y.Color = RBColor.Black;
                        z.Parent.Parent.Color = RBColor.Red;
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Right)
                        {
                            z = z.Parent;
                            LeftRotate(z);
                        }
                        z.Parent.Color = RBColor.Black;
                        z.Parent.Parent.Color = RBColor.Red;
                        RightRotate(z.Parent.Parent);
                    }
                }
                else
                {
                    RBNode<TKey, TValue> y = z.Parent.Parent.Left;
                    if (y.Color == RBColor.Red)
                    {
                        z.Parent.Color = RBColor.Black;
                        y.Color = RBColor.Black;
                        z.Parent.Parent.Color = RBColor.Red;
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Left)
                        {
                            z = z.Parent;
                            RightRotate(z);
                        }
                        z.Parent.Color = RBColor.Black;
                        z.Parent.Parent.Color = RBColor.Red;
                        LeftRotate(z.Parent.Parent);
                    }
                }
            }
            root.Color = RBColor.Black;
        }

        private void LeftRotate(RBNode<TKey, TValue> x)
        {
            RBNode<TKey, TValue> y = x.Right;
            x.Right = y.Left;
            if (y.Left != nil)
            {
                y.Left.Parent = x;
            }
            y.Parent = x.Parent;
            if (x.Parent == nil)
            {
                root = y;
            }
            else if (x == x.Parent.Left)
            {
                x.Parent.Left = y;
            }
            else
            {
                x.Parent.Right = y;
            }
            y.Left = x;
            x.Parent = y;
        }

        private void RightRotate(RBNode<TKey, TValue> y)
        {
            RBNode<TKey, TValue> x = y.Left;
            y.Left = x.Right;
            if (x.Right != nil)
            {
                x.Right.Parent = y;
            }
            x.Parent = y.Parent;
            if (y.Parent == nil)
            {
                root = x;
            }
            else if (y == y.Parent.Right)
            {
                y.Parent.Right = x;
            }
            else
            {
                y.Parent.Left = x;
            }
            x.Right = y;
            y.Parent = x;
        }

        public TValue Search(TKey key)
        {
            var node = SearchRecursive(root, key);
            return node != nil ? node.Value : default(TValue);
        }

        private RBNode<TKey, TValue> SearchRecursive(RBNode<TKey, TValue> node, TKey key)
        {
            if (node == nil || key.CompareTo(node.Key) == 0)
            {
                return node;
            }

            if (key.CompareTo(node.Key) < 0)
            {
                return SearchRecursive(node.Left, key);
            }
            else
            {
                return SearchRecursive(node.Right, key);
            }
        }
    }

    // HEAP - Max-Heap Priority Queue
    // Purpose: O(1) access to highest priority requests
    public class ServiceRequestHeap
    {
        private List<ServiceRequest> heap;

        public ServiceRequestHeap()
        {
            heap = new List<ServiceRequest>();
        }

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
                if (heap[index].GetPriorityValue() > heap[parentIndex].GetPriorityValue())
                {
                    Swap(index, parentIndex);
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        public ServiceRequest ExtractMax()
        {
            if (heap.Count == 0)
            {
                return null;
            }

            ServiceRequest max = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);

            if (heap.Count > 0)
            {
                HeapifyDown(0);
            }

            return max;
        }

        private void HeapifyDown(int index)
        {
            while (true)
            {
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;
                int largest = index;

                if (leftChild < heap.Count &&
                    heap[leftChild].GetPriorityValue() > heap[largest].GetPriorityValue())
                {
                    largest = leftChild;
                }

                if (rightChild < heap.Count &&
                    heap[rightChild].GetPriorityValue() > heap[largest].GetPriorityValue())
                {
                    largest = rightChild;
                }

                if (largest != index)
                {
                    Swap(index, largest);
                    index = largest;
                }
                else
                {
                    break;
                }
            }
        }

        private void Swap(int i, int j)
        {
            ServiceRequest temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        public List<ServiceRequest> GetTopPriority(int count)
        {
            return heap.OrderByDescending(r => r.GetPriorityValue()).Take(count).ToList();
        }

        public int Count => heap.Count;
    }

 
    // GRAPH - Adjacency List with BFS, DFS, MST
    // Purpose: Model complex request dependencies
    public class GraphEdge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Weight { get; set; }

        public GraphEdge(int from, int to, int weight)
        {
            From = from;
            To = to;
            Weight = weight;
        }
    }

    public class ServiceRequestGraph
    {
        private Dictionary<int, ServiceRequest> nodes;
        private Dictionary<int, List<GraphEdge>> adjacencyList;

        public ServiceRequestGraph()
        {
            nodes = new Dictionary<int, ServiceRequest>();
            adjacencyList = new Dictionary<int, List<GraphEdge>>();
        }

        public void AddNode(int id, ServiceRequest request)
        {
            if (!nodes.ContainsKey(id))
            {
                nodes[id] = request;
                adjacencyList[id] = new List<GraphEdge>();
            }
        }

        public void AddEdge(int from, int to, int weight = 1)
        {
            if (adjacencyList.ContainsKey(from))
            {
                adjacencyList[from].Add(new GraphEdge(from, to, weight));
            }
        }

        public List<int> GetDependencies(int nodeId)
        {
            if (adjacencyList.ContainsKey(nodeId))
            {
                return adjacencyList[nodeId].Select(e => e.To).ToList();
            }
            return new List<int>();
        }

        public List<GraphEdge> GetMinimumSpanningTree()
        {
            List<GraphEdge> allEdges = new List<GraphEdge>();
            foreach (var kvp in adjacencyList)
            {
                allEdges.AddRange(kvp.Value);
            }

            allEdges = allEdges.OrderBy(e => e.Weight).ToList();

            List<GraphEdge> mst = new List<GraphEdge>();
            UnionFind uf = new UnionFind(nodes.Keys.Max() + 1);

            foreach (var edge in allEdges)
            {
                if (uf.Find(edge.From) != uf.Find(edge.To))
                {
                    mst.Add(edge);
                    uf.Union(edge.From, edge.To);
                }
            }

            return mst;
        }

        public List<int> BreadthFirstSearch(int startNode)
        {
            List<int> visited = new List<int>();
            Queue<int> queue = new Queue<int>();

            queue.Enqueue(startNode);
            visited.Add(startNode);

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

        public List<int> DepthFirstSearch(int startNode)
        {
            List<int> visited = new List<int>();
            DFSRecursive(startNode, visited);
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
                    {
                        DFSRecursive(edge.To, visited);
                    }
                }
            }
        }
    }

   
    // UNION-FIND - For MST Cycle Detection
    // Purpose: Efficient cycle detection in Kruskal's algorithm
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
                parent[i] = i;
                rank[i] = 0;
            }
        }

        public int Find(int x)
        {
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]);
            }
            return parent[x];
        }

        public void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            if (rootX != rootY)
            {
                if (rank[rootX] < rank[rootY])
                {
                    parent[rootX] = rootY;
                }
                else if (rank[rootX] > rank[rootY])
                {
                    parent[rootY] = rootX;
                }
                else
                {
                    parent[rootY] = rootX;
                    rank[rootX]++;
                }
            }
        }
    }
}