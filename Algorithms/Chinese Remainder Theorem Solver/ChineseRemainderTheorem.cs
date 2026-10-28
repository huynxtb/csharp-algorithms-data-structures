using System;

namespace Algorithms
{
    /// <summary>
    /// Implements the Chinese Remainder Theorem solver.
    /// </summary>
    public static class ChineseRemainderTheorem
    {
        /// <summary>
        /// Solves a system of simultaneous congruences using the Chinese Remainder Theorem.
        /// </summary>
        /// <param name="remainders">An array of remainders (r_i).</param>
        /// <param name="moduli">An array of pairwise coprime moduli (m_i).</param>
        /// <returns>The unique solution x such that 0 <= x < M, where M is the product of all moduli.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the input arrays have different lengths, are null, or are empty.
        /// Thrown if the moduli are not pairwise coprime.
        /// </exception>
        public static long Solve(long[] remainders, long[] moduli)
        {
            if (remainders == null || moduli == null)
            {
                throw new ArgumentException("Input arrays cannot be null.");
            }
            if (remainders.Length == 0 || moduli.Length == 0)
            {
                throw new ArgumentException("Input arrays cannot be empty.");
            }
            if (remainders.Length != moduli.Length)
            {
                throw new ArgumentException("Remainders and moduli arrays must have the same length.");
            }

            if (!ArePairwiseCoprime(moduli))
            {
                throw new ArgumentException("Moduli must be pairwise coprime.");
            }

            long M = 1;
            foreach (long m in moduli)
            {
                M *= m;
            }

            long solution = 0;
            for (int i = 0; i < moduli.Length; i++)
            {
                long mi = moduli[i];
                long ri = remainders[i];
                long Mi = M / mi;
                long yi = ModInverse(Mi, mi);

                // Ensure intermediate calculations handle negative numbers correctly
                long term = (ri * Mi) % M;
                term = (term * yi) % M;
                solution = (solution + term) % M;
            }

            // Ensure the final result is non-negative
            return (solution + M) % M;
        }

        /// <summary>
        /// Checks if all values in the array are pairwise coprime.
        /// </summary>
        /// <param name="moduli">An array of integers.</param>
        /// <returns>True if all pairs have a GCD of 1, false otherwise.</returns>
        public static bool ArePairwiseCoprime(long[] moduli)
        {
            if (moduli == null || moduli.Length < 2)
            {
                return true; // Vacuously true for 0 or 1 element
            }

            for (int i = 0; i < moduli.Length; i++)
            {
                for (int j = i + 1; j < moduli.Length; j++)
                {
                    if (ExtendedGcd(moduli[i], moduli[j], out _, out _) != 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Computes the Extended Euclidean Algorithm.
        /// </summary>
        /// <param name="a">The first integer.</param>
        /// <param name="b">The second integer.</param>
        /// <param name="x">Output parameter for the coefficient of a.</param>
        /// <param name="y">Output parameter for the coefficient of b.</param>
        /// <returns>The greatest common divisor (GCD) of a and b.</returns>
        public static long ExtendedGcd(long a, long b, out long x, out long y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            long x1, y1;
            long gcd = ExtendedGcd(b % a, a, out x1, out y1);

            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }

        /// <summary>
        /// Computes the modular multiplicative inverse of 'a' modulo 'm'.
        /// </summary>
        /// <param name="a">The integer for which to find the inverse.</param>
        /// <param name="m">The modulus.</param>
        /// <returns>The modular multiplicative inverse of a modulo m.</returns>
        /// <exception cref="ArgumentException">Thrown if the inverse does not exist (i.e., GCD(a, m) != 1).</exception>
        public static long ModInverse(long a, long m)
        {
            long x, y;
            long gcd = ExtendedGcd(a, m, out x, out y);

            if (gcd != 1)
            {
                throw new ArgumentException($"Modular inverse does not exist for {a} mod {m} because GCD is {gcd}.");
            }

            // Ensure the result is non-negative
            return (x % m + m) % m;
        }
    }
}