# Introduction
The LZW compression algorithm is a lossless compression technique used to compress data by building a dictionary of substrings as they appear in the data and replacing each occurrence with a reference to the dictionary entry. This algorithm is particularly useful for compressing data that contains repeated patterns or sequences.

# Usage
```csharp
var input = "TOBEORNOTTOBEORTOBEORNOT";
var compressed = LZWCompression.Compress(input);
var decompressed = LZWCompression.Decompress(compressed);
Console.WriteLine(decompressed);
```

# Detailed Explanation
The LZW compression algorithm works by maintaining a dictionary of substrings as they appear in the input data. The dictionary is initialized with all possible single characters (0-255). The algorithm then iterates over the input data, building a string `w` of characters. If `w` is already in the dictionary, the algorithm continues to the next character. If `w` is not in the dictionary, the algorithm adds the current value of `w` to the result list, adds `w` plus the next character to the dictionary, and resets `w` to the next character. The decompression algorithm works by reversing this process, using the dictionary to rebuild the original input data.

# Complexity Analysis
* Time complexity: O(n), where n is the length of the input data.
* Space complexity: O(n), where n is the length of the input data.