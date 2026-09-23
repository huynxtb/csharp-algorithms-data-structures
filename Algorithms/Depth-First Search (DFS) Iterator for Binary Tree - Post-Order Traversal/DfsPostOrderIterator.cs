using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a node in a binary tree with a generic value type.
/// </summary>
/// <typeparam name="T">The type of value stored in the node.</typeparam>
public class TreeNode<T>
{
    /// <summary>
    /// Gets or sets the value stored in this node.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Gets or sets the left child node.
    /// </summary>
    public TreeNode<T> Left { get; set; }

    /// <summary>
    /// Gets or sets the right child node.
    /// </summary>
    public TreeNode<T> Right { get; set; }

    /// <summary>
    /// Initializes a new instance of the TreeNode class with the specified value.
    /// </summary>
    /// <param name="value">The value to store in the node.</param>
    public TreeNode(T value)
    {
        Value = value;
        Left = null;
        Right = null;
    }
}

/// <summary>
/// Provides a lazy, iterator-based depth-first search traversal of a binary tree using post-order strategy.
/// Post-order traversal visits nodes in the order: left subtree → right subtree → root.
/// </summary>
/// <typeparam name="T">The type of values stored in the tree nodes.</typeparam>
public class DfsPostOrderIterator<T> : IEnumerable<T>, IEnumerator<T>
{
    private readonly TreeNode<T> _root;
    private Stack<(TreeNode<T> node, bool childrenProcessed)> _traversalStack;
    private T _current;
    private bool _initialized;
    private bool _disposed;

    /// <summary>
    /// Gets the current element in the traversal.
    /// </summary>
    public T Current
    {
        get
        {
            if (!_initialized)
                throw new InvalidOperationException("Enumeration has not started. Call MoveNext first.");
            return _current;
        }
    }

    /// <summary>
    /// Gets the current element in the traversal (explicit interface implementation).
    /// </summary>
    object IEnumerator.Current => Current;

    /// <summary>
    /// Initializes a new instance of the DfsPostOrderIterator class for the specified binary tree root.
    /// </summary>
    /// <param name="root">The root node of the binary tree to traverse. Can be null for an empty tree.</param>
    public DfsPostOrderIterator(TreeNode<T> root)
    {
        _root = root;
        _initialized = false;
        _disposed = false;
        _traversalStack = new Stack<(TreeNode<T>, bool)>();
    }

    /// <summary>
    /// Advances the enumerator to the next element in post-order traversal.
    /// </summary>
    /// <returns>true if the enumerator successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
    public bool MoveNext()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(DfsPostOrderIterator<T>));

        // Initialize the stack on first call
        if (!_initialized)
        {
            _initialized = true;
            if (_root != null)
            {
                _traversalStack.Push((_root, false));
            }
        }

        // Process the stack to find the next node in post-order
        while (_traversalStack.Count > 0)
        {
            var (node, childrenProcessed) = _traversalStack.Pop();

            if (childrenProcessed)
            {
                // Both children have been processed, so visit this node
                _current = node.Value;
                return true;
            }
            else
            {
                // Push the node back with childrenProcessed = true
                _traversalStack.Push((node, true));

                // Push right child first (so it's processed after left child due to stack LIFO)
                if (node.Right != null)
                {
                    _traversalStack.Push((node.Right, false));
                }

                // Push left child second (so it's processed first due to stack LIFO)
                if (node.Left != null)
                {
                    _traversalStack.Push((node.Left, false));
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Resets the enumerator to its initial position.
    /// </summary>
    public void Reset()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(DfsPostOrderIterator<T>));

        _initialized = false;
        _traversalStack.Clear();
    }

    /// <summary>
    /// Releases all resources used by the enumerator.
    /// </summary>
    public void Dispose()
    {
        if (!_disposed)
        {
            _traversalStack?.Clear();
            _traversalStack = null;
            _disposed = true;
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the binary tree in post-order DFS.
    /// </summary>
    /// <returns>An enumerator for the binary tree.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        return new DfsPostOrderIterator<T>(_root);
    }

    /// <summary>
    /// Returns an enumerator that iterates through the binary tree in post-order DFS (explicit interface implementation).
    /// </summary>
    /// <returns>An enumerator for the binary tree.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}