using System;
using System.Collections.Generic;

namespace DataStructures.Trees
{
    /// <summary>
    /// A weight-balanced order statistic tree (BB[α] tree) that maintains balance using
    /// weight-based rebalancing and supports efficient rank and select queries.
    /// Allows duplicate values.
    /// </summary>
    /// <typeparam name="T">The type of elements in the tree (must implement IComparable).</typeparam>
    public class WeightBalancedOrderStatisticTree<T> where T : IComparable<T>
    {
        /// <summary>The root node of the tree.</summary>
        private Node _root;

        /// <summary>Balance parameter α ≈ 0.292 (1 - √2/2).</summary>
        private const double Alpha = 0.292;

        /// <summary>Secondary parameter β for rotation selection ≈ 0.35.</summary>
        private const double Beta = 0.35;

        /// <summary>
        /// Gets the number of elements in the tree.
        /// Time Complexity: O(1)
        /// </summary>
        public int Count => _root?.Size ?? 0;

        /// <summary>
        /// Inserts a value into the tree. Duplicates are allowed.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="value">The value to insert.</param>
        public void Insert(T value)
        {
            _root = Insert(_root, value);
        }

        /// <summary>
        /// Removes a value from the tree. Removes only one occurrence if duplicates exist.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="value">The value to remove.</param>
        /// <returns>True if the value was found and removed; false otherwise.</returns>
        public bool Remove(T value)
        {
            int originalCount = Count;
            _root = Remove(_root, value);
            return Count < originalCount;
        }

        /// <summary>
        /// Checks if a value exists in the tree.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>True if the value exists; false otherwise.</returns>
        public bool Contains(T value)
        {
            return Contains(_root, value);
        }

        /// <summary>
        /// Returns the k-th smallest element (0-indexed).
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="k">The rank (0-indexed).</param>
        /// <returns>The k-th smallest element.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if k is out of bounds.</exception>
        public T Select(int k)
        {
            if (k < 0 || k >= Count)
                throw new ArgumentOutOfRangeException(nameof(k), "Index out of range.");
            return Select(_root, k);
        }

        /// <summary>
        /// Returns the rank (0-indexed position) of where a value would be inserted.
        /// This is the count of elements strictly less than the value.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="value">The value to find the rank for.</param>
        /// <returns>The number of elements strictly less than the value.</returns>
        public int Rank(T value)
        {
            return Rank(_root, value);
        }

        /// <summary>
        /// Returns the minimum element in the tree.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <returns>The minimum element.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the tree is empty.</exception>
        public T Min()
        {
            if (_root == null)
                throw new InvalidOperationException("Tree is empty.");
            Node current = _root;
            while (current.Left != null)
                current = current.Left;
            return current.Value;
        }

        /// <summary>
        /// Returns the maximum element in the tree.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <returns>The maximum element.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the tree is empty.</exception>
        public T Max()
        {
            if (_root == null)
                throw new InvalidOperationException("Tree is empty.");
            Node current = _root;
            while (current.Right != null)
                current = current.Right;
            return current.Value;
        }

        /// <summary>
        /// Returns an in-order traversal of the tree elements using an iterative approach.
        /// Time Complexity: O(n)
        /// </summary>
        /// <returns>An enumerable of elements in sorted order.</returns>
        public IEnumerable<T> InOrderTraversal()
        {
            var stack = new Stack<Node>();
            Node current = _root;

            while (current != null || stack.Count > 0)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                current = stack.Pop();
                yield return current.Value;
                current = current.Right;
            }
        }

        /// <summary>
        /// Clears all elements from the tree.
        /// Time Complexity: O(1)
        /// </summary>
        public void Clear()
        {
            _root = null;
        }

        // ============ Private Helper Methods ============

        private Node Insert(Node node, T value)
        {
            if (node == null)
                return new Node(value);

            int cmp = value.CompareTo(node.Value);
            if (cmp <= 0)
                node.Left = Insert(node.Left, value);
            else
                node.Right = Insert(node.Right, value);

            UpdateSize(node);
            return Rebalance(node);
        }

        private Node Remove(Node node, T value)
        {
            if (node == null)
                return null;

            int cmp = value.CompareTo(node.Value);

            if (cmp < 0)
            {
                node.Left = Remove(node.Left, value);
            }
            else if (cmp > 0)
            {
                node.Right = Remove(node.Right, value);
            }
            else
            {
                // Node to remove found
                if (node.Left == null)
                    return node.Right;
                if (node.Right == null)
                    return node.Left;

                // Two children: replace with in-order successor
                Node successor = node.Right;
                while (successor.Left != null)
                    successor = successor.Left;

                node.Value = successor.Value;
                node.Right = Remove(node.Right, successor.Value);
            }

            if (node != null)
            {
                UpdateSize(node);
                node = Rebalance(node);
            }

            return node;
        }

