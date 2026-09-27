# Sieve of Eratosthenes

## 1. Introduction
The **Sieve of Eratosthenes** is an ancient, highly efficient algorithm for generating all prime numbers up to a given limit $n$. Instead of checking each number individually for primality, it systematically eliminates multiples of known primes.

It is ideally used when:
- You need to compute all prime numbers in a range from $2$ to $n$.
- Multiple primality lookups are required up to a fixed bound $n$.
- High computational efficiency is required for ranges up to millions or tens of millions.

## 2. Usage

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        SieveOfEratosthenes sieve = new SieveOfEratosthenes();

        // Generate all primes up to 50
        List<int> primes = sieve.FindPrimes(50);
        Console.WriteLine("Primes up to 50: " + string.Join(", ", primes));

        // Check primality of individual numbers
        bool is29Prime = sieve.IsPrime(29);
        bool is30Prime = sieve.IsPrime(30);

        Console.WriteLine($"Is 29 prime? {is29Prime}"); // True
        Console.WriteLine($"Is 30 prime? {is30Prime}"); // False
    }
}
```

## 3. Detailed Explanation
1. **Initialization:** A boolean array `isComposite` of size $n + 1$ is allocated, where all entries default to `false` (meaning initially assumed prime).
2. **Elimination Phase:** Starting from the first prime $p = 2$, the algorithm checks if $p$ is marked. If not marked:
   - All multiples of $p$ starting from $p^2$ (i.e., $p \times p, p \times (p + 1), \dots$) are marked as composite (`true`). Smaller multiples such as $2p, 3p$ have already been marked by earlier primes.
   - The outer loop terminates when $p > \sqrt{n}$, as any composite number $\le n$ must have a prime factor $\le \sqrt{n}$.
3. **Result Collection:** All numbers from $2$ to $n$ with `isComposite[i] == false` are gathered into a `List<int>` in ascending order.
4. **Overflow Prevention:** Inner loop indices are computed using `long` to avoid 32-bit signed integer overflow when $p^2 > \text{int.MaxValue}$.

## 4. Complexity Analysis

- **Time Complexity:** 
  - `FindPrimes(int n)`: $\mathcal{O}(n \log \log n)$ - The harmonic series of primes leads to near-linear performance.
  - `IsPrime(int number)`: $\mathcal{O}(\sqrt{n})$ - Using optimized trial division ($6k \pm 1$).
- **Space Complexity:**
  - $\mathcal{O}(n)$ auxiliary space to maintain the boolean sieve array.