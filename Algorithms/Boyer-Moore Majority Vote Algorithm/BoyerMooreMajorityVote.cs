public class BoyerMooreMajorityVote
{
    private int[] _elements;

    /// <summary>
    /// Set the input array on which to operate.
    /// </summary>
    /// <param name="elements">Array of integers to find majority element in.</param>
    public void SetInput(int[] elements)
    {
        _elements = elements;
    }

    /// <summary>
    /// Returns the majority element if it exists, otherwise returns -1.
    /// </summary>
    /// <returns>The majority element or -1 if none exists.</returns>
    public int GetMajorityElement()
    {
        if (_elements == null || _elements.Length == 0)
            return -1;

        // Phase 1: Find a candidate for majority element
        int candidate = _elements[0];
        int count = 1;

        for (int i = 1; i < _elements.Length; i++)
        {
            if (_elements[i] == candidate)
            {
                count++;
            }
            else
            {
                count--;
                if (count == 0)
                {
                    candidate = _elements[i];
                    count = 1;
                }
            }
        }

        // Phase 2: Verify if the candidate is actually a majority
        count = 0;
        for (int i = 0; i < _elements.Length; i++)
        {
            if (_elements[i] == candidate)
                count++;
        }

        return count > _elements.Length / 2 ? candidate : -1;
    }
}