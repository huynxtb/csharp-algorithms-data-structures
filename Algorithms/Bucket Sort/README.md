# Bucket Sort

### 1. Introduction
Bucket Sort is a distribution-based sorting algorithm that partitions an array into a number of buckets. Each bucket is then sorted individually, either using a different sorting algorithm or by recursively applying the bucket sorting algorithm. It is highly efficient when the input elements are uniformly distributed over a known range, typically $[0, 1)$.

### 2. Usage
```csharp
using System;

class Program
{
    static void Main()
    {
        double[] data = { 0.78, 0.17, 0.39, 0.26, 0.72, 0.94, 0.21, 0.12, 0.23, 0.68 };
        
        BucketSort.Sort(data);
        
        Console.WriteLine(string.Join(", ", data));
        // Output: 0.12, 0.17, 0.21, 0.23, 0.26, 0.39, 0.68, 0.72, 0.78, 0.94
    }
}
```

### 3. Detailed Explanation
1. **Validation**: The algorithm verifies that the input array is not null and that all elements fall within the valid range $[0, 1)$.
2. **Initialization**: An array of empty lists (buckets) is created. The number of buckets equals the length of the input array ($n$).
3. **Distribution**: Each element $array[i]$ is mapped to a bucket index using the formula `bucketIndex = floor(n * array[i])` and appended to that bucket.
4. **Sorting Buckets**: Each individual bucket is sorted using a stable Insertion Sort helper method. Insertion sort is chosen because buckets are expected to contain few elements, making insertion sort highly efficient.
5. **Concatenation**: The sorted elements from each bucket are written back into the original array sequentially, resulting in a fully sorted array.

### 4. Complexity Analysis
- **Time Complexity**:
  - **Best Case**: $O(n + k)$ when elements are uniformly distributed, resulting in $O(1)$ elements per bucket.
  - **Average Case**: $O(n + k)$ where $n$ is the number of elements and $k$ is the number of buckets.
  - **Worst Case**: $O(n^2)$ when all elements are distributed into a single bucket, reducing the performance to that of the underlying insertion sort.
- **Space Complexity**: $O(n + k)$ auxiliary space to store the buckets and their elements.