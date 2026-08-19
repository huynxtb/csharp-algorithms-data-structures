using System;
using System.Numerics;

namespace Algorithms
{
    /// <summary>
    /// Provides methods for primality testing using the Miller-Rabin algorithm.
    /// </summary>
    public static class MillerRabinTester
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Determines if a BigInteger is probably prime using the Miller-Rabin primality test.
        /// </summary>
        /// <param name="value">The BigInteger to test for primality.</param>
        /// <param name="witnesses">The number of iterations (witnesses) to run. Higher values increase accuracy.</param>
        /// <returns>True if the value is probably prime; false if it is definitely composite.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when witnesses is less than or equal to 0.</exception>
        public static bool IsPrime(BigInteger value, int witnesses)
        {
            if (witnesses <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(witnesses), "Number of witnesses must be greater than zero.");
            }

            if (value <= 1) return false;
            if (value == 2 || value == 3) return true;
            if (value.IsEven) return false;

            // Factor value - 1 as s * 2^d
            BigInteger s = value - 1;
            int d = 0;
            while (s.IsEven)
            {
                s /= 2;
                d++;
            }

            for (int i = 0; i < witnesses; i++)
            {
                BigInteger a = GetRandomBigInteger(2, value - 2);
                BigInteger x = BigInteger.ModPow(a, s, value);

                if (x == 1 || x == value - 1)
                {
                    continue;
                }

                bool composite = true;
                for (int r = 1; r < d; r++)
                {
                    x = BigInteger.ModPow(x, 2, value);
                    if (x == value - 1)
                    {
                        composite = false;
                        break;
                    }
                }

                if (composite)
                {
                    return false;
                }
            }

            return true;
        }

        private static BigInteger GetRandomBigInteger(BigInteger min, BigInteger max)
        {
            BigInteger range = max - min;
            byte[] bytes = range.ToByteArray();
            BigInteger value;

            lock (_random)
            {
                do
                {
                    _random.NextBytes(bytes);
                    bytes[bytes.Length - 1] &= 0x7F; // Ensure positive
                    value = new BigInteger(bytes);
                } while (value > range);
            }

            return value + min;
        }
    }
}