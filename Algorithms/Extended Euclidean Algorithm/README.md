# Extended Euclidean Algorithm

### Introduction
The Extended Euclidean Algorithm is an extension of the classical Euclidean algorithm. In addition to computing the greatest common divisor (GCD) of two integers $a$ and $b$, it also computes the Bézout coefficients $x$ and $y$ such that:

$$ax + by = \gcd(a, b)$$

This algorithm is widely used in cryptography (e.g., finding modular multiplicative inverses in RSA), solving Diophantine equations, and computing modular arithmetic operations.

### Usage

```csharp
using System;

public class Program
{
    public static void Main()
    {
        long a = 240;
        long b = 46;

        var result = ExtendedEuclideanSolver.Solve(a, b);

        Console.WriteLine($"GCD: {result.Gcd}"); // Output: 2
        Console.WriteLine($"x: {result.X}");     // Output: -9
        Console.WriteLine($"y: {result.Y}");     // Output: 47
        // Verification: 240 * (-9) + 46 * 47 = -2160 + 2162 = 2
    }
}
```

### Detailed Explanation
The algorithm iteratively updates the quotients and remainders while maintaining the linear combinations of $a$ and $b$ that yield the current remainders. 

1. Initialize coefficients $x_0 = 1, y_0 = 0$ and $x_1 = 0, y_1 = 1$.
2. In each step, compute the quotient $q = a / b$ and remainder $r = a \pmod b$.
3. Update the coefficients: $nextX = x_0 - q \cdot x_1$ and $nextY = y_0 - q \cdot y_1$.
4. Shift variables: $a$ becomes $b$, $b$ becomes $r$, and the coefficients shift accordingly.
5. Repeat until the remainder becomes $0$. The last non-zero remainder is the GCD.
6. If the computed GCD is negative, negate both the GCD and the coefficients to ensure a positive GCD.

### Complexity Analysis
- **Time Complexity:** $O(\log(\min(|a|, |b|)))$ because the remainder decreases by at least a factor of 2 every two steps.
- **Space Complexity:** $O(1)$ auxiliary space as the implementation is iterative and uses a constant number of variables.