# Strassen's Matrix Multiplication

## 1. Introduction
Strassen's algorithm is a divide-and-conquer method for matrix multiplication. It reduces the number of recursive multiplications from 8 (in standard divide-and-conquer) to 7, yielding an asymptotic time complexity of $O(N^{\log_2 7}) \approx O(N^{2.807})$. It is highly effective for large matrices where the reduction in arithmetic operations outweighs the overhead of matrix additions and subtractions.

## 2. Usage
```csharp
using System;
using Algorithms.Matrix;

class Program
{
    static void Main()
    {
        double[,] A = {
            { 1.0, 2.0 },
            { 3.0, 4.0 }
        };
        double[,] B = {
            { 5.0, 6.0 },
            { 7.0, 8.0 }
        };

        double[,] result = StrassenMultiplier.Multiply(A, B);
    }
}
```

## 3. Detailed Explanation
The implementation performs the following steps:
1. **Validation**: Checks that the input matrices are non-empty and that the number of columns in `matrixA` matches the number of rows in `matrixB`.
2. **Padding**: Pads the input matrices with zeros to the next power of two ($N = 2^k$) based on the maximum dimension. This allows the algorithm to handle arbitrary rectangular and square matrices.
3. **Recursion & Fallback**: Recursively divides the matrices into four submatrices. If the submatrix size falls below the threshold (64), it switches to standard $O(N^3)$ matrix multiplication to avoid recursion overhead.
4. **Strassen Formulas**: Computes 7 intermediate matrix products ($M_1$ to $M_7$) using additions and subtractions, then combines them to form the submatrices of the result.
5. **Slicing**: Extracts the original dimensions ($rowsA \times colsB$) from the padded result matrix.

## 4. Complexity Analysis
- **Time Complexity**:
  - Strassen Step: $O(N^{\log_2 7}) \approx O(N^{2.807})$
  - Fallback Step: $O(N^3)$ for submatrices of size $\le 64$.
  - Overall: $O(N^{2.807})$ for large $N$.
- **Space Complexity**: $O(N^2)$ auxiliary space due to the allocation of submatrices at each recursion level.