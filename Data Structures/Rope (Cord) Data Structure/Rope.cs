using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Rope (Cord) data structure for efficient manipulation of large mutable strings.
/// Uses a binary tree where leaf nodes store string fragments and internal nodes store weights.
/// Weight = total length of all characters in the left subtree.
/// </summary>
public class Rope
{
    /// <summary>
    /// Maximum length for leaf node strings. Smaller values increase tree depth but reduce leaf size.
    /// </summary>
    private const int LeafMaxLength = 10;

    /// <summary>
    /// Represents a node in the Rope tree.
    /// </summary>
    private abstract class Node
    {
        public abstract int Length { get; }
        public abstract char Index(int i);
        public abstract Node Clone();
    }

    /// <summary>
    /// Leaf node containing an actual string fragment.
    /// </summary>
    private class LeafNode : Node
    {
        public string Value { get; private set; }

        public override int Length => Value.Length;

        public LeafNode(string value)
        {
            Value = value ?? string.Empty;
        }

        public override char Index(int i)
        {
            if (i < 0 || i >= Value.Length)
                throw new ArgumentOutOfRangeException(nameof(i));
            return Value[i];
        }

        public override Node Clone()
        {
            return new LeafNode(Value);
        }
    }

    /// <summary>
    /// Internal node with left and right children.
    /// Weight stores the total length of characters in the left subtree.
    /// </summary>
    private class InternalNode : Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Weight { get; set; }

        public override int Length => Weight + (Right?.Length ?? 0);

        public InternalNode(Node left, Node right)
        {
            Left = left;
            Right = right;
            Weight = left?.Length ?? 0;
        }

        public override char Index(int i)
        {
            if (i < Weight)
            {
                if (Left == null)
                    throw new ArgumentOutOfRangeException(nameof(i));
                return Left.Index(i);
            }
            else
            {
                if (Right == null)
                    throw new ArgumentOutOfRangeException(nameof(i));
                return Right.Index(i - Weight);
            }
        }

