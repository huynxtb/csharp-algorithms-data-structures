# Introduction
The Longest Palindromic Substring algorithm is used to find the longest substring in a given string that is a palindrome. A palindrome is a sequence that reads the same backward as forward.

# Usage
```csharp
string input = "babad";
string longestPalindrome = LongestPalindromicSubstring.LongestPalindrome(input);
Console.WriteLine(longestPalindrome);
```

# Detailed Explanation
The implementation works by iterating over the input string and treating each character as the center of a potential palindrome. It checks for both odd-length and even-length palindromes by calling the `ExpandAroundCenter` method. This method expands around the center of the palindrome as long as the characters on both sides are equal, effectively finding the longest palindromic substring centered at the given position.

# Complexity Analysis
- Time complexity: O(n^2) where n is the length of the input string. This is because in the worst case, we are potentially expanding around each character in the string.
- Space complexity: O(1) since we only use a constant amount of space to store the start and end indices of the longest palindromic substring.