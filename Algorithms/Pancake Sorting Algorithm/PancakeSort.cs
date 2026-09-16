using System;
using System.Collections.Generic;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Implements the Pancake Sorting Algorithm.
    /// Pancake Sort is a sorting algorithm that uses only flip operations to sort an array.
    /// A flip reverses the order of the first k elements (like flipping pancakes with a spatula).
    /// 
    /// Time Complexity: O(n²) comparisons, O(n) flips in the worst case.
    /// Space Complexity: O(n) for the flip sequence tracking.
    /// </summary>
    public static class PancakeSort
    {
        /// <summary>
        /// Sorts the given integer array in ascending order in-place using pancake flips.
        /// </summary>
        /// <param name="array">The array to sort. Will be sorted in-place.</param>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        /// <remarks>
        /// Time Complexity: O(n²) comparisons, O(n) flips in worst case.
        /// Space Complexity: O(1) extra space (in-place sorting).
        /// </remarks>
        public static void Sort(int[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            Sort(array, Comparer<int>.Default);
        }

        /// <summary>
        /// Generic version of pancake sort supporting any comparable type.
        /// Sorts the given array in ascending order in-place using pancake flips.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array. Must implement IComparable&lt;T&gt;.</typeparam>
        /// <param name="array">The array to sort. Will be sorted in-place.</param>
        /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
        /// <remarks>
        /// Time Complexity: O(n²) comparisons, O(n) flips in worst case.
        /// Space Complexity: O(1) extra space (in-place sorting).
        /// </remarks>
        public static void Sort<T>(T[] array) where T : IComparable<T>
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            Sort(array, Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts the given array in ascending order using a custom comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to sort. Will be sorted in-place.</param>
        /// <param name="comparer">The comparer to use for element comparison.</param>
        /// <exception cref="ArgumentNullException">Thrown when array or comparer is null.</exception>
        /// <remarks>
        /// Time Complexity: O(n²) comparisons, O(n) flips in worst case.
        /// Space Complexity: O(1) extra space (in-place sorting).
        /// </remarks>
        public static void Sort<T>(T[] array, IComparer<T> comparer)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            int n = array.Length;
            if (n <= 1)
                return;

            for (int unsortedSize = n; unsortedSize > 1; unsortedSize--)
            {
                int maxIndex = FindMaxIndex(array, unsortedSize, comparer);

                // If the maximum is already at the correct position, skip this round
                if (maxIndex == unsortedSize - 1)
                    continue;

                // If the maximum is not at index 0, flip it to index 0
                if (maxIndex > 0)
                    Flip(array, maxIndex + 1);

                // Flip the entire unsorted portion to place the maximum at its sorted position
                Flip(array, unsortedSize);
            }
        }

        /// <summary>
        /// Returns the sequence of flip sizes (k values) that would sort a copy of the array.
        /// The input array is not modified.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array for which to compute the flip sequence.</param>
        /// <param name="comparer">The comparer to use for element comparison.</param>
        /// <returns>A read-only list of flip sizes representing the pancake sort operations.</returns>
        /// <exception cref="ArgumentNullException">Thrown when array or comparer is null.</exception>
        /// <remarks>
        /// Time Complexity: O(n²) comparisons, O(n) flips in worst case.
        /// Space Complexity: O(n) for the flip sequence list and the array copy.
        /// </remarks>
        public static IReadOnlyList<int> GetFlipSequence<T>(T[] array, IComparer<T> comparer)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            T[] arrayCopy = new T[array.Length];
            Array.Copy(array, arrayCopy, array.Length);

            List<int> flipSequence = new List<int>();
            int n = arrayCopy.Length;

            if (n <= 1)
                return flipSequence;

            for (int unsortedSize = n; unsortedSize > 1; unsortedSize--)
            {
                int maxIndex = FindMaxIndex(arrayCopy, unsortedSize, comparer);

                // If the maximum is already at the correct position, skip
                if (maxIndex == unsortedSize - 1)
                    continue;

                // If the maximum is not at index 0, flip it to index 0
                if (maxIndex > 0)
                {
                    Flip(arrayCopy, maxIndex + 1);
                    flipSequence.Add(maxIndex + 1);
                }

                // Flip the entire unsorted portion to place the maximum at its sorted position
                Flip(arrayCopy, unsortedSize);
                flipSequence.Add(unsortedSize);
            }

            return flipSequence;
        }

        /// <summary>
        /// Reverses the first k elements of the array (indices 0 through k-1).
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to flip.</param>
        /// <param name="k">The number of elements to flip from the beginning.</param>
        private static void Flip<T>(T[] array, int k)
        {
            int left = 0;
            int right = k - 1;

            while (left < right)
            {
                T temp = array[left];
                array[left] = array[right];
                array[right] = temp;

                left++;
                right--;
            }
        }

        /// <summary>
        /// Finds the index of the maximum element within the range [0, endExclusive).
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to search.</param>
        /// <param name="endExclusive">The exclusive end index for the search range.</param>
        /// <param name="comparer">The comparer to use for element comparison.</param>
        /// <returns>The index of the maximum element in the range.</returns>
        private static int FindMaxIndex<T>(T[] array, int endExclusive, IComparer<T> comparer)
        {
            int maxIndex = 0;

            for (int i = 1; i < endExclusive; i++)
            {
                if (comparer.Compare(array[i], array[maxIndex]) > 0)
                    maxIndex = i;
            }

            return maxIndex;
        }
    }
}