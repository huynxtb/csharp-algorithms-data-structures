using System;
using System.Text;

namespace SequenceAlignment
{
    public class NeedlemanWunsch
    {
        private readonly int _matchScore;
        private readonly int _mismatchPenalty;
        private readonly int _gapPenalty;

        public NeedlemanWunsch(int matchScore, int mismatchPenalty, int gapPenalty)
        {
            _matchScore = matchScore;
            _mismatchPenalty = mismatchPenalty;
            _gapPenalty = gapPenalty;
        }

        public AlignmentResult Align(string sequenceA, string sequenceB)
        {
            if (sequenceA == null) throw new ArgumentNullException(nameof(sequenceA));
            if (sequenceB == null) throw new ArgumentNullException(nameof(sequenceB));

            int n = sequenceA.Length;
            int m = sequenceB.Length;

            int[,] matrix = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++)
            {
                matrix[i, 0] = i * _gapPenalty;
            }
            for (int j = 0; j <= m; j++)
            {
                matrix[0, j] = j * _gapPenalty;
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int match = matrix[i - 1, j - 1] + (sequenceA[i - 1] == sequenceB[j - 1] ? _matchScore : _mismatchPenalty);
                    int delete = matrix[i - 1, j] + _gapPenalty;
                    int insert = matrix[i, j - 1] + _gapPenalty;

                    matrix[i, j] = Math.Max(match, Math.Max(delete, insert));
                }
            }

            StringBuilder alignedA = new StringBuilder();
            StringBuilder alignedB = new StringBuilder();

            int currI = n;
            int currJ = m;

            while (currI > 0 || currJ > 0)
            {
                if (currI > 0 && currJ > 0)
                {
                    int scoreDiag = sequenceA[currI - 1] == sequenceB[currJ - 1] ? _matchScore : _mismatchPenalty;
                    if (matrix[currI, currJ] == matrix[currI - 1, currJ - 1] + scoreDiag)
                    {
                        alignedA.Append(sequenceA[currI - 1]);
                        alignedB.Append(sequenceB[currJ - 1]);
                        currI--;
                        currJ--;
                        continue;
                    }
                }

                if (currI > 0 && matrix[currI, currJ] == matrix[currI - 1, currJ] + _gapPenalty)
                {
                    alignedA.Append(sequenceA[currI - 1]);
                    alignedB.Append('-');
                    currI--;
                }
                else if (currJ > 0)
                {
                    alignedA.Append('-');
                    alignedB.Append(sequenceB[currJ - 1]);
                    currJ--;
                }
            }

            char[] arrA = alignedA.ToString().ToCharArray();
            char[] arrB = alignedB.ToString().ToCharArray();
            Array.Reverse(arrA);
            Array.Reverse(arrB);

            return new AlignmentResult(new string(arrA), new string(arrB), matrix[n, m]);
        }
    }

    public struct AlignmentResult
    {
        public string AlignedSequenceA { get; }
        public string AlignedSequenceB { get; }
        public int Score { get; }

        public AlignmentResult(string alignedSequenceA, string alignedSequenceB, int score)
        {
            AlignedSequenceA = alignedSequenceA;
            AlignedSequenceB = alignedSequenceB;
            Score = score;
        }
    }
}