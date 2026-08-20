using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

/// <summary>
/// Represents a Ternary Search Tree (TST) that associates string keys with generic values.
/// </summary>
/// <typeparam name="TValue">The type of the value associated with each key.</typeparam>
public class TernarySearchTree<TValue>
{
    private class Node
    {
        public char CharValue { get; set; }
        public Node? Left { get; set; }
        public Node? Mid { get; set; }
        public Node? Right { get; set; }
        public TValue? Value { get; set; }
        public bool IsEndOfKey { get; set; }

        public Node(char charValue)
        {
            CharValue = charValue;
        }
    }

    private Node? _root;

    /// <summary>
    /// Inserts a key-value pair into the Ternary Search Tree.
    /// </summary>
    /// <param name="key">The string key to insert.</param>
    /// <param name="value">The value associated with the key.</param>
    /// <exception cref="ArgumentNullException">Thrown when the key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the key is empty.</exception>
    public void Insert(string key, TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (key.Length == 0) throw new ArgumentException("Key cannot be empty.", nameof(key));

        _root = Insert(_root, key, value, 0);
    }

    private Node Insert(Node? node, string key, TValue value, int index)
    {
        char c = key[index];
        if (node == null)
        {
            node = new Node(c);
        }

        if (c < node.CharValue)
        {
            node.Left = Insert(node.Left, key, value, index);
        }
        else if (c > node.CharValue)
        {
            node.Right = Insert(node.Right, key, value, index);
        }
        else if (index < key.Length - 1)
        {
            node.Mid = Insert(node.Mid, key, value, index + 1);
        }
        else
        {
            node.Value = value;
            node.IsEndOfKey = true;
        }

        return node;
    }

    /// <summary>
    /// Retrieves the value associated with the specified key.
    /// </summary>
    /// <param name="key">The key to locate.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
    /// <returns>true if the key is found; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the key is null.</exception>
    public bool TryGetValue(string key, out TValue? value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (key.Length == 0)
        {
            value = default;
            return false;
        }

        Node? node = Search(_root, key, 0);
        if (node != null && node.IsEndOfKey)
        {
            value = node.Value;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Determines whether the Ternary Search Tree contains the specified key.
    /// </summary>
    /// <param name="key">The key to locate.</param>
    /// <returns>true if the key is found; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the key is null.</exception>
    public bool Contains(string key)
    {
        return TryGetValue(key, out _);
    }

    /// <summary>
    /// Removes the key and its associated value from the Ternary Search Tree.
    /// </summary>
    /// <param name="key">The key to delete.</param>
    /// <returns>true if the key was successfully found and deleted; otherwise, false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the key is null.</exception>
    public bool Delete(string key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (key.Length == 0) return false;

        bool deleted = false;
        _root = Delete(_root, key, 0, ref deleted);
        return deleted;
    }

    private Node? Delete(Node? node, string key, int index, ref bool deleted)
    {
        if (node == null) return null;

        char c = key[index];
        if (c < node.CharValue)
        {
            node.Left = Delete(node.Left, key, index, ref deleted);
        }
        else if (c > node.CharValue)
        {
            node.Right = Delete(node.Right, key, index, ref deleted);
        }
        else if (index < key.Length - 1)
        {
            node.Mid = Delete(node.Mid, key, index + 1, ref deleted);
        }
        else
        {
            if (node.IsEndOfKey)
            {
                node.IsEndOfKey = false;
                node.Value = default;
                deleted = true;
            }
        }

        if (!node.IsEndOfKey && node.Left == null && node.Mid == null && node.Right == null)
        {
            return null;
        }

        return node;
    }

    /// <summary>
    /// Returns all keys in the Ternary Search Tree that start with the specified prefix.
    /// </summary>
    /// <param name="prefix">The prefix to search for.</param>
    /// <returns>An enumerable collection of keys that match the prefix.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the prefix is null.</exception>
    public IEnumerable<string> KeysWithPrefix(string prefix)
    {
        if (prefix == null) throw new ArgumentNullException(nameof(prefix));

        var results = new List<string>();
        if (prefix.Length == 0)
        {
            Collect(_root, new StringBuilder(), results);
            return results;
        }

        Node? node = Search(_root, prefix, 0);
        if (node == null) return results;

        if (node.IsEndOfKey)
        {
            results.Add(prefix);
        }

        var sb = new StringBuilder(prefix);
        Collect(node.Mid, sb, results);

        return results;
    }

    private Node? Search(Node? node, string key, int index)
    {
        if (node == null) return null;

        char c = key[index];
        if (c < node.CharValue)
        {
            return Search(node.Left, key, index);
        }
        else if (c > node.CharValue)
        {
            return Search(node.Right, key, index);
        }
        else if (index < key.Length - 1)
        {
            return Search(node.Mid, key, index + 1);
        }
        else
        {
            return node;
        }
    }

    private void Collect(Node? node, StringBuilder prefix, List<string> results)
    {
        if (node == null) return;

        Collect(node.Left, prefix, results);

        prefix.Append(node.CharValue);
        if (node.IsEndOfKey)
        {
            results.Add(prefix.ToString());
        }
        Collect(node.Mid, prefix, results);
        prefix.Length--;

        Collect(node.Right, prefix, results);
    }
}