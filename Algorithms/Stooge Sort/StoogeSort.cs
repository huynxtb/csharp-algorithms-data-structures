using System;
using System.Collections.Generic;

public static class StoogeSort
{
    public static void Sort<T>(IList<T> list) where T : IComparable<T>
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (list.Count <= 1)
        {
            return;
        }

        Sort(list, 0, list.Count - 1, Comparer<T>.Default);
    }

    public static void Sort<T>(IList<T> list, IComparer<T>? comparer)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (list.Count <= 1)
        {
            return;
        }

        Sort(list, 0, list.Count - 1, comparer ?? Comparer<T>.Default);
    }

    public static void Sort<T>(IList<T> list, Comparison<T> comparison)
    {
        if (comparison == null)
        {
            throw new ArgumentNullException(nameof(comparison));
        }

        Sort(list, Comparer<T>.Create(comparison));
    }

    public static void Sort<T>(IList<T> list, int index, int count) where T : IComparable<T>
    {
        Sort(list, index, count, Comparer<T>.Default);
    }

    public static void Sort<T>(IList<T> list, int index, int count, IComparer<T>? comparer)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be non-negative.");
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative.");
        }

        if (index + count > list.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Index and count exceed list bounds.");
        }

        if (count <= 1)
        {
            return;
        }

        Sort(list, index, index + count - 1, comparer ?? Comparer<T>.Default);
    }

    public static void Sort<T>(IList<T> list, int index, int count, Comparison<T> comparison)
    {
        if (comparison == null)
        {
            throw new ArgumentNullException(nameof(comparison));
        }

        Sort(list, index, count, Comparer<T>.Create(comparison));
    }

    public static void Sort<T>(Span<T> span) where T : IComparable<T>
    {
        if (span.Length <= 1)
        {
            return;
        }

        Sort(span, 0, span.Length - 1, Comparer<T>.Default);
    }

    public static void Sort<T>(Span<T> span, IComparer<T>? comparer)
    {
        if (span.Length <= 1)
        {
            return;
        }

        Sort(span, 0, span.Length - 1, comparer ?? Comparer<T>.Default);
    }

    public static void Sort<T>(Span<T> span, Comparison<T> comparison)
    {
        if (comparison == null)
        {
            throw new ArgumentNullException(nameof(comparison));
        }

        Sort(span, Comparer<T>.Create(comparison));
    }

    private static void Sort<T>(IList<T> list, int start, int end, IComparer<T> comparer)
    {
        if (comparer.Compare(list[start], list[end]) > 0)
        {
            (list[start], list[end]) = (list[end], list[start]);
        }

        int length = end - start + 1;
        if (length >= 3)
        {
            int third = length / 3;
            Sort(list, start, end - third, comparer);
            Sort(list, start + third, end, comparer);
            Sort(list, start, end - third, comparer);
        }
    }

    private static void Sort<T>(Span<T> span, int start, int end, IComparer<T> comparer)
    {
        if (comparer.Compare(span[start], span[end]) > 0)
        {
            (span[start], span[end]) = (span[end], span[start]);
        }

        int length = end - start + 1;
        if (length >= 3)
        {
            int third = length / 3;
            Sort(span, start, end - third, comparer);
            Sort(span, start + third, end, comparer);
            Sort(span, start, end - third, comparer);
        }
    }
}