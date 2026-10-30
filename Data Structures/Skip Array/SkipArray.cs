using System;
using System.Collections;
using System.Collections.Generic;

public class SkipArray<T> : IEnumerable<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Node Next { get; set; }
        public Node Down { get; set; }
        public int Span { get; set; } // Number of nodes skipped by the 'Next' pointer

        public Node(T value)
        {
            Value = value;
            Next = null;
            Down = null;
            Span = 1;
        }
    }

    private Node head;
    private int count;
    private Random random;
    private const double PROBABILITY = 0.5;

    public SkipArray()
    {
        head = new Node(default(T)); // Sentinel node
        count = 0;
        random = new Random();
    }

    public int Count
    {
        get { return count; }
    }

    public bool IsEmpty
    {
        get { return count == 0; }
    }

    public void Add(T item)
    {
        var update = new List<Node>();
        var current = head;

        // Find insertion points at each level
        while (current != null)
        {
            while (current.Next != null && Comparer<T>.Default.Compare(current.Next.Value, item) < 0)
            {
                current = current.Next;
            }
            update.Add(current);
            current = current.Down;
        }

        // Determine the level for the new node
        int level = 0;
        while (random.NextDouble() < PROBABILITY)
        {
            level++;
        }

        // Add new levels if necessary
        while (level >= update.Count)
        {
            var newNode = new Node(default(T));
            newNode.Down = head;
            head = newNode;
            update.Insert(0, head);
        }

        Node newNodeDown = null;
        // Insert the new node at each level up to its determined level
        for (int i = update.Count - 1; i >= update.Count - 1 - level; i--)
        {
            var newNode = new Node(item);
            newNode.Next = update[i].Next;
            update[i].Next = newNode;
            newNode.Down = newNodeDown;
            newNode.Span = (update[i].Next != null ? update[i].Next.Span : 0) - (newNode.Next != null ? newNode.Next.Span : 0) + 1;
            if (update[i].Next != null) update[i].Next.Span = 1;
            newNodeDown = newNode;
        }

        // Adjust spans for nodes that were skipped over
        for (int i = 0; i < update.Count - 1 - level; i++)
        {
            update[i].Span++;
        }

        count++;
    }

    public bool Remove(T item)
    {
        var update = new List<Node>();
        var current = head;
        bool found = false;

        // Find the node to remove and record predecessors
        while (current != null)
        {
            while (current.Next != null && Comparer<T>.Default.Compare(current.Next.Value, item) < 0)
            {
                current = current.Next;
            }
            update.Add(current);
            current = current.Down;
        }

        // Check if the item exists at the lowest level
        if (update.Count > 0 && update[update.Count - 1].Next != null && Comparer<T>.Default.Compare(update[update.Count - 1].Next.Value, item) == 0)
        {
            found = true;
            var nodeToRemove = update[update.Count - 1].Next;

            // Remove the node from each level it exists in
            for (int i = update.Count - 1; i >= 0; i--)
            {
                if (update[i].Next != null && Comparer<T>.Default.Compare(update[i].Next.Value, item) == 0)
                {
                    update[i].Span += update[i].Next.Span - 1;
                    update[i].Next = update[i].Next.Next;
                }
                else
                {
                    // If the node wasn't at this level, but we passed it, decrement span
                    if (update[i].Next != null && Comparer<T>.Default.Compare(update[i].Next.Value, item) > 0)
                    {
                        update[i].Span--;
                    }
                }
            }
            count--;

            // Remove empty levels from the top
            while (head.Next == null && head.Down != null)
            {
                head = head.Down;
            }
        }

        return found;
    }

    public bool Contains(T item)
    {
        var current = head;
        while (current != null)
        {
            while (current.Next != null && Comparer<T>.Default.Compare(current.Next.Value, item) < 0)
            {
                current = current.Next;
            }
            if (current.Next != null && Comparer<T>.Default.Compare(current.Next.Value, item) == 0)
            {
                return true;
            }
            current = current.Down;
        }
        return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = head;
        // Traverse down to the lowest level
        while (current.Down != null)
        {
            current = current.Down;
        }
        // Traverse the lowest level
        current = current.Next;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}