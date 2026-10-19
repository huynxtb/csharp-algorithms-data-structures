using System;
using System.Collections.Generic;

public class Graph
{
    private Dictionary<int, List<int>> adjacencyList;

    public Graph()
    {
        adjacencyList = new Dictionary<int, List<int>>();
    }

    public void AddEdge(int source, int destination, bool isDirected = false)
    {
        if (!adjacencyList.ContainsKey(source))
        {
            adjacencyList[source] = new List<int>();
        }
        adjacencyList[source].Add(destination);

        if (!isDirected)
        {
            if (!adjacencyList.ContainsKey(destination))
            {
                adjacencyList[destination] = new List<int>();
            }
            adjacencyList[destination].Add(source);
        }
    }

    public void DFSIterative(int start)
    {
        HashSet<int> visited = new HashSet<int>();
        Stack<int> stack = new Stack<int>();

        stack.Push(start);

        while (stack.Count > 0)
        {
            int node = stack.Pop();
            if (!visited.Contains(node))
            {
                visited.Add(node);
                Console.WriteLine(node);

                if (adjacencyList.ContainsKey(node))
                {
                    foreach (var neighbor in adjacencyList[node])
                    {
                        if (!visited.Contains(neighbor))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }
        }
    }
}