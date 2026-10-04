using System;
using System.Collections.Generic;

namespace Algorithms.DataStructures
{
    /// <summary>
    /// A Sparse Table implementation for Range Maximum Query (RMQ).
    /// Provides O(1) time query after O(n log n) preprocessing.
    /// </summary>
    /// <typeparam name="T">The type of elements, must implement IComparable&lt;T&gt;.</typeparam>
    public class SparseTableRMQ<T> where T : IComparable<T>
    {
        private readonly T[][] _table;
        private readonly int[] _log2;
        private readonly int _n;

        /// <summary>
        /// Initializes a new instance of the SparseTableRMQ class with the given data.
        /// Performs O(n log n) preprocessing to build the sparse table.
        /// </summary>
        /// <param name="data">The read-only list of elements to build the sparse table from.</param>
        /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
        /// <exception cref="ArgumentException">Thrown when data is empty.</exception>
        public SparseTableRMQ(IReadOnlyList<T> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data), "Data cannot be null.");
            if (data.Count == 0)
                throw new ArgumentException("Data cannot be empty.", nameof(data));

            _n = data.Count;
            int levels = FloorLog2(_n) + 1;

            // Initialize the sparse table
            _table = new T[levels][];
            for (int k = 0; k < levels; k++)
                _table[k] = new T[_n];

            // Initialize base level (k=0): each cell contains the element itself
            for (int i = 0; i < _n; i++)
                _table[0][i] = data[i];

            // Fill the sparse table using dynamic programming
            for (int k = 1; k < levels; k++)
            {
                for (int i = 0; i + (1 << k) <= _n; i++)
                {
                    _table[k][i] = Max(_table[k - 1][i], _table[k - 1][i + (1 << (k - 1))]);
                }
            }

            // Precompute log2 table for O(1) query
            _log2 = new int[_n + 1];
            _log2[1] = 0;
            for (int i = 2; i <= _n; i++)
                _log2[i] = _log2[i / 2] + 1;
        }

        /// <summary>
        /// Gets the number of elements in the sparse table.
        /// </summary>
        public int Count => _n;

        /// <summary>
        /// Gets the number of levels in the sparse table.
        /// </summary>
        public int Levels => _table.Length;

        /// <summary>
        /// Queries the maximum value in the range [left, right] (inclusive, 0-indexed).
        /// </summary>
        /// <param name="left">The left boundary of the range (inclusive).</param>
        /// <param name="right">The right boundary of the range (inclusive).</param>
        /// <returns>The maximum value in the range.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the range is invalid.</exception>
        public T Query(int left, int right)
        {
            if (left < 0 || right >= _n || left > right)
                throw new ArgumentOutOfRangeException(
                    $"Invalid range [{left}, {right}]. Must satisfy 0 <= left <= right < {_n}.");

            int rangeLength = right - left + 1;
            int k = _log2[rangeLength];
            int step = 1 << k; // 2^k

            return Max(_table[k][left], _table[k][right - step + 1]);
        }

        /// <summary>
        /// Queries the maximum value in the range [left, right] and returns the 0-based index of the maximum.
        /// In case of a tie, returns the leftmost index.
        /// </summary>
        /// <param name="left">The left boundary of the range (inclusive).</param>
        /// <param name="right">The right boundary of the range (inclusive).</param>
        /// <param name="index">The 0-based index of the maximum element found.</param>
        /// <returns>The maximum value in the range.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the range is invalid.</exception>
        public T QueryIndex(int left, int right, out int index)
        {
            if (left < 0 || right >= _n || left > right)
                throw new ArgumentOutOfRangeException(
                    $"Invalid range [{left}, {right}]. Must satisfy 0 <= left <= right < {_n}.");

            int rangeLength = right - left + 1;
            int k = _log2[rangeLength];
            int step = 1 << k; // 2^k

            int rightPos = right - step + 1;
            T leftMax = _table[k][left];
            T rightMax = _table[k][rightPos];

            MaxWithIndex(leftMax, left, rightMax, rightPos, out index);
            return MaxWithIndex(leftMax, left, rightMax, rightPos, out _);
        }

        /// <summary>
        /// Returns the maximum of two comparable values.
        /// </summary>
        private static T Max(T a, T b)
        {
            return a.CompareTo(b) >= 0 ? a : b;
        }

        /// <summary>
        /// Returns the maximum of two comparable values and the index of the winner.
        /// If a >= b, the index of a is returned; otherwise, the index of b.
        /// </summary>
        private static T MaxWithIndex(T a, int ai, T b, int bi, out int winnerIndex)
        {
            if (a.CompareTo(b) >= 0)
            {
                winnerIndex = ai;
                return a;
            }
            else
            {
                winnerIndex = bi;
                return b;
            }
        }

        /// <summary>
        /// Computes floor(log base 2 of n).
        /// </summary>
        private static int FloorLog2(int n)
        {
            if (n <= 1) return 0;
            int log = 0;
            while ((1 << (log + 1)) <= n)
                log++;
            return log;
        }
    }
}