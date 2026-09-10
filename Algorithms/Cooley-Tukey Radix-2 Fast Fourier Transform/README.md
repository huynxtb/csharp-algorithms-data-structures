# Cooley-Tukey Radix-2 Fast Fourier Transform (FFT)

## 1. Introduction
The Fast Fourier Transform (FFT) is an efficient algorithm to compute the Discrete Fourier Transform (DFT) and its inverse (IDFT). The DFT converts a sequence of data points from the time or space domain to the frequency domain, revealing the constituent frequencies of a signal. The Cooley-Tukey algorithm is the most common FFT algorithm, and the Radix-2 variant is particularly efficient when the input size `N` is a power of two. It significantly reduces the computational complexity from O(N²) for a direct DFT to O(N log N).

FFT is widely used in various fields, including:
-   **Signal Processing**: Audio and image processing, filtering, spectral analysis.
-   **Data Compression**: JPEG, MP3.
-   **Solving Partial Differential Equations**: Numerical methods.
-   **Convolution and Correlation**: Efficient computation of these operations.

This implementation provides an in-place, iterative Cooley-Tukey Radix-2 Decimation-In-Time (DIT) FFT, suitable for .NET 6.0+ applications using `System.Numerics.Complex`.

## 2. Usage
To use the `FastFourierTransform` class, ensure your input data is a `Complex[]` array whose length is a power of two.

```csharp
using System;
using System.Numerics;

// Assume FastFourierTransform class is available

// 1. Prepare your input data
int N = 16; // N must be a power of two (e.g., 2, 4, 8, 16, 32, ...)
Complex[] data = new Complex[N];

// Populate with sample data (e.g., a sine wave)
for (int i = 0; i < N; i++)
{
    data[i] = new Complex(Math.Sin(2 * Math.PI * i / N), 0); // Real sine wave
}

// 2. Perform Forward FFT
FastFourierTransform.Transform(data);

// 'data' now contains the frequency domain representation.
// You can inspect data[0] (DC component), data[1] (fundamental frequency), etc.

// 3. Perform Inverse FFT to get back to the original domain
FastFourierTransform.InverseTransform(data);

// 'data' now contains the original time-domain signal (or very close due to floating point precision).

// 4. Error Handling for invalid input
try
{
    Complex[] invalidData = new Complex[10]; // Length is not a power of two
    FastFourierTransform.Transform(invalidData);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Error: {ex.Message}"); // Output: "Input buffer length must be a power of two and greater than zero."
}
```

## 3. Detailed Explanation
The Cooley-Tukey Radix-2 DIT FFT algorithm proceeds in several stages:

1.  **Input Validation**: The algorithm requires the input buffer length `N` to be a power of two. If `N` is not a power of two or is zero, an `ArgumentException` is thrown.
2.  **Bit-Reversal Permutation**: Before the main FFT computation, the input array elements are reordered. This step is crucial for the iterative DIT algorithm as it places elements in the correct "bit-reversed" order, allowing the subsequent butterfly operations to be performed in-place without requiring additional memory for intermediate results or complex indexing. For an index `i`, its bit-reversed index `j` is found by reversing the bits of `i` (e.g., if `N=8` (binary `1000`), `i=1` (binary `001`) becomes `j=4` (binary `100`)).
3.  **Iterative Butterfly Computation**: The core of the FFT involves `log₂(N)` stages. Each stage combines pairs of intermediate results using a "butterfly" operation.
    *   **Outer Loop (`m`)**: This loop controls the size of the current sub-transforms, starting with `m=2` and doubling in each iteration up to `N`. `m` represents the size of the butterfly groups.
    *   **Middle Loop (`k`)**: This loop iterates through the start indices of each `m`-sized group within the `N` length buffer.
    *   **Inner Loop (`j`)**: This loop iterates through the first half of each `m`-sized group, performing the butterfly operation on `buffer[k+j]` and `buffer[k+j+m/2]`.
    *   **Twiddle Factor (`w`)**: For each butterfly, a complex exponential term called the "twiddle factor" `W_m^j = e^(-2*pi*i*j/m)` is calculated. This factor rotates the phase of one of the butterfly inputs.
    *   **Butterfly Operation**: For a pair of elements `u = buffer[k+j]` and `v = buffer[k+j+m/2]`, the operation is:
        *   `t = w * v`
        *   `buffer[k+j] = u + t`
        *   `buffer[k+j+m/2] = u - t`
    This process efficiently combines the results from smaller sub-transforms to build up the full transform.

4.  **Inverse Fast Fourier Transform (IFFT)**: The IFFT is computed by applying a slightly modified forward FFT.
    *   It uses the conjugate of the twiddle factors (`e^(2*pi*i*j/m)` instead of `e^(-2*pi*i*j/m)`).
    *   After the transformation, each element in the resulting array is scaled by `1/N` to revert the implicit scaling introduced by the forward transform.

## 4. Complexity Analysis
-   **Time Complexity**:
    -   **Bit-Reversal Permutation**: O(N log N) due to iterating through N elements and performing bit reversal (which takes O(log N) operations for each element).
    -   **FFT/IFFT Core**: The algorithm has `log₂(N)` stages. In each stage, there are `N/2` butterfly operations. Each butterfly operation involves a few complex multiplications and additions, which are constant time. Therefore, the core computation is O(N log N).
    -   **Total Time Complexity**: O(N log N).
-   **Space Complexity**:
    -   The algorithm performs the transformation in-place, meaning it modifies the input buffer directly.
    -   **Auxiliary Space Complexity**: O(1) (excluding the input buffer itself), as only a few temporary variables are used during computation.
    -   **Total Space Complexity**: O(N) for storing the input/output buffer.