# Pollard's Rho Algorithm for Integer Factorization

## Introduction

Pollard's Rho is a probabilistic algorithm for finding non-trivial factors of composite integers. It is significantly faster than trial division for large numbers containing moderately-sized prime factors. The algorithm combines Floyd's cycle-detection technique (also known as the "tortoise and hare" method) with a pseudo-random sequence generator to efficiently discover factors without exhaustively testing divisibility.

Pollard's Rho is particularly useful when:
- Factoring large composite numbers quickly
- Numbers have small to medium-sized prime factors
- Trial division or simple primality testing would be too slow

## Usage

```csharp
// Find a single factor of a composite number
long n = 91;  // 7 * 13
long factor = PollardRhoFactorization.FindFactor(n);
Console.WriteLine($"Factor of {n}: {factor}");  // Output: 7 or 13

// Get complete prime factorization
long composite = 315;  // 3 * 3 * 5 * 7
List<long> primeFactors = PollardRhoFactorization.Factorize(composite);
Console.WriteLine(string.Join(", ", primeFactors));  // Output: 3, 3, 5, 7

// Check if a number is prime
bool isPrime = PollardRhoFactorization.IsProbablePrime(97);
Console.WriteLine($"Is 97 prime? {isPrime}");  // Output: True

// Safely compute modular multiplication
long a = 9223372036854775800L;  // Near long.MaxValue
long b = 9223372036854775800L;
long mod = 1000000007;
long result = PollardRhoFactorization.MulMod(a, b, mod);
Console.WriteLine($"({a} * {b}) % {mod} = {result}");
```

## Detailed Explanation

### Core Algorithm

Pollard's Rho works by constructing a pseudo-random sequence using the recurrence relation:
```
x_{i+1} = g(x_i) = (x_i^2 + c) mod n
```

Since this sequence is modulo n and takes values in a finite set, it must eventually repeat (cycle). By detecting this cycle using Floyd's algorithm, we can identify a non-trivial factor:

1. **Floyd's Cycle Detection**: Maintain two pointers (tortoise at x, hare at y):
   - Tortoise moves: x → g(x)
   - Hare moves: y → g(g(y))
   - When a cycle occurs, x and y will eventually meet at some point

2. **Factor Detection**: Periodically compute d = gcd(|x - y|, n). When d > 1 and d < n, we have found a non-trivial factor.

3. **Retry on Failure**: If gcd returns n (meaning the cycle is on a divisor), increment c and restart with a new pseudo-random sequence.

### Key Implementation Details

**MulMod for Overflow Safety**: Since multiplying two `long` values near 2^63 - 1 can overflow, this implementation uses `BigInteger` to safely compute (a * b) % mod without precision loss.

**IsProbablePrime**: A deterministic Miller-Rabin test using a fixed set of small prime bases {2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37}. This is deterministic for all n < 3,317,044,064,679,887,385,961,981.

**Factorize**: Recursively applies FindFactor to extract all prime factors with their multiplicities, using the primality test to identify when a factor is prime.

### Algorithm Flow

1. Check if n is even, prime, or ≤ 3
2. Initialize c = 1, x = 2, y = 2, d = 1
3. Loop until d > 1:
   - Advance tortoise: x = g(x) = (x² + c) mod n
   - Advance hare twice: y = g(g(y))
   - Compute d = gcd(|x - y|, n)
4. If d < n, return d as the factor
5. If d = n, increment c and retry

## Complexity Analysis

### Time Complexity

- **FindFactor**: O(n^(1/4) log n) expected time on average. The algorithm typically finds a factor in O(√p) iterations where p is the smallest prime factor of n. With gcd and modular operations costing O(log n), the total is roughly O(n^(1/4) log n).
- **Factorize**: O(n^(1/4) log² n) for complete factorization of n.
- **IsProbablePrime**: O(log n) bit operations using 12 fixed bases.
- **MulMod**: O(log n) using binary multiplication or O(1) amortized with BigInteger.
- **ModPow**: O(log exp · log n) using binary exponentiation.

### Space Complexity

- **FindFactor**: O(log n) for storing intermediate values and gcd computation.
- **Factorize**: O(ω(n) log n) where ω(n) is the number of distinct prime factors, due to the factors list.
- **IsProbablePrime**: O(log n) for temporary variables in exponentiation.

### Practical Performance

For a 60-bit composite number with moderately sized factors, Pollard's Rho typically finds a factor in microseconds to milliseconds. For comparison, trial division would take exponentially longer. The algorithm is particularly efficient when the target composite has at least one prime factor ≤ 10^9.
