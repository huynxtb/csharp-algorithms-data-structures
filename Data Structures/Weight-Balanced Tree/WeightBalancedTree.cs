using System;

public class WeightBalancedTree<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value;
        public Node Left;
        public Node Right;
        public int Size;

        public Node(T value)
        {
            Value = value;
            Size = 1;
        }
    }

    private Node root;

    public void Insert(T value)
    {
        root = Insert(root, value);
    }

    private Node Insert(Node node, T value)
    {
        if (node == null)
            return new Node(value);

        int cmp = value.CompareTo(node.Value);
        if (cmp < 0)
            node.Left = Insert(node.Left, value);
        else if (cmp > 0)
            node.Right = Insert(node.Right, value);

        node.Size = 1 + Size(node.Left) + Size(node.Right);
        return Balance(node);
    }

    public void Delete(T value)
    {
        root = Delete(root, value);
    }

    private Node Delete(Node node, T value)
    {
        if (node == null) return null;

        int cmp = value.CompareTo(node.Value);
        if (cmp < 0)
            node.Left = Delete(node.Left, value);
        else if (cmp > 0)
            node.Right = Delete(node.Right, value);
        else
        {
            if (node.Right == null) return node.Left;
            if (node.Left == null) return node.Right;

            Node minNode = GetMin(node.Right);
            node.Value = minNode.Value;
            node.Right = Delete(node.Right, minNode.Value);
        }

        node.Size = 1 + Size(node.Left) + Size(node.Right);
        return Balance(node);
    }

    private Node GetMin(Node node)
    {
        while (node.Left != null)
            node = node.Left;
        return node;
    }

    public bool Search(T value)
    {
        return Search(root, value);
    }

    private bool Search(Node node, T value)
    {
        if (node == null) return false;
        int cmp = value.CompareTo(node.Value);
        if (cmp < 0) return Search(node.Left, value);
        else if (cmp > 0) return Search(node.Right, value);
        else return true;
    }

    private Node Balance(Node node)
    {
        int balanceFactor = Size(node.Left) - Size(node.Right);

        if (balanceFactor > 1)
        {
            if (Size(node.Left.Left) < Size(node.Left.Right))
                node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }
        else if (balanceFactor < -1)
        {
            if (Size(node.Right.Right) < Size(node.Right.Left))
                node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    private Node RotateLeft(Node node)
    {
        Node newRoot = node.Right;
        node.Right = newRoot.Left;
        newRoot.Left = node;
        node.Size = 1 + Size(node.Left) + Size(node.Right);
        newRoot.Size = 1 + Size(newRoot.Left) + Size(newRoot.Right);
        return newRoot;
    }

    private Node RotateRight(Node node)
    {
        Node newRoot = node.Left;
        node.Left = newRoot.Right;
        newRoot.Right = node;
        node.Size = 1 + Size(node.Left) + Size(node.Right);
        newRoot.Size = 1 + Size(newRoot.Left) + Size(newRoot.Right);
        return newRoot;
    }

    private int Size(Node node)
    {
        return node == null ? 0 : node.Size;
    }
}