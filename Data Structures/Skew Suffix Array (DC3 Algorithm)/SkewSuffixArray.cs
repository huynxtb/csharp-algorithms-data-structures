using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Implements the Skew (DC3) algorithm for constructing suffix arrays in linear time.
/// </summary>
public class SkewSuffixArray
{
    private readonly string _text;
    private readonly int[] _suffixArray;
    private int[] _rank;

    /// <summary>
    /// Gets the original input string.
    /// </summary>
    public string Text => _text;

    /// <summary>
    /// Gets the constructed suffix array.
    /// </summary>
    public int[] SuffixArray => _suffixArray;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkewSuffixArray"/> class.
    /// </summary>
    /// <param name="text">The input string for which to construct the suffix array.</param>
    public SkewSuffixArray(string text)
    {
        _text = text ?? throw new ArgumentNullException(nameof(text));
        _suffixArray = ConstructSuffixArray(text);
        _rank = ComputeRankArray(_suffixArray, text.Length);
    }

    /// <summary>
    /// Constructs the suffix array for the given text using the Skew (DC3) algorithm.
    /// </summary>
    /// <param name="text">The input string.</param>
    /// <returns>The suffix array.</returns>
    private static int[] ConstructSuffixArray(string text)
    {
        int n = text.Length;
        if (n == 0) return new int[0];
        if (n == 1) return new int[] { 0 };

        // Pad the text with three sentinel characters (smaller than any character in the alphabet)
        // This simplifies boundary conditions and ensures all triplets are well-defined.
        // We use 0 as the sentinel value, assuming characters are mapped to positive integers.
        int[] t = new int[n + 3];
        for (int i = 0; i < n; i++) t[i] = text[i];
        t[n] = t[n + 1] = t[n + 2] = 0;

        // Map characters to ranks (alphabet transformation)
        int alphabetSize = MapToRanks(t, n);

        // Recursively construct the suffix array for suffixes starting at positions 1 and 2 mod 3.
        // These are the 'non-divisible' suffixes.
        int[] sa12 = ConstructSuffixArrayRecursive(t, n, alphabetSize);

        // Construct the suffix array for suffixes starting at positions 0 mod 3.
        // These are the 'divisible' suffixes.
        int[] sa0 = ConstructSuffixArray0(t, n, sa12);

        // Merge the two sorted suffix arrays.
        return MergeSuffixArrays(t, n, sa0, sa12);
    }

    /// <summary>
    /// Maps characters in the text to ranks (integers) to simplify sorting.
    /// </summary>
    /// <param name="t">The integer representation of the text with padding.</param>
    /// <param name="n">The original length of the text.</param>
    /// <returns>The size of the alphabet (number of unique ranks).</returns>
    private static int MapToRanks(int[] t, int n)
    {
        // Collect unique characters and sort them to assign ranks.
        var uniqueChars = new SortedSet<int>();
        for (int i = 0; i < n; i++) uniqueChars.Add(t[i]);

        var rankMap = new Dictionary<int, int>();
        int rank = 1; // Start ranks from 1 to reserve 0 for sentinel
        foreach (var c in uniqueChars)
        {
            rankMap[c] = rank++;
        }

        // Update the text array with ranks.
        for (int i = 0; i < n; i++) t[i] = rankMap[t[i]];
        return rankMap.Count + 1; // +1 for the sentinel character
    }

