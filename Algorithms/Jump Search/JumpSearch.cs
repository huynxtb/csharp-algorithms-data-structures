using System;

public class JumpSearch
{
    public static int Search(int[] arr, int target)
    {
        int n = arr.Length;
        int blockSize = (int)Math.Sqrt(n);
        int prev = 0;

        // Jump ahead by blockSize
        while (arr[Math.Min(blockSize, n) - 1] < target)
        {
            prev = blockSize;
            blockSize += (int)Math.Sqrt(n);
            if (prev >= n)
                return -1;
        }

        // Linear search in the previous block
        for (int i = prev; i < Math.Min(blockSize, n); i++)
        {
            if (arr[i] == target)
                return i;
        }

        return -1; // Target not found
    }
}