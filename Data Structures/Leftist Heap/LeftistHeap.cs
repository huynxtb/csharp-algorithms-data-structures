using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.LeftistHeap
{
    /// <summary>
    /// Represents a generic Min-Priority Queue implemented using a Leftist Heap (Leftist Tree).
    /// Supports efficient O(log n) merging of two heaps.
    /// </summary>
    /// <typeparam name="T">The type of elements in the heap.</typeparam>
    public class LeftistHeap<T> : IEnumerable<T>
    {
        /// <summary>
        /// Internal node class representing a node in the Leftist Heap.
        /// </summary>
        private sealed class Node
        {
            public T Value { get; set; }
            public Node? Left { get; set; }
            public Node? Right { get; set; }
            public int Npl { get; set; } // Null Path Length (s-value)

            public Node(T value)
            {
                Value = value;
                Left = null;
                Right = null;
                Npl = 0;
            }
        }

        private Node? _root;
        private int _count;
        private readonly IComparer<T> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeftistHeap{T}"/> class with default comparer.
        /// </summary>
        public LeftistHeap()
            : this(Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeftistHeap{T}"/> class with a custom comparer.
        /// </summary>
        /// <param name="comparer">The comparer used to order elements.</param>
        public LeftistHeap(IComparer<T>? comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;
            _root = null;
            _count = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LeftistHeap{T}"/> class populated with elements from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new heap.</param>
        /// <param name="comparer">Optional comparer for element ordering.</param>
        public LeftistHeap(IEnumerable<T> collection, IComparer<T>? comparer = null)
            : this(comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (T item in collection)
            {
                Insert(item);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the heap.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Gets a value indicating whether the heap is empty.
        /// </summary>
        public bool IsEmpty => _count == 0;

        /// <summary>
        /// Gets the comparer used to order the elements in the heap.
        /// </summary>
        public IComparer<T> Comparer => _comparer;

        /// <summary>
        /// Inserts an item into the heap in O(log n) time.
        /// </summary>
        /// <param name="item">The item to insert.</param>
        public void Insert(T item)
        {
            Node newNode = new Node(item);
            _root = MergeNodes(_root, newNode);
            _count++;
        }

        /// <summary>
        /// Returns the minimum element without removing it from the heap in O(1) time.
        /// </summary>
        /// <returns>The minimum element in the heap.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the heap is empty.</exception>
        public T Peek()
        {
            if (_root == null)
            {
                throw new InvalidOperationException("The heap is empty.");
            }

            return _root.Value;
        }

        /// <summary>
        /// Removes and returns the minimum element from the heap in O(log n) time.
        /// </summary>
        /// <returns>The minimum element removed from the heap.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the heap is empty.</exception>
        public T ExtractMin()
        {
            if (_root == null)
            {
                throw new InvalidOperationException("The heap is empty.");
            }

            T minItem = _root.Value;
            _root = MergeNodes(_root.Left, _root.Right);
            _count--;
            return minItem;
        }

        /// <summary>
        /// Removes and returns the minimum element from the heap in O(log n) time. Alias for ExtractMin.
        /// </summary>
        /// <returns>The minimum element removed from the heap.</returns>
        public T Pop() => ExtractMin();

        /// <summary>
        /// Merges another <see cref="LeftistHeap{T}"/> into this instance in O(log n) time.
        /// The other heap is cleared and emptied in the process.
        /// </summary>
        /// <param name="other">The leftist heap to merge into this instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when other is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when attempting to merge a heap with itself.</exception>
        public void Merge(LeftistHeap<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (ReferenceEquals(this, other))
            {
                throw new InvalidOperationException("Cannot merge a heap with itself.");
            }

            _root = MergeNodes(_root, other._root);
            _count += other._count;

            // Clear the other heap to prevent shared mutable state
            other._root = null;
            other._count = 0;
        }

        /// <summary>
        /// Creates a new <see cref="LeftistHeap{T}"/> by merging two leftist heaps.
        /// The input heaps are cleared in the process.
        /// </summary>
        /// <param name="h1">The first leftist heap.</param>
        /// <param name="h2">The second leftist heap.</param>
        /// <returns>A new <see cref="LeftistHeap{T}"/> containing all elements of both heaps.</returns>
        /// <exception cref="ArgumentNullException">Thrown when h1 or h2 is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when h1 and h2 are the same instance.</exception>
        public static LeftistHeap<T> Merge(LeftistHeap<T> h1, LeftistHeap<T> h2)
        {
            if (h1 == null)
            {
                throw new ArgumentNullException(nameof(h1));
            }

            if (h2 == null)
            {
                throw new ArgumentNullException(nameof(h2));
            }

            if (ReferenceEquals(h1, h2))
            {
                throw new InvalidOperationException("Cannot merge a heap with itself.");
            }

            LeftistHeap<T> result = new LeftistHeap<T>(h1._comparer);
            result._root = result.MergeNodes(h1._root, h2._root);
            result._count = h1._count + h2._count;

            h1._root = null;
            h1._count = 0;
            h2._root = null;
            h2._count = 0;

            return result;
        }

        /// <summary>
        /// Removes all elements from the heap.
        /// </summary>
        public void Clear()
        {
            _root = null;
            _count = 0;
        }

        /// <summary>
        /// Recursive helper to merge two leftist trees maintaining min-heap and leftist invariants.
        /// </summary>
        private Node? MergeNodes(Node? h1, Node? h2)
        {
            if (h1 == null)
            {
                return h2;
            }

            if (h2 == null)
            {
                return h1;
            }

            // Enforce Min-Heap property: h1 must contain smaller root value
            if (_comparer.Compare(h1.Value, h2.Value) > 0)
            {
                Node temp = h1;
                h1 = h2;
                h2 = temp;
            }

            // Recursively merge the right child of h1 with h2
            h1.Right = MergeNodes(h1.Right, h2);

            // Enforce Leftist property: left child NPL >= right child NPL
            int leftNpl = h1.Left?.Npl ?? -1;
            int rightNpl = h1.Right?.Npl ?? -1;

            if (leftNpl < rightNpl)
            {
                Node? swap = h1.Left;
                h1.Left = h1.Right;
                h1.Right = swap;
            }

            // Update NPL of the current root
            h1.Npl = (h1.Right?.Npl ?? -1) + 1;

            return h1;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the heap in level-order (breadth-first traversal).
        /// </summary>
        /// <returns>An enumerator for the heap.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (_root == null)
            {
                yield break;
            }

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(_root);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                yield return current.Value;

                if (current.Left != null)
                {
                    queue.Enqueue(current.Left);
                }

                if (current.Right != null)
                {
                    queue.Enqueue(current.Right);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}