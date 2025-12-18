# Boyer-Moore String Search Algorithm

## 1. Introduction
The Boyer-Moore string search algorithm is an efficient pattern matching technique used to find occurrences of a "pattern" string within a larger "text" string. It is particularly effective for large texts and longer patterns. The algorithm preprocesses the pattern to create heuristics that allow it to skip sections of the text, reducing unnecessary character comparisons. Two main heuristics used are the bad character rule and the good suffix rule.

Use the Boyer-Moore algorithm when you need a fast and reliable way to find all occurrences of a substring within a large string. It is well-suited for text processing, searching in DNA sequences, and other applications involving string matching.

## 2. Usage

using System;
using System.Collections.Generic;

class Example
{
    public static void Main()
    {
        string text = "HERE IS A SIMPLE EXAMPLE";
        string pattern = "EXAMPLE";

        List<int> occurrences = BoyerMoore.Search(text, pattern);

        foreach (int index in occurrences)
        {
            Console.WriteLine($"Pattern found at index: {index}");
        }
    }
}

## 3. Detailed Explanation
The implementation is encapsulated in a static class `BoyerMoore` that provides a public method `Search`:

- **Preprocessing Steps:**
  * Bad Character Heuristic: Creates an array storing the last occurrence of each character in the pattern. This information helps decide how much to shift the pattern when a mismatch occurs.
  * Good Suffix Heuristic: Uses arrays `suffix` and `prefix` to determine how far to shift the pattern to align with previously matched suffixes when a mismatch occurs.

- **Search Algorithm:**
  1. The search starts from the beginning of the text and aligns the pattern.
  2. Compare characters in the pattern from right to left with the section of text under consideration.
  3. On a mismatch, calculate shifts based on both heuristics and move the pattern the maximum shift distance.
  4. On a match of the entire pattern, record the index and shift the pattern to the right by the pattern length to continue searching.

This combination of heuristics reduces redundant comparisons, often allowing shifts larger than one position, which significantly speeds up the search.

## 4. Complexity Analysis
- **Time Complexity**:
  - Worst Case: O(n * m) (rare due to heuristics)
  - Average Case: O(n / m) for typical inputs, where n is the length of the text and m is the length of the pattern.

- **Space Complexity**:
  - O(m + k), where m is the length of the pattern and k is the size of the character set (256 in this implementation).

This makes Boyer-Moore a highly efficient algorithm, especially for searching large texts with relatively short patterns.