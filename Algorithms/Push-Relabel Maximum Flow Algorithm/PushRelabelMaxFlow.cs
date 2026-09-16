using System;
using System.Collections.Generic;

namespace FlowAlgorithms
{
    /// <summary>
    /// Represents a directed edge in the residual flow network.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Gets the source vertex index.
        /// </summary>
        public int From { get; }

        /// <summary>
        /// Gets the destination vertex index.
        /// </summary>
        public int To { get; }

        /// <summary>
        /// Gets the maximum capacity of this edge.
        /// </summary>
        public long Capacity { get; }

        /// <summary>
        /// Gets or sets the current flow along this edge.
        /// </summary>
        public long Flow { get; set; }

        /// <summary>
        /// Gets the index of the reverse edge in the adjacency list of the destination vertex.
        /// </summary>
        public int ReverseIndex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="from">Source vertex index.</param>
        /// <param name="to">Destination vertex index.</param>
        /// <param name="capacity">Maximum capacity of the edge.</param>
        /// <param name="reverseIndex">Index of the reverse edge in adjacency list of destination vertex.</param>
        public Edge(int from, int to, long capacity, int reverseIndex)
        {
            From = from;
            To = to;
            Capacity = capacity;
            Flow = 0;
            ReverseIndex = reverseIndex;
        }
    }

    /// <summary>
    /// Implements the Push-Relabel (Preflow-Push) maximum network flow algorithm
    /// with the Gap Heuristic and FIFO discharge mechanism.
    /// </summary>
    public class PushRelabelMaxFlow
    {
        private readonly int _nodeCount;
        private readonly List<Edge>[] _adjacencyList;
        private readonly List<Edge> _allEdges;
        private readonly long[] _excess;
        private readonly int[] _height;
        private readonly int[] _heightCount;
        private readonly bool[] _inQueue;
        private readonly Queue<int> _activeQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushRelabelMaxFlow"/> class.
        /// </summary>
        /// <param name="nodeCount">The total number of vertices in the graph (0 to nodeCount - 1).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when nodeCount is less than or equal to 0.</exception>
        public PushRelabelMaxFlow(int nodeCount)
        {
            if (nodeCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(nodeCount), "Node count must be positive.");
            }

            _nodeCount = nodeCount;
            _adjacencyList = new List<Edge>[nodeCount];
            for (int i = 0; i < nodeCount; i++)
            {
                _adjacencyList[i] = new List<Edge>();
            }

            _allEdges = new List<Edge>();
            _excess = new long[nodeCount];
            _height = new int[nodeCount];
            _heightCount = new int[2 * nodeCount + 1];
            _inQueue = new bool[nodeCount];
            _activeQueue = new Queue<int>();
        }

        /// <summary>
        /// Adds a directed edge with a given capacity to the network.
        /// </summary>
        /// <param name="from">Source vertex index.</param>
        /// <param name="to">Destination vertex index.</param>
        /// <param name="capacity">Non-negative flow capacity.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when vertex indices are out of bounds or capacity is negative.</exception>
        public void AddEdge(int from, int to, long capacity)
        {
            if (from < 0 || from >= _nodeCount)
            {
                throw new ArgumentOutOfRangeException(nameof(from), "Source vertex out of range.");
            }
            if (to < 0 || to >= _nodeCount)
            {
                throw new ArgumentOutOfRangeException(nameof(to), "Destination vertex out of range.");
            }
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative.");
            }

            Edge forwardEdge = new Edge(from, to, capacity, _adjacencyList[to].Count);
            Edge backwardEdge = new Edge(to, from, 0, _adjacencyList[from].Count);

            _adjacencyList[from].Add(forwardEdge);
            _adjacencyList[to].Add(backwardEdge);
            _allEdges.Add(forwardEdge);
        }

        /// <summary>
        /// Computes and returns the maximum flow from the source vertex to the sink vertex.
        /// </summary>
        /// <param name="source">Source vertex index.</param>
        /// <param name="sink">Sink vertex index.</param>
        /// <returns>The total maximum flow value.</returns>
        /// <exception cref="ArgumentException">Thrown when source and sink vertices are identical or out of range.</exception>
        public long GetMaxFlow(int source, int sink)
        {
            if (source < 0 || source >= _nodeCount || sink < 0 || sink >= _nodeCount)
            {
                throw new ArgumentOutOfRangeException("Source or sink vertex is out of range.");
            }
            if (source == sink)
            {
                throw new ArgumentException("Source and sink must be distinct vertices.");
            }

            InitializePreflow(source);

            while (_activeQueue.Count > 0)
            {
                int u = _activeQueue.Dequeue();
                _inQueue[u] = false;

                if (u == source || u == sink)
                {
                    continue;
                }

                Discharge(u, source, sink);
            }

            long maxFlow = 0;
            foreach (Edge edge in _adjacencyList[source])
            {
                maxFlow += edge.Flow;
            }

            return maxFlow;
        }

        /// <summary>
        /// Returns all forward edges in the network with their calculated flows.
        /// </summary>
        /// <returns>A read-only collection of directed edges.</returns>
        public IReadOnlyList<Edge> GetEdges()
        {
            return _allEdges.AsReadOnly();
        }

        /// <summary>
        /// Identifies the minimum s-t cut partition reachable from the source in the residual graph.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <returns>A boolean array where true indicates membership in the source cut component S.</returns>
        public bool[] GetMinCut(int source)
        {
            if (source < 0 || source >= _nodeCount)
            {
                throw new ArgumentOutOfRangeException(nameof(source), "Source vertex out of range.");
            }

            bool[] visited = new bool[_nodeCount];
            Queue<int> queue = new Queue<int>();

            visited[source] = true;
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();
                foreach (Edge edge in _adjacencyList[u])
                {
                    if (!visited[edge.To] && edge.Capacity - edge.Flow > 0)
                    {
                        visited[edge.To] = true;
                        queue.Enqueue(edge.To);
                    }
                }
            }

            return visited;
        }

        private void InitializePreflow(int source)
        {
            Array.Clear(_excess, 0, _nodeCount);
            Array.Clear(_height, 0, _nodeCount);
            Array.Clear(_heightCount, 0, _heightCount.Length);
            Array.Clear(_inQueue, 0, _nodeCount);
            _activeQueue.Clear();

            foreach (Edge edge in _allEdges)
            {
                edge.Flow = 0;
                _adjacencyList[edge.To][edge.ReverseIndex].Flow = 0;
            }

            _height[source] = _nodeCount;
            _heightCount[_nodeCount] = 1;
            _heightCount[0] = _nodeCount - 1;

            foreach (Edge edge in _adjacencyList[source])
            {
                long pushFlow = edge.Capacity;
                if (pushFlow > 0)
                {
                    edge.Flow += pushFlow;
                    _adjacencyList[edge.To][edge.ReverseIndex].Flow -= pushFlow;
                    _excess[edge.To] += pushFlow;
                    _excess[source] -= pushFlow;

                    EnqueueActive(edge.To, source);
                }
            }
        }

        private void Push(Edge edge, int source, int sink)
        {
            long pushAmount = Math.Min(_excess[edge.From], edge.Capacity - edge.Flow);
            if (pushAmount <= 0 || _height[edge.From] != _height[edge.To] + 1)
            {
                return;
            }

            edge.Flow += pushAmount;
            _adjacencyList[edge.To][edge.ReverseIndex].Flow -= pushAmount;
            _excess[edge.From] -= pushAmount;
            _excess[edge.To] += pushAmount;

            if (edge.To != source && edge.To != sink)
            {
                EnqueueActive(edge.To, source);
            }
        }

        private void Relabel(int u)
        {
            int minNeighborHeight = int.MaxValue;
            foreach (Edge edge in _adjacencyList[u])
            {
                if (edge.Capacity - edge.Flow > 0)
                {
                    minNeighborHeight = Math.Min(minNeighborHeight, _height[edge.To]);
                }
            }

            if (minNeighborHeight < int.MaxValue)
            {
                int oldHeight = _height[u];
                int newHeight = minNeighborHeight + 1;

                _heightCount[oldHeight]--;
                _height[u] = newHeight;

                if (newHeight < _heightCount.Length)
                {
                    _heightCount[newHeight]++;
                }

                // Gap Heuristic: if a height level becomes empty, disconnected nodes above it cannot reach the sink
                if (_heightCount[oldHeight] == 0 && oldHeight < _nodeCount)
                {
                    for (int v = 0; v < _nodeCount; v++)
                    {
                        if (_height[v] > oldHeight && _height[v] < _nodeCount)
                        {
                            _heightCount[_height[v]]--;
                            _height[v] = _nodeCount + 1;
                            _heightCount[_height[v]]++;
                        }
                    }
                }
            }
        }

        private void Discharge(int u, int source, int sink)
        {
            while (_excess[u] > 0)
            {
                bool pushedAny = false;
                foreach (Edge edge in _adjacencyList[u])
                {
                    if (edge.Capacity - edge.Flow > 0 && _height[u] == _height[edge.To] + 1)
                    {
                        Push(edge, source, sink);
                        pushedAny = true;
                        if (_excess[u] == 0)
                        {
                            break;
                        }
                    }
                }

                if (!pushedAny)
                {
                    Relabel(u);
                    if (_height[u] >= _nodeCount)
                    {
                        break;
                    }
                }
            }
        }

        private void EnqueueActive(int u, int source)
        {
            if (u != source && !_inQueue[u] && _excess[u] > 0)
            {
                _inQueue[u] = true;
                _activeQueue.Enqueue(u);
            }
        }
    }
}