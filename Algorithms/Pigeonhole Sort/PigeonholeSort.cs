public static class SortingAlgorithms {
    public static void PigeonholeSort(int[] array) {
        if (array == null || array.Length == 0) return;
        if (array.Length == 1) return;

        int min = array[0];
        int max = array[0];

        // Find the minimum and maximum values in the array
        for (int i = 1; i < array.Length; i++) {
            if (array[i] < min) min = array[i];
            if (array[i] > max) max = array[i];
        }

        // Create pigeonholes for the range of values
        int range = max - min + 1;
        int[] pigeonholes = new int[range];

        // Distribute the elements into the pigeonholes
        for (int i = 0; i < array.Length; i++) {
            pigeonholes[array[i] - min]++;
        }

        // Collect the elements from the pigeonholes back into the array
        int index = 0;
        for (int i = 0; i < pigeonholes.Length; i++) {
            while (pigeonholes[i] > 0) {
                array[index++] = i + min;
                pigeonholes[i]--;
            }
        }
    }
}