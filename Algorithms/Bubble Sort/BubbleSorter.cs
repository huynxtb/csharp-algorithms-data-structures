using System;

public static class BubbleSorter
{
    public static void Sort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null || array.Length <= 1)
        {
            return;
        }

        int n = array.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (array[j].CompareTo(array[j + 1]) > 0)
                {
                    T temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                    swapped = true;
                }
            }
            if (!swapped)
            {
                break;
            }
        }
    }
}