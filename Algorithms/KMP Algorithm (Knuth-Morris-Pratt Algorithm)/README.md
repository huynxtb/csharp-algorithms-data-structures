# KMP Algorithm (Knuth-Morris-Pratt Algorithm)

## Introduction

The KMP Algorithm, named after Donald Knuth, James H. Morris, and Vaughan Pratt, is an efficient string searching algorithm that finds all occurrences of a pattern within a given text. Unlike the naive approach which has a worst-case time complexity of O(n × m), the KMP algorithm achieves linear time complexity of O(n + m) by intelligently using information about previously matched characters.

The key insight of the KMP algorithm is the preprocessing of the pattern to create an LPS (Longest Prefix Suffix) array. This array helps skip redundant comparisons by determining how many characters can be skipped when a mismatch occurs.

**When to use KMP:**
- Searching for patterns in large text documents
- DNA sequence matching
- Finding all occurrences of a substring
- Real-time text processing where performance is critical
- Situations where the pattern is fixed and reused multiple times

## Usage

```csharp
// Example 1: Basic pattern search
KMPAlgorithm kmp = new KMPAlgorithm();
string text = "ABAB DABA DAB ABAB CAB";
string pattern = "DAB";
List<int> matches = kmp.KMPSearch(text, pattern);

// Output: matches contains [6, 10, 14]
// Meaning "DAB" is found at indices 6, 10, and 14

// Example 2: Finding all occurrences of a pattern
string text2 = "aabaacaadaabaaba";
string pattern2 = "aab";
List<int> result = kmp.KMPSearch(text2, pattern2);

// Output: result contains [0, 9, 12]
// The pattern "aab" is found at indices 0, 9, and 12

// Example 3: Pattern not found
string text3 = "hello world";
string pattern3 = "xyz";
List<int> noMatches = kmp.KMPSearch(text3, pattern3);

// Output: noMatches is an empty list (pattern not found)

// Example 4: Computing LPS array directly
int[] lps = kmp.ComputeLPS("ABABC");
// Output: lps = [0, 0, 1, 2, 0]
```

## Detailed Explanation

### How the KMP Algorithm Works

1. **LPS Array Construction:**
   - The LPS (Longest Prefix Suffix) array is the foundation of the KMP algorithm
   - For each position i in the pattern, LPS[i] stores the length of the longest proper prefix which is also a suffix for pattern[0..i]
   - A proper prefix/suffix means it cannot be the entire string
   - Example: For pattern "ABABC":
     - LPS[0] = 0 (no prefix/suffix for single character)
     - LPS[1] = 0 ("AB" has no matching prefix and suffix)
     - LPS[2] = 1 ("ABA" has "A" as both prefix and suffix)
     - LPS[3] = 2 ("ABAB" has "AB" as both prefix and suffix)
     - LPS[4] = 0 ("ABABC" has no matching prefix and suffix)

2. **Pattern Matching:**
   - Two pointers are maintained: i for the text and j for the pattern
   - Characters are compared starting from position 0 in both text and pattern
   - When characters match, both pointers advance
   - When a mismatch occurs:
     - If j > 0, use LPS[j-1] to determine how many characters can be skipped
     - If j = 0, just advance the text pointer
   - When the entire pattern is matched, the starting index is recorded
   - The search continues by using LPS to find overlapping matches

3. **Key Optimization:**
   - The LPS array allows us to avoid re-examining characters that we know will match
   - This reduces redundant comparisons and achieves linear time complexity

### Example Walkthrough

Searching for "DAB" in "ABABDABA":

1. Compute LPS for "DAB": [0, 0, 0]
2. Start matching:
   - "A" vs "D": mismatch, j=0, advance text pointer
   - "B" vs "D": mismatch, j=0, advance text pointer
   - "A" vs "D": mismatch, j=0, advance text pointer
   - "B" vs "D": mismatch, j=0, advance text pointer
   - "D" vs "D": match, advance both
   - "A" vs "A": match, advance both
   - "B" vs "B": match, entire pattern found!
   - Record index 4 as a match

## Complexity Analysis

### Time Complexity

- **LPS Array Computation:** O(m) where m is the length of the pattern
  - Each character in the pattern is examined at most twice
  - Once in the outer loop and once through the LPS array references

- **Pattern Matching:** O(n) where n is the length of the text
  - The text pointer i only moves forward and never backtracks
  - The pattern pointer j moves forward and backward but the total movement is bounded by n

- **Overall Time Complexity:** O(n + m)
  - This is significantly better than the naive algorithm's O(n × m)
  - The improvement becomes more noticeable with longer patterns and texts

### Space Complexity

- **Space Complexity:** O(m) for the LPS array
  - The algorithm uses an additional array of size m to store the LPS values
  - The output list storing match positions can have at most n entries
  - Overall auxiliary space: O(m + k) where k is the number of matches

### Comparison with Other Algorithms

| Algorithm | Time Complexity | Space Complexity | Notes |
|-----------|-----------------|------------------|-------|
| Naive Search | O(n × m) | O(1) | Simple but inefficient |
| KMP | O(n + m) | O(m) | Efficient, preprocesses pattern |
| Boyer-Moore | O(n/m) to O(n × m) | O(k + m) | Often faster in practice |
| Rabin-Karp | O(n + m) avg, O(n × m) worst | O(1) | Good for multiple patterns |

The KMP algorithm is particularly valuable when the pattern is reused multiple times or when worst-case performance guarantees are important.