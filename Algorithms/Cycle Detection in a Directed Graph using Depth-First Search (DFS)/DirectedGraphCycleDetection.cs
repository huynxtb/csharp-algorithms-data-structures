using System.Collections.Generic;

public class DirectedGraph
{
    private readonly Dictionary<int, List<int>> adjacencyList;

    public DirectedGraph()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }

    // Adds a directed edge from source to destination
    public void AddEdge(int source, int destination)
    {
        if (!adjacencyList.ContainsKey(source))
        {
            adjacencyList[source] = new List<int>();
        }
        adjacencyList[source].Add(destination);

        // Ensure the destination node is also in the adjacency list, even if it has no out edges
        if (!adjacencyList.ContainsKey(destination))
        {
            adjacencyList[destination] = new List<int>();
        }
    }

    // Checks if the graph contains a cycle using DFS
    public bool ContainsCycle()
    {
        var visited = new HashSet<int>();
        var recursionStack = new HashSet<int>();

        foreach (var node in adjacencyList.Keys)
        {
            if (!visited.Contains(node))
            {
                if (DetectCycleDFS(node, visited, recursionStack))
                {
                    return true; // Cycle found
                }
            }
        }

        return false; // No cycle found
    }

    private bool DetectCycleDFS(int node, HashSet<int> visited, HashSet<int> recursionStack)
    {
        visited.Add(node);
        recursionStack.Add(node);

        foreach (var neighbor in adjacencyList[node])
        {
            if (!visited.Contains(neighbor))
            {
                if (DetectCycleDFS(neighbor, visited, recursionStack))
                {
                    return true;
                }
            }
            else if (recursionStack.Contains(neighbor))
            {
                // Found a back edge indicating a cycle
                return true;
            }
        }

        recursionStack.Remove(node);
        return false;
    }
}