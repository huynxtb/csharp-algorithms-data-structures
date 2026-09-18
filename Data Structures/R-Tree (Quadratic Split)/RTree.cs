using System;
using System.Collections;
using System.Collections.Generic;

public readonly struct Rectangle2D : IEquatable<Rectangle2D>
{
    public double MinX { get; }
    public double MinY { get; }
    public double MaxX { get; }
    public double MaxY { get; }

    public Rectangle2D(double minX, double minY, double maxX, double maxY)
    {
        if (minX > maxX)
        {
            double temp = minX;
            minX = maxX;
            maxX = temp;
        }

        if (minY > maxY)
        {
            double temp = minY;
            minY = maxY;
            maxY = temp;
        }

        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;
    }

    public static Rectangle2D FromPoint(double x, double y)
    {
        return new Rectangle2D(x, y, x, y);
    }

    public double Width => Math.Max(0.0, MaxX - MinX);

    public double Height => Math.Max(0.0, MaxY - MinY);

    public double Area => Width * Height;

    public double Margin => 2.0 * (Width + Height);

    public bool Intersects(in Rectangle2D other)
    {
        return MinX <= other.MaxX && MaxX >= other.MinX &&
               MinY <= other.MaxY && MaxY >= other.MinY;
    }

    public bool Contains(in Rectangle2D other)
    {
        return MinX <= other.MinX && MaxX >= other.MaxX &&
               MinY <= other.MinY && MaxY >= other.MaxY;
    }

    public bool Contains(double x, double y)
    {
        return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
    }

    public Rectangle2D Union(in Rectangle2D other)
    {
        return new Rectangle2D(
            Math.Min(MinX, other.MinX),
            Math.Min(MinY, other.MinY),
            Math.Max(MaxX, other.MaxX),
            Math.Max(MaxY, other.MaxY)
        );
    }

    public Rectangle2D? Intersection(in Rectangle2D other)
    {
        if (!Intersects(other))
        {
            return null;
        }

        return new Rectangle2D(
            Math.Max(MinX, other.MinX),
            Math.Max(MinY, other.MinY),
            Math.Min(MaxX, other.MaxX),
            Math.Min(MaxY, other.MaxY)
        );
    }

    public double DistanceSquaredTo(double x, double y)
    {
        double dx = 0.0;
        if (x < MinX)
        {
            dx = MinX - x;
        }
        else if (x > MaxX)
        {
            dx = x - MaxX;
        }

        double dy = 0.0;
        if (y < MinY)
        {
            dy = MinY - y;
        }
        else if (y > MaxY)
        {
            dy = y - MaxY;
        }

        return dx * dx + dy * dy;
    }

    public bool Equals(Rectangle2D other)
    {
        const double epsilon = 1e-12;
        return Math.Abs(MinX - other.MinX) < epsilon &&
               Math.Abs(MinY - other.MinY) < epsilon &&
               Math.Abs(MaxX - other.MaxX) < epsilon &&
               Math.Abs(MaxY - other.MaxY) < epsilon;
    }

    public override bool Equals(object? obj)
    {
        return obj is Rectangle2D other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + MinX.GetHashCode();
            hash = hash * 31 + MinY.GetHashCode();
            hash = hash * 31 + MaxX.GetHashCode();
            hash = hash * 31 + MaxY.GetHashCode();
            return hash;
        }
    }

    public static bool operator ==(Rectangle2D left, Rectangle2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Rectangle2D left, Rectangle2D right)
    {
        return !left.Equals(right);
    }
}

public class RTree<T> : IEnumerable<T>
{
    public readonly struct Entry
    {
        public Rectangle2D Bounds { get; }
        public T? Data { get; }
        internal RTreeNode<T>? ChildNode { get; }

        public Entry(Rectangle2D bounds, T data)
        {
            Bounds = bounds;
            Data = data;
            ChildNode = null;
        }

        internal Entry(Rectangle2D bounds, RTreeNode<T> childNode)
        {
            Bounds = bounds;
            Data = default;
            ChildNode = childNode;
        }
    }

