using System;
using System.Collections.Generic;

public static class ShellSorter
{
    public static void Sort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        Sort(array, Comparer<T>.Default);
    }

    public static void Sort<T>(T[] array, IComparer<T> comparer)
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        int n = array.Length;
        if (n <= 1)
        {
            return;
        }

        int h = 1;
        while (h < n / 3)
        {
            h = 3 * h + 1;
        }

        while (h >= 1)
        {
            for (int i = h; i < n; i++)
            { 
                T temp = array[i];
                int j = i;
                while (j >= h && comparer.Compare(array[j - h], temp) > 0)
                { 
                    array[j] = array[j - h];
                    j -= h;
                }
                array[j] = temp;
            }
            h /= 3;
        }
    }
}