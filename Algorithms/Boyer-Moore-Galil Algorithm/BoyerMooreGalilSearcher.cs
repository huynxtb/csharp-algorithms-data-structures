using System;
using System.Collections.Generic;

namespace BoyerMooreGalilAlgorithm
{
    /// <summary>
    /// Implements the Boyer-Moore string search algorithm enhanced with the Galil rule.
    /// The Galil rule guarantees linear O(n) worst-case time complexity by exploiting pattern periodicity
    /// and skipping redundant character comparisons when a match or partial match occurs.
    /// </summary>
    public sealed class BoyerMooreGalilSearcher
    {
        private const int AlphabetSize = 65536;
        private readonly string _pattern;
        private readonly int _m;
        private readonly int _period;
        private readonly int[] _badChar;
        private readonly int[] _goodSuffixShift;

        /// <summary>
        /// Gets the pattern being searched for.
        /// </summary>
        public string Pattern => _pattern;

        /// <summary>
        /// Gets the period of the pattern (the length of the shortest period).
        /// </summary>
        public int Period => _period;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoyerMooreGalilSearcher"/> class with the specified pattern.
        /// Precomputes the Bad Character table, Good Suffix table, and the pattern's shortest period.
        /// </summary>
        /// <param name="pattern">The pattern string to search for.</param>
        /// <exception cref="ArgumentNullException">Thrown when pattern is null.</exception>
        /// <exception cref="ArgumentException">Thrown when pattern is empty.</exception>
        public BoyerMooreGalilSearcher(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            if (pattern.Length == 0)
            {
                throw new ArgumentException("Pattern cannot be empty.", nameof(pattern));
            }

            _pattern = pattern;
            _m = pattern.Length;
            _badChar = new int[AlphabetSize];
            _goodSuffixShift = new int[_m + 1];

            ComputeBadCharacterTable();
            ComputeGoodSuffixTable();
            _period = ComputeShortestPeriod();
        }

        /// <summary>
        /// Finds the 0-based index of the first occurrence of the pattern in the given text.
        /// </summary>
        /// <param name="text">The text to search within.</param>
        /// <returns>The 0-based index of the first occurrence, or -1 if not found.</returns>
        public int FindFirst(ReadOnlySpan<char> text)
        {
            int n = text.Length;
            if (_m > n)
            {
                return -1;
            }

            int s = 0;
            int l = 0;

            while (s <= n - _m)
            {
                int i = _m - 1;

                while (i >= l && _pattern[i] == text[s + i])
                {
                    i--;
                }

                if (i < l)
                {
                    return s;
                }
                else
                {
                    char mismatchChar = text[s + i];
                    int bcShift = i - _badChar[mismatchChar];
                    int gsShift = _goodSuffixShift[i + 1];
                    int shift = Math.Max(1, Math.Max(bcShift, gsShift));
                    s += shift;
                    l = 0;
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds the 0-based index of the first occurrence of the pattern in the given string.
        /// </summary>
        /// <param name="text">The text to search within.</param>
        /// <returns>The 0-based index of the first occurrence, or -1 if not found.</returns>
        public int FindFirst(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            return FindFirst(text.AsSpan());
        }

        /// <summary>
        /// Finds all 0-based start indices of the pattern occurrences within the given text.
        /// </summary>
        /// <param name="text">The text to search within.</param>
        /// <returns>An enumerable collection of match start indices.</returns>
        public IEnumerable<int> FindAllOccurrences(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            int n = text.Length;
            if (_m > n)
            {
                yield break;
            }

            int s = 0;
            int l = 0;

            while (s <= n - _m)
            {
                int i = _m - 1;

                while (i >= l && _pattern[i] == text[s + i])
                {
                    i--;
                }

                if (i < l)
                {
                    yield return s;
                    s += _period;
                    l = _m - _period;
                }
                else
                {
                    char mismatchChar = text[s + i];
                    int bcShift = i - _badChar[mismatchChar];
                    int gsShift = _goodSuffixShift[i + 1];
                    int shift = Math.Max(1, Math.Max(bcShift, gsShift));
                    s += shift;
                    l = 0;
                }
            }
        }

        /// <summary>
        /// Precomputes the Bad Character heuristic table.
        /// </summary>
        private void ComputeBadCharacterTable()
        {
            Array.Fill(_badChar, -1);
            for (int i = 0; i < _m; i++)
            {
                _badChar[_pattern[i]] = i;
            }
        }

        /// <summary>
        /// Precomputes the Strong Good Suffix heuristic table.
        /// </summary>
        private void ComputeGoodSuffixTable()
        {
            int[] suff = ComputeSuffixes();

            // Case 2: Suffix occurs as a prefix of the pattern
            for (int i = 0; i <= _m; i++)
            {
                _goodSuffixShift[i] = _m;
            }

            int j = 0;
            for (int i = _m - 1; i >= -1; i--)
            {
                if (i == -1 || suff[i] == i + 1)
                {
                    for (; j < _m - 1 - i; j++)
                    {
                        if (_goodSuffixShift[j] == _m)
                        {
                            _goodSuffixShift[j] = _m - 1 - i;
                        }
                    }
                }
            }

            // Case 1: Suffix occurs elsewhere in the pattern
            for (int i = 0; i < _m - 1; i++)
            {
                _goodSuffixShift[_m - 1 - suff[i]] = _m - 1 - i;
            }
        }

        /// <summary>
        /// Computes the longest common suffix lengths between pattern prefixes and pattern.
        /// </summary>
        private int[] ComputeSuffixes()
        {
            int[] suff = new int[_m];
            suff[_m - 1] = _m;
            int g = _m - 1;
            int f = _m - 1;

            for (int i = _m - 2; i >= 0; --i)
            {
                if (i > g && suff[i + _m - 1 - f] < i - g)
                {
                    suff[i] = suff[i + _m - 1 - f];
                }
                else
                {
                    if (i < g)
                    {
                        g = i;
                    }
                    f = i;
                    while (g >= 0 && _pattern[g] == _pattern[g + _m - 1 - f])
                    {
                        g--;
                    }
                    suff[i] = f - g;
                }
            }

            return suff;
        }

        /// <summary>
        /// Computes the shortest period length of the pattern using the Knuth-Morris-Pratt prefix function.
        /// </summary>
        private int ComputeShortestPeriod()
        {
            int[] pi = new int[_m];
            int k = 0;

            for (int i = 1; i < _m; i++)
            {
                while (k > 0 && _pattern[k] != _pattern[i])
                {
                    k = pi[k - 1];
                }
                if (_pattern[k] == _pattern[i])
                {
                    k++;
                }
                pi[i] = k;
            }

            int longestBorder = pi[_m - 1];
            int period = _m - longestBorder;
            return (_m % period == 0) ? period : _m;
        }
    }
}