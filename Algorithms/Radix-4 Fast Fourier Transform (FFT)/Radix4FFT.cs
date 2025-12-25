using System;
using System.Numerics;

/// <summary>
/// Class implementing the Radix-4 Fast Fourier Transform (FFT) algorithm.
/// </summary>
public static class Radix4FFT
{
    /// <summary>
    /// Computes the Radix-4 FFT of the input complex array. The length of the input must be a power of 4.
    /// </summary>
    /// <param name="input">An array of Complex numbers whose length is a power of 4.</param>
    /// <returns>A new Complex array containing the FFT output.</returns>
    public static Complex[] Compute(Complex[] input)
    {
        int n = input.Length;

        // Check if length of input is a power of 4
        if (!IsPowerOfFour(n))
            throw new ArgumentException("Input length must be a power of 4.");

        // Copy input to avoid modifying original array
        Complex[] data = new Complex[n];
        Array.Copy(input, data, n);

        // Bit-reverse ordering for radix-4 base
        BitReverseShuffle(data);

        // Perform the FFT stages
        for (int stageSize = 4; stageSize <= n; stageSize <<= 2)
        {
            // Number of groups per stage
            int groups = n / stageSize;
            // Twiddle factor base: e^(-2*pi*i / stageSize)
            double angleUnit = -2.0 * Math.PI / stageSize;

            for (int group = 0; group < groups; group++)
            {
                int groupStart = group * stageSize;

                for (int j = 0; j < stageSize / 4; j++)
                {
                    // Calculate twiddle factors for k=0,1,2,3
                    // Wn = e^(i*angleUnit)
                    double angle1 = angleUnit * j;
                    double angle2 = 2 * angle1;
                    double angle3 = 3 * angle1;

                    Complex w1 = Complex.FromPolarCoordinates(1.0, angle1);
                    Complex w2 = Complex.FromPolarCoordinates(1.0, angle2);
                    Complex w3 = Complex.FromPolarCoordinates(1.0, angle3);

                    int idx0 = groupStart + j;
                    int idx1 = idx0 + stageSize / 4;
                    int idx2 = idx1 + stageSize / 4;
                    int idx3 = idx2 + stageSize / 4;

                    // Fetch the four points for the radix-4 butterfly
                    Complex x0 = data[idx0];
                    Complex x1 = data[idx1] * w1;
                    Complex x2 = data[idx2] * w2;
                    Complex x3 = data[idx3] * w3;

                    // Radix-4 butterfly computations
                    Complex t0 = x0 + x2;           // x0 + x2
                    Complex t1 = x0 - x2;           // x0 - x2
                    Complex t2 = x1 + x3;           // x1 + x3
                    Complex t3 = (x1 - x3) * new Complex(0, -1); // -i * (x1 - x3)

                    // Combine to produce output values
                    data[idx0] = t0 + t2;
                    data[idx1] = t1 + t3;
                    data[idx2] = t0 - t2;
                    data[idx3] = t1 - t3;
                }
            }
        }

        return data;
    }

    /// <summary>
    /// Checks if a given integer is a power of four.
    /// </summary>
    private static bool IsPowerOfFour(int x)
    {
        if (x <= 0) return false;
        // Check power of two condition
        if ((x & (x - 1)) != 0) return false;
        // Check even bits set only (positions are 1-based: bits 1,3,5,...)
        // Power of four numbers have only one bit set in even positions (bit indices starting at 1 from LSB)
        // Mask for power of four bits: 0x55555555 (binary 01010101...)
        return (x & 0x55555555) != 0;
    }

    /// <summary>
    /// Reorders the array elements using base-4 digit reversal (bit-reversal adapted for radix-4).
    /// </summary>
    private static void BitReverseShuffle(Complex[] data)
    {
        int n = data.Length;
        int log4 = 0;

        // Compute log base 4 of n
        for (int temp = n; temp > 1; temp >>= 2)
            log4++;

        for (int i = 0; i < n; i++)
        {
            int reversedIndex = ReverseBase4(i, log4);
            if (reversedIndex > i)
            {
                // Swap elements at indices i and reversedIndex
                Complex temp = data[i];
                data[i] = data[reversedIndex];
                data[reversedIndex] = temp;
            }
        }
    }

    /// <summary>
    /// Reverses the base-4 digits of the integer given the digit length.
    /// </summary>
    private static int ReverseBase4(int x, int digits)
    {
        int reversed = 0;
        for (int i = 0; i < digits; i++)
        {
            reversed <<= 2;
            reversed |= (x & 0x3); // extract two bits (one base-4 digit)
            x >>= 2;
        }
        return reversed;
    }
}