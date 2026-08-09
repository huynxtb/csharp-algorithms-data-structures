using System;
using System.Collections.Generic;
using System.Linq;

public class WeightedGraph
{
    private Dictionary<int, List<(int, int)>> adjacencyList;
    private int numVertices;

    public WeightedGraph(int numVertices)
    {
        this.numVertices = numVertices;
        adjacencyList = new Dictionary<int, List<(int, int)>>();
        for (int i = 0; i < numVertices; i++)
        {
            adjacencyList[i] = new List<(int, int)>();
        }
    }

    public void AddEdge(int source, int destination, int weight)
    {
        adjacencyList[source].Add((destination, weight));
    }

    public Dictionary<int, int> Dijkstra(int source)
    {
        var distances = new Dictionary<int, int>();
        for (int i = 0; i < numVertices; i++)
        {
            distances[i] = int.MaxValue;
        }
        distances[source] = 0;

        var priorityQueue = new SortedSet<(int, int)>();
        priorityQueue.Add((0, source));

        while (priorityQueue.Count > 0)
        {
            var (currentDistance, currentNode) = priorityQueue.Min;
            priorityQueue.Remove((currentDistance, currentNode));

            foreach (var (neighbor, weight) in adjacencyList[currentNode])
            {
                var distance = currentDistance + weight;
                if (distance < distances[neighbor])
                {
                    distances[neighbor] = distance;
                    priorityQueue.Add((distance, neighbor));
                }
            }
        }

        return distances;
    }
}