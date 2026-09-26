# Bitap Algorithm (Shift-Or / Wu-Manber)

## 1. Introduction
The **Bitap algorithm** (also known as the Shift-Or or Shift-And algorithm) is a fast, bit-parallel string matching algorithm developed by Ricardo Baeza-Yates and Gaston Gonnet. It processes string matching using bitwise operations, testing pattern prefixes in parallel.

Its primary advantage lies in its extension to **approximate (fuzzy) string matching**, formulated by Sun Wu and Udi Manber. The Wu-Manber variant can search for patterns within a bounded Levenshtein edit distance (substitutions, insertions, deletions) with minimal computational overhead for short-to-medium length patterns (up to 63 characters with standard 64-bit word sizes).

### When to Use It
- Short pattern searching (e.g., DNA sequence motifs, user search queries, keywords).
- Approximate searching where typos, insertions, or deletions are expected.
- Low-latency lookup scenarios avoiding heavy dynamic programming allocations.

---

## 2. Usage

```csharp
using System;
using System.Collections.Generic;
using Algorithms;

class Program
{
    static void Main()
    {
        string text = "the quick brown fox jumps over the lazy dog";
        string pattern = "brown";

        // 1. Exact Search
        int exactIndex = BitapSearch.ExactSearch(text, pattern);
        Console.WriteLine($"Exact match start: {exactIndex}"); // Output: 10

        // 2. All Exact Matches
        List<int> allMatches = BitapSearch.ExactSearchAll("aba ba bba", "ba");
        Console.WriteLine($"All exact matches at: {string.Join(", ", allMatches)}"); // Output: 1, 4, 8

        // 3. Fuzzy Search (Within max edit distance)
        string fuzzyPattern = "brawn"; // 1 substitution away from 'brown'
        int fuzzyEndIndex = BitapSearch.FuzzySearch(text, fuzzyPattern, 1);
        Console.WriteLine($"Fuzzy match end index: {fuzzyEndIndex}"); // Output: 14

        // 4. All Fuzzy Matches
        List<int> allFuzzy = BitapSearch.FuzzySearchAll("cat cot cut act", "cat", 1);
        Console.WriteLine($"Fuzzy match ends at: {string.Join(", ", allFuzzy)}");
    }
}
```

---

## 3. Detailed Explanation

### Shift-Or Representation
The algorithm uses inverted bit representations (`0` indicates match, `1` indicates mismatch) to simplify updates using bitwise OR operations:

1. **Preprocessing (`BuildPatternMask`):**
   - A bitmask is generated for each unique character in the pattern.
   - Bit `i` is set to `0` if `pattern[i] == c`, otherwise `1`.

2. **Exact Matching:**
   - A bit-vector `state` is maintained, initialized with `~1L`.
   - For every character in the text: `state = (state << 1) | patternMask[c]`.
   - If the $(m-1)$-th bit of `state` becomes `0`, a full match has been identified.

3. **Fuzzy Matching (Wu-Manber Approach):**
   - An array of $k + 1$ state vectors `r[0..k]` tracks potential matches with up to $k$ errors.
   - Each step transitions the states using combinations of:
     - **Exact advance:** `(r[d] << 1) | mask`
     - **Substitution:** `(r[d - 1] << 1) | mask`
     - **Insertion:** `r[d - 1] | mask`
     - **Deletion:** `(r[d] << 1) | mask`
   - Combining these operations via bitwise AND evaluates all transitions in $O(k)$ operations per text character.

---

## 4. Complexity Analysis

| Operation | Time Complexity | Space Complexity |
|---|---|---|
| Pattern Preprocessing | $O(m + \Sigma)$ | $O(\min(m, \Sigma))$ |
| Exact Search | $O(n)$ | $O(1)$ auxiliary |
| Exact Search All | $O(n)$ | $O(1)$ auxiliary (excluding result list) |
| Fuzzy Search ($k$ errors) | $O(k \cdot n)$ | $O(k)$ auxiliary |
| Fuzzy Search All | $O(k \cdot n)$ | $O(k)$ auxiliary |

*Where $n$ is the length of `text`, $m$ is the length of `pattern` ($m \le 63$), $k$ is `maxEditDistance`, and $\Sigma$ is the pattern's alphabet size.*