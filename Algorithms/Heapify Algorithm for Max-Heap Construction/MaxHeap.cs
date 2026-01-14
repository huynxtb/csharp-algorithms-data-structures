public static class MaxHeap
{
    public static void Heapify(int[] array)
    {
        int n = array.Length;
        // Start from the last parent node and sift down each node
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            SiftDown(array, n, i);
        }
    }

    private static void SiftDown(int[] array, int n, int i)
    {
        int largest = i;        
        int left = 2 * i + 1;  
        int right = 2 * i + 2; 

        // If left child is larger than root
        if (left < n && array[left] > array[largest])
            largest = left;

        // If right child is larger than largest so far
        if (right < n && array[right] > array[largest])
            largest = right;

        // If largest is not root
        if (largest != i)
        {
            int swap = array[i];
            array[i] = array[largest];
            array[largest] = swap;

            // Recursively sift down the affected sub-tree
            SiftDown(array, n, largest);
        }
    }
}