using System;
using System.Collections.Generic;

namespace Algorithms
{
    public enum EditOperation
    {
        Keep,
        Insert,
        Delete,
        Substitute
    }

    public readonly struct EditStep<T>
    {
        public EditOperation Operation { get; }
        public T SourceElement { get; }
        public T TargetElement { get; }
        public int SourceIndex { get; }

        public EditStep(EditOperation operation, T sourceElement, T targetElement, int sourceIndex)
        {
            Operation = operation;
            SourceElement = sourceElement;
            TargetElement = targetElement;
            SourceIndex = sourceIndex;
        }
    }

    public class LevenshteinResult<T>
    {
        public int Distance { get; }
        public IReadOnlyList<EditStep<T>> Steps { get; }

        public LevenshteinResult(int distance, IReadOnlyList<EditStep<T>> steps)
        {
            Distance = distance;
            Steps = steps;
        }
    }

    public static class Levenshtein
    {
        public static LevenshteinResult<T> Compute<T>(
            IEnumerable<T> source,
            IEnumerable<T> target,
            IEqualityComparer<T> comparer = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));

            comparer ??= EqualityComparer<T>.Default;

            var sList = source as IReadOnlyList<T> ?? new List<T>(source);
            var tList = target as IReadOnlyList<T> ?? new List<T>(target);

            int m = sList.Count;
            int n = tList.Count;

            int[,] dp = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
            {
                dp[i, 0] = i;
            }
            for (int j = 0; j <= n; j++)
            {
                dp[0, j] = j;
            }

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (comparer.Equals(sList[i - 1], tList[j - 1]))
                    {
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                    else
                    {
                        int deleteCost = dp[i - 1, j] + 1;
                        int insertCost = dp[i, j - 1] + 1;
                        int substituteCost = dp[i - 1, j - 1] + 1;

                        dp[i, j] = Math.Min(substituteCost, Math.Min(deleteCost, insertCost));
                    }
                }
            }

            int dist = dp[m, n];
            var steps = new List<EditStep<T>>();
            int currI = m;
            int currJ = n;

            while (currI > 0 || currJ > 0)
            {
                if (currI > 0 && currJ > 0 && comparer.Equals(sList[currI - 1], tList[currJ - 1]))
                {
                    steps.Add(new EditStep<T>(EditOperation.Keep, sList[currI - 1], tList[currJ - 1], currI - 1));
                    currI--;
                    currJ--;
                }
                else if (currI > 0 && currJ > 0 && dp[currI, currJ] == dp[currI - 1, currJ - 1] + 1)
                {
                    steps.Add(new EditStep<T>(EditOperation.Substitute, sList[currI - 1], tList[currJ - 1], currI - 1));
                    currI--;
                    currJ--;
                }
                else if (currI > 0 && dp[currI, currJ] == dp[currI - 1, currJ] + 1)
                {
                    steps.Add(new EditStep<T>(EditOperation.Delete, sList[currI - 1], default, currI - 1));
                    currI--;
                }
                else if (currJ > 0 && dp[currI, currJ] == dp[currI, currJ - 1] + 1)
                {
                    steps.Add(new EditStep<T>(EditOperation.Insert, default, tList[currJ - 1], currI));
                    currJ--;
                }
                else
                {
                    throw new InvalidOperationException("Invalid state encountered during backtracking.");
                }
            }

            steps.Reverse();
            return new LevenshteinResult<T>(dist, steps);
        }
    }
}