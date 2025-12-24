using System;
using System.Collections.Generic;

public class KdTree
{
    private class Node
    {
        public double[] Point;
        public Node Left;
        public Node Right;

        public Node(double[] point)
        {
            Point = point;
            Left = null;
            Right = null;
        }
    }

    private Node root;
    private readonly int k; // number of dimensions

    public KdTree(int dimensions)
    {
        if (dimensions <= 0)
            throw new ArgumentException("Dimensions must be positive.");
        k = dimensions;
        root = null;
    }

    public void Insert(double[] point)
    {
        if (point == null || point.Length != k)
            throw new ArgumentException($"Point must have exactly {k} dimensions.");
        root = Insert(root, point, 0);
    }

    private Node Insert(Node node, double[] point, int depth)
    {
        if (node == null)
            return new Node(point);

        int axis = depth % k;

        if (point[axis] < node.Point[axis])
            node.Left = Insert(node.Left, point, depth + 1);
        else
            node.Right = Insert(node.Right, point, depth + 1);

        return node;
    }

    public double[] NearestNeighbor(double[] target)
    {
        if (target == null || target.Length != k)
            throw new ArgumentException($"Target point must have exactly {k} dimensions.");
        if (root == null)
            return null;

        return NearestNeighbor(root, target, 0, root.Point, double.MaxValue);
    }

    private double[] NearestNeighbor(Node node, double[] target, int depth, double[] bestPoint, double bestDist)
    {
        if (node == null)
            return bestPoint;

        double dist = DistanceSquared(node.Point, target);
        if (dist < bestDist)
        {
            bestDist = dist;
            bestPoint = node.Point;
        }

        int axis = depth % k;

        Node nextNode = null;
        Node otherNode = null;

        if (target[axis] < node.Point[axis])
        {
            nextNode = node.Left;
            otherNode = node.Right;
        }
        else
        {
            nextNode = node.Right;
            otherNode = node.Left;
        }

        bestPoint = NearestNeighbor(nextNode, target, depth + 1, bestPoint, bestDist);
        bestDist = DistanceSquared(bestPoint, target);

        // Check if we need to explore the other side
        double diff = target[axis] - node.Point[axis];
        if (diff * diff < bestDist)
        {
            bestPoint = NearestNeighbor(otherNode, target, depth + 1, bestPoint, bestDist);
        }

        return bestPoint;
    }

    /// <summary>
    /// Returns all points within the given rectangular range.
    /// The range is specified by two points: minPoint and maxPoint representing
    /// the opposite corners of the k-dimensional rectangular range.
    /// </summary>
    /// <param name="minPoint">Minimum corner of the rectangular range</param>
    /// <param name="maxPoint">Maximum corner of the rectangular range</param>
    /// <returns>List of points inside the given range</returns>
    public List<double[]> RangeSearch(double[] minPoint, double[] maxPoint)
    {
        if (minPoint == null || maxPoint == null || minPoint.Length != k || maxPoint.Length != k)
            throw new ArgumentException($"Range points must have exactly {k} dimensions.");

        var results = new List<double[]>();
        RangeSearch(root, minPoint, maxPoint, 0, results);
        return results;
    }

    private void RangeSearch(Node node, double[] minPoint, double[] maxPoint, int depth, List<double[]> results)
    {
        if (node == null) return;

        if (IsPointInRange(node.Point, minPoint, maxPoint))
            results.Add(node.Point);

        int axis = depth % k;

        // If left subtree could have points in range
        if (minPoint[axis] <= node.Point[axis])
            RangeSearch(node.Left, minPoint, maxPoint, depth + 1, results);

        // If right subtree could have points in range
        if (maxPoint[axis] >= node.Point[axis])
            RangeSearch(node.Right, minPoint, maxPoint, depth + 1, results);
    }

    private static bool IsPointInRange(double[] point, double[] minPoint, double[] maxPoint)
    {
        for (int i = 0; i < point.Length; i++)
        {
            if (point[i] < minPoint[i] || point[i] > maxPoint[i])
                return false;
        }
        return true;
    }

    private static double DistanceSquared(double[] a, double[] b)
    {
        double dist = 0;
        for (int i = 0; i < a.Length; i++)
        {
            double diff = a[i] - b[i];
            dist += diff * diff;
        }
        return dist;
    }
}
