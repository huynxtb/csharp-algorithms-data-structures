using System;
using System.Collections.Generic;

/// <summary>
/// Implements the Boyer-Moore string search algorithm.
/// Allows searching for all occurrences of a pattern within a given text efficiently.
/// </summary>
public static class BoyerMoore
{
    /// <summary>
    /// Searches for all occurrences of the pattern in the provided text using the Boyer-Moore algorithm.
    /// </summary>
    /// <param name="text">The text to search in.</param>
    /// <param name="pattern">The pattern to search for.</param>
    /// <returns>A list of starting indices where the pattern occurs in the text.</returns>
    public static List<int> Search(string text, string pattern)
    {
        var result = new List<int>();
        if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(text) || pattern.Length > text.Length)
            return result;

        int[] badCharShift = PreprocessBadCharacterShift(pattern);
        int[] suffix = new int[pattern.Length];
        bool[] prefix = new bool[pattern.Length];
        PreprocessGoodSuffixShift(pattern, suffix, prefix);

        int n = text.Length;
        int m = pattern.Length;

        int i = 0;  // i is the index in text where the current alignment of pattern starts

        while (i <= n - m)
        {
            int j;
            for (j = m - 1; j >= 0; j--)
            {
                if (pattern[j] != text[i + j])
                    break;
            }
            if (j < 0)
            {
                // pattern found at index i
                result.Add(i);
                i += m; // shift pattern to align after the found match
            }
            else
            {
                int badCharIndex = i + j;
                char badChar = text[badCharIndex];
                int badCharShiftAmount = j - badCharShift[badChar];
                if (badCharShiftAmount < 1)
                    badCharShiftAmount = 1;

                int goodSuffixShiftAmount = 0;
                if (j < m - 1)
                    goodSuffixShiftAmount = MoveByGoodSuffix(j, m, suffix, prefix);

                i += Math.Max(badCharShiftAmount, goodSuffixShiftAmount);
            }
        }

        return result;
    }

    // Preprocessing for bad character heuristic
    private static int[] PreprocessBadCharacterShift(string pattern)
    {
        const int ALPHABET_SIZE = 256;
        int[] badCharShift = new int[ALPHABET_SIZE];

        for (int i = 0; i < ALPHABET_SIZE; i++)
            badCharShift[i] = -1;

        for (int i = 0; i < pattern.Length; i++)
        {
            badCharShift[(int)pattern[i]] = i;
        }

        return badCharShift;
    }

    // Preprocessing for good suffix heuristic
    private static void PreprocessGoodSuffixShift(string pattern, int[] suffix, bool[] prefix)
    {
        int m = pattern.Length;
        for (int i = 0; i < m; i++)
        {
            suffix[i] = -1;
            prefix[i] = false;
        }

        for (int i = 0; i < m - 1; i++)
        {
            int j = i;
            int k = 0; // length of suffix

            // Find the longest suffix of pattern[0..i] which matches a suffix of pattern
            while (j >= 0 && pattern[j] == pattern[m - 1 - k])
            {
                j--;
                k++;
                suffix[k] = j + 1;
            }

            if (j == -1)
            {
                prefix[k] = true;
            }
        }
    }

    /// <summary>
    /// Calculates shift by good suffix rule using precomputed values.
    /// </summary>
    private static int MoveByGoodSuffix(int j, int m, int[] suffix, bool[] prefix)
    {
        int k = m - 1 - j; // length of good suffix

        if (suffix[k] != -1)
            return j - suffix[k] + 1;

        for (int r = j + 2; r <= m - 1; r++)
        {
            if (prefix[m - r])
                return r;
        }

        return m;
    }
}
