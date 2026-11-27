using System;
using System.Collections.Generic;

public class DAG
{
    private Dictionary<int, List<int>> adjacencyList;

    public DAG()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }

    public void AddEdge(int from, int to)
    {
        if (!adjacencyList.ContainsKey(from))
        {
            adjacencyList[from] = new List<int>();
        }
        adjacencyList[from].Add(to);
    }

    public List<int> DFS(int startVertex)
    {
        HashSet<int> visited = new HashSet<int>();
        List<int> result = new List<int>();
        DFSUtil(startVertex, visited, result);
        return result;
    }

    private void DFSUtil(int vertex, HashSet<int> visited, List<int> result)
    {
        visited.Add(vertex);
        result.Add(vertex);

        if (adjacencyList.ContainsKey(vertex))
        {
            foreach (int neighbor in adjacencyList[vertex])
            {
                if (!visited.Contains(neighbor))
                {
                    DFSUtil(neighbor, visited, result);
                }
            }
        }
    }
}