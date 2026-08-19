# Miller-Rabin Primality Test

### 1. Introduction
The Miller-Rabin primality test is a probabilistic algorithm used to determine whether a given number is prime. It is highly efficient for testing very large integers, making it a cornerstone of modern cryptography (e.g., RSA key generation). Unlike deterministic tests, it has a small probability of declaring a composite number as prime (a pseudoprime), which decreases exponentially with the number of test rounds (witnesses).

### 2. Usage
```csharp
using System;
using System.Numerics;
using Algorithms;

class Program
{
    static void Main()
    {
        BigInteger largeCandidate = BigInteger.Parse("27338955874852185169691013678977");
        int iterations = 10; // Probability of false positive is less than 4^(-10)

        bool isPrime = MillerRabinTester.IsPrime(largeCandidate, iterations);
        Console.WriteLine($"Is Prime: {isPrime}");
    }
}
```

### 3. Detailed Explanation
The algorithm operates on the properties of modular arithmetic:
1. **Decomposition**: It factors $n - 1$ into the form $s \cdot 2^d$, where $s$ is an odd integer.
2. **Witness Loop**: For each iteration, a random base $a$ is selected in the range $[2, n - 2]$.
3. **Modular Exponentiation**: It computes $x = a^s \pmod n$. If $x = 1$ or $x = n - 1$, the candidate passes this round.
4. **Squaring Loop**: If not, it repeatedly squares $x$ modulo $n$ up to $d - 1$ times. If $x$ becomes $n - 1$ during this sequence, the candidate passes. If $x$ becomes $1$ without having reached $n - 1$, or if the loop finishes without $x$ reaching $n - 1$, the number is definitely composite.
5. **Probabilistic Guarantee**: If the candidate passes all rounds, it is prime with a probability of at least $1 - 4^{-k}$, where $k$ is the number of witnesses.

### 4. Complexity Analysis
- **Time Complexity**: $\mathcal{O}(k \log^3 n)$ where $n$ is the value being tested and $k$ is the number of witnesses. This is driven by the modular exponentiation operations.
- **Space Complexity**: $\mathcal{O}(\log n)$ auxiliary space to store the bit representations of the large integers during calculation.