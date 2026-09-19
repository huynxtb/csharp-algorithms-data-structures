using System;
using System.Collections.Generic;

public class AvlList
{
    private class Node
    {
        public int Value;
        public Node Left;
        public Node Right;
        public int Height;

        public Node(int value)
        {
            Value = value;
            Height = 1;
        }
    }

    private Node _root;
    private int _count;

    public int Count => _count;

    private static int GetHeight(Node node)
    {
        return node == null ? 0 : node.Height;
    }

    private static int GetBalance(Node node)
    {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
    }

    private static void UpdateHeight(Node node)
    {
        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    private static Node RotateRight(Node y)
    {
        Node x = y.Left;
        Node t2 = x.Right;

        x.Right = y;
        y.Left = t2;

        UpdateHeight(y);
        UpdateHeight(x);

        return x;
    }

    private static Node RotateLeft(Node x)
    {
        Node y = x.Right;
        Node t2 = y.Left;

        y.Left = x;
        x.Right = t2;

        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }

    private static Node Rebalance(Node node)
    {
        UpdateHeight(node);
        int balance = GetBalance(node);

        if (balance > 1)
        {
            if (GetBalance(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            return RotateRight(node);
        }

        if (balance < -1)
        {
            if (GetBalance(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
            }
            return RotateLeft(node);
        }

        return node;
    }

    /// <summary>
    /// Adds an integer to the list, keeping it sorted. Duplicates are ignored.
    /// Returns true if the value was added, false if it already existed.
    /// </summary>
    public bool Add(int value)
    {
        bool added = false;
        _root = Insert(_root, value, ref added);
        if (added)
        {
            _count++;
        }
        return added;
    }

    private Node Insert(Node node, int value, ref bool added)
    {
        if (node == null)
        {
            added = true;
            return new Node(value);
        }

        if (value < node.Value)
        {
            node.Left = Insert(node.Left, value, ref added);
        }
        else if (value > node.Value)
        {
            node.Right = Insert(node.Right, value, ref added);
        }
        else
        {
            added = false;
            return node;
        }

        return Rebalance(node);
    }

    /// <summary>
    /// Removes an integer from the list.
    /// Returns true if the value was removed, false if it was not found.
    /// </summary>
    public bool Remove(int value)
    {
        bool removed = false;
        _root = Delete(_root, value, ref removed);
        if (removed)
        {
            _count--;
        }
        return removed;
    }

    private Node Delete(Node node, int value, ref bool removed)
    {
        if (node == null)
        {
            return null;
        }

        if (value < node.Value)
        {
            node.Left = Delete(node.Left, value, ref removed);
        }
        else if (value > node.Value)
        {
            node.Right = Delete(node.Right, value, ref removed);
        }
        else
        {
            removed = true;

            if (node.Left == null || node.Right == null)
            {
                Node child = node.Left ?? node.Right;
                if (child == null)
                {
                    return null;
                }
                node = child;
            }
            else
            {
                Node successor = FindMin(node.Right);
                node.Value = successor.Value;
                bool temp = false;
                node.Right = Delete(node.Right, successor.Value, ref temp);
            }
        }

        return Rebalance(node);
    }

    private static Node FindMin(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    /// <summary>
    /// Checks whether the given integer exists in the list.
    /// </summary>
    public bool Contains(int value)
    {
        Node current = _root;
        while (current != null)
        {
            if (value < current.Value)
            {
                current = current.Left;
            }
            else if (value > current.Value)
            {
                current = current.Right;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns a sorted array containing all integers in the list.
    /// </summary>
    public int[] ToSortedArray()
    {
        var result = new List<int>(_count);
        InOrder(_root, result);
        return result.ToArray();
    }

    private void InOrder(Node node, List<int> result)
    {
        if (node == null)
        {
            return;
        }
        InOrder(node.Left, result);
        result.Add(node.Value);
        InOrder(node.Right, result);
    }

    /// <summary>
    /// Removes all elements from the list.
    /// </summary>
    public void Clear()
    {
        _root = null;
        _count = 0;
    }
}