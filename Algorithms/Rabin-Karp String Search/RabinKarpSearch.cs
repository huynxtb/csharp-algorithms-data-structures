using System;
using System.Collections.Generic;

/// <summary>
/// Implements the Rabin-Karp string searching algorithm.
/// </summary>
public static class RabinKarpSearch
{
    private const int PrimeMultiplier = 256; // A prime number used for hashing (e.g., size of alphabet)
    private const long PrimeModulus = 101; // A large prime number for the modulus to minimize collisions

    /// <summary>
    /// Searches for all occurrences of a pattern within a text using the Rabin-Karp algorithm.
    /// </summary>
    /// <param name="text">The text to search within.</param>
    /// <param name="pattern">The pattern to search for.</param>
    /// <returns>An IEnumerable of integers representing the 0-based starting indices of all occurrences of the pattern in the text.</returns>
    public static IEnumerable<int> Search(string text, string pattern)
    {
        if (text == null || pattern == null)
        {
            throw new ArgumentNullException("Input strings cannot be null.");
        }

        int n = text.Length;
        int m = pattern.Length;

        if (m == 0)
        {
            // An empty pattern is considered to match at every position, including after the last character.
            for (int i = 0; i <= n; ++i) yield return i;
            yield break;
        }

        if (n == 0 || m > n)
        {
            // No matches possible if text is empty or pattern is longer than text.
            yield break;
        }

        long patternHash = 0;
        long textHash = 0;
        long highestPower = 1;

        // Calculate (PrimeMultiplier^(m-1)) % PrimeModulus
        for (int i = 0; i < m - 1; i++)
        {
            highestPower = (highestPower * PrimeMultiplier) % PrimeModulus;
        }

        // Calculate the hash value for the pattern and the first window of text
        for (int i = 0; i < m; i++)
        {
            patternHash = (patternHash * PrimeMultiplier + pattern[i]) % PrimeModulus;
            textHash = (textHash * PrimeMultiplier + text[i]) % PrimeModulus;
        }

        // Slide the pattern over the text one by one
        for (int i = 0; i <= n - m; i++)
        {
            // Check if hash values match. If they do, then only check characters one by one.
            if (patternHash == textHash)
            {
                // Perform character-by-character verification to handle collisions
                bool match = true;
                for (int j = 0; j < m; j++)
                {
                    if (text[i + j] != pattern[j])
                    {
                        match = false;
                        break;
                    }
                }

                // If patternHash and textHash match, and characters match, then pattern found
                if (match)
                {
                    yield return i;
                }
            }

            // Calculate hash value for the next window of text: Remove leading character, add trailing character
            if (i < n - m)
            {
                // Remove the hash contribution of the leading character
                textHash = (textHash - text[i] * highestPower) % PrimeModulus;
                // Ensure textHash is non-negative
                if (textHash < 0) textHash = (textHash + PrimeModulus);

                // Add the hash contribution of the trailing character
                textHash = (textHash * PrimeMultiplier + text[i + m]) % PrimeModulus;
            }
        }
    }
}