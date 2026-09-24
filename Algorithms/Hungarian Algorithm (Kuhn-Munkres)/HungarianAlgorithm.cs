using System;
using System.Collections.Generic;

namespace Algorithms.Graph
{
    /// <summary>
    /// Represents the result of an assignment matching computed by the Hungarian algorithm.
    /// </summary>
    public sealed class HungarianResult
    {
        /// <summary>
        /// Gets the total optimal objective value (total minimum cost or maximum weight).
        /// </summary>
        public double TotalWeight { get; }

        /// <summary>
        /// Gets the zero-based matching array mapping each left node index (row) to its assigned right node index (column).
        /// Unassigned nodes are represented by -1.
        /// </summary>
        public IReadOnlyList<int> LeftAssignment { get; }

        /// <summary>
        /// Gets the zero-based matching array mapping each right node index (column) to its assigned left node index (row).
        /// Unassigned nodes are represented by -1.
        /// </summary>
        public IReadOnlyList<int> RightAssignment { get; }

        /// <summary>
        /// Gets a value indicating whether a complete valid assignment was successfully constructed.
        /// </summary>
        public bool IsFeasible { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HungarianResult"/> class.
        /// </summary>
        /// <param name="totalWeight">The total optimal objective weight.</param>
        /// <param name="leftAssignment">The left-to-right matching array.</param>
        /// <param name="rightAssignment">The right-to-left matching array.</param>
        /// <param name="isFeasible">A value indicating whether the assignment is feasible.</param>
        public HungarianResult(double totalWeight, int[] leftAssignment, int[] rightAssignment, bool isFeasible)
        {
            TotalWeight = totalWeight;
            LeftAssignment = leftAssignment ?? throw new ArgumentNullException(nameof(leftAssignment));
            RightAssignment = rightAssignment ?? throw new ArgumentNullException(nameof(rightAssignment));
            IsFeasible = isFeasible;
        }
    }

    /// <summary>
    /// Provides production-grade implementations of the Hungarian (Kuhn-Munkres) algorithm
    /// for Maximum Weight Bipartite Matching and Minimum Cost Assignment problems in O(N^2 * M) time.
    /// </summary>
    public static class HungarianAlgorithm
    {
        private const double Epsilon = 1e-9;
        private const double Infinity = 1e18;

        /// <summary>
        /// Finds the maximum weight matching in a bipartite graph represented by a rectangular weight matrix.
        /// Missing or non-existent edges should be set to <see cref="double.NegativeInfinity"/> or a sufficiently large negative value.
        /// </summary>
        /// <param name="weights">An N x M rectangular matrix of weights where N &lt;= M.</param>
        /// <returns>A <see cref="HungarianResult"/> containing the total weight and assignment mappings.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="weights"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the matrix is jagged or rows exceed columns (N &gt; M).</exception>
        public static HungarianResult FindMaximumWeightMatching(double[,] weights)
        {
            if (weights == null)
            {
                throw new ArgumentNullException(nameof(weights));
            }

            int n = weights.GetLength(0);
            int m = weights.GetLength(1);

            if (n > m)
            {
                throw new ArgumentException($"Number of left nodes (rows={n}) cannot exceed number of right nodes (columns={m}).", nameof(weights));
            }

            if (n == 0 || m == 0)
            {
                return new HungarianResult(0.0, Array.Empty<int>(), Array.Empty<int>(), true);
            }

            // Transform max-weight matching into min-cost assignment by negating weights
            double[,] costMatrix = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    double w = weights[i, j];
                    if (double.IsNegativeInfinity(w) || double.IsNaN(w))
                    {
                        costMatrix[i, j] = Infinity;
                    }
                    else
                    {
                        costMatrix[i, j] = -w;
                    }
                }
            }

            HungarianResult minCostResult = SolveMinimumCost(costMatrix, n, m);

            // Recover the actual weight from original input weights
            double totalMaxWeight = 0.0;
            bool isFeasible = true;
            int[] leftAssignment = new int[n];
            for (int i = 0; i < n; i++)
            {
                int j = minCostResult.LeftAssignment[i];
                leftAssignment[i] = j;
                if (j != -1)
                {
                    double originalWeight = weights[i, j];
                    if (double.IsNegativeInfinity(originalWeight) || double.IsNaN(originalWeight) || costMatrix[i, j] >= Infinity / 2)
                    {
                        isFeasible = false;
                    }
                    else
                    {
                        totalMaxWeight += originalWeight;
                    }
                }
                else
                {
                    isFeasible = false;
                }
            }

            int[] rightAssignment = new int[m];
            for (int j = 0; j < m; j++)
            {
                rightAssignment[j] = minCostResult.RightAssignment[j];
            }

            return new HungarianResult(isFeasible ? totalMaxWeight : 0.0, leftAssignment, rightAssignment, isFeasible);
        }

