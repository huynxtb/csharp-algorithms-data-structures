# Aho-Corasick String Search

## Introduction

The Aho-Corasick algorithm is an efficient string searching algorithm that finds all occurrences of a finite set of keywords within a text. It is particularly useful when searching for multiple patterns simultaneously, outperforming repeated single-pattern searches. Its primary advantage lies in its ability to process the text in a single pass, achieving linear time complexity relative to the text length and the total length of the keywords.

Use Aho-Corasick when:
* You need to find all occurrences of many different keywords in a large text.
* Performance is critical, and repeated single searches are too slow.
* Keywords can overlap, and all overlapping matches need to be reported.

## Usage

```csharp
using System;
using System.Collections.Generic;

public class Example
{
    public static void Main(string[] args)
    {
        var keywords = new List<string> { "he", "she", "his", "hers" };
        var searcher = new AhoCorasickSearcher();
        searcher.Build(keywords);

        string text = "ushers";
        var results = searcher.Search(text);

        Console.WriteLine($"Searching for keywords in: '{text}'");
        foreach (var result in results)
        {
            Console.WriteLine($"Found '{result.Keyword}' at index {result.Index} (Length: {result.Length})");
        }
        // Expected output:
        // Searching for keywords in: 'ushers'
        // Found 'she' at index 1 (Length: 3)
        // Found 'he' at index 2 (Length: 2)
        // Found 'hers' at index 2 (Length: 4)
    }
}
```

## Detailed Explanation

The Aho-Corasick implementation consists of two main phases: building the automaton and searching the text.

1.  **Trie Construction (`Build` method):
    *   A trie (prefix tree) is built from the given set of keywords. Each node in the trie represents a prefix of one or more keywords. Edges are labeled with characters.
    *   When a keyword is inserted, its path is traversed or created in the trie. The node corresponding to the end of a keyword is marked, and the keyword itself is stored in that node's `OutputKeywords` list.

2.  **Failure Link Construction (`ConstructFailureLinks` method):
    *   After the trie is built, failure links are computed for each node using a Breadth-First Search (BFS).
    *   The failure link of a node `u` points to the longest proper suffix of the string represented by `u` that is also a prefix of some keyword.
    *   If a character transition from the current node does not exist, the algorithm follows the failure link to find a shorter matching prefix.
    *   Crucially, failure links also propagate `OutputKeywords`. If a node `v` has a failure link to node `w`, then any keyword ending at `w` is also considered a match when reaching `v`.

3.  **Text Searching (`Search` method):
    *   The algorithm iterates through the input text character by character.
    *   It maintains a `currentNode` pointer, starting at the trie's root.
    *   For each character in the text, it attempts to transition to a child node. If a direct transition is not possible, it follows the `FailureLink` until a transition is found or the root is reached.
    *   After each successful transition, it checks the `currentNode` for any `OutputKeywords`. If keywords are found, `SearchResult` objects are created and added to the results list, indicating the keyword, its starting index in the text, and its length.
    *   The starting index is calculated by subtracting the keyword's length from the current text index and adding 1 (since indices are 0-based and the current index `i` points to the *end* of the match).

## Complexity Analysis

Let:
*   `k` be the number of keywords.
*   `m` be the sum of the lengths of all keywords.
*   `n` be the length of the text to be searched.
*   `z` be the total number of matches found.

*   **Build Time Complexity:** O(m)
    *   Inserting `k` keywords with a total length of `m` into the trie takes O(m) time. Constructing failure links using BFS also takes O(m) time because each node and edge is visited a constant number of times.

*   **Search Time Complexity:** O(n + m + z)
    *   The search phase iterates through the text of length `n`. Each character involves a constant number of trie transitions and failure link traversals. The total number of transitions and failure link traversals is bounded by O(n).
    *   Reporting matches: In the worst case, each keyword might be reported multiple times if it's a suffix of another keyword. However, the total number of `SearchResult` objects generated is `z`. The `OutputKeywords.AddRange` operation during failure link construction ensures that all relevant keywords are available at each node, contributing to the O(m) factor in the overall complexity for setup and the O(z) factor for reporting.

*   **Space Complexity:** O(m)
    *   The space required is dominated by the trie structure, which stores all characters of all keywords. The failure links and output lists add to this space, but the total is proportional to the total length of the keywords, `m`.
