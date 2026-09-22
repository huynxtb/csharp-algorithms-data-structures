using System;
using System.Collections.Generic;

namespace TspOptimization
{
    /// <summary>
    /// Represents an immutable point in a 2D Euclidean coordinate system.
    /// </summary>
    /// <param name="X">The X coordinate.</param>
    /// <param name="Y">The Y coordinate.</param>
    public readonly record struct Point2D(double X, double Y)
    {
        /// <summary>
        /// Calculates the Euclidean distance to another point.
        /// </summary>
        /// <param name="other">The target point.</param>
        /// <returns>The straight-line Euclidean distance.</returns>
        public double DistanceTo(Point2D other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }

    /// <summary>
    /// Represents the search strategy used by the 2-Opt algorithm.
    /// </summary>
    public enum TwoOptStrategy
    {
        /// <summary>
        /// Applies the first valid swap that yields an improvement in tour length.
        /// </summary>
        FirstImprovement,

        /// <summary>
        /// Evaluates all possible 2-opt swaps in an iteration and applies the swap yielding the largest improvement.
        /// </summary>
        BestImprovement
    }

    /// <summary>
    /// Configuration options for the 2-Opt TSP solver.
    /// </summary>
    public sealed class TwoOptOptions
    {
        /// <summary>
        /// Gets or sets the search strategy (First Improvement or Best Improvement).
        /// Default is <see cref="TwoOptStrategy.FirstImprovement"/>.
        /// </summary>
        public TwoOptStrategy Strategy { get; set; } = TwoOptStrategy.FirstImprovement;

        /// <summary>
        /// Gets or sets the maximum number of full 2-opt improvement iterations allowed.
        /// Default is 10,000.
        /// </summary>
        public int MaxIterations { get; set; } = 10000;

        /// <summary>
        /// Gets or sets the minimum delta reduction required to treat a move as an improvement.
        /// Helps guard against floating-point precision issues. Default is 1e-9.
        /// </summary>
        public double Tolerance { get; set; } = 1e-9;

        /// <summary>
        /// Gets or sets a value indicating whether to precalculate an O(N^2) distance matrix.
        /// Useful when N is moderate to avoid repetitive square-root operations.
        /// Default is true.
        /// </summary>
        public bool UsePrecomputedDistanceMatrix { get; set; } = true;
    }

    /// <summary>
    /// Encapsulates the results of a Traveling Salesperson Problem optimization.
    /// </summary>
    public sealed class TspResult
    {
        /// <summary>
        /// Gets the ordered sequence of city/node indices representing the optimized tour.
        /// </summary>
        public IReadOnlyList<int> TourIndices { get; }

        /// <summary>
        /// Gets the total Euclidean round-trip distance of the tour.
        /// </summary>
        public double TotalDistance { get; }

        /// <summary>
        /// Gets the number of 2-opt iterations executed.
        /// </summary>
        public int IterationCount { get; }

        /// <summary>
        /// Gets a value indicating whether the search converged to a 2-optimal state
        /// without prematurely terminating due to iteration limits.
        /// </summary>
        public bool HasConverged { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TspResult"/> class.
        /// </summary>
        /// <param name="tourIndices">The tour index sequence.</param>
        /// <param name="totalDistance">Total tour distance.</param>
        /// <param name="iterationCount">Total iterations completed.</param>
        /// <param name="hasConverged">Indicates if the local optimum was reached.</param>
        public TspResult(int[] tourIndices, double totalDistance, int iterationCount, bool hasConverged)
        {
            TourIndices = tourIndices ?? throw new ArgumentNullException(nameof(tourIndices));
            TotalDistance = totalDistance;
            IterationCount = iterationCount;
            HasConverged = hasConverged;
        }
    }

    /// <summary>
    /// Production-grade 2-Opt local search solver for symmetric 2D Euclidean Traveling Salesperson Problems.
    /// </summary>
    public class TwoOptTspSolver
    {
        private readonly TwoOptOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoOptTspSolver"/> class with custom options.
        /// </summary>
        /// <param name="options">The solver configuration options. If null, default options are used.</param>
        public TwoOptTspSolver(TwoOptOptions? options = null)
        {
            _options = options ?? new TwoOptOptions();
        }

        /// <summary>
        /// Solves the Traveling Salesperson Problem for the specified set of 2D points using the 2-Opt local search algorithm.
        /// </summary>
        /// <param name="points">The collection of 2D coordinates representing cities/nodes.</param>
        /// <param name="initialTour">An optional initial tour permutation. If null, a canonical sequential tour [0..N-1] is used.</param>
        /// <returns>A <see cref="TspResult"/> containing the optimized tour and execution metrics.</returns>
        /// <exception cref="ArgumentNullException">Thrown when points is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the initial tour length does not match points length.</exception>
        public TspResult Solve(IReadOnlyList<Point2D> points, IReadOnlyList<int>? initialTour = null)
        {
            if (points == null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            int n = points.Count;
            if (n == 0)
            {
                return new TspResult(Array.Empty<int>(), 0.0, 0, true);
            }

            if (n <= 3)
            {
                int[] trivialTour = new int[n];
                for (int i = 0; i < n; i++)
                {
                    trivialTour[i] = i;
                }
                double trivialDistance = ComputeTotalDistance(trivialTour, points);
                return new TspResult(trivialTour, trivialDistance, 0, true);
            }

            int[] tour = new int[n];
            if (initialTour != null)
            {
                if (initialTour.Count != n)
                {
                    throw new ArgumentException("Initial tour size must match the number of points.", nameof(initialTour));
                }

                for (int i = 0; i < n; i++)
                {
                    tour[i] = initialTour[i];
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    tour[i] = i;
                }
            }

            double[,]? distanceMatrix = null;
            if (_options.UsePrecomputedDistanceMatrix)
            {
                distanceMatrix = BuildDistanceMatrix(points);
            }

            Func<int, int, double> getDistance = distanceMatrix != null
                ? (u, v) => distanceMatrix[u, v]
                : (u, v) => points[u].DistanceTo(points[v]);

            int iterations = 0;
            bool locallyOptimal = false;

            while (iterations < _options.MaxIterations && !locallyOptimal)
            {
                iterations++;
                bool improved = _options.Strategy == TwoOptStrategy.FirstImprovement
                    ? ApplyFirstImprovement(tour, n, getDistance, _options.Tolerance)
                    : ApplyBestImprovement(tour, n, getDistance, _options.Tolerance);

                if (!improved)
                {
                    locallyOptimal = true;
                }
            }

            double finalDistance = ComputeTourDistance(tour, getDistance);
            return new TspResult(tour, finalDistance, iterations, locallyOptimal);
        }

        /// <summary>
        /// Scans for candidate 2-opt moves and immediately applies the first move that reduces tour length.
        /// </summary>
        private static bool ApplyFirstImprovement(int[] tour, int n, Func<int, int, double> getDist, double tolerance)
        {
            for (int i = 0; i < n - 1; i++)
            {
                int a = tour[i];
                int b = tour[i + 1];

                for (int k = i + 2; k < n; k++)
                {
                    // Avoid checking adjacent wrapped edge for (0, n-1)
                    if (i == 0 && k == n - 1)
                    {
                        continue;
                    }

                    int c = tour[k];
                    int d = tour[(k + 1) % n];

                    double currentCost = getDist(a, b) + getDist(c, d);
                    double newCost = getDist(a, c) + getDist(b, d);
                    double delta = newCost - currentCost;

                    if (delta < -tolerance)
                    {
                        ReverseSegment(tour, i + 1, k);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Scans all candidate 2-opt moves in a pass and applies the single move that yields maximum reduction.
        /// </summary>
        private static bool ApplyBestImprovement(int[] tour, int n, Func<int, int, double> getDist, double tolerance)
        {
            double bestDelta = -tolerance;
            int bestI = -1;
            int bestK = -1;

            for (int i = 0; i < n - 1; i++)
            {
                int a = tour[i];
                int b = tour[i + 1];

                for (int k = i + 2; k < n; k++)
                {
                    if (i == 0 && k == n - 1)
                    {
                        continue;
                    }

                    int c = tour[k];
                    int d = tour[(k + 1) % n];

                    double currentCost = getDist(a, b) + getDist(c, d);
                    double newCost = getDist(a, c) + getDist(b, d);
                    double delta = newCost - currentCost;

                    if (delta < bestDelta)
                    {
                        bestDelta = delta;
                        bestI = i;
                        bestK = k;
                    }
                }
            }

            if (bestI != -1 && bestK != -1)
            {
                ReverseSegment(tour, bestI + 1, bestK);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reverses the segment in the tour array from index <paramref name="start"/> to <paramref name="end"/> inclusive.
        /// </summary>
        private static void ReverseSegment(int[] tour, int start, int end)
        {
            while (start < end)
            {
                int temp = tour[start];
                tour[start] = tour[end];
                tour[end] = temp;
                start++;
                end--;
            }
        }

        /// <summary>
        /// Precomputes the N x N symmetric Euclidean distance matrix.
        /// </summary>
        private static double[,] BuildDistanceMatrix(IReadOnlyList<Point2D> points)
        {
            int n = points.Count;
            double[,] matrix = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                matrix[i, i] = 0.0;
                for (int j = i + 1; j < n; j++)
                {
                    double dist = points[i].DistanceTo(points[j]);
                    matrix[i, j] = dist;
                    matrix[j, i] = dist;
                }
            }
            return matrix;
        }

        /// <summary>
        /// Computes the total Euclidean tour distance directly from coordinates.
        /// </summary>
        private static double ComputeTotalDistance(int[] tour, IReadOnlyList<Point2D> points)
        {
            int n = tour.Length;
            if (n <= 1) return 0.0;

            double total = 0.0;
            for (int i = 0; i < n; i++)
            {
                int u = tour[i];
                int v = tour[(i + 1) % n];
                total += points[u].DistanceTo(points[v]);
            }
            return total;
        }

        /// <summary>
        /// Computes the total Euclidean tour distance using a distance accessor function.
        /// </summary>
        private static double ComputeTourDistance(int[] tour, Func<int, int, double> getDist)
        {
            int n = tour.Length;
            if (n <= 1) return 0.0;

            double total = 0.0;
            for (int i = 0; i < n; i++)
            {
                int u = tour[i];
                int v = tour[(i + 1) % n];
                total += getDist(u, v);
            }
            return total;
        }
    }
}