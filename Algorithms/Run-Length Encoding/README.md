# Run-Length Encoding

## Introduction
Run-Length Encoding (RLE) is a simple form of data compression that replaces consecutive identical characters in a string with a single character followed by the number of occurrences. It is particularly effective for data with many repeated characters, making it useful in scenarios such as image compression and simple text compression.

## Usage
```csharp
string original = "AAABBC";
string encoded = RunLengthEncoding.Encode(original); // Expected: "A3B2C1"
string decoded = RunLengthEncoding.Decode(encoded); // Expected: "AAABBC"
```

## Detailed Explanation
The `RunLengthEncoding` class contains two static methods: `Encode` and `Decode`. The `Encode` method iterates through the input string, counting consecutive characters and appending them along with their counts to a `StringBuilder`. The `Decode` method reverses this process by reading the encoded string, extracting characters and their counts, and reconstructing the original string. Both methods handle edge cases such as empty strings and single-character inputs gracefully.

## Complexity Analysis
- **Time Complexity:**
  - `Encode`: O(n), where n is the length of the input string, as it processes each character once.
  - `Decode`: O(m), where m is the length of the encoded string, as it processes each character and digit.
- **Space Complexity:** O(n) for the encoded string in the worst case, where no characters are repeated.