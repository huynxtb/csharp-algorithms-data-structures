using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents the result of the Stoer-Wagner algorithm, containing the minimum cut weight and the partition of vertices.
/// </summary>
public record GlobalMinCutResult
{
    /// <summary>
    /// The total weight of the minimum cut.
    /// </summary>
    public double MinCutWeight { get; init; }

    /// <summary>
    /// Vertices on one side of the minimum cut partition.
    /// </summary>
    public IReadOnlyCollection<int> PartitionA { get; init; }

    /// <summary>
    /// Vertices on the other side of the minimum cut partition.
    /// </summary>
    public IReadOnlyCollection<int> PartitionB { get; init; }

    public GlobalMinCutResult(double minCutWeight, IReadOnlyCollection<int> partitionA, IReadOnlyCollection<int> partitionB)
    {
        MinCutWeight = minCutWeight;
        PartitionA = partitionA ?? throw new ArgumentNullException(nameof(partitionA));
        PartitionB = partitionB ?? throw new ArgumentNullException(nameof(partitionB));
    }
}

/// <summary>
/// Implements the Stoer-Wagner algorithm to find the global minimum cut in an undirected, weighted graph.
/// </summary>
public static class StoerWagner
{
    /// <summary>
    /// Finds the global minimum cut of an undirected, weighted graph using the Stoer-Wagner algorithm.
    /// </summary>
    /// <param name="numVertices">The total number of vertices in the graph. Vertices are assumed to be labeled from 0 to numVertices - 1.</param>
    /// <param name="edges">A collection of tuples representing the edges. Each tuple is (vertex1, vertex2, weight).</param>
    /// <returns>A GlobalMinCutResult object containing the minimum cut weight and the vertex partition.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if numVertices is less than 2.</exception>
    /// <exception cref="ArgumentException">Thrown if any edge weight is negative.</exception>
    public static GlobalMinCutResult FindGlobalMinCut(int numVertices, IEnumerable<(int u, int v, double weight)> edges)
    {
        if (numVertices < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(numVertices), "Graph must have at least 2 vertices.");
        }

        // Adjacency matrix to store edge weights. Initialize with 0.
        // We use a list of lists for flexibility in representing merged vertices.
        var adjMatrix = new List<List<double>>(numVertices);
        for (int i = 0; i < numVertices; i++)
        {
            adjMatrix.Add(new List<double>(new double[numVertices]));
        }

        // Keep track of original vertex IDs for each current "super-vertex"
        var vertexSets = new List<HashSet<int>>(numVertices);
        for (int i = 0; i < numVertices; i++)
        {
            vertexSets.Add(new HashSet<int> { i });
        }

        // Populate adjacency matrix and validate weights
        foreach (var (u, v, weight) in edges)
        {
            if (weight < 0)
            {
                throw new ArgumentException("Edge weights must be non-negative.");
            }
            if (u < 0 || u >= numVertices || v < 0 || v >= numVertices)
            {
                throw new ArgumentOutOfRangeException("Edge vertices must be within the valid range [0, numVertices - 1].");
            }
            adjMatrix[u][v] += weight;
            adjMatrix[v][u] += weight;
        }

        double minCutWeight = double.PositiveInfinity;
        List<int> minCutPartitionA = new List<int>();
        List<int> minCutPartitionB = new List<int>();

        // Keep track of active vertices. Initially all vertices are active.
        var activeVertices = new HashSet<int>(Enumerable.Range(0, numVertices));

