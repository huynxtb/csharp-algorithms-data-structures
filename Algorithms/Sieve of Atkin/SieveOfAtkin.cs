using System;
using System.Collections.Generic;
using System.Math;

public static class SieveOfAtkin
{
    public static List<int> GeneratePrimes(int limit)
    {
        if (limit < 2) return new List<int>();

        bool[] isPrime = new bool[limit + 1];
        for (int i = 0; i <= limit; i++) isPrime[i] = false;

        // Mark 2 and 3 as prime
        isPrime[2] = true;
        isPrime[3] = true;

        // Main algorithm
        for (int x = 1; x <= (int)Sqrt(limit); x++)
        {
            for (int y = 1; y <= (int)Sqrt(limit); y++)
            {
                int n = 4 * x * x + y * y;
                if (n <= limit && (n % 12 == 1 || n % 12 == 5))
                    isPrime[n] = !isPrime[n];

                n = 3 * x * x + y * y;
                if (n <= limit && n % 12 == 7)
                    isPrime[n] = !isPrime[n];

                n = 3 * x * x - y * y;
                if (x > y && n <= limit && n % 12 == 11)
                    isPrime[n] = !isPrime[n];
            }
        }

        // Mark all multiples of squares of primes as non-prime
        for (int r = 5; r <= (int)Sqrt(limit); r++)
        {
            if (isPrime[r])
            {
                for (int k = r * r; k <= limit; k += r * r)
                    isPrime[k] = false;
            }
        }

        // Collecting all prime numbers
        List<int> primes = new List<int>();
        for (int i = 2; i <= limit; i++)
        {
            if (isPrime[i])
                primes.Add(i);
        }

        return primes;
    }
}