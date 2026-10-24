using System;

public static class FloodFillSolver
{
    /// <summary>
    /// Implements the Flood Fill algorithm using recursion.
    /// </summary>
    /// <param name="image">A 2D character array representing the grid or image.</param>
    /// <param name="startRow">The row index of the starting point.</param>
    /// <param name="startCol">The column index of the starting point.</param>
    /// <param name="newColor">The new character value (color) to fill the region with.</param>
    public static void FloodFill(char[,] image, int startRow, int startCol, char newColor)
    {
        if (image == null || image.Length == 0)
        {
            return;
        }

        int numRows = image.GetLength(0);
        int numCols = image.GetLength(1);

        // Validate start coordinates
        if (startRow < 0 || startRow >= numRows || startCol < 0 || startCol >= numCols)
        {
            return;
        }

        char originalColor = image[startRow, startCol];

        // If the starting color is already the new color, no fill is needed.
        if (originalColor == newColor)
        {
            return;
        }

        FillRecursive(image, startRow, startCol, originalColor, newColor, numRows, numCols);
    }

    /// <summary>
    /// Recursive helper function for the Flood Fill algorithm.
    /// </summary>
    private static void FillRecursive(char[,] image, int row, int col, char originalColor, char newColor, int numRows, int numCols)
    {
        // Base cases for recursion:
        // 1. Out of bounds
        // 2. Current cell's color is not the original color (already visited or different region)
        if (row < 0 || row >= numRows || col < 0 || col >= numCols || image[row, col] != originalColor)
        {
            return;
        }

        // Change the color of the current cell
        image[row, col] = newColor;

        // Recursively call for adjacent cells (4-directional)
        FillRecursive(image, row + 1, col, originalColor, newColor, numRows, numCols); // Down
        FillRecursive(image, row - 1, col, originalColor, newColor, numRows, numCols); // Up
        FillRecursive(image, row, col + 1, originalColor, newColor, numRows, numCols); // Right
        FillRecursive(image, row, col - 1, originalColor, newColor, numRows, numCols); // Left
    }
}