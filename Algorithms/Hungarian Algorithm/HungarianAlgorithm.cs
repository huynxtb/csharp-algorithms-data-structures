using System;

/// <summary>
/// Provides an implementation of the Hungarian Algorithm (Kuhn-Munkres) 
/// to solve the Assignment Problem in O(N^3) time complexity.
/// </summary>
public static class HungarianAlgorithm
{
    /// <summary>
    /// Finds the minimum weight assignment for a square cost matrix.
    /// </summary>
    /// <param name="costMatrix">A square 2D array representing the costs of assigning rows to columns.</param>
    /// <returns>An array where the value at index i is the column index assigned to row i.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the cost matrix is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the cost matrix is not square or is empty.</exception>
    public static int[] FindMinWeightAssignment(double[,] costMatrix)
    {
        if (costMatrix == null)
        {
            throw new ArgumentNullException(nameof(costMatrix), "Cost matrix cannot be null.");
        }

        int rows = costMatrix.GetLength(0);
        int cols = costMatrix.GetLength(1);

        if (rows != cols)
        {
            throw new ArgumentException("Cost matrix must be square (N x N).", nameof(costMatrix));
        }

        if (rows == 0)
        {
            throw new ArgumentException("Cost matrix cannot be empty.", nameof(costMatrix));
        }

        int n = rows;
        double[] u = new double[n + 1];
        double[] v = new double[n + 1];
        int[] p = new int[n + 1];
        int[] way = new int[n + 1];

        for (int i = 1; i <= n; i++)
        {
            p[0] = i;
            int j0 = 0;
            double[] minv = new double[n + 1];
            for (int j = 0; j <= n; j++)
            {
                minv[j] = double.MaxValue;
            }
            bool[] used = new bool[n + 1];

            do
            {
                used[j0] = true;
                int i0 = p[j0];
                double delta = double.MaxValue;
                int j1 = 0;

                for (int j = 1; j <= n; j++)
                {
                    if (!used[j])
                    {
                        double cur = costMatrix[i0 - 1, j - 1] - u[i0] - v[j];
                        if (cur < minv[j])
                        {
                            minv[j] = cur;
                            way[j] = j0;
                        }
                        if (minv[j] < delta)
                        {
                            delta = minv[j];
                            j1 = j;
                        }
                    }
                }

                for (int j = 0; j <= n; j++)
                {
                    if (used[j])
                    {
                        u[p[j]] += delta;
                        v[j] -= delta;
                    }
                    else
                    {
                        minv[j] -= delta;
                    }
                }

                j0 = j1;
            } while (p[j0] != 0);

            do
            {
                int j1 = way[j0];
                p[j0] = p[j1];
                j0 = j1;
            } while (j0 != 0);
        }

        int[] result = new int[n];
        for (int j = 1; j <= n; j++)
        {
            result[p[j] - 1] = j - 1;
        }

        return result;
    }
}