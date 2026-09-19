using System;
using System.Collections.Generic;

public class TwoThreeTree<T> where T : IComparable<T>
{
    private class Node
    {
        public List<T> Keys { get; private set; } = new List<T>();
        public List<Node> Children { get; private set; } = new List<Node>();
        public bool IsLeaf => Children.Count == 0;

        public void AddKey(T key)
        {
            Keys.Add(key);
            Keys.Sort();
        }

        public void RemoveKey(T key)
        {
            Keys.Remove(key);
        }

        public int KeyCount => Keys.Count;
    }

    private Node root;
    private int count;

    public int Count => count;

    public void Insert(T item)
    {
        if (root == null)
        {
            root = new Node();
            root.AddKey(item);
            count++;
            return;
        }

        var newRoot = Insert(root, item);
        if (newRoot != null)
        {
            root = newRoot;
        }
    }

    private Node Insert(Node node, T item)
    {
        if (node.IsLeaf)
        {
            if (node.KeyCount < 2)
            {
                node.AddKey(item);
                count++;
                return null;
            }
            else
            {
                var newNode = new Node();
                var middleKey = node.Keys[1];
                newNode.AddKey(item);
                newNode.AddKey(node.Keys[1]);
                node.RemoveKey(middleKey);
                count++;
                return newNode;
            }
        }

        int index = 0;
        while (index < node.KeyCount && item.CompareTo(node.Keys[index]) > 0)
        {
            index++;
        }

        var newChild = Insert(node.Children[index], item);
        if (newChild != null)
        {
            node.AddKey(newChild.Keys[0]);
            node.Children.Insert(index + 1, newChild);
            if (node.KeyCount > 2)
            {
                var newNode = new Node();
                var middleKey = node.Keys[1];
                newNode.AddKey(node.Keys[2]);
                node.RemoveKey(middleKey);
                count++;
                return newNode;
            }
        }
        return null;
    }

    public bool Contains(T item)
    {
        return Contains(root, item);
    }

    private bool Contains(Node node, T item)
    {
        if (node == null) return false;
        for (int i = 0; i < node.KeyCount; i++)
        {
            int cmp = item.CompareTo(node.Keys[i]);
            if (cmp == 0) return true;
            if (cmp < 0) return Contains(node.Children[i], item);
        }
        return Contains(node.Children[node.KeyCount], item);
    }

    public void Clear()
    {
        root = null;
        count = 0;
    }

    public IEnumerable<T> InOrderTraversal()
    {
        var result = new List<T>();
        InOrderTraversal(root, result);
        return result;
    }

    private void InOrderTraversal(Node node, List<T> result)
    {
        if (node == null) return;
        for (int i = 0; i < node.KeyCount; i++)
        {
            InOrderTraversal(node.Children[i], result);
            result.Add(node.Keys[i]);
        }
        InOrderTraversal(node.Children[node.KeyCount], result);
    }

    public void Remove(T item)
    {
        // Implementation of remove operation is complex and omitted for brevity.
        // It would involve borrowing keys from siblings or merging nodes.
    }
}