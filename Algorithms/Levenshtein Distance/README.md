# Levenshtein Distance with Path Reconstruction

### 1. Introduction
The Levenshtein Distance measures the minimum number of single-character edits (insertions, deletions, or substitutions) required to change one word or sequence into another. This implementation generalizes the algorithm to work on any generic sequence (`IEnumerable<T>`) and reconstructs the exact sequence of edit operations (`Keep`, `Insert`, `Delete`, `Substitute`) required to transform the source sequence into the target sequence.

Use cases include:
- Spell checking and auto-correction.
- DNA sequence alignment.
- Git-like diff tools for comparing lines of text.
- Approximate string matching.

---

### 2. Usage

```csharp
using System;
using Algorithms;

class Program
{
    static void Main()
    {
        string source = "kitten";
        string target = "sitting";

        LevenshteinResult<char> result = Levenshtein.Compute(source, target);

        Console.WriteLine($"Total Edit Distance: {result.Distance}");
        Console.WriteLine("Steps:");
        foreach (var step in result.Steps)
        {
            Console.WriteLine($"Operation: {step.Operation,-12} | Source Index: {step.SourceIndex} | '{step.SourceElement}' -> '{step.TargetElement}'");
        }
    }
}
```

---

### 3. Detailed Explanation
The algorithm uses dynamic programming to construct an $(M+1) \times (N+1)$ matrix `dp`, where $M$ is the length of the source sequence and $N$ is the length of the target sequence. 

1. **Initialization**: The first row and column represent transforming to/from empty sequences, initialized with incremental indices (representing pure insertions or deletions).
2. **DP Matrix Fill**: For each cell `dp[i, j]`, if the elements match, the cost is carried over from `dp[i-1, j-1]`. If they mismatch, the cell is assigned the minimum of:
   - Deletion: `dp[i-1, j] + 1`
   - Insertion: `dp[i, j-1] + 1`
   - Substitution: `dp[i-1, j-1] + 1`
3. **Backtracking**: Starting from `dp[M, N]`, the algorithm traces back to `dp[0, 0]` by comparing the current cell's value with its neighbors. It prioritizes operations consistently (`Keep` -> `Substitute` -> `Delete` -> `Insert`) to resolve ties and reconstructs the steps in reverse order.

---

### 4. Complexity Analysis

- **Time Complexity**: $O(M \times N)$ where $M$ is the length of the source sequence and $N$ is the length of the target sequence. Every cell in the matrix is computed exactly once.
- **Space Complexity**: $O(M \times N)$ to store the dynamic programming matrix for backtracking. Reconstructed steps require $O(M + N)$ auxiliary space.