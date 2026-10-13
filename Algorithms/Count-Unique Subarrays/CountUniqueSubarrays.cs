using System;
using System.Collections.Generic;

public class CountUniqueSubarrays
{
    /// <summary>
    /// Counts the number of unique contiguous subarrays in the given integer array.
    /// A subarray is considered unique based on its sequence of elements.
    /// </summary>
    /// <param name="array">The input array of integers.</param>
    /// <returns>The count of unique contiguous subarrays.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input array is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the input array is empty.</exception>
    public static int Count(int[] array)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array), "Input array must not be null.");

        if (array.Length == 0)
            throw new ArgumentException("Input array must contain at least one element.", nameof(array));

        var uniqueSubarrays = new HashSet<string>();

        for (int start = 0; start < array.Length; start++)
        {
            var current = new System.Text.StringBuilder();

            for (int end = start; end < array.Length; end++)
            {
                if (end > start)
                    current.Append(',');

                current.Append(array[end]);

                uniqueSubarrays.Add(current.ToString());
            }
        }

        return uniqueSubarrays.Count;
    }

    /// <summary>
    /// Counts the number of unique contiguous subarrays and also returns them.
    /// </summary>
    /// <param name="array">The input array of integers.</param>
    /// <param name="uniqueSubarrayList">Output list of all unique subarrays as string representations.</param>
    /// <returns>The count of unique contiguous subarrays.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input array is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the input array is empty.</exception>
    public static int CountWithDetails(int[] array, out List<string> uniqueSubarrayList)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array), "Input array must not be null.");

        if (array.Length == 0)
            throw new ArgumentException("Input array must contain at least one element.", nameof(array));

        var uniqueSubarrays = new HashSet<string>();

        for (int start = 0; start < array.Length; start++)
        {
            var current = new System.Text.StringBuilder();

            for (int end = start; end < array.Length; end++)
            {
                if (end > start)
                    current.Append(',');

                current.Append(array[end]);

                uniqueSubarrays.Add(current.ToString());
            }
        }

        uniqueSubarrayList = new List<string>(uniqueSubarrays);
        return uniqueSubarrays.Count;
    }
}