        public override Node Clone()
        {
            return new InternalNode(Left?.Clone(), Right?.Clone())
            {
                Weight = Weight
            };
        }
    }

    private Node _root;

    /// <summary>
    /// Creates an empty Rope.
    /// </summary>
    public Rope()
    {
        _root = null;
    }

    /// <summary>
    /// Creates a Rope from the given string, building a balanced tree.
    /// </summary>
    public Rope(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            _root = null;
        }
        else
        {
            var leaves = new List<Node>();
            for (int i = 0; i < s.Length; i += LeafMaxLength)
            {
                int len = Math.Min(LeafMaxLength, s.Length - i);
                leaves.Add(new LeafNode(s.Substring(i, len)));
            }
            _root = BuildBalancedTree(leaves, 0, leaves.Count - 1);
        }
    }

    /// <summary>
    /// Recursively builds a balanced binary tree from a list of leaf nodes.
    /// </summary>
    private static Node BuildBalancedTree(List<Node> leaves, int start, int end)
    {
        if (start > end)
            return null;
        if (start == end)
            return leaves[start];

        int mid = start + (end - start) / 2;
        Node left = BuildBalancedTree(leaves, start, mid);
        Node right = BuildBalancedTree(leaves, mid + 1, end);
        return new InternalNode(left, right);
    }

    /// <summary>
    /// Gets the character at index i. O(log n) complexity.
    /// </summary>
    public char Index(int i)
    {
        if (_root == null || i < 0 || i >= Length)
            throw new ArgumentOutOfRangeException(nameof(i));
        return _root.Index(i);
    }

    /// <summary>
    /// Gets the total length of the Rope.
    /// </summary>
    public int Length => _root?.Length ?? 0;

    /// <summary>
    /// Concatenates this Rope with another and returns a new Rope. O(1) amortized.
    /// </summary>
    public Rope Concat(Rope other)
    {
        if (other == null || other.Length == 0)
            return new Rope { _root = _root?.Clone() };
        if (Length == 0)
            return new Rope { _root = other._root?.Clone() };

        var result = new Rope
        {
            _root = new InternalNode(_root, other._root)
        };
        return result;
    }

    /// <summary>
    /// Splits the Rope at index i into two Ropes. O(log n) complexity.
    /// Returns (left, right) where left contains [0, i) and right contains [i, length).
    /// </summary>
    public (Rope left, Rope right) Split(int i)
    {
        if (i < 0 || i > Length)
            throw new ArgumentOutOfRangeException(nameof(i));

        if (i == 0)
            return (new Rope(), new Rope { _root = _root?.Clone() });
        if (i == Length)
            return (new Rope { _root = _root?.Clone() }, new Rope());

        var (leftNode, rightNode) = SplitNode(_root, i);
        return (new Rope { _root = leftNode }, new Rope { _root = rightNode });
    }

    /// <summary>
    /// Recursively splits a node at position i.
    /// Returns (left subtree, right subtree).
    /// </summary>
    private static (Node, Node) SplitNode(Node node, int i)
    {
        if (node == null)
            return (null, null);

        if (node is LeafNode leaf)
        {
            if (i <= 0)
                return (null, leaf);
            if (i >= leaf.Length)
                return (leaf, null);
            var leftStr = leaf.Value.Substring(0, i);
            var rightStr = leaf.Value.Substring(i);
            return (new LeafNode(leftStr), new LeafNode(rightStr));
        }

        var internalNode = (InternalNode)node;
        if (i <= internalNode.Weight)
        {
            var (leftSubLeft, leftSubRight) = SplitNode(internalNode.Left, i);
            return (leftSubLeft, new InternalNode(leftSubRight, internalNode.Right));
        }
        else
        {
            var (rightSubLeft, rightSubRight) = SplitNode(internalNode.Right, i - internalNode.Weight);
            return (new InternalNode(internalNode.Left, rightSubLeft), rightSubRight);
        }
    }

    /// <summary>
    /// Inserts a string at index i. O(log n) complexity.
    /// </summary>
    public Rope Insert(int i, string s)
    {
        if (i < 0 || i > Length)
            throw new ArgumentOutOfRangeException(nameof(i));
        if (string.IsNullOrEmpty(s))
            return new Rope { _root = _root?.Clone() };

        var (left, right) = Split(i);
        var inserted = new Rope(s);
        return left.Concat(inserted).Concat(right);
    }

    /// <summary>
    /// Deletes length characters starting at index start. O(log n) complexity.
    /// </summary>
    public Rope Delete(int start, int length)
    {
        if (start < 0 || start > Length)
            throw new ArgumentOutOfRangeException(nameof(start));
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));
        if (length == 0)
            return new Rope { _root = _root?.Clone() };

        int end = Math.Min(start + length, Length);
        var (left, _) = Split(end);
        var (_, right) = left.Split(start);
        return left.Concat(new Rope { _root = right._root });
    }

    /// <summary>
    /// Extracts a substring from start with the given length. O(log n + m) where m is substring length.
    /// </summary>
    public string Substring(int start, int length)
    {
        if (start < 0 || start > Length)
            throw new ArgumentOutOfRangeException(nameof(start));
        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        int end = Math.Min(start + length, Length);
        var (_, right) = Split(end);
        var (result, _) = right._root != null ? (new Rope { _root = right._root }, null) : (new Rope(), null);

        var (left, _2) = Split(start);
        var (_, sub) = left.Split(start);
        return sub.ToString();
    }

    /// <summary>
    /// Rebalances the Rope tree to prevent degeneration. O(n) complexity.
    /// </summary>
    public Rope Rebalance()
    {
        if (_root == null)
            return new Rope();

        var leaves = new List<Node>();
        CollectLeaves(_root, leaves);

        if (leaves.Count == 0)
            return new Rope();

        var balancedRoot = BuildBalancedTree(leaves, 0, leaves.Count - 1);
        return new Rope { _root = balancedRoot };
    }

    /// <summary>
    /// Collects all leaf nodes in in-order traversal.
    /// </summary>
    private static void CollectLeaves(Node node, List<Node> leaves)
    {
        if (node == null)
            return;

        if (node is LeafNode leaf)
        {
            leaves.Add(leaf.Clone());
        }
        else if (node is InternalNode internal)
        {
            CollectLeaves(internal.Left, leaves);
            CollectLeaves(internal.Right, leaves);
        }
    }

    /// <summary>
    /// Reconstructs and returns the full string represented by the Rope. O(n) complexity.
    /// </summary>
    public override string ToString()
    {
        if (_root == null)
            return string.Empty;

        var sb = new System.Text.StringBuilder();
        BuildString(_root, sb);
        return sb.ToString();
    }

    /// <summary>
    /// Recursively builds the string from the tree.
    /// </summary>
    private static void BuildString(Node node, System.Text.StringBuilder sb)
    {
        if (node == null)
            return;

        if (node is LeafNode leaf)
        {
            sb.Append(leaf.Value);
        }
        else if (node is InternalNode internal)
        {
            BuildString(internal.Left, sb);
            BuildString(internal.Right, sb);
        }
    }
}