    internal class RTreeNode<TNode>
    {
        public bool IsLeaf { get; set; }
        public List<Entry> Entries { get; }
        public RTreeNode<TNode>? Parent { get; set; }

        public RTreeNode(bool isLeaf, int capacity)
        {
            IsLeaf = isLeaf;
            Entries = new List<Entry>(capacity + 1);
            Parent = null;
        }

        public Rectangle2D ComputeBounds()
        {
            if (Entries.Count == 0)
            {
                return new Rectangle2D(0, 0, 0, 0);
            }

            Rectangle2D b = Entries[0].Bounds;
            for (int i = 1; i < Entries.Count; i++)
            {
                b = b.Union(Entries[i].Bounds);
            }
            return b;
        }
    }

    private readonly int _minEntries;
    private readonly int _maxEntries;
    private readonly IEqualityComparer<T> _comparer;
    private RTreeNode<T> _root;
    private int _count;

    public int Count => _count;
    public int MinEntries => _minEntries;
    public int MaxEntries => _maxEntries;
    public Rectangle2D Bounds => _root.ComputeBounds();

    public RTree(int minEntries = 2, int maxEntries = 4, IEqualityComparer<T>? comparer = null)
    {
        if (minEntries < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(minEntries), "minEntries must be at least 1.");
        }
        if (maxEntries < minEntries * 2)
        {
            throw new ArgumentException("maxEntries must be at least 2 * minEntries for balanced splitting.", nameof(maxEntries));
        }

