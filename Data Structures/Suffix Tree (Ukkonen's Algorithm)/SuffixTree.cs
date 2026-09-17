using System;
using System.Collections.Generic;

namespace SuffixTreeAlgorithm
{
    /// <summary>
    /// Represents a single node or state within the Suffix Tree.
    /// </summary>
    public class SuffixTreeNode
    {
        /// <summary>
        /// Child transitions keyed by the starting character of the edge.
        /// </summary>
        public Dictionary<char, SuffixTreeNode> Children { get; } = new Dictionary<char, SuffixTreeNode>();

        /// <summary>
        /// Pointer to another internal node representing the longest proper suffix of the string path to this node.
        /// </summary>
        public SuffixTreeNode SuffixLink { get; set; }

        /// <summary>
        /// Starting index in the source string of the edge leading into this node.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Reference to the ending index in the source string of the edge leading into this node.
        /// For leaf nodes, this points to a shared global end index.
        /// </summary>
        public EndIndex End { get; set; }

        /// <summary>
        /// Suffix index assigned to leaf nodes (0-based start index of the suffix in the original string).
        /// For internal nodes, this remains -1.
        /// </summary>
        public int SuffixIndex { get; set; } = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuffixTreeNode"/> class.
        /// </summary>
        /// <param name="start">Start index in the master text.</param>
        /// <param name="end">End index reference in the master text.</param>
        public SuffixTreeNode(int start, EndIndex end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Calculates the length of the incoming edge to this node.
        /// </summary>
        /// <returns>Edge length in characters.</returns>
        public int EdgeLength()
        {
            if (Start == -1 || End == null)
            {
                return 0;
            }
            return End.Value - Start + 1;
        }
    }

    /// <summary>
    /// Reference wrapper for edge ending positions, enabling O(1) leaf edge extensions (Trick 1).
    /// </summary>
    public class EndIndex
    {
        /// <summary>
        /// The current ending index value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndIndex"/> class.
        /// </summary>
        /// <param name="val">The initial index value.</param>
        public EndIndex(int val)
        {
            Value = val;
        }
    }

    /// <summary>
    /// Implementation of Ukkonen's Algorithm for constructing a Suffix Tree in linear time and space.
    /// </summary>
    public class SuffixTree
    {
        private const char SentinelChar = '$';

        private readonly string _text;
        private readonly SuffixTreeNode _root;

        private SuffixTreeNode _activeNode;
        private int _activeEdge = -1;
        private int _activeLength = 0;
        private int _remainingSuffixCount = 0;
        private readonly EndIndex _leafEnd = new EndIndex(-1);

        /// <summary>
        /// Gets the root node of the suffix tree.
        /// </summary>
        public SuffixTreeNode Root => _root;

        /// <summary>
        /// Gets the indexed text including the appended unique sentinel character if applicable.
        /// </summary>
        public string Text => _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuffixTree"/> class and builds the tree in O(N) time.
        /// </summary>
        /// <param name="text">The input string to index.</param>
        public SuffixTree(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            // Ensure the text ends with a unique sentinel character
            _text = text.EndsWith(SentinelChar.ToString(), StringComparison.Ordinal) ? text : text + SentinelChar;

            _root = new SuffixTreeNode(-1, new EndIndex(-1));
            _activeNode = _root;

            Build();
            SetSuffixIndices(_root, 0);
        }

        /// <summary>
        /// Executes Ukkonen's algorithm across all phases.
        /// </summary>
        private void Build()
        {
            for (int i = 0; i < _text.Length; i++)
            {
                ExtendSuffixTree(i);
            }
        }

        /// <summary>
        /// Extends the suffix tree by adding character at index <paramref name="phase"/>.
        /// </summary>
        /// <param name="phase">The current phase index.</param>
        private void ExtendSuffixTree(int phase)
        {
            _leafEnd.Value = phase;
            _remainingSuffixCount++;
            SuffixTreeNode lastCreatedInternalNode = null;

            while (_remainingSuffixCount > 0)
            {
                if (_activeLength == 0)
                {
                    _activeEdge = phase;
                }

                char currentEdgeChar = _text[_activeEdge];

                if (!_activeNode.Children.ContainsKey(currentEdgeChar))
                {
                    // Rule 2: Create a new leaf edge
                    _activeNode.Children[currentEdgeChar] = new SuffixTreeNode(phase, _leafEnd);

                    if (lastCreatedInternalNode != null)
                    {
                        lastCreatedInternalNode.SuffixLink = _activeNode;
                        lastCreatedInternalNode = null;
                    }
                }
                else
                {
                    SuffixTreeNode next = _activeNode.Children[currentEdgeChar];
                    int edgeLen = next.EdgeLength();

                    // Trick 2: Skip/Count down the tree
                    if (_activeLength >= edgeLen)
                    {
                        _activeEdge += edgeLen;
                        _activeLength -= edgeLen;
                        _activeNode = next;
                        continue;
                    }

                    // Rule 3: Character already exists on edge (Observation 1: stop phase)
                    if (_text[next.Start + _activeLength] == _text[phase])
                    {
                        if (lastCreatedInternalNode != null && _activeNode != _root)
                        {
                            lastCreatedInternalNode.SuffixLink = _activeNode;
                            lastCreatedInternalNode = null;
                        }

                        _activeLength++;
                        break;
                    }

                    // Rule 2: Split edge and insert new internal node
                    var splitEnd = new EndIndex(next.Start + _activeLength - 1);
                    var split = new SuffixTreeNode(next.Start, splitEnd);
                    _activeNode.Children[currentEdgeChar] = split;

                    // New leaf edge for incoming character
                    split.Children[_text[phase]] = new SuffixTreeNode(phase, _leafEnd);

                    // Adjust existing next node
                    next.Start += _activeLength;
                    split.Children[_text[next.Start]] = next;

                    if (lastCreatedInternalNode != null)
                    {
                        lastCreatedInternalNode.SuffixLink = split;
                    }

                    lastCreatedInternalNode = split;
                }

                _remainingSuffixCount--;

                if (_activeNode == _root && _activeLength > 0)
                {
                    _activeLength--;
                    _activeEdge = phase - _remainingSuffixCount + 1;
                }
                else if (_activeNode != _root)
                {
                    _activeNode = _activeNode.SuffixLink ?? _root;
                }
            }
        }

        /// <summary>
        /// Traverses the tree via DFS to assign suffix indices to all leaf nodes.
        /// </summary>
        private void SetSuffixIndices(SuffixTreeNode node, int labelHeight)
        {
            if (node == null)
            {
                return;
            }

            bool isLeaf = true;
            foreach (var child in node.Children.Values)
            {
                isLeaf = false;
                SetSuffixIndices(child, labelHeight + child.EdgeLength());
            }

            if (isLeaf)
            {
                node.SuffixIndex = _text.Length - labelHeight;
            }
        }

        /// <summary>
        /// Determines whether the specified pattern exists as a substring within the indexed text in O(M) time.
        /// </summary>
        /// <param name="pattern">The search pattern.</param>
        /// <returns><c>true</c> if pattern is found; otherwise, <c>false</c>.</returns>
        public bool ContainsSubstring(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            if (pattern.Length == 0)
            {
                return true;
            }

            SuffixTreeNode current = _root;
            int i = 0;
            while (i < pattern.Length)
            {
                char c = pattern[i];
                if (!current.Children.TryGetValue(c, out SuffixTreeNode child))
                {
                    return false;
                }

                int edgeLen = child.EdgeLength();
                int j = 0;
                while (j < edgeLen && i < pattern.Length)
                {
                    if (_text[child.Start + j] != pattern[i])
                    {
                        return false;
                    }
                    i++;
                    j++;
                }

                current = child;
            }

            return true;
        }

        /// <summary>
        /// Finds all 0-based starting indices where the given pattern occurs in the indexed text.
        /// </summary>
        /// <param name="pattern">The search pattern.</param>
        /// <returns>A list of 0-based start positions.</returns>
        public List<int> FindAllOccurrences(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var result = new List<int>();
            if (pattern.Length == 0)
            {
                return result;
            }

            SuffixTreeNode current = _root;
            int i = 0;
            while (i < pattern.Length)
            {
                char c = pattern[i];
                if (!current.Children.TryGetValue(c, out SuffixTreeNode child))
                {
                    return result;
                }

                int edgeLen = child.EdgeLength();
                int j = 0;
                while (j < edgeLen && i < pattern.Length)
                {
                    if (_text[child.Start + j] != pattern[i])
                    {
                        return result;
                    }
                    i++;
                    j++;
                }

                current = child;
            }

            CollectLeafIndices(current, result);
            result.Sort();
            return result;
        }

        /// <summary>
        /// Gathers all suffix indices rooted at a given node.
        /// </summary>
        private void CollectLeafIndices(SuffixTreeNode node, List<int> result)
        {
            if (node == null)
            {
                return;
            }

            if (node.SuffixIndex != -1)
            {
                result.Add(node.SuffixIndex);
                return;
            }

            foreach (var child in node.Children.Values)
            {
                CollectLeafIndices(child, result);
            }
        }

        /// <summary>
        /// Finds and returns the longest substring that appears at least twice in the indexed text.
        /// </summary>
        /// <returns>The longest repeated substring, or an empty string if none exists.</returns>
        public string GetLongestRepeatedSubstring()
        {
            int maxLen = 0;
            int maxStart = 0;

            void Dfs(SuffixTreeNode node, int currentDepth, int pathStart)
            {
                if (node == null)
                {
                    return;
                }

                // An internal node has >= 2 children representing repeated substrings
                if (node.Children.Count >= 2 && currentDepth > maxLen)
                {
                    maxLen = currentDepth;
                    maxStart = pathStart;
                }

                foreach (var child in node.Children.Values)
                {
                    int edgeLen = child.EdgeLength();
                    int childPathStart = (currentDepth == 0) ? child.Start : pathStart;
                    Dfs(child, currentDepth + edgeLen, childPathStart);
                }
            }

            foreach (var child in _root.Children.Values)
            {
                Dfs(child, child.EdgeLength(), child.Start);
            }

            return maxLen > 0 ? _text.Substring(maxStart, maxLen) : string.Empty;
        }
    }
}