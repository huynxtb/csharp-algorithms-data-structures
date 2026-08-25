using System;
using System.Collections.Generic;

public class AdjacencyListGraph<TVertex> where TVertex : IEquatable<TVertex>
{
    private readonly Dictionary<TVertex, HashSet<TVertex>> _adjacencyList;
    private readonly bool _isDirected;
    private int _edgeCount;

    public AdjacencyListGraph(bool isDirected)
    {
        _adjacencyList = new Dictionary<TVertex, HashSet<TVertex>>();
        _isDirected = isDirected;
        _edgeCount = 0;
    }

    public int VertexCount => _adjacencyList.Count;
    public int EdgeCount => _edgeCount;

    public void AddVertex(TVertex vertex)
    {
        if (vertex == null)
            throw new ArgumentNullException(nameof(vertex));
        if (_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException("Vertex already exists in the graph.", nameof(vertex));

        _adjacencyList[vertex] = new HashSet<TVertex>();
    }

    public void RemoveVertex(TVertex vertex)
    {
        if (vertex == null)
            throw new ArgumentNullException(nameof(vertex));
        if (!_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException("Vertex not found in the graph.", nameof(vertex));

        if (_isDirected)
            {
            foreach (var kvp in _adjacencyList)
            {
                if (!kvp.Key.Equals(vertex) && kvp.Value.Remove(vertex))
                {
                    _edgeCount--;
                }
            }
            _edgeCount -= _adjacencyList[vertex].Count;
        }
        else
        {
            foreach (var neighbor in _adjacencyList[vertex])
            {
                if (!neighbor.Equals(vertex))
                {
                    _adjacencyList[neighbor].Remove(vertex);
                }
                _edgeCount--;
            }
        }

        _adjacencyList.Remove(vertex);
    }

    public void AddEdge(TVertex source, TVertex destination)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (destination == null) throw new ArgumentNullException(nameof(destination));

        if (!_adjacencyList.ContainsKey(source))
            throw new ArgumentException("Source vertex not found.", nameof(source));
        if (!_adjacencyList.ContainsKey(destination))
            throw new ArgumentException("Destination vertex not found.", nameof(destination));

        if (_adjacencyList[source].Contains(destination))
            throw new ArgumentException("Edge already exists.");

        _adjacencyList[source].Add(destination);

        if (!_isDirected)
        {
            if (!source.Equals(destination))
            {
                _adjacencyList[destination].Add(source);
            }
        }

        _edgeCount++;
    }

    public void RemoveEdge(TVertex source, TVertex destination)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (destination == null) throw new ArgumentNullException(nameof(destination));

        if (!_adjacencyList.ContainsKey(source))
            throw new ArgumentException("Source vertex not found.", nameof(source));
        if (!_adjacencyList.ContainsKey(destination))
            throw new ArgumentException("Destination vertex not found.", nameof(destination));

        if (!_adjacencyList[source].Contains(destination))
            throw new ArgumentException("Edge does not exist.");

        _adjacencyList[source].Remove(destination);

        if (!_isDirected)
        {
            if (!source.Equals(destination))
            {
                _adjacencyList[destination].Remove(source);
            }
        }

        _edgeCount--;
    }

    public bool HasVertex(TVertex vertex)
    {
        if (vertex == null) throw new ArgumentNullException(nameof(vertex));
        return _adjacencyList.ContainsKey(vertex);
    }

    public bool HasEdge(TVertex source, TVertex destination)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (destination == null) throw new ArgumentNullException(nameof(destination));

        if (!_adjacencyList.ContainsKey(source) || !_adjacencyList.ContainsKey(destination))
            return false;

        return _adjacencyList[source].Contains(destination);
    }

    public IEnumerable<TVertex> GetNeighbors(TVertex vertex)
    {
        if (vertex == null) throw new ArgumentNullException(nameof(vertex));
        if (!_adjacencyList.ContainsKey(vertex))
            throw new ArgumentException("Vertex not found.", nameof(vertex));

        return _adjacencyList[vertex];
    }

    public IEnumerable<TVertex> GetVertices()
    {
        return _adjacencyList.Keys;
    }
}