using System;

public class ExponentialTreeNode
{
    public int Value { get; set; }
    public ExponentialTreeNode Left { get; set; }
    public ExponentialTreeNode Right { get; set; }
    public int Height { get; set; }

    public ExponentialTreeNode(int value)
    {
        Value = value;
        Height = 1;
    }
}

public class SelfBalancingExponentialTree
{
    private ExponentialTreeNode root;

    public SelfBalancingExponentialTree()
    {
        root = null;
    }

    private int GetHeight(ExponentialTreeNode node)
    {
        return node == null ? 0 : node.Height;
    }

    private int GetBalanceFactor(ExponentialTreeNode node)
    {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
    }

    private void UpdateHeight(ExponentialTreeNode node)
    {
        if (node != null)
        {
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }
    }

    private ExponentialTreeNode RightRotate(ExponentialTreeNode y)
    {
        ExponentialTreeNode x = y.Left;
        ExponentialTreeNode T2 = x.Right;

        x.Right = y;
        y.Left = T2;

        UpdateHeight(y);
        UpdateHeight(x);

        return x;
    }

    private ExponentialTreeNode LeftRotate(ExponentialTreeNode x)
    {
        ExponentialTreeNode y = x.Right;
        ExponentialTreeNode T2 = y.Left;

        y.Left = x;
        x.Right = T2;

        UpdateHeight(x);
        UpdateHeight(y);

        return y;
    }

    private ExponentialTreeNode Balance(ExponentialTreeNode node)
    {
        UpdateHeight(node);
        int balanceFactor = GetBalanceFactor(node);

        // Left Heavy
        if (balanceFactor > 1)
        {
            // Left-Left Case
            if (GetBalanceFactor(node.Left) >= 0)
            {
                return RightRotate(node);
            }
            // Left-Right Case
            else
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }
        }

        // Right Heavy
        if (balanceFactor < -1)
        {
            // Right-Right Case
            if (GetBalanceFactor(node.Right) <= 0)
            {
                return LeftRotate(node);
            }
            // Right-Left Case
            else
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }
        }

        return node;
    }

    public void Insert(int value)
    {
        root = InsertRecursive(root, value);
    }

    private ExponentialTreeNode InsertRecursive(ExponentialTreeNode node, int value)
    {
        if (node == null)
        {
            return new ExponentialTreeNode(value);
        }

        if (value < node.Value)
        {
            node.Left = InsertRecursive(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = InsertRecursive(node.Right, value);
        }
        else
        {
            // Duplicate values are not allowed in this implementation
            return node;
        }

        return Balance(node);
    }

    public bool Search(int value)
    {
        return SearchRecursive(root, value);
    }

    private bool SearchRecursive(ExponentialTreeNode node, int value)
    {
        if (node == null)
        {
            return false;
        }

        if (value == node.Value)
        {
            return true;
        }
        else if (value < node.Value)
        {
            return SearchRecursive(node.Left, value);
        }
        else
        {
            return SearchRecursive(node.Right, value);
        }
    }

    public void Delete(int value)
    {
        root = DeleteRecursive(root, value);
    }

    private ExponentialTreeNode DeleteRecursive(ExponentialTreeNode node, int value)
    {
        if (node == null)
        {
            return node;
        }

        if (value < node.Value)
        {
            node.Left = DeleteRecursive(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = DeleteRecursive(node.Right, value);
        }
        else
        {
            // Node with only one child or no child
            if ((node.Left == null) || (node.Right == null))
            {
                ExponentialTreeNode temp = node.Left ?? node.Right;

                // No child case
                if (temp == null)
                {
                    node = null;
                }
                else // One child case
                {
                    node = temp; // Copy the contents of the non-empty child
                }
            }
            else
            {
                // Node with two children: Get the inorder successor (smallest in the right subtree)
                ExponentialTreeNode temp = GetMinValueNode(node.Right);

                // Copy the inorder successor's data to this node
                node.Value = temp.Value;

                // Delete the inorder successor
                node.Right = DeleteRecursive(node.Right, temp.Value);
            }
        }

        // If the tree had only one node then return
        if (node == null)
        {
            return node;
        }

        return Balance(node);
    }

    private ExponentialTreeNode GetMinValueNode(ExponentialTreeNode node)
    {
        ExponentialTreeNode current = node;

        // Loop down to find the leftmost leaf
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current;
    }
}