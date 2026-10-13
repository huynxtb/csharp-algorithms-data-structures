using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Strings
{
    /// <summary>
    /// Implements a Suffix Array with LCP (Longest Common Prefix) Array using Kasai's Algorithm.
    /// Provides efficient string operations such as substring search, occurrence counting, and distinct substring counting.
    /// </summary>
    public class SuffixArrayWithLCP
    {
        private readonly string _text;
        private readonly int[] _suffixArray;
        private readonly int[] _lcpArray;
        private readonly int[] _rank;
        private readonly int[] _inverseRank;

        /// <summary>
        /// Gets the suffix array containing indices of all suffixes sorted lexicographically.
        /// </summary>
        public int[] SuffixArray => _suffixArray;

        /// <summary>
        /// Gets the LCP array where LcpArray[i] is the longest common prefix length
        /// between suffixes at SuffixArray[i-1] and SuffixArray[i].
        /// </summary>
        public int[] LcpArray => _lcpArray;

        /// <summary>
        /// Initializes a new instance of the SuffixArrayWithLCP class.
        /// Constructs the suffix array using prefix-doubling and computes the LCP array using Kasai's algorithm.
        /// </summary>
        /// <param name="text">The input string for which to build the suffix array.</param>
        /// <exception cref="ArgumentNullException">Thrown when text is null.</exception>
        public SuffixArrayWithLCP(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            _text = text;
            int n = text.Length;

            if (n == 0)
            {
                _suffixArray = new int[0];
                _lcpArray = new int[0];
                _rank = new int[0];
                _inverseRank = new int[0];
                return;
            }

            _rank = new int[n];
            _inverseRank = new int[n];
            _suffixArray = BuildSuffixArray();
            _lcpArray = BuildLcpArray();
        }

        /// <summary>
        /// Builds the suffix array using the prefix-doubling algorithm in O(n log n) time.
        /// </summary>
        private int[] BuildSuffixArray()
        {
            int n = _text.Length;
            int[] sa = new int[n];
            int[] tmp = new int[n];
            int[] count = new int[Math.Max(256, n)];

            // Initialize ranks based on characters
            for (int i = 0; i < n; i++)
            {
                sa[i] = i;
                _rank[i] = _text[i];
            }

            // Prefix doubling
            for (int k = 1; k < n; k *= 2)
            {
                // Sort by second component (rank at position i+k)
                Array.Clear(count, 0, count.Length);
                for (int i = 0; i < n; i++)
                    count[i + k < n ? _rank[i + k] : 0]++;
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = n - 1; i >= 0; i--)
                    tmp[--count[i + k < n ? _rank[i + k] : 0]] = i;

                // Sort by first component (rank at position i) with stable sort
                Array.Clear(count, 0, count.Length);
                for (int i = 0; i < n; i++)
                    count[_rank[i]]++;
                for (int i = 1; i < count.Length; i++)
                    count[i] += count[i - 1];
                for (int i = n - 1; i >= 0; i--)
                    sa[--count[_rank[tmp[i]]]] = tmp[i];

                // Update ranks
                int[] newRank = new int[n];
                int r = 0;
                newRank[sa[0]] = r;
                for (int i = 1; i < n; i++)
                {
                    if (_rank[sa[i - 1]] != _rank[sa[i]] ||
                        (sa[i - 1] + k < n ? _rank[sa[i - 1] + k] : -1) !=
                        (sa[i] + k < n ? _rank[sa[i] + k] : -1))
                    {
                        r++;
                    }
                    newRank[sa[i]] = r;
                }

                Array.Copy(newRank, _rank, n);

                if (_rank[sa[n - 1]] == n - 1)
                    break;
            }

            return sa;
        }

        /// <summary>
        /// Builds the LCP array using Kasai's algorithm in O(n) time.
        /// </summary>
        private int[] BuildLcpArray()
        {
            int n = _text.Length;
            int[] lcp = new int[n];
            int[] inverseRank = new int[n];

            // Build inverse suffix array (rank to position mapping)
            for (int i = 0; i < n; i++)
                inverseRank[_suffixArray[i]] = i;

            int h = 0;
            for (int i = 0; i < n; i++)
            {
                if (inverseRank[i] > 0)
                {
                    int j = _suffixArray[inverseRank[i] - 1];
                    while (i + h < n && j + h < n && _text[i + h] == _text[j + h])
                        h++;
                    lcp[inverseRank[i]] = h;
                    if (h > 0)
                        h--;
                }
            }

            return lcp;
        }

        /// <summary>
        /// Determines whether the specified pattern exists in the text using binary search.
        /// </summary>
        /// <param name="pattern">The pattern to search for.</param>
        /// <returns>True if the pattern is found; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when pattern is null.</exception>
        public bool Contains(string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (pattern.Length == 0 || _text.Length == 0)
                return pattern.Length == 0;

            return BinarySearchLeft(pattern) < _text.Length &&
                   _text.Substring(_suffixArray[BinarySearchLeft(pattern)]).StartsWith(pattern);
        }

        /// <summary>
        /// Counts the number of occurrences of the specified pattern in the text.
        /// </summary>
        /// <param name="pattern">The pattern to count.</param>
        /// <returns>The number of occurrences of the pattern.</returns>
        /// <exception cref="ArgumentNullException">Thrown when pattern is null.</exception>
        public int CountOccurrences(string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            if (pattern.Length == 0 || _text.Length == 0)
                return 0;

            int left = BinarySearchLeft(pattern);
            int right = BinarySearchRight(pattern);

            if (left > right || left >= _text.Length)
                return 0;

            return right - left + 1;
        }

        /// <summary>
        /// Finds all starting indices where the specified pattern occurs in the text.
        /// </summary>
        /// <param name="pattern">The pattern to find.</param>
        /// <returns>A list of all starting indices where the pattern is found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when pattern is null.</exception>
        public List<int> FindAllOccurrences(string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException(nameof(pattern));

            var occurrences = new List<int>();

            if (pattern.Length == 0 || _text.Length == 0)
                return occurrences;

            int left = BinarySearchLeft(pattern);
            int right = BinarySearchRight(pattern);

            if (left > right || left >= _text.Length)
                return occurrences;

            for (int i = left; i <= right; i++)
                occurrences.Add(_suffixArray[i]);

            return occurrences;
        }

        /// <summary>
        /// Finds the longest substring that appears at least twice in the text using the LCP array.
        /// </summary>
        /// <returns>The longest repeated substring, or an empty string if no repeated substring exists.</returns>
        public string LongestRepeatedSubstring()
        {
            if (_text.Length == 0 || _lcpArray.Length == 0)
                return string.Empty;

            int maxLcp = 0;
            int maxIndex = 0;

            for (int i = 0; i < _lcpArray.Length; i++)
            {
                if (_lcpArray[i] > maxLcp)
                {
                    maxLcp = _lcpArray[i];
                    maxIndex = _suffixArray[i];
                }
            }

            return maxLcp > 0 ? _text.Substring(maxIndex, maxLcp) : string.Empty;
        }

        /// <summary>
        /// Counts the total number of distinct non-empty substrings in the text.
        /// </summary>
        /// <returns>The count of distinct substrings.</returns>
        public long CountDistinctSubstrings()
        {
            int n = _text.Length;
            if (n == 0)
                return 0;

            long totalSubstrings = (long)n * (n + 1) / 2;
            long lcpSum = _lcpArray.Sum();

            return totalSubstrings - lcpSum;
        }

        /// <summary>
        /// Binary searches for the leftmost (smallest) suffix array index where the pattern could match.
        /// </summary>
        private int BinarySearchLeft(string pattern)
        {
            int left = 0, right = _text.Length;

            while (left < right)
            {
                int mid = left + (right - left) / 2;
                if (CompareSuffix(_suffixArray[mid], pattern) < 0)
                    left = mid + 1;
                else
                    right = mid;
            }

            return left;
        }

        /// <summary>
        /// Binary searches for the rightmost (largest) suffix array index where the pattern could match.
        /// </summary>
        private int BinarySearchRight(string pattern)
        {
            int left = 0, right = _text.Length;

            while (left < right)
            {
                int mid = left + (right - left) / 2;
                if (CompareSuffix(_suffixArray[mid], pattern) <= 0)
                    left = mid + 1;
                else
                    right = mid;
            }

            return left - 1;
        }

        /// <summary>
        /// Compares a suffix starting at suffixIndex with the pattern.
        /// Returns: negative if suffix < pattern, 0 if suffix starts with pattern, positive if suffix > pattern.
        /// </summary>
        private int CompareSuffix(int suffixIndex, string pattern)
        {
            int patternIdx = 0;
            while (suffixIndex < _text.Length && patternIdx < pattern.Length)
            {
                int cmp = _text[suffixIndex].CompareTo(pattern[patternIdx]);
                if (cmp != 0)
                    return cmp;
                suffixIndex++;
                patternIdx++;
            }

            if (patternIdx < pattern.Length)
                return -1;
            if (suffixIndex < _text.Length)
                return 1;
            return 0;
        }
    }
}