using System;
using System.Collections.Generic;

public class SelfAdjustingList<T>
{
    private LinkedList<T> list;
    private HashSet<T> set;

    public SelfAdjustingList()
    {
        list = new LinkedList<T>();
        set = new HashSet<T>();
    }

    public void Insert(T value)
    {
        if (set.Contains(value)) return; // Prevent duplicates
        list.AddFirst(value);
        set.Add(value);
    }

    public bool Access(T value)
    {
        if (!set.Contains(value)) return false; // Element not found
        list.Remove(value);
        list.AddFirst(value); // Move accessed element to front
        return true;
    }

    public bool Remove(T value)
    {
        if (!set.Contains(value)) return false; // Element not found
        list.Remove(value);
        set.Remove(value);
        return true;
    }

    public void PrintList()
    {
        foreach (var item in list)
        {
            Console.Write(item + " ");
        }
        Console.WriteLine();
    }
}