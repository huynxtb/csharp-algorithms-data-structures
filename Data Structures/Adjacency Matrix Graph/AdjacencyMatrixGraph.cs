using System;
using System.Collections.Generic;

public class AdjacencyMatrixGraph<TVertex>
{
    private readonly bool[,] _adjacencyMatrix;
    private readonly Dictionary<TVertex, int> _vertexToIndex;
    private readonly TVertex[] _indexToVertex;
    private int _vertexCount;

    public int VertexCount => _vertexCount;
    public int Capacity => _indexToVertex.Length;

    public AdjacencyMatrixGraph(int capacity)
    {
        if (capacity <= 0)
        { 
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        }

        _adjacencyMatrix = new bool[capacity, capacity];
        _vertexToIndex = new Dictionary<TVertex, int>();
        _indexToVertex = new TVertex[capacity];
        _vertexCount = 0;
    }

    public void AddVertex(TVertex vertex)
    {
        if (vertex == null)
        { 
            throw new ArgumentException("Vertex cannot be null.", nameof(vertex));
        }

        if (_vertexCount >= Capacity)
        { 
            throw new InvalidOperationException("Graph has reached its capacity.");
        }

        if (_vertexToIndex.ContainsKey(vertex))
        { 
            throw new ArgumentException("Vertex already exists in the graph.", nameof(vertex));
        }

        int index = _vertexCount;
        _vertexToIndex[vertex] = index;
        _indexToVertex[index] = vertex;
        _vertexCount++;
    }

    public void AddEdge(TVertex vertex1, TVertex vertex2)
    {
        if (vertex1 == null) throw new ArgumentException("Vertex1 cannot be null.", nameof(vertex1));
        if (vertex2 == null) throw new ArgumentException("Vertex2 cannot be null.", nameof(vertex2));

        if (vertex1.Equals(vertex2))
        { 
            throw new ArgumentException("Self-loops are not allowed.");
        }

        if (!_vertexToIndex.TryGetValue(vertex1, out int index1))
        { 
            throw new ArgumentException("Vertex1 does not exist in the graph.", nameof(vertex1));
        }

        if (!_vertexToIndex.TryGetValue(vertex2, out int index2))
        { 
            throw new ArgumentException("Vertex2 does not exist in the graph.", nameof(vertex2));
        }

        _adjacencyMatrix[index1, index2] = true;
        _adjacencyMatrix[index2, index1] = true;
    }

    public void RemoveEdge(TVertex vertex1, TVertex vertex2)
    {
        if (vertex1 == null) throw new ArgumentException("Vertex1 cannot be null.", nameof(vertex1));
        if (vertex2 == null) throw new ArgumentException("Vertex2 cannot be null.", nameof(vertex2));

        if (!_vertexToIndex.TryGetValue(vertex1, out int index1))
        { 
            throw new ArgumentException("Vertex1 does not exist in the graph.", nameof(vertex1));
        }

        if (!_vertexToIndex.TryGetValue(vertex2, out int index2))
        { 
            throw new ArgumentException("Vertex2 does not exist in the graph.", nameof(vertex2));
        }

        _adjacencyMatrix[index1, index2] = false;
        _adjacencyMatrix[index2, index1] = false;
    }

    public bool HasEdge(TVertex vertex1, TVertex vertex2)
    {
        if (vertex1 == null) throw new ArgumentException("Vertex1 cannot be null.", nameof(vertex1));
        if (vertex2 == null) throw new ArgumentException("Vertex2 cannot be null.", nameof(vertex2));

        if (!_vertexToIndex.TryGetValue(vertex1, out int index1))
        { 
            throw new ArgumentException("Vertex1 does not exist in the graph.", nameof(vertex1));
        }

        if (!_vertexToIndex.TryGetValue(vertex2, out int index2))
        { 
            throw new ArgumentException("Vertex2 does not exist in the graph.", nameof(vertex2));
        }

        return _adjacencyMatrix[index1, index2];
    }

    public IEnumerable<TVertex> GetNeighbors(TVertex vertex)
    {
        if (vertex == null) throw new ArgumentException("Vertex cannot be null.", nameof(vertex));

        if (!_vertexToIndex.TryGetValue(vertex, out int index))
        { 
            throw new ArgumentException("Vertex does not exist in the graph.", nameof(vertex));
        }

        for (int i = 0; i < _vertexCount; i++)
        { 
            if (_adjacencyMatrix[index, i])
            { 
                yield return _indexToVertex[i];
            }
        }
    }

    public bool ContainsVertex(TVertex vertex)
    {
        if (vertex == null) throw new ArgumentException("Vertex cannot be null.", nameof(vertex));
        return _vertexToIndex.ContainsKey(vertex);
    }
}