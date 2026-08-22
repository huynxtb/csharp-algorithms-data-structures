# Gaussian Elimination with Partial Pivoting

### 1. Introduction
Gaussian Elimination is a fundamental numerical algorithm used to solve systems of linear equations of the form $Ax = B$. Partial pivoting is a technique employed during elimination to enhance numerical stability. By swapping the current row with the row containing the largest absolute value in the active column, the algorithm minimizes rounding errors caused by dividing by small numbers and avoids division by zero.

### 2. Usage
```csharp
using System;
using Algorithms.Numerical;

class Program
{
    static void Main()
    {
        // Solve the system:
        // 2x + 1y - 1z = 8
        // -3x - 1y + 2z = -11
        // -2x + 1y + 2z = -3
        
        double[,] coefficients = {
            { 2, 1, -1 },
            { -3, -1, 2 },
            { -2, 1, 2 }
        };
        double[] constants = { 8, -11, -3 };

        try
        {
            double[] solution = GaussianEliminationSolver.Solve(coefficients, constants);
            Console.WriteLine($"x = {solution[0]}, y = {solution[1]}, z = {solution[2]}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

### 3. Detailed Explanation
1. **Augmentation**: The coefficient matrix $A$ and the constant vector $B$ are combined into an augmented matrix $[A | B]$ of size $N \times (N + 1)$.
2. **Partial Pivoting**: For each column $i$, the algorithm scans rows from $i$ to $N-1$ to find the element with the maximum absolute value. The row containing this maximum element is swapped with row $i$. This ensures the divisor (pivot) is as large as possible.
3. **Singularity Check**: If the maximum absolute value in the pivot column is below a threshold (e.g., $10^{-9}$), the matrix is singular or near-singular, and the algorithm throws an exception.
4. **Forward Elimination**: Row operations are performed to eliminate elements below the diagonal, transforming the augmented matrix into an upper triangular form.
5. **Back Substitution**: Starting from the last row, the algorithm solves for each variable $x_i$ using the previously computed values of $x_{i+1}$ to $x_{N-1}$.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Forward Elimination**: $O(N^3)$ operations due to nested loops iterating over rows and columns.
  - **Back Substitution**: $O(N^2)$ operations.
  - **Total Time Complexity**: $O(N^3)$, where $N$ is the number of equations/variables.
- **Space Complexity**: $O(N^2)$ to store the augmented matrix.