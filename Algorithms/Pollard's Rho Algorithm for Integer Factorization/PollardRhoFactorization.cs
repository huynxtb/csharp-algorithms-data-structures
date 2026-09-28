using System;
using System.Collections.Generic;
using System.Numerics;

/// <summary>
/// Pollard's Rho Algorithm for Integer Factorization.
/// A probabilistic algorithm for finding non-trivial factors of composite integers
/// using Floyd's cycle detection and a pseudo-random sequence generator.
/// </summary>
public static class PollardRhoFactorization
{
    /// <summary>
    /// Finds a single non-trivial factor of the composite input n using Pollard's Rho algorithm.
    /// </summary>
    /// <param name="n">The integer to factorize.</param>
    /// <returns>A non-trivial factor of n, or n itself if n is prime or 1.</returns>
    public static long FindFactor(long n)
    {
        if (n <= 1) return n;
        if (n == 2) return 2;
        if (n % 2 == 0) return 2;
        if (IsProbablePrime(n)) return n;

        long c = 1;
        while (true)
        {
            long x = 2;
            long y = 2;
            long d = 1;

            while (d == 1)
            {
                x = MulMod(x, x, n);
                x = (x + c) % n;
                if (x < 0) x += n;

                y = MulMod(y, y, n);
                y = (y + c) % n;
                if (y < 0) y += n;
                y = MulMod(y, y, n);
                y = (y + c) % n;
                if (y < 0) y += n;

                d = Gcd(Math.Abs(x - y), n);
            }

            if (d != n)
            {
                return d;
            }

            c++;
        }
    }

    /// <summary>
    /// Computes (a * b) % mod safely without overflow using BigInteger.
    /// </summary>
    /// <param name="a">First operand.</param>
    /// <param name="b">Second operand.</param>
    /// <param name="mod">Modulus.</param>
    /// <returns>The result of (a * b) % mod.</returns>
    public static long MulMod(long a, long b, long mod)
    {
        BigInteger result = (BigInteger)a * (BigInteger)b % (BigInteger)mod;
        return (long)result;
    }

    /// <summary>
    /// Computes the greatest common divisor of a and b using the Euclidean algorithm.
    /// </summary>
    /// <param name="a">First integer.</param>
    /// <param name="b">Second integer.</param>
    /// <returns>The GCD of a and b.</returns>
    private static long Gcd(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    /// <summary>
    /// Performs a deterministic Miller-Rabin primality test using small bases.
    /// </summary>
    /// <param name="n">The integer to test.</param>
    /// <returns>True if n is probably prime, false if n is composite.</returns>
    public static bool IsProbablePrime(long n)
    {
        if (n < 2) return false;
        if (n == 2 || n == 3) return true;
        if (n % 2 == 0) return false;

        long d = n - 1;
        int r = 0;
        while (d % 2 == 0)
        {
            d /= 2;
            r++;
        }

        long[] bases = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37 };
        foreach (long a in bases)
        {
            if (a >= n) continue;
            long x = ModPow(a, d, n);
            if (x == 1 || x == n - 1) continue;

            bool composite = true;
            for (int i = 0; i < r - 1; i++)
            {
                x = MulMod(x, x, n);
                if (x == n - 1)
                {
                    composite = false;
                    break;
                }
            }

            if (composite) return false;
        }

        return true;
    }

    /// <summary>
    /// Computes (base ^ exp) % mod using binary exponentiation.
    /// </summary>
    /// <param name="base">The base.</param>
    /// <param name="exp">The exponent.</param>
    /// <param name="mod">The modulus.</param>
    /// <returns>The result of (base ^ exp) % mod.</returns>
    private static long ModPow(long @base, long exp, long mod)
    {
        long result = 1;
        @base %= mod;
        if (@base < 0) @base += mod;

        while (exp > 0)
        {
            if ((exp & 1) == 1)
                result = MulMod(result, @base, mod);
            @base = MulMod(@base, @base, mod);
            exp >>= 1;
        }

        return result;
    }

    /// <summary>
    /// Returns the complete prime factorization of n with multiplicities.
    /// </summary>
    /// <param name="n">The integer to factorize.</param>
    /// <returns>A list of prime factors in non-decreasing order.</returns>
    public static List<long> Factorize(long n)
    {
        List<long> factors = new List<long>();
        if (n <= 1) return factors;

        while (n > 1)
        {
            if (IsProbablePrime(n))
            {
                factors.Add(n);
                break;
            }

            long factor = FindFactor(n);
            while (n % factor == 0)
            {
                factors.Add(factor);
                n /= factor;
            }
        }

        factors.Sort();
        return factors;
    }
}