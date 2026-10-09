# Rabin-Karp Multiple Pattern String Search Algorithm

## Introduction

The Rabin-Karp algorithm is a string searching algorithm that uses rolling hashes to find patterns within text. While it can search for a single pattern, this implementation is specifically optimized for **multiple pattern matching**, making it significantly more efficient than searching for each pattern individually.

Unlike naive algorithms that compare character-by-character, Rabin-Karp computes hash values for substrings and compares hashes first. This is particularly effective when:
- Searching for multiple patterns simultaneously in a single text
- Patterns are long and repeated comparisons are expensive
- You need to find all occurrences of several keywords in a document

The algorithm groups patterns by length and performs a single rolling-hash scan per pattern length, making it extremely efficient for multi-pattern scenarios.

## Usage

```csharp
// Example: Search for multiple patterns in text
var patterns = new[] { "abc", "def", "ghi" };
var searcher = new RabinKarpMultiPattern(patterns);

string text = "abcdefghiabcxyz";
var matches = searcher.Search(text);

foreach (var match in matches)
{
    Console.WriteLine($"Found '{match.Pattern}' at index {match.StartIndex}");
    // Output:
    // Found 'abc' at index 0
    // Found 'def' at index 3
    // Found 'ghi' at index 6
    // Found 'abc' at index 9
}

// Quick check if any pattern exists
bool hasMatch = searcher.ContainsAny(text); // true
bool hasMatch2 = searcher.ContainsAny("xyz"); // false

// Custom prime and base values for different use cases
var searcher2 = new RabinKarpMultiPattern(patterns, prime: 101, baseValue: 31);
```

## Detailed Explanation

### How It Works

1. **Pattern Preprocessing**: All patterns are deduplicated and grouped by length. For each group, a hash value is precomputed using the formula: `hash = (hash * base + char) % prime`.

2. **H-Value Computation**: For each distinct pattern length m, the algorithm precomputes `h = base^(m-1) % prime` using fast modular exponentiation. This value is used to efficiently remove the leftmost character's contribution in the rolling hash.

3. **Rolling Hash Mechanism**: The rolling hash formula removes the leftmost character and adds the rightmost character in O(1) time:
   ```
   newHash = ((oldHash - leftChar * h) * base + rightChar) % prime
   ```
   This allows scanning the entire text in linear time for all patterns of a given length.

4. **Hash Collision Handling**: When a hash matches, the algorithm performs character-by-character verification to confirm the actual match (eliminating false positives from hash collisions).

5. **Multi-Length Optimization**: Since patterns may have different lengths, the algorithm:
   - Groups patterns by length internally
   - Performs one rolling-hash scan per pattern length
   - Reports all matches sorted by start index and pattern name

### Example Walkthrough

Searching for patterns `["ab", "ba"]` in text `"ababa"`:

1. Both patterns have length 2, so they're grouped together.
2. Precompute `h = 256^1 % prime`.
3. Hash window `"ab"` at index 0: matches pattern `"ab"`.
4. Roll to index 1: hash window `"ba"`: matches pattern `"ba"`.
5. Roll to index 2: hash window `"ab"`: matches pattern `"ab"`.
6. Roll to index 3: hash window `"ba"`: matches pattern `"ba"`.
7. Results: `[(0, "ab"), (1, "ba"), (2, "ab"), (3, "ba")]`

## Complexity Analysis

### Time Complexity

- **Preprocessing (Constructor)**: O(P + K·m)
  - P = total characters across all patterns
  - K = number of distinct pattern lengths
  - m = average pattern length
  - Computing hashes for all patterns and h-values for each length

- **Search**: O(n·K + (n + m·c) where hash collision occurs)
  - n = length of text
  - K = number of distinct pattern lengths
  - m = average pattern length
  - c = number of hash collisions (typically very small)
  - Best/Average case: O(n·K) — one pass per pattern length
  - Worst case: O(n·m·K) — if many hash collisions occur

- **ContainsAny**: O(n·K) average case, stops on first match

### Space Complexity

- O(P + K)
  - P = total characters in all unique patterns (stored as deduplicated set)
  - K = number of distinct pattern lengths (for h-value storage)
  - Hash table for grouping patterns by length and storing their hashes

### Advantages Over Alternatives

| Method | Time | Space | Notes |
|--------|------|-------|-------|
| Naive (per-pattern) | O(n·m·P) | O(1) | Very slow for multiple patterns |
| Rabin-Karp Multi | O(n·K) avg | O(P + K) | Optimal for multiple patterns |
| Aho-Corasick | O(n + m + z) | O(m·σ) | Complex to implement; better for repeated use |
| Boyer-Moore-Multi | O(n/m) best | O(m) | Good for long patterns; less predictable |

Rabin-Karp multi-pattern is ideal when you need a simple, efficient solution for one-time or occasional multi-pattern searches.
