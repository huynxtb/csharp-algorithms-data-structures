using System;
using System.Collections.Generic;

/// <summary>
/// Provides an implementation of the Iterative Deepening Depth-First Search (IDDFS) algorithm.
/// </summary>
/// <typeparam name="T">The type of nodes in the graph. Must be non-null and comparable for equality.</typeparam>
public class IterativeDeepeningDFS<T> where T : notnull
{
    private readonly IReadOnlyDictionary<T, List<T>> _adjacencyList;
    private readonly IEqualityComparer<T> _comparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="IterativeDeepeningDFS{T}"/> class with a given adjacency list.
    /// </summary>
    /// <param name="adjacencyList">The graph represented as an adjacency list dictionary.</param>
    /// <param name="comparer">An optional equality comparer for node instances.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="adjacencyList"/> is null.</exception>
    public IterativeDeepeningDFS(Dictionary<T, List<T>> adjacencyList, IEqualityComparer<T>? comparer = null)
    {
        if (adjacencyList == null)
        {
            throw new ArgumentNullException(nameof(adjacencyList));
        }

        _comparer = comparer ?? EqualityComparer<T>.Default;
        _adjacencyList = new Dictionary<T, List<T>>(adjacencyList, _comparer);
    }

    /// <summary>
    /// Searches for the shortest path from the start node to the goal node within the specified maximum depth limit.
    /// </summary>
    /// <param name="start">The starting node.</param>
    /// <param name="goal">The target goal node.</param>
    /// <param name="maxDepth">The maximum depth (number of edges) to explore.</param>
    /// <returns>
    /// A <see cref="List{T}"/> containing the ordered sequence of nodes from <paramref name="start"/> to <paramref name="goal"/>
    /// if a path is found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxDepth"/> is negative.</exception>
    public List<T>? Search(T start, T goal, int maxDepth)
    {
        if (maxDepth < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDepth), "Maximum depth must be non-negative.");
        }

        // Iteratively increase the depth limit from 0 up to maxDepth
        for (int depth = 0; depth <= maxDepth; depth++)
        {
            var currentPath = new List<T> { start };
            var visitedInCurrentPath = new HashSet<T>(_comparer) { start };

            bool pathFound = DepthLimitedSearch(start, goal, depth, currentPath, visitedInCurrentPath);
            if (pathFound)
            {
                return currentPath;
            }
        }

        return null;
    }

    /// <summary>
    /// Performs a recursive Depth-Limited Search (DLS).
    /// </summary>
    /// <param name="current">The current node being visited.</param>
    /// <param name="goal">The target goal node.</param>
    /// <param name="remainingDepth">The remaining depth allowed from the current node.</param>
    /// <param name="path">The accumulator list representing the path explored so far.</param>
    /// <param name="visitedOnBranch">A set tracking nodes in the current recursion branch to prevent cycles.</param>
    /// <returns><c>true</c> if the goal node was reached within the depth limit; otherwise, <c>false</c>.</returns>
    private bool DepthLimitedSearch(
        T current,
        T goal,
        int remainingDepth,
        List<T> path,
        HashSet<T> visitedOnBranch)
    {
        if (_comparer.Equals(current, goal))
        {
            return true;
        }

        if (remainingDepth <= 0)
        {
            return false;
        }

        if (!_adjacencyList.TryGetValue(current, out var neighbors) || neighbors == null)
        {
            return false;
        }

        foreach (var neighbor in neighbors)
        {
            // Avoid cycles within the current DFS recursion path
            if (visitedOnBranch.Add(neighbor))
            {
                path.Add(neighbor);

                if (DepthLimitedSearch(neighbor, goal, remainingDepth - 1, path, visitedOnBranch))
                {
                    return true;
                }

                // Backtrack
                path.RemoveAt(path.Count - 1);
                visitedOnBranch.Remove(neighbor);
            }
        }

        return false;
    }
}