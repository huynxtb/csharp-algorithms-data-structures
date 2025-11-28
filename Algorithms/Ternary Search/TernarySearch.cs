using System;

/// <summary>
/// Provides a static method to perform iterative ternary search on a sorted integer array.
/// </summary>
public static class TernarySearch
{
    /// <summary>
    /// Searches for a target value in a sorted array using iterative ternary search algorithm.
    /// </summary>
    /// <param name="arr">The sorted integer array to search.</param>
    /// <param name="target">The integer value to search for.</param>
    /// <returns>The zero-based index of the target if found; otherwise, -1.</returns>
    public static int Search(int[] arr, int target)
    {
        int left = 0;
        int right = arr.Length - 1;

        while (left <= right)
        {
            // Calculate the two mid points dividing the range into three parts
            int mid1 = left + (right - left) / 3;
            int mid2 = right - (right - left) / 3;

            if (arr[mid1] == target)
                return mid1;
            if (arr[mid2] == target)
                return mid2;

            if (target < arr[mid1])
            {
                // Target is in the first third
                right = mid1 - 1;
            }
            else if (target > arr[mid2])
            {
                // Target is in the third third
                left = mid2 + 1;
            }
            else
            {
                // Target is in the middle third
                left = mid1 + 1;
                right = mid2 - 1;
            }
        }

        // Element not found
        return -1;
    }
}