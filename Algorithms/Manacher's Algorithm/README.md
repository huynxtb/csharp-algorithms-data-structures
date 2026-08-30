# Manacher's Algorithm

### 1. Introduction
Manacher's Algorithm finds the longest palindromic substring in a given string in linear time. It optimizes the naive expansion approach by reusing previously computed palindrome symmetry information, avoiding redundant comparisons.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        string input = "babad";
        string longest = Manacher.FindLongestPalindromicSubstring(input);
        Console.WriteLine($"Longest Palindrome: {longest}"); // Output: "bab" or "aba"

        int[] table = Manacher.GetPalindromeTable(input);
        Console.WriteLine($"Table Length: {table.Length}");
    }
}
```

### 3. Detailed Explanation
- **Preprocessing**: The algorithm inserts a delimiter character (e.g., `#`) between every character, and adds start/end anchors (e.g., `^` and `$`). This transforms the string so that all palindromes (even and odd length) become odd-length palindromes, simplifying the expansion logic.
- **State Tracking**: It tracks the center `C` and the right boundary `R` of the rightmost palindrome found so far.
- **Mirroring**: For each index `i`, it calculates its mirror position `iMirror` relative to `C`. If `i` is within `R`, the palindrome radius at `i` is initialized to at least `Math.Min(R - i, P[iMirror])`.
- **Expansion**: The algorithm expands outward from `i` only when necessary, updating `C` and `R` when the expansion exceeds `R`.

### 4. Complexity Analysis
- **Time Complexity**: $O(N)$ where $N$ is the length of the input string. The inner loop comparison pointer only moves forward, ensuring linear time.
- **Space Complexity**: $O(N)$ auxiliary space to store the preprocessed string and the palindrome radius table.