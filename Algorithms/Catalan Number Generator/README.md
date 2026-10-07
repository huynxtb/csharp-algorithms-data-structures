# Catalan Number Generator (Dynamic Programming)

## Introduction

The **Catalan Number Generator** is a dynamic programming implementation for computing Catalan numbers efficiently. Catalan numbers are a fascinating sequence of natural numbers (1, 1, 2, 5, 14, 42, 132, ...) that arise in a wide variety of combinatorial problems, including:

- **Balanced Parentheses:** The number of ways to correctly match `n` pairs of parentheses.
- **Binary Trees:** The number of structurally unique binary search trees with `n` nodes.
- **Polygon Triangulation:** The number of ways to divide a convex polygon with `n+2` sides into triangles.
- **Lattice Paths:** The number of monotonic lattice paths along the edges of a grid that do not pass above the diagonal.
- **Stack-Sortable Permutations:** The number of permutations of `{1, ..., n}` that are stack-sortable.

Use this implementation whenever you need to compute one or more Catalan numbers efficiently without worrying about integer overflow for values of `n` up to at least 30 (and beyond, thanks to the `decimal` data type).

## Usage

The `CatalanNumberGenerator` is a static class with multiple methods for computing Catalan numbers:

```csharp
// Compute a single Catalan number using the DP summation approach
decimal c5 = CatalanNumberGenerator.GetCatalanNumber(5);
// c5 = 42

// Compute a single Catalan number using the iterative multiplicative formula
decimal c10 = CatalanNumberGenerator.GetCatalanNumberIterative(10);
// c10 = 16796

// Compute a single Catalan number using the binomial coefficient formula
decimal c7 = CatalanNumberGenerator.GetCatalanNumberBinomial(7);
// c7 = 429

// Generate a sequence of the first 10 Catalan numbers: C(0) through C(9)
decimal[] sequence = CatalanNumberGenerator.GenerateCatalanSequence(10);
// sequence = [1, 1, 2, 5, 14, 42, 132, 429, 1430, 4862]

// Edge cases are handled correctly
decimal c0 = CatalanNumberGenerator.GetCatalanNumber(0); // 1
decimal c1 = CatalanNumberGenerator.GetCatalanNumber(1); // 1

// Large values work thanks to decimal precision
decimal c30 = CatalanNumberGenerator.GetCatalanNumber(30);
// c30 = 5909761445129385600
```

## Detailed Explanation

This implementation provides three distinct approaches to computing Catalan numbers, all designed to avoid overflow issues:

### 1. Summation-Based DP (`GetCatalanNumber`)

This method uses the classic recurrence relation:

```
C(0) = 1
C(n) = Σ C(i) × C(n-1-i)  for i = 0 to n-1
```

An array `catalanTable` of size `n+1` is allocated. Starting from the base case `C(0) = 1`, each subsequent value `C(k)` is computed by summing the products of previously computed values. This bottom-up approach ensures that all required sub-problems are solved before they are needed.

### 2. Iterative Multiplicative Formula (`GetCatalanNumberIterative`)

This method uses the recurrence:

```
C(0) = 1
C(n) = C(n-1) × 2(2n-1) / (n+1)
```

This is the most space-efficient approach, using only a single variable to track the current Catalan number. It iteratively multiplies and divides to compute the result without storing intermediate values.

### 3. Binomial Coefficient Formula (`GetCatalanNumberBinomial`)

This method computes the Catalan number using:

```
C(n) = Binomial(2n, n) / (n+1)
```

The binomial coefficient `Binomial(2n, n)` is computed iteratively as `Product of (n+k)/k for k=1 to n`, which avoids computing large factorials directly and prevents overflow.

### 4. Sequence Generation (`GenerateCatalanSequence`)

This method generates an entire sequence of Catalan numbers from `C(0)` to `C(count-1)` using the summation-based DP approach. It is optimal when you need multiple consecutive Catalan numbers, as it reuses all previously computed values.

All methods use the `decimal` data type, which provides 28-29 significant decimal digits of precision, allowing correct computation of Catalan numbers for `n` values well beyond 30.

## Complexity Analysis

| Method | Time Complexity | Space Complexity |
|---|---|---|
| `GetCatalanNumber(n)` | O(n²) | O(n) |
| `GetCatalanNumberIterative(n)` | O(n) | O(1) |
| `GetCatalanNumberBinomial(n)` | O(n) | O(1) |
| `GenerateCatalanSequence(count)` | O(count²) | O(count) |

### Detailed Breakdown:

- **`GetCatalanNumber(n)`**: The nested loop structure (outer loop runs `n` times, inner loop runs up to `k` times) results in O(n²) time. The DP table requires O(n) space.
- **`GetCatalanNumberIterative(n)`**: A single loop of `n` iterations with constant work per iteration yields O(n) time and O(1) space. This is the most efficient for computing a single Catalan number.
- **`GetCatalanNumberBinomial(n)`**: Similarly O(n) time and O(1) space, computing the binomial coefficient iteratively.
- **`GenerateCatalanSequence(count)`**: Computes all values up to `C(count-1)` with O(count²) total time due to the summation recurrence, using O(count) space for the result array.

For computing a single Catalan number, `GetCatalanNumberIterative` or `GetCatalanNumberBinomial` are recommended due to their O(n) time and O(1) space. For generating a range of Catalan numbers, `GenerateCatalanSequence` is ideal as it avoids redundant computation.