using System;

public static class ModularExponentiation
{
    public static long Power(long baseNum, long exponent, long modulus)
    {
        if (modulus == 1) return 0; // since any number mod 1 is 0
        if (exponent == 0) return 1 % modulus; // base case: x^0 = 1

        long result = 1;
        baseNum = baseNum % modulus; // Update base if it is more than or equal to modulus

        while (exponent > 0)
        {
            // If exponent is odd, multiply base with result
            if ((exponent & 1) == 1)
            {
                result = (result * baseNum) % modulus;
            }
            // exponent must be even now
            exponent >>= 1; // exponent = exponent / 2
            baseNum = (baseNum * baseNum) % modulus; // Change base to base^2
        }
        return result;
    }
}