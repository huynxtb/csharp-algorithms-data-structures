using System;

public class Quickselect
{
    public static int Select(int[] arr, int k)
    {
        if (arr == null || arr.Length == 0 || k < 1 || k > arr.Length)
        {
            throw new ArgumentOutOfRangeException("k is out of bounds.");
        }
        return QuickSelect(arr, 0, arr.Length - 1, k - 1);
    }

    private static int QuickSelect(int[] arr, int left, int right, int k)
    {
        if (left == right)
        {
            return arr[left];
        }

        int pivotIndex = Partition(arr, left, right);

        if (k == pivotIndex)
        {
            return arr[k];
        }
        else if (k < pivotIndex)
        {
            return QuickSelect(arr, left, pivotIndex - 1, k);
        }
        else
        {
            return QuickSelect(arr, pivotIndex + 1, right, k);
        }
    }

    private static int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[right];
        int i = left;

        for (int j = left; j < right; j++)
        {
            if (arr[j] < pivot)
            {
                Swap(arr, i, j);
                i++;
            }
        }
        Swap(arr, i, right);
        return i;
    }

    private static void Swap(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }
}