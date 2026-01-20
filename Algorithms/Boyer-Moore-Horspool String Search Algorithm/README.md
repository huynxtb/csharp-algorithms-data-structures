# Boyer-Moore-Horspool String Search Algorithm

## 1. Introduction
The Boyer-Moore-Horspool (BMH) algorithm is an efficient substring search algorithm used to find all occurrences of a pattern string within a larger text. It is a simplified variant of the Boyer-Moore algorithm, optimized for practical use with good average-case performance. BMH focuses on a "bad character shift" heuristic allowing the search to skip sections of the text, rather than checking every character.

This algorithm is particularly useful when searching for patterns in large texts where performance matters and the pattern is fixed or reused multiple times.

## 2. Usage
var bmh = new BoyerMooreHorspool("needle");

string haystack = "finding a needle in a haystack needle";
foreach (int index in bmh.Search(haystack))
{
    Console.WriteLine($"Pattern found at index: {index}");
}

Note: The search is case-sensitive and performs exact matches without altering the text or pattern.

## 3. Detailed Explanation
The implementation consists of the following key parts:

- **Constructor and Preprocessing:**
  - The constructor takes the pattern and initializes a bad character shift table for all possible byte values (assuming extended ASCII).
  - The `PreprocessBadCharShift` method fills this table, setting default shifts to the pattern length, and then updating shift values based on the pattern's characters except for the last one.

- **Search Method:**
  - The `Search` method scans the text, starting at index 0.
  - At each alignment, it compares the pattern characters from right to left with the corresponding characters in the text.
  - If a mismatch occurs, it uses the bad character in the text at the pattern's last character position to determine how far to skip forward.
  - If the whole pattern matches, it yields the current index and shifts by 1 to continue searching for subsequent occurrences.

This algorithm is efficient because it skips sections of the text, potentially jumping ahead more than one character, unlike naive search methods that check every position.

## 4. Complexity Analysis
- **Time Complexity:**
  - Average case: O(n) where n is the length of the text.
  - Worst case: O(n * m) where m is the length of the pattern (rare in typical usage due to the heuristic skipping).

- **Space Complexity:**
  - O(m) for storing the pattern.
  - O(1) additional space for the bad character shift table of fixed size (256 entries).

This implementation is optimized for readability and direct integration into C# projects without external dependencies or runtime entry points.