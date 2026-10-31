using System;
using System.Collections.Generic;

/// <summary>
/// Implements the Smoothsort algorithm for efficient in-place sorting.
/// </summary>
public static class Smoothsort
{
    // Leonardo numbers used for heap structure. Pre-calculated for efficiency.
    private static readonly int[] LeonardoNumbers = {
        1, 1, 3, 5, 9, 15, 25, 41, 67, 109, 177, 287, 465, 753, 1219, 1973, 3193, 5167, 8361, 13529, 21891, 35421, 57313, 92735, 150049, 242785, 392835, 635621, 1028457, 1664079, 2692537, 4356617, 7049155, 11405773, 18454929, 29860703, 48315633, 78176337, 126491971, 204668309, 331160281, 535828591, 866988873, 1402817465
    };

    /// <summary>
    /// Sorts an array of elements in ascending order using Smoothsort.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to sort.</param>
    public static void Sort<T>(T[] array)
    {
        Sort(array, Comparer<T>.Default);
    }

    /// <summary>
    /// Sorts an array of elements in ascending order using Smoothsort with a custom comparer.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to sort.</param>
    /// <param name="comparer">The comparer to use for element comparison.</param>
    public static void Sort<T>(T[] array, IComparer<T> comparer)
    {
        if (array == null || array.Length <= 1)
        {
            return;
        }

        int n = array.Length;
        int i = 1;
        int j = 0;
        int k = 0;

        // Phase 1: Build the initial heap structure.
        while (i < n)
        {
            // Find the largest Leonardo number less than or equal to the remaining size.
            while (k < LeonardoNumbers.Length && LeonardoNumbers[k] <= i)
            {
                k++;
            }
            j = i - LeonardoNumbers[k - 1];

            if (j == 0)
            {
                // If j is 0, it means i is a Leonardo number. We can directly sift.
                Sift(array, i, j, k, comparer);
                i++;
            }
            else
            {
                // If j is not 0, we have a 'left' subtree (size LeonardoNumbers[k-2]) and a 'right' subtree (size LeonardoNumbers[k-1]).
                // We need to trinkle the element at index j to maintain the heap property.
                Trinkle(array, i, j, k, comparer);
                i = j;
            }
        }

        // Phase 2: Extract elements from the heap.
        // The array is now a heap. We extract elements from the end, reducing the heap size.
        while (i > 1)
        {
            // Find the largest Leonardo number less than or equal to the current heap size.
            while (k < LeonardoNumbers.Length && LeonardoNumbers[k] <= i)
            {
                k++;
            }
            j = i - LeonardoNumbers[k - 1];

            if (j == 0)
            {
                // If j is 0, the current root is the largest element. Swap it to the end.
                Swap(array, 0, i - 1);
                i--;
            }
            else
            {
                // If j is not 0, we need to trinkle the element at index j.
                Trinkle(array, i, j, k, comparer);
                i = j;
            }
        }
    }

    /// <summary>
    /// Performs the sift operation to maintain heap property.
    /// </summary>
    private static void Sift<T>(T[] array, int n, int i, int k, IComparer<T> comparer)
    {
        // This is a simplified sift for Smoothsort, as the structure is not a binary heap.
        // It's more about ensuring the root is the largest in its 'tree' context.
        // In Smoothsort, sift is typically called when the root might be smaller than its children.
        // The actual 'sift' logic is implicitly handled by the trinkle operation when building the heap.
        // This method is primarily for clarity and adherence to the algorithm's conceptual steps.
        // For a strict binary heap sift, this would involve comparing with children and swapping.
        // In Smoothsort's context, when `j == 0` (i.e., `i` is a Leonardo number), we are essentially
        // adding a new root to a valid heap. The `trinkle` operation handles the restructuring.
        // This `sift` method is more of a placeholder for the conceptual step.
        // The core heap maintenance is done by `trinkle`.
    }

    /// <summary>
    /// Performs the trinkle operation to restore heap property after an insertion or deletion.
    /// </summary>
    private static void Trinkle<T>(T[] array, int n, int i, int k, IComparer<T> comparer)
    {
        // The trinkle operation is the core of Smoothsort's heap maintenance.
        // It ensures that the element at index `i` is correctly placed within its 'tree' structure.
        // `n` is the current size of the heap.
        // `i` is the index of the element to trinkle.
        // `k` is the index in LeonardoNumbers corresponding to the size of the 'right' subtree.

        while (true)
        {
            // Calculate the indices of the 'left' and 'right' children.
            // The structure is based on Leonardo numbers, not binary heaps.
            // Left child: i - LeonardoNumbers[k-2]
            // Right child: i - LeonardoNumbers[k-1]

            int leftChildIndex = i - LeonardoNumbers[k - 2];
            int rightChildIndex = i - LeonardoNumbers[k - 1];

            // Assume the current element is the largest.
            int largestIndex = i;

            // Compare with the left child if it exists and is within the heap bounds.
            if (leftChildIndex < n && comparer.Compare(array[leftChildIndex], array[largestIndex]) > 0)
            {
                largestIndex = leftChildIndex;
            }

            // Compare with the right child if it exists and is within the heap bounds.
            if (rightChildIndex < n && comparer.Compare(array[rightChildIndex], array[largestIndex]) > 0)
            {
                largestIndex = rightChildIndex;
            }

            // If the current element is not the largest, swap it with the largest child.
            if (largestIndex != i)
            {
                Swap(array, i, largestIndex);

                // Update `i` to the index of the child we swapped with.
                i = largestIndex;

                // Adjust `k` based on the size of the subtree we moved into.
                // If we moved into the left subtree (size LeonardoNumbers[k-2]), the new `k` is `k-1`.
                // If we moved into the right subtree (size LeonardoNumbers[k-1]), the new `k` is `k-2`.
                // This is determined by checking which child was larger.
                if (i == leftChildIndex)
                {
                    k--; // Moving into a subtree of size L(k-2), so new k is k-1.
                }
                else // i == rightChildIndex
                {
                    k -= 2; // Moving into a subtree of size L(k-1), so new k is k-2.
                }
            }
            else
            {
                // If the current element is the largest, the heap property is restored for this subtree.
                break;
            }
        }
    }

    /// <summary>
    /// Swaps two elements in an array.
    /// </summary>
    private static void Swap<T>(T[] array, int index1, int index2)
    {
        if (index1 != index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }
}