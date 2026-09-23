using System;
using System.Collections.Generic;

namespace AdvancedDataStructures
{
    /// <summary>
    /// Sleator and Tarjan's Link-Cut Tree data structure for maintaining a dynamic forest of trees.
    /// Supports dynamic connectivity, edge insertions/deletions, path aggregate queries, and path updates.
    /// </summary>
    public class LinkCutTree
    {
        private sealed class Node
        {
            public int Id;
            public Node? Parent;
            public Node? Left;
            public Node? Right;
            public bool Reversed;

            public long Value;
            public long AggregateSum;
            public long AggregateMin;
            public long AggregateMax;
            public int SubtreeSize;

            public Node(int id, long value)
            {
                Id = id;
                Value = value;
                AggregateSum = value;
                AggregateMin = value;
                AggregateMax = value;
                SubtreeSize = 1;
            }
        }

        private readonly Node[] _nodes;
        private readonly int _capacity;

        /// <summary>
        /// Initializes a Link-Cut Tree with a fixed number of nodes (0-indexed: 0 to capacity - 1).
        /// Each node is initialized with an initial value of 0.
        /// </summary>
        /// <param name="capacity">The maximum number of nodes in the forest.</param>
        public LinkCutTree(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be strictly positive.");
            }

            _capacity = capacity;
            _nodes = new Node[capacity];
            for (int i = 0; i < capacity; i++)
            {
                _nodes[i] = new Node(i, 0);
            }
        }

        /// <summary>
        /// Initializes a Link-Cut Tree with node initial values.
        /// </summary>
        /// <param name="initialValues">The initial values for nodes 0 through length - 1.</param>
        public LinkCutTree(IReadOnlyList<long> initialValues)
        {
            if (initialValues == null)
            {
                throw new ArgumentNullException(nameof(initialValues));
            }
            if (initialValues.Count == 0)
            {
                throw new ArgumentException("Initial values cannot be empty.", nameof(initialValues));
            }

            _capacity = initialValues.Count;
            _nodes = new Node[_capacity];
            for (int i = 0; i < _capacity; i++)
            {
                _nodes[i] = new Node(i, initialValues[i]);
            }
        }

        /// <summary>
        /// Determines if a node is the root of an auxiliary Splay Tree.
        /// </summary>
        private static bool IsSplayRoot(Node x)
        {
            return x.Parent == null || (x.Parent.Left != x && x.Parent.Right != x);
        }

        /// <summary>
        /// Reverses the subtree represented by the given node.
        /// </summary>
        private static void PushFlip(Node? x)
        {
            if (x == null) return;
            x.Reversed = !x.Reversed;
            var temp = x.Left;
            x.Left = x.Right;
            x.Right = temp;
        }

        /// <summary>
        /// Propagates lazy tags (path reversal) downwards to children.
        /// </summary>
        private static void PushDown(Node x)
        {
            if (x.Reversed)
            {
                PushFlip(x.Left);
                PushFlip(x.Right);
                x.Reversed = false;
            }
        }

        /// <summary>
        /// Recalculates subtree aggregates from children.
        /// </summary>
        private static void PushUp(Node x)
        {
            x.SubtreeSize = 1;
            x.AggregateSum = x.Value;
            x.AggregateMin = x.Value;
            x.AggregateMax = x.Value;

            if (x.Left != null)
            {
                x.SubtreeSize += x.Left.SubtreeSize;
                x.AggregateSum += x.Left.AggregateSum;
                x.AggregateMin = Math.Min(x.AggregateMin, x.Left.AggregateMin);
                x.AggregateMax = Math.Max(x.AggregateMax, x.Left.AggregateMax);
            }

            if (x.Right != null)
            {
                x.SubtreeSize += x.Right.SubtreeSize;
                x.AggregateSum += x.Right.AggregateSum;
                x.AggregateMin = Math.Min(x.AggregateMin, x.Right.AggregateMin);
                x.AggregateMax = Math.Max(x.AggregateMax, x.Right.AggregateMax);
            }
        }

        /// <summary>
        /// Rotates node x upwards in its auxiliary Splay tree.
        /// </summary>
        private static void Rotate(Node x)
        {
            var p = x.Parent!;
            var g = p.Parent;
            bool isLeftChild = (p.Left == x);

            if (isLeftChild)
            {
                p.Left = x.Right;
                if (x.Right != null) x.Right.Parent = p;
                x.Right = p;
            }
            else
            {
                p.Right = x.Left;
                if (x.Left != null) x.Left.Parent = p;
                x.Left = p;
            }

            p.Parent = x;
            x.Parent = g;

            if (g != null)
            {
                if (g.Left == p)
                {
                    g.Left = x;
                }
                else if (g.Right == p)
                {
                    g.Right = x;
                }
            }

            PushUp(p);
            PushUp(x);
        }

