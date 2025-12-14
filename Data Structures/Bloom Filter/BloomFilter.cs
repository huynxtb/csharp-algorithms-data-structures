using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class BloomFilter
{
    private readonly BitArray bitArray;
    private readonly int size;
    private readonly int hashFunctionCount;

    // Constructor to initialize Bloom Filter with specified bit array size and number of hash functions
    public BloomFilter(int size, int hashFunctionCount)
    {
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive.");
        if (hashFunctionCount <= 0) throw new ArgumentOutOfRangeException(nameof(hashFunctionCount), "Hash function count must be positive.");
        this.size = size;
        this.hashFunctionCount = hashFunctionCount;
        bitArray = new BitArray(size);
    }

    // Add an item to the Bloom Filter
    public void Add(string item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        foreach (int position in GetHashes(item))
        {
            bitArray.Set(position, true);
        }
    }

    // Check if an item might be in the Bloom Filter
    // Returns true if the item might be present (with some false positive probability)
    // Returns false if the item definitely is not present
    public bool Contains(string item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        foreach (int position in GetHashes(item))
        {
            if (!bitArray.Get(position))
                return false;
        }
        return true;
    }

    // Generate multiple hash values using Double Hashing technique
    // Uses a base hash and a different hash seed to simulate multiple hash functions
    private int[] GetHashes(string item)
    {
        // Use SHA256 to create a base hash
        byte[] itemBytes = Encoding.UTF8.GetBytes(item);
        byte[] hash1;
        byte[] hash2;

        using (SHA256 sha256 = SHA256.Create())
        {
            hash1 = sha256.ComputeHash(itemBytes);
            // To create a second hash, hash again with appended byte
            byte[] modifiedItem = new byte[itemBytes.Length + 1];
            Buffer.BlockCopy(itemBytes, 0, modifiedItem, 0, itemBytes.Length);
            modifiedItem[itemBytes.Length] = 0xFF; // arbitrary salt
            hash2 = sha256.ComputeHash(modifiedItem);
        }

        int[] positions = new int[hashFunctionCount];

        // Convert first 4 bytes of hash arrays to int
        int baseHash1 = BitConverter.ToInt32(hash1, 0) & 0x7FFFFFFF; // Ensure positive
        int baseHash2 = BitConverter.ToInt32(hash2, 0) & 0x7FFFFFFF; // Ensure positive

        for (int i = 0; i < hashFunctionCount; i++)
        {
            // Double hashing formula: (hash1 + i * hash2) mod size
            int combinedHash = (baseHash1 + i * baseHash2) % size;
            if (combinedHash < 0) combinedHash += size; // handle negative
            positions[i] = combinedHash;
        }

        return positions;
    }
}
