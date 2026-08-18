using System;
using System.Collections.Generic;

/// <summary>
/// Represents a node in a Fibonacci Heap.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement IComparable.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class FibonacciHeapNode<TKey, TValue> where TKey : IComparable<TKey>
{
    /// <summary>
    /// Gets the key of the node.
    /// </summary>
    public TKey Key { get; internal set; }

    /// <summary>
    /// Gets or sets the value of the node.
    /// </summary>
    public TValue Value { get; set; }

    /// <summary>
    /// Gets the parent node.
    /// </summary>
    public FibonacciHeapNode<TKey, TValue> Parent { get; internal set; }

    /// <summary>
    /// Gets the first child node.
    /// </summary>
    public FibonacciHeapNode<TKey, TValue> Child { get; internal set; }

    /// <summary>
    /// Gets the left sibling node.
    /// </summary>
    public FibonacciHeapNode<TKey, TValue> Left { get; internal set; }

    /// <summary>
    /// Gets the right sibling node.
    /// </summary>
    public FibonacciHeapNode<TKey, TValue> Right { get; internal set; }

    /// <summary>
    /// Gets the degree of the node (number of children).
    /// </summary>
    public int Degree { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the node is marked.
    /// </summary>
    public bool IsMarked { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the FibonacciHeapNode class.
    /// </summary>
    public FibonacciHeapNode(TKey key, TValue value)
    {
        Key = key;
        Value = value;
        Left = this;
        Right = this;
    }
}

/// <summary>
/// Represents a Fibonacci Heap data structure.
/// </summary>
/// <typeparam name="TKey">The type of the key, which must implement IComparable.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
public class FibonacciHeap<TKey, TValue> where TKey : IComparable<TKey>
{
    private FibonacciHeapNode<TKey, TValue> min;
    private int count;

    /// <summary>
    /// Gets the total number of items in the heap.
    /// </summary>
    public int Count => count;

    /// <summary>
    /// Inserts a new key-value pair into the heap.
    /// </summary>
    public FibonacciHeapNode<TKey, TValue> Insert(TKey key, TValue value)
    {
        var node = new FibonacciHeapNode<TKey, TValue>(key, value);
        if (min == null)
        {
            min = node;
        }
        else
        {
            node.Right = min.Right;
            node.Left = min;
            min.Right.Left = node;
            min.Right = node;

            if (node.Key.CompareTo(min.Key) < 0)
            {
                min = node;
            }
        }
        count++;
        return node;
    }

    /// <summary>
    /// Returns the node containing the minimum key without removing it.
    /// </summary>
    public FibonacciHeapNode<TKey, TValue> Minimum()
    {
        return min;
    }

    /// <summary>
    /// Removes and returns the node containing the minimum key.
    /// </summary>
    public FibonacciHeapNode<TKey, TValue> ExtractMin()
    {
        var z = min;
        if (z != null)
        {
            if (z.Child != null)
            {
                var child = z.Child;
                var children = new List<FibonacciHeapNode<TKey, TValue>>();
                var current = child;
                do
                {
                    children.Add(current);
                    current = current.Right;
                } while (current != child);

                foreach (var c in children)
                {
                    c.Left.Right = c.Right;
                    c.Right.Left = c.Left;

                    c.Right = min.Right;
                    c.Left = min;
                    min.Right.Left = c;
                    min.Right = c;

                    c.Parent = null;
                }
            }

            z.Left.Right = z.Right;
            z.Right.Left = z.Left;

            if (z == z.Right)
            {
                min = null;
            }
            else
            {
                min = z.Right;
                Consolidate();
            }
            count--;
        }
        return z;
    }

    /// <summary>
    /// Decreases the key of the given node to a new key.
    /// </summary>
    public void DecreaseKey(FibonacciHeapNode<TKey, TValue> node, TKey newKey)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));
        if (newKey.CompareTo(node.Key) > 0)
        {
            throw new ArgumentException("New key is greater than current key.", nameof(newKey));
        }

        node.Key = newKey;
        var y = node.Parent;
        if (y != null && node.Key.CompareTo(y.Key) < 0)
        {
            Cut(node, y);
            CascadingCut(y);
        }
        if (node.Key.CompareTo(min.Key) < 0)
        {
            min = node;
        }
    }

    /// <summary>
    /// Deletes the given node from the heap.
    /// </summary>
    public void Delete(FibonacciHeapNode<TKey, TValue> node)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        if (node == min)
        {
            ExtractMin();
            return;
        }

        var parent = node.Parent;
        if (parent != null)
        {
            Cut(node, parent);
            CascadingCut(parent);
        }

        if (node.Child != null)
        {
            var child = node.Child;
            var children = new List<FibonacciHeapNode<TKey, TValue>>();
            var current = child;
            do
            {
                children.Add(current);
                current = current.Right;
            }\ while (current != child);

            foreach (var c in children)
            {
                c.Left.Right = c.Right;
                c.Right.Left = c.Left;

                c.Right = min.Right;
                c.Left = min;
                min.Right.Left = c;
                min.Right = c;

                c.Parent = null;
            }
        }

        node.Left.Right = node.Right;
        node.Right.Left = node.Left;
        count--;
    }

    /// <summary>
    /// Merges another Fibonacci Heap into the current heap.
    /// </summary>
    public void Union(FibonacciHeap<TKey, TValue> other)
    {
        if (other == null || other.min == null) return;

        if (this.min == null)
        {
            this.min = other.min;
            this.count = other.count;
        }
        else
        {
            var thisMinNext = this.min.Right;
            var otherMinNext = other.min.Right;

            this.min.Right = otherMinNext;
            otherMinNext.Left = this.min;

            other.min.Right = thisMinNext;
            thisMinNext.Left = other.min;

            if (other.min.Key.CompareTo(this.min.Key) < 0)
            {
                this.min = other.min;
            }
            this.count += other.count;
        }

        other.min = null;
        other.count = 0;
    }

    private void Consolidate()
    {
        var maxDegree = 64;
        var A = new FibonacciHeapNode<TKey, TValue>[maxDegree];

        var rootList = new List<FibonacciHeapNode<TKey, TValue>>();
        var start = min;
        if (start != null)
        {
            var current = start;
            do
            {
                rootList.Add(current);
                current = current.Right;
            } while (current != start);
        }

        foreach (var w in rootList)
        {
            var x = w;
            var d = x.Degree;
            while (d < maxDegree && A[d] != null)
            {
                var y = A[d];
                if (x.Key.CompareTo(y.Key) > 0)
                {
                    var temp = x;
                    x = y;
                    y = temp;
                }
                Link(y, x);
                A[d] = null;
                d++;
            }
            if (d < maxDegree)
            {
                A[d] = x;
            }
        }

        min = null;
        for (int i = 0; i < maxDegree; i++)
        {
            if (A[i] != null)
            {
                if (min == null)
                {
                    min = A[i];
                    min.Left = min;
                    min.Right = min;
                }
                else
                {
                    A[i].Right = min.Right;
                    A[i].Left = min;
                    min.Right.Left = A[i];
                    min.Right = A[i];

                    if (A[i].Key.CompareTo(min.Key) < 0)
                    {
                        min = A[i];
                    }
                }
            }
        }
    }

    private void Link(FibonacciHeapNode<TKey, TValue> y, FibonacciHeapNode<TKey, TValue> x)
    {
        y.Left.Right = y.Right;
        y.Right.Left = y.Left;

        y.Parent = x;
        if (x.Child == null)
        {
            x.Child = y;
            y.Left = y;
            y.Right = y;
        }
        else
        {
            y.Right = x.Child.Right;
            y.Left = x.Child;
            x.Child.Right.Left = y;
            x.Child.Right = y;
        }
        x.Degree++;
        y.IsMarked = false;
    }

    private void Cut(FibonacciHeapNode<TKey, TValue> x, FibonacciHeapNode<TKey, TValue> y)
    {
        if (x.Right == x)
        {
            y.Child = null;
        }
        else
        {
            x.Left.Right = x.Right;
            x.Right.Left = x.Left;
            if (y.Child == x)
            {
                y.Child = x.Right;
            }
        }
        y.Degree--;

        x.Right = min.Right;
        x.Left = min;
        min.Right.Left = x;
        min.Right = x;

        x.Parent = null;
        x.IsMarked = false;
    }

    private void CascadingCut(FibonacciHeapNode<TKey, TValue> y)
    {
        var z = y.Parent;
        if (z != null)
        {
            if (!y.IsMarked)
            {
                y.IsMarked = true;
            }
            else
            {
                Cut(y, z);
                CascadingCut(z);
            }
        }
    }
}