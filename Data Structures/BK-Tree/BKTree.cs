using System;
using System.Collections.Generic;

public class BKTree
{
    private class Node
    {
        public string Word { get; }
        public Dictionary<int, Node> Children { get; }

        public Node(string word)
        {
            Word = word ?? throw new ArgumentNullException(nameof(word));
            Children = new Dictionary<int, Node>();
        }
    }

    private Node _root;

    public void Add(string word)
    {
        if (word == null) throw new ArgumentNullException(nameof(word));

        if (_root == null)
        {
            _root = new Node(word);
        }
        else
        {
            AddRecursive(_root, word);
        }
    }

    private void AddRecursive(Node node, string word)
    {
        int distance = GetLevenshteinDistance(node.Word, word);
        if (distance == 0) return;

        if (node.Children.TryGetValue(distance, out Node child))
        {
            AddRecursive(child, word);
        }
        else
        {
            node.Children[distance] = new Node(word);
        }
    }

    public List<(string Word, int Distance)> Search(string query, int maxDistance)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        if (maxDistance < 0) throw new ArgumentException("Max distance cannot be negative.", nameof(maxDistance));

        var results = new List<(string Word, int Distance)>();
        if (_root == null) return results;

        SearchRecursive(_root, query, maxDistance, results);
        return results;
    }

    private void SearchRecursive(Node node, string query, int maxDistance, List<(string Word, int Distance)> results)
    {
        int distance = GetLevenshteinDistance(node.Word, query);

        if (distance <= maxDistance)
        {
            results.Add((node.Word, distance));
        }

        int minLimit = distance - maxDistance;
        int maxLimit = distance + maxDistance;

        foreach (var childKeyValuePair in node.Children)
        {
            int childDistance = childKeyValuePair.Key;
            if (childDistance >= minLimit && childDistance <= maxLimit)
            {
                SearchRecursive(childKeyValuePair.Value, query, maxDistance, results);
            }
        }
    }

    private static int GetLevenshteinDistance(string s, string t)
    {
        if (s.Length < t.Length)
        {
            var temp = s;
            s = t;
            t = temp;
        }

        int m = s.Length;
        int n = t.Length;

        if (n == 0) return m;

        int[] previousRow = new int[n + 1];
        int[] currentRow = new int[n + 1];

        for (int j = 0; j <= n; j++)
        {
            previousRow[j] = j;
        }

        for (int i = 1; i <= m; i++)
        {
            currentRow[0] = i;
            for (int j = 1; j <= n; j++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                currentRow[j] = Math.Min(
                    Math.Min(currentRow[j - 1] + 1, previousRow[j] + 1),
                    previousRow[j - 1] + cost
                );
            }
            var tempRow = previousRow;
            previousRow = currentRow;
            currentRow = tempRow;
        }

        return previousRow[n];
    }
}