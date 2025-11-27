using System;

/// <summary>
/// Provides a method for performing Interpolation Search on a sorted array of integers.
/// </summary>
public static class InterpolationSearchAlgorithm
{
    /// <summary>
    /// Searches for a target value within a sorted integer array using the Interpolation Search algorithm.
    /// </summary>
    /// <param name="sortedArray">The sorted array of integers in ascending order to search.</param>
    /// <param name="target">The integer value to search for.</param>
    /// <returns>The index of the target element if found; otherwise, -1.</returns>
    public static int InterpolationSearch(int[] sortedArray, int target)
    {
        if (sortedArray == null || sortedArray.Length == 0)
        {
            return -1; // Handle empty or null array
        }

        int low = 0;
        int high = sortedArray.Length - 1;

        while (low <= high && target >= sortedArray[low] && target <= sortedArray[high])
        {
            // Handle case where low and high point to the same element to avoid division by zero
            if (low == high)
            {
                if (sortedArray[low] == target) return low;
                return -1;
            }

            // Calculate the probable position using linear interpolation formula
            // pos = low + ((target - sortedArray[low]) * (high - low) / (sortedArray[high] - sortedArray[low]))
            int pos = low + (int)((long)(target - sortedArray[low]) * (high - low) / (sortedArray[high] - sortedArray[low]));

            // Check if the target is found
            if (sortedArray[pos] == target)
                return pos;

            // If target is larger, target is in upper part
            if (sortedArray[pos] < target)
                low = pos + 1;
            // If target is smaller, target is in lower part
            else
                high = pos - 1;
        }

        // Element not found
        return -1;
    }
}
