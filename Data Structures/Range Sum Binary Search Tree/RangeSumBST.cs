using System;

public class RangeSumBST
{
    private class Node
    {
        public int Value;
        public int SubtreeSum;
        public Node Left;
        public Node Right;

        public Node(int value)
        {
            Value = value;
            SubtreeSum = value;
            Left = null;
            Right = null;
        }
    }

    private Node root;

    // Public methods

    /// <summary>
    /// Inserts the value into the BST.
    /// </summary>
    /// <param name="value">The integer value to insert.</param>
    public void Insert(int value)
    {
        root = Insert(root, value);
    }

    /// <summary>
    /// Deletes a value from the BST if it exists.
    /// </summary>
    /// <param name="value">The integer value to delete.</param>
    public void Delete(int value)
    {
        root = Delete(root, value);
    }

    /// <summary>
    /// Searches for a value in the BST.
    /// </summary>
    /// <param name="value">The integer value to search for.</param>
    /// <returns>True if found, otherwise false.</returns>
    public bool Search(int value)
    {
        return Search(root, value);
    }

    /// <summary>
    /// Returns the sum of all node values x such that low <= x <= high.
    /// </summary>
    /// <param name="low">Lower bound (inclusive).</param>
    /// <param name="high">Upper bound (inclusive).</param>
    /// <returns>The sum of node values within the specified range.</returns>
    public int RangeSum(int low, int high)
    {
        return RangeSum(root, low, high);
    }

    // Private helper methods

    private Node Insert(Node node, int value)
    {
        if (node == null) return new Node(value);

        if (value < node.Value)
            node.Left = Insert(node.Left, value);
        else if (value > node.Value)
            node.Right = Insert(node.Right, value);
        else
            return node; // Duplicate values not inserted

        UpdateSubtreeSum(node);
        return node;
    }

    private Node Delete(Node node, int value)
    {
        if (node == null) return null;

        if (value < node.Value)
        {
            node.Left = Delete(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = Delete(node.Right, value);
        }
        else
        {
            // Found the node to delete
            if (node.Left == null) return node.Right;
            if (node.Right == null) return node.Left;

            // Node with two children: Get the inorder successor (smallest in right subtree)
            Node successor = FindMin(node.Right);
            node.Value = successor.Value;
            node.Right = Delete(node.Right, successor.Value);
        }

        UpdateSubtreeSum(node);
        return node;
    }

    private bool Search(Node node, int value)
    {
        if (node == null) return false;
        if (value == node.Value) return true;
        if (value < node.Value)
            return Search(node.Left, value);
        else
            return Search(node.Right, value);
    }

    /// <summary>
    /// Return sum of all values x where low <= x <= high in subtree rooted at node.
    /// </summary>
    private int RangeSum(Node node, int low, int high)
    {
        if (node == null) return 0;

        if (node.Value < low)
        {
            // Current value too low, ignore left subtree
            return RangeSum(node.Right, low, high);
        }
        else if (node.Value > high)
        {
            // Current value too high, ignore right subtree
            return RangeSum(node.Left, low, high);
        }
        else
        {
            // Value within range, sum left subtree, node value, and right subtree
            int leftSum = RangeSum(node.Left, low, high);
            int rightSum = RangeSum(node.Right, low, high);
            return node.Value + leftSum + rightSum;
        }
    }

    private Node FindMin(Node node)
    {
        while (node.Left != null) node = node.Left;
        return node;
    }

    private void UpdateSubtreeSum(Node node)
    {
        int leftSum = node.Left?.SubtreeSum ?? 0;
        int rightSum = node.Right?.SubtreeSum ?? 0;
        node.SubtreeSum = leftSum + rightSum + node.Value;
    }
}