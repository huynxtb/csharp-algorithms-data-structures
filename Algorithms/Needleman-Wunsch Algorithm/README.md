# Needleman-Wunsch Algorithm

### 1. Introduction
The Needleman-Wunsch algorithm performs global sequence alignment on two strings (typically DNA, RNA, or protein sequences). It uses dynamic programming to find the alignment that maximizes similarity based on a user-defined scoring system for matches, mismatches, and gaps.

### 2. Usage
```csharp
using System;
using SequenceAlignment;

class Program
{
    static void Main()
    {
        // Match: 1, Mismatch: -1, Gap: -2
        var aligner = new NeedlemanWunsch(1, -1, -2);
        AlignmentResult result = aligner.Align("GATTACA", "GCATGCU");

        Console.WriteLine($"Aligned A: {result.AlignedSequenceA}");
        Console.WriteLine($"Aligned B: {result.AlignedSequenceB}");
        Console.WriteLine($"Score: {result.Score}");
    }
}
```

### 3. Detailed Explanation
- **Matrix Initialization**: A 2D grid of size `(N+1) x (M+1)` is created. The first row and column are initialized with cumulative gap penalties.
- **Matrix Filling**: The algorithm iterates through the matrix, calculating the score for each cell based on three possible moves: diagonal (match/mismatch), up (gap in sequence B), and left (gap in sequence A). The maximum value is stored.
- **Traceback**: Starting from the bottom-right cell `(N, M)`, the algorithm traces back to `(0, 0)` by checking which neighbor yielded the current cell's score. Ties are resolved deterministically with diagonal moves prioritized first, followed by vertical, then horizontal.

### 4. Complexity Analysis
- **Time Complexity**: $O(N \times M)$ where $N$ and $M$ are the lengths of the two sequences. Every cell in the matrix is computed exactly once.
- **Space Complexity**: $O(N \times M)$ to store the dynamic programming scoring matrix.