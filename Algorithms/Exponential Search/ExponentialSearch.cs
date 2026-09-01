using System;

public static class ExponentialSearch
{
    /// <summary>
    /// Searches for a specified value in a sorted array using the Exponential Search algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array, which must implement IComparable&lt;T&gt;.</typeparam>
    /// <param name="array">The sorted array to search within.</param>
    /// <param name="value">The value to search for.</param>
    /// <returns>The zero-based index of the value if found; otherwise, -1.</returns>
    public static int Search<T>(T[] array, T value) where T : IComparable<T>
    {
        if (array == null || array.Length == 0)
        {
            return -1; // Handle null or empty array
        }

        // Check if the value is smaller than the first element
        // or if it's the first element itself.
        if (array[0].CompareTo(value) > 0)
        {
            return -1;
        }
        if (array[0].CompareTo(value) == 0)
        {
            return 0;
        }

        // Find the range for binary search by repeatedly doubling the index
        int bound = 1;
        while (bound < array.Length && array[bound].CompareTo(value) < 0)
        {
            bound *= 2;
        }

        // Perform binary search within the found range [bound/2, min(bound, array.Length - 1)]
        int left = bound / 2;
        int right = Math.Min(bound, array.Length - 1);

        return BinarySearch(array, value, left, right);
    }

    /// <summary>
    /// Performs a standard binary search within a specified range of a sorted array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array, which must implement IComparable&lt;T&gt;.</typeparam>
    /// <param name="array">The sorted array to search within.</param>
    /// <param name="value">The value to search for.</param>
    /// <param name="left">The starting index of the range to search.</param>
    /// <param name="right">The ending index of the range to search.</param>
    /// <returns>The zero-based index of the value if found; otherwise, -1.</returns>
    private static int BinarySearch<T>(T[] array, T value, int left, int right) where T : IComparable<T>
    {
        while (left <= right)
        {
            int mid = left + (right - left) / 2; // To prevent potential overflow

            int comparisonResult = array[mid].CompareTo(value);

            if (comparisonResult == 0)
            {
                return mid; // Value found
            }
            else if (comparisonResult < 0)
            {
                left = mid + 1; // Value is in the right half
            }
            else
            {
                right = mid - 1; // Value is in the left half
            }
        }
        return -1; // Value not found
    }
}