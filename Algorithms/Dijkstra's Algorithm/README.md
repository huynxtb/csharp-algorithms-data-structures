# Introduction
Dijkstra's algorithm is a well-known algorithm in graph theory for finding the shortest path between nodes in a graph. It works by maintaining a priority queue of nodes, where the priority of each node is its minimum distance from the source node. The algorithm repeatedly selects the node with the minimum priority and updates the distances of its neighbors.

# Usage
```csharp
var graph = new WeightedGraph(5);
graph.AddEdge(0, 1, 4);
graph.AddEdge(0, 2, 1);
graph.AddEdge(1, 3, 1);
graph.AddEdge(2, 1, 2);
graph.AddEdge(2, 3, 5);
graph.AddEdge(3, 4, 3);
var distances = graph.Dijkstra(0);
foreach (var (node, distance) in distances)
{
    Console.WriteLine($"Shortest distance from node 0 to node {node}: {distance}");
}
```

# Detailed Explanation
The implementation consists of a `WeightedGraph` class, which represents a weighted graph using an adjacency list. The `AddEdge` method is used to add edges to the graph, and the `Dijkstra` method implements Dijkstra's algorithm to find the shortest path from a given source node to all other nodes. The algorithm uses a priority queue to efficiently select the next node to visit.

# Complexity Analysis
* Time complexity: O((V + E) log V), where V is the number of vertices and E is the number of edges, since we use a priority queue to select the next node to visit.
* Space complexity: O(V + E), since we need to store the adjacency list and the priority queue.