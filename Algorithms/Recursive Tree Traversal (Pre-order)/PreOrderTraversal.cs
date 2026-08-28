using System;

public class TreeNode<T>
{
    public T Value { get; set; }
    public TreeNode<T> Left { get; set; }
    public TreeNode<T> Right { get; set; }

    public TreeNode(T value)
    {
        Value = value;
        Left = null;
        Right = null;
    }
}

public static class PreOrderTraversal<T>
{
    public static void Traverse(TreeNode<T> root, Action<T> visitAction)
    {
        if (visitAction == null)
        {
            throw new ArgumentNullException(nameof(visitAction));
        }

        if (root == null)
        {
            return;
        }

        visitAction(root.Value);
        Traverse(root.Left, visitAction);
        Traverse(root.Right, visitAction);
    }
}