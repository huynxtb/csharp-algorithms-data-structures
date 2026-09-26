using System;
using System.Collections.Generic;

namespace Algorithms
{
    /// <summary>
    /// Implements the Bitap (Shift-Or / Shift-And / Wu-Manber) algorithm for exact and approximate (fuzzy) string matching.
    /// </summary>
    public static class BitapSearch
    {
        private const int MaxPatternLength = 63;

        /// <summary>
        /// Finds the starting index of the first exact match of the pattern in the given text.
        /// </summary>
        /// <param name="text">The text string to search within.</param>
        /// <param name="pattern">The pattern string to find.</param>
        /// <returns>The zero-based starting index of the first exact match, or -1 if no match is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="pattern"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="pattern"/> length exceeds 63 characters.</exception>
        public static int ExactSearch(string text, string pattern)
        {
            ValidateInputs(text, pattern);

            int m = pattern.Length;
            if (m == 0)
            {
                return 0;
            }

            int n = text.Length;
            if (n < m)
            {
                return -1;
            }

            Dictionary<char, long> patternMask = BuildPatternMask(pattern);
            long state = ~1L;
            long matchMask = 1L << (m - 1);

            for (int i = 0; i < n; i++)
            {
                char c = text[i];
                long charMask = patternMask.TryGetValue(c, out long mask) ? mask : ~0L;
                state = (state << 1) | charMask;

                if ((state & matchMask) == 0)
                {
                    return i - m + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds all zero-based starting indices where the pattern occurs exactly in the text.
        /// </summary>
        /// <param name="text">The text string to search within.</param>
        /// <param name="pattern">The pattern string to find.</param>
        /// <returns>A list of starting indices where exact matches occur.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="pattern"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="pattern"/> length exceeds 63 characters.</exception>
        public static List<int> ExactSearchAll(string text, string pattern)
        {
            ValidateInputs(text, pattern);

            var matches = new List<int>();
            int m = pattern.Length;
            if (m == 0)
            {
                return matches;
            }

            int n = text.Length;
            if (n < m)
            {
                return matches;
            }

            Dictionary<char, long> patternMask = BuildPatternMask(pattern);
            long state = ~1L;
            long matchMask = 1L << (m - 1);

            for (int i = 0; i < n; i++)
            {
                char c = text[i];
                long charMask = patternMask.TryGetValue(c, out long mask) ? mask : ~0L;
                state = (state << 1) | charMask;

                if ((state & matchMask) == 0)
                {
                    matches.Add(i - m + 1);
                }
            }

            return matches;
        }

        /// <summary>
        /// Finds the ending index of the first approximate match of the pattern in text within the specified edit distance.
        /// </summary>
        /// <param name="text">The text string to search within.</param>
        /// <param name="pattern">The pattern string to find.</param>
        /// <param name="maxEditDistance">The maximum allowable Levenshtein distance (insertions, deletions, substitutions).</param>
        /// <returns>The zero-based ending index (inclusive) of the first match, or -1 if no match is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="pattern"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="pattern"/> length exceeds 63 characters, or <paramref name="maxEditDistance"/> is negative.</exception>
        public static int FuzzySearch(string text, string pattern, int maxEditDistance)
        {
            ValidateInputs(text, pattern);
            if (maxEditDistance < 0)
            {
                throw new ArgumentException("Max edit distance cannot be negative.", nameof(maxEditDistance));
            }

            int m = pattern.Length;
            if (m == 0)
            {
                return 0;
            }

            int n = text.Length;
            if (n == 0 && maxEditDistance < m)
            {
                return -1;
            }

            if (maxEditDistance >= m)
            {
                return 0;
            }

            Dictionary<char, long> patternMask = BuildPatternMask(pattern);
            long matchMask = 1L << (m - 1);
            long[] r = new long[maxEditDistance + 1];

            for (int k = 0; k <= maxEditDistance; k++)
            {
                r[k] = ~1L;
            }

            for (int i = 0; i < n; i++)
            {
                char c = text[i];
                long charMask = patternMask.TryGetValue(c, out long mask) ? mask : ~0L;
                long oldR0 = r[0];

                r[0] = (r[0] << 1) | charMask;

                for (int d = 1; d <= maxEditDistance; d++)
                {
                    long sub = (oldR0 << 1) | charMask;
                    long ins = oldR0 | charMask;
                    long del = (r[d] << 1) | charMask;
                    long match = (r[d] << 1) | charMask;

                    oldR0 = r[d];
                    r[d] = sub & ins & del & match;
                }

                if ((r[maxEditDistance] & matchMask) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds all ending indices of approximate matches of the pattern in text within the specified edit distance.
        /// </summary>
        /// <param name="text">The text string to search within.</param>
        /// <param name="pattern">The pattern string to find.</param>
        /// <param name="maxEditDistance">The maximum allowable Levenshtein distance (insertions, deletions, substitutions).</param>
        /// <returns>A list of zero-based ending indices (inclusive) of approximate matches.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="pattern"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="pattern"/> length exceeds 63 characters, or <paramref name="maxEditDistance"/> is negative.</exception>
        public static List<int> FuzzySearchAll(string text, string pattern, int maxEditDistance)
        {
            ValidateInputs(text, pattern);
            if (maxEditDistance < 0)
            {
                throw new ArgumentException("Max edit distance cannot be negative.", nameof(maxEditDistance));
            }

            var matches = new List<int>();
            int m = pattern.Length;
            if (m == 0)
            {
                return matches;
            }

            int n = text.Length;
            if (n == 0)
            {
                return matches;
            }

            Dictionary<char, long> patternMask = BuildPatternMask(pattern);
            long matchMask = 1L << (m - 1);
            long[] r = new long[maxEditDistance + 1];

            for (int k = 0; k <= maxEditDistance; k++)
            {
                r[k] = ~1L;
            }

            for (int i = 0; i < n; i++)
            {
                char c = text[i];
                long charMask = patternMask.TryGetValue(c, out long mask) ? mask : ~0L;
                long oldPrev = r[0];

                r[0] = (r[0] << 1) | charMask;

                for (int d = 1; d <= maxEditDistance; d++)
                {
                    long sub = (oldPrev << 1) | charMask;
                    long ins = oldPrev | charMask;
                    long del = (r[d] << 1) | charMask;
                    long match = (r[d] << 1) | charMask;

                    oldPrev = r[d];
                    r[d] = sub & ins & del & match;
                }

                if ((r[maxEditDistance] & matchMask) == 0)
                {
                    matches.Add(i);
                }
            }

            return matches;
        }

        private static void ValidateInputs(string text, string pattern)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (pattern.Length > MaxPatternLength)
            {
                throw new ArgumentException($"Pattern length ({pattern.Length}) exceeds the maximum supported length of {MaxPatternLength} characters.", nameof(pattern));
            }
        }

        private static Dictionary<char, long> BuildPatternMask(string pattern)
        {
            var mask = new Dictionary<char, long>();
            for (int i = 0; i < pattern.Length; i++)
            {
                char c = pattern[i];
                if (!mask.ContainsKey(c))
                {
                    mask[c] = ~0L;
                }
                mask[c] &= ~(1L << i);
            }
            return mask;
        }
    }
}