        /// <summary>
        /// Splays node x to the root of its auxiliary Splay tree.
        /// </summary>
        private static void Splay(Node x)
        {
            // Push lazy tags along the path down to x
            static void PushDownFromRoot(Node curr)
            {
                if (!IsSplayRoot(curr) && curr.Parent != null)
                {
                    PushDownFromRoot(curr.Parent);
                }
                PushDown(curr);
            }

            PushDownFromRoot(x);

            while (!IsSplayRoot(x))
            {
                var p = x.Parent!;
                var g = p.Parent;

                if (!IsSplayRoot(p) && g != null)
                {
                    bool zigzig = (p.Left == x) == (g.Left == p);
                    if (zigzig)
                    {
                        Rotate(p);
                    }
                    else
                    {
                        Rotate(x);
                    }
                }
                Rotate(x);
            }
        }

        /// <summary>
        /// Makes the path from the root of the represented tree to node x a preferred path,
        /// represented as a single Splay tree where x is the rightmost/deepest node.
        /// </summary>
        /// <param name="x">The target node.</param>
        /// <returns>The last path-parent encountered.</returns>
        private static Node? Access(Node x)
        {
            Node? last = null;
            for (Node? curr = x; curr != null; curr = curr.Parent)
            {
                Splay(curr);
                curr.Right = last;
                PushUp(curr);
                last = curr;
            }
            Splay(x);
            return last;
        }

        /// <summary>
        /// Makes node x the root of its represented tree in the forest.
        /// </summary>
        private static void MakeRoot(Node x)
        {
            Access(x);
            PushFlip(x);
        }

        private void ValidateId(int id)
        {
            if (id < 0 || id >= _capacity)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"Node ID {id} is out of bounds [0, {_capacity - 1}].");
            }
        }

        /// <summary>
        /// Makes the path from the tree root to node u a preferred path.
        /// </summary>
        public void Access(int u)
        {
            ValidateId(u);
            Access(_nodes[u]);
        }

        /// <summary>
        /// Re-roots the tree containing node u such that u becomes the root of the represented tree.
        /// </summary>
        public void MakeRoot(int u)
        {
            ValidateId(u);
            MakeRoot(_nodes[u]);
        }

        /// <summary>
        /// Finds the identifier of the root of the represented tree containing node u.
        /// </summary>
        public int FindRoot(int u)
        {
            ValidateId(u);
            var node = _nodes[u];
            Access(node);
            while (node.Left != null)
            {
                PushDown(node);
                node = node.Left;
            }
            Splay(node);
            return node.Id;
        }

        /// <summary>
        /// Connects nodes u and v by adding an undirected edge between them.
        /// Returns false if u and v are already in the same tree.
        /// </summary>
        public bool Link(int u, int v)
        {
            ValidateId(u);
            ValidateId(v);
            if (u == v) return false;

            var nodeU = _nodes[u];
            var nodeV = _nodes[v];

            MakeRoot(nodeU);
            if (FindRoot(v) == u)
            {
                return false; // Already connected
            }

            nodeU.Parent = nodeV;
            return true;
        }

        /// <summary>
        /// Disconnects the edge between node u and node v.
        /// Returns false if no direct edge exists between u and v.
        /// </summary>
        public bool Cut(int u, int v)
        {
            ValidateId(u);
            ValidateId(v);
            if (u == v) return false;

            var nodeU = _nodes[u];
            var nodeV = _nodes[v];

            MakeRoot(nodeU);
            if (FindRoot(v) != u || nodeV.Parent != nodeU || nodeV.Left != null)
            {
                return false; // No direct edge exists
            }

            nodeV.Parent = null;
            nodeU.Right = null;
            PushUp(nodeU);
            return true;
        }

        /// <summary>
        /// Checks if node u and node v reside in the same connected tree.
        /// </summary>
        public bool IsConnected(int u, int v)
        {
            ValidateId(u);
            ValidateId(v);
            if (u == v) return true;
            return FindRoot(u) == FindRoot(v);
        }

        /// <summary>
        /// Updates the stored value at node u.
        /// </summary>
        public void SetValue(int u, long value)
        {
            ValidateId(u);
            var node = _nodes[u];
            Access(node);
            node.Value = value;
            PushUp(node);
        }

        /// <summary>
        /// Retrieves the value stored at node u.
        /// </summary>
        public long GetValue(int u)
        {
            ValidateId(u);
            return _nodes[u].Value;
        }

        /// <summary>
        /// Returns aggregate metrics along the simple path between u and v.
        /// </summary>
        /// <param name="u">Start node id.</param>
        /// <param name="v">End node id.</param>
        /// <returns>A tuple containing (Sum, Min, Max, Count) along the path.</returns>
        /// <exception cref="InvalidOperationException">Thrown if u and v are not in the same tree.</exception>
        public (long Sum, long Min, long Max, int Count) QueryPath(int u, int v)
        {
            ValidateId(u);
            ValidateId(v);

            var nodeU = _nodes[u];
            var nodeV = _nodes[v];

            MakeRoot(nodeU);
            Access(nodeV);

            if (u != v && nodeU.Parent == null && nodeV.Left != nodeU)
            {
                // Check if actually connected
                if (FindRoot(v) != u)
                {
                    throw new InvalidOperationException($"Nodes {u} and {v} are not connected.");
                }
                // Re-establish root after FindRoot side-effects
                MakeRoot(nodeU);
                Access(nodeV);
            }

            return (nodeV.AggregateSum, nodeV.AggregateMin, nodeV.AggregateMax, nodeV.SubtreeSize);
        }
    }
}