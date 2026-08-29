using System;
using System.Collections.Generic;

/// <summary>
/// Provides an optimized implementation of the Gale-Shapley algorithm for solving the Stable Matching Problem.
/// </summary>
public static class StableMatchingSolver
{
    /// <summary>
    /// Solves the Stable Matching Problem for an equal number of men and women.
    /// </summary>
    /// <param name="menPreferences">An N x N array where menPreferences[i] lists women indices in order of preference for man i.</param>
    /// <param name="womenPreferences">An N x N array where womenPreferences[j] lists men indices in order of preference for woman j.</param>
    /// <returns>A dictionary mapping each man's index to his stably matched woman's index.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either input array is null.</exception>
    /// <exception cref="ArgumentException">Thrown when input arrays are not square matrices of equal size, or contain invalid/duplicate indices.</exception>
    public static Dictionary<int, int> Solve(int[][] menPreferences, int[][] womenPreferences)
    {
        if (menPreferences == null)
        {
            throw new ArgumentNullException(nameof(menPreferences));
        }
        if (womenPreferences == null)
        {
            throw new ArgumentNullException(nameof(womenPreferences));
        }

        int n = menPreferences.Length;
        if (womenPreferences.Length != n)
        {
            throw new ArgumentException("The number of men and women must be equal.");
        }

        for (int i = 0; i < n; i++)
        {
            if (menPreferences[i] == null)
            {
                throw new ArgumentException($"Men preference list at index {i} is null.");
            }
            if (menPreferences[i].Length != n)
            {
                throw new ArgumentException($"Men preference list at index {i} must have length {n}.");
            }
            if (womenPreferences[i] == null)
            {
                throw new ArgumentException($"Women preference list at index {i} is null.");
            }
            if (womenPreferences[i].Length != n)
            {
                throw new ArgumentException($"Women preference list at index {i} must have length {n}.");
            }
        }

        for (int i = 0; i < n; i++)
        {
            bool[] seenWomen = new bool[n];
            bool[] seenMen = new bool[n];
            for (int j = 0; j < n; j++)
            {
                int w = menPreferences[i][j];
                if (w < 0 || w >= n)
                {
                    throw new ArgumentException($"Invalid woman index {w} in man {i}'s preference list.");
                }
                if (seenWomen[w])
                {
                    throw new ArgumentException($"Duplicate woman index {w} in man {i}'s preference list.");
                }
                seenWomen[w] = true;

                int m = womenPreferences[i][j];
                if (m < 0 || m >= n)
                {
                    throw new ArgumentException($"Invalid man index {m} in woman {i}'s preference list.");
                }
                if (seenMen[m])
                {
                    throw new ArgumentException($"Duplicate man index {m} in woman {i}'s preference list.");
                }
                seenMen[m] = true;
            }
        }

        int[][] womenRanking = new int[n][];
        for (int w = 0; w < n; w++)
        {
            womenRanking[w] = new int[n];
            for (int rank = 0; rank < n; rank++)
            {
                int m = womenPreferences[w][rank];
                womenRanking[w][m] = rank;
            }
        }

        int[] husbandOf = new int[n];
        for (int i = 0; i < n; i++)
        {
            husbandOf[i] = -1;
        }

        int[] nextProposal = new int[n];
        Queue<int> freeMen = new Queue<int>(n);
        for (int m = 0; m < n; m++)
        {
            freeMen.Enqueue(m);
        }

        while (freeMen.Count > 0)
        {
            int m = freeMen.Dequeue();
            int w = menPreferences[m][nextProposal[m]];
            nextProposal[m]++;

            int currentHusband = husbandOf[w];
            if (currentHusband == -1)
            {
                husbandOf[w] = m;
            }
            else
            {
                if (womenRanking[w][m] < womenRanking[w][currentHusband])
                {
                    husbandOf[w] = m;
                    freeMen.Enqueue(currentHusband);
                }
                else
                {
                    freeMen.Enqueue(m);
                }
            }
        }

        Dictionary<int, int> matches = new Dictionary<int, int>(n);
        for (int w = 0; w < n; w++)
        {
            matches[husbandOf[w]] = w;
        }

        return matches;
    }
}