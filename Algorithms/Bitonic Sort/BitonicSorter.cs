using System;
using System.Collections.Generic;

namespace AdvancedAlgorithms.Sorting
{
    /// <summary>
    /// Specifies the sorting direction for the Bitonic Sort algorithm.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Ascending order (lowest to highest).
        /// </summary>
        Ascending,

        /// <summary>
        /// Descending order (highest to lowest).
        /// </summary>
        Descending
    }

    /// <summary>
    /// Provides production-grade generic implementations of the Bitonic Sort algorithm.
    /// Handles arbitrary length arrays in-place without requiring external power-of-two padding allocations.
    /// </summary>
    public static class BitonicSorter
    {
        /// <summary>
        /// Sorts an array in ascending order using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array. Must implement <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="array">The zero-based array to be sorted.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is null.</exception>
        public static void Sort<T>(T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            Sort(array, 0, array.Length, Comparer<T>.Default, SortDirection.Ascending);
        }

        /// <summary>
        /// Sorts an array in the specified direction using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array. Must implement <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="array">The zero-based array to be sorted.</param>
        /// <param name="direction">The direction to sort the elements (Ascending or Descending).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is null.</exception>
        public static void Sort<T>(T[] array, SortDirection direction)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            Sort(array, 0, array.Length, Comparer<T>.Default, direction);
        }

        /// <summary>
        /// Sorts an array using a specified custom comparer in ascending order.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The zero-based array to be sorted.</param>
        /// <param name="comparer">The comparer implementation to determine order.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is null.</exception>
        public static void Sort<T>(T[] array, IComparer<T> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            Sort(array, 0, array.Length, comparer ?? Comparer<T>.Default, SortDirection.Ascending);
        }

        /// <summary>
        /// Sorts an array using a specified custom comparer in the specified direction.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The zero-based array to be sorted.</param>
        /// <param name="comparer">The comparer implementation to determine order.</param>
        /// <param name="direction">The direction to sort the elements (Ascending or Descending).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is null.</exception>
        public static void Sort<T>(T[] array, IComparer<T> comparer, SortDirection direction)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            Sort(array, 0, array.Length, comparer ?? Comparer<T>.Default, direction);
        }

        /// <summary>
        /// Sorts a sub-range of an array using Bitonic Sort.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The zero-based array to be sorted.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="length">The number of elements in the range to sort.</param>
        /// <param name="comparer">The comparer implementation to determine order.</param>
        /// <param name="direction">The direction to sort the elements (Ascending or Descending).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index or length is negative, or range exceeds array bounds.</exception>
        public static void Sort<T>(T[] array, int index, int length, IComparer<T> comparer, SortDirection direction = SortDirection.Ascending)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            }

            if (length < 0 || index + length > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length is out of range.");
            }

            if (length <= 1)
            {
                return;
            }

            IComparer<T> comp = comparer ?? Comparer<T>.Default;
            bool ascending = direction == SortDirection.Ascending;
            BitonicSortRecursive(array, index, length, ascending, comp);
        }

        /// <summary>
        /// Recursively constructs a bitonic sequence and sorts it.
        /// Adapts arbitrary lengths by splitting at the greatest power of 2 less than <paramref name="count"/>.
        /// </summary>
        private static void BitonicSortRecursive<T>(T[] array, int low, int count, bool ascending, IComparer<T> comparer)
        {
            if (count <= 1)
            {
                return;
            }

            int mid = GreatestPowerOfTwoLessThan(count);

            // Sort first part in ascending order
            BitonicSortRecursive(array, low, mid, !ascending, comparer);

            // Sort second part in descending order
            BitonicSortRecursive(array, low + mid, count - mid, ascending, comparer);

            // Merge whole sequence in desired direction
            BitonicMergeRecursive(array, low, count, ascending, comparer);
        }

        /// <summary>
        /// Recursively merges a bitonic sequence into monotonic order.
        /// </summary>
        private static void BitonicMergeRecursive<T>(T[] array, int low, int count, bool ascending, IComparer<T> comparer)
        {
            if (count <= 1)
            {
                return;
            }

            int mid = GreatestPowerOfTwoLessThan(count);

            for (int i = low; i < low + count - mid; i++)
            {
                CompareAndSwap(array, i, i + mid, ascending, comparer);
            }

            BitonicMergeRecursive(array, low, mid, ascending, comparer);
            BitonicMergeRecursive(array, low + mid, count - mid, ascending, comparer);
        }

        /// <summary>
        /// Compares two elements at indices <paramref name="i"/> and <paramref name="j"/> and swaps them if they are out of the desired order.
        /// </summary>
        private static void CompareAndSwap<T>(T[] array, int i, int j, bool ascending, IComparer<T> comparer)
        {
            int comparison = comparer.Compare(array[i], array[j]);
            if ((ascending && comparison > 0) || (!ascending && comparison < 0))
            {
                T temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        /// <summary>
        /// Calculates the greatest power of two strictly less than <paramref name="n"/>.
        /// </summary>
        private static int GreatestPowerOfTwoLessThan(int n)
        {
            int k = 1;
            while (k > 0 && k < n)
            {
                k <<= 1;
            }

            return k >> 1;
        }
    }
}