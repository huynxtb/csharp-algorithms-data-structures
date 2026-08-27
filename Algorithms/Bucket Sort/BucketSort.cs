using System;
using System.Collections.Generic;

/// <summary>
/// Provides an implementation of the Bucket Sort algorithm for floating-point numbers in the range [0, 1).
/// </summary>
public static class BucketSort
{
    /// <summary>
    /// Sorts an array of double-precision floating-point numbers in the range [0, 1) in-place using Bucket Sort.
    /// </summary>
    /// <param name="array">The array of double-precision floating-point numbers to sort.</param>
    /// <exception cref="ArgumentNullException">Thrown when the input array is null.</exception>
    /// <exception cref="ArgumentException">Thrown when any element in the array is outside the range [0, 1).</exception>
    public static void Sort(double[] array)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array), "Array cannot be null.");
        }

        if (array.Length <= 1)
        {
            return;
        }

        int n = array.Length;
        List<double>[] buckets = new List<double>[n];

        for (int i = 0; i < n; i++)
        {
            buckets[i] = new List<double>();
        }

        for (int i = 0; i < n; i++)
        {
            double val = array[i];
            if (val < 0.0 || val >= 1.0)
            {
                throw new ArgumentException("All elements must be in the range [0, 1).", nameof(array));
            }

            int bucketIndex = (int)Math.Floor(n * val);
            buckets[bucketIndex].Add(val);
        }

        for (int i = 0; i < n; i++)
        {
            InsertionSort(buckets[i]);
        }

        int index = 0;
        for (int i = 0; i < n; i++)
        {
            List<double> bucket = buckets[i];
            for (int j = 0; j < bucket.Count; j++)
            { 
                array[index++] = bucket[j];
            }
        }
    }

    /// <summary>
    /// Sorts a list of double-precision floating-point numbers in-place using a stable Insertion Sort algorithm.
    /// </summary>
    /// <param name="bucket">The list of double-precision floating-point numbers to sort.</param>
    private static void InsertionSort(List<double> bucket)
    {
        int count = bucket.Count;
        for (int i = 1; i < count; i++)
        {
            double key = bucket[i];
            int j = i - 1;

            while (j >= 0 && bucket[j] > key)
            {
                bucket[j + 1] = bucket[j];
                j--;
            }
            bucket[j + 1] = key;
        }
    }
}