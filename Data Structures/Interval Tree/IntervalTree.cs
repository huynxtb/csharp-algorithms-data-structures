using System;
using System.Collections.Generic;

public struct Interval<T> where T : IComparable<T>
{
    public T Low { get; }
    public T High { get; }

    public Interval(T low, T high)
    {
        if (low.CompareTo(high) > 0)
        {
            throw new ArgumentException("Low boundary must be less than or equal to High boundary.");
        }
        Low = low;
        High = high;
    }

    public bool Overlaps(Interval<T> other)
    {
        return Low.CompareTo(other.High) <= 0 && other.Low.CompareTo(High) <= 0;
    }

    public override string ToString() => $"[{Low}, {High}]";
}

public class IntervalTreeNode<T> where T : IComparable<T>
{
    public Interval<T> Interval { get; set; }
    public T Max { get; set; }
    public IntervalTreeNode<T> Left { get; set; }
    public IntervalTreeNode<T> Right { get; set; }

    public IntervalTreeNode(Interval<T> interval)
    {
        Interval = interval;
        Max = interval.High;
    }
}

public class IntervalTree<T> where T : IComparable<T>
{
    private IntervalTreeNode<T> root;

    public void Insert(Interval<T> interval)
    {
        root = Insert(root, interval);
    }

    private IntervalTreeNode<T> Insert(IntervalTreeNode<T> node, Interval<T> interval)
    {
        if (node == null)
        {
            return new IntervalTreeNode<T>(interval);
        }

        if (interval.Low.CompareTo(node.Interval.Low) < 0)
        {
            node.Left = Insert(node.Left, interval);
        }
        else
        {
            node.Right = Insert(node.Right, interval);
        }

        if (node.Max.CompareTo(interval.High) < 0)
        {
            node.Max = interval.High;
        }

        return node;
    }

    public Interval<T>? SearchAny(Interval<T> interval)
    {
        IntervalTreeNode<T> current = root;
        while (current != null)
        {
            if (current.Interval.Overlaps(interval))
            {
                return current.Interval;
            }

            if (current.Left != null && current.Left.Max.CompareTo(interval.Low) >= 0)
            {
                current = current.Left;
            }
            else
            {
                current = current.Right;
            }
        }
        return null;
    }

    public IEnumerable<Interval<T>> SearchAll(Interval<T> interval)
    {
        List<Interval<T>> result = new List<Interval<T>>();
        SearchAll(root, interval, result);
        return result;
    }

    private void SearchAll(IntervalTreeNode<T> node, Interval<T> interval, List<Interval<T>> result)
    {
        if (node == null)
        {
            return;
        }

        if (node.Interval.Overlaps(interval))
        {
            result.Add(node.Interval);
        }

        if (node.Left != null && node.Left.Max.CompareTo(interval.Low) >= 0)
        {
            SearchAll(node.Left, interval, result);
        }

        if (node.Right != null && node.Interval.Low.CompareTo(interval.High) <= 0)
        {
            SearchAll(node.Right, interval, result);
        }
    }
}