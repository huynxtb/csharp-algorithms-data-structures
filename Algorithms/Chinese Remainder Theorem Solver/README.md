# Chinese Remainder Theorem Solver

## Introduction

The Chinese Remainder Theorem (CRT) is a powerful mathematical theorem that provides a way to solve systems of simultaneous congruences. It states that if you have a set of congruences of the form `x ≡ r_i (mod m_i)`, where all the moduli `m_i` are pairwise coprime (their greatest common divisor is 1 for any pair), then there exists a unique solution for `x` modulo the product of all `m_i`.

This implementation provides a static class `ChineseRemainderTheorem` that can solve such systems, check for pairwise coprimality of moduli, and compute modular inverses using the Extended Euclidean Algorithm.

## Usage

```csharp
using System;
using Algorithms;

public class Example
{
    public static void Main(string[] args)
    {
        // Example system of congruences:
        // x ≡ 2 (mod 3)
        // x ≡ 3 (mod 5)
        // x ≡ 2 (mod 7)

        long[] remainders = { 2, 3, 2 };
        long[] moduli = { 3, 5, 7 };

        try
        {
            long solution = ChineseRemainderTheorem.Solve(remainders, moduli);
            Console.WriteLine($"The unique solution is x ≡ {solution} (mod {3 * 5 * 7})"); // Output: The unique solution is x ≡ 23 (mod 105)
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Example of checking pairwise coprimality
        long[] coprimeModuli = { 3, 5, 7 };
        long[] nonCoprimeModuli = { 3, 6, 7 };

        Console.WriteLine($"Are {{3, 5, 7}} pairwise coprime? {ChineseRemainderTheorem.ArePairwiseCoprime(coprimeModuli)}"); // Output: Are {3, 5, 7} pairwise coprime? True
        Console.WriteLine($"Are {{3, 6, 7}} pairwise coprime? {ChineseRemainderTheorem.ArePairwiseCoprime(nonCoprimeModuli)}"); // Output: Are {3, 6, 7} pairwise coprime? False

        // Example of modular inverse
        try
        {
            long inverse = ChineseRemainderTheorem.ModInverse(3, 11);
            Console.WriteLine($"Modular inverse of 3 mod 11 is: {inverse}"); // Output: Modular inverse of 3 mod 11 is: 4 (since 3 * 4 = 12 ≡ 1 mod 11)
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

## Detailed Explanation

### `Solve(long[] remainders, long[] moduli)`

This is the main method for solving the system of congruences.

1.  **Input Validation**: It first checks if the input arrays are null, empty, or have different lengths. It also calls `ArePairwiseCoprime` to ensure all moduli are pairwise coprime, throwing an `ArgumentException` if any of these conditions are not met.
2.  **Calculate Product of Moduli (M)**: It computes the product `M` of all moduli `m_i`. This `M` will be the modulus of the final unique solution.
3.  **Iterate Through Congruences**: For each congruence `x ≡ r_i (mod m_i)`:
    *   It calculates `Mi = M / m_i`. This is the product of all moduli *except* `m_i`.
    *   It computes `yi = ModInverse(Mi, m_i)`. This is the modular multiplicative inverse of `Mi` modulo `m_i`. This inverse exists because `m_i` and `Mi` are coprime (since all `m_j` for `j != i` are coprime to `m_i`).
    *   It calculates the contribution of this congruence to the total solution: `(r_i * Mi * yi) % M`. This term ensures that when the final sum is taken modulo `M`, this specific congruence `x ≡ r_i (mod m_i)` is satisfied.
4.  **Sum Contributions**: The contributions from each congruence are summed up. The result is taken modulo `M` to ensure it falls within the range `[0, M-1]`.
5.  **Non-negative Result**: Finally, `(solution + M) % M` is returned to guarantee a non-negative result.

### `ArePairwiseCoprime(long[] moduli)`

This method iterates through all unique pairs of moduli in the input array and uses `ExtendedGcd` to find their greatest common divisor. If the GCD of any pair is not 1, it returns `false`. If all pairs have a GCD of 1, it returns `true`.

### `ExtendedGcd(long a, long b, out long x, out long y)`

This is a standard recursive implementation of the Extended Euclidean Algorithm. It computes `gcd(a, b)` and also finds integers `x` and `y` such that `a*x + b*y = gcd(a, b)`. This is crucial for finding modular inverses.

### `ModInverse(long a, long m)`

This method uses `ExtendedGcd` to find the modular multiplicative inverse of `a` modulo `m`. If `gcd(a, m)` is not 1, the inverse does not exist, and an `ArgumentException` is thrown. Otherwise, the `x` value returned by `ExtendedGcd` (such that `a*x + m*y = 1`) is the modular inverse. The result is adjusted to be in the range `[0, m-1]`.

## Complexity Analysis

Let `k` be the number of congruences (i.e., the length of the `remainders` and `moduli` arrays).
Let `m_max` be the maximum value in the `moduli` array.
Let `M` be the product of all moduli.

*   **`ExtendedGcd(a, b)`**: The time complexity is `O(log(min(a, b)))`. In our context, for `ModInverse(Mi, mi)`, it's `O(log(mi))`. For `ArePairwiseCoprime`, it's `O(log(m_max))` for each pair.

*   **`ArePairwiseCoprime(moduli)`**: There are `k * (k - 1) / 2` pairs of moduli. For each pair, `ExtendedGcd` takes `O(log(m_max))`. So, the total time complexity is `O(k^2 * log(m_max))`.

*   **`ModInverse(a, m)`**: This is dominated by `ExtendedGcd`, so it's `O(log(m))`.

*   **`Solve(remainders, moduli)`**: 
    *   Calculating `M`: `O(k)` multiplications. The size of `M` can grow very large.
    *   The loop runs `k` times.
    *   Inside the loop:
        *   `M / mi`: `O(1)` (assuming fixed-size arithmetic, but practically depends on the size of `M`).
        *   `ModInverse(Mi, mi)`: `O(log(mi))`.
        *   Modular multiplications: `O(1)` (again, assuming fixed-size arithmetic).
    *   The dominant factor is the loop. The overall time complexity is roughly `O(k * log(m_max))` plus the cost of `ArePairwiseCoprime` if it's called within `Solve`. If `ArePairwiseCoprime` is called separately, `Solve` itself is `O(k * log(m_max))`. However, the arithmetic operations on potentially large numbers (up to `M`) can increase the complexity if arbitrary-precision arithmetic is considered. For standard `long` types, we assume `O(1)` for these operations.

**Space Complexity**: 
*   `ExtendedGcd`: `O(log(min(a, b)))` due to recursion stack depth.
*   `ArePairwiseCoprime`: `O(1)` (excluding input array).
*   `ModInverse`: `O(log(m))` (due to `ExtendedGcd`).
*   `Solve`: `O(1)` (excluding input arrays). The intermediate variables are constant in number.

**Note**: The size of `M` can exceed the capacity of `long` if `k` is large or the moduli are large. For such cases, a BigInteger implementation would be necessary.