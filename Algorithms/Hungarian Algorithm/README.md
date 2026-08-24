# Hungarian Algorithm (Kuhn-Munkres)

### 1. Introduction
The Hungarian Algorithm (also known as the Kuhn-Munkres algorithm) is a combinatorial optimization algorithm that solves the assignment problem in polynomial time. Given a square cost matrix where the element at row `i` and column `j` represents the cost of assigning worker `i` to job `j`, the algorithm finds the assignment that minimizes the total cost.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        double[,] costMatrix = new double[,]
        {
            { 4, 1, 3 },
            { 2, 0, 5 },
            { 3, 2, 2 }
        };

        int[] assignment = HungarianAlgorithm.FindMinWeightAssignment(costMatrix);

        for (int i = 0; i < assignment.Length; i++)
        {
            Console.WriteLine($"Row {i} is assigned to Column {assignment[i]} (Cost: {costMatrix[i, assignment[i]]})");
        }
    }
}
```

### 3. Detailed Explanation
This implementation uses the $O(N^3)$ augmenting path method with potentials (dual variables):
- **Potentials ($u$ and $v$):** Row potentials $u$ and column potentials $v$ are maintained such that $u[i] + v[j] \le costMatrix[i][j]$ for all $i, j$.
- **Slack Array (`minv`):** Tracks the minimum slack values to quickly find the next node to add to the alternating tree, reducing the complexity of finding augmenting paths.
- **Augmenting Paths:** In each step, the algorithm finds an augmenting path using a BFS-like search. If no augmenting path is found, the potentials are updated using the minimum slack value (`delta`), which guarantees that at least one new zero-slack edge is introduced.
- **Backtracking:** Once an augmenting path is found, the matching is updated along the path.

### 4. Complexity Analysis
- **Time Complexity:** $O(N^3)$ where $N$ is the number of rows (and columns) in the square matrix. The algorithm performs $N$ iterations, and each iteration takes $O(N^2)$ time to find an augmenting path and update potentials.
- **Space Complexity:** $O(N)$ auxiliary space to store potentials, matching states, and slack arrays.