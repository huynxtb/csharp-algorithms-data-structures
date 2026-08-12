using System;
using System.Collections.Generic;

public class DinicMaxFlow
{
    private class Edge
    { 
        public int To { get; }
        public int Rev { get; }
        public long Capacity { get; }
        public long Flow { get; set; }

        public Edge(int to, int rev, long capacity)
        { 
            To = to;
            Rev = rev;
            Capacity = capacity;
            Flow = 0;
        }
    }

    private readonly List<Edge>[] adj;
    private readonly int[] level;
    private readonly int[] ptr;

    public DinicMaxFlow(int vertices)
    {
        if (vertices < 0) throw new ArgumentOutOfRangeException(nameof(vertices));
        adj = new List<Edge>[vertices];
        for (int i = 0; i < vertices; i++)
        {
            adj[i] = new List<Edge>();
        }
        level = new int[vertices];
        ptr = new int[vertices];
    }

    public void AddEdge(int from, int to, long capacity)
    {
        if (from < 0 || from >= adj.Length) throw new ArgumentOutOfRangeException(nameof(from));
        if (to < 0 || to >= adj.Length) throw new ArgumentOutOfRangeException(nameof(to));
        if (capacity < 0) throw new ArgumentException("Capacity cannot be negative.", nameof(capacity));

        Edge forward = new Edge(to, adj[to].Count, capacity);
        Edge backward = new Edge(from, adj[from].Count, 0);
        adj[from].Add(forward);
        adj[to].Add(backward);
    }

    public long ComputeMaxFlow(int source, int sink)
    {
        if (source < 0 || source >= adj.Length) throw new ArgumentOutOfRangeException(nameof(source));
        if (sink < 0 || sink >= adj.Length) throw new ArgumentOutOfRangeException(nameof(sink));

        long totalFlow = 0;
        while (BFS(source, sink))
        {
            Array.Fill(ptr, 0);
            while (true)
            { 
                long pushed = DFS(source, sink, long.MaxValue);
                if (pushed == 0) break;
                totalFlow += pushed;
            }
        }
        return totalFlow;
    }

    private bool BFS(int source, int sink)
    {
        Array.Fill(level, -1);
        level[source] = 0;
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(source);

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();
            foreach (var edge in adj[u])
            {
                if (edge.Capacity - edge.Flow > 0 && level[edge.To] == -1)
                { 
                    level[edge.To] = level[u] + 1;
                    queue.Enqueue(edge.To);
                }
            }
        }
        return level[sink] != -1;
    }

    private long DFS(int u, int sink, long pushed)
    {
        if (pushed == 0) return 0;
        if (u == sink) return pushed;

        for (int i = ptr[u]; i < adj[u].Count; i++)
        {
            ptr[u] = i;
            Edge edge = adj[u][i];
            int v = edge.To;

            if (level[u] + 1 != level[v] || edge.Capacity - edge.Flow == 0)
                continue;

            long tr = DFS(v, sink, Math.Min(pushed, edge.Capacity - edge.Flow));
            if (tr == 0) continue;

            edge.Flow += tr;
            adj[v][edge.Rev].Flow -= tr;
            return tr;
        }
        return 0;
    }
}