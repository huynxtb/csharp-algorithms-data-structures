using System;
using System.Numerics;

namespace Algorithms.Subarray
{
    /// <summary>
    /// Represents the result of the maximum subarray algorithm.
    /// </summary>
    /// <typeparam name="T">The numeric type of the elements.</typeparam>
    public readonly struct SubarrayResult<T> where T : INumber<T>
    {
        /// <summary>
        /// Gets the sum of the maximum subarray.
        /// </summary>
        public T Sum { get; }

        /// <summary>
        /// Gets the starting index of the maximum subarray.
        /// </summary>
        public int StartIndex { get; }

        /// <summary>
        /// Gets the ending index (inclusive) of the maximum subarray.
        /// </summary>
        public int EndIndex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubarrayResult{T}"/> struct.
        /// </summary>
        /// <param name="sum">The sum of the subarray.</param>
        /// <param name="startIndex">The starting index.</param>
        /// <param name="endIndex">The ending index.</param>
        public SubarrayResult(T sum, int startIndex, int endIndex)
        {
            Sum = sum;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }

    /// <summary>
    /// Provides an implementation of Kadane's algorithm to find the maximum subarray.
    /// </summary>
    public static class KadaneAlgorithm
    {
        /// <summary>
        /// Finds the contiguous subarray within a one-dimensional array of numbers which has the largest sum.
        /// </summary>
        /// <typeparam name="T">The numeric type of the elements, implementing <see cref="INumber{T}"/>.</typeparam>
        /// <param name="numbers">The read-only span of numbers to search.</param>
        /// <returns>A <see cref="SubarrayResult{T}"/> containing the sum and indices of the maximum subarray.</returns>
        /// <exception cref="ArgumentException">Thrown when the input span is empty.</exception>
        public static SubarrayResult<T> FindMaximumSubarray<T>(ReadOnlySpan<T> numbers) where T : INumber<T>
        {
            if (numbers.IsEmpty)
            {
                throw new ArgumentException("Input span cannot be empty.", nameof(numbers));
            }

            T maxSoFar = numbers[0];
            T maxEndingHere = numbers[0];
            int start = 0;
            int end = 0;
            int tempStart = 0;

            for (int i = 1; i < numbers.Length; i++)
            {
                T current = numbers[i];

                if (maxEndingHere + current < current)
                {
                    maxEndingHere = current;
                    tempStart = i;
                }
                else
                {
                    maxEndingHere += current;
                }

                if (maxEndingHere > maxSoFar)
                {
                    maxSoFar = maxEndingHere;
                    start = tempStart;
                    end = i;
                }
            }

            return new SubarrayResult<T>(maxSoFar, start, end);
        }
    }
}