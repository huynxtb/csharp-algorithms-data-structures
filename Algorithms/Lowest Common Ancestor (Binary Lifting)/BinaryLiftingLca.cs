using System;
using System.Collections.Generic;

namespace AdvancedAlgorithms.Graphs
{
    /// <summary>
    /// Computes the Lowest Common Ancestor (LCA) of nodes in a tree using the Binary Lifting technique.
    /// </summary>
    public class BinaryLiftingLca
    {
        private readonly int _nodeCount;
        private readonly int _log;
        private readonly int[] _depth;
        private readonly int[,] _up;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryLiftingLca"/> class.
        /// </summary>
        /// <param name="nodeCount">The total number of nodes in the tree.</param>
        /// <param name="adjacencyList">The adjacency list representing the tree.</param>
        /// <param name="root">The root node of the tree.</param>
        /// <exception cref="ArgumentNullException">Thrown when adjacencyList is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when nodeCount is less than or equal to 0, or root is out of bounds.</exception>
        /// <exception cref="ArgumentException">Thrown when adjacencyList length does not match nodeCount.</exception>
        public BinaryLiftingLca(int nodeCount, IList<int>[] adjacencyList, int root = 0)
        {
            if (adjacencyList == null)
            {
                throw new ArgumentNullException(nameof(adjacencyList));
            }
            if (nodeCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nodeCount), "Node count must be greater than zero.");
            }
            if (adjacencyList.Length != nodeCount)
            {
                throw new ArgumentException("Adjacency list length must match the node count.", nameof(adjacencyList));
            }
            if (root < 0 || root >= nodeCount)
            {
                throw new ArgumentOutOfRangeException(nameof(root), "Root node index is out of bounds.");
            }

            _nodeCount = nodeCount;
            
            // Calculate max power of 2 needed
            _log = 1;
            while ((1 << _log) <= _nodeCount)
            {
                _log++;
            }

            _depth = new int[_nodeCount];
            _up = new int[_nodeCount, _log];

            Initialize(adjacencyList, root);
        }

        private void Initialize(IList<int>[] adjacencyList, int root)
        {
            var visited = new bool[_nodeCount];
            var queue = new Queue<int>();

            queue.Enqueue(root);
            visited[root] = true;
            _up[root, 0] = root;
            _depth[root] = 0;

            while (queue.Count > 0)
            {
                int curr = queue.Dequeue();
                IList<int> neighbors = adjacencyList[curr];
                if (neighbors == null) continue;

                foreach (int neighbor in neighbors)
                {
                    if (neighbor < 0 || neighbor >= _nodeCount)
                    {
                        throw new ArgumentException($"Invalid neighbor index {neighbor} detected in adjacency list.");
                    }

                    if (!visited[neighbor])
                    {
                        visited[neighbor] = true;
                        _depth[neighbor] = _depth[curr] + 1;
                        _up[neighbor, 0] = curr;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            // Fill the binary lifting table
            for (int j = 1; j < _log; j++)
            {
                for (int i = 0; i < _nodeCount; i++)
                {
                    _up[i, j] = _up[_up[i, j - 1], j - 1];
                }
            }
        }

        /// <summary>
        /// Gets the Lowest Common Ancestor of nodes u and v.
        /// </summary>
        /// <param name="u">The first node.</param>
        /// <param name="v">The second node.</param>
        /// <returns>The node index of the LCA.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when u or v are out of bounds.</exception>
        public int GetLca(int u, int v)
        {
            ValidateNodeIndex(u, nameof(u));
            ValidateNodeIndex(v, nameof(v));

            if (u == v)
            {
                return u;
            }

            // Ensure u is deeper than or equal to v
            if (_depth[u] < _depth[v])
            {
                int temp = u;
                u = v;
                v = temp;
            }

            // Lift u to the same depth as v
            int diff = _depth[u] - _depth[v];
            for (int j = _log - 1; j >= 0; j--)
            {
                if ((diff & (1 << j)) != 0)
                {
                    u = _up[u, j];
                }
            }

            if (u == v)
            {
                return u;
            }

            // Lift both nodes together until they are just below their LCA
            for (int j = _log - 1; j >= 0; j--)
            {
                if (_up[u, j] != _up[v, j])
                {
                    u = _up[u, j];
                    v = _up[v, j];
                }
            }

            return _up[u, 0];
        }

        /// <summary>
        /// Gets the distance (number of edges) between nodes u and v.
        /// </summary>
        /// <param name="u">The first node.</param>
        /// <param name="v">The second node.</param>
        /// <returns>The distance between the nodes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when u or v are out of bounds.</exception>
        public int GetDistance(int u, int v)
        {
            ValidateNodeIndex(u, nameof(u));
            ValidateNodeIndex(v, nameof(v));

            int lca = GetLca(u, v);
            return _depth[u] + _depth[v] - 2 * _depth[lca];
        }

        private void ValidateNodeIndex(int node, string paramName)
        {
            if (node < 0 || node >= _nodeCount)
            {
                throw new ArgumentOutOfRangeException(paramName, $"Node index {node} is out of bounds. Must be between 0 and {_nodeCount - 1}.");
            }
        }
    }
}