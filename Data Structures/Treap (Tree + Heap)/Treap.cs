using System;
using System.Collections.Generic;

public class Treap<TKey> where TKey : IComparable<TKey>
{
    private class Node
    {
        public TKey Key;
        public int Priority;
        public Node Left;
        public Node Right;

        public Node(TKey key, int priority)
        {
            Key = key;
            Priority = priority;
            Left = null;
            Right = null;
        }
    }

    private Node root;
    private Random random;

    public Treap()
    {
        root = null;
        random = new Random();
    }

    /// <summary>
    /// Inserts the specified key into the treap.
    /// </summary>
    public void Insert(TKey key)
    {
        root = Insert(root, key);
    }

    private Node Insert(Node node, TKey key)
    {
        if (node == null)
        {
            return new Node(key, random.Next());
        }

        int cmp = key.CompareTo(node.Key);
        if (cmp < 0)
        {
            node.Left = Insert(node.Left, key);
            if (node.Left.Priority > node.Priority)
                node = RotateRight(node);
        }
        else if (cmp > 0)
        {
            node.Right = Insert(node.Right, key);
            if (node.Right.Priority > node.Priority)
                node = RotateLeft(node);
        }
        else
        {
            // Key already exists, do nothing or update as needed
        }

        return node;
    }

    /// <summary>
    /// Deletes the specified key from the treap.
    /// </summary>
    public void Delete(TKey key)
    {
        root = Delete(root, key);
    }

    private Node Delete(Node node, TKey key)
    {
        if (node == null) return null;

        int cmp = key.CompareTo(node.Key);
        if (cmp < 0)
        {
            node.Left = Delete(node.Left, key);
        }
        else if (cmp > 0)
        {
            node.Right = Delete(node.Right, key);
        }
        else
        {
            // Node to be deleted found
            if (node.Left == null && node.Right == null)
            {
                return null;
            }
            else if (node.Left == null)
            {
                node = RotateLeft(node);
                node.Left = Delete(node.Left, key);
            }
            else if (node.Right == null)
            {
                node = RotateRight(node);
                node.Right = Delete(node.Right, key);
            }
            else
            {
                if (node.Left.Priority > node.Right.Priority)
                {
                    node = RotateRight(node);
                    node.Right = Delete(node.Right, key);
                }
                else
                {
                    node = RotateLeft(node);
                    node.Left = Delete(node.Left, key);
                }
            }
        }
        return node;
    }

    /// <summary>
    /// Checks if the treap contains the specified key.
    /// </summary>
    public bool Contains(TKey key)
    {
        Node current = root;
        while (current != null)
        {
            int cmp = key.CompareTo(current.Key);
            if (cmp < 0) current = current.Left;
            else if (cmp > 0) current = current.Right;
            else return true;
        }
        return false;
    }

    /// <summary>
    /// Returns an IEnumerable of the keys in sorted order.
    /// </summary>
    public IEnumerable<TKey> InOrderTraversal()
    {
        return InOrderTraversal(root);
    }

    private IEnumerable<TKey> InOrderTraversal(Node node)
    {
        if (node != null)
        {
            foreach (var key in InOrderTraversal(node.Left))
                yield return key;
            yield return node.Key;
            foreach (var key in InOrderTraversal(node.Right))
                yield return key;
        }
    }

    private Node RotateRight(Node y)
    {
        Node x = y.Left;
        Node T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        return x;
    }

    private Node RotateLeft(Node x)
    {
        Node y = x.Right;
        Node T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        return y;
    }
}