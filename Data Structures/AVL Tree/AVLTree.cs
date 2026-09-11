using System;
using System.Collections.Generic;

public class AVLTree<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value;
        public Node Left;
        public Node Right;
        public int Height;

        public Node(T value)
        {
            Value = value;
            Height = 1;
        }
    }

    private Node _root;

    public int Count { get; private set; }

    public bool Contains(T item)
    {
        return Contains(_root, item);
    }

    private bool Contains(Node node, T item)
    {
        if (node == null) return false;
        int cmp = item.CompareTo(node.Value);
        if (cmp < 0) return Contains(node.Left, item);
        if (cmp > 0) return Contains(node.Right, item);
        return true;
    }

    public void Insert(T item)
    {
        _root = Insert(_root, item);
    }

    private Node Insert(Node node, T item)
    {
        if (node == null)
        {
            Count++;
            return new Node(item);
        }

        int cmp = item.CompareTo(node.Value);
        if (cmp < 0)
        {
            node.Left = Insert(node.Left, item);
        }
        else if (cmp > 0)
        {
            node.Right = Insert(node.Right, item);
        }
        else
        {
            return node;
        }

        UpdateHeight(node);
        return Balance(node);
    }

    public bool Remove(T item)
    {
        int initialCount = Count;
        _root = Remove(_root, item);
        return Count < initialCount;
    }

    private Node Remove(Node node, T item)
    {
        if (node == null) return null;

        int cmp = item.CompareTo(node.Value);
        if (cmp < 0)
        {
            node.Left = Remove(node.Left, item);
        }
        else if (cmp > 0)
        {
            node.Right = Remove(node.Right, item);
        }
        else
        {
            Count--;
            if (node.Left == null || node.Right == null)
            {
                node = node.Left ?? node.Right;
            }
            else
            {
                Node minNode = GetMin(node.Right);
                node.Value = minNode.Value;
                node.Right = Remove(node.Right, minNode.Value);
                Count++;
            }
        }

        if (node == null) return null;

        UpdateHeight(node);
        return Balance(node);
    }

    private Node GetMin(Node node)
    {
        while (node.Left != null) node = node.Left;
        return node;
    }

    private int Height(Node node) => node?.Height ?? 0;

    private int GetBalance(Node node) => node == null ? 0 : Height(node.Left) - Height(node.Right);

    private void UpdateHeight(Node node)
    {
        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    private Node Balance(Node node)
    {
        int balanceFactor = GetBalance(node);

        if (balanceFactor > 1)
        {
            if (GetBalance(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            return RotateRight(node);
        }

        if (balanceFactor < -1)
        {
            if (GetBalance(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
            }
            return RotateLeft(node);
        }

        return node;
    }

    private Node RotateRight(Node y)
    {
        Node x = y.Left;
        Node T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        UpdateHeight(y);
        UpdateHeight(x);

        return x;
    }

    private Node RotateLeft(Node x)
    {
        Node y = x.Right;
        Node T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }

    public IEnumerable<T> InOrderTraversal()
    {
        var list = new List<T>();
        InOrderTraversal(_root, list);
        return list;
    }

    private void InOrderTraversal(Node node, List<T> list)
    {
        if (node == null) return;
        InOrderTraversal(node.Left, list);
        list.Add(node.Value);
        InOrderTraversal(node.Right, list);
    }
}