using System;
using System.Collections.Generic;

public class SkipArray
{
    private int[] array;
    private int count;
    private int capacity;
    private const double LoadFactor = 0.75;

    public SkipArray(int initialCapacity = 4)
    {
        capacity = initialCapacity;
        array = new int[capacity];
        count = 0;
    }

    public void Insert(int value)
    {
        if (count >= capacity * LoadFactor)
        {
            Resize();
        }
        int index = Array.BinarySearch(array, 0, count, value);
        if (index >= 0)
        {
            return; // Value already exists
        }
        index = ~index; // Get the index where the value should be inserted
        Array.Copy(array, index, array, index + 1, count - index);
        array[index] = value;
        count++;
    }

    public void Delete(int value)
    {
        int index = Array.BinarySearch(array, 0, count, value);
        if (index < 0)
        {
            return; // Value not found
        }
        Array.Copy(array, index + 1, array, index, count - index - 1);
        count--;
    }

    public bool Search(int value)
    {
        int index = Array.BinarySearch(array, 0, count, value);
        return index >= 0;
    }

    private void Resize()
    {
        capacity *= 2;
        int[] newArray = new int[capacity];
        Array.Copy(array, newArray, count);
        array = newArray;
    }
}