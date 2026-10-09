using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.StringSearch
{
    /// <summary>
    /// Rabin-Karp multiple pattern string search algorithm.
    /// Efficiently searches for multiple patterns in a single text using rolling hash values.
    /// Patterns are grouped by length for optimized searching.
    /// </summary>
    public sealed class RabinKarpMultiPattern
    {
        private readonly Dictionary<int, List<(string pattern, long hash)>> _patternsByLength;
        private readonly Dictionary<int, long> _hValues; // h = base^(length-1) mod prime, per length
        private readonly long _prime;
        private readonly int _baseValue;
        private readonly HashSet<string> _uniquePatterns;

        /// <summary>
        /// Initializes a new instance of the RabinKarpMultiPattern class.
        /// </summary>
        /// <param name="patterns">Collection of patterns to search for.</param>
        /// <param name="prime">Prime modulus for hash computation (default: 1000000007).</param>
        /// <param name="baseValue">Base value for rolling hash (default: 256).</param>
        /// <exception cref="ArgumentException">Thrown if patterns is null, empty, or contains null/empty strings.</exception>
        public RabinKarpMultiPattern(IEnumerable<string> patterns, long prime = 1000000007L, int baseValue = 256)
        {
            if (patterns == null)
                throw new ArgumentException("Patterns collection cannot be null.", nameof(patterns));

            var patternList = patterns.ToList();
            if (patternList.Count == 0)
                throw new ArgumentException("Patterns collection cannot be empty.", nameof(patterns));

            _prime = prime;
            _baseValue = baseValue;
            _uniquePatterns = new HashSet<string>();
            _patternsByLength = new Dictionary<int, List<(string, long)>>();
            _hValues = new Dictionary<int, long>();

            foreach (var pattern in patternList)
            {
                if (pattern == null || pattern.Length == 0)
                    throw new ArgumentException("Patterns cannot be null or empty.", nameof(patterns));

                _uniquePatterns.Add(pattern);
            }

            // Group unique patterns by length and compute hashes
            foreach (var pattern in _uniquePatterns)
            {
                int length = pattern.Length;

                if (!_patternsByLength.ContainsKey(length))
                {
                    _patternsByLength[length] = new List<(string, long)>();
                    _hValues[length] = ComputeH(length);
                }

                long hash = ComputeInitialHash(pattern);
                _patternsByLength[length].Add((pattern, hash));
            }
        }

        /// <summary>
        /// Searches for all occurrences of any pattern in the given text.
        /// </summary>
        /// <param name="text">The text to search in.</param>
        /// <returns>A read-only list of MatchResult objects ordered by start index and pattern.</returns>
        public IReadOnlyList<MatchResult> Search(string text)
        {
            var results = new List<MatchResult>();

            if (string.IsNullOrEmpty(text))
                return results.AsReadOnly();

            // Search for each distinct pattern length
            foreach (var kvp in _patternsByLength)
            {
                int patternLength = kvp.Key;
                var patternsOfLength = kvp.Value;

                if (patternLength > text.Length)
                    continue;

                long h = _hValues[patternLength];

                // Compute initial hash for first window
                long textHash = ComputeInitialHash(text.Substring(0, patternLength));

                // Check first window
                foreach (var (pattern, patternHash) in patternsOfLength)
                {
                    if (textHash == patternHash && VerifyMatch(text, 0, pattern))
                    {
                        results.Add(new MatchResult(0, pattern));
                    }
                }

                // Roll through the rest of the text
                for (int i = patternLength; i < text.Length; i++)
                {
                    // Remove leftmost character and add rightmost character
                    textHash = RollingHash(textHash, text[i - patternLength], text[i], h);

                    // Check current window
                    foreach (var (pattern, patternHash) in patternsOfLength)
                    {
                        if (textHash == patternHash && VerifyMatch(text, i - patternLength + 1, pattern))
                        {
                            results.Add(new MatchResult(i - patternLength + 1, pattern));
                        }
                    }
                }
            }

            // Sort by start index, then by pattern name
            return results.OrderBy(r => r.StartIndex).ThenBy(r => r.Pattern).ToList().AsReadOnly();
        }

        /// <summary>
        /// Determines whether the text contains at least one of the patterns.
        /// </summary>
        /// <param name="text">The text to search in.</param>
        /// <returns>True if at least one pattern is found; otherwise, false.</returns>
        public bool ContainsAny(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            foreach (var kvp in _patternsByLength)
            {
                int patternLength = kvp.Key;
                var patternsOfLength = kvp.Value;

                if (patternLength > text.Length)
                    continue;

                long h = _hValues[patternLength];
                long textHash = ComputeInitialHash(text.Substring(0, patternLength));

                foreach (var (pattern, patternHash) in patternsOfLength)
                {
                    if (textHash == patternHash && VerifyMatch(text, 0, pattern))
                        return true;
                }

                for (int i = patternLength; i < text.Length; i++)
                {
                    textHash = RollingHash(textHash, text[i - patternLength], text[i], h);

                    foreach (var (pattern, patternHash) in patternsOfLength)
                    {
                        if (textHash == patternHash && VerifyMatch(text, i - patternLength + 1, pattern))
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Computes the initial hash for a given string.
        /// </summary>
        private long ComputeInitialHash(string str)
        {
            long hash = 0;
            for (int i = 0; i < str.Length; i++)
            {
                hash = (hash * _baseValue + str[i]) % _prime;
            }
            return hash;
        }

        /// <summary>
        /// Computes h = base^(length-1) mod prime using modular exponentiation.
        /// </summary>
        private long ComputeH(int length)
        {
            return ModPow(_baseValue, length - 1, _prime);
        }

        /// <summary>
        /// Performs modular exponentiation: (base^exp) mod mod.
        /// </summary>
        private long ModPow(long baseValue, int exponent, long mod)
        {
            long result = 1;
            long base_ = baseValue % mod;

            while (exponent > 0)
            {
                if ((exponent & 1) == 1)
                    result = (result * base_) % mod;

                base_ = (base_ * base_) % mod;
                exponent >>= 1;
            }

            return result;
        }

        /// <summary>
        /// Computes the rolling hash for the next window.
        /// Formula: newHash = ((oldHash - text[i] * h) * base + text[i+m]) % prime
        /// </summary>
        private long RollingHash(long oldHash, char removedChar, char addedChar, long h)
        {
            long newHash = (oldHash - removedChar * h) % _prime;
            newHash = (newHash * _baseValue + addedChar) % _prime;

            // Handle negative modulo
            if (newHash < 0)
                newHash += _prime;

            return newHash;
        }

        /// <summary>
        /// Verifies that a pattern actually matches at the given position in text.
        /// Used to eliminate false positives from hash collisions.
        /// </summary>
        private bool VerifyMatch(string text, int startIndex, string pattern)
        {
            if (startIndex + pattern.Length > text.Length)
                return false;

            for (int i = 0; i < pattern.Length; i++)
            {
                if (text[startIndex + i] != pattern[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Represents a single pattern match result.
        /// </summary>
        public readonly struct MatchResult : IEquatable<MatchResult>
        {
            /// <summary>
            /// Gets the zero-based start index of the match in the text.
            /// </summary>
            public int StartIndex { get; }

            /// <summary>
            /// Gets the matched pattern string.
            /// </summary>
            public string Pattern { get; }

            /// <summary>
            /// Initializes a new instance of the MatchResult struct.
            /// </summary>
            /// <param name="startIndex">The start index of the match.</param>
            /// <param name="pattern">The matched pattern.</param>
            public MatchResult(int startIndex, string pattern)
            {
                StartIndex = startIndex;
                Pattern = pattern;
            }

            /// <summary>
            /// Returns a string representation of the match result.
            /// </summary>
            public override string ToString()
            {
                return $"Match: '{Pattern}' at index {StartIndex}";
            }

            /// <summary>
            /// Determines whether the specified MatchResult is equal to the current MatchResult.
            /// </summary>
            public bool Equals(MatchResult other)
            {
                return StartIndex == other.StartIndex && Pattern == other.Pattern;
            }

            /// <summary>
            /// Determines whether the specified object is equal to the current MatchResult.
            /// </summary>
            public override bool Equals(object obj)
            {
                return obj is MatchResult other && Equals(other);
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            public override int GetHashCode()
            {
                return HashCode.Combine(StartIndex, Pattern);
            }
        }
    }
}