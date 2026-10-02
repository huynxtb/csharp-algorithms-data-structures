using System;
using System.Collections.Generic;

/// <summary>
/// Represents a directed edge with a non-negative integer weight.
/// </summary>
public sealed class Edge
{
    public int Destination { get; }
    public int Weight { get; }

    public Edge(int destination, int weight)
    {
        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Edge weight must be non-negative.");
        }

        Destination = destination;
        Weight = weight;
    }
}

/// <summary>
/// Represents a directed weighted graph using an adjacency list representation.
/// </summary>
public class Graph
{
    private readonly Dictionary<int, List<Edge>> _adjacencyList;

    public IReadOnlyDictionary<int, List<Edge>> AdjacencyList => _adjacencyList;

    public Graph()
    {
        _adjacencyList = new Dictionary<int, List<Edge>>();
    }

    /// <summary>
    /// Adds a vertex to the graph if it does not already exist.
    /// </summary>
    /// <param name="vertex">The vertex identifier.</param>
    public void AddVertex(int vertex)
    {
        if (!_adjacencyList.ContainsKey(vertex))
        {
            _adjacencyList[vertex] = new List<Edge>();
        }
    }

    /// <summary>
    /// Adds a directed, weighted edge between the source and destination vertices.
    /// Automatically adds vertices if they do not already exist.
    /// </summary>
    /// <param name="source">The source vertex.</param>
    /// <param name="destination">The destination vertex.</param>
    /// <param name="weight">The non-negative weight of the edge.</param>
    public void AddEdge(int source, int destination, int weight)
    {
        if (weight < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Edge weight must be non-negative.");
        }

        AddVertex(source);
        AddVertex(destination);
        _adjacencyList[source].Add(new Edge(destination, weight));
    }

    /// <summary>
    /// Retrieves the outgoing edges for a given vertex.
    /// </summary>
    /// <param name="vertex">The vertex identifier.</param>
    /// <returns>A read-only list of outgoing edges.</returns>
    public IReadOnlyList<Edge> GetNeighbors(int vertex)
    {
        if (_adjacencyList.TryGetValue(vertex, out var neighbors))
        {
            return neighbors;
        }
        return Array.Empty<Edge>();
    }

    /// <summary>
    /// Checks if a vertex exists in the graph.
    /// </summary>
    /// <param name="vertex">The vertex identifier.</param>
    /// <returns>True if vertex exists; otherwise, false.</returns>
    public bool ContainsVertex(int vertex)
    {
        return _adjacencyList.ContainsKey(vertex);
    }
}

/// <summary>
/// A min-heap based priority queue implementation for Dijkstra's algorithm.
/// </summary>
/// <typeparam name="TElement">The element type stored in the queue.</typeparam>
/// <typeparam name="TPriority">The priority type implementing IComparable.</typeparam>
public class MinPriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private readonly List<(TElement Element, TPriority Priority)> _heap = new();

    public int Count => _heap.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        _heap.Add((element, priority));
        SiftUp(_heap.Count - 1);
    }

    public TElement Dequeue()
    {
        if (_heap.Count == 0)
        {
            throw new InvalidOperationException("Priority queue is empty.");
        }

        TElement rootElement = _heap[0].Element;
        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];
        _heap.RemoveAt(lastIndex);

        if (_heap.Count > 0)
        {
            SiftDown(0);
        }

        return rootElement;
    }

    private void SiftUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (_heap[index].Priority.CompareTo(_heap[parentIndex].Priority) >= 0)
            {
                break;
            }

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void SiftDown(int index)
    {
        int lastIndex = _heap.Count - 1;
        while (true)
        {
            int leftChild = (index * 2) + 1;
            int rightChild = (index * 2) + 2;
            int smallest = index;

            if (leftChild <= lastIndex && _heap[leftChild].Priority.CompareTo(_heap[smallest].Priority) < 0)
            {
                smallest = leftChild;
            }

            if (rightChild <= lastIndex && _heap[rightChild].Priority.CompareTo(_heap[smallest].Priority) < 0)
            {
                smallest = rightChild;
            }

            if (smallest == index)
            {
                break;
            }

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
    }
}

/// <summary>
/// Implements Dijkstra's Shortest Path Algorithm using a priority queue (min-heap).
/// </summary>
public class DijkstraPathFinder
{
    private readonly Graph _graph;

    public DijkstraPathFinder(Graph graph)
    {
        _graph = graph ?? throw new ArgumentNullException(nameof(graph));
    }

    /// <summary>
    /// Computes the shortest distance from the source vertex to all reachable vertices in the graph.
    /// </summary>
    /// <param name="sourceVertex">The starting vertex identifier.</param>
    /// <returns>A dictionary mapping reachable vertices to their shortest distance from source.</returns>
    public Dictionary<int, int> FindShortestDistances(int sourceVertex)
    {
        var (distances, _) = ComputeShortestPaths(sourceVertex);
        return distances;
    }

    /// <summary>
    /// Reconstructs the shortest path from source to destination as an ordered list of vertices.
    /// </summary>
    /// <param name="sourceVertex">The starting vertex identifier.</param>
    /// <param name="destinationVertex">The destination vertex identifier.</param>
    /// <returns>An ordered list of vertices from source to destination, or empty list if no path exists.</returns>
    public List<int> GetShortestPath(int sourceVertex, int destinationVertex)
    {
        if (!_graph.ContainsVertex(sourceVertex) || !_graph.ContainsVertex(destinationVertex))
        {
            return new List<int>();
        }

        if (sourceVertex == destinationVertex)
        {
            return new List<int> { sourceVertex };
        }

        var (distances, predecessors) = ComputeShortestPaths(sourceVertex);

        if (!distances.ContainsKey(destinationVertex))
        {
            // Destination is unreachable
            return new List<int>();
        }

        var path = new List<int>();
        int current = destinationVertex;

        while (current != sourceVertex)
        {
            path.Add(current);
            if (!predecessors.TryGetValue(current, out current))
            {
                return new List<int>();
            }
        }

        path.Add(sourceVertex);
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Core algorithm execution calculating shortest distances and path predecessors.
    /// </summary>
    private (Dictionary<int, int> distances, Dictionary<int, int> predecessors) ComputeShortestPaths(int sourceVertex)
    {
        var distances = new Dictionary<int, int>();
        var predecessors = new Dictionary<int, int>();
        var priorityQueue = new MinPriorityQueue<int, int>();

        if (!_graph.ContainsVertex(sourceVertex))
        {
            return (distances, predecessors);
        }

        distances[sourceVertex] = 0;
        priorityQueue.Enqueue(sourceVertex, 0);

        while (priorityQueue.Count > 0)
        {
            int currentVertex = priorityQueue.Dequeue();
            int currentDistance = distances[currentVertex];

            foreach (var edge in _graph.GetNeighbors(currentVertex))
            {
                int neighbor = edge.Destination;
                int newDistance = currentDistance + edge.Weight;

                // Edge relaxation
                if (!distances.TryGetValue(neighbor, out int knownDistance) || newDistance < knownDistance)
                {
                    distances[neighbor] = newDistance;
                    predecessors[neighbor] = currentVertex;
                    priorityQueue.Enqueue(neighbor, newDistance);
                }
            }
        }

        return (distances, predecessors);
    }
}