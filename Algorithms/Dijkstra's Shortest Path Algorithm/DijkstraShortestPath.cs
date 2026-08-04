using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents an edge in a weighted graph.
/// </summary>
/// <typeparam name="TVertex">The type of the vertex identifier.</typeparam>
public class Edge<TVertex> where TVertex : notnull, IComparable<TVertex>, IEquatable<TVertex>
{
    /// <summary>
    /// Gets the destination vertex of the edge.
    /// </summary>
    public TVertex Destination { get; }

    /// <summary>
    /// Gets the weight of the edge.
    /// </summary>
    public double Weight { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Edge{TVertex}"/> class.
    /// </summary>
    /// <param name="destination">The destination vertex.</param>
    /// <param name="weight">The weight of the edge. Must be non-negative.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the weight is negative.</exception>
    public Edge(TVertex destination, double weight)
    {
        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Edge weight cannot be negative for Dijkstra's algorithm.");
        }
        Destination = destination;
        Weight = weight;
    }
}

/// <summary>
/// Represents a weighted graph using an adjacency list.
/// </summary>
/// <typeparam name="TVertex">The type of the vertex identifier.</typeparam>
public class WeightedGraph<TVertex> where TVertex : notnull, IComparable<TVertex>, IEquatable<TVertex>
{
    /// <summary>
    /// Gets the adjacency list representation of the graph.
    /// Each key is a vertex, and its value is a list of edges originating from that vertex.
    /// </summary>
    public Dictionary<TVertex, List<Edge<TVertex>>> AdjacencyList { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeightedGraph{TVertex}"/> class.
    /// </summary>
    public WeightedGraph()
    {
        AdjacencyList = new Dictionary<TVertex, List<Edge<TVertex>>>();
    }

    /// <summary>
    /// Adds a vertex to the graph.
    /// </summary>
    /// <param name="vertex">The vertex to add.</param>
    public void AddVertex(TVertex vertex)
    {
        if (!AdjacencyList.ContainsKey(vertex))
        {
            AdjacencyList[vertex] = new List<Edge<TVertex>>();
        }
    }

    /// <summary>
    /// Adds a directed edge to the graph. If vertices do not exist, they are added.
    /// </summary>
    /// <param name="source">The source vertex of the edge.</param>
    /// <param name="destination">The destination vertex of the edge.</param>
    /// <param name="weight">The weight of the edge. Must be non-negative.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the weight is negative.</exception>
    public void AddEdge(TVertex source, TVertex destination, double weight)
    {
        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Edge weight cannot be negative for Dijkstra's algorithm.");
        }

        AddVertex(source);
        AddVertex(destination);
        AdjacencyList[source].Add(new Edge<TVertex>(destination, weight));
    }

    /// <summary>
    /// Gets the neighbors (edges) of a specified vertex.
    /// </summary>
    /// <param name="vertex">The vertex whose neighbors are to be retrieved.</param>
    /// <returns>An enumerable collection of edges originating from the specified vertex.</returns>
    public IEnumerable<Edge<TVertex>> GetNeighbors(TVertex vertex)
    {
        if (AdjacencyList.TryGetValue(vertex, out var edges))
        {
            return edges;
        }
        return Enumerable.Empty<Edge<TVertex>>();
    }
}

/// <summary>
/// Represents the result of Dijkstra's algorithm, containing shortest distances and path reconstruction capabilities.
/// </summary>
/// <typeparam name="TVertex">The type of the vertex identifier.</typeparam>
public class DijkstraResult<TVertex> where TVertex : notnull, IComparable<TVertex>, IEquatable<TVertex>
{
    /// <summary>
    /// Gets a dictionary where keys are vertices and values are their shortest distances from the source vertex.
    /// If a vertex is not reachable, its distance will be <see cref="double.PositiveInfinity"/>.
    /// </summary>
    public Dictionary<TVertex, double> ShortestDistances { get; }

    /// <summary>
    /// Gets a dictionary where keys are vertices and values are their immediate predecessors on the shortest path from the source.
    /// This is used to reconstruct the actual paths.
    /// </summary>
    internal Dictionary<TVertex, TVertex> Predecessors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DijkstraResult{TVertex}"/> class.
    /// </summary>
    /// <param name="shortestDistances">A dictionary of shortest distances.</param>
    /// <param name="predecessors">A dictionary of predecessors for path reconstruction.</param>
    internal DijkstraResult(Dictionary<TVertex, double> shortestDistances, Dictionary<TVertex, TVertex> predecessors)
    {
        ShortestDistances = shortestDistances;
        Predecessors = predecessors;
    }

