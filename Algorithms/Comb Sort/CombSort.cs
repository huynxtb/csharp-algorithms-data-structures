using System;
using System.Collections.Generic;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Provides an in-place implementation of the Comb Sort algorithm.
    /// </summary>
    public static class CombSort
    {
        /// <summary>
        /// Sorts the elements in the entire <see cref="IList{T}"/> in-place using the default comparer.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list, which must implement <see cref="IComparable{T}"/>.</typeparam>
        /// <param name="list">The mutable list to sort.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> is null.</exception>
        public static void Sort<T>(IList<T> list) where T : IComparable<T>
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            Sort(list, Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="IList{T}"/> in-place using the specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The mutable list to sort.</param>
        /// <param name="comparer">The comparer implementation to use when comparing elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="list"/> or <paramref name="comparer"/> is null.</exception>
        public static void Sort<T>(IList<T> list, IComparer<T> comparer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            int n = list.Count;
            if (n <= 1)
            {
                return;
            }

            int gap = n;
            const double shrinkFactor = 1.3;
            bool swapped = true;

            while (gap > 1 || swapped)
            {
                gap = (int)(gap / shrinkFactor);
                if (gap < 1)
                {
                    gap = 1;
                }

                swapped = false;

                for (int i = 0; i + gap < n; i++)
                {
                    if (comparer.Compare(list[i], list[i + gap]) > 0)
                    {
                        T temp = list[i];
                        list[i] = list[i + gap];
                        list[i + gap] = temp;
                        swapped = true;
                    }
                }
            }
        }
    }
}