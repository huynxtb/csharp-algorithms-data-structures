using System;

/// <summary>
/// Provides a static method to sort an array using the Heap Sort algorithm.
/// </summary>
/// <typeparam name="T">The type of elements in the array, which must implement <see cref="IComparable{T}"/>.</typeparam>
public static class HeapSorter<T> where T : IComparable<T>
{
    /// <summary>
    /// Sorts an array in ascending order using the Heap Sort algorithm.
    /// </summary>
    /// <param name="array">The array to be sorted.</param>
    public static void Sort(T[] array)
    {
        if (array == null || array.Length <= 1)
        {
            return; // Already sorted or nothing to sort
        }

        int n = array.Length;

        // Step 1: Build a max-heap from the input array.
        // Iterate from the last non-leaf node up to the root.
        // The last non-leaf node is at index (n / 2) - 1.
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(array, n, i);
        }

        // Step 2: Extract elements one by one from the heap.
        // The largest element (root) is swapped with the last element of the heap,
        // then the heap size is reduced, and the new root is heapified.
        for (int i = n - 1; i > 0; i--)
        {
            // Move current root (largest element) to the end of the unsorted part
            Swap(array, 0, i);

            // Call max Heapify on the reduced heap (excluding the now sorted element)
            Heapify(array, i, 0);
        }
    }

    /// <summary>
    /// Restores the max-heap property for a subtree rooted at <paramref name="rootIndex"/>.
    /// Assumes that the subtrees are already max-heaps.
    /// </summary>
    /// <param name="array">The array representing the heap.</param>
    /// <param name="heapSize">The current size of the heap (number of elements in the array considered part of the heap).</param>
    /// <param name="rootIndex">The index of the root of the subtree to heapify.</param>
    private static void Heapify(T[] array, int heapSize, int rootIndex)
    {
        int largest = rootIndex; // Initialize largest as root
        int leftChild = 2 * rootIndex + 1; // Left child index
        int rightChild = 2 * rootIndex + 2; // Right child index

        // If left child exists and is larger than current largest
        if (leftChild < heapSize && array[leftChild].CompareTo(array[largest]) > 0)
        {
            largest = leftChild;
        }

        // If right child exists and is larger than current largest
        if (rightChild < heapSize && array[rightChild].CompareTo(array[largest]) > 0)
        {
            largest = rightChild;
        }

        // If largest is not the root, swap them and recursively heapify the affected sub-tree
        if (largest != rootIndex)
        {
            Swap(array, rootIndex, largest);
            Heapify(array, heapSize, largest);
        }
    }

    /// <summary>
    /// Swaps two elements in the array.
    /// </summary>
    /// <param name="array">The array containing the elements.</param>
    /// <param name="index1">The index of the first element.</param>
    /// <param name="index2">The index of the second element.</param>
    private static void Swap(T[] array, int index1, int index2)
    {
        T temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }
}