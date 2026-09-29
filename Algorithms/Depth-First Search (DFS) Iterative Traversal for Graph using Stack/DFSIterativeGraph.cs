using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Graph
{
    /// <summary>
    /// Represents an undirected graph using adjacency list representation.
    /// Provides iterative Depth-First Search (DFS) traversal methods.
    /// </summary>
    public class Graph
    {
        private readonly int _vertexCount;
        private readonly List<int>[] _adjacencyList;

        /// <summary>
        /// Initializes a new instance of the Graph class with the specified number of vertices.
        /// </summary>
        /// <param name="vertexCount">The number of vertices in the graph.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when vertexCount is less than or equal to 0.</exception>
        public Graph(int vertexCount)
        {
            if (vertexCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(vertexCount), "Vertex count must be greater than 0.");

            _vertexCount = vertexCount;
            _adjacencyList = new List<int>[vertexCount];

            for (int i = 0; i < vertexCount; i++)
            {
                _adjacencyList[i] = new List<int>();
            }
        }

        /// <summary>
        /// Adds an undirected edge between the source and destination vertices.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="destination">The destination vertex.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when source or destination is out of valid range.</exception>
        public void AddEdge(int source, int destination)
        {
            if (source < 0 || source >= _vertexCount)
                throw new ArgumentOutOfRangeException(nameof(source), "Source vertex is out of range.");
            if (destination < 0 || destination >= _vertexCount)
                throw new ArgumentOutOfRangeException(nameof(destination), "Destination vertex is out of range.");

            // Add edge from source to destination
            if (!_adjacencyList[source].Contains(destination))
                _adjacencyList[source].Add(destination);

            // Add edge from destination to source (undirected graph)
            if (!_adjacencyList[destination].Contains(source))
                _adjacencyList[destination].Add(source);
        }

        /// <summary>
        /// Performs an iterative Depth-First Search starting from the specified vertex.
        /// </summary>
        /// <param name="startVertex">The starting vertex for DFS traversal.</param>
        /// <returns>A list of vertices in the order they were visited.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when startVertex is out of valid range.</exception>
        public List<int> DFSIterative(int startVertex)
        {
            if (startVertex < 0 || startVertex >= _vertexCount)
                throw new ArgumentOutOfRangeException(nameof(startVertex), "Start vertex is out of range.");

            var visited = new bool[_vertexCount];
            var result = new List<int>();
            var stack = new Stack<int>();

            stack.Push(startVertex);
            visited[startVertex] = true;

            while (stack.Count > 0)
            {
                int vertex = stack.Pop();
                result.Add(vertex);

                // Push all adjacent unvisited vertices onto the stack
                for (int i = _adjacencyList[vertex].Count - 1; i >= 0; i--)
                {
                    int neighbor = _adjacencyList[vertex][i];
                    if (!visited[neighbor])
                    {
                        visited[neighbor] = true;
                        stack.Push(neighbor);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Performs iterative Depth-First Search on all vertices in the graph,
        /// ensuring all connected components are visited.
        /// </summary>
        /// <returns>A list of all vertices in DFS order across all connected components.</returns>
        public List<int> DFSIterativeAll()
        {
            var visited = new bool[_vertexCount];
            var result = new List<int>();
            var stack = new Stack<int>();

            for (int i = 0; i < _vertexCount; i++)
            {
                if (!visited[i])
                {
                    stack.Push(i);
                    visited[i] = true;

                    while (stack.Count > 0)
                    {
                        int vertex = stack.Pop();
                        result.Add(vertex);

                        // Push all adjacent unvisited vertices onto the stack
                        for (int j = _adjacencyList[vertex].Count - 1; j >= 0; j--)
                        {
                            int neighbor = _adjacencyList[vertex][j];
                            if (!visited[neighbor])
                            {
                                visited[neighbor] = true;
                                stack.Push(neighbor);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether a path exists between the source and destination vertices
        /// using iterative Depth-First Search.
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="destination">The destination vertex.</param>
        /// <returns>True if a path exists; otherwise, false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when source or destination is out of valid range.</exception>
        public bool HasPath(int source, int destination)
        {
            if (source < 0 || source >= _vertexCount)
                throw new ArgumentOutOfRangeException(nameof(source), "Source vertex is out of range.");
            if (destination < 0 || destination >= _vertexCount)
                throw new ArgumentOutOfRangeException(nameof(destination), "Destination vertex is out of range.");

            if (source == destination)
                return true;

            var visited = new bool[_vertexCount];
            var stack = new Stack<int>();

            stack.Push(source);
            visited[source] = true;

            while (stack.Count > 0)
            {
                int vertex = stack.Pop();

                foreach (int neighbor in _adjacencyList[vertex])
                {
                    if (neighbor == destination)
                        return true;

                    if (!visited[neighbor])
                    {
                        visited[neighbor] = true;
                        stack.Push(neighbor);
                    }
                }
            }

            return false;
        }
    }
}