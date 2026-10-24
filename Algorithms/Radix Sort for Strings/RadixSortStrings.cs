using System;
using System.Collections.Generic;

public class RadixSortStrings
{
    public void Sort(string[] arr)
    {
        if (arr == null || arr.Length == 0)
            return;

        int maxLength = GetMaxLength(arr);

        for (int pos = maxLength - 1; pos >= 0; pos--)
        {
            CountingSort(arr, pos);
        }
    }

    private int GetMaxLength(string[] arr)
    {
        int maxLength = 0;
        foreach (var str in arr)
        {
            if (str.Length > maxLength)
                maxLength = str.Length;
        }
        return maxLength;
    }

    private void CountingSort(string[] arr, int pos)
    {
        int[] count = new int[256]; // Assuming ASCII characters
        string[] output = new string[arr.Length];

        // Initialize count array
        for (int i = 0; i < arr.Length; i++)
        {
            int index = GetCharAtPosition(arr[i], pos);
            count[index]++;
        }

        // Change count[i] so that it contains actual position of this character in output[]
        for (int i = 1; i < count.Length; i++)
        {
            count[i] += count[i - 1];
        }

        // Build the output array
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            int index = GetCharAtPosition(arr[i], pos);
            output[count[index] - 1] = arr[i];
            count[index]--;
        }

        // Copy the output array to arr[]
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = output[i];
        }
    }

    private int GetCharAtPosition(string str, int pos)
    {
        if (pos < str.Length)
            return (int)str[pos];
        return 0; // Return 0 for characters that do not exist (for shorter strings)
    }
}