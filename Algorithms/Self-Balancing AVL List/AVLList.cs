using System;
using System.Collections.Generic;

public class AVLListNode<T> where T : IComparable<T>
{
    public T Data { get; set; }
    public AVLListNode<T> Next { get; set; }
    public AVLListNode<T> Left { get; set; }
    public AVLListNode<T> Right { get; set; }
    public int Height { get; set; }

    public AVLListNode(T data)
    {
        Data = data;
        Next = null;
        Left = null;
        Right = null;
        Height = 1;
    }
}

public class AVLList<T> where T : IComparable<T>
{
    private AVLListNode<T> root;
    private int count;

    public AVLList()
    {
        root = null;
        count = 0;
    }

    public int Count => count;

    public bool IsEmpty => count == 0;

    private int GetHeight(AVLListNode<T> node)
    {
        return node == null ? 0 : node.Height;
    }

    private int GetBalanceFactor(AVLListNode<T> node)
    {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
    }

    private void UpdateHeight(AVLListNode<T> node)
    {
        if (node != null)
        {
            node.Height = Math.Max(GetHeight(node.Left), GetHeight(node.Right)) + 1;
        }
    }

    private AVLListNode<T> RotateRight(AVLListNode<T> node)
    {
        AVLListNode<T> leftChild = node.Left;
        AVLListNode<T> leftRightChild = leftChild.Right;

        leftChild.Right = node;
        node.Left = leftRightChild;

        UpdateHeight(node);
        UpdateHeight(leftChild);

        return leftChild;
    }

    private AVLListNode<T> RotateLeft(AVLListNode<T> node)
    {
        AVLListNode<T> rightChild = node.Right;
        AVLListNode<T> rightLeftChild = rightChild.Left;

        rightChild.Left = node;
        node.Right = rightLeftChild;

        UpdateHeight(node);
        UpdateHeight(rightChild);

        return rightChild;
    }

    private AVLListNode<T> Balance(AVLListNode<T> node)
    {
        UpdateHeight(node);
        int balanceFactor = GetBalanceFactor(node);

        if (balanceFactor > 1)
        {
            if (GetBalanceFactor(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            return RotateRight(node);
        }

        if (balanceFactor < -1)
        {
            if (GetBalanceFactor(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
            }
            return RotateLeft(node);
        }

        return node;
    }

    public void Insert(T data)
    {
        root = InsertRecursive(root, data);
    }

    private AVLListNode<T> InsertRecursive(AVLListNode<T> node, T data)
    {
        if (node == null)
        {
            count++;
            return new AVLListNode<T>(data);
        }

        int comparison = data.CompareTo(node.Data);

        if (comparison < 0)
        {
            node.Left = InsertRecursive(node.Left, data);
        }
        else if (comparison > 0)
        {
            node.Right = InsertRecursive(node.Right, data);
        }
        else
        {
            return node;
        }

        return Balance(node);
    }

    public bool Delete(T data)
    {
        int initialCount = count;
        root = DeleteRecursive(root, data);
        return count < initialCount;
    }

    private AVLListNode<T> DeleteRecursive(AVLListNode<T> node, T data)
    {
        if (node == null)
        {
            return null;
        }

        int comparison = data.CompareTo(node.Data);

        if (comparison < 0)
        {
            node.Left = DeleteRecursive(node.Left, data);
        }
        else if (comparison > 0)
        {
            node.Right = DeleteRecursive(node.Right, data);
        }
        else
        {
            count--;

            if (node.Left == null)
            {
                return node.Right;
            }
            else if (node.Right == null)
            {
                return node.Left;
            }
            else
            {
                AVLListNode<T> minNode = FindMin(node.Right);
                node.Data = minNode.Data;
                node.Right = DeleteRecursive(node.Right, minNode.Data);
            }
        }

        return Balance(node);
    }

    private AVLListNode<T> FindMin(AVLListNode<T> node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    public bool Search(T data)
    {
        return SearchRecursive(root, data);
    }

    private bool SearchRecursive(AVLListNode<T> node, T data)
    {
        if (node == null)
        {
            return false;
        }

        int comparison = data.CompareTo(node.Data);

        if (comparison == 0)
        {
            return true;
        }
        else if (comparison < 0)
        {
            return SearchRecursive(node.Left, data);
        }
        else
        {
            return SearchRecursive(node.Right, data);
        }
    }

    public List<T> TraverseInOrder()
    {
        List<T> result = new List<T>();
        InOrderTraversal(root, result);
        return result;
    }

    private void InOrderTraversal(AVLListNode<T> node, List<T> result)
    {
        if (node == null)
        {
            return;
        }

        InOrderTraversal(node.Left, result);
        result.Add(node.Data);
        InOrderTraversal(node.Right, result);
    }

    public void Clear()
    {
        root = null;
        count = 0;
    }

    public int GetHeight()
    {
        return GetHeight(root);
    }

    public bool IsBalanced()
    {
        return IsBalancedRecursive(root);
    }

    private bool IsBalancedRecursive(AVLListNode<T> node)
    {
        if (node == null)
        {
            return true;
        }

        int balanceFactor = GetBalanceFactor(node);
        if (Math.Abs(balanceFactor) > 1)
        {
            return false;
        }

        return IsBalancedRecursive(node.Left) && IsBalancedRecursive(node.Right);
    }

    public T GetMin()
    {
        if (root == null)
        {
            throw new InvalidOperationException("List is empty");
        }
        AVLListNode<T> minNode = FindMin(root);
        return minNode.Data;
    }

    public T GetMax()
    {
        if (root == null)
        {
            throw new InvalidOperationException("List is empty");
        }
        return GetMaxRecursive(root);
    }

    private T GetMaxRecursive(AVLListNode<T> node)
    {
        while (node.Right != null)
        {
            node = node.Right;
        }
        return node.Data;
    }
}