        /// <summary>
        /// Finds the minimum cost matching in a bipartite graph represented by a rectangular cost matrix.
        /// Non-existent edges should be set to <see cref="double.PositiveInfinity"/> or a sufficiently large positive value.
        /// </summary>
        /// <param name="costs">An N x M rectangular matrix of costs where N &lt;= M.</param>
        /// <returns>A <see cref="HungarianResult"/> containing the total minimum cost and assignment mappings.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="costs"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the matrix is jagged or rows exceed columns (N &gt; M).</exception>
        public static HungarianResult FindMinimumCostMatching(double[,] costs)
        {
            if (costs == null)
            {
                throw new ArgumentNullException(nameof(costs));
            }

            int n = costs.GetLength(0);
            int m = costs.GetLength(1);

            if (n > m)
            {
                throw new ArgumentException($"Number of left nodes (rows={n}) cannot exceed number of right nodes (columns={m}).", nameof(costs));
            }

            if (n == 0 || m == 0)
            {
                return new HungarianResult(0.0, Array.Empty<int>(), Array.Empty<int>(), true);
            }

            double[,] normalizedCosts = new double[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    double c = costs[i, j];
                    if (double.IsPositiveInfinity(c) || double.IsNaN(c))
                    {
                        normalizedCosts[i, j] = Infinity;
                    }
                    else
                    {
                        normalizedCosts[i, j] = c;
                    }
                }
            }

            HungarianResult result = SolveMinimumCost(normalizedCosts, n, m);

            double totalCost = 0.0;
            bool isFeasible = true;
            int[] leftAssignment = new int[n];
            for (int i = 0; i < n; i++)
            {
                int j = result.LeftAssignment[i];
                leftAssignment[i] = j;
                if (j != -1)
                {
                    if (normalizedCosts[i, j] >= Infinity / 2)
                    {
                        isFeasible = false;
                    }
                    else
                    {
                        totalCost += costs[i, j];
                    }
                }
                else
                {
                    isFeasible = false;
                }
            }

            int[] rightAssignment = new int[m];
            for (int j = 0; j < m; j++)
            {
                rightAssignment[j] = result.RightAssignment[j];
            }

            return new HungarianResult(isFeasible ? totalCost : double.PositiveInfinity, leftAssignment, rightAssignment, isFeasible);
        }

        /// <summary>
        /// Internal 1-indexed O(N^2 * M) Hungarian algorithm for minimum cost bipartite matching.
        /// </summary>
        private static HungarianResult SolveMinimumCost(double[,] costMatrix, int n, int m)
        {
            // Potential dual variables
            // u corresponds to rows (1..n), v corresponds to columns (0..m)
            double[] u = new double[n + 1];
            double[] v = new double[m + 1];

            // p[j] stores the row assigned to column j (1-based, 0 means unassigned)
            int[] p = new int[m + 1];

            // way[j] stores the column predecessor in the alternating tree
            int[] way = new int[m + 1];

            // minv[j] stores the minimum reduced cost slack for column j
            double[] minv = new double[m + 1];
            bool[] used = new bool[m + 1];

            for (int i = 1; i <= n; i++)
            {
                p[0] = i;
                int j0 = 0;

                for (int j = 0; j <= m; j++)
                {
                    minv[j] = Infinity;
                    used[j] = false;
                }

                do
                {
                    used[j0] = true;
                    int i0 = p[j0];
                    double delta = Infinity;
                    int j1 = 0;

                    for (int j = 1; j <= m; j++)
                    {
                        if (!used[j])
                        {
                            double cur = costMatrix[i0 - 1, j - 1] - u[i0] - v[j];
                            if (cur < minv[j])
                            {
                                minv[j] = cur;
                                way[j] = j0;
                            }

                            if (minv[j] < delta)
                            {
                                delta = minv[j];
                                j1 = j;
                            }
                        }
                    }

                    if (delta >= Infinity / 2)
                    {
                        // Infeasible augment path: infinite cost edges encountered
                        break;
                    }

                    for (int j = 0; j <= m; j++)
                    {
                        if (used[j])
                        {
                            u[p[j]] += delta;
                            v[j] -= delta;
                        }
                        else
                        {
                            minv[j] -= delta;
                        }
                    }

                    j0 = j1;
                } while (p[j0] != 0);

                // Reconstruct augmenting path if complete
                if (p[j0] == 0)
                {
                    do
                    {
                        int j1 = way[j0];
                        p[j0] = p[j1];
                        j0 = j1;
                    } while (j0 != 0);
                }
            }

            int[] leftAssignment = new int[n];
            int[] rightAssignment = new int[m];

            for (int i = 0; i < n; i++)
            {
                leftAssignment[i] = -1;
            }
            for (int j = 0; j < m; j++)
            {
                rightAssignment[j] = -1;
            }

            for (int j = 1; j <= m; j++)
            {
                if (p[j] > 0)
                {
                    int leftIdx = p[j] - 1;
                    int rightIdx = j - 1;
                    leftAssignment[leftIdx] = rightIdx;
                    rightAssignment[rightIdx] = leftIdx;
                }
            }

            // Objective function value is -v[0]
            double totalCost = -v[0];
            return new HungarianResult(totalCost, leftAssignment, rightAssignment, true);
        }
    }
}