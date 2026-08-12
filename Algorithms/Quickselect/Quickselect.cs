using System;
using System.Collections.Generic;

/// <summary>
/// Provides a static method for finding the k-th smallest element in a list using the Quickselect algorithm.
/// </summary>
public static class Quickselect
{
    /// <summary>
    /// Finds the k-th smallest element (0-indexed) in a list using the Quickselect algorithm.
    /// The list is modified in-place during the selection process.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list, which must implement <see cref="IComparable{T}"/>.</typeparam>
    /// <param name="list">The list of elements to search within.</param>
    /// <param name="k">The 0-indexed rank of the element to find (e.g., k=0 for the smallest, k=list.Count-1 for the largest).</param>
    /// <returns>The k-th smallest element in the list.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input list is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the input list is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if k is less than 0 or greater than or equal to the list's count.</exception>
    public static T Select<T>(IList<T> list, int k) where T : IComparable<T>
    {
        // Input validation
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list), "The input list cannot be null.");
        }
        if (list.Count == 0)
        {
            throw new ArgumentException("The input list cannot be empty.", nameof(list));
        }
        if (k < 0 || k >= list.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(k), "k must be a non-negative integer less than the list's count.");
        }

        // Call the recursive helper to perform Quickselect
        return QuickselectRecursive(list, 0, list.Count - 1, k);
    }

    /// <summary>
    /// Recursive helper method for the Quickselect algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list of elements.</param>
    /// <param name="left">The starting index of the current sub-array.</param>
    /// <param name="right">The ending index of the current sub-array.</param>
    /// <param name="k">The 0-indexed rank of the element to find.</param>
    /// <returns>The k-th smallest element in the specified range.</returns>
    private static T QuickselectRecursive<T>(IList<T> list, int left, int right, int k) where T : IComparable<T>
    {
        // Base case: if the sub-array contains only one element, it must be the k-th element
        if (left == right)
        {
            return list[left];
        }

        // Partition the list around a pivot element. Lomuto's partition scheme is used.
        // The pivotIndex will be the final sorted position of the pivot element.
        int pivotIndex = Partition(list, left, right);

        // Check if the pivot element is the k-th smallest element
        if (k == pivotIndex)
        {
            return list[k];
        }
        else if (k < pivotIndex)
        {
            // If k is less than the pivot's index, the k-th element must be in the left sub-array
            return QuickselectRecursive(list, left, pivotIndex - 1, k);
        }
        else
        {
            // If k is greater than the pivot's index, the k-th element must be in the right sub-array
            return QuickselectRecursive(list, pivotIndex + 1, right, k);
        }
    }

    /// <summary>
    /// Partitions a sub-array of the list using Lomuto's partitioning scheme.
    /// It selects the last element as the pivot and rearranges the sub-array
    /// such that all elements less than or equal to the pivot are to its left,
    /// and all elements greater than the pivot are to its right.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list of elements.</param>
    /// <param name="left">The starting index of the sub-array to partition.</param>
    /// <param name="right">The ending index of the sub-array to partition (pivot element's initial position).</param>
    /// <returns>The final index of the pivot element after partitioning.</returns>
    private static int Partition<T>(IList<T> list, int left, int right) where T : IComparable<T>
    {
        T pivot = list[right]; // Choose the last element as the pivot
        int i = left; // 'i' is the index of the smaller element, and also the count of elements smaller than or equal to the pivot

        // Iterate through the sub-array from 'left' to 'right-1'
        for (int j = left; j < right; j++)
        {
            // If the current element is less than or equal to the pivot
            if (list[j].CompareTo(pivot) <= 0)
            {
                // Swap it with the element at index 'i' and increment 'i'
                Swap(list, i, j);
                i++;
            }
        }

        // After the loop, all elements from 'left' to 'i-1' are less than or equal to the pivot.
        // All elements from 'i' to 'right-1' are greater than the pivot.
        // Place the pivot element (originally at 'right') at its correct sorted position, which is 'i'.
        Swap(list, i, right);

        return i; // Return the final index of the pivot
    }

    /// <summary>
    /// Swaps two elements in the list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list containing the elements.</param>
    /// <param name="i">The index of the first element to swap.</param>
    /// <param name="j">The index of the second element to swap.</param>
    private static void Swap<T>(IList<T> list, int i, int j)
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}