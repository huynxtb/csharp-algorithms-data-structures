using System;
using System.Collections.Generic;

namespace FlowNetworkAlgorithm
{
    /// <summary>
    /// Represents a flow network graph with capacities.
    /// </summary>
    public class FlowNetwork
    {
        private readonly int _verticesCount;
        private readonly List<FlowEdge>[] _adjacencyList;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowNetwork"/> class.
        /// </summary>
        /// <param name="verticesCount">The number of vertices in the network.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when verticesCount is less than or equal to zero.</exception>
        public FlowNetwork(int verticesCount)
        {
            if (verticesCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(verticesCount), "Number of vertices must be greater than zero.");

            _verticesCount = verticesCount;
            _adjacencyList = new List<FlowEdge>[verticesCount];
            for (int i = 0; i < verticesCount; i++)
            {
                _adjacencyList[i] = new List<FlowEdge>();
            }
        }

        /// <summary>
        /// Adds a directed edge with a specified capacity to the flow network.
        /// </summary>
        /// <param name="from">The starting vertex of the edge.</param>
        /// <param name="to">The ending vertex of the edge.</param>
        /// <param name="capacity">The capacity of the edge.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when vertices are out of bounds.</exception>
        /// <exception cref="ArgumentException">Thrown when capacity is negative.</exception>
        public void AddEdge(int from, int to, int capacity)
        {
            ValidateVertex(from);
            ValidateVertex(to);
            if (capacity < 0)
                throw new ArgumentException("Edge capacity cannot be negative.", nameof(capacity));

            var forwardEdge = new FlowEdge(from, to, capacity);
            var backwardEdge = new FlowEdge(to, from, 0);

            forwardEdge.Residual = backwardEdge;
            backwardEdge.Residual = forwardEdge;

            _adjacencyList[from].Add(forwardEdge);
            _adjacencyList[to].Add(backwardEdge);
        }

        /// <summary>
        /// Computes the maximum flow from the source to the sink using the Ford-Fulkerson algorithm with DFS.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="sink">The sink vertex.</param>
        /// <returns>The maximum flow value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when source or sink is out of bounds.</exception>
        /// <exception cref="ArgumentException">Thrown when source and sink are the same.</exception>
        public int ComputeMaxFlow(int source, int sink)
        {
            ValidateVertex(source);
            ValidateVertex(sink);
            if (source == sink)
                throw new ArgumentException("Source and sink vertices must be distinct.");

            int maxFlow = 0;
            var parentEdges = new FlowEdge[_verticesCount];

            while (true)
            {
                var visited = new bool[_verticesCount];
                Array.Fill(parentEdges, null);

                if (!FindAugmentingPathDfs(source, sink, visited, parentEdges))
                {
                    break;
                }

                int bottleneck = int.MaxValue;
                int current = sink;
                while (current != source)
                {
                    var edge = parentEdges[current];
                    bottleneck = Math.Min(bottleneck, edge.ResidualCapacityTo(current));
                    current = edge.From;
                }

                current = sink;
                while (current != source)
                {
                    var edge = parentEdges[current];
                    edge.AddFlowTo(current, bottleneck);
                    current = edge.From;
                }

                maxFlow += bottleneck;
            }

            return maxFlow;
        }

        private bool FindAugmentingPathDfs(int current, int sink, bool[] visited, FlowEdge[] parentEdges)
        {
            visited[current] = true;

            if (current == sink)
            {
                return true;
            }

            foreach (var edge in _adjacencyList[current])
            {
                int to = edge.To;
                if (!visited[to] && edge.ResidualCapacityTo(to) > 0)
                {
                    parentEdges[to] = edge;
                    if (FindAugmentingPathDfs(to, sink, visited, parentEdges))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ValidateVertex(int vertex)
        {
            if (vertex < 0 || vertex >= _verticesCount)
                throw new ArgumentOutOfRangeException(nameof(vertex), $"Vertex index {vertex} is out of bounds [0, {_verticesCount - 1}].");
        }
    }

    /// <summary>
    /// Represents a directed edge in the flow network.
    /// </summary>
    public class FlowEdge
    {
        /// <summary>
        /// Gets the starting vertex of the edge.
        /// </summary>
        public int From { get; }

        /// <summary>
        /// Gets the ending vertex of the edge.
        /// </summary>
        public int To { get; }

        /// <summary>
        /// Gets the capacity of the edge.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Gets the current flow of the edge.
        /// </summary>
        public int Flow { get; internal set; }

        /// <summary>
        /// Gets the residual (reverse) edge.
        /// </summary>
        public FlowEdge Residual { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FlowEdge"/> class.
        /// </summary>
        /// <param name="from">The starting vertex.</param>
        /// <param name="to">The ending vertex.</param>
        /// <param name="capacity">The capacity of the edge.</param>
        public FlowEdge(int from, int to, int capacity)
        {
            From = from;
            To = to;
            Capacity = capacity;
            Flow = 0;
        }

        /// <summary>
        /// Calculates the residual capacity of the edge towards the target vertex.
        /// </summary>
        /// <param name="target">The target vertex.</param>
        /// <returns>The residual capacity.</returns>
        public int ResidualCapacityTo(int target)
        {
            if (target == To) return Capacity - Flow;
            if (target == From) return Flow;
            throw new ArgumentException("Invalid target vertex for edge.");
        }

        /// <summary>
        /// Adds flow to the edge towards the target vertex.
        /// </summary>
        /// <param name="target">The target vertex.</param>
        /// <param name="deltaFlow">The amount of flow to add.</param>
        public void AddFlowTo(int target, int deltaFlow)
        {
            if (target == To)
            {
                Flow += deltaFlow;
                Residual.Flow -= deltaFlow;
            }
            else if (target == From)
            {
                Flow -= deltaFlow;
                Residual.Flow += deltaFlow;
            }
            else
            {
                throw new ArgumentException("Invalid target vertex for edge.");
            }
        }
    }
}