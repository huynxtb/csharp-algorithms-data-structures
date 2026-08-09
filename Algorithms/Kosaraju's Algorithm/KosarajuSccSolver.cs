using System;
using System.Collections.Generic;

/// <summary>
/// Provides methods to find Strongly Connected Components (SCC) in a directed graph using Kosaraju's Algorithm.
/// </summary>
public class KosarajuSccSolver
{ 
    /// <summary>
    /// Computes the Strongly Connected Components (SCCs) of a directed graph.
    /// </summary>
    /// <param name="graph">The directed graph represented as an adjacency list where indices represent vertices.</param>
    /// <returns>A list of strongly connected components, where each component is a list of vertex indices.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input graph is null.</exception>
    public List<List<int>> GetSccs(List<int>[] graph)
    {
        if (graph == null)
        {
            throw new ArgumentNullException(nameof(graph), "Graph cannot be null.");
        }

        int numVertices = graph.Length;
        List<List<int>> sccs = new List<List<int>>();
        if (numVertices == 0)
        {
            return sccs;
        }

        bool[] visited = new bool[numVertices];
        Stack<int> finishStack = new Stack<int>();

        // Pass 1: Fill vertices in stack according to their finishing times
        for (int i = 0; i < numVertices; i++)
        {
            if (!visited[i])
            {
                FillOrder(i, graph, visited, finishStack);
            }
        }

        // Transpose the graph
        List<int>[] transposedGraph = Transpose(graph);

        // Pass 2: Process all vertices in order defined by stack
        Array.Clear(visited, 0, visited.Length);
        while (finishStack.Count > 0)
        {
            int v = finishStack.Pop();
            if (!visited[v])
            {
                List<int> component = new List<int>();
                DfsTranspose(v, transposedGraph, visited, component);
                sccs.Add(component);
            }
        }

        return sccs;
    }

    private void FillOrder(int u, List<int>[] graph, bool[] visited, Stack<int> stack)
    {
        visited[u] = true;
        if (graph[u] != null)
        {
            foreach (int v in graph[u])
            {
                if (!visited[v])
                {
                    FillOrder(v, graph, visited, stack);
                }
            }
        }
        stack.Push(u);
    }

    private List<int>[] Transpose(List<int>[] graph)
    {
        int numVertices = graph.Length;
        List<int>[] transposed = new List<int>[numVertices];
        for (int i = 0; i < numVertices; i++)
        {
            transposed[i] = new List<int>();
        }

        for (int u = 0; u < numVertices; u++)
        {
            if (graph[u] != null)
            {
                foreach (int v in graph[u])
                {
                    transposed[v].Add(u);
                }
            }
        }
        return transposed;
    }

    private void DfsTranspose(int u, List<int>[] transposedGraph, bool[] visited, List<int> component)
    {
        visited[u] = true;
        component.Add(u);
        if (transposedGraph[u] != null)
        {
            foreach (int v in transposedGraph[u])
            {
                if (!visited[v])
                {
                    DfsTranspose(v, transposedGraph, visited, component);
                }
            }
        }
    }
}