# Flood Fill Algorithm

## Introduction

The Flood Fill algorithm is a graph traversal algorithm used to identify and modify a connected region in a multi-dimensional array, most commonly a 2D grid or image. It's analogous to the "paint bucket" tool in graphics software. Starting from a specified seed point, it recursively or iteratively explores all adjacent cells that share the same initial "color" (value) and replaces them with a new "color". This is useful for tasks like filling areas, identifying connected components, or performing region-based operations.

## Usage

```csharp
using System;

public class ExampleUsage
{
    public static void Main(string[] args)
    {
        char[,] image = {
            {'R', 'R', 'G', 'B'},
            {'R', 'R', 'R', 'G'},
            {'G', 'R', 'R', 'R'},
            {'B', 'B', 'R', 'R'}
        };

        Console.WriteLine("Original Image:");
        PrintImage(image);

        // Fill the region starting at (1, 1) with 'Y'
        FloodFillSolver.FloodFill(image, 1, 1, 'Y');

        Console.WriteLine("\nImage after Flood Fill:");
        PrintImage(image);
    }

    public static void PrintImage(char[,] img)
    {
        for (int i = 0; i < img.GetLength(0); i++)
        {
            for (int j = 0; j < img.GetLength(1); j++)
            {
                Console.Write(img[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
```

## Detailed Explanation

The `FloodFillSolver` class contains a static method `FloodFill` that initiates the process. It takes a 2D character array `image`, the starting row `startRow`, the starting column `startCol`, and the `newColor` to fill with.

1.  **Input Validation**: The method first checks if the `image` is null or empty. It also validates that the `startRow` and `startCol` are within the bounds of the `image` array.
2.  **Color Check**: It retrieves the `originalColor` at the `startRow`, `startCol`. If this `originalColor` is already the same as `newColor`, there's nothing to fill, and the method returns early.
3.  **Recursive Filling**: The core logic is delegated to the private recursive helper method `FillRecursive`. This method performs the actual traversal and color replacement.
    *   **Base Cases**: The recursion stops if:
        *   The current `row` or `col` is out of the `image` bounds.
        *   The color of the current cell `image[row, col]` is not the `originalColor`. This condition implicitly handles already visited cells (which would have been changed to `newColor`) and cells that are part of a different colored region.
    *   **Color Change**: If the base cases are not met, the current cell `image[row, col]` is updated to `newColor`.
    *   **Recursive Calls**: The `FillRecursive` method then calls itself for the four adjacent cells: down (`row + 1`), up (`row - 1`), right (`col + 1`), and left (`col - 1`). This process continues until all connected cells with the `originalColor` have been visited and recolored.

This implementation uses the implicit call stack of recursion to manage the cells to be visited, avoiding explicit `Stack` or `Queue` data structures as per the requirements.

## Complexity Analysis

*   **Time Complexity**: O(M * N), where M is the number of rows and N is the number of columns in the `image`. In the worst case, every cell in the grid might be visited and processed exactly once.
*   **Space Complexity**: O(M * N) in the worst case due to the recursion depth. The call stack can grow up to the size of the connected component being filled. If the entire grid is one color, the recursion depth could be proportional to the total number of cells.