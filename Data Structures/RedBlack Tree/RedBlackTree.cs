using System;
using System.Collections;
using System.Collections.Generic;

public enum RedBlackColor
{
    Red,
    Black
}

public class RedBlackTree<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : IComparable<TKey>
{
    internal class Node
    {
        public TKey Key;
        public TValue Value;
        public Node Left;
        public Node Right;
        public Node Parent;
        public RedBlackColor Color;

        public Node(TKey key, TValue value, RedBlackColor color)
        {
            Key = key;
            Value = value;
            Color = color;
        }
    }

    private readonly Node nil;
    private Node root;
    private int count;

    public int Count => count;

    public RedBlackTree()
    {
        nil = new Node(default(TKey), default(TValue), RedBlackColor.Black);
        nil.Left = nil;
        nil.Right = nil;
        nil.Parent = nil;
        root = nil;
        count = 0;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            Node node = SearchNode(key);
            if (node == nil) throw new KeyNotFoundException("The given key was not present in the tree.");
            return node.Value;
        }
        set
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            Node node = SearchNode(key);
            if (node != nil)
            {
                node.Value = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));

        Node z = new Node(key, value, RedBlackColor.Red);
        z.Left = nil;
        z.Right = nil;

        Node y = nil;
        Node x = root;

        while (x != nil && x != nil)
        {
            y = x;
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0)
            {
                x = x.Left;
            }
            else if (cmp > 0)
            {
                x = x.Right;
            }
            else
            {
                throw new ArgumentException("An item with the same key has already been added.");
            }
        }

        z.Parent = y;
        if (y == null || y == nil)
        {
            root = z;
        }
        else
        {
            int cmp = z.Key.CompareTo(y.Key);
            if (cmp < 0)
            {
                y.Left = z;
            }
            else
            {
                y.Right = z;
            }
        }

        InsertFixup(z);
        count++;
    }

    public bool Remove(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        Node z = SearchNode(key);
        if (z == nil)
        {
            return false;
        }
        DeleteNode(z);
        count--;
        return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        Node node = SearchNode(key);
        if (node != nil)
        {
            value = node.Value;
            return true;
        }
        value = default(TValue);
        return false;
    }

    public bool ContainsKey(TKey key)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        return SearchNode(key) != nil;
    }

    public void Clear()
    {
        root = nil;
        count = 0;
    }

    private Node SearchNode(TKey key)
    {
        Node x = root;
        while (x != nil && x != nil)
        {
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0)
            {
                x = x.Left;
            }
            else if (cmp > 0)
            {
                x = x.Right;
            }
            else
            {
                return x;
            }
        }
        return nil;
    }

    private void LeftRotate(Node x)
    {
        Node y = x.Right;
        x.Right = y.Left;
        if (y.Left != nil)
        {
            y.Left.Parent = x;
        }
        y.Parent = x.Parent;
        if (x.Parent == nil)
        {
            root = y;
        }
        else if (x == x.Parent.Left)
        {
            x.Parent.Left = y;
        }
        else
        {
            x.Parent.Right = y;
        }
        y.Left = x;
        x.Parent = y;
    }

    private void RightRotate(Node y)
    {
        Node x = y.Left;
        y.Left = x.Right;
        if (x.Right != nil)
        {
            x.Right.Parent = y;
        }
        x.Parent = y.Parent;
        if (y.Parent == nil)
        {
            root = x;
        }
        else if (y == y.Parent.Right)
        {
            y.Parent.Right = x;
        }
        else
        {
            y.Parent.Left = x;
        }
        x.Right = y;
        y.Parent = x;
    }

    private void InsertFixup(Node z)
    {
        while (z.Parent != nil && z.Parent.Color == RedBlackColor.Red)
        {
            if (z.Parent == z.Parent.Parent.Left)
            {
                Node y = z.Parent.Parent.Right;
                if (y.Color == RedBlackColor.Red)
                {
                    z.Parent.Color = RedBlackColor.Black;
                    y.Color = RedBlackColor.Black;
                    z.Parent.Parent.Color = RedBlackColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Right)
                    {
                        z = z.Parent;
                        LeftRotate(z);
                    }
                    z.Parent.Color = RedBlackColor.Black;
                    z.Parent.Parent.Color = RedBlackColor.Red;
                    RightRotate(z.Parent.Parent);
                }
            }
            else
            {
                Node y = z.Parent.Parent.Left;
                if (y.Color == RedBlackColor.Red)
                {
                    z.Parent.Color = RedBlackColor.Black;
                    y.Color = RedBlackColor.Black;
                    z.Parent.Parent.Color = RedBlackColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Left)
                    {
                        z = z.Parent;
                        RightRotate(z);
                    }
                    z.Parent.Color = RedBlackColor.Black;
                    z.Parent.Parent.Color = RedBlackColor.Red;
                    LeftRotate(z.Parent.Parent);
                }
            }
        }
        root.Color = RedBlackColor.Black;
    }

    private void Transplant(Node u, Node v)
    {
        if (u.Parent == nil)
        {
            root = v;
        }
        else if (u == u.Parent.Left)
        {
            u.Parent.Left = v;
        }
        else
        {
            u.Parent.Right = v;
        }
        v.Parent = u.Parent;
    }

    private Node Minimum(Node node)
    {
        while (node.Left != nil)
        {
            node = node.Left;
        }
        return node;
    }

    private void DeleteNode(Node z)
    {
        Node y = z;
        RedBlackColor yOriginalColor = y.Color;
        Node x;

        if (z.Left == nil)
        {
            x = z.Right;
            Transplant(z, z.Right);
        }
        else if (z.Right == nil)
        {
            x = z.Left;
            Transplant(z, z.Left);
        }
        else
        {
            y = Minimum(z.Right);
            yOriginalColor = y.Color;
            x = y.Right;
            if (y.Parent == z)
            {
                x.Parent = y;
            }
            else
            {
                Transplant(y, y.Right);
                y.Right = z.Right;
                y.Right.Parent = y;
            }
            Transplant(z, y);
            y.Left = z.Left;
            y.Left.Parent = y;
            y.Color = z.Color;
        }

        if (yOriginalColor == RedBlackColor.Black)
        {
            DeleteFixup(x);
        }
        nil.Parent = nil;
    }

    private void DeleteFixup(Node x)
    {
        while (x != root && x.Color == RedBlackColor.Black)
        {
            if (x == x.Parent.Left)
            {
                Node w = x.Parent.Right;
                if (w.Color == RedBlackColor.Red)
                {
                    w.Color = RedBlackColor.Black;
                    x.Parent.Color = RedBlackColor.Red;
                    LeftRotate(x.Parent);
                    w = x.Parent.Right;
                }
                if (w.Left.Color == RedBlackColor.Black && w.Right.Color == RedBlackColor.Black)
                {
                    w.Color = RedBlackColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w.Right.Color == RedBlackColor.Black)
                    {
                        w.Left.Color = RedBlackColor.Black;
                        w.Color = RedBlackColor.Red;
                        RightRotate(w);
                        w = x.Parent.Right;
                    }
                    w.Color = x.Parent.Color;
                    x.Parent.Color = RedBlackColor.Black;
                    w.Right.Color = RedBlackColor.Black;
                    LeftRotate(x.Parent);
                    x = root;
                }
            }
            else
            {
                Node w = x.Parent.Left;
                if (w.Color == RedBlackColor.Red)
                {
                    w.Color = RedBlackColor.Black;
                    x.Parent.Color = RedBlackColor.Red;
                    RightRotate(x.Parent);
                    w = x.Parent.Left;
                }
                if (w.Right.Color == RedBlackColor.Black && w.Left.Color == RedBlackColor.Black)
                {
                    w.Color = RedBlackColor.Red;
                    x = x.Parent;
                }
                else
                {
                    if (w.Left.Color == RedBlackColor.Black)
                    {
                        w.Right.Color = RedBlackColor.Black;
                        w.Color = RedBlackColor.Red;
                        LeftRotate(w);
                        w = x.Parent.Left;
                    }
                    w.Color = x.Parent.Color;
                    x.Parent.Color = RedBlackColor.Black;
                    w.Left.Color = RedBlackColor.Black;
                    RightRotate(x.Parent);
                    x = root;
                }
            }
        }
        x.Color = RedBlackColor.Black;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return InOrderTraversal(root).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<KeyValuePair<TKey, TValue>> InOrderTraversal(Node node)
    {
        if (node != nil)
        {
            foreach (var kvp in InOrderTraversal(node.Left))
            {
                yield return kvp;
            }
            yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
            foreach (var kvp in InOrderTraversal(node.Right))
            {
                yield return kvp;
            }
        }
    }
}