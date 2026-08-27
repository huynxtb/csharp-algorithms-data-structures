using System;
using System.Collections.Generic;

public static class GraphDfs
{
    public static List<int> Traverse(int startNode, Dictionary<int, List<int>> adjacencyList)
    {
        if (adjacencyList == null)
        {
            throw new ArgumentNullException(nameof(adjacencyList));
        }

        var visitedOrder = new List<int>();
        var visited = new HashSet<int>();
        var stack = new Stack<int>();

        if (!adjacencyList.ContainsKey(startNode))
        {
            return visitedOrder;
        }

        stack.Push(startNode);

        while (stack.Count > 0)
        {
            int currentNode = stack.Pop();

            if (!visited.Contains(currentNode))
            { 
                visited.Add(currentNode);
                visitedOrder.Add(currentNode);

                if (adjacencyList.TryGetValue(currentNode, out var neighbors) && neighbors != null)
                {
                    for (int i = neighbors.Count - 1; i >= 0; i--)
                    {
                        int neighbor = neighbors[i];
                        if (!visited.Contains(neighbor))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }
        }

        return visitedOrder;
    }
}