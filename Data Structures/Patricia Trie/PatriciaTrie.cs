using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.DataStructures
{
    public class PatriciaTrie<TValue>
    {
        private class Node
        {
            public string Label { get; set; }
            public Dictionary<char, Node> Children { get; }\ = new Dictionary<char, Node>();
            public bool HasValue { get; set; }
            public TValue Value { get; set; }

            public Node(string label)
            {
                Label = label;
            }
        }

        private readonly Node _root = new Node(string.Empty);
        public int Count { get; private set; }

        public void Insert(string key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (key == string.Empty)
            {
                if (!_root.HasValue)
                {
                    Count++;
                }
                _root.HasValue = true;
                _root.Value = value;
                return;
            }

            Node curr = _root;
            string remaining = key;

            while (true)
            {
                char firstChar = remaining[0];
                if (!curr.Children.TryGetValue(firstChar, out Node child))
                {
                    var newNode = new Node(remaining)
                    {
                        HasValue = true,
                        Value = value
                    };
                    curr.Children.Add(firstChar, newNode);
                    Count++;
                    return;
                }

                int commonLength = GetCommonPrefixLength(remaining, child.Label);

                if (commonLength < child.Label.Length)
                {
                    var splitNode = new Node(child.Label.Substring(commonLength))
                    {
                        HasValue = child.HasValue,
                        Value = child.Value
                    };
                    foreach (var kvp in child.Children)
                    {
                        splitNode.Children.Add(kvp.Key, kvp.Value);
                    }
                    child.Children.Clear();
                    child.Label = child.Label.Substring(0, commonLength);
                    child.HasValue = false;
                    child.Value = default;
                    child.Children.Add(splitNode.Label[0], splitNode);

                    if (commonLength == remaining.Length)
                    {
                        child.HasValue = true;
                        child.Value = value;
                        Count++;
                    }
                    else
                    {
                        var newNode = new Node(remaining.Substring(commonLength))
                        {
                            HasValue = true,
                            Value = value
                        };
                        child.Children.Add(newNode.Label[0], newNode);
                        Count++;
                    }
                    return;
                }
                else
                {
                    if (remaining.Length == child.Label.Length)
                    {
                        if (!child.HasValue)
                        {
                            Count++;
                        }
                        child.HasValue = true;
                        child.Value = value;
                        return;
                    }
                    else
                    {
                        curr = child;
                        remaining = remaining.Substring(commonLength);
                    }
                }
            }
        }

        public bool TryGetValue(string key, out TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (key == string.Empty)
            {
                if (_root.HasValue)
                {
                    value = _root.Value;
                    return true;
                }
                value = default;
                return false;
            }

            Node curr = _root;
            string remaining = key;

            while (remaining.Length > 0)
            {
                if (!curr.Children.TryGetValue(remaining[0], out Node child))
                {
                    value = default;
                    return false;
                }

                if (remaining.StartsWith(child.Label))
                {
                    remaining = remaining.Substring(child.Label.Length);
                    curr = child;
                }
                else
                {
                    value = default;
                    return false;
                }
            }

            if (curr.HasValue)
            {
                value = curr.Value;
                return true;
            }

            value = default;
            return false;
        }

        public bool Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            if (key == string.Empty)
            {
                if (_root.HasValue)
                {
                    _root.HasValue = false;
                    _root.Value = default;
                    Count--;
                    return true;
                }
                return false;
            }

            Node parent = null;
            Node curr = _root;
            string remaining = key;

            while (remaining.Length > 0)
            {
                if (!curr.Children.TryGetValue(remaining[0], out Node child))
                {
                    return false;
                }

                if (remaining.StartsWith(child.Label))
                {
                    remaining = remaining.Substring(child.Label.Length);
                    parent = curr;
                    curr = child;
                }
                else
                {
                    return false;
                }
            }

            if (!curr.HasValue)
            {
                return false;
            }

            curr.HasValue = false;
            curr.Value = default;
            Count--;

            if (curr.Children.Count == 0)
            {
                if (parent != null)
                {
                    parent.Children.Remove(curr.Label[0]);
                    if (parent != _root && !parent.HasValue && parent.Children.Count == 1)
                    { 
                        MergeWithSingleChild(parent);
                    }
                }
            }
            else if (curr.Children.Count == 1)
            {
                MergeWithSingleChild(curr);
            }

            return true;
        }

        public IEnumerable<KeyValuePair<string, TValue>> GetByPrefix(string prefix)
        {
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));

            if (prefix == string.Empty)
            {
                return GetAllKeys(_root, string.Empty);
            }

            Node curr = _root;
            string remaining = prefix;
            string accumulated = string.Empty;

            while (remaining.Length > 0)
            {
                if (!curr.Children.TryGetValue(remaining[0], out Node child))
                {
                    return Enumerable.Empty<KeyValuePair<string, TValue>>();
                }

                if (remaining.StartsWith(child.Label))
                {
                    remaining = remaining.Substring(child.Label.Length);
                    accumulated += child.Label;
                    curr = child;
                }
                else if (child.Label.StartsWith(remaining))
                {
                    accumulated += child.Label;
                    curr = child;
                    break;
                }
                else
                {
                    return Enumerable.Empty<KeyValuePair<string, TValue>>();
                }
            }

            return GetAllKeys(curr, accumulated);
        }

        public void Clear()
        {
            _root.Children.Clear();
            _root.HasValue = false;
            _root.Value = default;
            Count = 0;
        }

        private int GetCommonPrefixLength(string s1, string s2)
        {
            int minLength = Math.Min(s1.Length, s2.Length);
            for (int i = 0; i < minLength; i++)
            {
                if (s1[i] != s2[i])
                {
                    return i;
                }
            }
            return minLength;
        }

        private void MergeWithSingleChild(Node node)
        {
            if (node.Children.Count != 1) return;

            var childKey = node.Children.Keys.First();
            var child = node.Children[childKey];

            node.Label = node.Label + child.Label;
            node.HasValue = child.HasValue;
            node.Value = child.Value;

            node.Children.Clear();
            foreach (var kvp in child.Children)
            {
                node.Children.Add(kvp.Key, kvp.Value);
            }
        }

        private IEnumerable<KeyValuePair<string, TValue>> GetAllKeys(Node node, string currentPath)
        {
            if (node.HasValue)
            {
                yield return new KeyValuePair<string, TValue>(currentPath, node.Value);
            }

            foreach (var child in node.Children.Values)
            {
                foreach (var kvp in GetAllKeys(child, currentPath + child.Label))
                {
                    yield return kvp;
                }
            }
        }
    }
}