    /// <summary>
    /// Reconstructs the shortest path from the source vertex to the specified target vertex.
    /// </summary>
    /// <param name="targetVertex">The target vertex for which to reconstruct the path.</param>
    /// <returns>An enumerable collection of vertices representing the shortest path from source to target,
    /// or an empty enumerable if the target is unreachable or is the source itself and no path exists (e.g., isolated).</returns>
    /// <remarks>
    /// The path is returned in order from source to target.
    /// If the target is the source, it returns a path containing only the source.
    /// If the target is unreachable, an empty enumerable is returned.
    /// </remarks>
    public IEnumerable<TVertex> GetPath(TVertex targetVertex)
    {
        var path = new Stack<TVertex>();
        TVertex current = targetVertex;

        // Check if target is reachable (has a predecessor or is the source itself with distance 0)
        if (!ShortestDistances.TryGetValue(targetVertex, out double distance) || distance == double.PositiveInfinity)
        {
            return Enumerable.Empty<TVertex>();
        }

        // Reconstruct path by backtracking from target to source
        while (Predecessors.ContainsKey(current))
        {
            path.Push(current);
            current = Predecessors[current];
        }

        // Add the source vertex to the path if it's not already added (i.e., if target was not the source itself)
        // The 'current' variable now holds the source vertex after the loop.
        // If targetVertex was the source, current would still be targetVertex, and it needs to be pushed.
        // If the path is empty (meaning target was source and no predecessors), push source.
        if (!path.Any() || !path.Peek().Equals(current))
        {
            path.Push(current);
        }

        return path.Reverse(); // Path is built from target to source, so reverse it.
    }
}

/// <summary>
/// Implements Dijkstra's algorithm for finding single-source shortest paths in a weighted graph
/// with non-negative edge weights.
/// </summary>
/// <typeparam name="TVertex">The type of the vertex identifier. Must be non-null, comparable, and equatable.</typeparam>
public class DijkstraShortestPath<TVertex> where TVertex : notnull, IComparable<TVertex>, IEquatable<TVertex>
{
    /// <summary>
    /// Finds the shortest paths from a specified source vertex to all other reachable vertices in the graph.
    /// </summary>
    /// <param name="graph">The weighted graph on which to run Dijkstra's algorithm.</param>
    /// <param name="source">The starting vertex for finding shortest paths.</param>
    /// <returns>A <see cref="DijkstraResult{TVertex}"/> object containing the shortest distances and path reconstruction information.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the graph or source vertex is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the source vertex is not present in the graph.</exception>
    public DijkstraResult<TVertex> FindShortestPaths(WeightedGraph<TVertex> graph, TVertex source)
    {
        if (graph == null)
        {
            throw new ArgumentNullException(nameof(graph), "Graph cannot be null.");
        }
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source), "Source vertex cannot be null.");
        }
        if (!graph.AdjacencyList.ContainsKey(source))
        {
            throw new ArgumentException($"Source vertex '{source}' not found in the graph.", nameof(source));
        }

        // Initialize distances: all to infinity, source to 0
        var distances = new Dictionary<TVertex, double>();
        var predecessors = new Dictionary<TVertex, TVertex>();
        var visited = new HashSet<TVertex>();

        foreach (var vertex in graph.AdjacencyList.Keys)
        {
            distances[vertex] = double.PositiveInfinity;
        }
        distances[source] = 0;

        // Priority queue to store vertices to visit, ordered by their current shortest distance
        // Element: vertex, Priority: distance
        var priorityQueue = new PriorityQueue<TVertex, double>();
        priorityQueue.Enqueue(source, 0);

        while (priorityQueue.Count > 0)
        {
            TVertex currentVertex = priorityQueue.Dequeue();

            // If we've already processed this vertex with a shorter path, skip
            // This can happen because we might enqueue the same vertex multiple times with different distances
            // but only the first (shortest) one matters.
            if (visited.Contains(currentVertex))
            {
                continue;
            }

            visited.Add(currentVertex);

            // Explore neighbors
            foreach (var edge in graph.GetNeighbors(currentVertex))
            {
                TVertex neighbor = edge.Destination;
                double weight = edge.Weight;

                double newDistance = distances[currentVertex] + weight;

                // If a shorter path to the neighbor is found
                if (newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    predecessors[neighbor] = currentVertex;
                    priorityQueue.Enqueue(neighbor, newDistance);
                }
            }
        }

        return new DijkstraResult<TVertex>(distances, predecessors);
    }
}