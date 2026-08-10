using System;
using System.Collections.Generic;
using System.Linq;

public class FlowNetwork
{
    private readonly Dictionary<int, Dictionary<int, int>> _adjacencyList;
    private readonly HashSet<int> _vertices;

    public FlowNetwork()
    {
        _adjacencyList = new Dictionary<int, Dictionary<int, int>>();
        _vertices = new HashSet<int>();
    }

    public void AddVertex(int vertexId)
    {
        if (!_vertices.Contains(vertexId))
        {
            _vertices.Add(vertexId);
            _adjacencyList[vertexId] = new Dictionary<int, int>();
        }
    }

    public void AddEdge(int fromVertexId, int toVertexId, int capacity)
    {
        if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
        if (!_vertices.Contains(fromVertexId)) AddVertex(fromVertexId);
        if (!_vertices.Contains(toVertexId)) AddVertex(toVertexId);

        if (_adjacencyList[fromVertexId].ContainsKey(toVertexId))
        {
            _adjacencyList[fromVertexId][toVertexId] += capacity;
        }
        else
        {
            _adjacencyList[fromVertexId][toVertexId] = capacity;
        }
    }

    public IEnumerable<int> GetVertices() => _vertices;

    public Dictionary<int, int> GetNeighbors(int vertexId)
    {
        if (!_adjacencyList.ContainsKey(vertexId)) return new Dictionary<int, int>();
        return _adjacencyList[vertexId];
    }

    public int GetCapacity(int fromVertexId, int toVertexId)
    {
        if (_adjacencyList.ContainsKey(fromVertexId) && _adjacencyList[fromVertexId].ContainsKey(toVertexId))
        {
            return _adjacencyList[fromVertexId][toVertexId];
        }
        return 0;
    }

    public void UpdateCapacity(int fromVertexId, int toVertexId, int newCapacity)
    {
        if (!_adjacencyList.ContainsKey(fromVertexId))
        {
            _adjacencyList[fromVertexId] = new Dictionary<int, int>();
        }
        _adjacencyList[fromVertexId][toVertexId] = newCapacity;
    }
}

public static class EdmondsKarpSolver
{
    public static int ComputeMaxFlow(FlowNetwork network, int sourceId, int sinkId)
    {
        if (network == null) throw new ArgumentNullException(nameof(network));
        if (!network.GetVertices().Contains(sourceId)) throw new ArgumentException("Source vertex not found in network.", nameof(sourceId));
        if (!network.GetVertices().Contains(sinkId)) throw new ArgumentException("Sink vertex not found in network.", nameof(sinkId));
        if (sourceId == sinkId) return 0;

        var residualNetwork = CreateResidualNetwork(network);
        int maxFlow = 0;

        while (true)
        {
            var (pathExists, parentMap) = FindAugmentingPath(residualNetwork, sourceId, sinkId);
            if (!pathExists)
            {
                break;
            }

            int pathFlow = int.MaxValue;
            int currentNode = sinkId;
            while (currentNode != sourceId)
            {
                int prevNode = parentMap[currentNode];
                pathFlow = Math.Min(pathFlow, residualNetwork.GetCapacity(prevNode, currentNode));
                currentNode = prevNode;
            }

            maxFlow += pathFlow;

            currentNode = sinkId;
            while (currentNode != sourceId)
            {
                int prevNode = parentMap[currentNode];
                residualNetwork.UpdateCapacity(prevNode, currentNode, residualNetwork.GetCapacity(prevNode, currentNode) - pathFlow);
                residualNetwork.UpdateCapacity(currentNode, prevNode, residualNetwork.GetCapacity(currentNode, prevNode) + pathFlow);
                currentNode = prevNode;
            }
        }

        return maxFlow;
    }

    private static FlowNetwork CreateResidualNetwork(FlowNetwork originalNetwork)
    {
        var residualNetwork = new FlowNetwork();
        foreach (var vertex in originalNetwork.GetVertices())
        {
            residualNetwork.AddVertex(vertex);
        }

        foreach (var u in originalNetwork.GetVertices())
        {
            foreach (var edge in originalNetwork.GetNeighbors(u))
            {
                int v = edge.Key;
                int capacity = edge.Value;
                residualNetwork.AddEdge(u, v, capacity);
                residualNetwork.AddEdge(v, u, 0); // Add reverse edge with 0 capacity initially
            }
        }
        return residualNetwork;
    }

    private static (bool, Dictionary<int, int>) FindAugmentingPath(FlowNetwork residualNetwork, int sourceId, int sinkId)
    {
        var parentMap = new Dictionary<int, int>();
        var visited = new HashSet<int>();
        var queue = new Queue<int>();

        queue.Enqueue(sourceId);
        visited.Add(sourceId);
        parentMap[sourceId] = -1; // Sentinel value for source

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();

            foreach (var edge in residualNetwork.GetNeighbors(u))
            {
                int v = edge.Key;
                int capacity = edge.Value;

                if (!visited.Contains(v) && capacity > 0)
                {
                    queue.Enqueue(v);
                    visited.Add(v);
                    parentMap[v] = u;
                    if (v == sinkId) return (true, parentMap);
                }
            }
        }

        return (false, parentMap);
    }
}