using System;

public class BinomialHeapNode<TKey, TValue> where TKey : IComparable<TKey>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }
    public int Degree { get; set; }
    public BinomialHeapNode<TKey, TValue> Parent { get; set; }
    public BinomialHeapNode<TKey, TValue> Child { get; set; }
    public BinomialHeapNode<TKey, TValue> Sibling { get; set; }

    public BinomialHeapNode(TKey key, TValue value)
    {
        Key = key;
        Value = value;
        Degree = 0;
        Parent = null;
        Child = null;
        Sibling = null;
    }
}

public class BinomialHeap<TKey, TValue> where TKey : IComparable<TKey>
{
    private BinomialHeapNode<TKey, TValue> head;

    public BinomialHeap()
    {
        head = null;
    }

    public bool IsEmpty() => head == null;

    public void Insert(TKey key, TValue value)
    {
        var tempHeap = new BinomialHeap<TKey, TValue>();
        tempHeap.head = new BinomialHeapNode<TKey, TValue>(key, value);
        Union(tempHeap);
    }

    public BinomialHeapNode<TKey, TValue> GetMinimum()
    {
        if (head == null) return null;

        BinomialHeapNode<TKey, TValue> minNode = head;
        BinomialHeapNode<TKey, TValue> current = head.Sibling;

        while (current != null)
        {
            if (current.Key.CompareTo(minNode.Key) < 0)
            {
                minNode = current;
            }
            current = current.Sibling;
        }

        return minNode;
    }

    public BinomialHeapNode<TKey, TValue> ExtractMinimum()
    {
        if (head == null) return null;

        BinomialHeapNode<TKey, TValue> minNode = head;
        BinomialHeapNode<TKey, TValue> minNodePrev = null;
        BinomialHeapNode<TKey, TValue> current = head;
        BinomialHeapNode<TKey, TValue> prev = null;

        while (current != null)
        {
            if (current.Key.CompareTo(minNode.Key) < 0)
            {
                minNode = current;
                minNodePrev = prev;
            }
            prev = current;
            current = current.Sibling;
        }

        if (minNodePrev == null)
        {
            head = minNode.Sibling;
        }
        else
        {
            minNodePrev.Sibling = minNode.Sibling;
        }

        BinomialHeapNode<TKey, TValue> child = minNode.Child;
        BinomialHeapNode<TKey, TValue> childHead = null;

        while (child != null)
        {
            BinomialHeapNode<TKey, TValue> next = child.Sibling;
            child.Sibling = childHead;
            child.Parent = null;
            childHead = child;
            child = next;
        }

        var tempHeap = new BinomialHeap<TKey, TValue>();
        tempHeap.head = childHead;

        Union(tempHeap);

        return minNode;
    }

    public void Union(BinomialHeap<TKey, TValue> other)
    {
        if (other == null || other.head == null) return;

        head = MergeRoots(this.head, other.head);
        other.head = null;

        if (head == null) return;

        BinomialHeapNode<TKey, TValue> prev = null;
        BinomialHeapNode<TKey, TValue> current = head;
        BinomialHeapNode<TKey, TValue> next = head.Sibling;

        while (next != null)
        {
            if ((current.Degree != next.Degree) || 
                (next.Sibling != null && next.Sibling.Degree == current.Degree))
            {
                prev = current;
                current = next;
            }
            else
            {
                if (current.Key.CompareTo(next.Key) <= 0)
                {
                    current.Sibling = next.Sibling;
                    Link(next, current);
                }
                else
                {
                    if (prev == null)
                    {
                        head = next;
                    }
                    else
                    {
                        prev.Sibling = next;
                    }
                    Link(current, next);
                    current = next;
                }
            }
            next = current.Sibling;
        }
    }

    private void Link(BinomialHeapNode<TKey, TValue> child, BinomialHeapNode<TKey, TValue> parent)
    {
        child.Parent = parent;
        child.Sibling = parent.Child;
        parent.Child = child;
        parent.Degree++;
    }

    private BinomialHeapNode<TKey, TValue> MergeRoots(BinomialHeapNode<TKey, TValue> h1, BinomialHeapNode<TKey, TValue> h2)
    {
        if (h1 == null) return h2;
        if (h2 == null) return h1;

        BinomialHeapNode<TKey, TValue> dummy = new BinomialHeapNode<TKey, TValue>(default, default);
        BinomialHeapNode<TKey, TValue> tail = dummy;

        while (h1 != null && h2 != null)
        {
            if (h1.Degree <= h2.Degree)
            {
                tail.Sibling = h1;
                h1 = h1.Sibling;
            }
            else
            {
                tail.Sibling = h2;
                h2 = h2.Sibling;
            }
            tail = tail.Sibling;
        }

        tail.Sibling = (h1 != null) ? h1 : h2;
        return dummy.Sibling;
    }

    public void DecreaseKey(BinomialHeapNode<TKey, TValue> node, TKey newKey)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));
        if (newKey.CompareTo(node.Key) > 0)
        {
            throw new ArgumentException("New key is greater than current key.");
        }

        node.Key = newKey;
        BinomialHeapNode<TKey, TValue> current = node;
        BinomialHeapNode<TKey, TValue> parent = current.Parent;

        while (parent != null && current.Key.CompareTo(parent.Key) < 0)
        {
            TKey tempKey = current.Key;
            TValue tempVal = current.Value;

            current.Key = parent.Key;
            current.Value = parent.Value;

            parent.Key = tempKey;
            parent.Value = tempVal;

            current = parent;
            parent = current.Parent;
        }
    }

    public void Delete(BinomialHeapNode<TKey, TValue> node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        BinomialHeapNode<TKey, TValue> current = node;
        BinomialHeapNode<TKey, TValue> parent = current.Parent;

        while (parent != null)
        {
            TKey tempKey = current.Key;
            TValue tempVal = current.Value;

            current.Key = parent.Key;
            current.Value = parent.Value;

            parent.Key = tempKey;
            parent.Value = tempVal;

            current = parent;
            parent = current.Parent;
        }

        BinomialHeapNode<TKey, TValue> prev = null;
        BinomialHeapNode<TKey, TValue> search = head;

        while (search != null && search != current)
        {
            prev = search;
            search = search.Sibling;
        }

        if (search == null)
        {
            throw new InvalidOperationException("Node not found in the heap.");
        }

        if (prev == null)
        {
            head = current.Sibling;
        }
        else
        {
            prev.Sibling = current.Sibling;
        }

        BinomialHeapNode<TKey, TValue> child = current.Child;
        BinomialHeapNode<TKey, TValue> childHead = null;

        while (child != null)
        {
            BinomialHeapNode<TKey, TValue> next = child.Sibling;
            child.Sibling = childHead;
            child.Parent = null;
            childHead = child;
            child = next;
        }

        var tempHeap = new BinomialHeap<TKey, TValue>();
        tempHeap.head = childHead;

        Union(tempHeap);
    }
}