    /// <summary>
    /// Recursively constructs the suffix array for suffixes starting at positions 1 and 2 mod 3.
    /// </summary>
    /// <param name="t">The integer representation of the text with padding.</param>
    /// <param name="n">The original length of the text.</param>
    /// <param name="alphabetSize">The current alphabet size.</param>
    /// <returns>The suffix array for suffixes starting at 1 and 2 mod 3.</returns>
    private static int[] ConstructSuffixArrayRecursive(int[] t, int n, int alphabetSize)
    {
        int n12 = (n + 2) / 3;
        int n0 = (n + 1) / 3;
        int n1 = n0;
        int n2 = n12 - n1;

        // Create arrays for suffixes starting at 1 and 2 mod 3.
        // We combine them into a single array for recursive sorting.
        int[] s12 = new int[n12];
        int k = 0;
        for (int i = 0; i < n; i++) if (i % 3 != 0) s12[k++] = i;

        // Sort the triplets (t[i], t[i+1], t[i+2]) for suffixes in s12.
        // This is the core of the recursive step.
        RadixSortTriplets(t, s12, n12, alphabetSize);

        // Assign ranks to the sorted triplets.
        // If two triplets are identical, they get the same rank.
        int[] name = new int[n12];
        int c = 0;
        name[s12[0] / 3 + (s12[0] % 3 == 1 ? 0 : n1)] = c;
        for (int i = 1; i < n12; i++)
        {
            if (!AreTripletsEqual(t, s12[i - 1], s12[i])) c++;
            name[s12[i] / 3 + (s12[i] % 3 == 1 ? 0 : n1)] = c;
        }
        c++;

        int[] sa12;
        if (c < n12) // If there are duplicate triplets, recurse.
        {
            sa12 = ConstructSuffixArrayRecursive(name, n12, c);
        }
        else // Otherwise, the ranks are unique, and we have the sorted order.
        {
            sa12 = new int[n12];
            for (int i = 0; i < n12; i++) sa12[name[i]] = i;
        }

        // Convert the suffix array of ranks back to original indices.
        int[] finalSa12 = new int[n12];
        for (int i = 0; i < n12; i++)
        {
            int idx = sa12[i];
            if (idx < n1) finalSa12[i] = 3 * idx + 1;
            else finalSa12[i] = 3 * (idx - n1) + 2;
        }

        return finalSa12;
    }

    /// <summary>
    /// Constructs the suffix array for suffixes starting at positions 0 mod 3.
    /// </summary>
    /// <param name="t">The integer representation of the text with padding.</param>
    /// <param name="n">The original length of the text.</param>
    /// <param name="sa12">The sorted suffix array for positions 1 and 2 mod 3.</param>
    /// <returns>The suffix array for suffixes starting at 0 mod 3.</returns>
    private static int[] ConstructSuffixArray0(int[] t, int n, int[] sa12)
    {
        int n0 = (n + 1) / 3;
        int[] sa0 = new int[n0];
        int[] s0 = new int[n0];
        int k = 0;
        for (int i = 0; i < n; i++) if (i % 3 == 0) s0[k++] = i;

        // Create the inverse suffix array for sa12 to quickly find ranks.
        int[] rank12 = new int[n + 3];
        for (int i = 0; i < sa12.Length; i++) rank12[sa12[i]] = i + 1;

        // Sort suffixes starting at 0 mod 3 based on their first character and the rank of the suffix starting at i+1.
        // This is done by sorting pairs (t[i], rank12[i+1]).
        RadixSortPairs(t, s0, n0, n + 1, rank12);

        // The sorted s0 is now sa0.
        for (int i = 0; i < n0; i++) sa0[i] = s0[i];
        return sa0;
    }

    /// <summary>
    /// Merges the sorted suffix arrays for positions 0 mod 3 and 1, 2 mod 3.
    /// </summary>
    /// <param name="t">The integer representation of the text with padding.</param>
    /// <param name="n">The original length of the text.</param>
    /// <param name="sa0">The sorted suffix array for positions 0 mod 3.</param>
    /// <param name="sa12">The sorted suffix array for positions 1 and 2 mod 3.</param>
    /// <returns>The complete sorted suffix array.</returns>
    private static int[] MergeSuffixArrays(int[] t, int n, int[] sa0, int[] sa12)
    {
        int[] sa = new int[n];
        int p = 0, p0 = 0, p12 = 0;
        int n0 = sa0.Length;
        int n12 = sa12.Length;

        // Create the inverse suffix array for sa12 to quickly find ranks.
        int[] rank12 = new int[n + 3];
        for (int i = 0; i < n12; i++) rank12[sa12[i]] = i + 1;

        while (p0 < n0 && p12 < n12)
        {
            int i = sa0[p0];
            int j = sa12[p12];

            bool suffix1IsSmaller;
            if (j % 3 == 1) // Suffix j starts at 1 mod 3
            {
                // Compare (t[i], rank12[i+1]) with (t[j], rank12[j+1])
                suffix1IsSmaller = (t[i] < t[j]) || (t[i] == t[j] && rank12[i + 1] < rank12[j + 1]);
            }
            else // Suffix j starts at 2 mod 3
            {
                // Compare (t[i], t[i+1], rank12[i+2]) with (t[j], t[j+1], rank12[j+2])
                suffix1IsSmaller = (t[i] < t[j]) ||
                                   (t[i] == t[j] && t[i + 1] < t[j + 1]) ||
                                   (t[i] == t[j] && t[i + 1] == t[j + 1] && rank12[i + 2] < rank12[j + 2]);
            }

            if (suffix1IsSmaller)
            {
                sa[p++] = i;
                p0++;
            }
            else
            {
                sa[p++] = j;
                p12++;
            }
        }

        // Append remaining suffixes.
        while (p0 < n0) sa[p++] = sa0[p0++];
        while (p12 < n12) sa[p++] = sa12[p12++];

        return sa;
    }