        _minEntries = minEntries;
        _maxEntries = maxEntries;
        _comparer = comparer ?? EqualityComparer<T>.Default;
        _root = new RTreeNode<T>(true, _maxEntries);
        _count = 0;
    }

    public void Clear()
    {
        _root = new RTreeNode<T>(true, _maxEntries);
        _count = 0;
    }

    public void Insert(Rectangle2D bounds, T data)
    {
        Entry entry = new Entry(bounds, data);
        RTreeNode<T> leaf = ChooseSubtree(_root, entry);
        leaf.Entries.Add(entry);
        _count++;

        if (leaf.Entries.Count > _maxEntries)
        {
            RTreeNode<T>? splitNode = SplitNode(leaf);
            AdjustTree(leaf, splitNode);
        }
        else
        {
            AdjustTree(leaf, null);
        }
    }

    public bool Remove(Rectangle2D bounds, T data)
    {
        RTreeNode<T>? leaf = FindLeaf(_root, bounds, data);
        if (leaf == null)
        {
            return false;
        }

        int index = -1;
        for (int i = 0; i < leaf.Entries.Count; i++)
        {
            if (leaf.Entries[i].Bounds.Equals(bounds) && _comparer.Equals(leaf.Entries[i].Data!, data))
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            return false;
        }

        leaf.Entries.RemoveAt(index);
        _count--;

        CondenseTree(leaf);

        if (!_root.IsLeaf && _root.Entries.Count == 1)
        {
            _root = _root.Entries[0].ChildNode!;
            _root.Parent = null;
        }
        else if (_root.IsLeaf && _root.Entries.Count == 0)
        {
            _root = new RTreeNode<T>(true, _maxEntries);
        }

        return true;
    }

    public bool Contains(Rectangle2D bounds, T data)
    {
        return FindLeaf(_root, bounds, data) != null;
    }

    public List<T> Search(Rectangle2D searchBounds)
    {
        List<T> results = new List<T>();
        SearchInternal(_root, searchBounds, results);
        return results;
    }

    public List<Entry> SearchEntries(Rectangle2D searchBounds)
    {
        List<Entry> results = new List<Entry>();
        SearchEntriesInternal(_root, searchBounds, results);
        return results;
    }

    public (T? Item, Rectangle2D Bounds, double DistanceSquared)? NearestNeighbor(double x, double y)
    {
        if (_count == 0)
        {
            return null;
        }

        var pq = new PriorityQueue<RTreeNode<T>, double>();
        pq.Enqueue(_root, _root.ComputeBounds().DistanceSquaredTo(x, y));

        T? bestItem = default;
        Rectangle2D bestBounds = default;
        double bestDistanceSq = double.PositiveInfinity;
        bool found = false;

        while (pq.Count > 0)
        {
            if (pq.TryDequeue(out RTreeNode<T>? node, out double nodeDistSq))
            {
                if (nodeDistSq > bestDistanceSq)
                {
                    break;
                }

                if (node.IsLeaf)
                {
                    foreach (var entry in node.Entries)
                    {
                        double distSq = entry.Bounds.DistanceSquaredTo(x, y);
                        if (distSq < bestDistanceSq)
                        {
                            bestDistanceSq = distSq;
                            bestItem = entry.Data;
                            bestBounds = entry.Bounds;
                            found = true;
                        }
                    }
                }
                else
                {
                    foreach (var entry in node.Entries)
                    {
                        if (entry.ChildNode != null)
                        {
                            double distSq = entry.Bounds.DistanceSquaredTo(x, y);
                            if (distSq < bestDistanceSq)
                            {
                                pq.Enqueue(entry.ChildNode, distSq);
                            }
                        }
                    }
                }
            }
        }

        if (!found)
        {
            return null;
        }

        return (bestItem, bestBounds, bestDistanceSq);
    }

    private void SearchInternal(RTreeNode<T> node, in Rectangle2D searchBounds, List<T> results)
    {
        foreach (var entry in node.Entries)
        {
            if (entry.Bounds.Intersects(searchBounds))
            {
                if (node.IsLeaf)
                {
                    if (entry.Data != null)
                    {
                        results.Add(entry.Data);
                    }
                }
                else if (entry.ChildNode != null)
                {
                    SearchInternal(entry.ChildNode, searchBounds, results);
                }
            }
        }
    }

    private void SearchEntriesInternal(RTreeNode<T> node, in Rectangle2D searchBounds, List<Entry> results)
    {
        foreach (var entry in node.Entries)
        {
            if (entry.Bounds.Intersects(searchBounds))
            {
                if (node.IsLeaf)
                {
                    results.Add(entry);
                }
                else if (entry.ChildNode != null)
                {
                    SearchEntriesInternal(entry.ChildNode, searchBounds, results);
                }
            }
        }
    }

    private RTreeNode<T> ChooseSubtree(RTreeNode<T> node, in Entry entry)
    {
        if (node.IsLeaf)
        {
            return node;
        }

        RTreeNode<T>? bestChild = null;
        double minEnlargement = double.PositiveInfinity;
        double minArea = double.PositiveInfinity;

        foreach (var childEntry in node.Entries)
        {
            Rectangle2D currentBounds = childEntry.Bounds;
            double currentArea = currentBounds.Area;
            Rectangle2D enlarged = currentBounds.Union(entry.Bounds);
            double enlargedArea = enlarged.Area;
            double enlargement = enlargedArea - currentArea;

            if (enlargement < minEnlargement)
            {
                minEnlargement = enlargement;
                minArea = currentArea;
                bestChild = childEntry.ChildNode;
            }
            else if (Math.Abs(enlargement - minEnlargement) < 1e-12)
            {
                if (currentArea < minArea)
                {
                    minArea = currentArea;
                    bestChild = childEntry.ChildNode;
                }
            }
        }

        return ChooseSubtree(bestChild!, entry);
    }

    private void AdjustTree(RTreeNode<T> l, RTreeNode<T>? ll)
    {
        RTreeNode<T>? current = l;
        RTreeNode<T>? partner = ll;

        while (current != null)
        {
            if (current.Parent == null)
            {
                if (partner != null)
                {
                    RTreeNode<T> newRoot = new RTreeNode<T>(false, _maxEntries);
                    newRoot.Entries.Add(new Entry(current.ComputeBounds(), current));
                    newRoot.Entries.Add(new Entry(partner.ComputeBounds(), partner));
                    current.Parent = newRoot;
                    partner.Parent = newRoot;
                    _root = newRoot;
                }
                return;
            }

            RTreeNode<T> parent = current.Parent;
            for (int i = 0; i < parent.Entries.Count; i++)
            {
                if (parent.Entries[i].ChildNode == current)
                {
                    parent.Entries[i] = new Entry(current.ComputeBounds(), current);
                    break;
                }
            }

            if (partner != null)
            {
                parent.Entries.Add(new Entry(partner.ComputeBounds(), partner));
                partner.Parent = parent;

                if (parent.Entries.Count > _maxEntries)
                {
                    partner = SplitNode(parent);
                }
                else
                {
                    partner = null;
                }
            }

            current = parent;
        }
    }

    private RTreeNode<T> SplitNode(RTreeNode<T> node)
    {
        List<Entry> allEntries = new List<Entry>(node.Entries);
        node.Entries.Clear();

        RTreeNode<T> newNode = new RTreeNode<T>(node.IsLeaf, _maxEntries)
        {
            Parent = node.Parent
        };

        PickSeeds(allEntries, out int seed1, out int seed2);

        Entry entry1 = allEntries[seed1];
        Entry entry2 = allEntries[seed2];

        node.Entries.Add(entry1);
        newNode.Entries.Add(entry2);

        if (!node.IsLeaf)
        {
            if (entry1.ChildNode != null) entry1.ChildNode.Parent = node;
            if (entry2.ChildNode != null) entry2.ChildNode.Parent = newNode;
        }

        if (seed1 > seed2)
        {
            allEntries.RemoveAt(seed1);
            allEntries.RemoveAt(seed2);
        }
        else
        {
            allEntries.RemoveAt(seed2);
            allEntries.RemoveAt(seed1);
        }

        Rectangle2D bbox1 = entry1.Bounds;
        Rectangle2D bbox2 = entry2.Bounds;

        while (allEntries.Count > 0)
        {
            if (node.Entries.Count + allEntries.Count == _minEntries)
            {
                foreach (var remaining in allEntries)
                {
                    node.Entries.Add(remaining);
                    if (!node.IsLeaf && remaining.ChildNode != null)
                    {
                        remaining.ChildNode.Parent = node;
                    }
                }
                break;
            }
            if (newNode.Entries.Count + allEntries.Count == _minEntries)
            {
                foreach (var remaining in allEntries)
                {
                    newNode.Entries.Add(remaining);
                    if (!newNode.IsLeaf && remaining.ChildNode != null)
                    {
                        remaining.ChildNode.Parent = newNode;
                    }
                }
                break;
            }

            int nextIndex = PickNext(allEntries, bbox1, bbox2, out bool addToGroup1);
            Entry nextEntry = allEntries[nextIndex];
            allEntries.RemoveAt(nextIndex);

            if (addToGroup1)
            {
                node.Entries.Add(nextEntry);
                bbox1 = bbox1.Union(nextEntry.Bounds);
                if (!node.IsLeaf && nextEntry.ChildNode != null)
                {
                    nextEntry.ChildNode.Parent = node;
                }
            }
            else
            {
                newNode.Entries.Add(nextEntry);
                bbox2 = bbox2.Union(nextEntry.Bounds);
                if (!newNode.IsLeaf && nextEntry.ChildNode != null)
                {
                    nextEntry.ChildNode.Parent = newNode;
                }
            }
        }

        return newNode;
    }

    private static void PickSeeds(List<Entry> entries, out int seed1, out int seed2)
    {
        double maxD = double.NegativeInfinity;
        seed1 = 0;
        seed2 = 1;

        for (int i = 0; i < entries.Count - 1; i++)
        {
            double areaI = entries[i].Bounds.Area;
            for (int j = i + 1; j < entries.Count; j++)
            {
                double areaJ = entries[j].Bounds.Area;
                Rectangle2D combined = entries[i].Bounds.Union(entries[j].Bounds);
                double d = combined.Area - areaI - areaJ;
                if (d > maxD)
                {
                    maxD = d;
                    seed1 = i;
                    seed2 = j;
                }
            }
        }
    }

    private static int PickNext(List<Entry> entries, in Rectangle2D bbox1, in Rectangle2D bbox2, out bool addToGroup1)
    {
        double maxDiff = double.NegativeInfinity;
        int bestIndex = 0;
        addToGroup1 = true;

        double area1 = bbox1.Area;
        double area2 = bbox2.Area;

        for (int i = 0; i < entries.Count; i++)
        {
            Rectangle2D union1 = bbox1.Union(entries[i].Bounds);
            double d1 = union1.Area - area1;

            Rectangle2D union2 = bbox2.Union(entries[i].Bounds);
            double d2 = union2.Area - area2;

            double diff = Math.Abs(d1 - d2);
            if (diff > maxDiff)
            {
                maxDiff = diff;
                bestIndex = i;
                if (d1 < d2)
                {
                    addToGroup1 = true;
                }
                else if (d2 < d1)
                {
                    addToGroup1 = false;
                }
                else
                {
                    if (area1 < area2)
                    {
                        addToGroup1 = true;
                    }
                    else if (area2 < area1)
                    {
                        addToGroup1 = false;
                    }
                    else
                    {
                        addToGroup1 = bbox1.Width * bbox1.Height <= bbox2.Width * bbox2.Height;
                    }
                }
            }
        }

        return bestIndex;
    }

    private RTreeNode<T>? FindLeaf(RTreeNode<T> node, in Rectangle2D bounds, T data)
    {
        if (node.IsLeaf)
        {
            foreach (var entry in node.Entries)
            {
                if (entry.Bounds.Equals(bounds) && _comparer.Equals(entry.Data!, data))
                {
                    return node;
                }
            }
            return null;
        }

        foreach (var entry in node.Entries)
        {
            if (entry.Bounds.Intersects(bounds) && entry.ChildNode != null)
            {
                RTreeNode<T>? result = FindLeaf(entry.ChildNode, bounds, data);
                if (result != null)
                {
                    return result;
                }
            }
        }

        return null;
    }

    private void CondenseTree(RTreeNode<T> leaf)
    {
        RTreeNode<T>? node = leaf;
        List<RTreeNode<T>> reinsertNodes = new List<RTreeNode<T>>();

        while (node != null && node != _root)
        {
            RTreeNode<T>? parent = node.Parent;
            if (parent == null)
            {
                break;
            }

            if (node.Entries.Count < _minEntries)
            {
                for (int i = 0; i < parent.Entries.Count; i++)
                {
                    if (parent.Entries[i].ChildNode == node)
                    {
                        parent.Entries.RemoveAt(i);
                        break;
                    }
                }
                reinsertNodes.Add(node);
            }
            else
            {
                for (int i = 0; i < parent.Entries.Count; i++)
                {
                    if (parent.Entries[i].ChildNode == node)
                    {
                        parent.Entries[i] = new Entry(node.ComputeBounds(), node);
                        break;
                    }
                }
            }

            node = parent;
        }

        foreach (var rNode in reinsertNodes)
        {
            ReinsertAllEntries(rNode);
        }
    }

    private void ReinsertAllEntries(RTreeNode<T> node)
    {
        if (node.IsLeaf)
        {
            foreach (var entry in node.Entries)
            {
                Insert(entry.Bounds, entry.Data!);
                _count--;
            }
        }
        else
        {
            foreach (var entry in node.Entries)
            {
                if (entry.ChildNode != null)
                {
                    ReinsertAllEntries(entry.ChildNode);
                }
            }
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        List<T> allItems = new List<T>();
        CollectAllLeaves(_root, allItems);
        return allItems.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static void CollectAllLeaves(RTreeNode<T> node, List<T> list)
    {
        if (node.IsLeaf)
        {
            foreach (var entry in node.Entries)
            {
                if (entry.Data != null)
                {
                    list.Add(entry.Data);
                }
            }
        }
        else
        {
            foreach (var entry in node.Entries)
            {
                if (entry.ChildNode != null)
                {
                    CollectAllLeaves(entry.ChildNode, list);
                }
            }
        }
    }
}