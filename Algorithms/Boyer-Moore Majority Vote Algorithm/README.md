# Boyer-Moore Majority Vote Algorithm

## 1. Introduction

The Boyer-Moore Majority Vote algorithm is a well-known algorithm in computer science used to find the majority element in a given array. A majority element is defined as an element that appears more than half the times in the array. This problem occurs in various fields such as voting systems, streaming data analysis, and consensus algorithms.

This algorithm is highly efficient, operating in O(n) time complexity and O(1) space complexity, making it optimal for large datasets and streaming data scenarios where memory overhead needs to be minimal.

## 2. Usage

You can use the `BoyerMooreMajorityVote` class by setting an integer array as input using the `SetInput` method, and then calling the `GetMajorityElement` method to retrieve the majority element if it exists.

Example usage:

var majorityVote = new BoyerMooreMajorityVote();
majorityVote.SetInput(new int[] { 2, 2, 1, 1, 1, 2, 2 });
int majorityElement = majorityVote.GetMajorityElement();
// majorityElement would be 2 in this case

## 3. Detailed Explanation

The algorithm operates in two phases:

- **Phase 1: Finding a Candidate**
  - Iterate through the array tracking a `candidate` and its `count`.
  - Initially, the first element is considered the candidate with a count of 1.
  - For each subsequent element:
    - If it matches the candidate, increment the count.
    - Otherwise, decrement the count.
  - If the count reaches zero, change the candidate to the current element and reset count to 1.

- **Phase 2: Verification**
  - Once a candidate is found, iterate over the array again to count its occurrences.
  - Return the candidate only if it appears more than half of the array length; otherwise, return -1 indicating no majority element exists.

This works because the majority element's frequency will always outweigh the sum frequency of all other elements combined, allowing it to remain as the candidate after the first phase.

## 4. Complexity Analysis

- **Time Complexity:** O(n) — The algorithm makes a single pass to find the candidate and another pass to verify its majority status.
- **Space Complexity:** O(1) — It uses a constant amount of space regardless of input size.

This makes the Boyer-Moore Majority Vote Algorithm an optimal and elegant solution for majority detection problems.