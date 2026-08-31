using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Graphs
{
    /// <summary>
    /// Represents a weighted, undirected edge in a graph.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices.</typeparam>
    /// <typeparam name="TWeight">The type of the edge weight.</typeparam>
    public class Edge<TVertex, TWeight> : IComparable<Edge<TVertex, TWeight>>
        where TVertex : notnull
        where TWeight : IComparable<TWeight>
    {
        /// <summary>
        /// Gets the source vertex.
        /// </summary>
        public TVertex Source { get; }

        /// <summary>
        /// Gets the target vertex.
        /// </summary>
        public TVertex Target { get; }

        /// <summary>
        /// Gets the weight of the edge.
        /// </summary>
        public TWeight Weight { get; }

        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        public Edge(TVertex source, TVertex target, TWeight weight)
        {
            Source = source;
            Target = target;
            Weight = weight;
        }

        /// <inheritdoc />
        public int CompareTo(Edge<TVertex, TWeight>? other)
        {
            if (other == null) return 1;
            return Weight.CompareTo(other.Weight);
        }
    }

    /// <summary>
    /// Represents an undirected graph with weighted edges.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices.</typeparam>
    /// <typeparam name="TWeight">The type of the edge weight.</typeparam>
    public class Graph<TVertex, TWeight>
        where TVertex : notnull
        where TWeight : IComparable<TWeight>
    {
        /// <summary>
        /// Gets the set of vertices in the graph.
        /// </summary>
        public HashSet<TVertex> Vertices { get; } = new();

        /// <summary>
        /// Gets the list of edges in the graph.
        /// </summary>
        public List<Edge<TVertex, TWeight>> Edges { get; } = new();

        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        public void AddVertex(TVertex vertex)
        {
            Vertices.Add(vertex);
        }

        /// <summary>
        /// Adds a weighted undirected edge to the graph.
        /// </summary>
        public void AddEdge(TVertex source, TVertex target, TWeight weight)
        {
            Vertices.Add(source);
            Vertices.Add(target);
            Edges.Add(new Edge<TVertex, TWeight>(source, target, weight));
        }
    }

    /// <summary>
    /// Represents the result of the Borůvka's algorithm execution.
    /// </summary>
    public class BoruvkaResult<TVertex, TWeight>
        where TVertex : notnull
        where TWeight : IComparable<TWeight>
    {
        /// <summary>
        /// Gets the edges that form the Minimum Spanning Tree.
        /// </summary>
        public List<Edge<TVertex, TWeight>> MstEdges { get; }

        /// <summary>
        /// Gets the total weight of the Minimum Spanning Tree.
        /// </summary>
        public TWeight TotalWeight { get; }

        /// <summary>
        /// Initializes a new instance of the BoruvkaResult class.
        /// </summary>
        public BoruvkaResult(List<Edge<TVertex, TWeight>> mstEdges, TWeight totalWeight)
        {
            MstEdges = mstEdges;
            TotalWeight = totalWeight;
        }
    }

    /// <summary>
    /// Provides methods to find the Minimum Spanning Tree of a graph using Borůvka's Algorithm.
    /// </summary>
    public static class BoruvkaSolver
    {
        /// <summary>
        /// Finds the Minimum Spanning Tree (MST) of a connected, weighted, undirected graph.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the graph is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the graph is disconnected.</exception>
        public static BoruvkaResult<TVertex, TWeight> FindMinSpanningTree<TVertex, TWeight>(Graph<TVertex, TWeight> graph)
            where TVertex : notnull
            where TWeight : IComparable<TWeight>
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            if (graph.Vertices.Count == 0)
            {
                return new BoruvkaResult<TVertex, TWeight>(new List<Edge<TVertex, TWeight>>(), default!);
            }

            var verticesList = graph.Vertices.ToList();
            int n = verticesList.Count;
            var vertexToIndex = new Dictionary<TVertex, int>();
            for (int i = 0; i < n; i++)
            {
                vertexToIndex[verticesList[i]] = i;
            }

            var uf = new UnionFind(n);
            var mstEdges = new List<Edge<TVertex, TWeight>>();
            var cheapest = new Edge<TVertex, TWeight>?[n];

            int numComponents = n;
            bool progress = true;

            while (numComponents > 1 && progress)
            {
                progress = false;
                Array.Clear(cheapest, 0, cheapest.Length);

                foreach (var edge in graph.Edges)
                {
                    int u = vertexToIndex[edge.Source];
                    int v = vertexToIndex[edge.Target];

                    int setU = uf.Find(u);
                    int setV = uf.Find(v);

                    if (setU == setV) continue;

                    if (cheapest[setU] == null || edge.Weight.CompareTo(cheapest[setU]!.Weight) < 0)
                    {
                        cheapest[setU] = edge;
                    }

                    if (cheapest[setV] == null || edge.Weight.CompareTo(cheapest[setV]!.Weight) < 0)
                    {
                        cheapest[setV] = edge;
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    var edge = cheapest[i];
                    if (edge != null)
                    {
                        int u = vertexToIndex[edge.Source];
                        int v = vertexToIndex[edge.Target];

                        if (uf.Union(u, v))
                        {
                            mstEdges.Add(edge);
                            numComponents--;
                            progress = true;
                        }
                    }
                }
            }

            if (numComponents > 1 && graph.Vertices.Count > 1)
            {
                throw new InvalidOperationException("Graph is disconnected; Minimum Spanning Tree cannot span all vertices.");
            }

            TWeight totalWeight = default!;
            if (mstEdges.Count > 0)
            {
                dynamic sum = mstEdges[0].Weight;
                for (int i = 1; i < mstEdges.Count; i++)
                {
                    sum += (dynamic)mstEdges[i].Weight;
                }
                totalWeight = (TWeight)sum;
            }

            return new BoruvkaResult<TVertex, TWeight>(mstEdges, totalWeight);
        } 

        private class UnionFind
        {
            private readonly int[] parent;
            private readonly int[] rank;

            public UnionFind(int size)
            {
                parent = new int[size];
                rank = new int[size];
                for (int i = 0; i < size; i++)
                {
                    parent[i] = i;
                }
            }

            public int Find(int i)
            {
                if (parent[i] == i)
                    return i;
                return parent[i] = Find(parent[i]);
            }

            public bool Union(int x, int y)
            {
                int rootX = Find(x);
                int rootY = Find(y);
                if (rootX != rootY)
                {
                    if (rank[rootX] < rank[rootY])
                    {
                        parent[rootX] = rootY;
                    }
                    else if (rank[rootX] > rank[rootY])
                    {
                        parent[rootY] = rootX;
                    }
                    else
                    {
                        parent[rootY] = rootX;
                        rank[rootX]++;
                    }
                    return true;
                }
                return false;
            }
        }
    }
}