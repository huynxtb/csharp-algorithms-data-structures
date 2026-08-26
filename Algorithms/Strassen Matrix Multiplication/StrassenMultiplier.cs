using System;

namespace Algorithms.Matrix
{
    public static class StrassenMultiplier
    {
        private const int Threshold = 64;

        public static double[,] Multiply(double[,] matrixA, double[,] matrixB)
        {
            if (matrixA == null) throw new ArgumentNullException(nameof(matrixA));
            if (matrixB == null) throw new ArgumentNullException(nameof(matrixB));

            int rowsA = matrixA.GetLength(0);
            int colsA = matrixA.GetLength(1);
            int rowsB = matrixB.GetLength(0);
            int colsB = matrixB.GetLength(1);

            if (rowsA == 0 || colsA == 0 || rowsB == 0 || colsB == 0)
            {
                throw new ArgumentException("Matrices must not be empty.");
            }

            if (colsA != rowsB)
            {
                throw new ArgumentException("Matrix A columns must equal Matrix B rows.");
            }

            int maxDim = Math.Max(rowsA, Math.Max(colsA, colsB));
            int n = 1;
            while (n < maxDim)
            {
                n *= 2;
            }

            double[,] paddedA = PadMatrix(matrixA, rowsA, colsA, n);
            double[,] paddedB = PadMatrix(matrixB, rowsB, colsB, n);

            double[,] paddedResult = StrassenMultiplyRecursive(paddedA, paddedB);

            double[,] result = new double[rowsA, colsB];
            for (int i = 0; i < rowsA; i++)
            {
                for (int j = 0; j < colsB; j++)
                 {
                    result[i, j] = paddedResult[i, j];
                }
            }

            return result;
        }

        private static double[,] PadMatrix(double[,] matrix, int originalRows, int originalCols, int newSize)
        {
            double[,] padded = new double[newSize, newSize];
            for (int i = 0; i < originalRows; i++)
            {
                for (int j = 0; j < originalCols; j++)
                {
                    padded[i, j] = matrix[i, j];
                }
            }
            return padded;
        }

        private static double[,] StrassenMultiplyRecursive(double[,] A, double[,] B)
        {
            int n = A.GetLength(0);

            if (n <= Threshold)
            {
                return StandardMultiply(A, B);
            }

            int mid = n / 2;

            double[,] a11 = new double[mid, mid];
            double[,] a12 = new double[mid, mid];
            double[,] a21 = new double[mid, mid];
            double[,] a22 = new double[mid, mid];

            double[,] b11 = new double[mid, mid];
            double[,] b12 = new double[mid, mid];
            double[,] b21 = new double[mid, mid];
            double[,] b22 = new double[mid, mid];

            Split(A, a11, 0, 0);
            Split(A, a12, 0, mid);
            Split(A, a21, mid, 0);
            Split(A, a22, mid, mid);

            Split(B, b11, 0, 0);
            Split(B, b12, 0, mid);
            Split(B, b21, mid, 0);
            Split(B, b22, mid, mid);

            double[,] m1 = StrassenMultiplyRecursive(Add(a11, a22), Add(b11, b22));
            double[,] m2 = StrassenMultiplyRecursive(Add(a21, a22), b11);
            double[,] m3 = StrassenMultiplyRecursive(a11, Subtract(b12, b22));
            double[,] m4 = StrassenMultiplyRecursive(a22, Subtract(b21, b11));
            double[,] m5 = StrassenMultiplyRecursive(Add(a11, a12), b22);
            double[,] m6 = StrassenMultiplyRecursive(Subtract(a21, a11), Add(b11, b12));
            double[,] m7 = StrassenMultiplyRecursive(Subtract(a12, a22), Add(b21, b22));

            double[,] c11 = Add(Subtract(Add(m1, m4), m5), m7);
            double[,] c12 = Add(m3, m5);
            double[,] c21 = Add(m2, m4);
            double[,] c22 = Add(Add(Subtract(m1, m2), m3), m6);

            double[,] C = new double[n, n];
            Join(c11, C, 0, 0);
            Join(c12, C, 0, mid);
            Join(c21, C, mid, 0);
            Join(c22, C, mid, mid);

            return C;
        }

        private static double[,] StandardMultiply(double[,] A, double[,] B)
        {
            int n = A.GetLength(0);
            double[,] C = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int k = 0; k < n; k++)
                {
                    double temp = A[i, k];
                    for (int j = 0; j < n; j++)
                    {
                        C[i, j] += temp * B[k, j];
                    }
                }
            }
            return C;
        }

        private static double[,] Add(double[,] A, double[,] B)
        {
            int n = A.GetLength(0);
            double[,] C = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j] + B[i, j];
                }
            }
            return C;
        }

        private static double[,] Subtract(double[,] A, double[,] B)
        {
            int n = A.GetLength(0);
            double[,] C = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C[i, j] = A[i, j] - B[i, j];
                }
            }
            return C;
        }

        private static void Split(double[,] parent, double[,] child, int startRow, int startCol)
        {
            int n = child.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    child[i, j] = parent[startRow + i, startCol + j];
                }
            }
        }

        private static void Join(double[,] child, double[,] parent, int startRow, int startCol)
        {
            int n = child.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    parent[startRow + i, startCol + j] = child[i, j];
                }
            }
        }
    }
}