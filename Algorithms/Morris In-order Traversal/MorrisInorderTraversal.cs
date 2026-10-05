using System;
using System.Collections.Generic;

public class TreeNode
{
    public int Value { get; set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }

    public TreeNode(int value, TreeNode left = null, TreeNode right = null)
    {
        Value = value;
        Left = left;
        Right = right;
    }
}

public class MorrisInorderTraversal
{
    /// <summary>
    /// Performs an in-order traversal of a binary tree using Morris Traversal.
    /// Achieves O(N) time complexity and O(1) auxiliary space complexity (excluding output list).
    /// </summary>
    /// <param name="root">The root node of the binary tree.</param>
    /// <returns>A list of node values in in-order sequence.</returns>
    public List<int> MorrisInorder(TreeNode root)
    {
        var result = new List<int>();
        TreeNode current = root;

        while (current != null)
        {
            if (current.Left == null)
            {
                // Case 1: No left subtree exists. Visit the current node and move right.
                result.Add(current.Value);
                current = current.Right;
            }
            else
            {
                // Case 2: Left subtree exists. Find the in-order predecessor.
                // The predecessor is the rightmost node in the left subtree.
                TreeNode predecessor = current.Left;
                while (predecessor.Right != null && predecessor.Right != current)
                {
                    predecessor = predecessor.Right;
                }

                if (predecessor.Right == null)
                {
                    // Thread creation: Establish a temporary link back to current.
                    predecessor.Right = current;
                    current = current.Left;
                }
                else
                {
                    // Thread removal: Link already exists, indicating left subtree has been visited.
                    // Revert the tree structure, visit the current node, and move to the right child.
                    predecessor.Right = null;
                    result.Add(current.Value);
                    current = current.Right;
                }
            }
        }

        return result;
    }
}