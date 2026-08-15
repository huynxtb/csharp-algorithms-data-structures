using System;
using System.Collections.Generic;

public class EulerianPathFinder
{
    public List<int> FindEulerianPath(int verticesCount, List<Tuple<int, int>> edges)
    {
        if (verticesCount <= 0) return new List<int>();
        if (edges == null || edges.Count == 0) return new List<int>();

        int[] inDegree = new int[verticesCount];
        int[] outDegree = new int[verticesCount];
        List<int>[] adj = new List<int>[verticesCount];
        
        for (int i = 0; i < verticesCount; i++)
        {
            adj[i] = new List<int>();
        }

        foreach (var edge in edges)
        {
            int u = edge.Item1;
            int v = edge.Item2;
            if (u < 0 || u >= verticesCount || v < 0 || v >= verticesCount)
            {
                return new List<int>();
            }
            adj[u].Add(v);
            outDegree[u]++;
            inDegree[v]++;
        }

        int startVertices = 0;
        int endVertices = 0;
        int startNode = -1;

        for (int i = 0; i < verticesCount; i++)
        {
            int diff = outDegree[i] - inDegree[i];
            if (diff == 1)
            {
                startVertices++;
                startNode = i;
            }
            else if (diff == -1)
            {
                endVertices++;
            }
            else if (diff != 0)
            {
                return new List<int>();
            }
        }

        if (startVertices > 1 || endVertices > 1 || startVertices != endVertices)
        {
            return new List<int>();
        }

        if (startNode == -1)
        {
            for (int i = 0; i < verticesCount; i++)
            {
                if (outDegree[i] > 0)
                {
                    startNode = i;
                    break;
                }
            }
        }

        if (startNode == -1)
        {
            return new List<int>();
        }

        int[] outEdgeIndex = new int[verticesCount];
        Stack<int> currPath = new Stack<int>();
        List<int> circuit = new List<int>();

        currPath.Push(startNode);

        while (currPath.Count > 0)
        {
            int u = currPath.Peek();
            if (outEdgeIndex[u] < adj[u].Count)
            {
                int v = adj[u][outEdgeIndex[u]];
                outEdgeIndex[u]++;
                currPath.Push(v);
            }
            else
            {
                circuit.Add(currPath.Pop());
            }
        }

        circuit.Reverse();

        if (circuit.Count != edges.Count + 1)
        {
            return new List<int>();
        }

        return circuit;
    }
}