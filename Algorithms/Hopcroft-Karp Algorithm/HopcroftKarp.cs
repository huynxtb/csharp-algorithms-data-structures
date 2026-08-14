using System;
using System.Collections.Generic;

public class HopcroftKarp
{
    private readonly int[][] graph;
    private readonly int[] pair_U, pair_V, dist;
    private readonly int n, m;

    /// <summary>
    /// Initializes a new instance of the <see cref="HopcroftKarp"/> class.
    /// </summary>
    /// <param name="graph">The bipartite graph structure.</param>
    public HopcroftKarp(int[][] graph)
    {
        this.graph = graph;
        this.n = graph.Length;
        this.m = graph[0].Length;
        this.pair_U = new int[n];
        this.pair_V = new int[m];
        this.dist = new int[n];
    }

    /// <summary>
    /// Computes the maximum matching in the bipartite graph.
    /// </summary>
    /// <returns>An array representing the matching and the total size of the maximum matching.</returns>
    public (int[], int) ComputeMaximumMatching()
    {
        int matchingSize = 0;
        for (int u = 0; u < n; u++)
        {
            Array.Fill(dist, -1);
            if (Bfs())
            {
                for (int v = 0; v < n; v++)
                {
                    if (pair_U[v] == -1 && Dfs(v))
                    {
                        matchingSize++;
                    }
                }
            }
        }

        return (pair_U, matchingSize);
    }

    private bool Bfs()
    {
        var queue = new Queue<int>();
        for (int u = 0; u < n; u++)
        {
            if (pair_U[u] == -1)
            {
                dist[u] = 0;
                queue.Enqueue(u);
            }
            else
            {
                dist[u] = -1;
            }
        }

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            if (dist[u] < n)
            {
                foreach (int v in graph[u])
                {
                    int pu = pair_V[v];
                    if (pu == -1)
                    {
                        return true;
                    }
                    else if (dist[pu] == -1)
                    {
                        dist[pu] = dist[u] + 1;
                        queue.Enqueue(pu);
                    }
                }
            }
        }

        return false;
    }

    private bool Dfs(int u)
    {
        if (u == -1)
        {
            return true;
        }

        foreach (int v in graph[u])
        {
            int pu = pair_V[v];
            if (dist[pu] == dist[u] + 1 && Dfs(pu))
            {
                pair_V[v] = u;
                pair_U[u] = v;
                return true;
            }
        }

        dist[u] = -1;
        return false;
    }
}