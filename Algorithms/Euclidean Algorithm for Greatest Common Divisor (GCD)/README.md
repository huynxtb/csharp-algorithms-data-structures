# Introduction
The Euclidean Algorithm is an efficient method for computing the greatest common divisor (GCD) of two non-negative integers. The GCD is the largest positive integer that divides both numbers without leaving a remainder. This algorithm is fundamental in number theory and has applications in cryptography, computer graphics, and modular arithmetic.

# Usage
```csharp
int gcdRecursive = EuclideanAlgorithm.GetGCDRecursive(48, 18);
int gcdIterative = EuclideanAlgorithm.GetGCDIterative(48, 18);
```

# Detailed Explanation
The `EuclideanAlgorithm` class provides two methods to compute the GCD: `GetGCDRecursive` and `GetGCDIterative`. Both methods first validate the input parameters to ensure they are non-negative integers. If both integers are zero, they return zero. The recursive method uses the principle that GCD(a, b) = GCD(b, a % b), while the iterative method uses a loop to repeatedly apply the same principle until one of the integers becomes zero.

# Complexity Analysis
- **Time Complexity:** Both methods have a time complexity of O(log(min(a, b))) due to the nature of the division operations involved in the algorithm.
- **Space Complexity:** The recursive method has a space complexity of O(log(min(a, b))) due to the call stack, while the iterative method has a space complexity of O(1) as it uses a constant amount of space.