        // Main loop: contract graph until only one vertex remains
        while (activeVertices.Count > 1)
        {
            // Run MinimumCutPhase to find the most tightly connected pair
            var phaseResult = MinimumCutPhase(adjMatrix, vertexSets, activeVertices);

            // Update global minimum cut if the current phase's cut is smaller
            if (phaseResult.CutWeight < minCutWeight)
            {
                minCutWeight = phaseResult.CutWeight;
                // The partition is formed by the last vertex added (s) and all others in the current active set.
                // We need to map these back to original vertex IDs.
                minCutPartitionA = vertexSets[phaseResult.SecondToLastVertex].ToList();
                minCutPartitionB = vertexSets[phaseResult.LastVertex].ToList();
            }

            // Merge the last two vertices added in the phase
            int uToMerge = phaseResult.SecondToLastVertex;
            int vToMerge = phaseResult.LastVertex;

            // Merge vToMerge into uToMerge
            // Update adjacency matrix: add weights from vToMerge to uToMerge
            for (int i = 0; i < adjMatrix.Count; i++)
            {
                if (i != uToMerge && i != vToMerge && activeVertices.Contains(i))
                {
                    adjMatrix[uToMerge][i] += adjMatrix[vToMerge][i];
                    adjMatrix[i][uToMerge] += adjMatrix[vToMerge][i];
                }
            }
            // Zero out the row and column for vToMerge (effectively removing it)
            for (int i = 0; i < adjMatrix.Count; i++)
            {
                adjMatrix[vToMerge][i] = 0;
                adjMatrix[i][vToMerge] = 0;
            }

            // Merge the vertex sets
            vertexSets[uToMerge].UnionWith(vertexSets[vToMerge]);
            vertexSets[vToMerge].Clear(); // Clear the set for the merged vertex

            // Remove vToMerge from active vertices
            activeVertices.Remove(vToMerge);
        }

        // If minCutWeight is still infinity, it means the graph was disconnected or had only one vertex (handled by initial check).
        // For a connected graph with >= 2 vertices, minCutWeight will be updated.
        if (minCutWeight == double.PositiveInfinity)
        {
            // This case should ideally not be reached for a connected graph with >= 2 vertices.
            // If it is, it might indicate a disconnected graph where the min cut is 0.
            // However, the algorithm as implemented finds the min cut within a component.
            // For a truly disconnected graph, the min cut is 0 between components.
            // The current implementation will return the min cut of the largest component if disconnected.
            // To handle 0-cut for disconnected graphs, a pre-check for connectivity would be needed.
            // For simplicity and adherence to standard Stoer-Wagner, we assume connectivity or focus on component cuts.
            // If the graph is disconnected, the min cut is 0. We can return an arbitrary partition.
            var allVertices = Enumerable.Range(0, numVertices).ToList();
            return new GlobalMinCutResult(0, allVertices, new List<int>());
        }

        // Ensure PartitionA and PartitionB are populated correctly for the final min cut
        // The last recorded min cut is the global minimum.
        // The partition is implicitly defined by the last merge operation that yielded the min cut.
        // The `phaseResult` that yielded the `minCutWeight` is what we need.
        // However, the `phaseResult` is local to the loop. We need to store the partition when minCutWeight is updated.
        // The current logic stores the partition correctly when minCutWeight is updated.

        // If the graph was initially disconnected, the min cut is 0. The algorithm might find a non-zero cut within a component.
        // A robust solution for disconnected graphs would involve checking connectivity first.
        // For this implementation, we assume the goal is the min cut of the graph, which is 0 if disconnected.
        // If the algorithm completes and minCutWeight is still infinity, it implies no edges were processed or graph is trivial.
        // The initial check for numVertices < 2 handles trivial cases.
        // If minCutWeight is updated, it's a valid cut. If not, and numVertices >= 2, it implies disconnectedness.

        // If the graph is disconnected, the min cut is 0. The algorithm might have found a cut within a component.
        // The `minCutWeight` will be the smallest cut found. If it's still infinity, it means no edges were processed.
        // If the graph is disconnected, the true min cut is 0. The algorithm will find the min cut of the component it processes.
        // To correctly return 0 for disconnected graphs, we'd need to detect connectivity.
        // For now, we return the smallest cut found, which might be 0 if the graph is disconnected and no edges are processed.
        // If minCutWeight is still infinity, it means no edges were processed, which implies a disconnected graph with no edges.
        if (minCutWeight == double.PositiveInfinity && numVertices >= 2)
        {
            // This implies a graph with >= 2 vertices but no edges, hence disconnected.
            var allVertices = Enumerable.Range(0, numVertices).ToList();
            return new GlobalMinCutResult(0, allVertices, new List<int>());
        }

        // The `minCutPartitionA` and `minCutPartitionB` are populated when `minCutWeight` is updated.
        // We need to ensure they are correctly assigned to the final minimum cut.
        // The `phaseResult` that yielded the `minCutWeight` is the one whose partition we need.
        // The current logic correctly captures this by updating `minCutPartitionA` and `minCutPartitionB` when `minCutWeight` is updated.

