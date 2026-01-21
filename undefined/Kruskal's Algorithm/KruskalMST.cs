using System;
using System.Collections.Generic;

/// <summary>
/// Represents an undirected, weighted edge between two vertices.
/// </summary>
public struct Edge : IComparable<Edge>
{
    public int From, To;
    public int Weight;

    public Edge(int from, int to, int weight)
    {
        From = from;
        To = to;
        Weight = weight;
    }

    // Compare edges by weight to assist in sorting
    public int CompareTo(Edge other) => Weight.CompareTo(other.Weight);
}

/// <summary>
/// Implements Kruskal's Algorithm for finding a Minimum Spanning Tree (MST) of a connected, undirected, weighted graph.
/// </summary>
public class KruskalMST
{
    private int vertexCount;
    private List<Edge> edges = new List<Edge>();
    private List<Edge> mstEdges = new List<Edge>();

    // Disjoint Set Union (Union-Find) internal data structure:
    private int[] parent;
    private int[] rank;

    /// <summary>
    /// Initializes the KruskalMST instance.
    /// </summary>
    /// <param name="vertexCount">Number of vertices in the graph. Vertices are assumed 0-based indexed.</param>
    public KruskalMST(int vertexCount)
    {
        this.vertexCount = vertexCount;
        parent = new int[vertexCount];
        rank = new int[vertexCount];

        for (int i = 0; i < vertexCount; i++)
            parent[i] = i;
    }

    /// <summary>
    /// Initializes the edge list of the graph.
    /// </summary>
    /// <param name="edgeList">Collection of edges (from, to, weight).</param>
    public void SetEdges(IEnumerable<Edge> edgeList)
    {
        edges.Clear();
        edges.AddRange(edgeList);
        mstEdges.Clear();
    }

    /// <summary>
    /// Finds the representative (root) of the set that element x belongs to.
    /// Path compression is applied for improved efficiency.
    /// </summary>
    private int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]);
        return parent[x];
    }

    /// <summary>
    /// Joins two sets if they are disjoint.
    /// Union by rank heuristic is used.
    /// Returns true if union happened; false if already in same set.
    /// </summary>
    private bool Union(int x, int y)
    {
        int rootX = Find(x);
        int rootY = Find(y);
        if (rootX == rootY) return false;

        if (rank[rootX] < rank[rootY])
        {
            parent[rootX] = rootY;
        }
        else if (rank[rootX] > rank[rootY])
        {
            parent[rootY] = rootX;
        }
        else
        {
            parent[rootY] = rootX;
            rank[rootX]++;
        }
        return true;
    }

    /// <summary>
    /// Executes Kruskal's algorithm to compute the Minimum Spanning Tree.
    /// 
    /// Clears and fills MST edges if graph is connected; otherwise returns MST of the connected components.
    /// </summary>
    public void BuildMST()
    {
        mstEdges.Clear();
        edges.Sort(); // Sort edges in ascending order by weight

        foreach (var edge in edges)
        {
            if (Union(edge.From, edge.To))
            {
                mstEdges.Add(edge);
                // Early stop: If mstEdges == vertexCount - 1, MST is complete
                if (mstEdges.Count == vertexCount - 1)
                    break;
            }
        }
    }

    /// <summary>
    /// Returns the edges that comprise the Minimum Spanning Tree after BuildMST is called.
    /// </summary>
    public IReadOnlyList<Edge> GetMSTEdges() => mstEdges.AsReadOnly();
}