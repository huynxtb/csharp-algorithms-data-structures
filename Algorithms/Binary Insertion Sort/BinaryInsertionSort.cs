using System;

/// <summary>
/// Provides a method to sort an array of integers using the Binary Insertion Sort algorithm.
/// </summary>
public static class BinaryInsertionSort
{
    /// <summary>
    /// Sorts the input array in ascending order using the Binary Insertion Sort algorithm.
    /// </summary>
    /// <param name="array">The array of integers to sort.</param>
    public static void Sort(int[] array)
    {
        int n = array.Length;
        // Start from the second element because the first element is trivially "sorted"
        for (int i = 1; i < n; i++)
        {
            int key = array[i];
            // Find the correct insertion position for array[i] in the sorted portion array[0..i-1]
            int insertPos = BinarySearch(array, key, 0, i - 1);

            // Shift elements to the right to make space for the key
            int j = i - 1;
            while (j >= insertPos)
            {
                array[j + 1] = array[j];
                j--;
            }

            // Insert the key at its correct position
            array[insertPos] = key;
        }
    }

    /// <summary>
    /// Uses binary search to find the index where the given key should be inserted in the sorted subarray.
    /// </summary>
    /// <param name="array">The array containing the sorted subarray.</param>
    /// <param name="key">The value to insert.</param>
    /// <param name="low">The starting index of the sorted subarray.</param>
    /// <param name="high">The ending index of the sorted subarray.</param>
    /// <returns>The index where the key should be inserted.</returns>
    private static int BinarySearch(int[] array, int key, int low, int high)
    {
        while (low <= high)
        {
            int mid = low + (high - low) / 2;
            if (array[mid] == key)
            {
                // If an equal element is found, we decide to insert after it for stable sorting
                return mid + 1;
            }
            else if (array[mid] < key)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }
        // If not found, low is the insertion point
        return low;
    }
}