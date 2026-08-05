using System;
using System.Collections.Generic; 

public class AvlTree<T> where T : IComparable<T>
{
    private class AvlNode<TNode>
    {
        public TNode Value { get; set; }
        public AvlNode<TNode> Left { get; set; }
        public AvlNode<TNode> Right { get; set; }
        public int Height { get; set; }

        public AvlNode(TNode value)
        {
            Value = value;
            Height = 1;
        }
    }

    private AvlNode<T> root;

    public int Count { get; private set; }

    public void Insert(T value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        root = Insert(root, value);
    }

    private AvlNode<T> Insert(AvlNode<T> node, T value)
    {
        if (node == null)
        {
            Count++;
            return new AvlNode<T>(value);
        }

        int compare = value.CompareTo(node.Value);
        if (compare < 0)
        {
            node.Left = Insert(node.Left, value);
        }
        else if (compare > 0)
        {
            node.Right = Insert(node.Right, value);
        }
        else
        {
            return node;
        }

        UpdateHeight(node);
        return Balance(node);
    }

    public void Delete(T value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        root = Delete(root, value);
    }

    private AvlNode<T> Delete(AvlNode<T> node, T value)
    {
        if (node == null) return null;

        int compare = value.CompareTo(node.Value);
        if (compare < 0)
        {
            node.Left = Delete(node.Left, value);
        }
        else if (compare > 0)
        {
            node.Right = Delete(node.Right, value);
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
                AvlNode<T> successor = MinValueNode(node.Right);
                node.Value = successor.Value;
                node.Right = Delete(node.Right, successor.Value);
                Count++;
            }
        }

        if (node == null) return null;

        UpdateHeight(node);
        return Balance(node);
    }

    public bool Contains(T value)
    {
        if (value == null) return false;
        AvlNode<T> current = root;
        while (current != null)
        {
            int compare = value.CompareTo(current.Value);
            if (compare < 0)
                current = current.Left;
            else if (compare > 0)
                current = current.Right;
            else
                return true;
        }
        return false;
    }

    public IEnumerable<T> InOrderTraversal()
    {
        return InOrderTraversal(root);
    }

    private IEnumerable<T> InOrderTraversal(AvlNode<T> node)
    {
        if (node != null)
        {
            foreach (var val in InOrderTraversal(node.Left))
            {
                yield return val;
            }
            yield return node.Value;
            foreach (var val in InOrderTraversal(node.Right))
            {
                yield return val;
            }
        }
    }

    private AvlNode<T> MinValueNode(AvlNode<T> node)
    {
        AvlNode<T> current = node;
        while (current.Left != null)
        {
            current = current.Left;
        }
        return current;
    }

    private int GetHeight(AvlNode<T> node)
    {
        return node?.Height ?? 0;
    }

    private int GetBalance(AvlNode<T> node)
    {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
    }

    private void UpdateHeight(AvlNode<T> node)
    {
        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
    }

    private AvlNode<T> RotateRight(AvlNode<T> y)
    {
        AvlNode<T> x = y.Left;
        AvlNode<T> T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        UpdateHeight(y);
        UpdateHeight(x);

        return x;
    }

    private AvlNode<T> RotateLeft(AvlNode<T> x)
    {
        AvlNode<T> y = x.Right;
        AvlNode<T> T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }

    private AvlNode<T> Balance(AvlNode<T> node)
    {
        int balance = GetBalance(node);

        if (balance > 1 && GetBalance(node.Left) >= 0)
        {
            return RotateRight(node);
        }

        if (balance > 1 && GetBalance(node.Left) < 0)
        {
            node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        if (balance < -1 && GetBalance(node.Right) <= 0)
        {
            return RotateLeft(node);
        }

        if (balance < -1 && GetBalance(node.Right) > 0)
        {
            node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }
}