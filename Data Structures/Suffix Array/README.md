# Suffix Array

## 1. Introduction
A **Suffix Array** is a powerful data structure used in string processing and algorithmic applications. It consists of an array of integers representing the starting indices of all suffixes of a string, sorted in lexicographical order. This allows efficient querying and solving of problems such as substring searches, pattern matching, longest common prefix computations, and more.

Suffix arrays are widely used in various fields including bioinformatics (e.g., genome analysis), text search engines, and data compression algorithms.

## 2. Usage
// Create a suffix array for the input string
var sa = new SuffixArray("banana");

// Retrieve the suffix array, which contains starting indices of suffixes in sorted order
int[] suffixIndices = sa.GetSuffixArray();

// Example output for "banana": [5, 3, 1, 0, 4, 2]
// Corresponding suffixes: "a", "ana", "anana", "banana", "na", "nana"

foreach (int index in suffixIndices)
{
    Console.WriteLine($"{index}: {"banana".Substring(index)}");
}

## 3. Detailed Explanation
The implementation builds the suffix array using a commonly used O(n log n) approach:

- Initialization: Each suffix is initially represented by its starting index and its rank is set according to the ASCII value of the first character.
- Sorting by 2^k length substrings: The suffixes are sorted based on the rank of their first 2^k characters. This is done repeatedly by doubling k each iteration:
  - The suffix array is sorted with a custom comparator that compares two suffixes primarily by their current rank and secondarily by the rank of the suffix starting 2^k characters ahead.
  - New ranks are assigned based on sorted order, ensuring equal ranks for suffixes that are identical up to the current length.
- Termination: The process continues until all suffixes have distinct ranks or we've considered substring sizes greater or equal to the string length.

This algorithm cleverly uses the fact that knowing the rank of substrings of length 2^k helps build the rank for substrings of length 2^{k+1}.

## 4. Complexity Analysis
- Time Complexity:
  - Building the suffix array runs in O(n log n) time where n is the length of the string. Each iteration doubles the considered substring length and sorting plus rank reassignment are O(n log n) in total.
- Space Complexity:
  - The implementation uses O(n) space for the suffix array and rank arrays.

This time complexity is efficient enough for typical use cases involving suffix arrays in string processing tasks.