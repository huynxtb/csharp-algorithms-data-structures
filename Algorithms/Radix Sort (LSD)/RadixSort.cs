using System;
using System.Linq;

/// <summary>
/// RadixSort class implements the Radix Sort algorithm using the Least Significant Digit (LSD) approach.
/// This algorithm sorts non-negative integers by processing individual digits, using Counting Sort as a subroutine.
/// </summary>
public class RadixSort
{
    /// <summary>
    /// Sorts an array of non-negative integers in ascending order using the Radix Sort algorithm.
    /// The algorithm uses the Least Significant Digit (LSD) approach with Counting Sort as a subroutine.
    /// </summary>
    /// <param name="array">The array of non-negative integers to be sorted</param>
    /// <returns>A new sorted array of integers in ascending order</returns>
    public static int[] Sort(int[] array)
    {
        // Handle edge cases: null or empty arrays
        if (array == null || array.Length == 0)
            return array;

        // Handle arrays with single element
        if (array.Length == 1)
            return array;

        // Create a copy to avoid modifying the original array
        int[] result = (int[])array.Clone();

        // Find the maximum number to determine the number of digits
        int maxNumber = result.Max();

        // If the maximum number is 0, the array is already sorted
        if (maxNumber == 0)
            return result;

        // Process each digit position, starting from the least significant digit
        // Loop through each digit position (ones, tens, hundreds, etc.)
        for (int digitPlace = 1; maxNumber / digitPlace > 0; digitPlace *= 10)
        {
            // Use Counting Sort to sort by the current digit
            result = CountingSortByDigit(result, digitPlace);
        }

        return result;
    }

    /// <summary>
    /// Performs Counting Sort on the array based on a specific digit position.
    /// This is a subroutine of the Radix Sort algorithm.
    /// </summary>
    /// <param name="array">The array to be sorted by a specific digit</param>
    /// <param name="digitPlace">The current digit position (1 for ones, 10 for tens, 100 for hundreds, etc.)</param>
    /// <returns>A new array sorted by the specified digit position</returns>
    private static int[] CountingSortByDigit(int[] array, int digitPlace)
    {
        int arrayLength = array.Length;
        int[] output = new int[arrayLength];

        // Array to store the count of digits (0-9)
        int[] digitCount = new int[10];

        // Step 1: Count the occurrences of each digit at the current digit position
        for (int i = 0; i < arrayLength; i++)
        {
            int digit = (array[i] / digitPlace) % 10;
            digitCount[digit]++;
        }

        // Step 2: Convert counts to cumulative counts
        // This tells us the position where each digit's elements should start in the output array
        for (int i = 1; i < 10; i++)
        {
            digitCount[i] += digitCount[i - 1];
        }

        // Step 3: Place elements into the output array in reverse order (to maintain stability)
        // Processing from right to left ensures that the sort is stable
        for (int i = arrayLength - 1; i >= 0; i--)
        {
            int digit = (array[i] / digitPlace) % 10;
            digitCount[digit]--;
            output[digitCount[digit]] = array[i];
        }

        return output;
    }
}