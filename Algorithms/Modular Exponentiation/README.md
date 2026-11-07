# Modular Exponentiation

## Introduction
Modular Exponentiation is an efficient algorithm to compute large powers of a number modulo some integer. It is particularly useful in fields such as cryptography, where large numbers are common, and direct computation can lead to overflow or inefficiency. The algorithm uses the method of Exponentiation by Squaring to reduce the number of multiplications required.

## Usage
Here is an example of how to use the `ModularExponentiation` class:
```csharp
long result = ModularExponentiation.Power(2, 10, 1000); // result will be 24
```

## Detailed Explanation
The `Power` method calculates `(baseNum^exponent) % modulus` using an iterative approach. It works by repeatedly squaring the base and reducing the exponent by half. If the exponent is odd, it multiplies the current result by the base. This method ensures that all intermediate results are taken modulo `modulus`, preventing overflow and maintaining efficiency. The algorithm handles special cases such as when the exponent is zero or when the modulus is one.

## Complexity Analysis
- **Time Complexity:** O(log exponent) due to the halving of the exponent in each iteration.
- **Space Complexity:** O(1) as it uses a constant amount of space regardless of the input size.