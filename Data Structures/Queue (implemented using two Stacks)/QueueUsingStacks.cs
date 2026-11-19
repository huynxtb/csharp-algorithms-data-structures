using System;
using System.Collections.Generic;

public class Queue<T>
{
    private Stack<T> stack1 = new Stack<T>();
    private Stack<T> stack2 = new Stack<T>();

    public void Enqueue(T item)
    {
        stack1.Push(item);
    }

    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Queue is empty.");

        if (stack2.Count == 0)
        {
            while (stack1.Count > 0)
            {
                stack2.Push(stack1.Pop());
            }
        }

        return stack2.Pop();
    }

    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Queue is empty.");

        if (stack2.Count == 0)
        {
            while (stack1.Count > 0)
            {
                stack2.Push(stack1.Pop());
            }
        }

        return stack2.Peek();
    }

    public int Count
    {
        get { return stack1.Count + stack2.Count; }
    }

    public bool IsEmpty
    {
        get { return Count == 0; }
    }
}