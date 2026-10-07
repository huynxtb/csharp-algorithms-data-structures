using System;

/// <summary>
/// Provides methods to generate Catalan numbers using dynamic programming.
/// Catalan numbers are a sequence of natural numbers that appear in many
/// combinatorial problems, such as counting binary trees, valid parenthesizations,
/// and polygon triangulations.
/// </summary>
public static class CatalanNumberGenerator
{
    /// <summary>
    /// Computes the nth Catalan number using the summation-based dynamic programming approach.
    /// C(0) = 1
    /// C(n) = Sum of C(i) * C(n-1-i) for i from 0 to n-1
    /// Uses decimal to handle large values up to at least n = 30.
    /// </summary>
    /// <param name="n">The index of the Catalan number to compute (non-negative integer).</param>
    /// <returns>The nth Catalan number as a decimal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when n is negative.</exception>
    public static decimal GetCatalanNumber(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Input must be a non-negative integer.");
        }

        // Array to store intermediate Catalan numbers (dynamic programming table)
        decimal[] catalanTable = new decimal[n + 1];

        // Base case: C(0) = 1
        catalanTable[0] = 1;

        // Build up the table iteratively using the recurrence relation:
        // C(k) = Sum of C(i) * C(k-1-i) for i = 0 to k-1
        for (int k = 1; k <= n; k++)
        {
            catalanTable[k] = 0;

            for (int i = 0; i < k; i++)
            {
                catalanTable[k] += catalanTable[i] * catalanTable[k - 1 - i];
            }
        }

        return catalanTable[n];
    }

    /// <summary>
    /// Computes the nth Catalan number using the iterative multiplicative formula.
    /// C(n) = C(n-1) * 2(2n-1) / (n+1)
    /// This approach avoids computing large factorials directly and is more space-efficient.
    /// Uses decimal to handle large values up to at least n = 30.
    /// </summary>
    /// <param name="n">The index of the Catalan number to compute (non-negative integer).</param>
    /// <returns>The nth Catalan number as a decimal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when n is negative.</exception>
    public static decimal GetCatalanNumberIterative(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Input must be a non-negative integer.");
        }

        // Base case: C(0) = 1
        decimal catalanValue = 1;

        // Iteratively compute C(k) from C(k-1) using:
        // C(k) = C(k-1) * 2(2k - 1) / (k + 1)
        for (int k = 1; k <= n; k++)
        {
            catalanValue = catalanValue * 2 * (2 * k - 1) / (k + 1);
        }

        return catalanValue;
    }

    /// <summary>
    /// Computes the nth Catalan number using the binomial coefficient formula
    /// implemented via dynamic programming to avoid factorial overflow.
    /// C(n) = (2n)! / ((n+1)! * n!) = Binomial(2n, n) / (n+1)
    /// The binomial coefficient is computed iteratively to prevent overflow.
    /// </summary>
    /// <param name="n">The index of the Catalan number to compute (non-negative integer).</param>
    /// <returns>The nth Catalan number as a decimal.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when n is negative.</exception>
    public static decimal GetCatalanNumberBinomial(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Input must be a non-negative integer.");
        }

        // Compute Binomial(2n, n) iteratively to avoid factorial overflow
        // Binomial(2n, n) = Product of (n + k) / k for k = 1 to n
        decimal binomialCoefficient = 1;

        for (int k = 1; k <= n; k++)
        {
            binomialCoefficient = binomialCoefficient * (n + k) / k;
        }

        // C(n) = Binomial(2n, n) / (n + 1)
        decimal catalanValue = binomialCoefficient / (n + 1);

        return catalanValue;
    }

    /// <summary>
    /// Generates a sequence of Catalan numbers from C(0) to C(count - 1).
    /// Uses the summation-based DP approach for efficiency when generating multiple values.
    /// </summary>
    /// <param name="count">The number of Catalan numbers to generate (must be positive).</param>
    /// <returns>An array containing Catalan numbers C(0) through C(count - 1).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when count is not positive.</exception>
    public static decimal[] GenerateCatalanSequence(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be a positive integer.");
        }

        decimal[] catalanTable = new decimal[count];

        // Base case
        catalanTable[0] = 1;

        // Build up each subsequent Catalan number using the recurrence relation
        for (int k = 1; k < count; k++)
        {
            catalanTable[k] = 0;

            for (int i = 0; i < k; i++)
            {
                catalanTable[k] += catalanTable[i] * catalanTable[k - 1 - i];
            }
        }

        return catalanTable;
    }
}