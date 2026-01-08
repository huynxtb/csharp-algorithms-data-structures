public class JumpSearchAlgorithm
{
    public static int JumpSearch(int[] sortedArray, int target)
    {
        int length = sortedArray.Length;
        if (length == 0) return -1;

        int step = (int)System.Math.Floor(System.Math.Sqrt(length));
        int prev = 0;

        // Finding the block where element is present
        while (sortedArray[Math.Min(step, length) - 1] < target)
        {
            prev = step;
            step += (int)System.Math.Floor(System.Math.Sqrt(length));
            if (prev >= length) return -1;
        }

        // Linear search backward within the block
        for (int i = prev; i < System.Math.Min(step, length); i++)
        {
            if (sortedArray[i] == target)
                return i;
        }

        return -1;
    }
}