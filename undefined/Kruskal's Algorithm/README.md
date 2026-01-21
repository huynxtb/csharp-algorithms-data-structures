# Kruskal's Algorithm

## 1. Introduction
Kruskal's Algorithm is a classic greedy approach to find the Minimum Spanning Tree (MST) of a connected, undirected, weighted graph. An MST is a subset of edges that connects all vertices in the graph with the minimum possible total edge weight, without any cycles. This algorithm is essential in network design, circuit design, clustering, and other fields where the goal is to connect components with minimal cost.

## 2. Usage
The `KruskalMST` class encapsulates this algorithm in a reusable, self-contained implementation. The primary usage steps are:

- Instantiate `KruskalMST` with the number of vertices.
- Initialize the graph edges using `SetEdges()`.
- Compute the MST using `BuildMST()`.
- Retrieve the MST edges with `GetMSTEdges()`.

Example:

int vertices = 5;
var edges = new List<Edge> {
    new Edge(0, 1, 10),
    new Edge(0, 2, 6),
    new Edge(0, 3, 5),
    new Edge(1, 3, 15),
    new Edge(2, 3, 4),
};

var kruskal = new KruskalMST(vertices);
kruskal.SetEdges(edges);
kruskal.BuildMST();
var mst = kruskal.GetMSTEdges();

## 3. Detailed Explanation
- **Edge Struct:** Represents an undirected edge with two endpoint vertices and a weight. Implements `IComparable` to enable sorting by weight.

- **Union-Find:** Uses a Disjoint Set Union internal structure:
  - Each vertex is initially in its own set.
  - `Find` with path compression for efficiency.
  - `Union` by rank to keep trees shallow.

- **Algorithm:**
  1. Sort edges ascending by weight.
  2. Add edges if they don't create cycles (checked by union-find).
  3. Stop when MST has `vertexCount - 1` edges.

## 4. Complexity
- Time: O(E log E) due to sorting edges.
- Space: O(V + E) for union-find and edges storage.

This approach is efficient for sparse to moderately dense graphs.

---
This implementation is designed for easy integration in C# projects needing MST computations.