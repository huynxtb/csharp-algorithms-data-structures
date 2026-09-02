using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents a node in a Cartesian Tree.
/// </summary>
/// <typeparam name="T">The type of the value stored in the node, which must be comparable.</typeparam>
public class CartesianTreeNode<T> where T : IComparable<T>
{
    /// <summary>
    /// Gets the value stored in this node.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Gets or sets the left child of this node.
    /// </summary>
    /// <remarks>
    /// The setter is internal to allow the CartesianTree class to build the tree structure,
    /// but prevent external modification.
    /// </remarks>
    public CartesianTreeNode<T> Left { get; internal set; }

    /// <summary>
    /// Gets or sets the right child of this node.
    /// </summary>
    /// <remarks>
    /// The setter is internal to allow the CartesianTree class to build the tree structure,
    /// but prevent external modification.
    /// </remarks>
    public CartesianTreeNode<T> Right { get; internal set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CartesianTreeNode{T}"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value to store in the node.</param>
    public CartesianTreeNode(T value)
    {
        Value = value;
        Left = null;
        Right = null;
    }
}

/// <summary>
/// Represents a Cartesian Tree data structure.
/// A Cartesian Tree is a binary tree derived from a sequence of numbers,
/// satisfying both the min-heap property and the in-order traversal property
/// (in-order traversal yields the original sequence).
/// </summary>
/// <typeparam name="T">The type of the items in the tree, which must be comparable.</typeparam>
public class CartesianTree<T> where T : IComparable<T>
{
    /// <summary>
    /// Gets the root node of the Cartesian Tree.
    /// </summary>
    public CartesianTreeNode<T> Root { get; private set; }

    /// <summary>
    /// Prevents direct instantiation of the <see cref="CartesianTree{T}"/> class.
    /// Use the static <see cref="Build(IEnumerable{T})"/> method to create a tree.
    /// </summary>
    private CartesianTree() { }

    /// <summary>
    /// Builds a Cartesian Tree from an enumerable collection of items in O(N) linear time.
    /// The tree satisfies the min-heap property (parent value is less than children values)
    /// and an in-order traversal yields the original sequence of items.
    /// </summary>
    /// <param name="items">The collection of items to build the tree from.</param>
    /// <returns>A new <see cref="CartesianTree{T}"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the input collection is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the input collection contains null items.</exception>
    public static CartesianTree<T> Build(IEnumerable<T> items)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items), "Input collection cannot be null.");
        }

        var tree = new CartesianTree<T>();
        var stack = new Stack<CartesianTreeNode<T>>();

        foreach (var item in items)
        {
            if (item == null)
            {
                throw new ArgumentException("Input collection cannot contain null items.", nameof(items));
            }

            var newNode = new CartesianTreeNode<T>(item);
            CartesianTreeNode<T> lastPopped = null;

            // Pop nodes from the stack while their values are greater than newNode's value.
            // These popped nodes will form the right spine of a subtree that becomes newNode's left child.
            while (stack.Count > 0 && stack.Peek().Value.CompareTo(newNode.Value) > 0)
            {
                lastPopped = stack.Pop();
            }

            // If a node was popped, it (and its subtree) becomes the left child of newNode.
            if (lastPopped != null)
            {
                newNode.Left = lastPopped;
            }

            // If the stack is not empty, the top of the stack becomes the parent of newNode's right child.
            // This means newNode becomes the right child of the current stack top.
            if (stack.Count > 0)
            {
                stack.Peek().Right = newNode;
            }

            // Push newNode onto the stack. The stack maintains nodes in increasing order of value
            // from bottom to top, forming the rightmost path of the tree being constructed.
            stack.Push(newNode);
        }

        // After processing all elements, if the stack is not empty,
        // the bottom-most element in the stack is the root of the Cartesian Tree.
        if (stack.Count > 0)
        {
            // Stack.ToArray() returns elements in LIFO order. The last element in this array
            // corresponds to the bottom-most element in the stack.
            tree.Root = stack.ToArray().Last();
        }

        return tree;
    }

    /// <summary>
    /// Performs an in-order traversal of the Cartesian Tree.
    /// An in-order traversal of a Cartesian Tree yields the original sequence of items.
    /// </summary>
    /// <returns>An enumerable collection of items in in-order sequence.</returns>
    public IEnumerable<T> InOrderTraversal()
    {
        return InOrderTraversal(Root);
    }

    /// <summary>
    /// Recursively performs an in-order traversal starting from a given node.
    /// </summary>
    /// <param name="node">The current node to start traversal from.</param>
    /// <returns>An enumerable collection of items in in-order sequence.</returns>
    private IEnumerable<T> InOrderTraversal(CartesianTreeNode<T> node)
    {
        if (node == null)
        {
            yield break;
        }

        foreach (var item in InOrderTraversal(node.Left))
        {
            yield return item;
        }

        yield return node.Value;

        foreach (var item in InOrderTraversal(node.Right))
        {
            yield return item;
        }
    }

    /// <summary>
    /// Performs a pre-order traversal of the Cartesian Tree.
    /// </summary>
    /// <returns>An enumerable collection of items in pre-order sequence.</returns>
    public IEnumerable<T> PreOrderTraversal()
    {
        return PreOrderTraversal(Root);
    }

    /// <summary>
    /// Recursively performs a pre-order traversal starting from a given node.
    /// </summary>
    /// <param name="node">The current node to start traversal from.</param>
    /// <returns>An enumerable collection of items in pre-order sequence.</returns>
    private IEnumerable<T> PreOrderTraversal(CartesianTreeNode<T> node)
    {
        if (node == null)
        {
            yield break;
        }

        yield return node.Value;

        foreach (var item in PreOrderTraversal(node.Left))
        {
            yield return item;
        }

        foreach (var item in PreOrderTraversal(node.Right))
        {
            yield return item;
        }
    }
}