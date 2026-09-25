using System;
using System.Collections;
using System.Collections.Generic;

namespace AdvancedDataStructures
{
    /// <summary>
    /// Represents a generic 2-3-4 (2-4) balanced search tree mapping keys to values.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the tree.</typeparam>
    /// <typeparam name="TValue">The type of values in the tree.</typeparam>
    public class TwoThreeFourTree<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private Node? _root;
        private int _count;
        private readonly IComparer<TKey> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoThreeFourTree{TKey, TValue}"/> class.
        /// </summary>
        public TwoThreeFourTree() : this(Comparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoThreeFourTree{TKey, TValue}"/> class using a specified comparer.
        /// </summary>
        /// <param name="comparer">The key comparer to use.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="comparer"/> is null.</exception>
        public TwoThreeFourTree(IComparer<TKey>? comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            _root = null;
            _count = 0;
        }

        /// <summary>
        /// Gets the number of key-value pairs stored in the tree.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Gets the height of the tree. An empty tree has height 0.
        /// </summary>
        public int Height
        {
            get
            {
                if (_root == null) return 0;
                int height = 0;
                Node? current = _root;
                while (current != null)
                {
                    height++;
                    current = current.IsLeaf ? null : current.Children[0];
                }
                return height;
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when key is not found in get accessor.</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (key == null) throw new ArgumentNullException(nameof(key));
                if (Search(key, out TValue value))
                {
                    return value;
                }
                throw new KeyNotFoundException($"The key '{key}' was not found in the tree.");
            }
            set
            {
                if (key == null) throw new ArgumentNullException(nameof(key));
                InsertOrUpdate(key, value, out _);
            }
        }

        /// <summary>
        /// Inserts a key-value pair into the tree. Throws if the key already exists.
        /// </summary>
        /// <param name="key">The key to insert.</param>
        /// <param name="value">The associated value.</param>
        /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
        /// <exception cref="ArgumentException">Thrown when an element with the same key already exists.</exception>
        public void Insert(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!InsertOrUpdate(key, value, out bool added) || !added)
            {
                throw new ArgumentException($"An item with key '{key}' already exists.", nameof(key));
            }
        }

        /// <summary>
        /// Inserts a key-value pair or updates the existing value.
        /// </summary>
        /// <param name="key">The key to insert or update.</param>
        /// <param name="value">The value to associate.</param>
        /// <param name="added">True if a new key was inserted; false if updated.</param>
        /// <returns>True if the operation succeeded.</returns>
        public bool InsertOrUpdate(TKey key, TValue value, out bool added)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (_root == null)
            {
                _root = new Node();
                _root.InsertKey(0, key, value);
                _count++;
                added = true;
                return true;
            }

            if (_root.KeyCount == 3)
            {
                Node newRoot = new Node();
                newRoot.Children[0] = _root;
                SplitChild(newRoot, 0);
                _root = newRoot;
            }

