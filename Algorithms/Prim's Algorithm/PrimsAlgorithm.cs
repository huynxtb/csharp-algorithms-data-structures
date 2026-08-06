using System;
using System.Collections.Generic;

/// <summary>
/// Represents an edge in a weighted, undirected graph.
/// </summary>
public class Edge
{
    /// <summary>
    /// Gets the source vertex of the edge.
    /// </summary>
    public int Source { get; }

    /// <summary>
    /// Gets the destination vertex of the edge.
    /// </summary>
    public int Destination { get; }

    /// <summary>
    /// Gets the weight of the edge.
    /// </summary>
    public int Weight { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Edge"/> class.
    /// </summary>
    /// <param name="source">The source vertex.</param>
    /// <param name="destination">The destination vertex.</param>
    /// <param name="weight">The weight of the edge.</param>
    public Edge(int source, int destination, int weight)
    {
        Source = source;
        Destination = destination;
        Weight = weight;
    }

    /// <summary>
    /// Returns a string representation of the edge.
    /// </summary>
    /// <returns>A string in the format "Source --(Weight)--> Destination".</returns>
    public override string ToString()
    {
        return $"{Source} --({Weight})--> {Destination}";
    }
}

/// <summary>
/// Represents a weighted, undirected graph using an adjacency list.
/// </summary>
public class Graph
{
    /// <summary>
    /// Gets the total number of vertices in the graph.
    /// </summary>
    public int NumberOfVertices { get; }

    /// <summary>
    /// Gets the adjacency list representation of the graph.
    /// Each index corresponds to a vertex, and its value is a list of edges connected to that vertex.
    /// </summary>
    public List<Edge>[] AdjacencyList { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Graph"/> class with a specified number of vertices.
    /// </summary>
    /// <param name="numberOfVertices">The total number of vertices in the graph.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="numberOfVertices"/> is less than 0.</exception>
    public Graph(int numberOfVertices)
    {
        if (numberOfVertices < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfVertices), "Number of vertices cannot be negative.");
        }

        NumberOfVertices = numberOfVertices;
        AdjacencyList = new List<Edge>[numberOfVertices];
        for (int i = 0; i < numberOfVertices; i++)
        {
            AdjacencyList[i] = new List<Edge>();
        }
    }

    /// <summary>
    /// Adds an undirected edge to the graph.
    /// </summary>
    /// <param name="u">The first vertex of the edge.</param>
    /// <param name="v">The second vertex of the edge.</param>
    /// <param name="weight">The weight of the edge.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="u"/> or <paramref name="v"/> are out of bounds.</exception>
    public void AddEdge(int u, int v, int weight)
    {
        if (u < 0 || u >= NumberOfVertices)
        {
            throw new ArgumentOutOfRangeException(nameof(u), $"Vertex {u} is out of bounds for a graph with {NumberOfVertices} vertices.");
        }
        if (v < 0 || v >= NumberOfVertices)
        {
            throw new ArgumentOutOfRangeException(nameof(v), $"Vertex {v} is out of bounds for a graph with {NumberOfVertices} vertices.");
        }

        // For an undirected graph, add edges in both directions
        AdjacencyList[u].Add(new Edge(u, v, weight));
        AdjacencyList[v].Add(new Edge(v, u, weight));
    }
}

/// <summary>
/// Represents the result of Prim's algorithm, containing the MST edges, total weight, and connectivity status.
/// </summary>
public struct PrimsResult
{
    /// <summary>
    /// Gets the collection of edges that form the Minimum Spanning Tree.
    /// </summary>
    public IEnumerable<Edge> MstEdges { get; }

    /// <summary>
    /// Gets the total weight of the Minimum Spanning Tree.
    /// </summary>
    public long TotalWeight { get; }

