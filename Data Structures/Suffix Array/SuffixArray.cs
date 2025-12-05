using System;
using System.Collections.Generic;

public class SuffixArray
{
    private readonly string _text;
    private readonly int[] _suffixArray;

    public SuffixArray(string text)
    {
        _text = text ?? throw new ArgumentNullException(nameof(text));
        _suffixArray = BuildSuffixArray(_text);
    }

    public int[] GetSuffixArray()
    {
        // Return a copy to prevent external modification
        int[] result = new int[_suffixArray.Length];
        Array.Copy(_suffixArray, result, _suffixArray.Length);
        return result;
    }

    private static int[] BuildSuffixArray(string s)
    {
        int n = s.Length;
        int[] suffixArray = new int[n];
        int[] rank = new int[n];
        int[] tempRank = new int[n];

        // Initialize suffix array and rank for single characters
        for (int i = 0; i < n; i++)
        {
            suffixArray[i] = i;
            rank[i] = s[i];
        }

        for (int length = 1; length < n; length <<= 1)
        {
            Array.Sort(suffixArray, (a, b) =>
            {
                if (rank[a] != rank[b])
                    return rank[a] - rank[b];

                int rankA = a + length < n ? rank[a + length] : -1;
                int rankB = b + length < n ? rank[b + length] : -1;

                return rankA - rankB;
            });

            tempRank[suffixArray[0]] = 0;
            for (int i = 1; i < n; i++)
            {
                bool samePrev = rank[suffixArray[i]] == rank[suffixArray[i - 1]];
                bool sameNext = (suffixArray[i] + length < n && suffixArray[i - 1] + length < n) &&
                                (rank[suffixArray[i] + length] == rank[suffixArray[i - 1] + length]);

                tempRank[suffixArray[i]] = tempRank[suffixArray[i - 1]] + (samePrev && sameNext ? 0 : 1);
            }

            var swap = rank;
            rank = tempRank;
            tempRank = swap;

            if (rank[suffixArray[n - 1]] == n - 1)
                break;
        }

        return suffixArray;
    }
}