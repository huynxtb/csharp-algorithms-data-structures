public static class PolynomialRollingHash {
    public static long[] ComputePrefixHashes(string s, long prime, long baseValue) {
        long[] prefixHashes = new long[s.Length + 1];
        long hash = 0;
        long basePower = 1;
        for (int i = 0; i < s.Length; i++) {
            hash = (hash + (s[i] * basePower) % prime) % prime;
            prefixHashes[i + 1] = hash;
            basePower = (basePower * baseValue) % prime;
        }
        return prefixHashes;
    }

    public static long[] ComputePowersOfBase(int maxLength, long prime, long baseValue) {
        long[] powersOfBase = new long[maxLength + 1];
        powersOfBase[0] = 1;
        for (int i = 1; i <= maxLength; i++) {
            powersOfBase[i] = (powersOfBase[i - 1] * baseValue) % prime;
        }
        return powersOfBase;
    }

    public static long GetSubstringHash(long[] prefixHashes, long[] powersOfBase, long prime, int startIndex, int length) {
        if (startIndex < 0 || length < 0 || startIndex + length > prefixHashes.Length - 1) {
            throw new ArgumentOutOfRangeException("Invalid startIndex or length.");
        }
        int endIndex = startIndex + length - 1;
        long hash = (prefixHashes[endIndex + 1] - (prefixHashes[startIndex] * powersOfBase[length]) % prime + prime) % prime;
        return hash;
    }
}