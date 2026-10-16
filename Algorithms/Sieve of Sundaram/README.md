# Sieve of Sundaram

## Introduction
The Sieve of Sundaram is an efficient algorithm to generate all prime numbers up to a given integer n by eliminating non-prime numbers. It is particularly useful for generating a list of primes in a range and is an alternative to the more commonly known Sieve of Eratosthenes.

## Usage
Here is an example of how to use the Sieve of Sundaram algorithm:
```csharp
SieveOfSundaram sieve = new SieveOfSundaram();
List<int> primes = sieve.GeneratePrimes(30);
// primes now contains all prime numbers less than or equal to 30.
```

## Detailed Explanation
The algorithm works by first calculating a limit based on the input n. It then creates a boolean array to mark non-prime numbers. For each integer i from 1 to the limit, it marks numbers of the form i + j + 2ij as non-prime. Finally, it collects all numbers that are not marked in the boolean array, which correspond to the prime numbers.

## Complexity Analysis
- **Time Complexity:** O(n log n) in the worst case due to the nested loops for marking non-prime numbers.
- **Space Complexity:** O(n) for the boolean array used to mark non-prime numbers.