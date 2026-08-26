using System;
using System.Text;

/// <summary>
/// Provides methods for performing local sequence alignment using the Smith-Waterman algorithm.
/// </summary>
public class SmithWaterman
{
    /// <summary>
    /// Represents the result of the local sequence alignment.
    /// </summary>
    public class AlignmentResult
    {
        /// <summary>
        /// The optimal local alignment score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// The aligned portion of sequence A, including gaps represented by hyphens.
        /// </summary>
        public string AlignedSequenceA { get; set; }

        /// <summary>
        /// The aligned portion of sequence B, including gaps represented by hyphens.
        /// </summary>
        public string AlignedSequenceB { get; set; }

        /// <summary>
        /// The 0-based start index of the alignment in the original sequence A.
        /// </summary>
        public int StartIndexA { get; set; }

        /// <summary>
        /// The 0-based end index of the alignment in the original sequence A.
        /// </summary>
        public int EndIndexA { get; set; }

        /// <summary>
        /// The 0-based start index of the alignment in the original sequence B.
        /// </summary>
        public int StartIndexB { get; set; }

        /// <summary>
        /// The 0-based end index of the alignment in the original sequence B.
        /// </summary>
        public int EndIndexB { get; set; }
    }

    /// <summary>
    /// Aligns two sequences locally using the Smith-Waterman algorithm.
    /// </summary>
    /// <param name="seqA">The first sequence to align.</param>
    /// <param name="seqB">The second sequence to align.</param>
    /// <param name="matchScore">The score added for matching characters (typically positive).</param>
    /// <param name="mismatchPenalty">The penalty added for mismatching characters (typically negative).</param>
    /// <param name="gapPenalty">The penalty added for introducing a gap (typically negative).</param>
    /// <returns>An AlignmentResult containing the score, aligned sequences, and indices.</returns>
    public static AlignmentResult Align(string seqA, string seqB, int matchScore, int mismatchPenalty, int gapPenalty)
    {
        if (string.IsNullOrEmpty(seqA) || string.IsNullOrEmpty(seqB))
        {
            return new AlignmentResult
            {
                Score = 0,
                AlignedSequenceA = string.Empty,
                AlignedSequenceB = string.Empty,
                StartIndexA = -1,
                EndIndexA = -1,
                StartIndexB = -1,
                EndIndexB = -1
            };
        }

        int n = seqA.Length;
        int m = seqB.Length;
        int[,] h = new int[n + 1, m + 1];

        int maxScore = 0;
        int maxI = 0;
        int maxJ = 0;

        // Fill the dynamic programming scoring matrix
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int match = h[i - 1, j - 1] + (seqA[i - 1] == seqB[j - 1] ? matchScore : mismatchPenalty);
                int delete = h[i - 1, j] + gapPenalty;
                int insert = h[i, j - 1] + gapPenalty;

                h[i, j] = Math.Max(0, Math.Max(match, Math.Max(delete, insert)));

                if (h[i, j] > maxScore)
                {
                    maxScore = h[i, j];
                    maxI = i;
                    maxJ = j;
                }
            }
        }

        // If no alignment scores above 0, return empty alignment
        if (maxScore == 0)
        {
            return new AlignmentResult
            {
                Score = 0,
                AlignedSequenceA = string.Empty,
                AlignedSequenceB = string.Empty,
                StartIndexA = -1,
                EndIndexA = -1,
                StartIndexB = -1,
                EndIndexB = -1
            };
        }

        // Backtrack to find the alignment path
        StringBuilder alignedA = new StringBuilder();
        StringBuilder alignedB = new StringBuilder();

        int currI = maxI;
        int currJ = maxJ;

        while (currI > 0 && currJ > 0 && h[currI, currJ] > 0)
        {
            int score = h[currI, currJ];
            int scoreDiag = h[currI - 1, currJ - 1];
            int scoreUp = h[currI - 1, currJ];
            int scoreLeft = h[currI, currJ - 1];

            int stepScore = seqA[currI - 1] == seqB[currJ - 1] ? matchScore : mismatchPenalty;

            if (score == scoreDiag + stepScore)
            {
                alignedA.Append(seqA[currI - 1]);
                alignedB.Append(seqB[currJ - 1]);
                currI--;
                currJ--;
            }
            else if (score == scoreUp + gapPenalty)
            {
                alignedA.Append(seqA[currI - 1]);
                alignedB.Append('-');
                currI--;
            }
            else if (score == scoreLeft + gapPenalty)
            {
                alignedA.Append('-');
                alignedB.Append(seqB[currJ - 1]);
                currJ--;
            }
            else
            {
                break;
            }
        }

        // Reverse the aligned sequences since backtracking builds them backwards
        char[] arrA = alignedA.ToString().ToCharArray();
        Array.Reverse(arrA);
        char[] arrB = alignedB.ToString().ToCharArray();
        Array.Reverse(arrB);

        return new AlignmentResult
        {
            Score = maxScore,
            AlignedSequenceA = new string(arrA),
            AlignedSequenceB = new string(arrB),
            StartIndexA = currI,
            EndIndexA = maxI - 1,
            StartIndexB = currJ,
            EndIndexB = maxJ - 1
        };
    }
}