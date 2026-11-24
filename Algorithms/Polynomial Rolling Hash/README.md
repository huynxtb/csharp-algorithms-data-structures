# Polynomial Rolling Hash

## Introduction
The Polynomial Rolling Hash is a technique used for efficiently computing and comparing hash values of substrings within a larger string. It is particularly useful in string processing algorithms such as Rabin-Karp string matching, finding palindromic substrings, or detecting duplicate substrings. The algorithm represents a string as a polynomial and computes its value modulo a large prime number, allowing for efficient updates when moving a window over the string.

## Usage
```csharp
string input = "example";
long prime = 1000000007;
long baseValue = 31;

long[] prefixHashes = PolynomialRollingHash.ComputePrefixHashes(input, prime, baseValue);
long[] powersOfBase = PolynomialRollingHash.ComputePowersOfBase(input.Length, prime, baseValue);

int startIndex = 1;
int length = 3;
long substringHash = PolynomialRollingHash.GetSubstringHash(prefixHashes, powersOfBase, prime, startIndex, length);
```  

## Detailed Explanation
1. **ComputePrefixHashes**: This method computes the hash values for all prefixes of the input string. It uses a polynomial accumulation method where each character contributes to the hash based on its position and a base raised to the power of its index.
2. **ComputePowersOfBase**: This method precomputes the powers of the base value modulo the prime number, which is used to adjust the hash values when extracting substring hashes.
3. **GetSubstringHash**: This method retrieves the hash of a substring using precomputed prefix hashes and powers of the base. It ensures that the calculations respect modular arithmetic to avoid negative values.

## Complexity Analysis
- **Time Complexity**:  
  - `ComputePrefixHashes`: O(n) where n is the length of the string.  
  - `ComputePowersOfBase`: O(n) where n is the maximum length.  
  - `GetSubstringHash`: O(1) as it uses precomputed values.  

- **Space Complexity**:  
  - O(n) for storing prefix hashes and powers of base.