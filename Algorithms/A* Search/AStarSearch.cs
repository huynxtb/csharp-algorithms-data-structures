using System;
using System.Collections.Generic;

public class Node<T> where T : IEquatable<T>
{
    public T Id { get; }

    public Node(T id)
    {
        Id = id;
    }

    public override bool Equals(object obj)
    {
        if (obj is Node<T> other)
            return Id.Equals(other.Id);
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}

public class Edge<T> where T : IEquatable<T>
{
    public Node<T> Target { get; }
    public double Weight { get; }

    public Edge(Node<T> target, double weight)
    {
        Target = target;
        Weight = weight;
    }
}

public class Graph<T> where T : IEquatable<T>
{
    private readonly Dictionary<Node<T>, List<Edge<T>>> _adjacencyList = new();

    public void AddNode(Node<T> node)
    {
        if (!_adjacencyList.ContainsKey(node))
            _adjacencyList[node] = new List<Edge<T>>();
    }

    public void AddEdge(Node<T> from, Node<T> to, double weight)
    {
        AddNode(from);
        AddNode(to);
        _adjacencyList[from].Add(new Edge<T>(to, weight));
    }

    public IReadOnlyDictionary<Node<T>, List<Edge<T>>> AdjacencyList => _adjacencyList;
}

internal class PriorityQueue<TKey, TValue> where TKey : IComparable<TKey>
{
    private List<(TKey Key, TValue Value)> _heap = new();

    public int Count => _heap.Count;

    public void Enqueue(TKey key, TValue value)
    {
        _heap.Add((key, value));
        HeapifyUp(_heap.Count - 1);
    }

    public TValue Dequeue()
    {
        if (_heap.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");

        var result = _heap[0].Value;
        _heap[0] = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        if (_heap.Count > 0)
            HeapifyDown(0);
        return result;
    }

    public bool IsEmpty => _heap.Count == 0;

    private void HeapifyUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (_heap[i].Key.CompareTo(_heap[parent].Key) >= 0)
                break;

            Swap(i, parent);
            i = parent;
        }
    }

    private void HeapifyDown(int i)
    {
        int lastIndex = _heap.Count - 1;
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left <= lastIndex && _heap[left].Key.CompareTo(_heap[smallest].Key) < 0)
                smallest = left;
            if (right <= lastIndex && _heap[right].Key.CompareTo(_heap[smallest].Key) < 0)
                smallest = right;

            if (smallest == i)
                break;

            Swap(i, smallest);
            i = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        var temp = _heap[i];
        _heap[i] = _heap[j];
        _heap[j] = temp;
    }
}

public static class AStarSearch
{
    public static List<Node<T>> FindPath<T>(
        Graph<T> graph,
        Node<T> start,
        Node<T> goal,
        Func<Node<T>, Node<T>, double> heuristic
    ) where T : IEquatable<T>
    {
        var openSet = new PriorityQueue<double, Node<T>>();
        var cameFrom = new Dictionary<Node<T>, Node<T>>();

        var gScore = new Dictionary<Node<T>, double>();
        foreach (var node in graph.AdjacencyList.Keys)
            gScore[node] = double.PositiveInfinity;
        gScore[start] = 0;

        var fScore = new Dictionary<Node<T>, double>();
        foreach (var node in graph.AdjacencyList.Keys)
            fScore[node] = double.PositiveInfinity;
        fScore[start] = heuristic(start, goal);

        openSet.Enqueue(fScore[start], start);

        while (!openSet.IsEmpty)
        {
            var current = openSet.Dequeue();

            if (current.Equals(goal))
                return ReconstructPath(cameFrom, current);

            if (!graph.AdjacencyList.TryGetValue(current, out var edges))
                continue;

            foreach (var edge in edges)
            {
                double tentativeGScore = gScore[current] + edge.Weight;
                if (tentativeGScore < gScore.GetValueOrDefault(edge.Target, double.PositiveInfinity))
                {
                    cameFrom[edge.Target] = current;
                    gScore[edge.Target] = tentativeGScore;
                    fScore[edge.Target] = tentativeGScore + heuristic(edge.Target, goal);
                    openSet.Enqueue(fScore[edge.Target], edge.Target);
                }
            }
        }

        return new List<Node<T>>();
    }

    private static List<Node<T>> ReconstructPath<T>(Dictionary<Node<T>, Node<T>> cameFrom, Node<T> current) where T : IEquatable<T>
    {
        var totalPath = new List<Node<T>> { current };
        while (cameFrom.TryGetValue(current, out var prev))
        {
            current = prev;
            totalPath.Insert(0, current);
        }
        return totalPath;
    }
}