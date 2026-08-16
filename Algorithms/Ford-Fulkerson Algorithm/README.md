# Ford-Fulkerson Algorithm

### 1. Introduction
The Ford-Fulkerson algorithm computes the maximum possible flow in a flow network from a single source vertex to a single sink vertex. It is widely used in network routing, bipartite matching, and scheduling problems. This implementation uses Depth-First Search (DFS) to find augmenting paths in the residual graph.

### 2. Usage

```csharp
using System;
using FlowNetworkAlgorithm;

class Program
{
    static void Main()
    {
        // Create a flow network with 6 vertices (0 to 5)
        var network = new FlowNetwork(6);

        // Add edges with capacities
        network.AddEdge(0, 1, 16);
        network.AddEdge(0, 2, 13);
        network.AddEdge(1, 2, 10);
        network.AddEdge(1, 3, 12);
        network.AddEdge(2, 1, 4);
        network.AddEdge(2, 4, 14);
        network.AddEdge(3, 2, 9);
        network.AddEdge(3, 5, 20);
        network.AddEdge(4, 3, 7);
        network.AddEdge(4, 5, 4);

        int source = 0;
        int sink = 5;
        int maxFlow = network.ComputeMaxFlow(source, sink);

        Console.WriteLine($"The maximum possible flow is: {maxFlow}");
    }
}
```

### 3. Detailed Explanation
- **FlowNetwork**: Manages the graph structure using an adjacency list of `FlowEdge` objects. Each edge maintains a reference to its corresponding residual (reverse) edge.
- **FlowEdge**: Represents a directed edge. It tracks both the capacity and the current flow. The residual capacity is computed dynamically based on the direction of traversal.
- **DFS Augmentation**: The algorithm repeatedly performs a Depth-First Search to find a path from the source to the sink where every edge along the path has a residual capacity greater than zero. 
- **Flow Update**: Once a path is found, the bottleneck capacity (minimum residual capacity along the path) is determined. The flow is then updated along the path: added to forward edges and subtracted from backward edges.

### 4. Complexity Analysis
- **Time Complexity**: $O(E \cdot f)$, where $E$ is the number of edges and $f$ is the maximum flow of the network. In the worst case, each DFS takes $O(E)$ time and increases the flow by at least 1 unit.
- **Space Complexity**: $O(V + E)$ to store the adjacency list representation of the graph and the recursion stack for the DFS traversal, where $V$ is the number of vertices.