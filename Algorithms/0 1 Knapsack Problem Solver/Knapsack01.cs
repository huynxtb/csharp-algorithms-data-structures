using System;
using System.Collections.Generic;

namespace KnapsackSolver
{
    /// <summary>
    /// Represents an item candidate for the knapsack problem.
    /// </summary>
    public readonly record struct KnapsackItem(int Weight, int Value, string Id = "");

    /// <summary>
    /// Represents the optimal solution for a 0/1 knapsack configuration.
    /// </summary>
    public readonly record struct KnapsackResult(int TotalValue, int TotalWeight, IReadOnlyList<KnapsackItem> SelectedItems);

    /// <summary>
    /// Provides dynamic programming solvers for the 0/1 Knapsack Problem.
    /// </summary>
    public static class Knapsack01
    {
        /// <summary>
        /// Solves the 0/1 Knapsack problem with path reconstruction.
        /// </summary>
        /// <param name="items">Collection of items to select from.</param>
        /// <param name="capacity">Maximum knapsack weight capacity.</param>
        /// <returns>A KnapsackResult containing optimal value, total weight, and selected items.</returns>
        /// <exception cref="ArgumentNullException">Thrown when items is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when capacity or item weight/value is negative.</exception>
        public static KnapsackResult Solve(IReadOnlyList<KnapsackItem> items, int capacity)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
            }

            int n = items.Count;
            for (int i = 0; i < n; i++)
            {
                if (items[i].Weight < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(items), $"Item at index {i} has negative weight.");
                }

                if (items[i].Value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(items), $"Item at index {i} has negative value.");
                }
            }

            if (capacity == 0 || n == 0)
            {
                return new KnapsackResult(0, 0, Array.Empty<KnapsackItem>());
            }

            int[,] dp = new int[n + 1, capacity + 1];

            for (int i = 1; i <= n; i++)
            {
                KnapsackItem current = items[i - 1];
                int weight = current.Weight;
                int val = current.Value;

                for (int w = 0; w <= capacity; w++)
                {
                    if (weight <= w)
                    {
                        int include = dp[i - 1, w - weight] + val;
                        int exclude = dp[i - 1, w];
                        dp[i, w] = include > exclude ? include : exclude;
                    }
                    else
                    {
                        dp[i, w] = dp[i - 1, w];
                    }
                }
            }

            var selected = new List<KnapsackItem>();
            int remainingCapacity = capacity;
            int totalWeight = 0;

            for (int i = n; i > 0 && remainingCapacity > 0; i--)
            {
                if (dp[i, remainingCapacity] != dp[i - 1, remainingCapacity])
                {
                    KnapsackItem item = items[i - 1];
                    selected.Add(item);
                    remainingCapacity -= item.Weight;
                    totalWeight += item.Weight;
                }
            }

            selected.Reverse();
            return new KnapsackResult(dp[n, capacity], totalWeight, selected);
        }
    }
}