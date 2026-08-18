using System;
using System.Collections.Generic;

/// <summary>
/// Represents a generic B-Tree data structure.
/// </summary>
/// <typeparam name="TKey">The type of keys in the B-Tree.</typeparam>
/// <typeparam name="TValue">The type of values in the B-Tree.</typeparam>
public class BTree<TKey, TValue> where TKey : IComparable<TKey>
{
    private readonly int _t; // Minimum degree
    private BTreeNode _root;

    /// <summary>
    /// Initializes a new instance of the B-Tree class with the specified minimum degree.
    /// </summary>
    /// <param name="minimumDegree">The minimum degree of the B-Tree (must be >= 2).</param>
    public BTree(int minimumDegree)
    {
        if (minimumDegree < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumDegree), "Minimum degree must be at least 2.");
        }
        _t = minimumDegree;
        _root = new BTreeNode(true);
    }

    private class BTreeNode
    {
        public List<TKey> Keys { get; }
        public List<TValue> Values { get; }
        public List<BTreeNode> Children { get; }
        public bool IsLeaf { get; set; }

        public BTreeNode(bool isLeaf)
        {
            Keys = new List<TKey>();
            Values = new List<TValue>();
            Children = new List<BTreeNode>();
            IsLeaf = isLeaf;
        }
    }

    /// <summary>
    /// Searches for a key in the B-Tree and returns the associated value.
    /// </summary>
    /// <param name="key">The key to search for.</param>
    /// <returns>The value associated with the key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the key is not found.</exception>
    public TValue Search(TKey key)
    {
        var result = SearchNode(_root, key);
        if (result == null)
        {
            throw new KeyNotFoundException("The given key was not present in the B-Tree.");
        }
        return result.Value.Value;
    }

    private (BTreeNode Node, int Index)? SearchNode(BTreeNode node, TKey key)
    {
        int i = 0;
        while (i < node.Keys.Count && key.CompareTo(node.Keys[i]) > 0)
        {
            i++;
        }

        if (i < node.Keys.Count && key.CompareTo(node.Keys[i]) == 0)
        {
            return (node, i);
        }

        if (node.IsLeaf)
        {
            return null;
        }

        return SearchNode(node.Children[i], key);
    }

    /// <summary>
    /// Inserts a key-value pair into the B-Tree. If the key already exists, updates its value.
    /// </summary>
    /// <param name="key">The key to insert.</param>
    /// <param name="value">The value associated with the key.</param>
    public void Insert(TKey key, TValue value)
    {
        BTreeNode r = _root;
        if (r.Keys.Count == 2 * _t - 1)
        {
            BTreeNode s = new BTreeNode(false);
            _root = s;
            s.Children.Add(r);
            SplitChild(s, 0, r);
            InsertNonFull(s, key, value);
        }
        else
        {
            InsertNonFull(r, key, value);
        }
    }

    private void SplitChild(BTreeNode parent, int i, BTreeNode child)
    {
        BTreeNode z = new BTreeNode(child.IsLeaf);

        for (int j = 0; j < _t - 1; j++)
        {
            z.Keys.Add(child.Keys[j + _t]);
            z.Values.Add(child.Values[j + _t]);
        }

        if (!child.IsLeaf)
        {
            for (int j = 0; j < _t; j++)
            {
                z.Children.Add(child.Children[j + _t]);
            }
        }

        TKey medianKey = child.Keys[_t - 1];
        TValue medianValue = child.Values[_t - 1];

        child.Keys.RemoveRange(_t - 1, _t);
        child.Values.RemoveRange(_t - 1, _t);
        if (!child.IsLeaf)
        {
            child.Children.RemoveRange(_t, _t);
        }

        parent.Children.Insert(i + 1, z);
        parent.Keys.Insert(i, medianKey);
        parent.Values.Insert(i, medianValue);
    }

    private void InsertNonFull(BTreeNode node, TKey key, TValue value)
    {
        int i = node.Keys.Count - 1;

        if (node.IsLeaf)
        {
            while (i >= 0 && key.CompareTo(node.Keys[i]) < 0)
            {
                i--;
            }
            if (i >= 0 && key.CompareTo(node.Keys[i]) == 0)
            {
                node.Values[i] = value;
            }
            else
            {
                node.Keys.Insert(i + 1, key);
                node.Values.Insert(i + 1, value);
            }
        }
        else
        {
            while (i >= 0 && key.CompareTo(node.Keys[i]) < 0)
            {
                i--;
            }
            if (i >= 0 && key.CompareTo(node.Keys[i]) == 0)
            {
                node.Values[i] = value;
                return;
            }
            i++;
            if (node.Children[i].Keys.Count == 2 * _t - 1)
            {
                SplitChild(node, i, node.Children[i]);
                int compare = key.CompareTo(node.Keys[i]);
                if (compare == 0)
                {
                    node.Values[i] = value;
                    return;
                }
                else if (compare > 0)
                {
                    i++;
                }
            }
            InsertNonFull(node.Children[i], key, value);
        }
    }

    /// <summary>
    /// Deletes a key and its associated value from the B-Tree.
    /// </summary>
    /// <param name="key">The key to delete.</param>
    /// <returns>True if the key was found and deleted; otherwise, false.</returns>
    public bool Delete(TKey key)
    {
        bool deleted = DeleteNode(_root, key);
        if (deleted && _root.Keys.Count == 0 && !_root.IsLeaf)
        {
            _root = _root.Children[0];
        }
        return deleted;
    }

    private bool DeleteNode(BTreeNode node, TKey key)
    {
        int idx = FindKey(node, key);

        if (idx < node.Keys.Count && node.Keys[idx].CompareTo(key) == 0)
        {
            if (node.IsLeaf)
            {
                RemoveFromLeaf(node, idx);
            }
            else
            {
                RemoveFromNonLeaf(node, idx);
            }
            return true;
        }
        else
        {
            if (node.IsLeaf)
            {
                return false;
            }

            bool flag = (idx == node.Keys.Count);

            if (node.Children[idx].Keys.Count < _t)
            {
                Fill(node, idx);
            }

            if (flag && idx > node.Keys.Count)
            {
                return DeleteNode(node.Children[idx - 1], key);
            }
            else
            {
                return DeleteNode(node.Children[idx], key);
            }
        }
    }

    private int FindKey(BTreeNode node, TKey key)
    {
        int idx = 0;
        while (idx < node.Keys.Count && node.Keys[idx].CompareTo(key) < 0)
        {
            idx++;
        }
        return idx;
    }

    private void RemoveFromLeaf(BTreeNode node, int idx)
    {
        node.Keys.RemoveAt(idx);
        node.Values.RemoveAt(idx);
    }

    private void RemoveFromNonLeaf(BTreeNode node, int idx)
    {
        TKey key = node.Keys[idx];

        if (node.Children[idx].Keys.Count >= _t)
        {
            var pred = GetPred(node, idx);
            node.Keys[idx] = pred.Key;
            node.Values[idx] = pred.Value;
            DeleteNode(node.Children[idx], pred.Key);
        }
        else if (node.Children[idx + 1].Keys.Count >= _t)
        {
            var succ = GetSucc(node, idx);
            node.Keys[idx] = succ.Key;
            node.Values[idx] = succ.Value;
            DeleteNode(node.Children[idx + 1], succ.Key);
        }
        else
        {
            Merge(node, idx);
            DeleteNode(node.Children[idx], key);
        }
    }

    private (TKey Key, TValue Value) GetPred(BTreeNode node, int idx)
    {
        BTreeNode curr = node.Children[idx];
        while (!curr.IsLeaf)
        {
            curr = curr.Children[curr.Keys.Count];
        }
        return (curr.Keys[curr.Keys.Count - 1], curr.Values[curr.Keys.Count - 1]);
    }

    private (TKey Key, TValue Value) GetSucc(BTreeNode node, int idx)
    {
        BTreeNode curr = node.Children[idx + 1];
        while (!curr.IsLeaf)
        {
            curr = curr.Children[0];
        }
        return (curr.Keys[0], curr.Values[0]);
    }

    private void Fill(BTreeNode node, int idx)
    {
        if (idx != 0 && node.Children[idx - 1].Keys.Count >= _t)
        {
            BorrowFromPrev(node, idx);
        }
        else if (idx != node.Keys.Count && node.Children[idx + 1].Keys.Count >= _t)
        {
            BorrowFromNext(node, idx);
        }
        else
        {
            if (idx != node.Keys.Count)
            {
                Merge(node, idx);
            }
            else
            {
                Merge(node, idx - 1);
            }
        }
    }

    private void BorrowFromPrev(BTreeNode node, int idx)
    {
        BTreeNode child = node.Children[idx];
        BTreeNode sibling = node.Children[idx - 1];

        child.Keys.Insert(0, node.Keys[idx - 1]);
        child.Values.Insert(0, node.Values[idx - 1]);

        if (!child.IsLeaf)
        {
            child.Children.Insert(0, sibling.Children[sibling.Children.Count - 1]);
            sibling.Children.RemoveAt(sibling.Children.Count - 1);
        }

        node.Keys[idx - 1] = sibling.Keys[sibling.Keys.Count - 1];
        node.Values[idx - 1] = sibling.Values[sibling.Values.Count - 1];

        sibling.Keys.RemoveAt(sibling.Keys.Count - 1);
        sibling.Values.RemoveAt(sibling.Values.Count - 1);
    }

    private void BorrowFromNext(BTreeNode node, int idx)
    {
        BTreeNode child = node.Children[idx];
        BTreeNode sibling = node.Children[idx + 1];

        child.Keys.Add(node.Keys[idx]);
        child.Values.Add(node.Values[idx]);

        if (!child.IsLeaf)
        {
            child.Children.Add(sibling.Children[0]);
            sibling.Children.RemoveAt(0);
        }

        node.Keys[idx] = sibling.Keys[0];
        node.Values[idx] = sibling.Values[0];

        sibling.Keys.RemoveAt(0);
        sibling.Values.RemoveAt(0);
    }

    private void Merge(BTreeNode node, int idx)
    {
        BTreeNode child = node.Children[idx];
        BTreeNode sibling = node.Children[idx + 1];

        child.Keys.Add(node.Keys[idx]);
        child.Values.Add(node.Values[idx]);

        for (int i = 0; i < sibling.Keys.Count; i++)
        {
            child.Keys.Add(sibling.Keys[i]);
            child.Values.Add(sibling.Values[i]);
        }

        if (!child.IsLeaf)
        {
            for (int i = 0; i < sibling.Children.Count; i++)
            {
                child.Children.Add(sibling.Children[i]);
            }
        }

        node.Keys.RemoveAt(idx);
        node.Values.RemoveAt(idx);
        node.Children.RemoveAt(idx + 1);
    }
}