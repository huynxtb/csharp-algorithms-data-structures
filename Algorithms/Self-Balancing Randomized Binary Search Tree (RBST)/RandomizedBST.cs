using System;
using System.Collections.Generic;

/// <summary>
/// A Self-Balancing Randomized Binary Search Tree (RBST).
/// Uses randomization to keep the tree balanced with expected O(log n)
/// time complexity for search, insertion, and deletion.
/// Duplicate values are permitted.
/// </summary>
/// <typeparam name="T">The type of elements stored, must be comparable.</typeparam>
public class RandomizedBST<T> where T : IComparable<T>
{
    /// <summary>
    /// Internal node of the tree. Encapsulated so callers cannot manipulate structure directly.
    /// </summary>
    private sealed class Node
    {
        public T Value;
        public Node Left;
        public Node Right;
        // Size of the subtree rooted at this node (used for randomized balancing).
        public int Size;

        public Node(T value)
        {
            Value = value;
            Size = 1;
        }
    }

    private Node _root;
    private readonly Random _random;

    /// <summary>
    /// Creates an empty RBST with a time-based random seed.
    /// </summary>
    public RandomizedBST()
    {
        _random = new Random();
    }

    /// <summary>
    /// Creates an empty RBST with a fixed random seed (useful for reproducible tests).
    /// </summary>
    public RandomizedBST(int seed)
    {
        _random = new Random(seed);
    }

    /// <summary>
    /// Gets the number of elements currently stored in the tree.
    /// </summary>
    public int Count => SizeOf(_root);

    private static int SizeOf(Node node) => node?.Size ?? 0;

    private static void UpdateSize(Node node)
    {
        if (node != null)
        {
            node.Size = 1 + SizeOf(node.Left) + SizeOf(node.Right);
        }
    }

    /// <summary>
    /// Inserts a value into the tree. Duplicates are allowed.
    /// </summary>
    /// <param name="value">The value to insert.</param>
    public void Insert(T value)
    {
        _root = Insert(_root, value);
    }

    private Node Insert(Node node, T value)
    {
        if (node == null)
        {
            return new Node(value);
        }

        // With probability 1/(size+1), insert the new node as the root of this subtree.
        // This randomized decision is what keeps the tree balanced on average.
        if (_random.Next(node.Size + 1) == 0)
        {
            return InsertAtRoot(node, value);
        }

        if (value.CompareTo(node.Value) < 0)
        {
            node.Left = Insert(node.Left, value);
        }
        else
        {
            node.Right = Insert(node.Right, value);
        }

        UpdateSize(node);
        return node;
    }

    private Node InsertAtRoot(Node node, T value)
    {
        var newNode = new Node(value);
        Split(node, value, out newNode.Left, out newNode.Right);
        UpdateSize(newNode);
        return newNode;
    }

    /// <summary>
    /// Splits the subtree into two parts: 'left' contains values &lt; value,
    /// and 'right' contains values &gt;= value.
    /// </summary>
    private void Split(Node node, T value, out Node left, out Node right)
    {
        if (node == null)
        {
            left = null;
            right = null;
            return;
        }

        if (value.CompareTo(node.Value) <= 0)
        {
            Split(node.Left, value, out left, out Node subRight);
            node.Left = subRight;
            right = node;
            UpdateSize(node);
        }
        else
        {
            Split(node.Right, value, out Node subLeft, out right);
            node.Right = subLeft;
            left = node;
            UpdateSize(node);
        }
    }

    /// <summary>
    /// Determines whether the tree contains the specified value.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>True if the value exists in the tree; otherwise false.</returns>
    public bool Contains(T value)
    {
        Node current = _root;
        while (current != null)
        {
            int cmp = value.CompareTo(current.Value);
            if (cmp == 0)
            {
                return true;
            }
            current = cmp < 0 ? current.Left : current.Right;
        }
        return false;
    }

    /// <summary>
    /// Removes a single occurrence of the specified value from the tree.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    /// <returns>True if a value was removed; otherwise false.</returns>
    public bool Remove(T value)
    {
        int before = Count;
        _root = Remove(_root, value);
        return Count < before;
    }

    private Node Remove(Node node, T value)
    {
        if (node == null)
        {
            return null;
        }

        int cmp = value.CompareTo(node.Value);
        if (cmp == 0)
        {
            // Merge the two children to replace this node.
            Node merged = Merge(node.Left, node.Right);
            return merged;
        }

        if (cmp < 0)
        {
            node.Left = Remove(node.Left, value);
        }
        else
        {
            node.Right = Remove(node.Right, value);
        }

        UpdateSize(node);
        return node;
    }

    /// <summary>
    /// Randomly merges two subtrees (all values in 'left' are less than
    /// all values in 'right'), preserving expected balance.
    /// </summary>
    private Node Merge(Node left, Node right)
    {
        if (left == null)
        {
            return right;
        }
        if (right == null)
        {
            return left;
        }

        int total = left.Size + right.Size;
        // Choose the root proportionally to subtree sizes to maintain randomness.
        if (_random.Next(total) < left.Size)
        {
            left.Right = Merge(left.Right, right);
            UpdateSize(left);
            return left;
        }
        else
        {
            right.Left = Merge(left, right.Left);
            UpdateSize(right);
            return right;
        }
    }

    /// <summary>
    /// Returns all elements in sorted (ascending) order.
    /// </summary>
    /// <returns>A list of elements in sorted order.</returns>
    public List<T> InOrderTraversal()
    {
        var result = new List<T>(Count);
        InOrderTraversal(_root, result);
        return result;
    }

    private static void InOrderTraversal(Node node, List<T> result)
    {
        if (node == null)
        {
            return;
        }
        InOrderTraversal(node.Left, result);
        result.Add(node.Value);
        InOrderTraversal(node.Right, result);
    }

    /// <summary>
    /// Removes all elements from the tree.
    /// </summary>
    public void Clear()
    {
        _root = null;
    }
}