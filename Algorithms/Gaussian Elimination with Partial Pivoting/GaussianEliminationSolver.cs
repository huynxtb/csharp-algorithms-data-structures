using System;

namespace Algorithms.Numerical
{
    /// <summary>
    /// Provides methods for solving systems of linear equations using numerical algorithms.
    /// </summary>
    public static class GaussianEliminationSolver
    {
        /// <summary>
        /// Solves a system of linear equations Ax = B using Gaussian Elimination with Partial Pivoting.
        /// </summary>
        /// <param name="coefficients">The square coefficient matrix A of size N x N.</param>
        /// <param name="constants">The constant vector B of size N.</param>
        /// <returns>The solution vector x of size N.</returns>
        /// <exception cref="ArgumentNullException">Thrown when coefficients or constants are null.</exception>
        /// <exception cref="ArgumentException">Thrown when the coefficient matrix is not square or dimensions do not match.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the system has no unique solution (singular or poorly conditioned matrix).</exception>
        public static double[] Solve(double[,] coefficients, double[] constants)
        {
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients));
            }
            if (constants == null)
            {
                throw new ArgumentNullException(nameof(constants));
            }

            int n = coefficients.GetLength(0);
            if (coefficients.GetLength(1) != n)
            {
                throw new ArgumentException("Coefficients matrix must be square.", nameof(coefficients));
            }
            if (constants.Length != n)
            {
                throw new ArgumentException("Constants vector length must match the number of rows in the coefficients matrix.", nameof(constants));
            }

            // Construct the augmented matrix [A | B]
            double[,] augmented = new double[n, n + 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = coefficients[i, j];
                }
                augmented[i, n] = constants[i];
            }

            const double tolerance = 1e-9;

            // Forward Elimination with Partial Pivoting
            for (int i = 0; i < n; i++)
            {
                // Find the pivot row with the largest absolute value in the current column
                int pivotRow = i;
                double maxVal = Math.Abs(augmented[i, i]);
                for (int r = i + 1; r < n; r++)
                {
                    double val = Math.Abs(augmented[r, i]);
                    if (val > maxVal)
                    {
                        maxVal = val;
                        pivotRow = r;
                    }
                }

                // Check for singularity
                if (maxVal < tolerance)
                {
                    throw new InvalidOperationException("The system of equations has no unique solution (matrix is singular or poorly conditioned).");
                }

                // Swap current row with pivot row if necessary
                if (pivotRow != i)
                {
                    for (int c = i; c <= n; c++)
                    {
                        double temp = augmented[i, c];
                        augmented[i, c] = augmented[pivotRow, c];
                        augmented[pivotRow, c] = temp;
                    }
                }

                // Eliminate entries below the pivot
                for (int r = i + 1; r < n; r++)
                {
                    double factor = augmented[r, i] / augmented[i, i];
                    for (int c = i; c <= n; c++)
                    {
                        augmented[r, c] -= factor * augmented[i, c];
                    }
                }
            }

            // Back Substitution
            double[] solution = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = augmented[i, n];
                for (int j = i + 1; j < n; j++)
                {
                    sum -= augmented[i, j] * solution[j];
                }
                solution[i] = sum / augmented[i, i];
            }

            return solution;
        }
    }
}