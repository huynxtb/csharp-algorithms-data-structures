using System;
using System.Collections.Generic;

public class Edge
{
    public int Source { get; }
    public int Destination { get; }
    public int Weight { get; }

    public Edge(int source, int destination, int weight)
    {
        Source = source;
        Destination = destination;
        Weight = weight;
    }
}

public class NegativeWeightCycleException : Exception
{
    public NegativeWeightCycleException()
        : base("Graph contains a negative weight cycle reachable from the source.")
    {
    }
}

public class BellmanFord
{
    /// <summary>
    /// Executes the Bellman-Ford algorithm to find shortest paths from the source vertex to all other vertices.
    /// </summary>
    /// <param name="vertexCount">Number of vertices in the graph</param>
    /// <param name="edges">List of edges representing the graph</param>
    /// <param name="source">The source vertex index</param>
    /// <returns>Array of shortest distances from source to each vertex</returns>
    /// <exception cref="NegativeWeightCycleException">Thrown if a negative weight cycle is detected reachable from the source</exception>
    public int[] FindShortestPaths(int vertexCount, List<Edge> edges, int source)
    {
        int[] distances = new int[vertexCount];

        // Initialize distances
        for (int i = 0; i < vertexCount; i++)
        {
            distances[i] = int.MaxValue;
        }
        distances[source] = 0;

        // Relax edges repeatedly
        for (int i = 1; i < vertexCount; i++)
        {
            bool updated = false;
            foreach (Edge edge in edges)
            {
                if (distances[edge.Source] != int.MaxValue &&
                    distances[edge.Source] + edge.Weight < distances[edge.Destination])
                {
                    distances[edge.Destination] = distances[edge.Source] + edge.Weight;
                    updated = true;
                }
            }
            // Optimization: stop if no update in this iteration
            if (!updated)
            {
                break;
            }
        }

        // Check for negative weight cycles
        foreach (Edge edge in edges)
        {
            if (distances[edge.Source] != int.MaxValue &&
                distances[edge.Source] + edge.Weight < distances[edge.Destination])
            {
                throw new NegativeWeightCycleException();
            }
        }

        return distances;
    }
}