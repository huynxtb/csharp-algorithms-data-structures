using System;

/// <summary>
/// Provides an implementation of the Floyd-Warshall algorithm to find shortest paths between all pairs of vertices in a weighted directed graph.
/// </summary>
public static class FloydWarshall
{
    /// <summary>
    /// Computes the shortest paths between all pairs of vertices using the Floyd-Warshall algorithm.
    /// </summary>
    /// <param name="graph">Adjacency matrix representing the weighted directed graph.
    /// graph[i][j] holds the weight of edge from vertex i to j.
    /// Use int.MaxValue to represent no direct edge.</param>
    /// <returns>A matrix of shortest distances between every pair of vertices.
    /// distance[i][j] gives the shortest distance from i to j.
    /// If no path exists, distance[i][j] will be int.MaxValue.</returns>
    /// <remarks>
    /// The input matrix is not modified; a new matrix is returned.
    /// The graph must not contain negative weight cycles.
    /// </remarks>
    public static int[][] FindShortestPaths(int[][] graph)
    {
        int n = graph.Length;
        // Initialize the distance matrix with input graph values (deep copy)
        int[][] dist = new int[n][];
        for (int i = 0; i < n; i++)
        {
            dist[i] = new int[n];
            for (int j = 0; j < n; j++)
            {
                dist[i][j] = graph[i][j];
            }
        }

        // Distance from a vertex to itself should be zero
        for (int i = 0; i < n; i++)
        {
            dist[i][i] = 0;
        }

        // Floyd-Warshall main iteration
        // Consider each vertex as an intermediate point and update distances
        for (int k = 0; k < n; k++)
        {
            for (int i = 0; i < n; i++)
            {
                // Skip if no path from i to k
                if (dist[i][k] == int.MaxValue) continue;

                for (int j = 0; j < n; j++)
                {
                    // Skip if no path from k to j
                    if (dist[k][j] == int.MaxValue) continue;

                    // Check if path i->k->j is shorter than current i->j
                    long newDist = (long)dist[i][k] + dist[k][j];
                    if (newDist < dist[i][j])
                    {
                        dist[i][j] = (int)newDist;
                    }
                }
            }
        }

        return dist;
    }

    /// <summary>
    /// Checks if the graph contains any negative weight cycle using the shortest path matrix.
    /// </summary>
    /// <param name="distance">The matrix returned by the Floyd-Warshall algorithm.</param>
    /// <returns>True if there is a negative weight cycle, otherwise false.</returns>
    public static bool HasNegativeCycle(int[][] distance)
    {
        int n = distance.Length;
        // A negative weight cycle exists if distance[i][i] < 0 for any vertex i
        for (int i = 0; i < n; i++)
        {
            if (distance[i][i] < 0)
            {
                return true;
            }
        }
        return false;
    }
}
