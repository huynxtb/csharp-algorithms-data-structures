# Push-Relabel Maximum Flow Algorithm

## 1. Introduction
The **Push-Relabel Algorithm** (also known as the **Preflow-Push Algorithm**) is an efficient method for computing the maximum flow and minimum s-t cut in a capacitated directed network. Unlike augmenting path algorithms (e.g., Ford-Fulkerson, Edmonds-Karp, Dinic) that maintain a valid flow at all intermediate steps, Push-Relabel maintains a **preflow** (where incoming flow can exceed outgoing flow at non-source nodes) and height/distance labels. Excess flow is pushed locally along admissible edges to lower neighbors until all excess reaches either the sink or is returned to the source.

Use this implementation when:
- Solving dense max-flow networks where augmenting paths may perform excessive path searches.
- Optimal performance and practical scalability are required on arbitrary graphs via the **Gap Heuristic**.
- Determining the minimum weight s-t cut alongside the maximum network throughput.

---

## 2. Usage

```csharp
using System;
using FlowAlgorithms;

public class Example
{
    public static void Run()
    {
        // Create graph with 6 vertices: 0 to 5 (0: Source, 5: Sink)
        int n = 6;
        PushRelabelMaxFlow network = new PushRelabelMaxFlow(n);

        // Add directed edges: (from, to, capacity)
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

        // Compute Maximum Flow
        long maxFlow = network.GetMaxFlow(source, sink);
        Console.WriteLine($"Maximum Flow: {maxFlow}");

        // Inspect edge flows
        foreach (Edge edge in network.GetEdges())
        {
            Console.WriteLine($"Edge ({edge.From} -> {edge.To}): Flow = {edge.Flow}/{edge.Capacity}");
        }

        // Compute Minimum Cut
        bool[] inSourceCut = network.GetMinCut(source);
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"Vertex {i} is on Source side: {inSourceCut[i]}");
        }
    }
}
```

---

## 3. Detailed Explanation

1. **Initialization (`InitializePreflow`)**:
   - Vertex heights are initialized with $h(source) = V$ and $h(v) = 0$ for all $v \neq source$.
   - Saturates all outgoing edges from the source by sending flow equal to their capacities, creating excess at the adjacent vertices.

2. **Discharge & FIFO Queue**:
   - Vertices with positive excess are placed in a FIFO queue.
   - An active vertex repeatedly executes **Push** operations to valid neighbors ($h(u) = h(v) + 1$).
   - If excess remains and no push is valid, a **Relabel** operation updates the vertex height to $1 + \min_{(u, v) \in E_f} h(v)$.

3. **Gap Heuristic**:
   - Keeps count of how many vertices reside at each height level.
   - If a height level $k < V$ becomes empty after a relabel operation, all vertices with heights $k < h(v) < V$ are unreachable from the sink in the residual graph and are lifted immediately to $V + 1$, pruning redundant push/relabel steps.

4. **Minimum Cut (`GetMinCut`)**:
   - After maximum flow is computed, a breadth-first search identifies all nodes reachable from the source through residual edges ($Capacity - Flow > 0$). These reachable nodes form partition $S$, and the remaining nodes form partition $T$.

---

## 4. Complexity Analysis

- **Time Complexity**:
  - **Generic Push-Relabel**: $\mathcal{O}(V^2 E)$.
  - **FIFO Queue Selection + Gap Heuristic**: Worst-case $\mathcal{O}(V^3)$, running significantly faster in practice (often comparable to $\mathcal{O}(V \cdot E)$ on realistic networks).
  - **GetMinCut**: $\mathcal{O}(V + E)$ using standard BFS.

- **Space Complexity**:
  - $\mathcal{O}(V + E)$ to store the adjacency lists, height/excess arrays, and residual back-edge references.