    /// <summary>
    /// Gets a value indicating whether the graph is connected and an MST was successfully formed.
    /// </summary>
    public bool IsConnected { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PrimsResult"/> struct.
    /// </summary>
    /// <param name="mstEdges">The collection of edges in the MST.</param>
    /// <param name="totalWeight">The total weight of the MST.</param>
    /// <param name="isConnected">A flag indicating if the graph was connected.</param>
    public PrimsResult(IEnumerable<Edge> mstEdges, long totalWeight, bool isConnected)
    {
        MstEdges = mstEdges;
        TotalWeight = totalWeight;
        IsConnected = isConnected;
    }
}

/// <summary>
/// Implements Prim's Algorithm to find the Minimum Spanning Tree (MST) of a weighted, undirected graph.
/// </summary>
public static class PrimsAlgorithm
{
    /// <summary>
    /// Finds the Minimum Spanning Tree (MST) of a given weighted, undirected graph starting from a specified vertex.
    /// </summary>
    /// <param name="graph">The graph for which to find the MST.</param>
    /// <param name="startVertex">The starting vertex for the algorithm.</param>
    /// <returns>A <see cref="PrimsResult"/> containing the MST edges, total weight, and connectivity status.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="graph"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="startVertex"/> is out of bounds.</exception>
    public static PrimsResult FindMinimumSpanningTree(Graph graph, int startVertex)
    {
        if (graph == null)
        {
            throw new ArgumentNullException(nameof(graph), "Graph cannot be null.");
        }
        if (startVertex < 0 || startVertex >= graph.NumberOfVertices)
        {
            throw new ArgumentOutOfRangeException(nameof(startVertex), $"Start vertex {startVertex} is out of bounds for a graph with {graph.NumberOfVertices} vertices.");
        }

        int numVertices = graph.NumberOfVertices;

        // Handle trivial cases: graph with 0 or 1 vertex
        if (numVertices == 0)
        {
            return new PrimsResult(new List<Edge>(), 0, true);
        }
        if (numVertices == 1)
        {
            return new PrimsResult(new List<Edge>(), 0, true);
        }

        // PriorityQueue to store edges, ordered by weight (min-heap)
        // TElement is Edge, TPriority is int (weight)
        var minHeap = new PriorityQueue<Edge, int>();

        // Keep track of vertices already included in MST
        bool[] inMST = new bool[numVertices];

        // List to store the edges of the MST
        List<Edge> mstEdges = new List<Edge>();

        // Total weight of the MST
        long totalWeight = 0;

        // Start Prim's algorithm from the startVertex
        // Add all edges connected to the startVertex to the minHeap
        inMST[startVertex] = true;
        foreach (Edge edge in graph.AdjacencyList[startVertex])
        {
            minHeap.Enqueue(edge, edge.Weight);
        }

        // Continue until all vertices are included or minHeap is empty
        // An MST for V vertices has V-1 edges.
        while (minHeap.Count > 0 && mstEdges.Count < numVertices - 1)
        {
            // Get the edge with the minimum weight
            Edge minEdge = minHeap.Dequeue();

            int u = minEdge.Source;
            int v = minEdge.Destination;

            // If the destination vertex is already in MST, skip this edge (it forms a cycle)
            if (inMST[v])
            {
                continue;
            }

            // Add the edge to the MST
            mstEdges.Add(minEdge);
            totalWeight += minEdge.Weight;

            // Mark the destination vertex as included in MST
            inMST[v] = true;

            // Add all edges connected to the newly added vertex 'v' to the minHeap
            // Only consider edges leading to vertices not yet in MST
            foreach (Edge edge in graph.AdjacencyList[v])
            {
                if (!inMST[edge.Destination])
                {
                    minHeap.Enqueue(edge, edge.Weight);
                }
            }
        }

        // Check for disconnected graph: if not all vertices are included (and numVertices > 1)
        bool isConnected = (mstEdges.Count == numVertices - 1);
        if (numVertices > 1 && !isConnected)
        {
            // If the graph is disconnected, an MST cannot span all vertices.
            // Return an empty MST and 0 weight, indicating failure to connect all components.
            return new PrimsResult(new List<Edge>(), 0, false);
        }

        return new PrimsResult(mstEdges, totalWeight, true);
    }
}