public class FisherYatesShuffle
{
    /// <summary>
    /// Generates a random in-place permutation of the input array using the Fisher-Yates (Knuth) Shuffle algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The input array to be shuffled.</param>
    public static void Shuffle<T>(T[] array)
    {
        var random = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}