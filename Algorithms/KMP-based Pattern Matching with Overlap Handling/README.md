# KMP-based Pattern Matching with Overlap Handling

## Introduction
The KMP-based Pattern Matching algorithm is an efficient method for finding occurrences of a substring (pattern) within a larger string (text). This implementation is designed to handle overlapping occurrences of the pattern, making it suitable for scenarios where such overlaps are significant, such as DNA sequence analysis or text processing.

## Usage
To use the KMPPatternMatcher class, create an instance and call the `FindOccurrences` method with the text and pattern as arguments:
```csharp
KMPPatternMatcher matcher = new KMPPatternMatcher();
List<int> occurrences = matcher.FindOccurrences("ABABABAB", "ABAB");
// occurrences will contain the starting indices of all matches
```

## Detailed Explanation
The algorithm works by preprocessing the pattern to create an LPS (Longest Prefix Suffix) array, which is used to skip unnecessary comparisons in the text. The main loop iterates through the text, comparing characters with the pattern. When a match is found, the index of the match is recorded, and the algorithm continues to check for further matches, including overlaps, by using the LPS array to determine the next position in the pattern to compare.

## Complexity Analysis
- **Time Complexity:** O(n + m), where n is the length of the text and m is the length of the pattern. This efficiency is achieved by avoiding redundant comparisons through the use of the LPS array.
- **Space Complexity:** O(m) for storing the LPS array, which is proportional to the length of the pattern.