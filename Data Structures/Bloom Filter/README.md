# Bloom Filter

## 1. Introduction

A Bloom Filter is a space-efficient probabilistic data structure designed to test whether an element is a member of a set. It can return false positives (incorrectly indicating that an element is present), but it guarantees no false negatives (never indicating an element is absent if it is actually present). This characteristic makes Bloom Filters particularly useful in scenarios where memory efficiency is critical, and an occasional false positive is acceptable, such as in network caches, databases, and distributed systems.

## 2. Usage

// Create a Bloom Filter with 10,000 bits and 7 hash functions
BloomFilter bloomFilter = new BloomFilter(10000, 7);

// Add elements
bloomFilter.Add("apple");
bloomFilter.Add("banana");

// Check membership
bool mightContainApple = bloomFilter.Contains("apple");  // true
bool mightContainCherry = bloomFilter.Contains("cherry"); // false (most likely)

## 3. Detailed Explanation

- **Internal Storage:** The Bloom Filter utilizes a `BitArray` to store membership bits corresponding to hashed elements, promoting space efficiency.

- **Hash Functions:** Instead of implementing multiple distinct hash functions, this implementation uses a double hashing technique that derives multiple independent hashes from two SHA-256-based hashes. For each element, two SHA-256 hashes are computed: one on the original string and one on the string appended with a salt byte. The results are interpreted as integers and combined linearly to generate multiple hash values.

- **Adding Elements:** When adding an element, the filter computes all hash positions and sets the corresponding bits in the bit array.

- **Membership Check:** To test membership, the filter checks if all bits corresponding to the hash positions of the queried element are set. If any bit is not set, the element is definitely not present.

- **Configurability:** The size of the bit array and the number of hash functions can be specified on construction, allowing a trade-off between space, error rate, and performance.

## 4. Complexity Analysis

- **Add Operation:**
  - Time Complexity: O(k), where k is the number of hash functions. Hash computations and bit setting are constant time per hash.
  - Space Complexity: O(m), where m is the size of the bit array.

- **Contains Operation:**
  - Time Complexity: O(k), as it checks k bits.
  - Space Complexity: O(1) additional beyond the bit array.

This implementation balances simplicity, correctness, and efficiency, making it suitable for general-purpose Bloom Filter use cases in .NET environments.