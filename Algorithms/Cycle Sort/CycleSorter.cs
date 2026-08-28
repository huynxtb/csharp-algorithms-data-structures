using System;

/// <summary>
/// Provides an implementation of the Cycle Sort algorithm.
/// </summary>
public static class CycleSorter
{
    /// <summary>
    /// Sorts the specified array in-place using the Cycle Sort algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array, which must implement IComparable.</typeparam>
    /// <param name="array">The array to be sorted.</param>
    /// <exception cref="ArgumentNullException">Thrown when the array is null.</exception>
    /// <remarks>
    /// Time Complexity: O(n^2)
    /// Auxiliary Space Complexity: O(1)
    /// </remarks>
    public static void Sort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        int n = array.Length;
        if (n <= 1)
        {
            return;
        }

        for (int cycleStart = 0; cycleStart < n - 1; cycleStart++)
        {
            T item = array[cycleStart];
            int pos = cycleStart;

            for (int i = cycleStart + 1; i < n; i++)
            { 
                if (array[i].CompareTo(item) < 0)
                {
                    pos++;
                }
            }

            if (pos == cycleStart)
            {
                continue;
            }

            while (pos < n && item.CompareTo(array[pos]) == 0)
            {
                pos++;
            }

            if (pos != cycleStart)
            {
                T temp = array[pos];
                array[pos] = item;
                item = temp;
            }

            while (pos != cycleStart)
            {
                pos = cycleStart;
                for (int i = cycleStart + 1; i < n; i++)
                {
                    if (array[i].CompareTo(item) < 0)
                    {
                        pos++;
                    }
                }

                while (pos < n && item.CompareTo(array[pos]) == 0)
                {
                    pos++;
                }

                if (item.CompareTo(array[pos]) != 0)
                {
                    T temp = array[pos];
                    array[pos] = item;
                    item = temp;
                }
            }
        }
    }
}