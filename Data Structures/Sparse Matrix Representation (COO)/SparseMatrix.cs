using System;
using System.Collections.Generic;

public class SparseMatrix
{
    private List<int> rows;
    private List<int> cols;
    private List<double> values;
    private int rowCount;
    private int colCount;

    public SparseMatrix(int rowCount, int colCount)
    {
        this.rowCount = rowCount;
        this.colCount = colCount;
        rows = new List<int>();
        cols = new List<int>();
        values = new List<double>();
    }

    public void AddEntry(int row, int col, double value)
    {
        if (row < 0 || row >= rowCount || col < 0 || col >= colCount)
        {
            throw new IndexOutOfRangeException("Row or column index is out of bounds.");
        }
        if (value != 0)
        {
            rows.Add(row);
            cols.Add(col);
            values.Add(value);
        }
    }

    public double GetValue(int row, int col)
    {
        if (row < 0 || row >= rowCount || col < 0 || col >= colCount)
        {
            throw new IndexOutOfRangeException("Row or column index is out of bounds.");
        }
        for (int i = 0; i < rows.Count; i++)
        {
            if (rows[i] == row && cols[i] == col)
            {
                return values[i];
            }
        }
        return 0; // Return 0 if the entry is not found
    }

    public double[,] ToDenseMatrix()
    {
        double[,] denseMatrix = new double[rowCount, colCount];
        for (int i = 0; i < rows.Count; i++)
        {
            denseMatrix[rows[i], cols[i]] = values[i];
        }
        return denseMatrix;
    }
}