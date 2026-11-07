using System;
using System.Collections.Generic;

public class ThreeSumSolution
{
    /// <summary>
    /// Finds all unique triplets in the array that sum up to zero.
    /// </summary>
    /// <param name="nums">An array of integers.</param>
    /// <returns>A list of lists containing unique triplets that sum to zero.</returns>
    public List<List<int>> ThreeSum(int[] nums)
    {
        List<List<int>> result = new List<List<int>>();
        if (nums == null || nums.Length < 3)
        {
            return result;
        }

        Array.Sort(nums);

        for (int i = 0; i < nums.Length - 2; i++)
        {
            // Skip duplicate values for the first number
            if (i > 0 && nums[i] == nums[i - 1]) continue;

            int left = i + 1;
            int right = nums.Length - 1;

            while (left < right)
            {
                int sum = nums[i] + nums[left] + nums[right];

                if (sum == 0)
                {
                    result.Add(new List<int> { nums[i], nums[left], nums[right] });
                    // Skip duplicate values for the second number
                    while (left < right && nums[left] == nums[left + 1]) left++;
                    // Skip duplicate values for the third number
                    while (left < right && nums[right] == nums[right - 1]) right--;
                    left++;
                    right--;
                }
                else if (sum < 0)
                {
                    left++;
                }
                else
                {
                    right--;
                }
            }
        }

        return result;
    }
}