# Sparse Matrix

## Introduction
A Sparse Matrix is a data structure that efficiently stores matrices where most of the elements are zero. It optimizes space complexity by only storing non-zero elements, making it suitable for applications in scientific computing, machine learning, and any domain where large matrices with many zero values are common.

## Usage
```csharp
SparseMatrix matrix = new SparseMatrix(5, 5);
matrix.SetValue(0, 0, 1);
matrix.SetValue(1, 2, 3);
matrix.SetValue(3, 4, 5);
int value = matrix.GetValue(1, 2); // returns 3
SparseMatrix otherMatrix = new SparseMatrix(5, 5);
otherMatrix.SetValue(0, 0, 2);
SparseMatrix sumMatrix = matrix.Add(otherMatrix);
SparseMatrix productMatrix = matrix.Multiply(otherMatrix);
var nonZeroElements = matrix.GetNonZeroElements(); // returns list of non-zero elements
```

## Detailed Explanation
The `SparseMatrix` class uses a dictionary to store non-zero elements, where the key is a tuple representing the position (row, column) and the value is the non-zero integer. The class provides methods to set and get values, add two sparse matrices, multiply two sparse matrices, and retrieve all non-zero elements along with their positions.

## Complexity Analysis
- **SetValue**: O(1) on average for inserting into the dictionary.
- **GetValue**: O(1) on average for retrieving from the dictionary.
- **Add**: O(n + m) where n and m are the number of non-zero elements in the two matrices.
- **Multiply**: O(k * (n + m)) where k is the number of non-zero elements in the first matrix and n, m are the non-zero elements in the second matrix.
- **GetNonZeroElements**: O(n) where n is the number of non-zero elements stored.