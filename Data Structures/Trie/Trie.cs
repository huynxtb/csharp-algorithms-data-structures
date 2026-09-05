using System.Collections.Generic;

public class Trie
{
    private class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
        public bool IsEndOfWord { get; set; }
    }

    private readonly TrieNode _root;

    public Trie()
    {
        _root = new TrieNode();
    }

    public void Insert(string word)
    {
        if (string.IsNullOrEmpty(word)) return;

        TrieNode current = _root;
        foreach (char ch in word)
        {
            if (!current.Children.ContainsKey(ch))
            {
                current.Children[ch] = new TrieNode();
            }
            current = current.Children[ch];
        }
        current.IsEndOfWord = true;
    }

    public bool Search(string word)
    {
        if (string.IsNullOrEmpty(word)) return false;

        TrieNode node = GetNode(word);
        return node != null && node.IsEndOfWord;
    }

    public bool StartsWith(string prefix)
    {
        if (string.IsNullOrEmpty(prefix)) return false;

        return GetNode(prefix) != null;
    }

    private TrieNode GetNode(string key)
    {
        TrieNode current = _root;
        foreach (char ch in key)
        {
            if (!current.Children.TryGetValue(ch, out TrieNode next))
            {
                return null;
            }
            current = next;
        }
        return current;
    }
}