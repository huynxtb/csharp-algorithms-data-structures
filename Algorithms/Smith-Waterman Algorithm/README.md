# Smith-Waterman Algorithm

### 1. Introduction
The Smith-Waterman algorithm performs local sequence alignment to identify similar regions between two strings of nucleic acid or protein sequences. Unlike the Needleman-Wunsch algorithm, which aligns sequences globally from end to end, Smith-Waterman finds the optimal local alignment by preventing negative scores in the dynamic programming matrix, allowing alignments to start and end anywhere within the sequences.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        string seqA = "HELLOWORLD";
        string seqB = "WORLD";

        // Define scoring parameters
        int matchScore = 2;
        int mismatchPenalty = -1;
        int gapPenalty = -2;

        SmithWaterman.AlignmentResult result = SmithWaterman.Align(seqA, seqB, matchScore, mismatchPenalty, gapPenalty);

        Console.WriteLine($"Score: {result.Score}");
        Console.WriteLine($"Aligned A: {result.AlignedSequenceA} (Indices: {result.StartIndexA} to {result.EndIndexA})");
        Console.WriteLine($"Aligned B: {result.AlignedSequenceB} (Indices: {result.StartIndexB} to {result.EndIndexB})");
    }
}
```

### 3. Detailed Explanation
The algorithm constructs a scoring matrix $H$ of size $(M+1) \times (N+1)$, where $M$ and $N$ are the lengths of the two sequences. 

1. **Initialization**: The first row and column are initialized to 0.
2. **Matrix Filling**: Each cell $H[i, j]$ is calculated as the maximum of four values:
   - 0 (prevents negative scores, allowing local restarts)
   - $H[i-1, j-1] + \text{match/mismatch score}$ (diagonal transition)
   - $H[i-1, j] + \text{gap penalty}$ (vertical transition / deletion)
   - $H[i, j-1] + \text{gap penalty}$ (horizontal transition / insertion)
3. **Backtracking**: Starts at the maximum value in the entire matrix $H$ and traces back until a cell containing 0 is reached. This path defines the optimal local alignment.

### 4. Complexity Analysis
- **Time Complexity**: $O(M \times N)$ to fill the scoring matrix, where $M$ and $N$ are the lengths of the sequences. Backtracking takes $O(M + N)$ time.
- **Space Complexity**: $O(M \times N)$ to store the dynamic programming matrix.