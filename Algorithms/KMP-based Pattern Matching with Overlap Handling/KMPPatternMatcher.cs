using System;
using System.Collections.Generic;

public class KMPPatternMatcher
{
    // Method to find occurrences of the pattern in the text
    public List<int> FindOccurrences(string text, string pattern)
    {
        List<int> occurrences = new List<int>();
        int[] lps = ComputeLPSArray(pattern);
        int i = 0; // index for text
        int j = 0; // index for pattern

        while (i < text.Length)
        {
            if (pattern[j] == text[i])
            {
                i++;
                j++;
            }
            if (j == pattern.Length)
            {
                occurrences.Add(i - j); // Found a match
                j = lps[j - 1]; // Handle overlapping
            }
            else if (i < text.Length && pattern[j] != text[i])
            {
                if (j != 0)
                    j = lps[j - 1];
                else
                    i++;
            }
        }
        return occurrences;
    }

    // Method to compute the LPS (Longest Prefix Suffix) array
    private int[] ComputeLPSArray(string pattern)
    {
        int[] lps = new int[pattern.Length];
        int length = 0; // length of the previous longest prefix suffix
        int i = 1;
        lps[0] = 0; // lps[0] is always 0

        while (i < pattern.Length)
        {
            if (pattern[i] == pattern[length])
            {
                length++;
                lps[i] = length;
                i++;
            }
            else
            {
                if (length != 0)
                    length = lps[length - 1];
                else
                {
                    lps[i] = 0;
                    i++;
                }
            }
        }
        return lps;
    }
}