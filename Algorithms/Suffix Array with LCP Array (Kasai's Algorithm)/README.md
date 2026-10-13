# Suffix Array with LCP Array (Kasai's Algorithm)

## Introduction

A **Suffix Array** is a space-efficient data structure that stores the starting indices of all suffixes of a string in lexicographically sorted order. The **LCP (Longest Common Prefix) Array** complements the suffix array by storing the length of the longest common prefix between consecutive suffixes in the sorted order.

Together, these structures enable efficient solutions to many string problems:
- **Substring search**: Find all occurrences of a pattern in O(m log n) time (where m is pattern length, n is text length)
- **Counting distinct substrings**: Compute in O(n) time
- **Finding the longest repeated substring**: Identify in O(n) time
- **String matching and indexing**: Build compact full-text indices

This implementation uses:
- **Prefix-doubling algorithm** for suffix array construction in O(n log²n) time
- **Kasai's algorithm** for LCP array construction in O(n) time

## Usage

```csharp
using Algorithms.Strings;

// Create a suffix array instance
var sa = new SuffixArrayWithLCP("mississippi");

// Check if a pattern exists
bool found = sa.Contains("issi");  // true

// Count occurrences of a pattern
int count = sa.CountOccurrences("issi");  // 1

// Find all occurrences
var occurrences = sa.FindAllOccurrences("i");  // [1, 4, 7, 10]

// Find the longest repeated substring
string repeated = sa.LongestRepeatedSubstring();  // "issi"

// Count distinct substrings
long distinctCount = sa.CountDistinctSubstrings();  // 36

// Access the suffix array and LCP array directly
int[] suffixArray = sa.SuffixArray;
int[] lcpArray = sa.LcpArray;
```

## Detailed Explanation

### Suffix Array Construction (Prefix-Doubling)

1. **Initialization**: Each suffix is initially ranked by its first character (0-indexed position mapped to character value).

2. **Iterative Doubling**: For each iteration k (k = 1, 2, 4, 8, ...):
   - Each suffix is compared using two components: rank at position i and rank at position i+k
   - Radix sort is applied (stable sort by second component, then first component)
   - Ranks are updated based on the combined comparison
   - Process continues until all ranks are unique (n-1) or k exceeds n

3. **Output**: The suffix array `sa` where `sa[i]` is the starting index of the i-th lexicographically smallest suffix.

### LCP Array Construction (Kasai's Algorithm)

1. **Inverse Rank**: Build an inverse mapping from suffix indices to their positions in the sorted suffix array.

2. **Linear Scan**: For each position i in the original string:
   - Determine the rank of suffix starting at i in the sorted array
   - Compare suffix[i] with suffix at previous rank (suffix[rank-1])
   - Use the property that if lcp(suffix[i], suffix[i-1]) = h, then lcp(suffix[i+1], suffix[i]) >= h-1
   - This allows scanning linearly without nested loops

3. **Output**: The LCP array where `lcp[j]` is the longest common prefix length between suffixes at positions `sa[j-1]` and `sa[j]` (with `lcp[0] = 0`).

### String Operations

- **Contains**: Binary search on suffix array to find if pattern matches any suffix
- **CountOccurrences**: Binary search to find the range of suffixes starting with the pattern
- **FindAllOccurrences**: Return all indices in the range found by binary search
- **LongestRepeatedSubstring**: Find the maximum LCP value and extract the corresponding substring
- **CountDistinctSubstrings**: Use formula: total_substrings - sum_of_LCP_values = n(n+1)/2 - Σlcp[i]

## Complexity Analysis

### Time Complexity

- **Construction**: O(n log²n) for suffix array (prefix-doubling with radix sorts) + O(n) for LCP array (Kasai's algorithm) = **O(n log²n) overall**
- **Contains**: O(m log n) where m is pattern length (binary search with string comparisons)
- **CountOccurrences**: O(m log n) for binary search + O(log n) for counting range = O(m log n)
- **FindAllOccurrences**: O(m log n + k) where k is the number of occurrences
- **LongestRepeatedSubstring**: O(n) to scan the LCP array + O(n) to extract substring = O(n)
- **CountDistinctSubstrings**: O(n) to sum the LCP array

### Space Complexity

- **Storage**: O(n) for suffix array + O(n) for LCP array + O(n) for rank arrays = **O(n) overall**
- **No external space** is used beyond auxiliary rank arrays during construction

### Optimizations

- The prefix-doubling algorithm uses radix sort (counting sort) to avoid O(n log n) comparison-based sorting at each level
- Kasai's algorithm achieves O(n) by maintaining the "height" value h across iterations, avoiding redundant comparisons
- Binary search for pattern matching is logarithmic, making it practical for long texts

