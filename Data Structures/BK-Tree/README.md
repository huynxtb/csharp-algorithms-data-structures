# BK-Tree (Burkhard-Keller Tree)

### 1. Introduction
A BK-Tree is a metric tree specialized for fuzzy string matching, spell checking, and approximate string search. It organizes a dictionary of words based on their pairwise Levenshtein distances. By leveraging the triangle inequality property of metric spaces, the tree prunes large portions of the search space during queries, making it significantly faster than linear scans.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        BKTree tree = new BKTree();
        
        // Populate the tree
        tree.Add("book");
        tree.Add("books");
        tree.Add("boo");
        tree.Add("cake");
        tree.Add("cape");
        tree.Add("cart");

        // Search for words within an edit distance of 1 from "cape"
        var results = tree.Search("cape", 1);

        foreach (var result in results)
        {
            Console.WriteLine($"Match: {result.Word}, Distance: {result.Distance}");
        }
    }
}
```

### 3. Detailed Explanation
- **Node Structure**: Each node contains a string value and a dictionary mapping integer distances to child nodes.
- **Insertion (`Add`)**: Calculates the Levenshtein distance $d$ between the new word and the current node. If a child node exists at distance $d$, the insertion recurses down that branch. Otherwise, a new child node is created at key $d$.
- **Search (`Search`)**: Given a query string and a threshold `maxDistance`, the algorithm computes the distance $D$ between the query and the current node. If $D \le \text{maxDistance}$, the node's word is added to the results. The search then recursively visits only the children whose edge distance $d$ satisfies the triangle inequality condition: $D - \text{maxDistance} \le d \le D + \text{maxDistance}$.
- **Levenshtein Distance**: Implemented using a space-optimized dynamic programming approach. By keeping track of only the current and previous rows of the DP table, the auxiliary space complexity is reduced to $O(\min(M, N))$.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Insertion**: $O(L \cdot \log N)$ on average, where $L$ is the average word length and $N$ is the number of nodes. Worst-case is $O(L \cdot N)$ for highly degenerate trees.
  - **Search**: $O(L \cdot N^{\alpha})$ on average, where $\alpha < 1$ depending on the metric space density and the search threshold. Worst-case is $O(L \cdot N)$ when searching with a large threshold.
- **Space Complexity**:
  - **Tree Storage**: $O(N \cdot L)$ to store all words and tree pointers.
  - **Levenshtein Helper**: $O(\min(M, N))$ auxiliary space, where $M$ and $N$ are the lengths of the compared strings.