using System;
using System.Collections.Generic;

public class SelfBalancingLinkWeightedBinaryTree
{
    private class Node
    {
        public int Value;
        public int Weight;
        public Node Left;
        public Node Right;
        public int Height;

        public Node(int value, int weight)
        {
            Value = value;
            Weight = weight;
            Height = 1;
        }
    }

    private Node root;

    public void Insert(int value, int weight)
    {
        root = Insert(root, value, weight);
    }

    private Node Insert(Node node, int value, int weight)
    {
        if (node == null)
            return new Node(value, weight);

        if (value < node.Value)
            node.Left = Insert(node.Left, value, weight);
        else if (value > node.Value)
            node.Right = Insert(node.Right, value, weight);
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
                node = temp;
            }
            else
            {
                Node temp = GetMinValueNode(node.Right);
                node.Value = temp.Value;
                node.Weight = temp.Weight;
                node.Right = Delete(node.Right, temp.Value);
            }
        }

        if (node == null)
            return node;

        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        return Balance(node);
    }

    public Node Search(int value)
    {
        return Search(root, value);
    }

    private Node Search(Node node, int value)
    {
        if (node == null || node.Value == value)
            return node;

        if (value < node.Value)
            return Search(node.Left, value);
        else
            return Search(node.Right, value);
    }

    public List<int> GetMaxWeightedPath()
    {
        List<int> path = new List<int>();
        GetMaxWeightedPath(root, path, 0, 0);
        return path;
    }

    private void GetMaxWeightedPath(Node node, List<int> path, int currentWeight, int maxWeight)
    {
        if (node == null)
            return;

        currentWeight += node.Weight;

        if (node.Left == null && node.Right == null) // Leaf node
        {
            if (currentWeight > maxWeight)
            {
                maxWeight = currentWeight;
                path.Clear();
                path.Add(node.Value);
            }
        }
        else
        {
            GetMaxWeightedPath(node.Left, path, currentWeight, maxWeight);
            GetMaxWeightedPath(node.Right, path, currentWeight, maxWeight);
        }
    }

    private Node GetMinValueNode(Node node)
    {
        Node current = node;
        while (current.Left != null)
            current = current.Left;
        return current;
    }

    private int GetHeight(Node node)
    {
        return node == null ? 0 : node.Height;
    }

    private int GetBalance(Node node)
    {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
    }

    private Node Balance(Node node)
    {
        int balance = GetBalance(node);

        // Left Left Case
        if (balance > 1 && GetBalance(node.Left) >= 0)
            return RightRotate(node);

        // Right Right Case
        if (balance < -1 && GetBalance(node.Right) <= 0)
            return LeftRotate(node);

        // Left Right Case
        if (balance > 1 && GetBalance(node.Left) < 0)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        // Right Left Case
        if (balance < -1 && GetBalance(node.Right) > 0)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }

    private Node RightRotate(Node y)
    {
        Node x = y.Left;
        Node T2 = x.Right;

        // Perform rotation
        x.Right = y;
        y.Left = T2;

        // Update heights
        y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));
        x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));

        // Return new root
        return x;
    }

    private Node LeftRotate(Node x)
    {
        Node y = x.Right;
        Node T2 = y.Left;

        // Perform rotation
        y.Left = x;
        x.Right = T2;

        // Update heights
        x.Height = 1 + Math.Max(GetHeight(x.Left), GetHeight(x.Right));
        y.Height = 1 + Math.Max(GetHeight(y.Left), GetHeight(y.Right));

        // Return new root
        return y;
    }
}