            Node current = _root;
            while (true)
            {
                for (int i = 0; i < current.KeyCount; i++)
                {
                    int cmp = _comparer.Compare(key, current.Keys[i]);
                    if (cmp == 0)
                    {
                        current.Values[i] = value;
                        added = false;
                        return true;
                    }
                }

                if (current.IsLeaf)
                {
                    current.InsertKeyOrdered(key, value, _comparer);
                    _count++;
                    added = true;
                    return true;
                }

                int childIndex = 0;
                while (childIndex < current.KeyCount && _comparer.Compare(key, current.Keys[childIndex]) > 0)
                {
                    childIndex++;
                }

                Node? child = current.Children[childIndex];
                if (child != null && child.KeyCount == 3)
                {
                    SplitChild(current, childIndex);
                    int cmpPromoted = _comparer.Compare(key, current.Keys[childIndex]);
                    if (cmpPromoted == 0)
                    {
                        current.Values[childIndex] = value;
                        added = false;
                        return true;
                    }
                    if (cmpPromoted > 0)
                    {
                        childIndex++;
                    }
                }

                current = current.Children[childIndex]!;
            }
        }

        /// <summary>
        /// Determines whether the tree contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>True if the key is found; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return Search(key, out _);
        }

        /// <summary>
        /// Searches for a key in the tree and retrieves its associated value.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <param name="value">When this method returns, contains the associated value if found, or default.</param>
        /// <returns>True if the key was found; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
        public bool Search(TKey key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Node? current = _root;
            while (current != null)
            {
                int i = 0;
                while (i < current.KeyCount && _comparer.Compare(key, current.Keys[i]) > 0)
                {
                    i++;
                }

                if (i < current.KeyCount && _comparer.Compare(key, current.Keys[i]) == 0)
                {
                    value = current.Values[i]!;
                    return true;
                }

                if (current.IsLeaf)
                {
                    break;
                }

                current = current.Children[i];
            }

            value = default!;
            return false;
        }

        /// <summary>
        /// Deletes the specified key from the tree.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if the key was found and removed; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when key is null.</exception>
        public bool Delete(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (_root == null)
            {
                return false;
            }

            bool removed = DeleteTopDown(_root, null, -1, key);
            if (removed)
            {
                _count--;
                if (_root != null && _root.KeyCount == 0)
                {
                    _root = _root.IsLeaf ? null : _root.Children[0];
                }
            }

            return removed;
        }

        /// <summary>
        /// Removes all elements from the tree.
        /// </summary>
        public void Clear()
        {
            _root = null;
            _count = 0;
        }

        /// <summary>
        /// Tries to get the minimum key-value pair in the tree.
        /// </summary>
        /// <param name="min">The smallest key-value pair, if found.</param>
        /// <returns>True if the tree is non-empty; otherwise, false.</returns>
        public bool TryGetMin(out KeyValuePair<TKey, TValue> min)
        {
            if (_root == null)
            {
                min = default;
                return false;
            }

            Node current = _root;
            while (!current.IsLeaf)
            {
                current = current.Children[0]!;
            }

            min = new KeyValuePair<TKey, TValue>(current.Keys[0], current.Values[0]!);
            return true;
        }

        /// <summary>
        /// Tries to get the maximum key-value pair in the tree.
        /// </summary>
        /// <param name="max">The largest key-value pair, if found.</param>
        /// <returns>True if the tree is non-empty; otherwise, false.</returns>
        public bool TryGetMax(out KeyValuePair<TKey, TValue> max)
        {
            if (_root == null)
            {
                max = default;
                return false;
            }

            Node current = _root;
            while (!current.IsLeaf)
            {
                current = current.Children[current.KeyCount]!;
            }

            int lastIdx = current.KeyCount - 1;
            max = new KeyValuePair<TKey, TValue>(current.Keys[lastIdx], current.Values[lastIdx]!);
            return true;
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (_root == null) yield break;

            Stack<TraversalFrame> stack = new Stack<TraversalFrame>();
            stack.Push(new TraversalFrame(_root));

            while (stack.Count > 0)
            {
                TraversalFrame frame = stack.Peek();
                Node node = frame.Node;

                if (frame.CurrentIndex <= node.KeyCount)
                {
                    if (!node.IsLeaf && !frame.ChildVisited)
                    {
                        frame.ChildVisited = true;
                        if (node.Children[frame.CurrentIndex] != null)
                        {
                            stack.Push(new TraversalFrame(node.Children[frame.CurrentIndex]!));
                            continue;
                        }
                    }

                    if (frame.CurrentIndex < node.KeyCount)
                    {
                        yield return new KeyValuePair<TKey, TValue>(node.Keys[frame.CurrentIndex], node.Values[frame.CurrentIndex]!);
                        frame.CurrentIndex++;
                        frame.ChildVisited = false;
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void SplitChild(Node parent, int childIndex)
        {
            Node fullChild = parent.Children[childIndex]!;
            Node sibling = new Node();

            TKey promotedKey = fullChild.Keys[1];
            TValue promotedVal = fullChild.Values[1]!;

            sibling.Keys[0] = fullChild.Keys[2];
            sibling.Values[0] = fullChild.Values[2];
            sibling.KeyCount = 1;

            if (!fullChild.IsLeaf)
            {
                sibling.Children[0] = fullChild.Children[2];
                sibling.Children[1] = fullChild.Children[3];
                fullChild.Children[2] = null;
                fullChild.Children[3] = null;
            }

            fullChild.Keys[1] = default!;
            fullChild.Values[1] = default!;
            fullChild.Keys[2] = default!;
            fullChild.Values[2] = default!;
            fullChild.KeyCount = 1;

            parent.InsertKey(childIndex, promotedKey, promotedVal);
            parent.InsertChild(childIndex + 1, sibling);
        }

        private bool DeleteTopDown(Node current, Node? parent, int childIndexInParent, TKey key)
        {
            if (current != _root && current.KeyCount == 1 && parent != null)
            {
                current = EnsureMinKeys(parent, childIndexInParent);
            }

            int keyIndex = -1;
            for (int i = 0; i < current.KeyCount; i++)
            {
                if (_comparer.Compare(key, current.Keys[i]) == 0)
                {
                    keyIndex = i;
                    break;
                }
            }

            if (keyIndex != -1)
            {
                if (current.IsLeaf)
                {
                    current.RemoveKey(keyIndex);
                    return true;
                }

                Node leftChild = current.Children[keyIndex]!;
                Node rightChild = current.Children[keyIndex + 1]!;

                if (leftChild.KeyCount > 1)
                {
                    KeyValuePair<TKey, TValue> pred = DeletePredecessor(leftChild, current, keyIndex);
                    current.Keys[keyIndex] = pred.Key;
                    current.Values[keyIndex] = pred.Value;
                    return true;
                }
                else if (rightChild.KeyCount > 1)
                {
                    KeyValuePair<TKey, TValue> succ = DeleteSuccessor(rightChild, current, keyIndex + 1);
                    current.Keys[keyIndex] = succ.Key;
                    current.Values[keyIndex] = succ.Value;
                    return true;
                }
                else
                {
                    Node merged = MergeChildren(current, keyIndex);
                    return DeleteTopDown(merged, parent, childIndexInParent, key);
                }
            }
            else
            {
                if (current.IsLeaf)
                {
                    return false;
                }

                int nextChildIndex = 0;
                while (nextChildIndex < current.KeyCount && _comparer.Compare(key, current.Keys[nextChildIndex]) > 0)
                {
                    nextChildIndex++;
                }

                return DeleteTopDown(current.Children[nextChildIndex]!, current, nextChildIndex, key);
            }
        }

        private Node EnsureMinKeys(Node parent, int childIdx)
        {
            Node target = parent.Children[childIdx]!;
            if (target.KeyCount > 1) return target;

            if (childIdx > 0 && parent.Children[childIdx - 1]!.KeyCount > 1)
            {
                Node leftSibling = parent.Children[childIdx - 1]!;

                target.InsertKey(0, parent.Keys[childIdx - 1], parent.Values[childIdx - 1]!);
                if (!target.IsLeaf)
                {
                    target.InsertChild(0, leftSibling.Children[leftSibling.KeyCount]!);
                    leftSibling.Children[leftSibling.KeyCount] = null;
                }

                parent.Keys[childIdx - 1] = leftSibling.Keys[leftSibling.KeyCount - 1];
                parent.Values[childIdx - 1] = leftSibling.Values[leftSibling.KeyCount - 1];
                leftSibling.RemoveKey(leftSibling.KeyCount - 1);

                return target;
            }
            else if (childIdx < parent.KeyCount && parent.Children[childIdx + 1]!.KeyCount > 1)
            {
                Node rightSibling = parent.Children[childIdx + 1]!;

                target.InsertKey(target.KeyCount, parent.Keys[childIdx], parent.Values[childIdx]!);
                if (!target.IsLeaf)
                {
                    target.InsertChild(target.KeyCount, rightSibling.Children[0]!);
                    rightSibling.RemoveChild(0);
                }

                parent.Keys[childIdx] = rightSibling.Keys[0];
                parent.Values[childIdx] = rightSibling.Values[0];
                rightSibling.RemoveKey(0);

                return target;
            }
            else
            {
                if (childIdx < parent.KeyCount)
                {
                    return MergeChildren(parent, childIdx);
                }
                else
                {
                    return MergeChildren(parent, childIdx - 1);
                }
            }
        }

        private Node MergeChildren(Node parent, int leftChildIdx)
        {
            Node leftChild = parent.Children[leftChildIdx]!;
            Node rightChild = parent.Children[leftChildIdx + 1]!;

            leftChild.Keys[leftChild.KeyCount] = parent.Keys[leftChildIdx];
            leftChild.Values[leftChild.KeyCount] = parent.Values[leftChildIdx];
            leftChild.KeyCount++;

            for (int i = 0; i < rightChild.KeyCount; i++)
            {
                leftChild.Keys[leftChild.KeyCount] = rightChild.Keys[i];
                leftChild.Values[leftChild.KeyCount] = rightChild.Values[i];
                leftChild.KeyCount++;
            }

            if (!leftChild.IsLeaf)
            {
                int offset = leftChild.KeyCount - rightChild.KeyCount;
                for (int i = 0; i <= rightChild.KeyCount; i++)
                {
                    leftChild.Children[offset + i] = rightChild.Children[i];
                }
            }

            parent.RemoveKey(leftChildIdx);
            parent.RemoveChild(leftChildIdx + 1);

            return leftChild;
        }

        private KeyValuePair<TKey, TValue> DeletePredecessor(Node current, Node parent, int childIndex)
        {
            if (current.KeyCount == 1 && parent != null && current != _root)
            {
                current = EnsureMinKeys(parent, childIndex);
            }

            if (current.IsLeaf)
            {
                int last = current.KeyCount - 1;
                var kvp = new KeyValuePair<TKey, TValue>(current.Keys[last], current.Values[last]!);
                current.RemoveKey(last);
                return kvp;
            }

            return DeletePredecessor(current.Children[current.KeyCount]!, current, current.KeyCount);
        }

        private KeyValuePair<TKey, TValue> DeleteSuccessor(Node current, Node parent, int childIndex)
        {
            if (current.KeyCount == 1 && parent != null && current != _root)
            {
                current = EnsureMinKeys(parent, childIndex);
            }

            if (current.IsLeaf)
            {
                var kvp = new KeyValuePair<TKey, TValue>(current.Keys[0], current.Values[0]!);
                current.RemoveKey(0);
                return kvp;
            }

            return DeleteSuccessor(current.Children[0]!, current, 0);
        }

        private sealed class Node
        {
            public readonly TKey[] Keys = new TKey[3];
            public readonly TValue?[] Values = new TValue?[3];
            public readonly Node?[] Children = new Node?[4];
            public int KeyCount;

            public bool IsLeaf => Children[0] == null;

            public void InsertKey(int index, TKey key, TValue? value)
            {
                for (int i = KeyCount; i > index; i--)
                {
                    Keys[i] = Keys[i - 1];
                    Values[i] = Values[i - 1];
                }
                Keys[index] = key;
                Values[index] = value;
                KeyCount++;
            }

            public void InsertKeyOrdered(TKey key, TValue value, IComparer<TKey> comparer)
            {
                int i = KeyCount - 1;
                while (i >= 0 && comparer.Compare(key, Keys[i]) < 0)
                {
                    Keys[i + 1] = Keys[i];
                    Values[i + 1] = Values[i];
                    i--;
                }
                Keys[i + 1] = key;
                Values[i + 1] = value;
                KeyCount++;
            }

            public void InsertChild(int index, Node child)
            {
                for (int i = KeyCount; i > index; i--)
                {
                    Children[i] = Children[i - 1];
                }
                Children[index] = child;
            }

            public void RemoveKey(int index)
            {
                for (int i = index; i < KeyCount - 1; i++)
                {
                    Keys[i] = Keys[i + 1];
                    Values[i] = Values[i + 1];
                }
                Keys[KeyCount - 1] = default!;
                Values[KeyCount - 1] = default!;
                KeyCount--;
            }

            public void RemoveChild(int index)
            {
                for (int i = index; i <= KeyCount; i++)
                {
                    Children[i] = Children[i + 1];
                }
                Children[KeyCount + 1] = null;
            }
        }

        private sealed class TraversalFrame
        {
            public Node Node { get; }
            public int CurrentIndex { get; set; }
            public bool ChildVisited { get; set; }

            public TraversalFrame(Node node)
            {
                Node = node;
                CurrentIndex = 0;
                ChildVisited = false;
            }
        }
    }
}