    /// <summary>
    /// Checks if two triplets starting at indices i and j in text t are equal.
    /// </summary>
    private static bool AreTripletsEqual(int[] t, int i, int j)
    {
        return t[i] == t[j] && t[i + 1] == t[j + 1] && t[i + 2] == t[j + 2];
    }

    /// <summary>
    /// Performs radix sort on an array of indices based on triplets in the text.
    /// </summary>
    /// <param name="t">The integer representation of the text.</param>
    /// <param name="indices">The array of indices to sort.</param>
    /// <param name="n">The number of indices to sort.</param>
    /// <param name="alphabetSize">The maximum value in the alphabet.</param>
    private static void RadixSortTriplets(int[] t, int[] indices, int n, int alphabetSize)
    {
        // Sort by the third character (t[i+2])
        RadixSortHelper(t, indices, n, alphabetSize, 2);
        // Sort by the second character (t[i+1])
        RadixSortHelper(t, indices, n, alphabetSize, 1);
        // Sort by the first character (t[i])
        RadixSortHelper(t, indices, n, alphabetSize, 0);
    }

    /// <summary>
    /// Performs radix sort on an array of indices based on pairs in the text.
    /// </summary>
    /// <param name="t">The integer representation of the text.</param>
    /// <param name="indices">The array of indices to sort.</param>
    /// <param name="n">The number of indices to sort.</param>
    /// <param name="alphabetSize">The maximum value in the alphabet.</param>
    /// <param name="rank12">The rank array for suffixes starting at i+1.</param>
    private static void RadixSortPairs(int[] t, int[] indices, int n, int alphabetSize, int[] rank12)
    {
        // Sort by the second element of the pair (rank12[i+1])
        RadixSortHelperPairs(t, indices, n, alphabetSize, rank12, 1);
        // Sort by the first element of the pair (t[i])
        RadixSortHelperPairs(t, indices, n, alphabetSize, rank12, 0);
    }

    /// <summary>
    /// Helper for radix sort on triplets.
    /// </summary>
    /// <param name="t">The integer representation of the text.</param>
    /// <param name="indices">The array of indices to sort.</param>
    /// <param name="n">The number of indices to sort.</param>
    /// <param name="alphabetSize">The maximum value in the alphabet.</param>
    /// <param name="offset">The offset for the character to sort by (0, 1, or 2).</param>
    private static void RadixSortHelper(int[] t, int[] indices, int n, int alphabetSize, int offset)
    {
        int[] count = new int[alphabetSize + 1];
        int[] tempIndices = new int[n];

        // Count occurrences of each character
        for (int i = 0; i < n; i++) count[t[indices[i] + offset]]++;

        // Compute cumulative counts
        for (int i = 1; i <= alphabetSize; i++) count[i] += count[i - 1];

        // Place indices in sorted order
        for (int i = n - 1; i >= 0; i--)
        {
            tempIndices[--count[t[indices[i] + offset]]] = indices[i];
        }

        // Copy sorted indices back
        Array.Copy(tempIndices, indices, n);
    }

    /// <summary>
    /// Helper for radix sort on pairs.
    /// </summary>
    /// <param name="t">The integer representation of the text.</param>
    /// <param name="indices">The array of indices to sort.</param>
    /// <param name="n">The number of indices to sort.</param>
    /// <param name="alphabetSize">The maximum value in the alphabet.</param>
    /// <param name="rank12">The rank array for suffixes starting at i+1.</param>
    /// <param name="pairElement">0 for t[i], 1 for rank12[i+1].</param>
    private static void RadixSortHelperPairs(int[] t, int[] indices, int n, int alphabetSize, int[] rank12, int pairElement)
    {
        int[] count = new int[alphabetSize + 1];
        int[] tempIndices = new int[n];
        int maxRank = 0;
        for(int i = 0; i < n; i++) {
            int idx = indices[i];
            int val = (pairElement == 0) ? t[idx] : rank12[idx + 1];
            if (val > maxRank) maxRank = val;
        }

        // Count occurrences of each value
        for (int i = 0; i < n; i++)
        {
            int idx = indices[i];
            int val = (pairElement == 0) ? t[idx] : rank12[idx + 1];
            count[val]++;
        }

        // Compute cumulative counts
        for (int i = 1; i <= maxRank; i++) count[i] += count[i - 1];

        // Place indices in sorted order
        for (int i = n - 1; i >= 0; i--)
        {
            int idx = indices[i];
            int val = (pairElement == 0) ? t[idx] : rank12[idx + 1];
            tempIndices[--count[val]] = idx;
        }

        // Copy sorted indices back
        Array.Copy(tempIndices, indices, n);
    }

