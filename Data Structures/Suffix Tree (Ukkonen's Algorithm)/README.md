# Suffix Tree (Ukkonen's Algorithm)

## 1. Introduction
A **Suffix Tree** is a compressed trie containing all the suffixes of a given text. It is one of the most fundamental data structures in string processing, bioinformatics, and pattern matching.

**Ukkonen's Algorithm** constructs the suffix tree on-line in strictly linear time ($O(N)$) and linear space ($O(N)$), where $N$ is the length of the string. It employs three crucial optimizations:
- **Implicit Suffix Trees / Active Point**: Tracks the state across phases.
- **Global Leaf End (Trick 1)**: Automatically extends all existing leaves in $O(1)$ time per phase.
- **Skip/Count Trick (Trick 2)**: Steps through internal nodes in $O(1)$ time using edge lengths.
- **Suffix Links**: Allows fast transitions between internal nodes when inserting suffixes.

### When to Use
- Full-text search and multi-pattern lookup.
- Finding the Longest Repeated Substring (LRS).
- Finding the Longest Common Substring (LCS) across multiple strings.
- Exact substring pattern matching queries in $O(M)$ time.

---

## 2. Usage

```csharp
using System;
using System.Collections.Generic;
using SuffixTreeAlgorithm;

class Program
{
    static void Main()
    {
        string text = "banana";
        SuffixTree tree = new SuffixTree(text);

        // 1. Substring Existence Check - O(M)
        bool hasNan = tree.ContainsSubstring("nan"); // True
        bool hasApp = tree.ContainsSubstring("apple"); // False

        // 2. Find All Occurrences - O(M + K)
        List<int> occurrences = tree.FindAllOccurrences("an"); // [1, 3]

        // 3. Longest Repeated Substring
        string lrs = tree.GetLongestRepeatedSubstring(); // "ana"
    }
}
```

---

## 3. Detailed Explanation

### Core Classes
1. **`SuffixTreeNode`**:
   - `Children`: Map from edge leading character to the child node.
   - `Start` and `End`: Represent the edge substring range $[Start, End.Value]$ pointing into the master string.
   - `SuffixLink`: Pointer to another node for $O(1)$ suffix transitions.
   - `SuffixIndex`: 0-based suffix start position assigned to leaves.
2. **`EndIndex`**:
   - Reference-type wrapper around an integer to allow all leaf node edges to share a single incrementing variable (`_leafEnd`).
3. **`SuffixTree`**:
   - Manages tree construction via phases ($i = 0$ to $N-1$).
   - Tracks `_activeNode`, `_activeEdge`, `_activeLength`, and `_remainingSuffixCount`.

### Methods
- **`ContainsSubstring(pattern)`**: Traverses the edges matching the characters in `pattern`. If matched completely, returns `true`.
- **`FindAllOccurrences(pattern)`**: Navigates to the node matching the pattern and performs DFS to collect all leaf suffix indices.
- **`GetLongestRepeatedSubstring()`**: Performs a DFS over internal nodes (nodes with $\ge 2$ children) to find the node with maximum string depth.

---

## 4. Complexity Analysis

| Operation | Time Complexity | Space Complexity |
| :--- | :--- | :--- |
| **Tree Construction** | $O(N)$ | $O(N)$ |
| **Pattern Search (`ContainsSubstring`)** | $O(M)$ | $O(1)$ |
| **Find All Occurrences (`FindAllOccurrences`)** | $O(M + K)$ | $O(K)$ |
| **Longest Repeated Substring (`GetLongestRepeatedSubstring`)** | $O(N)$ | $O(N)$ |

*Where $N$ is the length of the indexed text, $M$ is the pattern length, and $K$ is the number of occurrences.*