using System;
using System.Linq;
using System.Collections.Generic;

public class QuickselectAlgorithm
{
    public static T FindKthSmallest<T>(T[] arr, int k) where T : IComparable<T>
    {
        if (k < 1 || k > arr.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(k), "k must be between 1 and the length of the array.");
        }
        return Quickselect(arr, 0, arr.Length - 1, k - 1);
    }

    private static T Quickselect<T>(T[] arr, int left, int right, int k) where T : IComparable<T>
    {
        if (left == right)
        {
            return arr[left];
        }

        int pivotIndex = new Random().Next(left, right + 1);
        pivotIndex = Partition(arr, left, right, pivotIndex);

        if (k == pivotIndex)
        {
            return arr[k];
        }
        else if (k < pivotIndex)
        {
            return Quickselect(arr, left, pivotIndex - 1, k);
        }
        else
        {
            return Quickselect(arr, pivotIndex + 1, right, k);
        }
    }

    private static int Partition<T>(T[] arr, int left, int right, int pivotIndex) where T : IComparable<T>
    {
        T pivotValue = arr[pivotIndex];
        Swap(arr, pivotIndex, right);
        int storeIndex = left;

        for (int i = left; i < right; i++)
        {
            if (arr[i].CompareTo(pivotValue) < 0)
            {
                Swap(arr, storeIndex, i);
                storeIndex++;
            }
        }
        Swap(arr, storeIndex, right);
        return storeIndex;
    }

    private static void Swap<T>(T[] arr, int i, int j)
    {
        T temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}