    /// <summary>
    /// Computes the rank array (inverse suffix array).
    /// </summary>
    /// <param name="sa">The suffix array.</param>
    /// <param name="n">The length of the text.</param>
    /// <returns>The rank array.</returns>
    private static int[] ComputeRankArray(int[] sa, int n)
    {
        int[] rank = new int[n];
        for (int i = 0; i < n; i++) rank[sa[i]] = i;
        return rank;
    }

    /// <summary>
    /// Computes the Longest Common Prefix (LCP) array using Kasai's algorithm.
    /// </summary>
    /// <returns>The LCP array.</returns>
    public int[] GetLCPArray()
    {
        int n = _text.Length;
        if (n == 0) return new int[0];

        int[] lcp = new int[n];
        int k = 0; // Length of the previous LCP

        // Iterate through the text, computing LCP for each suffix based on its predecessor in the suffix array.
        for (int i = 0; i < n; i++)
        {
            // If the current suffix is the first in the suffix array, its LCP is 0.
            if (_rank[i] == 0) continue;

            // Get the index of the suffix that precedes the current suffix in the sorted suffix array.
            int j = _suffixArray[_rank[i] - 1];

            // Extend the LCP from the previous suffix.
            // We know that LCP(i, j) >= LCP(i-1, j') - 1, where j' is the suffix preceding i-1.
            while (i + k < n && j + k < n && _text[i + k] == _text[j + k])
            {
                k++;
            }

            lcp[_rank[i]] = k;

            // Decrease k for the next iteration. If k > 0, the next LCP will be at least k-1.
            if (k > 0) k--;
        }

        return lcp;
    }

    /// <summary>
    /// Checks if a pattern exists in the text using binary search on the suffix array.
    /// </summary>
    /// <param name="pattern">The pattern to search for.</param>
    /// <returns>True if the pattern is found, false otherwise.</returns>
    public bool Contains(string pattern)
    {
        return FindAllOccurrences(pattern).Count > 0;
    }

    /// <summary>
    /// Finds all starting indices where a pattern occurs in the text.
    /// </summary>
    /// <param name="pattern">The pattern to search for.</param>
    /// <returns>A list of starting indices of the pattern occurrences.</returns>
    public List<int> FindAllOccurrences(string pattern)
    {
        List<int> occurrences = new List<int>();
        int n = _text.Length;
        int m = pattern.Length;
        if (m == 0 || n == 0 || m > n) return occurrences;

        int low = 0;
        int high = n - 1;
        int firstOccurrence = -1;

        // Binary search for the first occurrence
        while (low <= high)
        {
            int mid = low + (high - low) / 2;
            int cmp = string.Compare(_text.Substring(_suffixArray[mid]), 0, pattern, 0, Math.Min(m, n - _suffixArray[mid]));

            if (cmp == 0)
            {
                // Potential match, try to find the earliest one
                firstOccurrence = mid;
                high = mid - 1;
            }
            else if (cmp < 0)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        if (firstOccurrence == -1) return occurrences; // Pattern not found

        // Found at least one occurrence, now find all contiguous occurrences
        // Search left from firstOccurrence
        low = firstOccurrence;
        while (low >= 0)
        {
            int cmp = string.Compare(_text.Substring(_suffixArray[low]), 0, pattern, 0, Math.Min(m, n - _suffixArray[low]));
            if (cmp == 0) {
                occurrences.Add(_suffixArray[low]);
                low--;
            } else {
                break;
            }
        }

        // Search right from firstOccurrence + 1
        high = firstOccurrence + 1;
        while (high < n)
        {
            int cmp = string.Compare(_text.Substring(_suffixArray[high]), 0, pattern, 0, Math.Min(m, n - _suffixArray[high]));
            if (cmp == 0) {
                occurrences.Add(_suffixArray[high]);
                high++;
            } else {
                break;
            }
        }

        occurrences.Sort(); // Ensure occurrences are in ascending order
        return occurrences;
    }
}