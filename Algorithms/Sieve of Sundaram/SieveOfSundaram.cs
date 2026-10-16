using System;
using System.Collections.Generic;

public class SieveOfSundaram
{
    /// <summary>
    /// Generates all prime numbers less than or equal to the given integer n using the Sieve of Sundaram algorithm.
    /// </summary>
    /// <param name="n">The upper limit to generate prime numbers.</param>
    /// <returns>A list of prime numbers less than or equal to n.</returns>
    public List<int> GeneratePrimes(int n)
    {
        // If n is less than 2, return an empty list as there are no primes.
        if (n < 2) return new List<int>();

        // Calculate the limit for the Sieve of Sundaram.
        int limit = (n - 1) / 2;
        bool[] marked = new bool[limit + 1];

        // Mark non-prime numbers using the Sieve of Sundaram.
        for (int i = 1; i <= limit; i++)
        {
            for (int j = i; (i + j + 2 * i * j) <= limit; j++)
            {
                marked[i + j + 2 * i * j] = true;
            }
        }

        // Collecting the prime numbers from the marked array.
        List<int> primes = new List<int>();
        if (n >= 2) primes.Add(2); // 2 is a prime number.

        for (int i = 1; i <= limit; i++)
        {
            if (!marked[i])
            {
                primes.Add(2 * i + 1);
            }
        }

        return primes;
    }
}