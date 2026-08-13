using System;
using System.Collections.Generic;

namespace Algorithms.String
{
    /// <summary>
    /// Implements the Z-Algorithm for linear-time string matching.
    /// </summary>
    public static class ZAlgorithm
    {
        /// <summary>
        /// Calculates the Z-array for a given string.
        /// The Z-array Z for a string S of length N is an array of length N where Z[i] is the length of the longest substring starting from S[i] which is also a prefix of S.
        /// </summary>
        /// <param name="s">The input string.</param>
        /// <returns>The Z-array for the input string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input string is null.</exception>
        public static int[] CalculateZArray(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            int n = s.Length;
            if (n == 0)
            {
                return new int[0];
            }

            int[] z = new int[n];
            int l = 0, r = 0;

            for (int i = 1; i < n; i++)
            {
                if (i <= r)
                {
                    z[i] = Math.Min(r - i + 1, z[i - l]);
                }

                while (i + z[i] < n && s[z[i]] == s[i + z[i]])
                {
                    z[i]++;
                }

                if (i + z[i] - 1 > r)
                {
                    l = i;
                    r = i + z[i] - 1;
                }
            }
            return z;
        }

        /// <summary>
        /// Searches for all occurrences of a pattern within a text using the Z-Algorithm.
        /// </summary>
        /// <param name="text">The text to search within.</param>
        /// <param name="pattern">The pattern to search for.</param>
        /// <returns>A list of 0-based starting indices where the pattern is found in the text.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the text or pattern is null.</exception>
        public static List<int> Search(string text, string pattern)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            List<int> occurrences = new List<int>();
            int n = text.Length;
            int m = pattern.Length;

            if (m == 0)
            {
                // An empty pattern is considered to occur at every position.
                for (int i = 0; i <= n; i++) occurrences.Add(i);
                return occurrences;
            }

            if (m > n)
            {
                return occurrences; // Pattern cannot be found if it's longer than the text.
            }

            // Concatenate pattern, a sentinel character (that does not appear in either string), and text.
            // Using a character unlikely to be in typical strings, like '$'.
            // A more robust solution might involve checking for sentinel presence or using a different approach.
            string combined = pattern + '$' + text;
            int[] z = CalculateZArray(combined);

            // Iterate through the Z-array starting from the position after the pattern and sentinel.
            // If Z[i] equals the pattern length, it means the pattern is found starting at text index (i - m - 1).
            for (int i = m + 1; i < combined.Length; i++)
            {
                if (z[i] == m)
                {
                    occurrences.Add(i - m - 1);
                }
            }

            return occurrences;
        }
    }
}