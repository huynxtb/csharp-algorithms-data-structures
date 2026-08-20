using System;

public static class InsertionSort
{
    public static void Sort<T>(T[] array) where T : IComparable<T>
    {
        if (array == null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        for (int i = 1; i < array.Length; i++)
        {
            T key = array[i];
            int j = i - 1;

            while (j >= 0 && array[j].CompareTo(key) > 0)
            { 
                array[j + 1] = array[j];
                j--;
            }
            array[j + 1] = key;
        }
    }
}