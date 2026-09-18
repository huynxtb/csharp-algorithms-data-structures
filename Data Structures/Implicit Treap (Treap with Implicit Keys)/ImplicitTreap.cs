using System;
using System.Collections.Generic;

/// <summary>
/// An Implicit Treap is a balanced binary search tree that uses implicit keys (array indices)
/// instead of explicit keys, combined with random priorities to maintain balance.
/// </summary>
/// <typeparam name="T">The type of elements stored in the treap.</typeparam>
public class ImplicitTreap<T>
{
    private class Node
    {
        public T Value { get; set; }
        public int Priority { get; set; }
        public int Size { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(T value, int priority)
        {
            Value = value;
            Priority = priority;
            Size = 1;
            Left = null;
            Right = null;
        }
    }

    private Node _root;
    private static readonly Random _random = new Random();

    /// <summary>
    /// Initializes a new instance of the ImplicitTreap class.
    /// </summary>
    public ImplicitTreap()
    {
        _root = null;
    }

    /// <summary>
    /// Gets the number of elements in the treap.
    /// </summary>
    public int Count
    {
        get { return GetSize(_root); }
    }

    /// <summary>
    /// Gets the size of a subtree (0 if node is null).
    /// </summary>
    /// <param name="node">The node whose subtree size to calculate.</param>
    /// <returns>The size of the subtree rooted at node.</returns>
    private int GetSize(Node node)
    {
        return node == null ? 0 : node.Size;
    }

    /// <summary>
    /// Updates the size of a node based on its children.
    /// </summary>
    /// <param name="node">The node to update.</param>
    private void UpdateSize(Node node)
    {
        if (node != null)
        {
            node.Size = 1 + GetSize(node.Left) + GetSize(node.Right);
        }
    }

    /// <summary>
    /// Splits the treap at the specified key position.
    /// </summary>
    /// <param name="node">The root of the treap to split.</param>
    /// <param name="key">The position at which to split (0-based index).</param>
    /// <param name="left">Output: the left subtreap (elements at indices 0..key-1).</param>
    /// <param name="right">Output: the right subtreap (elements at indices key..).</param>
    private void Split(Node node, int key, out Node left, out Node right)
    {
        if (node == null)
        {
            left = null;
            right = null;
            return;
        }

        int leftSize = GetSize(node.Left);

        if (key <= leftSize)
        {
            Split(node.Left, key, out left, out Node temp);
            node.Left = temp;
            UpdateSize(node);
            right = node;
        }
        else
        {
            Split(node.Right, key - leftSize - 1, out Node temp, out right);
            node.Right = temp;
            UpdateSize(node);
            left = node;
        }
    }

    /// <summary>
    /// Merges two treaps maintaining the heap property.
    /// </summary>
    /// <param name="left">The left treap (all keys less than right).</param>
    /// <param name="right">The right treap (all keys greater than left).</param>
    /// <returns>The merged treap.</returns>
    private Node Merge(Node left, Node right)
    {
        if (left == null)
            return right;
        if (right == null)
            return left;

        if (left.Priority > right.Priority)
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
    /// Inserts a value at the specified index.
    /// </summary>
    /// <param name="index">The 0-based index at which to insert.</param>
    /// <param name="value">The value to insert.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
    public void Insert(int index, T value)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        Node newNode = new Node(value, _random.Next());
        Split(_root, index, out Node left, out Node right);
        _root = Merge(Merge(left, newNode), right);
    }

    /// <summary>
    /// Removes the element at the specified index.
    /// </summary>
    /// <param name="index">The 0-based index of the element to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        Split(_root, index, out Node left, out Node temp1);
        Split(temp1, 1, out Node _, out Node right);
        _root = Merge(left, right);
    }

    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The 0-based index of the element to retrieve.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
    public T GetAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        Node current = _root;
        while (current != null)
        {
            int leftSize = GetSize(current.Left);
            if (index == leftSize)
                return current.Value;
            if (index < leftSize)
                current = current.Left;
            else
            {
                index -= leftSize + 1;
                current = current.Right;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(index));
    }

    /// <summary>
    /// Sets the element at the specified index.
    /// </summary>
    /// <param name="index">The 0-based index of the element to set.</param>
    /// <param name="value">The new value.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
    public void SetAt(int index, T value)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        Node current = _root;
        while (current != null)
        {
            int leftSize = GetSize(current.Left);
            if (index == leftSize)
            {
                current.Value = value;
                return;
            }
            if (index < leftSize)
                current = current.Left;
            else
            {
                index -= leftSize + 1;
                current = current.Right;
            }
        }
    }

    /// <summary>
    /// Inserts a value at the beginning of the treap.
    /// </summary>
    /// <param name="value">The value to insert.</param>
    public void AddFirst(T value)
    {
        Insert(0, value);
    }

    /// <summary>
    /// Inserts a value at the end of the treap.
    /// </summary>
    /// <param name="value">The value to insert.</param>
    public void AddLast(T value)
    {
        Insert(Count, value);
    }

    /// <summary>
    /// Returns all elements as an array in order.
    /// </summary>
    /// <returns>An array containing all elements in order.</returns>
    public T[] ToArray()
    {
        T[] result = new T[Count];
        int index = 0;
        InOrderTraversal(_root, result, ref index);
        return result;
    }

    /// <summary>
    /// Performs in-order traversal to populate an array.
    /// </summary>
    /// <param name="node">The current node.</param>
    /// <param name="result">The result array.</param>
    /// <param name="index">The current index in the result array.</param>
    private void InOrderTraversal(Node node, T[] result, ref int index)
    {
        if (node == null)
            return;

        InOrderTraversal(node.Left, result, ref index);
        result[index++] = node.Value;
        InOrderTraversal(node.Right, result, ref index);
    }

    /// <summary>
    /// Extracts a subsequence as a new treap.
    /// </summary>
    /// <param name="startIndex">The 0-based start index of the subsequence.</param>
    /// <param name="length">The length of the subsequence.</param>
    /// <returns>A new ImplicitTreap containing the subsequence.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the range is invalid.</exception>
    public ImplicitTreap<T> SubSequence(int startIndex, int length)
    {
        if (startIndex < 0 || length < 0 || startIndex + length > Count)
            throw new ArgumentOutOfRangeException("Invalid subsequence range.");

        ImplicitTreap<T> result = new ImplicitTreap<T>();

        Split(_root, startIndex, out Node _, out Node temp1);
        Split(temp1, length, out Node middle, out Node __);
        _root = Merge(Split(_root, startIndex, out Node left, out _) == null ? null : left, Merge(middle, __));

        // Rebuild: extract elements and create new treap
        for (int i = 0; i < length; i++)
        {
            result.AddLast(GetAt(startIndex + i));
        }

        return result;
    }
}