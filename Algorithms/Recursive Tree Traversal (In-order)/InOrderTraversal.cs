using System.Collections.Generic;

public class TreeNode<T>
{
    public T Value { get; set; }
    public TreeNode<T> Left { get; set; }
    public TreeNode<T> Right { get; set; }

    public TreeNode(T value, TreeNode<T> left = null, TreeNode<T> right = null)
    {
        Value = value;
        Left = left;
        Right = right;
    }
}

public static class BinaryTreeTraversal
{
    public static List<T> InOrderTraversal<T>(TreeNode<T> root)
    {
        List<T> result = new List<T>();
        InOrderHelper(root, result);
        return result;
    }

    private static void InOrderHelper<T>(TreeNode<T> node, List<T> result)
    {
        if (node == null)
        {
            return;
        }

        InOrderHelper(node.Left, result);
        result.Add(node.Value);
        InOrderHelper(node.Right, result);
    }
}