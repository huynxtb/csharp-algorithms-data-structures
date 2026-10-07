using System;
using System.Collections.Generic;
using System.Linq;

namespace StoerWagnerAlgorithm
{
    /// <summary>
    /// Represents the result of a global minimum cut computation.
    /// </summary>
    public sealed class MinCutResult
    {
        /// <summary>
        /// Gets the total weight of the edges crossing the minimum cut.
        /// </summary>
        public double CutWeight { get; }

        /// <summary>
        /// Gets the vertices in partition A.
        /// </summary>
        public IReadOnlyCollection<int> PartitionA { get; }

        /// <summary>
        /// Gets the vertices in partition B.
        /// </summary>
        public IReadOnlyCollection<int> PartitionB { get; }

        public MinCutResult(double cutWeight, IEnumerable<int> partitionA, IEnumerable<int> partitionB)
        {
            CutWeight = cutWeight;
            PartitionA = partitionA.ToList().AsReadOnly();
            PartitionB = partitionB.ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Implements the Stoer-Wagner algorithm for finding the global minimum cut in undirected, weighted graphs.
    /// </summary>
    public static class StoerWagnerMinCut
    {
        /// <summary>
        /// Computes the global minimum cut for an undirected graph represented by an edge list.
        /// </summary>
        /// <param name="verticesCount">The total number of vertices (labeled 0 to verticesCount - 1).</param>
        /// <param name="edges">A collection of tuples representing (u, v, weight).</param>
        /// <returns>A MinCutResult containing the cut weight and the two vertex partitions.</returns>
        /// <exception cref="ArgumentException">Thrown when verticesCount < 2 or invalid edges/weights are provided.</exception>
        public static MinCutResult ComputeMinCut(int verticesCount, IEnumerable<(int u, int v, double weight)> edges)
        {
            if (verticesCount < 2)
            {
                throw new ArgumentException("A graph must contain at least 2 vertices to compute a cut.", nameof(verticesCount));
            }

            if (edges == null)
            {
                throw new ArgumentNullException(nameof(edges));
            }

            double[,] adjacencyMatrix = new double[verticesCount, verticesCount];
            foreach (var (u, v, weight) in edges)
            {
                if (u < 0 || u >= verticesCount || v < 0 || v >= verticesCount)
                {
                    throw new ArgumentOutOfRangeException(nameof(edges), "Vertex indices must be between 0 and verticesCount - 1.");
                }
                if (weight < 0)
                {
                    throw new ArgumentException("Edge weights must be non-negative.", nameof(edges));
                }
                if (u != v)
                {
                    adjacencyMatrix[u, v] += weight;
                    adjacencyMatrix[v, u] += weight;
                }
            }

            return ComputeMinCut(adjacencyMatrix);
        }

        /// <summary>
        /// Computes the global minimum cut for an undirected graph represented by a 2D adjacency matrix.
        /// </summary>
        /// <param name="matrix">A square 2D array representing edge weights between vertices.</param>
        /// <returns>A MinCutResult containing the cut weight and the two vertex partitions.</returns>
        /// <exception cref="ArgumentException">Thrown when the matrix is null, non-square, or smaller than 2x2.</exception>
        public static MinCutResult ComputeMinCut(double[,] matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }

            int n = matrix.GetLength(0);
            if (n != matrix.GetLength(1))
            {
                throw new ArgumentException("Adjacency matrix must be square.", nameof(matrix));
            }
            if (n < 2)
            {
                throw new ArgumentException("The graph must contain at least 2 vertices.", nameof(matrix));
            }

            // Working copy of adjacency matrix
            double[,] currentGraph = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] < 0)
                    {
                        throw new ArgumentException("Edge weights must be non-negative.", nameof(matrix));
                    }
                    currentGraph[i, j] = matrix[i, j];
                }
            }

            // Track the original vertices merged into each super-vertex
            List<HashSet<int>> nodeGroups = new List<HashSet<int>>(n);
            for (int i = 0; i < n; i++)
            {
                nodeGroups.Add(new HashSet<int> { i });
            }

            // List of active super-vertices indices
            List<int> activeNodes = Enumerable.Range(0, n).ToList();

            double minCutWeight = double.PositiveInfinity;
            HashSet<int> bestPartition = null;

            // Execute (n - 1) phases
            for (int phase = 0; phase < n - 1; phase++)
            {
                int activeCount = activeNodes.Count;
                double[] weights = new double[n];
                bool[] added = new bool[n];

                int prev = -1;
                int last = -1;

                // Maximum Adjacency Search (MAS)
                for (int step = 0; step < activeCount; step++)
                {
                    int next = -1;
                    double maxWeight = -1.0;

                    foreach (int v in activeNodes)
                    {
                        if (!added[v] && (next == -1 || weights[v] > maxWeight))
                        {
                            maxWeight = weights[v];
                            next = v;
                        }
                    }

                    added[next] = true;
                    prev = last;
                    last = next;

                    foreach (int v in activeNodes)
                    {
                        if (!added[v])
                        {
                            weights[v] += currentGraph[next, v];
                        }
                    }
                }

                // 'weights[last]' contains the cut-of-the-phase between 'last' and the rest of the graph
                double cutOfThePhase = weights[last];
                if (cutOfThePhase < minCutWeight)
                {
                    minCutWeight = cutOfThePhase;
                    bestPartition = new HashSet<int>(nodeGroups[last]);
                }

                // Merge the last vertex into the second-to-last vertex (prev)
                nodeGroups[prev].UnionWith(nodeGroups[last]);

                foreach (int v in activeNodes)
                {
                    if (v != prev && v != last)
                    {
                        currentGraph[prev, v] += currentGraph[last, v];
                        currentGraph[v, prev] = currentGraph[prev, v];
                    }
                }

                // Remove 'last' from active nodes
                activeNodes.Remove(last);
            }

            var allVertices = new HashSet<int>(Enumerable.Range(0, n));
            var partitionB = new HashSet<int>(allVertices.Except(bestPartition));

            return new MinCutResult(minCutWeight, bestPartition, partitionB);
        }
    }
}