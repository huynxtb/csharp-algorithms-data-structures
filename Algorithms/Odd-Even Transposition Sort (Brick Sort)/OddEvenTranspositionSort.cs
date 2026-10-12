using System;
using System.Collections.Generic;

/// <summary>
/// Odd-Even Transposition Sort (Brick Sort) implementation.
/// This is a comparison-based sorting algorithm that alternates between
/// odd and even phases of adjacent element comparisons and swaps.
/// It is particularly well-suited for parallel processing architectures.
/// </summary>
public static class OddEvenTranspositionSort
{
    /// <summary>
    /// Sorts an array using the Odd-Even Transposition Sort algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to sort in-place.</param>
    /// <param name="comparer">The comparer to use for element comparison. If null, uses Comparer&lt;T&gt;.Default.</param>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    /// <remarks>
    /// Time Complexity: O(n²) in worst and average case, O(n) in best case.
    /// Space Complexity: O(1) - sorts in-place with no auxiliary data structures.
    /// The algorithm alternates between odd-indexed and even-indexed adjacent pair comparisons
    /// until a complete pass (both phases) produces no swaps.
    /// </remarks>
    public static void Sort<T>(T[] array, IComparer<T> comparer = null)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (array.Length <= 1)
            return;

        comparer = comparer ?? Comparer<T>.Default;

        bool swapped = true;
        while (swapped)
        {
            swapped = false;

            // Odd phase: compare all (odd, even) indexed pairs
            for (int i = 1; i < array.Length - 1; i += 2)
            {
                if (comparer.Compare(array[i], array[i + 1]) > 0)
                {
                    Swap(ref array[i], ref array[i + 1]);
                    swapped = true;
                }
            }

            // Even phase: compare all (even, odd) indexed pairs
            for (int i = 0; i < array.Length - 1; i += 2)
            {
                if (comparer.Compare(array[i], array[i + 1]) > 0)
                {
                    Swap(ref array[i], ref array[i + 1]);
                    swapped = true;
                }
            }
        }
    }

    /// <summary>
    /// Sorts an array using the Odd-Even Transposition Sort algorithm with a custom comparison delegate.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to sort in-place.</param>
    /// <param name="comparison">A comparison delegate that defines the sort order.</param>
    /// <exception cref="ArgumentNullException">Thrown when array or comparison is null.</exception>
    /// <remarks>
    /// Time Complexity: O(n²) in worst and average case, O(n) in best case.
    /// Space Complexity: O(1) - sorts in-place with no auxiliary data structures.
    /// </remarks>
    public static void Sort<T>(T[] array, Comparison<T> comparison)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (comparison == null)
            throw new ArgumentNullException(nameof(comparison));

        Sort(array, Comparer<T>.Create(comparison));
    }

    /// <summary>
    /// Sorts an array in descending order using the Odd-Even Transposition Sort algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to sort in-place.</param>
    /// <param name="comparer">The comparer to use for element comparison. If null, uses Comparer&lt;T&gt;.Default.</param>
    /// <exception cref="ArgumentNullException">Thrown when array is null.</exception>
    /// <remarks>
    /// Time Complexity: O(n²) in worst and average case, O(n) in best case.
    /// Space Complexity: O(1) - sorts in-place with no auxiliary data structures.
    /// Sorts in descending order by inverting the comparison logic.
    /// </remarks>
    public static void SortDescending<T>(T[] array, IComparer<T> comparer = null)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        if (array.Length <= 1)
            return;

        comparer = comparer ?? Comparer<T>.Default;
        var invertedComparer = Comparer<T>.Create((a, b) => comparer.Compare(b, a));

        Sort(array, invertedComparer);
    }

    /// <summary>
    /// Swaps two elements in-place.
    /// </summary>
    /// <typeparam name="T">The type of elements to swap.</typeparam>
    /// <param name="a">The first element reference.</param>
    /// <param name="b">The second element reference.</param>
    private static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }
}