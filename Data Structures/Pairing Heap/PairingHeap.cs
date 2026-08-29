using System;
using System.Collections.Generic;

public class PairingHeapNode<T> where T : IComparable<T>
{
    public T Value { get; internal set; }
    public PairingHeapNode<T> Child { get; internal set; }
    public PairingHeapNode<T> Next { get; internal set; }
    public PairingHeapNode<T> Prev { get; internal set; }

    public PairingHeapNode(T value)
    {
        Value = value;
    }
}

public class PairingHeap<T> where T : IComparable<T>
{
    public PairingHeapNode<T> Root { get; private set; }
    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;

    public PairingHeap()
    {
        Root = null;
        Count = 0;
    }

    public PairingHeapNode<T> Insert(T value)
    {
        var newNode = new PairingHeapNode<T>(value);
        Root = Merge(Root, newNode);
        Count++;
        return newNode;
    }

    public T FindMin()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Heap is empty.");
        }
        return Root.Value;
    }

    public T DeleteMin()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Heap is empty.");
        }

        T minValue = Root.Value;
        var firstChild = Root.Child;

        if (Root != null)
        {
            Root.Child = null;
        }

        Root = CombineSiblings(firstChild);
        Count--;

        return minValue;
    }

    public void DecreaseKey(PairingHeapNode<T> node, T newValue)
    {
        if (node == null)
        {
            throw new ArgumentNullException(nameof(node));
        }
        if (newValue.CompareTo(node.Value) > 0)
        {
            throw new ArgumentException("New value is greater than current value.", nameof(newValue));
        }

        node.Value = newValue;

        if (node == Root)
        {
            return;
        }

        if (node.Next != null)
        {
            node.Next.Prev = node.Prev;
        }

        if (node.Prev != null)
        {
            if (node.Prev.Child == node)
            { 
                node.Prev.Child = node.Next;
            }
            else
            {
                node.Prev.Next = node.Next;
            }
        }

        node.Next = null;
        node.Prev = null;

        Root = Merge(Root, node);
    }

    public void Merge(PairingHeap<T> other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other));
        }
        if (other == this)
        {
            return;
        }

        Root = Merge(Root, other.Root);
        Count += other.Count;

        other.Root = null;
        other.Count = 0;
    }

    public void Clear()
    {
        Root = null;
        Count = 0;
    }

    private PairingHeapNode<T> Merge(PairingHeapNode<T> node1, PairingHeapNode<T> node2)
    {
        if (node1 == null) return node2;
        if (node2 == null) return node1;

        if (node1.Value.CompareTo(node2.Value) <= 0)
        {
            node2.Prev = node1;
            node2.Next = node1.Child;
            if (node1.Child != null)
            { 
                node1.Child.Prev = node2;
            }
            node1.Child = node2;
            return node1;
        }
        else
        {
            node1.Prev = node2;
            node1.Next = node2.Child;
            if (node2.Child != null)
            {
                node2.Child.Prev = node1;
            }
            node2.Child = node1;
            return node2;
        }
    }

    private PairingHeapNode<T> CombineSiblings(PairingHeapNode<T> firstSibling)
    {
        if (firstSibling == null) return null;

        var siblings = new List<PairingHeapNode<T>>();
        var current = firstSibling;
        while (current != null)
        { 
            var next = current.Next;
            current.Next = null;
            current.Prev = null;
            siblings.Add(current);
            current = next;
        }

        int count = siblings.Count;
        if (count == 1) return siblings[0];

        var treeArray = new PairingHeapNode<T>[count];
        for (int i = 0; i < count; i++)
        {
            treeArray[i] = siblings[i];
        }

        int j = 0;
        for (; j + 1 < count; j += 2)
        {
            treeArray[j] = Merge(treeArray[j], treeArray[j + 1]);
        }

        int last = (count % 2 == 0) ? j - 2 : count - 1;
        var result = treeArray[last];
        for (int k = last - 2; k >= 0; k -= 2)
        {
            result = Merge(treeArray[k], result);
        }

        return result;
    }
}