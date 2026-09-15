using System;
using System.Collections.Generic;

public class Edge
{
    public int To;
    public double Weight;
}

public class MinHeap
{
    private List<(double distance, int vertex)> heap;

    public int Count => heap.Count;

    public MinHeap()
    {
        heap = new List<(double distance, int vertex)>();
    }

    public void Insert((double distance, int vertex) item)
    {
        heap.Add(item);
        HeapifyUp(heap.Count - 1);
    }

    public (double distance, int vertex) ExtractMin()
    {
        if (heap.Count == 0)
        {
            throw new InvalidOperationException("Heap is empty.");
        }

        (double distance, int vertex) minItem = heap[0];
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        if (heap.Count > 0)
        {
            HeapifyDown(0);
        }

        return minItem;
    }

    private void HeapifyUp(int index)
    {
        int parentIndex = (index - 1) / 2;
        while (index > 0 && heap[index].distance < heap[parentIndex].distance)
        {
            Swap(index, parentIndex);
            index = parentIndex;
            parentIndex = (index - 1) / 2;
        }
    }

    private void HeapifyDown(int index)
    {
        int leftChildIndex = 2 * index + 1;
        int rightChildIndex = 2 * index + 2;
        int smallestIndex = index;

        if (leftChildIndex < heap.Count && heap[leftChildIndex].distance < heap[smallestIndex].distance)
        {
            smallestIndex = leftChildIndex;
        }

        if (rightChildIndex < heap.Count && heap[rightChildIndex].distance < heap[smallestIndex].distance)
        {
            smallestIndex = rightChildIndex;
        }

        if (smallestIndex != index)
        {
            Swap(index, smallestIndex);
            HeapifyDown(smallestIndex);
        }
    }

    private void Swap(int i, int j)
    {
        (double distance, int vertex) temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}

public class JohnsonAlgorithm
{
    private int V; // Number of vertices
    private List<List<Edge>> graph; // Original graph

    public JohnsonAlgorithm(int numVertices)
    {
        if (numVertices < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numVertices), "Number of vertices cannot be negative.");
        }
        V = numVertices;
        graph = new List<List<Edge>>(V);
        for (int i = 0; i < V; i++)
        {
            graph.Add(new List<Edge>());
        }
    }

    public void AddEdge(int u, int v, double weight)
    {
        if (u < 0 || u >= V || v < 0 || v >= V)
        {
            throw new ArgumentOutOfRangeException("Vertex index out of bounds. Vertices must be between 0 and V-1.");
        }
        graph[u].Add(new Edge { To = v, Weight = weight });
    }

    public double[,] ComputeAllPairsShortestPaths()
    {
        if (V == 0) return new double[0, 0];

        // Step 1: Run Bellman-Ford to compute potentials h
        double[] h = BellmanFord(graph, V);

        // If Bellman-Ford returned null, it means a negative cycle was detected.
        if (h == null)
        {
            throw new InvalidOperationException("Graph contains a negative-weight cycle reachable from the virtual source.");
        }

        // Step 2: Reweight the graph
        List<List<Edge>> reweightedGraph = new List<List<Edge>>(V);
        for (int i = 0; i < V; i++)
        {
            reweightedGraph.Add(new List<Edge>());
            foreach (var edge in graph[i])
            {
                reweightedGraph[i].Add(new Edge
                {
                    To = edge.To,
                    Weight = edge.Weight + h[i] - h[edge.To]
                });
            }
        }

        // Step 3: Run Dijkstra from each vertex on the reweighted graph
        double[,] distances = new double[V, V];

        for (int u = 0; u < V; u++)
        {
            double[] dPrime = Dijkstra(reweightedGraph, u);

            // Step 4: Convert distances back to original weights
            for (int v = 0; v < V; v++)
            {
                if (dPrime[v] == double.PositiveInfinity)
                {
                    distances[u, v] = double.PositiveInfinity;
                }
                else
                {
                    distances[u, v] = dPrime[v] - h[u] + h[v];
                }
            }
        }

        return distances;
    }

    // Bellman-Ford implementation
    private double[] BellmanFord(List<List<Edge>> originalGraph, int numVertices)
    {
        // Add a virtual source (numVertices) connected to all original vertices with weight 0
        int numNodesWithVirtualSource = numVertices + 1;
        List<List<Edge>> bfGraph = new List<List<Edge>>(numNodesWithVirtualSource);
        for (int i = 0; i < numNodesWithVirtualSource; i++)
        {
            bfGraph.Add(new List<Edge>());
        }

        // Add edges from virtual source to all original vertices
        for (int i = 0; i < numVertices; i++)
        {
            bfGraph[numVertices].Add(new Edge { To = i, Weight = 0 });
        }

        // Add original graph edges
        for (int u = 0; u < numVertices; u++)
        {
            foreach (var edge in originalGraph[u])
            {
                bfGraph[u].Add(edge);
            }
        }

        double[] dist = new double[numNodesWithVirtualSource];
        for (int i = 0; i < numNodesWithVirtualSource; i++)
        {
            dist[i] = double.PositiveInfinity;
        }
        dist[numVertices] = 0; // Distance from virtual source to itself is 0

        // Relax edges V times (numNodesWithVirtualSource - 1 iterations for shortest paths,
        // and one more iteration to detect negative cycles)
        for (int i = 0; i < numNodesWithVirtualSource; i++)
        {
            bool relaxedInThisIteration = false;
            for (int u = 0; u < numNodesWithVirtualSource; u++)
            {
                if (dist[u] == double.PositiveInfinity) continue; 

                foreach (var edge in bfGraph[u])
                {
                    if (dist[u] + edge.Weight < dist[edge.To])
                    {
                        dist[edge.To] = dist[u] + edge.Weight;
                        relaxedInThisIteration = true;
                    }
                }
            }

            // If relaxation occurs in the V-th iteration (0-indexed: i == numNodesWithVirtualSource - 1),
            // it means a negative cycle exists.
            if (i == numNodesWithVirtualSource - 1)
            {
                if (relaxedInThisIteration)
                {
                    // Negative cycle detected
                    return null;
                }
            }
            else if (!relaxedInThisIteration)
            {
                // No relaxation, paths are stable, can stop early
                break;
            }
        }

        // The potentials h are the shortest path distances from the virtual source
        // to each original vertex. We only need h for original vertices.
        double[] h = new double[numVertices];
        Array.Copy(dist, h, numVertices);
        return h;
    }

    // Dijkstra's implementation
    private double[] Dijkstra(List<List<Edge>> currentGraph, int startNode)
    {
        double[] dist = new double[V];
        for (int i = 0; i < V; i++)
        {
            dist[i] = double.PositiveInfinity;
        }
        dist[startNode] = 0;

        MinHeap pq = new MinHeap();
        pq.Insert((0, startNode)); // (distance, vertex)

        while (pq.Count > 0)
        {
            (double d, int u) = pq.ExtractMin();

            // If we've found a shorter path to u already, skip this older entry
            if (d > dist[u]) continue;

            foreach (var edge in currentGraph[u])
            {
                if (dist[u] != double.PositiveInfinity && dist[u] + edge.Weight < dist[edge.To])
                {
                    dist[edge.To] = dist[u] + edge.Weight;
                    pq.Insert((dist[edge.To], edge.To));
                }
            }
        }
        return dist;
    }
}