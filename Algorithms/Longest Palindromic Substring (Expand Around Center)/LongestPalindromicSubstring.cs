using System;

/// <summary>
/// Provides functionality for finding the longest palindromic substring within a string
/// using the Expand Around Center approach.
/// </summary>
public class LongestPalindromicSubstring
{
    /// <summary>
    /// Finds the longest palindromic substring within the given input string.
    /// If multiple palindromes of equal length exist, the first one found is returned.
    /// </summary>
    /// <param name="input">The input string to search.</param>
    /// <returns>The longest palindromic substring, or an empty string for null/empty input.</returns>
    public string FindLongestPalindrome(string input)
    {
        // Edge case: null or empty input returns empty string.
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // Edge case: single character is always a palindrome.
        if (input.Length == 1)
        {
            return input;
        }

        int startIndex = 0; // Starting index of the longest palindrome found.
        int maxLength = 1;  // Length of the longest palindrome found (at least 1).

        // Iterate through every possible center in the string.
        for (int i = 0; i < input.Length; i++)
        {
            // Case 1: Odd-length palindrome. Center is a single character at index i.
            int oddLength = ExpandAroundCenter(input, i, i);

            // Case 2: Even-length palindrome. Center lies between index i and i + 1.
            int evenLength = ExpandAroundCenter(input, i, i + 1);

            // Determine the maximum length found for this center.
            int currentMax = Math.Max(oddLength, evenLength);

            // Update tracking variables if we found a longer palindrome.
            if (currentMax > maxLength)
            {
                maxLength = currentMax;
                // Compute the starting index of the palindrome.
                // For a palindrome of length L centered at i (or between i and i+1),
                // the start index is i - (L - 1) / 2.
                startIndex = i - (currentMax - 1) / 2;
            }
        }

        // Extract and return the longest palindromic substring.
        return input.Substring(startIndex, maxLength);
    }

    /// <summary>
    /// Expands outward from a given center (or pair of centers) as long as the
    /// characters on both sides match, and returns the length of the palindrome found.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <param name="left">The left-side starting index of the center.</param>
    /// <param name="right">The right-side starting index of the center.</param>
    /// <returns>The length of the palindrome centered at the given indices.</returns>
    private int ExpandAroundCenter(string input, int left, int right)
    {
        // Expand outward while indices are within bounds and characters match.
        // Case-sensitive comparison via direct character equality.
        while (left >= 0 && right < input.Length && input[left] == input[right])
        {
            left--;
            right++;
        }

        // When the loop exits, left and right have moved one step beyond the palindrome.
        // The palindrome spans from (left + 1) to (right - 1), inclusive.
        // Length = (right - 1) - (left + 1) + 1 = right - left - 1.
        return right - left - 1;
    }
}