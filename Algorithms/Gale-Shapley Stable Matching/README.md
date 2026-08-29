# Gale-Shapley Stable Matching Algorithm

### 1. Introduction
The Gale-Shapley algorithm solves the Stable Matching Problem (also known as the Stable Marriage Problem). Given an equal number of two sets of elements (e.g., men and women) and their ordered preferences, the algorithm finds a stable matching where no two elements of opposite sets would both prefer each other over their current partners.

Use cases include:
- Matching medical students to residency programs (NRMP).
- Content Delivery Network (CDN) user-to-server assignments.
- Matching markets and resource allocation.

### 2. Usage
```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Preferences for 3 men (0, 1, 2)
        int[][] menPrefs = new int[][]
        {
            new int[] { 0, 1, 2 }, // Man 0 prefers Woman 0, then 1, then 2
            new int[] { 1, 2, 0 }, // Man 1 prefers Woman 1, then 2, then 0
            new int[] { 0, 1, 2 }  // Man 2 prefers Woman 0, then 1, then 2
        };

        // Preferences for 3 women (0, 1, 2)
        int[][] womenPrefs = new int[][]
        {
            new int[] { 2, 1, 0 }, // Woman 0 prefers Man 2, then 1, then 0
            new int[] { 0, 1, 2 }, // Woman 1 prefers Man 0, then 1, then 2
            new int[] { 2, 0, 1 }  // Woman 2 prefers Man 2, then 0, then 1
        };

        Dictionary<int, int> matches = StableMatchingSolver.Solve(menPrefs, womenPrefs);

        foreach (var match in matches)
        {
            Console.WriteLine($"Man {match.Key} is matched with Woman {match.Value}");
        }
    }
}
```

### 3. Detailed Explanation
The implementation uses the standard Gale-Shapley loop where men propose to women:
1. **Validation**: The input arrays are validated to ensure they are non-null, square matrices of equal size $N \times N$, and contain valid permutations of indices from $0$ to $N-1$.
2. **Preprocessing**: An inverse lookup table `womenRanking` is constructed. For each woman, it maps a man's index to his rank in her preference list. This allows $O(1)$ comparisons when a woman decides whether to keep her current partner or switch to a new proposer.
3. **Proposal Loop**: Free men are tracked in a queue. Each free man proposes to his most preferred woman to whom he has not yet proposed. If the woman is free, they are provisionally matched. If she is already matched, she compares the proposer with her current partner using the `womenRanking` table and keeps the preferred one, freeing the other.

### 4. Complexity Analysis
- **Time Complexity**: $O(N^2)$ where $N$ is the number of men/women. Preprocessing takes $O(N^2)$ time. The proposal loop runs at most $N^2$ times because each man proposes to each woman at most once.
- **Space Complexity**: $O(N^2)$ to store the `womenRanking` inverse lookup table.