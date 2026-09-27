/// <summary>
/// Provides methods for generating prime numbers and primality testing using the Sieve of Eratosthenes algorithm.
/// </summary>
public class SieveOfEratosthenes
{
    /// <summary>
    /// Finds and returns all prime numbers less than or equal to a specified limit.
    /// </summary>
    /// <param name="n">The upper limit integer up to which to search for prime numbers.</param>
    /// <returns>A list of prime numbers up to <paramref name="n"/> in ascending order.</returns>
    public List<int> FindPrimes(int n)
    {
        // Handle edge cases where no primes exist
        if (n < 2)
        {
            return new List<int>();
        }

        // isPrime[i] will indicate whether number i is prime.
        // Default is false in C#, so we populate with true or invert meaning.
        // Here, isComposite[i] = true indicates composite, false indicates prime.
        bool[] isComposite = new bool[n + 1];
        int limit = (int)Math.Sqrt(n);

        // Sieve process up to sqrt(n)
        for (int p = 2; p <= limit; p++)
        {
            if (!isComposite[p])
            {
                // Mark multiples of p starting from p * p
                // Using long for the loop variable to prevent 32-bit signed integer overflow
                for (long multiple = (long)p * p; multiple <= n; multiple += p)
                {
                    isComposite[(int)multiple] = true;
                }
            }
        }

        // Collect all unmarked numbers as primes
        List<int> primes = new List<int>();
        for (int i = 2; i <= n; i++)
        {
            if (!isComposite[i])
            {
                primes.Add(i);
            }
        }

        return primes;
    }

    /// <summary>
    /// Determines whether a specific integer is a prime number using trial division up to its square root.
    /// </summary>
    /// <param name="number">The integer to evaluate for primality.</param>
    /// <returns><c>true</c> if the number is prime; otherwise, <c>false</c>.</returns>
    public bool IsPrime(int number)
    {
        if (number < 2)
        {
            return false;
        }
        if (number == 2 || number == 3)
        {
            return true;
        }
        if (number % 2 == 0 || number % 3 == 0)
        {
            return false;
        }

        int limit = (int)Math.Sqrt(number);
        for (int i = 5; i <= limit; i += 6)
        {
            if (number % i == 0 || number % (i + 2) == 0)
            {
                return false;
            }
        }

        return true;
    }
}