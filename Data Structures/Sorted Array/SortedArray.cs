using System;

public class SortedArray {
    private int[] array;
    private int size;
    private int capacity;

    public SortedArray(int initialCapacity = 10) {
        capacity = initialCapacity;
        array = new int[capacity];
        size = 0;
    }

    public void Insert(int value) {
        if (size == capacity) {
            Resize();
        }
        int index = Array.BinarySearch(array, 0, size, value);
        if (index < 0) {
            index = ~index;
        }
        Array.Copy(array, index, array, index + 1, size - index);
        array[index] = value;
        size++;
    }

    public void Delete(int value) {
        int index = Search(value);
        if (index != -1) {
            Array.Copy(array, index + 1, array, index, size - index - 1);
            size--;
        }
    }

    public int Search(int value) {
        int index = Array.BinarySearch(array, 0, size, value);
        return index >= 0 ? index : -1;
    }

    public int Size() {
        return size;
    }

    private void Resize() {
        capacity *= 2;
        int[] newArray = new int[capacity];
        Array.Copy(array, newArray, size);
        array = newArray;
    }
}