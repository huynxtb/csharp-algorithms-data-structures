using System;
using System.Collections.Generic;

public class BalancedBinaryTree
{
    private class Node
    {
        public int Value;
        public Node Left;
        public Node Right;
        public int Height;

        public Node(int value)
        {
            Value = value;
            Height = 1;
        }
    }

    private Node root;

    public void Insert(int value)
    {
        root = Insert(root, value);
    }

    private Node Insert(Node node, int value)
    {
        if (node == null)
            return new Node(value);

        if (value < node.Value)
            node.Left = Insert(node.Left, value);
        else if (value > node.Value)
            node.Right = Insert(node.Right, value);
        else
            return node; // Duplicate values are not allowed

        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        return Balance(node);
    }

    public void Delete(int value)
    {
        root = Delete(root, value);
    }

    private Node Delete(Node node, int value)
    {
        if (node == null)
            return node;

        if (value < node.Value)
            node.Left = Delete(node.Left, value);
        else if (value > node.Value)
            node.Right = Delete(node.Right, value);
        else
        {
            if (node.Left == null || node.Right == null)
            {
                Node temp = node.Left ?? node.Right;
                if (temp == null)
                    return null;
                else
                    return temp;
            }

            Node tempNode = GetMinValueNode(node.Right);
            node.Value = tempNode.Value;
            node.Right = Delete(node.Right, tempNode.Value);
        }

        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        return Balance(node);
    }

    public bool Search(int value)
    {
        return Search(root, value);
    }

    private bool Search(Node node, int value)
    {
        if (node == null)
            return false;
        if (value < node.Value)
            return Search(node.Left, value);
        else if (value > node.Value)
            return Search(node.Right, value);
        else
            return true;
    }

    public List<int> InOrderTraversal()
    {
        var result = new List<int>();
        InOrderTraversal(root, result);
        return result;
    }

    private void InOrderTraversal(Node node, List<int> result)
    {
        if (node != null)
        {
            InOrderTraversal(node.Left, result);
            result.Add(node.Value);
            InOrderTraversal(node.Right, result);
        }
    }

    public List<int> PreOrderTraversal()
    {
        var result = new List<int>();
        PreOrderTraversal(root, result);
        return result;
    }

    private void PreOrderTraversal(Node node, List<int> result)
    {
        if (node != null)
        {
            result.Add(node.Value);
            PreOrderTraversal(node.Left, result);
            PreOrderTraversal(node.Right, result);
        }
    }

    public List<int> PostOrderTraversal()
    {
        var result = new List<int>();
        PostOrderTraversal(root, result);
        return result;
    }

    private void PostOrderTraversal(Node node, List<int> result)
    {
        if (node != null)
        {
            PostOrderTraversal(node.Left, result);
            PostOrderTraversal(node.Right, result);
            result.Add(node.Value);
        }
    }

    private Node Balance(Node node)
    {
        int balance = GetBalance(node);

        if (balance > 1)
        {
            if (GetBalance(node.Left) < 0)
                node.Left = RotateLeft(node.Left);
            return RotateRight(node);
        }

        if (balance < -1)
        {
            if (GetBalance(node.Right) > 0)
                node.Right = RotateRight(node.Right);
            return RotateLeft(node);
        }

        return node;
    }

    private int GetHeight(Node node)
    {
        return node?.Height ?? 0;
    }

    private int GetBalance(Node node)
    {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
    }

    private Node RotateRight(Node y)
    {
        Node x = y.Left;
        Node T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
        x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));

        return x;
    }

    private Node RotateLeft(Node x)
    {
        Node y = x.Right;
        Node T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
        y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));

        return y;
    }

    private Node GetMinValueNode(Node node)
    {
        Node current = node;
        while (current.Left != null)
            current = current.Left;
        return current;
    }
}