using System;
using System.Collections.Generic;

/// <summary>
/// Represents an adjacency list graph supporting both directed and undirected configurations,
/// along with recursive Depth-First Search (DFS) traversal operations.
/// </summary>
public class Graph
{
    private readonly Dictionary<int, List<int>> _adjacencyList;
    private readonly bool _isDirected;

    /// <summary>
    /// Gets the number of vertices currently in the graph.
    /// </summary>
    public int VertexCount => _adjacencyList.Count;

    /// <summary>
    /// Gets a value indicating whether the graph is directed.
    /// </summary>
    public bool IsDirected => _isDirected;

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph"/> class.
    /// </summary>
    /// <param name="isDirected">Specifies whether edges added to the graph are directed (default is false for undirected).</param>
    public Graph(bool isDirected = false)
    {
        _adjacencyList = new Dictionary<int, List<int>>();
        _isDirected = isDirected;
    }

    /// <summary>
    /// Adds a vertex to the graph if it does not already exist.
    /// </summary>
    /// <param name="vertex">The integer identifier of the vertex to add.</param>
    /// <returns><c>true</c> if the vertex was added; <c>false</c> if the vertex already existed.</returns>
    public bool AddVertex(int vertex)
    {
        if (_adjacencyList.ContainsKey(vertex))
        {
            return false;
        }

        _adjacencyList[vertex] = new List<int>();
        return true;
    }

    /// <summary>
    /// Adds an edge between the specified source and destination vertices.
    /// Automatically adds endpoints if they do not already exist in the graph.
    /// </summary>
    /// <param name="source">The starting vertex identifier.</param>
    /// <param name="destination">The ending vertex identifier.</param>
    public void AddEdge(int source, int destination)
    {
        AddVertex(source);
        AddVertex(destination);

        _adjacencyList[source].Add(destination);

        if (!_isDirected && source != destination)
        {
            _adjacencyList[destination].Add(source);
        }
    }

    /// <summary>
    /// Checks if a vertex exists in the graph.
    /// </summary>
    /// <param name="vertex">The vertex identifier to check.</param>
    /// <returns><c>true</c> if the vertex exists; otherwise, <c>false</c>.</returns>
    public bool ContainsVertex(int vertex)
    {
        return _adjacencyList.ContainsKey(vertex);
    }

    /// <summary>
    /// Retrieves the adjacent neighbors of a given vertex.
    /// </summary>
    /// <param name="vertex">The vertex identifier.</param>
    /// <returns>A read-only collection of neighbor vertex identifiers.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the specified vertex does not exist in the graph.</exception>
    public IReadOnlyList<int> GetNeighbors(int vertex)
    {
        if (!_adjacencyList.TryGetValue(vertex, out var neighbors))
        {
            throw new KeyNotFoundException($"Vertex {vertex} does not exist in the graph.");
        }

        return neighbors.AsReadOnly();
    }

    /// <summary>
    /// Performs a recursive Depth-First Search traversal starting from the specified vertex.
    /// </summary>
    /// <param name="startVertex">The vertex where traversal should begin.</param>
    /// <returns>A list of vertices in the order they were visited.</returns>
    /// <exception cref="ArgumentException">Thrown when the start vertex does not exist in the graph.</exception>
    public List<int> DFSRecursive(int startVertex)
    {
        if (!_adjacencyList.ContainsKey(startVertex))
        {
            throw new ArgumentException($"Start vertex {startVertex} does not exist in the graph.", nameof(startVertex));
        }

        var visited = new HashSet<int>();
        var result = new List<int>();

        DFSRecursiveHelper(startVertex, visited, result);

        return result;
    }

    /// <summary>
    /// Performs a recursive Depth-First Search traversal across all connected components of the graph.
    /// </summary>
    /// <returns>A list of all vertices visited in DFS order across all components.</returns>
    public List<int> DFSRecursiveAll()
    {
        var visited = new HashSet<int>();
        var result = new List<int>();

        foreach (var vertex in _adjacencyList.Keys)
        {
            if (!visited.Contains(vertex))
            {
                DFSRecursiveHelper(vertex, visited, result);
            }
        }

        return result;
    }

    /// <summary>
    /// Helper method for recursive DFS traversal.
    /// </summary>
    /// <param name="currentVertex">The current vertex being visited.</param>
    /// <param name="visited">Set tracking all visited vertices.</param>
    /// <param name="result">List accumulating vertices in visit order.</param>
    private void DFSRecursiveHelper(int currentVertex, HashSet<int> visited, List<int> result)
    {
        visited.Add(currentVertex);
        result.Add(currentVertex);

        if (_adjacencyList.TryGetValue(currentVertex, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    DFSRecursiveHelper(neighbor, visited, result);
                }
            }
        }
    }
}