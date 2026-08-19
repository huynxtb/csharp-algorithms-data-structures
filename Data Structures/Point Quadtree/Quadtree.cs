using System;
using System.Collections.Generic;

public struct Point2D
{
    public double X { get; }
    public double Y { get; }

    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }
}

public struct Boundary
{
    public double CenterX { get; }
    public double CenterY { get; }
    public double HalfWidth { get; }
    public double HalfHeight { get; }

    public Boundary(double centerX, double centerY, double halfWidth, double halfHeight)
    {
        CenterX = centerX;
        CenterY = centerY;
        HalfWidth = halfWidth;
        HalfHeight = halfHeight;
    }

    public bool Contains(Point2D point)
    {
        return point.X >= CenterX - HalfWidth &&
               point.X <= CenterX + HalfWidth &&
               point.Y >= CenterY - HalfHeight &&
               point.Y <= CenterY + HalfHeight;
    }

    public bool Intersects(Boundary other)
    {
        return !(other.CenterX - other.HalfWidth > CenterX + HalfWidth ||
                 other.CenterX + other.HalfWidth < CenterX - HalfWidth ||
                 other.CenterY - other.HalfHeight > CenterY + HalfHeight ||
                 other.CenterY + other.HalfHeight < CenterY - HalfHeight);
    }
}

public class QuadtreeItem<T>
{
    public Point2D Point { get; }
    public T Data { get; }

    public QuadtreeItem(Point2D point, T data)
    {
        Point = point;
        Data = data;
    }
}

public class Quadtree<T>
{
    private readonly int _capacity;
    private readonly Boundary _boundary;
    private readonly List<QuadtreeItem<T>> _items;

    private Quadtree<T> _northWest;
    private Quadtree<T> _northEast;
    private Quadtree<T> _southWest;
    private Quadtree<T> _southEast;
    private bool _isDivided;

    public Quadtree(Boundary boundary, int capacity = 4)
    {
        _boundary = boundary;
        _capacity = capacity;
        _items = new List<QuadtreeItem<T>>();
        _isDivided = false;
    }

    public bool Insert(Point2D point, T data)
    {
        if (!_boundary.Contains(point))
        {
            return false;
        }

        if (_items.Count < _capacity && !_isDivided)
        {
            _items.Add(new QuadtreeItem<T>(point, data));
            return true;
        }

        if (!_isDivided)
        {
            Subdivide();
        }

        if (_northWest.Insert(point, data)) return true;
        if (_northEast.Insert(point, data)) return true;
        if (_southWest.Insert(point, data)) return true;
        if (_southEast.Insert(point, data)) return true;

        return false;
    }

    private void Subdivide()
    { 
        double x = _boundary.CenterX;
        double y = _boundary.CenterY;
        double w = _boundary.HalfWidth / 2;
        double h = _boundary.HalfHeight / 2;

        _northWest = new Quadtree<T>(new Boundary(x - w, y + h, w, h), _capacity);
        _northEast = new Quadtree<T>(new Boundary(x + w, y + h, w, h), _capacity);
        _southWest = new Quadtree<T>(new Boundary(x - w, y - h, w, h), _capacity);
        _southEast = new Quadtree<T>(new Boundary(x + w, y - h, w, h), _capacity);

        _isDivided = true;

        for (int i = _items.Count - 1; i >= 0; i--)
        {
            var item = _items[i];
            if (_northWest.Insert(item.Point, item.Data) ||
                _northEast.Insert(item.Point, item.Data) ||
                _southWest.Insert(item.Point, item.Data) ||
                _southEast.Insert(item.Point, item.Data))
            {
                _items.RemoveAt(i);
            }
        }
    }

    public List<QuadtreeItem<T>> QueryRange(Boundary range)
    {
        var results = new List<QuadtreeItem<T>>();
        QueryRangeInternal(range, results);
        return results;
    }

    private void QueryRangeInternal(Boundary range, List<QuadtreeItem<T>> results)
    {
        if (!_boundary.Intersects(range))
        {
            return;
        }

        foreach (var item in _items)
        {
            if (range.Contains(item.Point))
            { 
                results.Add(item);
            }
        }

        if (_isDivided)
        {
            _northWest.QueryRangeInternal(range, results);
            _northEast.QueryRangeInternal(range, results);
            _southWest.QueryRangeInternal(range, results);
            _southEast.QueryRangeInternal(range, results);
        }
    }
}