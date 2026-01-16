public static class CountingSort
{
    public static int[] Sort(int[] array)
    {
        if (array == null || array.Length == 0)
            return new int[0];

        // Find the maximum value in the array to determine the range of counts
        int max = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > max)
                max = array[i];
        }

        // Initialize count array with size max+1 (to include max value index)
        int[] count = new int[max + 1];

        // Count the occurrences of each value
        for (int i = 0; i < array.Length; i++)
        {
            count[array[i]]++;
        }

        // Reconstruct the sorted array
        int[] sorted = new int[array.Length];
        int index = 0;
        for (int value = 0; value <= max; value++)
        {
            while (count[value] > 0)
            {
                sorted[index++] = value;
                count[value]--;
            }
        }

        return sorted;
    }
}