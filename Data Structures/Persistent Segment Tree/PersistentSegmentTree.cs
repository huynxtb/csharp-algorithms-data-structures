using System;
using System.Collections.Generic;

namespace DataStructures
{
    /// <summary>
    /// Represents a Fully Persistent Segment Tree supporting point updates and range sum queries across historical versions.
    /// </summary>
    public class PersistentSegmentTree
    {
        /// <summary>
        /// Internal node class representing a segment in the tree.
        /// </summary>
        internal class Node
        {
            public long Sum { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public Node(long sum, Node left = null, Node right = null)
            {
                Sum = sum;
                Left = left;
                Right = right;
            }
        }

        private readonly List<Node> _roots;
        private readonly int _size;

        /// <summary>
        /// Gets the total number of versions currently stored in the persistent segment tree.
        /// </summary>
        public int VersionCount => _roots.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistentSegmentTree"/> class and builds version 0.
        /// </summary>
        /// <param name="initialArray">The initial array of integers.</param>
        public PersistentSegmentTree(int[] initialArray)
        {
            if (initialArray == null)
            {
                throw new ArgumentNullException(nameof(initialArray));
            }

            _size = initialArray.Length;
            _roots = new List<Node>();

            if (_size > 0)
            {
                Node root0 = Build(initialArray, 0, _size - 1);
                _roots.Add(root0);
            }
            else
            {
                _roots.Add(null);
            }
        }

        /// <summary>
        /// Recursively builds the initial segment tree (Version 0).
        /// </summary>
        private Node Build(int[] arr, int lo, int hi)
        {
            if (lo == hi)
            {
                return new Node(arr[lo]);
            }

            int mid = lo + (hi - lo) / 2;
            Node leftChild = Build(arr, lo, mid);
            Node rightChild = Build(arr, mid + 1, hi);

            return new Node(leftChild.Sum + rightChild.Sum, leftChild, rightChild);
        }

        /// <summary>
        /// Creates a new version of the segment tree by updating the element at <paramref name="index"/> to <paramref name="newValue"/> based on <paramref name="version"/>.
        /// </summary>
        /// <param name="version">The version index to branch off from.</param>
        /// <param name="index">The 0-based index of the element to update.</param>
        /// <param name="newValue">The new value to assign.</param>
        /// <returns>The index of the newly created version.</returns>
        public int Update(int version, int index, int newValue)
        {
            if (version < 0 || version >= _roots.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(version), "Version index is out of range.");
            }

            if (index < 0 || index >= _size)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Array index is out of range.");
            }

            Node newRoot = Update(_roots[version], 0, _size - 1, index, newValue);
            _roots.Add(newRoot);
            return _roots.Count - 1;
        }

        /// <summary>
        /// Recursively creates new nodes along the path to the updated index while structurally sharing unchanged subtrees.
        /// </summary>
        private Node Update(Node node, int lo, int hi, int index, int newValue)
        {
            if (lo == hi)
            {
                return new Node(newValue);
            }

            int mid = lo + (hi - lo) / 2;
            if (index <= mid)
            {
                Node newLeft = Update(node?.Left, lo, mid, index, newValue);
                long rightSum = node?.Right?.Sum ?? 0;
                return new Node(newLeft.Sum + rightSum, newLeft, node?.Right);
            }
            else
            {
                Node newRight = Update(node?.Right, mid + 1, hi, index, newValue);
                long leftSum = node?.Left?.Sum ?? 0;
                return new Node(leftSum + newRight.Sum, node?.Left, newRight);
            }
        }

        /// <summary>
        /// Queries the sum of elements in the inclusive range [<paramref name="left"/>, <paramref name="right"/>] at a specific <paramref name="version"/>.
        /// </summary>
        /// <param name="version">The version to query.</param>
        /// <param name="left">The 0-based start index of the range (inclusive).</param>
        /// <param name="right">The 0-based end index of the range (inclusive).</param>
        /// <returns>The sum of elements in the range, or 0 if the range is invalid.</returns>
        public long Query(int version, int left, int right)
        {
            if (version < 0 || version >= _roots.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(version), "Version index is out of range.");
            }

            if (left > right || _size == 0)
            {
                return 0;
            }

            // Clamp query boundaries to valid array range
            int clampedLeft = Math.Max(0, left);
            int clampedRight = Math.Min(_size - 1, right);

            if (clampedLeft > clampedRight)
            {
                return 0;
            }

            return Query(_roots[version], 0, _size - 1, clampedLeft, clampedRight);
        }

        /// <summary>
        /// Recursively computes the range sum for the specified interval.
        /// </summary>
        private long Query(Node node, int lo, int hi, int left, int right)
        {
            if (node == null || left > hi || right < lo)
            {
                return 0;
            }

            if (left <= lo && hi <= right)
            {
                return node.Sum;
            }

            int mid = lo + (hi - lo) / 2;
            long leftSum = Query(node.Left, lo, mid, left, right);
            long rightSum = Query(node.Right, mid + 1, hi, left, right);

            return leftSum + rightSum;
        }
    }
}