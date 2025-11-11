using System;
using System.Collections.Generic;
using System.Linq;

namespace MunicipalServicesApp
{
    // ============================================================================
    // BASIC TREE (N-ary Tree) - Hierarchical Category Organization
    // ============================================================================
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

    // ============================================================================
    // BST FOR SERVICE REQUESTS (sorted by IssueID)
    // ============================================================================
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

        public void Insert(ServiceRequest request)
        {
            if (!int.TryParse(request.IssueID, out int key))
                throw new ArgumentException("IssueID must be numeric for BST insertion.");

            root = InsertRecursive(root, key, request);
        }

        private BSTNode InsertRecursive(BSTNode node, int key, ServiceRequest data)
        {
            if (node == null) return new BSTNode(key, data);

            if (key < node.Key)
                node.Left = InsertRecursive(node.Left, key, data);
            else if (key > node.Key)
                node.Right = InsertRecursive(node.Right, key, data);

            return node;
        }

        public BSTNode Search(int key) => SearchRecursive(root, key);

        private BSTNode SearchRecursive(BSTNode node, int key)
        {
            if (node == null || node.Key == key) return node;
            return key < node.Key ? SearchRecursive(node.Left, key) : SearchRecursive(node.Right, key);
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

    // ============================================================================
    // HEAP - Max-Heap for ServiceRequest Priority
    // ============================================================================
    public class ServiceRequestHeap
    {
        private List<ServiceRequest> heap;

        public ServiceRequestHeap() => heap = new List<ServiceRequest>();

        public void Insert(ServiceRequest request)
        {
            heap.Add(request);
            HeapifyUp(heap.Count - 1);
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (heap[index].GetPriorityValue() > heap[parent].GetPriorityValue())
                {
                    Swap(index, parent);
                    index = parent;
                }
                else break;
            }
        }

        public ServiceRequest ExtractMax()
        {
            if (heap.Count == 0) return null;

            ServiceRequest max = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            if (heap.Count > 0) HeapifyDown(0);
            return max;
        }

        private void HeapifyDown(int index)
        {
            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int largest = index;

                if (left < heap.Count && heap[left].GetPriorityValue() > heap[largest].GetPriorityValue())
                    largest = left;
                if (right < heap.Count && heap[right].GetPriorityValue() > heap[largest].GetPriorityValue())
                    largest = right;

                if (largest != index)
                {
                    Swap(index, largest);
                    index = largest;
                }
                else break;
            }
        }

        private void Swap(int i, int j)
        {
            var temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }

        public int Count => heap.Count;
    }

    // ============================================================================
    // GRAPH AND UNION-FIND FOR MST / DEPENDENCIES
    // ============================================================================
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
                adjacencyList[from].Add(new GraphEdge(from, to, weight));
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

    public class UnionFind
    {
        private int[] parent;
        private int[] rank;

        public UnionFind(int size)
        {
            parent = new int[size];
            rank = new int[size];
            for (int i = 0; i < size; i++) parent[i] = i;
        }

        public int Find(int x) => parent[x] == x ? x : (parent[x] = Find(parent[x]));

        public void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);
            if (rootX == rootY) return;

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
}
