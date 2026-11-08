using System;

public class MergeableHeap<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value { get; set; }
        public int Rank { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(T value)
        {
            Value = value;
            Rank = 0;
            Left = null;
            Right = null;
        }
    }

    private Node root;
    private int count;

    public MergeableHeap()
    {
        root = null;
        count = 0;
    }

    public bool IsEmpty => count == 0;

    public int Count => count;

    public T FindMin()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Heap is empty.");
        }
        return root.Value;
    }

    public void Insert(T value)
    {
        var newNode = new Node(value);
        root = Merge(root, newNode);
        count++;
    }

    public T DeleteMin()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Heap is empty.");
        }

        T minValue = root.Value;
        root = Merge(root.Left, root.Right);
        count--;
        return minValue;
    }

    public void Merge(MergeableHeap<T> otherHeap)
    {
        if (otherHeap == null || otherHeap.IsEmpty)
        {
            return;
        }

        root = Merge(root, otherHeap.root);
        count += otherHeap.count;

        // Clear the other heap to prevent accidental reuse of its nodes
        otherHeap.root = null;
        otherHeap.count = 0;
    }

    private Node Merge(Node h1, Node h2)
    {
        if (h1 == null)
        {
            return h2;
        }
        if (h2 == null)
        {
            return h1;
        }

        // Ensure h1 has the smaller root value
        if (h1.Value.CompareTo(h2.Value) > 0)
        {
            var temp = h1;
            h1 = h2;
            h2 = temp;
        }

        // Recursively merge h2 with the right child of h1
        h1.Right = Merge(h1.Right, h2);

        // Maintain the heap property by swapping children if necessary
        if (h1.Left == null || (h1.Right != null && h1.Right.Rank > h1.Left.Rank))
        {
            var temp = h1.Left;
            h1.Left = h1.Right;
            h1.Right = temp;
        }

        // Update the rank of h1
        h1.Rank = (h1.Right == null ? 0 : h1.Right.Rank) + 1;

        return h1;
    }
}