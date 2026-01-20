using System;
using System.Collections.Generic;

/// <summary>
/// Implements the Boyer-Moore-Horspool (BMH) string search algorithm.
/// Provides functionality to preprocess a pattern and search for all its occurrences in a given text.
/// </summary>
public class BoyerMooreHorspool
{
    private readonly string pattern;
    private readonly int patternLength;
    private readonly int[] badCharShift;

    /// <summary>
    /// Initializes a new instance of the <see cref="BoyerMooreHorspool"/> class
    /// with the specified pattern.
    /// </summary>
    /// <param name="pattern">The pattern string to search for.</param>
    public BoyerMooreHorspool(string pattern)
    {
        if (pattern == null)
            throw new ArgumentNullException(nameof(pattern));
        if (pattern.Length == 0)
            throw new ArgumentException("Pattern must not be empty.", nameof(pattern));

        this.pattern = pattern;
        this.patternLength = pattern.Length;
        this.badCharShift = new int[256]; // Assuming extended ASCII

        PreprocessBadCharShift();
    }

    /// <summary>
    /// Preprocesses the pattern to create the bad character shift table used by the BMH algorithm.
    /// </summary>
    private void PreprocessBadCharShift()
    {
        // Initialize all shifts to the length of the pattern
        for (int i = 0; i < badCharShift.Length; i++)
        {
            badCharShift[i] = patternLength;
        }

        // Fill shift values based on the position of each character in the pattern,
        // except the last character which is handled implicitly by the default length
        for (int i = 0; i < patternLength - 1; i++)
        {
            badCharShift[(byte)pattern[i]] = patternLength - 1 - i;
        }
    }

    /// <summary>
    /// Searches the given text for all occurrences of the pattern.
    /// </summary>
    /// <param name="text">The text string to be searched.</param>
    /// <returns>An IEnumerable&lt;int&gt; containing all starting indices where the pattern matches exactly in the text.</returns>
    public IEnumerable<int> Search(string text)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        int textLength = text.Length;

        if (patternLength > textLength)
            yield break; // Pattern longer than text, no matches

        int skip;
        int index = 0;

        while (index <= textLength - patternLength)
        {
            skip = 0;
            // Compare from the end of the pattern
            for (int j = patternLength - 1; j >= 0; j--)
            {
                if (pattern[j] != text[index + j])
                {
                    skip = badCharShift[(byte)text[index + patternLength - 1]];
                    break;
                }
            }
            if (skip == 0)
            {
                yield return index;
                // Shift by one to continue searching for next occurrences
                skip = 1;
            }
            index += skip;
        }
    }
}