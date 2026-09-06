public static class LinearInterpolationSearch
{
    /// <summary>
    /// Searches for a key in a sorted integer array using Linear Interpolation Search.
    /// </summary>
    /// <param name="array">Sorted array of integers.</param>
    /// <param name="key">Target integer value to search for.</param>
    /// <returns>Index of key if found; otherwise, -1.</returns>
    public static int Search(int[] array, int key)
    {
        if (array == null || array.Length == 0)
        {
            return -1;
        }

        int low = 0;
        int high = array.Length - 1;

        while (low <= high && key >= array[low] && key <= array[high])
        {
            if (low == high)
            {
                if (array[low] == key)
                {
                    return low;
                }
                return -1;
            }

            if (array[high] == array[low])
            {
                if (array[low] == key)
                {
                    return low;
                }
                return -1;
            }

            long posRatio = (long)(key - array[low]) * (high - low);
            int pos = low + (int)(posRatio / (array[high] - array[low]));

            if (pos < low || pos > high)
            {
                return -1;
            }

            if (array[pos] == key)
            {
                return pos;
            }

            if (array[pos] < key)
            {
                low = pos + 1;
            }
            else
            {
                high = pos - 1;
            }
        }

        return -1;
    }
}