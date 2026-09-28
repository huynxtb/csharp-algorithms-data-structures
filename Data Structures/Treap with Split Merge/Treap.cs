using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.Trees
{
    /// <summary>
    /// Represents a randomized binary search tree (Treap) implemented using Split and Merge operations.
    /// </summary>
    /// <typeparam name="TKey">The type of elements in the treap, which must implement <see cref="IComparable{TKey}"/>.</typeparam>
    public class Treap<TKey> : IEnumerable<TKey> where TKey : IComparable<TKey>
    {
        private sealed class Node
        {
            public TKey Key { get; set; }
            public int Priority { get; set; }
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public int Size { get; set; }

            public Node(TKey key, int priority)
            {
                Key = key;
                Priority = priority;
                Size = 1;
            }
        }

        private readonly Random _random;
        private Node? _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="Treap{TKey}"/> class with a default random seed.
        /// </summary>
        public Treap()
        {
            _random = new Random();
            _root = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Treap{TKey}"/> class with a specified random seed.
        /// </summary>
        /// <param name="seed">The seed used for priority generation.</param>
        public Treap(int seed)
        {
            _random = new Random(seed);
            _root = null;
        }

        /// <summary>
        /// Gets the total number of elements contained in the treap.
        /// </summary>
        public int Count => GetSize(_root);

        /// <summary>
        /// Determines whether the treap contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the treap.</param>
        /// <returns><c>true</c> if the treap contains the specified key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
        public bool Contains(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Node? current = _root;
            while (current != null)
            {
                int cmp = key.CompareTo(current.Key);
                if (cmp == 0)
                {
                    return true;
                }
                current = cmp < 0 ? current.Left : current.Right;
            }
            return false;
        }

        /// <summary>
        /// Inserts a key into the treap. If the key already exists, no operation is performed.
        /// </summary>
        /// <param name="key">The key to insert.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
        public void Insert(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Contains(key))
            {
                return;
            }

            var (left, right) = Split(_root, key);
            var newNode = new Node(key, _random.Next());
            _root = Merge(Merge(left, newNode), right);
        }

        /// <summary>
        /// Removes a key from the treap.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns><c>true</c> if the key was found and removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!Contains(key))
            {
                return false;
            }

            var (left, right) = Split(_root, key);
            var (equal, greater) = SplitFirst(right);

            _root = Merge(left, greater);
            return true;
        }

        /// <summary>
        /// Retrieves the k-th smallest element in the treap (1-indexed).
        /// </summary>
        /// <param name="k">The 1-based index of the element to retrieve.</param>
        /// <returns>The k-th smallest element.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="k"/> is less than 1 or greater than <see cref="Count"/>.</exception>
        public TKey GetKthSmallest(int k)
        {
            if (k < 1 || k > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(k), "Index must be between 1 and Count.");
            }

            Node? current = _root;
            while (current != null)
            {
                int leftSize = GetSize(current.Left);
                if (k == leftSize + 1)
                {
                    return current.Key;
                }

                if (k <= leftSize)
                {
                    current = current.Left;
                }
                else
                {
                    k -= leftSize + 1;
                    current = current.Right;
                }
            }

            throw new InvalidOperationException("Invalid state reached while querying k-th smallest element.");
        }

        /// <summary>
        /// Returns the number of elements in the treap strictly less than the specified key.
        /// </summary>
        /// <param name="key">The key to calculate rank for.</param>
        /// <returns>The number of elements strictly smaller than <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
        public int RankOf(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            int rank = 0;
            Node? current = _root;
            while (current != null)
            {
                int cmp = key.CompareTo(current.Key);
                if (cmp <= 0)
                {
                    current = current.Left;
                }
                else
                {
                    rank += GetSize(current.Left) + 1;
                    current = current.Right;
                }
            }
            return rank;
        }

        /// <summary>
        /// Enumerates all keys in the treap in ascending order iteratively.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TKey}"/> yielding keys in sorted order.</returns>
        public IEnumerable<TKey> InOrderTraversal()
        {
            var stack = new Stack<Node>();
            Node? current = _root;

            while (stack.Count > 0 || current != null)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                current = stack.Pop();
                yield return current.Key;
                current = current.Right;
            }
        }

        /// <summary>
        /// Removes all elements from the treap.
        /// </summary>
        public void Clear()
        {
            _root = null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the treap in sorted order.
        /// </summary>
        /// <returns>An enumerator for the treap.</returns>
        public IEnumerator<TKey> GetEnumerator()
        {
            return InOrderTraversal().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static int GetSize(Node? node)
        {
            return node?.Size ?? 0;
        }

        private static void UpdateSize(Node? node)
        {
            if (node != null)
            {
                node.Size = 1 + GetSize(node.Left) + GetSize(node.Right);
            }
        }

        private Node? Merge(Node? left, Node? right)
        {
            if (left == null)
            {
                return right;
            }
            if (right == null)
            {
                return left;
            }

            if (left.Priority >= right.Priority)
            {
                left.Right = Merge(left.Right, right);
                UpdateSize(left);
                return left;
            }
            else
            {
                right.Left = Merge(left, right.Left);
                UpdateSize(right);
                return right;
            }
        }

        private (Node? left, Node? right) Split(Node? root, TKey key)
        {
            if (root == null)
            {
                return (null, null);
            }

            if (root.Key.CompareTo(key) < 0)
            {
                var (subLeft, subRight) = Split(root.Right, key);
                root.Right = subLeft;
                UpdateSize(root);
                return (root, subRight);
            }
            else
            {
                var (subLeft, subRight) = Split(root.Left, key);
                root.Left = subRight;
                UpdateSize(root);
                return (subLeft, root);
            }
        }

        private (Node? first, Node? rest) SplitFirst(Node? root)
        {
            if (root == null)
            {
                return (null, null);
            }

            if (root.Left == null)
            {
                Node? rest = root.Right;
                root.Right = null;
                UpdateSize(root);
                return (root, rest);
            }

            var (first, newLeft) = SplitFirst(root.Left);
            root.Left = newLeft;
            UpdateSize(root);
            return (first, root);
        }
    }
}