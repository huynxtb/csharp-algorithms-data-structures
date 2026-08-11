using System;
using System.Collections.Generic;

public class MonotonePriorityQueue<T> where T : IComparable<T>
{
    private List<T> queue = new List<T>();
    private bool isIncreasing;

    public MonotonePriorityQueue(bool isIncreasing = true)
    {
        this.isIncreasing = isIncreasing;
    }

    public void Enqueue(T item)
    {
        if (queue.Count == 0)
        {
            queue.Add(item);
        }
        else
        {
            if ((isIncreasing && item.CompareTo(queue[queue.Count - 1]) >= 0) ||
                (!isIncreasing && item.CompareTo(queue[queue.Count - 1]) <= 0))
            {
                queue.Add(item);
            }
            else
            {
                throw new InvalidOperationException("Item does not maintain the monotone property");
            }
        }
    }

    public T Dequeue()
    {
        if (queue.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        T item = queue[0];
        queue.RemoveAt(0);
        return item;
    }

    public T Peek()
    {
        if (queue.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty");
        }
        return queue[0];
    }

    public bool IsEmpty()
    {
        return queue.Count == 0;
    }
}