        private bool Contains(Node node, T value)
        {
            if (node == null)
                return false;

            int cmp = value.CompareTo(node.Value);
            if (cmp == 0)
                return true;
            if (cmp < 0)
                return Contains(node.Left, value);
            return Contains(node.Right, value);
        }

        private T Select(Node node, int k)
        {
            int leftSize = node.Left?.Size ?? 0;

            if (k < leftSize)
                return Select(node.Left, k);
            if (k == leftSize)
                return node.Value;
            return Select(node.Right, k - leftSize - 1);
        }

        private int Rank(Node node, T value)
        {
            if (node == null)
                return 0;

            int cmp = value.CompareTo(node.Value);
            if (cmp <= 0)
                return Rank(node.Left, value);

            int leftSize = node.Left?.Size ?? 0;
            return leftSize + 1 + Rank(node.Right, value);
        }

        /// <summary>Updates the size of a node based on its children.</summary>
        private void UpdateSize(Node node)
        {
            if (node == null)
                return;
            node.Size = 1 + (node.Left?.Size ?? 0) + (node.Right?.Size ?? 0);
        }

        /// <summary>Rebalances the tree at the given node using weight-based criteria.</summary>
        private Node Rebalance(Node node)
        {
            if (node == null)
                return null;

            int weight = 1 + (node.Left?.Size ?? 0) + (node.Right?.Size ?? 0);
            int leftWeight = 1 + (node.Left?.Left?.Size ?? 0) + (node.Left?.Right?.Size ?? 0);
            int rightWeight = 1 + (node.Right?.Left?.Size ?? 0) + (node.Right?.Right?.Size ?? 0);

            // Check left imbalance
            if (node.Left != null && leftWeight < Alpha * weight)
            {
                // Left is too light; rotate right
                if (node.Left.Right != null)
                {
                    int llWeight = 1 + (node.Left.Left?.Size ?? 0);
                    int lrWeight = 1 + (node.Left.Right?.Size ?? 0);
                    if (lrWeight > Beta * leftWeight)
                        node = LeftRightRotate(node);
                    else
                        node = RightRotate(node);
                }
                else
                {
                    node = RightRotate(node);
                }
            }
            // Check right imbalance
            else if (node.Right != null && rightWeight < Alpha * weight)
            {
                // Right is too light; rotate left
                if (node.Right.Left != null)
                {
                    int rrWeight = 1 + (node.Right.Right?.Size ?? 0);
                    int rlWeight = 1 + (node.Right.Left?.Size ?? 0);
                    if (rlWeight > Beta * rightWeight)
                        node = RightLeftRotate(node);
                    else
                        node = LeftRotate(node);
                }
                else
                {
                    node = LeftRotate(node);
                }
            }

            return node;
        }

        /// <summary>Performs a left rotation.</summary>
        private Node LeftRotate(Node node)
        {
            Node right = node.Right;
            node.Right = right.Left;
            right.Left = node;
            UpdateSize(node);
            UpdateSize(right);
            return right;
        }

        /// <summary>Performs a right rotation.</summary>
        private Node RightRotate(Node node)
        {
            Node left = node.Left;
            node.Left = left.Right;
            left.Right = node;
            UpdateSize(node);
            UpdateSize(left);
            return left;
        }

        /// <summary>Performs a left-right rotation (left on left child, then right on node).</summary>
        private Node LeftRightRotate(Node node)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }

        /// <summary>Performs a right-left rotation (right on right child, then left on node).</summary>
        private Node RightLeftRotate(Node node)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        // ============ Node Class ============

        /// <summary>A node in the weight-balanced tree.</summary>
        private class Node
        {
            /// <summary>The value stored in this node.</summary>
            public T Value { get; set; }

            /// <summary>The left child.</summary>
            public Node Left { get; set; }

            /// <summary>The right child.</summary>
            public Node Right { get; set; }

            /// <summary>The size of the subtree rooted at this node (including itself).</summary>
            public int Size { get; set; }

            /// <summary>Initializes a new node with the given value.</summary>
            public Node(T value)
            {
                Value = value;
                Left = null;
                Right = null;
                Size = 1;
            }
        }
    }
}