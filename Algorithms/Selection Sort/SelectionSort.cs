using System;
using System.Collections.Generic;
using System.Collections;

public static class SortingAlgorithms
{
    public static void SelectionSort<T>(IList<T> list) where T : IComparable<T>
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[j].CompareTo(list[minIndex]) < 0)
                {
                    minIndex = j;
                }
            }
            if (minIndex != i)
            {
                T temp = list[i];
                list[i] = list[minIndex];
                list[minIndex] = temp;
            }
        }
    }
}