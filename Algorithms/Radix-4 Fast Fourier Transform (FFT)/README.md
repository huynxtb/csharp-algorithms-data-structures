# Radix-4 Fast Fourier Transform (FFT)

## 1. Introduction

The Radix-4 Fast Fourier Transform (FFT) is an efficient algorithm for computing the Discrete Fourier Transform (DFT) of a sequence whose length is a power of four. Compared to the standard radix-2 FFT, the radix-4 FFT reduces the number of arithmetic operations by processing the input in groups of four, giving computational advantages for input sizes that match this constraint.

This implementation is useful in signal processing, communications, and other fields where frequency analysis of discrete signals is required.

## 2. Usage

Below is a simple example showing how to use the `Radix4FFT` class to compute the FFT of a complex sequence:

using System;
using System.Numerics;

class Example
{
    public static void PerformFFT()
    {
        // Example input: 16 (4^2) complex samples
        Complex[] samples = new Complex[16];
        for (int i = 0; i < 16; i++)
        {
            samples[i] = new Complex(i + 1, 0); // real values 1 to 16
        }

        // Compute the Radix-4 FFT
        Complex[] result = Radix4FFT.Compute(samples);

        // The 'result' array now contains the frequency domain representation
        // (Further processing or analysis can be done here)
    }
}

## 3. Detailed Explanation

- **Input Requirements:**
  - The input array length must be a power of 4. The algorithm does not perform any padding or error correction.

- **Algorithm Outline:**
  1. **Bit-reversal shuffle:** The input data is reordered using base-4 digit reversal to allow in-place FFT computation.
  2. **Iterative stages:** The FFT is performed over multiple stages, each of which operates on groups of size `stageSize` expanding by powers of 4.
  3. **Radix-4 butterflies:** Each group is divided into four sub-points and combined using the radix-4 butterfly operations. Twiddle factors (complex exponential weights) are computed and applied to sub-points except the first.

- **Optimization Considerations:**
  - Twiddle factors are computed on-the-fly using polar coordinates to avoid extra memory usage.
  - The algorithm uses in-place updates to minimize memory consumption.

## 4. Complexity Analysis

- **Time complexity:**
  - The radix-4 FFT reduces the number of arithmetic operations compared to radix-2 FFT by roughly halving the number of complex multiplications.
  - Overall complexity is O(N log_4 N) which equals O(N * (log N / log 4)) and is asymptotically better than radix-2 FFT.

- **Space complexity:**
  - The algorithm performs in-place computation (except the initial copy), requiring O(N) space for the input/output array.

This implementation is efficient and suitable for applications where input lengths are powers of four, providing improved performance over radix-2 FFT implementations in such scenarios.