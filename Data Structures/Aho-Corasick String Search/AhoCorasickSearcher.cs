using System;
using System.Collections.Generic;
using System.Linq;

public class AhoCorasickSearcher
{
    public struct SearchResult
    {
        public string Keyword { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
    }

    private class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; private set; }
        public TrieNode FailureLink { get; set; }
        public List<string> OutputKeywords { get; private set; }
        public bool IsEndOfKeyword { get; set; }

        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode>();
            OutputKeywords = new List<string>();
        }
    }

    private TrieNode _root;

    public AhoCorasickSearcher()
    {
        _root = new TrieNode();
    }

    public void Build(IEnumerable<string> keywords)
    {
        _root = new TrieNode();
        foreach (var keyword in keywords)
        {
            if (string.IsNullOrEmpty(keyword))
                continue;
            InsertKeyword(_root, keyword);
        }
        ConstructFailureLinks();
    }

    private void InsertKeyword(TrieNode root, string keyword)
    {
        TrieNode currentNode = root;
        foreach (char c in keyword)
        {
            if (!currentNode.Children.TryGetValue(c, out TrieNode nextNode))
            {
                nextNode = new TrieNode();
                currentNode.Children[c] = nextNode;
            }
            currentNode = nextNode;
        }
        currentNode.IsEndOfKeyword = true;
        currentNode.OutputKeywords.Add(keyword);
    }

    private void ConstructFailureLinks()
    {
        var queue = new Queue<TrieNode>();

        foreach (var child in _root.Children.Values)
        {
            child.FailureLink = _root;
            queue.Enqueue(child);
        }

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();

            foreach (var kvp in currentNode.Children)
            {
                char transitionChar = kvp.Key;
                TrieNode childNode = kvp.Value;
                TrieNode tempFailureNode = currentNode.FailureLink;

                while (tempFailureNode != null && !tempFailureNode.Children.ContainsKey(transitionChar))
                {
                    tempFailureNode = tempFailureNode.FailureLink;
                }

                childNode.FailureLink = (tempFailureNode == null) ? _root : tempFailureNode.Children[transitionChar];
                childNode.OutputKeywords.AddRange(childNode.FailureLink.OutputKeywords);
                queue.Enqueue(childNode);
            }
        }
    }

    public IEnumerable<SearchResult> Search(string text)
    {
        if (string.IsNullOrEmpty(text) || _root == null)
        {
            return Enumerable.Empty<SearchResult>();
        }

        var results = new List<SearchResult>();
        TrieNode currentNode = _root;

        for (int i = 0; i < text.Length; i++)
        {
            char currentChar = text[i];

            while (currentNode != null && !currentNode.Children.ContainsKey(currentChar))
            {
                currentNode = currentNode.FailureLink;
            }

            if (currentNode == null)
            {
                currentNode = _root;
                continue;
            }

            currentNode = currentNode.Children[currentChar];

            if (currentNode.IsEndOfKeyword || currentNode.OutputKeywords.Any())
            {
                foreach (var keyword in currentNode.OutputKeywords)
                {
                    results.Add(new SearchResult
                    {
                        Keyword = keyword,
                        Index = i - keyword.Length + 1,
                        Length = keyword.Length
                    });
                }
            }
        }

        return results;
    }
}