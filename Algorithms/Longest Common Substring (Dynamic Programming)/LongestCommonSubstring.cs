using System;

/// <summary>
/// Provides methods to find the longest common substring between two strings
/// using a dynamic programming approach.
/// </summary>
public class LongestCommonSubstring
{
    /// <summary>
    /// Finds the longest common substring between two input strings using dynamic programming.
    /// </summary>
    /// <param name="text1">The first input string.</param>
    /// <param name="text2">The second input string.</param>
    /// <returns>
    /// The longest contiguous sequence of characters common to both strings.
    /// Returns an empty string if no common substring exists.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="text1"/> or <paramref name="text2"/> is null.
    /// </exception>
    public string FindLongestCommonSubstring(string text1, string text2)
    {
        if (text1 == null)
            throw new ArgumentNullException(nameof(text1), "The first input string cannot be null.");
        if (text2 == null)
            throw new ArgumentNullException(nameof(text2), "The second input string cannot be null.");

        if (text1.Length == 0 || text2.Length == 0)
            return string.Empty;

        int rows = text1.Length;
        int cols = text2.Length;
        int[,] dp = new int[rows + 1, cols + 1];

        int maxLength = 0;
        int endingIndex = 0;

        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= cols; j++)
            {
                if (text1[i - 1] == text2[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1] + 1;

                    if (dp[i, j] > maxLength)
                    {
                        maxLength = dp[i, j];
                        endingIndex = i;
                    }
                }
                else
                {
                    dp[i, j] = 0;
                }
            }
        }

        if (maxLength == 0)
            return string.Empty;

        int startIndex = endingIndex - maxLength;
        return text1.Substring(startIndex, maxLength);
    }

    /// <summary>
    /// Gets the length of the longest common substring between two input strings.
    /// </summary>
    /// <param name="text1">The first input string.</param>
    /// <param name="text2">The second input string.</param>
    /// <returns>
    /// The length of the longest contiguous sequence of characters common to both strings.
    /// Returns 0 if no common substring exists.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="text1"/> or <paramref name="text2"/> is null.
    /// </exception>
    public int GetLongestCommonSubstringLength(string text1, string text2)
    {
        if (text1 == null)
            throw new ArgumentNullException(nameof(text1), "The first input string cannot be null.");
        if (text2 == null)
            throw new ArgumentNullException(nameof(text2), "The second input string cannot be null.");

        if (text1.Length == 0 || text2.Length == 0)
            return 0;

        int rows = text1.Length;
        int cols = text2.Length;
        int[,] dp = new int[rows + 1, cols + 1];

        int maxLength = 0;

        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= cols; j++)
            {
                if (text1[i - 1] == text2[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1] + 1;

                    if (dp[i, j] > maxLength)
                    {
                        maxLength = dp[i, j];
                    }
                }
                else
                {
                    dp[i, j] = 0;
                }
            }
        }

        return maxLength;
    }
}