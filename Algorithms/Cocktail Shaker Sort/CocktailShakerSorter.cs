using System;
using System.Collections.Generic;

public static class CocktailShakerSorter
{
    /// <summary>
    /// Sorts an IList of elements in ascending order using the Cocktail Shaker Sort algorithm.
    /// Elements must implement IComparable&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The IList to sort.</param>
    public static void Sort<T>(IList<T> list) where T : IComparable<T>
    {
        if (list == null || list.Count <= 1)
        {
            return;
        }

        Sort(list, Comparer<T>.Default);
    }

    /// <summary>
    /// Sorts an IList of elements in ascending order using the Cocktail Shaker Sort algorithm
    /// and a specified IComparer&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The IList to sort.</param>
    /// <param name="comparer">The IComparer&lt;T&gt; to use for comparisons.</param>
    public static void Sort<T>(IList<T> list, IComparer<T> comparer)
    {
        if (list == null || list.Count <= 1)
        {
            return;
        }

        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer), "Comparer cannot be null.");
        }

        int left = 0;
        int right = list.Count - 1;
        bool swapped;

        do
        {
            swapped = false;

            // Forward pass: push largest element to the right
            for (int i = left; i < right; i++)
            {
                if (comparer.Compare(list[i], list[i + 1]) > 0)
                {
                    Swap(list, i, i + 1);
                    swapped = true;
                }
            }

            // If no elements were swapped, the list is sorted
            if (!swapped)
            {
                break;
            }

            // Decrease the right bound as the largest element is now in place
            right--;

            swapped = false; // Reset for the backward pass

            // Backward pass: push smallest element to the left
            for (int i = right; i > left; i--)
            {
                if (comparer.Compare(list[i], list[i - 1]) < 0)
                {
                    Swap(list, i, i - 1);
                    swapped = true;
                }
            }

            // Increase the left bound as the smallest element is now in place
            left++;

        } while (swapped && left <= right);
    }

    /// <summary>
    /// Swaps two elements in an IList.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The IList containing the elements.</param>
    /// <param name="indexA">The index of the first element.</param>
    /// <param name="indexB">The index of the second element.</param>
    private static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T temp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = temp;
    }
}