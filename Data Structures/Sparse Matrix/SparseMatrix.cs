using System;
using System.Collections.Generic;

public class SparseMatrix
{
    private Dictionary<(int, int), int> elements;
    private int rows;
    private int cols;

    public SparseMatrix(int rows, int cols)
    {
        if (rows <= 0 || cols <= 0)
            throw new ArgumentOutOfRangeException("Dimensions must be positive.");
        this.rows = rows;
        this.cols = cols;
        elements = new Dictionary<(int, int), int>();
    }

    public void SetValue(int row, int col, int value)
    {
        if (row < 0 || row >= rows || col < 0 || col >= cols)
            throw new ArgumentOutOfRangeException("Row or column is out of bounds.");
        if (value != 0)
            elements[(row, col)] = value;
        else
            elements.Remove((row, col));
    }

    public int GetValue(int row, int col)
    {
        if (row < 0 || row >= rows || col < 0 || col >= cols)
            throw new ArgumentOutOfRangeException("Row or column is out of bounds.");
        return elements.TryGetValue((row, col), out int value) ? value : 0;
    }

    public SparseMatrix Add(SparseMatrix other)
    {
        if (this.rows != other.rows || this.cols != other.cols)
            throw new ArgumentException("Matrices dimensions must match for addition.");
        SparseMatrix result = new SparseMatrix(this.rows, this.cols);
        foreach (var element in this.elements)
        {
            result.SetValue(element.Key.Item1, element.Key.Item2, element.Value);
        }
        foreach (var element in other.elements)
        {
            int newValue = result.GetValue(element.Key.Item1, element.Key.Item2) + element.Value;
            result.SetValue(element.Key.Item1, element.Key.Item2, newValue);
        }
        return result;
    }

    public SparseMatrix Multiply(SparseMatrix other)
    {
        if (this.cols != other.rows)
            throw new ArgumentException("Matrices dimensions must match for multiplication.");
        SparseMatrix result = new SparseMatrix(this.rows, other.cols);
        foreach (var elementA in this.elements)
        {
            int rowA = elementA.Key.Item1;
            int colA = elementA.Key.Item2;
            foreach (var elementB in other.elements)
            {
                int rowB = elementB.Key.Item1;
                int colB = elementB.Key.Item2;
                if (colA == rowB)
                {
                    int newValue = result.GetValue(rowA, colB) + elementA.Value * elementB.Value;
                    result.SetValue(rowA, colB, newValue);
                }
            }
        }
        return result;
    }

    public List<(int row, int col, int value)> GetNonZeroElements()
    {
        List<(int row, int col, int value)> nonZeroElements = new List<(int, int, int)>();
        foreach (var element in elements)
        {
            nonZeroElements.Add((element.Key.Item1, element.Key.Item2, element.Value));
        }
        return nonZeroElements;
    }
}