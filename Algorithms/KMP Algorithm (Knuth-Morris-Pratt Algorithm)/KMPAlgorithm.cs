using System;
using System.Collections.Generic;

/// <summary>
/// KMP Algorithm (Knuth-Morris-Pratt Algorithm) implementation for efficient substring searching.
/// This class provides methods to search for a pattern within a text using the KMP algorithm,
/// which preprocesses the pattern to achieve linear time complexity O(n + m).
/// </summary>
public class KMPAlgorithm
{
    /// <summary>
    /// Computes the LPS (Longest Prefix Suffix) array for the given pattern.
    /// The LPS array is used to avoid redundant comparisons during the pattern matching process.
    /// LPS[i] contains the length of the longest proper prefix which is also a suffix for pattern[0..i].
    /// </summary>
    /// <param name="pattern">The pattern string for which to compute the LPS array.</param>
    /// <returns>An integer array representing the LPS values for each position in the pattern.</returns>
    public int[] ComputeLPS(string pattern)
    {
        // Handle edge cases: null or empty pattern
        if (string.IsNullOrEmpty(pattern))
        {
            return new int[0];
        }

        int patternLength = pattern.Length;
        int[] lps = new int[patternLength];

        // lps[0] is always 0 because there is no proper prefix and suffix for a single character
        lps[0] = 0;

        // len is the length of the previous longest prefix suffix
        int len = 0;

        // Iterate from index 1 to the end of the pattern
        int i = 1;
        while (i < patternLength)
        {
            // If characters match, increment the length of the prefix suffix
            if (pattern[i] == pattern[len])
            {
                len++;
                lps[i] = len;
                i++;
            }
            else
            {
                // If characters do not match and len is not 0,
                // we use the previous LPS value to skip redundant comparisons
                if (len != 0)
                {
                    len = lps[len - 1];
                }
                else
                {
                    // If len is 0, set lps[i] to 0 and move to the next character
                    lps[i] = 0;
                    i++;
                }
            }
        }

        return lps;
    }

    /// <summary>
    /// Searches for all occurrences of a pattern within a given text using the KMP algorithm.
    /// Returns a list of starting indices where the pattern is found in the text.
    /// </summary>
    /// <param name="text">The text string in which to search for the pattern.</param>
    /// <param name="pattern">The pattern string to search for.</param>
    /// <returns>A list of integers representing the starting indices of all occurrences of the pattern in the text.</returns>
    public List<int> KMPSearch(string text, string pattern)
    {
        // Initialize the result list to store starting indices of matches
        List<int> matches = new List<int>();

        // Handle edge cases: null, empty text, or empty pattern
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
        {
            return matches;
        }

        // If pattern is longer than text, no match is possible
        if (pattern.Length > text.Length)
        {
            return matches;
        }

        int textLength = text.Length;
        int patternLength = pattern.Length;

        // Compute the LPS array for the pattern
        int[] lps = ComputeLPS(pattern);

        // i is the index for text, j is the index for pattern
        int i = 0;  // index for text
        int j = 0;  // index for pattern

        // Iterate through the text
        while (i < textLength)
        {
            // If characters match, increment both indices
            if (pattern[j] == text[i])
            {
                i++;
                j++;
            }

            // If we have matched the entire pattern, record the starting index
            if (j == patternLength)
            {
                // Add the starting index of the match (i - j) to the results
                matches.Add(i - j);

                // Use the LPS array to find the next potential match
                // This allows overlapping matches to be found
                j = lps[j - 1];
            }
            // If characters do not match
            else if (i < textLength && pattern[j] != text[i])
            {
                // If j is not 0, use the LPS array to avoid redundant comparisons
                if (j != 0)
                {
                    j = lps[j - 1];
                }
                else
                {
                    // If j is 0, simply move to the next character in the text
                    i++;
                }
            }
        }

        return matches;
    }
}