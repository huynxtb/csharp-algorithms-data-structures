using System;
using System.Numerics;

public static class FastFourierTransform
{
    /// <summary>
    /// Performs an in-place Cooley-Tukey Radix-2 Decimation-In-Time (DIT) Fast Fourier Transform (FFT) on the given complex buffer.
    /// The input buffer length must be a power of two.
    /// </summary>
    /// <param name="buffer">The complex array to transform. The result will overwrite this array.</param>
    /// <exception cref="ArgumentNullException">Thrown if the buffer is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the buffer length is not a power of two or is zero.</exception>
    public static void Transform(Complex[] buffer)
    {
        ValidateBuffer(buffer);
        int n = buffer.Length;
        BitReverse(buffer);

        for (int m = 2; m <= n; m <<= 1) // m is the size of the current butterfly group
        {
            // W_m = e^(-2*pi*i / m)
            Complex wm = Complex.FromPolarCoordinates(1.0, -2 * Math.PI / m);

            for (int k = 0; k < n; k += m) // k is the start index of the current group
            {
                Complex w = 1.0; // Twiddle factor for the current butterfly
                for (int j = 0; j < m / 2; j++) // j is the index within the butterfly group
                {
                    Complex t = w * buffer[k + j + m / 2];
                    Complex u = buffer[k + j];

                    buffer[k + j] = u + t;
                    buffer[k + j + m / 2] = u - t;

                    w *= wm; // Update twiddle factor for next butterfly in the group
                }
            }
        }
    }

    /// <summary>
    /// Performs an in-place Inverse Cooley-Tukey Radix-2 Decimation-In-Time (DIT) Fast Fourier Transform (IFFT) on the given complex buffer.
    /// The input buffer length must be a power of two.
    /// </summary>
    /// <param name="buffer">The complex array to inverse transform. The result will overwrite this array.</param>
    /// <exception cref="ArgumentNullException">Thrown if the buffer is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the buffer length is not a power of two or is zero.</exception>
    public static void InverseTransform(Complex[] buffer)
    {
        ValidateBuffer(buffer);
        int n = buffer.Length;

        // Perform forward FFT with conjugate twiddle factors
        BitReverse(buffer);

        for (int m = 2; m <= n; m <<= 1)
        {
            // W_m = e^(2*pi*i / m) for IFFT
            Complex wm = Complex.FromPolarCoordinates(1.0, 2 * Math.PI / m);

            for (int k = 0; k < n; k += m)
            {
                Complex w = 1.0;
                for (int j = 0; j < m / 2; j++)
                {
                    Complex t = w * buffer[k + j + m / 2];
                    Complex u = buffer[k + j];

                    buffer[k + j] = u + t;
                    buffer[k + j + m / 2] = u - t;

                    w *= wm;
                }
            }
        }

        // Scale by 1/N
        for (int i = 0; i < n; i++)
        {
            buffer[i] /= n;
        }
    }

    /// <summary>
    /// Validates the input buffer for null and power-of-two length.
    /// </summary>
    /// <param name="buffer">The complex array to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown if the buffer is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the buffer length is not a power of two or is zero.</exception>
    private static void ValidateBuffer(Complex[] buffer)
    {
        if (buffer == null)
        {
            throw new ArgumentNullException(nameof(buffer), "Input buffer cannot be null.");
        }
        int n = buffer.Length;
        if (n == 0 || (n & (n - 1)) != 0)
        {
            throw new ArgumentException("Input buffer length must be a power of two and greater than zero.", nameof(buffer));
        }
    }

    /// <summary>
    /// Performs an in-place bit-reversal permutation on the buffer.
    /// </summary>
    /// <param name="buffer">The complex array to bit-reverse.</param>
    private static void BitReverse(Complex[] buffer)
    {
        int n = buffer.Length;
        int log2N = 0;
        while ((1 << log2N) < n)
        {
            log2N++;
        }

        for (int i = 0; i < n; i++)
        {
            int j = ReverseBits(i, log2N);
            if (i < j) // Only swap if i < j to avoid double-swapping
            {
                Complex temp = buffer[i];
                buffer[i] = buffer[j];
                buffer[j] = temp;
            }
        }
    }

    /// <summary>
    /// Reverses the bits of an integer up to a specified number of bits.
    /// </summary>
    /// <param name="n">The integer to reverse.</param>
    /// <param name="numBits">The number of bits to consider for reversal.</param>
    /// <returns>The bit-reversed integer.</returns>
    private static int ReverseBits(int n, int numBits)
    {
        int reversedN = 0;
        for (int i = 0; i < numBits; i++)
        {
            reversedN = (reversedN << 1) | (n & 1);
            n >>= 1;
        }
        return reversedN;
    }
}