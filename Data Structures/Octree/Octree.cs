using System;
using System.Collections.Generic;

public interface IPositionable3D
{
    Point3D Position { get; }
}

public struct Point3D : IPositionable3D
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Point3D(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Point3D Position => this;
}

public struct AxisAlignedBoundingBox
{
    public Point3D Min { get; }
    public Point3D Max { get; }

    public AxisAlignedBoundingBox(Point3D min, Point3D max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(Point3D point)
    {
        return point.X >= Min.X && point.X <= Max.X &&
               point.Y >= Min.Y && point.Y <= Max.Y &&
               point.Z >= Min.Z && point.Z <= Max.Z;
    }

    public bool Intersects(AxisAlignedBoundingBox other)
    {
        return !(other.Min.X > Max.X || other.Max.X < Min.X ||
                 other.Min.Y > Max.Y || other.Max.Y < Min.Y ||
                 other.Min.Z > Max.Z || other.Max.Z < Min.Z);
    }

    public AxisAlignedBoundingBox GetOctantBounds(int octantIndex)
    {
        double centerX = (Min.X + Max.X) / 2;
        double centerY = (Min.Y + Max.Y) / 2;
        double centerZ = (Min.Z + Max.Z) / 2;

        Point3D newMin = new Point3D(
            (octantIndex & 1) == 0 ? Min.X : centerX,
            (octantIndex & 2) == 0 ? Min.Y : centerY,
            (octantIndex & 4) == 0 ? Min.Z : centerZ);

        Point3D newMax = new Point3D(
            (octantIndex & 1) == 0 ? centerX : Max.X,
            (octantIndex & 2) == 0 ? centerY : Max.Y,
            (octantIndex & 4) == 0 ? centerZ : Max.Z);

        return new AxisAlignedBoundingBox(newMin, newMax);
    }
}

internal class OctreeNode<T> where T : IPositionable3D
{
    public AxisAlignedBoundingBox Bounds { get; }
    public List<T> Items { get; }
    public OctreeNode<T>[] Children { get; private set; }

    private int _currentDepth;
    private int _maxDepth;
    private int _maxItemsPerNode;

    public OctreeNode(AxisAlignedBoundingBox bounds, int currentDepth, int maxDepth, int maxItemsPerNode)
    {
        Bounds = bounds;
        Items = new List<T>();
        _currentDepth = currentDepth;
        _maxDepth = maxDepth;
        _maxItemsPerNode = maxItemsPerNode;
        Children = null;
    }

    public void Insert(T item)
    {
        if (!Bounds.Contains(item.Position)) return;

        if (Items.Count < _maxItemsPerNode || _currentDepth >= _maxDepth)
        {
            Items.Add(item);
            return;
        }

        if (Children == null)
        {
            Subdivide();
        }

        foreach (var child in Children)
        {
            child.Insert(item);
        }
    }

    private void Subdivide()
    {
        Children = new OctreeNode<T>[8];
        for (int i = 0; i < 8; i++)
        {
            var childBounds = Bounds.GetOctantBounds(i);
            Children[i] = new OctreeNode<T>(childBounds, _currentDepth + 1, _maxDepth, _maxItemsPerNode);
        }

        foreach (var item in Items)
        {
            foreach (var child in Children)
            {
                child.Insert(item);
            }
        }

        Items.Clear();
    }

    public void Query(AxisAlignedBoundingBox queryBounds, List<T> result)
    {
        if (!Bounds.Intersects(queryBounds)) return;

        foreach (var item in Items)
        {
            if (queryBounds.Contains(item.Position))
            {
                result.Add(item);
            }
        }

        if (Children != null)
        {
            foreach (var child in Children)
            {
                child.Query(queryBounds, result);
            }
        }
    }
}

public class Octree<T> where T : IPositionable3D
{
    private OctreeNode<T> _root;
    private int _maxDepth;
    private int _maxItemsPerNode;

    public Octree(AxisAlignedBoundingBox initialBounds, int maxDepth, int maxItemsPerNode)
    {
        _root = new OctreeNode<T>(initialBounds, 0, maxDepth, maxItemsPerNode);
        _maxDepth = maxDepth;
        _maxItemsPerNode = maxItemsPerNode;
    }

    public void Insert(T item)
    {
        if (_root == null)
        {
            throw new InvalidOperationException("Octree is not initialized.");
        }
        _root.Insert(item);
    }

    public List<T> Query(AxisAlignedBoundingBox queryBounds)
    {
        var result = new List<T>();
        _root.Query(queryBounds, result);
        return result;
    }

    public void Clear()
    {
        _root = new OctreeNode<T>(_root.Bounds, 0, _maxDepth, _maxItemsPerNode);
    }
}