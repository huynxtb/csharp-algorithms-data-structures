# Sparse Matrix Representation (COO)

## Introduction
The Sparse Matrix Representation using Coordinate List (COO) format is a data structure designed to efficiently store and manipulate sparse matrices, which are matrices predominantly filled with zero elements. This representation optimizes space by only storing non-zero entries along with their corresponding row and column indices.

## Usage
```csharp
SparseMatrix sparseMatrix = new SparseMatrix(5, 5);
sparseMatrix.AddEntry(0, 1, 3.5);
sparseMatrix.AddEntry(2, 3, 4.2);
double value = sparseMatrix.GetValue(0, 1);
double[,] denseMatrix = sparseMatrix.ToDenseMatrix();
```

## Detailed Explanation
The `SparseMatrix` class contains three lists to store the row indices, column indices, and values of non-zero elements. The constructor initializes these lists and sets the dimensions of the matrix. The `AddEntry` method allows adding non-zero entries while checking for out-of-bounds indices. The `GetValue` method retrieves the value at a specified row and column, returning zero if the entry does not exist. The `ToDenseMatrix` method converts the sparse matrix back into a standard dense matrix format.

## Complexity Analysis
- **AddEntry:** O(1) for adding an entry, but O(n) in the worst case for searching duplicates.
- **GetValue:** O(n) in the worst case, as it may need to search through all non-zero entries.
- **ToDenseMatrix:** O(n), where n is the number of non-zero entries, as it iterates through the list to populate the dense matrix.