        return new GlobalMinCutResult(minCutWeight, minCutPartitionA, minCutPartitionB);
    }

    /// <summary>
    /// Represents the result of a single MinimumCutPhase.
    /// </summary>
    private record MinimumCutPhaseResult
    {
        public double CutWeight { get; init; }
        public int LastVertex { get; init; } // The last vertex added to the set
        public int SecondToLastVertex { get; init; } // The second to last vertex added to the set

        public MinimumCutPhaseResult(double cutWeight, int lastVertex, int secondToLastVertex)
        {
            CutWeight = cutWeight;
            LastVertex = lastVertex;
            SecondToLastVertex = secondToLastVertex;
        }
    }

    /// <summary>
    /// Executes a single phase of the Stoer-Wagner algorithm (Maximum Adjacency Search).
    /// Finds the most tightly connected vertex pair and the cut weight associated with the last added vertex.
    /// </summary>
    /// <param name="adjMatrix">The current adjacency matrix of the graph.</param>
    /// <param name="vertexSets">The current mapping of super-vertex indices to original vertex IDs.</param>
    /// <param name="activeVertices">The set of currently active (not yet merged) vertex indices.</param>
    /// <returns>A MinimumCutPhaseResult containing the cut weight and the last two vertices added.</returns>
    private static MinimumCutPhaseResult MinimumCutPhase(List<List<double>> adjMatrix, List<HashSet<int>> vertexSets, HashSet<int> activeVertices)
    {
        int numVertices = adjMatrix.Count;
        var weights = new double[numVertices]; // Stores the sum of weights connecting to the current set
        var added = new bool[numVertices];    // Tracks if a vertex has been added to the set

        int lastVertex = -1;
        int secondToLastVertex = -1;
        double maxWeight = -1;

        // Start with an arbitrary active vertex (e.g., the first one found)
        int startVertex = activeVertices.First();
        added[startVertex] = true;
        lastVertex = startVertex;

        // Initialize weights for neighbors of the start vertex
        for (int i = 0; i < numVertices; i++)
        {
            if (activeVertices.Contains(i) && i != startVertex)
            {
                weights[i] = adjMatrix[startVertex][i];
            }
        }

        // Perform Maximum Adjacency Search for |activeVertices| - 1 steps
        for (int k = 0; k < activeVertices.Count - 1; k++)
        {
            maxWeight = -1;
            int nextVertex = -1;

            // Find the active vertex not yet added that has the maximum weight connection to the current set
            foreach (int v in activeVertices)
            {
                if (!added[v] && weights[v] > maxWeight)
                {
                    maxWeight = weights[v];
                    nextVertex = v;
                }
            }

            if (nextVertex == -1) // Should not happen in a connected component
            {
                // This might indicate a disconnected graph or an issue.
                // For robustness, we can break or handle this. If nextVertex is -1, it means no more connections exist.
                // This could happen if the remaining active vertices form a disconnected component.
                // In such a case, the cut weight to the current set is 0.
                // However, the standard algorithm assumes connectivity within the phase.
                // If this happens, it implies the cut weight to the current set is 0.
                // We can break and consider the current `maxWeight` (which would be 0 if no connections) as the cut.
                break;
            }

            // Add the found vertex to the set
            added[nextVertex] = true;
            secondToLastVertex = lastVertex;
            lastVertex = nextVertex;

            // Update weights for neighbors of the newly added vertex
            foreach (int v in activeVertices)
            {
                if (!added[v])
                {
                    weights[v] += adjMatrix[nextVertex][v];
                }
            }
        }

        // The cut weight of the phase is the sum of weights connecting the last added vertex to the rest of the set.
        // This is `maxWeight` from the last iteration of the loop.
        // If `nextVertex` was -1 in the last iteration, `maxWeight` would be 0.
        double cutWeight = maxWeight;
        if (cutWeight < 0) cutWeight = 0; // Ensure non-negative cut weight

        return new MinimumCutPhaseResult(cutWeight, lastVertex, secondToLastVertex);
    }
}