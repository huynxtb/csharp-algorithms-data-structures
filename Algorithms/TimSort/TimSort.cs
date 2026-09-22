using System;
using System.Collections.Generic;

/// <summary>
/// Provides a generic, high-performance, stable implementation of the TimSort algorithm.
/// </summary>
/// <typeparam name="T">The type of elements in the array.</typeparam>
public class TimSorter<T>
{
    private const int MIN_MERGE = 32;
    private const int MIN_GALLOP = 7;

    private readonly T[] a;
    private readonly IComparer<T> c;
    private int minGallop = MIN_GALLOP;

    private T[] tmp;
    private int tmpBase;
    private int tmpLen;

    private int stackSize = 0;
    private readonly int[] runBase;
    private readonly int[] runLen;

    private TimSorter(T[] a, IComparer<T> c)
    {
        this.a = a;
        this.c = c ?? Comparer<T>.Default;
        int len = a.Length;

        int stackLen = (len < 120 ? 5 : len < 1542 ? 10 : len < 119151 ? 24 : 49);
        this.runBase = new int[stackLen];
        this.runLen = new int[stackLen];
    }

    /// <summary>
    /// Sorts the specified array into ascending order using the default comparer.
    /// </summary>
    /// <param name="array">The array to be sorted.</param>
    public static void Sort(T[] array)
    {
        Sort(array, Comparer<T>.Default);
    }

    /// <summary>
    /// Sorts the specified array into ascending order according to the specified comparer.
    /// </summary>
    /// <param name="array">The array to be sorted.</param>
    /// <param name="comparer">The comparer to determine the order of the elements.</param>
    public static void Sort(T[] array, IComparer<T> comparer)
    {
        if (array == null || array.Length < 2)
        {
            return;
        }

        int nRemaining = array.Length;
        if (nRemaining < MIN_MERGE)
        {
            int initRunLen = CountRunAndMakeAscending(array, 0, nRemaining, comparer ?? Comparer<T>.Default);
            BinarySort(array, 0, nRemaining, 0 + initRunLen, comparer ?? Comparer<T>.Default);
            return;
        }

        TimSorter<T> ts = new TimSorter<T>(array, comparer);
        int minRun = GetMinRunLength(nRemaining);
        int lo = 0;

        do
        {
            int runLength = CountRunAndMakeAscending(array, lo, lo + nRemaining, ts.c);
            if (runLength < minRun)
            {
                int force = nRemaining <= minRun ? nRemaining : minRun;
                BinarySort(array, lo, lo + force, lo + runLength, ts.c);
                runLength = force;
            }

            ts.PushRun(lo, runLength);
            ts.MergeCollapse();

            lo += runLength;
            nRemaining -= runLength;
        } while (nRemaining > 0);

        ts.MergeForceCollapse();
    }

    /// <summary>
    /// Calculates the minimum run length for TimSort.
    /// </summary>
    /// <param name="n">The length of the array to be sorted.</param>
    /// <returns>A value between 32 and 64 such that N/minRun is slightly less than or equal to a power of 2.</returns>
    public static int GetMinRunLength(int n)
    {
        int r = 0;
        while (n >= MIN_MERGE)
        {
            r |= (n & 1);
            n >>= 1;
        }
        return n + r;
    }

    private static int CountRunAndMakeAscending(T[] a, int lo, int hi, IComparer<T> c)
    {
        int runHi = lo + 1;
        if (runHi == hi)
        {
            return 1;
        }

        if (c.Compare(a[runHi++], a[lo]) < 0)
        {
            // Strictly descending: reverse in place to make ascending.
            while (runHi < hi && c.Compare(a[runHi], a[runHi - 1]) < 0)
            {
                runHi++;
            }
            ReverseRange(a, lo, runHi);
        }
        else
        {
            // Ascending (equal elements allowed).
            while (runHi < hi && c.Compare(a[runHi], a[runHi - 1]) >= 0)
            {
                runHi++;
            }
        }

        return runHi - lo;
    }

    private static void ReverseRange(T[] a, int lo, int hi)
    {
        hi--;
        while (lo < hi)
        {
            T t = a[lo];
            a[lo++] = a[hi];
            a[hi--] = t;
        }
    }

    private static void BinarySort(T[] a, int lo, int hi, int start, IComparer<T> c)
    {
        if (start == lo)
        {
            start++;
        }

        for (; start < hi; start++)
        {
            T pivot = a[start];
            int left = lo;
            int right = start;

            while (left < right)
            {
                int mid = (left + right) >>> 1;
                if (c.Compare(pivot, a[mid]) < 0)
                {
                    right = mid;
                }
                else
                {
                    left = mid + 1;
                }
            }

            int n = start - left;
            if (n > 0)
            {
                Array.Copy(a, left, a, left + 1, n);
            }
            a[left] = pivot;
        }
    }

    private void PushRun(int baseIndex, int len)
    {
        runBase[stackSize] = baseIndex;
        runLen[stackSize] = len;
        stackSize++;
    }

    private void MergeCollapse()
    {
        while (stackSize > 1)
        {
            int n = stackSize - 2;
            if ((n > 0 && runLen[n - 1] <= runLen[n] + runLen[n + 1]) ||
                (n > 1 && runLen[n - 2] <= runLen[n - 1] + runLen[n]))
            {
                if (runLen[n - 1] < runLen[n + 1])
                {
                    n--;
                }
                MergeAt(n);
            }
            else if (runLen[n] <= runLen[n + 1])
            {
                MergeAt(n);
            }
            else
            {
                break;
            }
        }
    }

    private void MergeForceCollapse()
    {
        while (stackSize > 1)
        {
            int n = stackSize - 2;
            if (n > 0 && runLen[n - 1] < runLen[n + 1])
            {
                n--;
            }
            MergeAt(n);
        }
    }

    private void MergeAt(int i)
    {
        int base1 = runBase[i];
        int len1 = runLen[i];
        int base2 = runBase[i + 1];
        int len2 = runLen[i + 1];

        runLen[i] = len1 + len2;
        if (i == stackSize - 3)
        {
            runBase[i + 1] = runBase[i + 2];
            runLen[i + 1] = runLen[i + 2];
        }
        stackSize--;

        int k = GallopRight(a[base2], a, base1, len1, 0, c);
        base1 += k;
        len1 -= k;
        if (len1 == 0)
        {
            return;
        }

        len2 = GallopLeft(a[base1 + len1 - 1], a, base2, len2, len2 - 1, c);
        if (len2 == 0)
        {
            return;
        }

        if (len1 <= len2)
        {
            MergeLo(base1, len1, base2, len2);
        }
        else
        {
            MergeHi(base1, len1, base2, len2);
        }
    }

    private int GallopLeft(T key, T[] a, int baseIndex, int len, int hint, IComparer<T> c)
    {
        int lastOfs = 0;
        int ofs = 1;
        if (c.Compare(key, a[baseIndex + hint]) > 0)
        {
            int maxOfs = len - hint;
            while (ofs < maxOfs && c.Compare(key, a[baseIndex + hint + ofs]) > 0)
            {
                lastOfs = ofs;
                ofs = (ofs << 1) + 1;
                if (ofs <= 0)
                {
                    ofs = maxOfs;
                }
            }
            if (ofs > maxOfs)
            {
                ofs = maxOfs;
            }

            lastOfs += hint;
            ofs += hint;
        }
        else
        {
            int maxOfs = hint + 1;
            while (ofs < maxOfs && c.Compare(key, a[baseIndex + hint - ofs]) <= 0)
            {
                lastOfs = ofs;
                ofs = (ofs << 1) + 1;
                if (ofs <= 0)
                {
                    ofs = maxOfs;
                }
            }
            if (ofs > maxOfs)
            {
                ofs = maxOfs;
            }

            int tmp = lastOfs;
            lastOfs = hint - ofs;
            ofs = hint - tmp;
        }

        lastOfs++;
        while (lastOfs < ofs)
        {
            int m = lastOfs + ((ofs - lastOfs) >>> 1);
            if (c.Compare(key, a[baseIndex + m]) > 0)
            {
                lastOfs = m + 1;
            }
            else
            {
                ofs = m;
            }
        }
        return ofs;
    }

    private int GallopRight(T key, T[] a, int baseIndex, int len, int hint, IComparer<T> c)
    {
        int ofs = 1;
        int lastOfs = 0;
        if (c.Compare(key, a[baseIndex + hint]) < 0)
        {
            int maxOfs = hint + 1;
            while (ofs < maxOfs && c.Compare(key, a[baseIndex + hint - ofs]) < 0)
            {
                lastOfs = ofs;
                ofs = (ofs << 1) + 1;
                if (ofs <= 0)
                {
                    ofs = maxOfs;
                }
            }
            if (ofs > maxOfs)
            {
                ofs = maxOfs;
            }

            int tmp = lastOfs;
            lastOfs = hint - ofs;
            ofs = hint - tmp;
        }
        else
        {
            int maxOfs = len - hint;
            while (ofs < maxOfs && c.Compare(key, a[baseIndex + hint + ofs]) >= 0)
            {
                lastOfs = ofs;
                ofs = (ofs << 1) + 1;
                if (ofs <= 0)
                {
                    ofs = maxOfs;
                }
            }
            if (ofs > maxOfs)
            {
                ofs = maxOfs;
            }

            lastOfs += hint;
            ofs += hint;
        }

        lastOfs++;
        while (lastOfs < ofs)
        {
            int m = lastOfs + ((ofs - lastOfs) >>> 1);
            if (c.Compare(key, a[baseIndex + m]) < 0)
            {
                ofs = m;
            }
            else
            {
                lastOfs = m + 1;
            }
        }
        return ofs;
    }

    private void MergeLo(int base1, int len1, int base2, int len2)
    {
        T[] a = this.a;
        T[] tmp = EnsureCapacity(len1);
        Array.Copy(a, base1, tmp, 0, len1);

        int cursor1 = 0;
        int cursor2 = base2;
        int dest = base1;

        a[dest++] = a[cursor2++];
        if (--len2 == 0)
        {
            Array.Copy(tmp, cursor1, a, dest, len1);
            return;
        }
        if (len1 == 1)
        {
            Array.Copy(a, cursor2, a, dest, len2);
            a[dest + len2] = tmp[cursor1];
            return;
        }

        IComparer<T> c = this.c;
        int minGallop = this.minGallop;

        while (true)
        {
            int count1 = 0;
            int count2 = 0;

            do
            {
                if (c.Compare(a[cursor2], tmp[cursor1]) < 0)
                {
                    a[dest++] = a[cursor2++];
                    count2++;
                    count1 = 0;
                    if (--len2 == 0)
                    {
                        goto OuterBreak;
                    }
                }
                else
                {
                    a[dest++] = tmp[cursor1++];
                    count1++;
                    count2 = 0;
                    if (--len1 == 1)
                    {
                        goto OuterBreak;
                    }
                }
            } while ((count1 | count2) < minGallop);

            do
            {
                count1 = GallopRight(a[cursor2], tmp, cursor1, len1, 0, c);
                if (count1 != 0)
                {
                    Array.Copy(tmp, cursor1, a, dest, count1);
                    dest += count1;
                    cursor1 += count1;
                    len1 -= count1;
                    if (len1 <= 1)
                    {
                        goto OuterBreak;
                    }
                }
                a[dest++] = a[cursor2++];
                if (--len2 == 0)
                {
                    goto OuterBreak;
                }

                count2 = GallopLeft(tmp[cursor1], a, cursor2, len2, 0, c);
                if (count2 != 0)
                {
                    Array.Copy(a, cursor2, a, dest, count2);
                    dest += count2;
                    cursor2 += count2;
                    len2 -= count2;
                    if (len2 == 0)
                    {
                        goto OuterBreak;
                    }
                }
                a[dest++] = tmp[cursor1++];
                if (--len1 == 1)
                {
                    goto OuterBreak;
                }
                minGallop--;
            } while (count1 >= MIN_GALLOP | count2 >= MIN_GALLOP);

            if (minGallop < 0)
            {
                minGallop = 0;
            }
            minGallop += 2;
        }

    OuterBreak:
        this.minGallop = minGallop < 1 ? 1 : minGallop;

        if (len1 == 1)
        {
            Array.Copy(a, cursor2, a, dest, len2);
            a[dest + len2] = tmp[cursor1];
        }
        else if (len1 == 0)
        {
            throw new InvalidOperationException("Comparison method violates its general contract!");
        }
        else
        {
            Array.Copy(tmp, cursor1, a, dest, len1);
        }
    }

    private void MergeHi(int base1, int len1, int base2, int len2)
    {
        T[] a = this.a;
        T[] tmp = EnsureCapacity(len2);
        Array.Copy(a, base2, tmp, 0, len2);

        int cursor1 = base1 + len1 - 1;
        int cursor2 = len2 - 1;
        int dest = base2 + len2 - 1;

        a[dest--] = a[cursor1--];
        if (--len1 == 0)
        {
            Array.Copy(tmp, 0, a, dest - (len2 - 1), len2);
            return;
        }
        if (len2 == 1)
        {
            dest -= len1;
            cursor1 -= len1;
            Array.Copy(a, cursor1 + 1, a, dest + 1, len1);
            a[dest] = tmp[cursor2];
            return;
        }

        IComparer<T> c = this.c;
        int minGallop = this.minGallop;

        while (true)
        {
            int count1 = 0;
            int count2 = 0;

            do
            {
                if (c.Compare(tmp[cursor2], a[cursor1]) < 0)
                {
                    a[dest--] = a[cursor1--];
                    count1++;
                    count2 = 0;
                    if (--len1 == 0)
                    {
                        goto OuterBreak;
                    }
                }
                else
                {
                    a[dest--] = tmp[cursor2--];
                    count2++;
                    count1 = 0;
                    if (--len2 == 1)
                    {
                        goto OuterBreak;
                    }
                }
            } while ((count1 | count2) < minGallop);

            do
            {
                count1 = len1 - GallopRight(tmp[cursor2], a, base1, len1, len1 - 1, c);
                if (count1 != 0)
                {
                    dest -= count1;
                    cursor1 -= count1;
                    len1 -= count1;
                    Array.Copy(a, cursor1 + 1, a, dest + 1, count1);
                    if (len1 == 0)
                    {
                        goto OuterBreak;
                    }
                }
                a[dest--] = tmp[cursor2--];
                if (--len2 == 1)
                {
                    goto OuterBreak;
                }

                count2 = len2 - GallopLeft(a[cursor1], tmp, 0, len2, len2 - 1, c);
                if (count2 != 0)
                {
                    dest -= count2;
                    cursor2 -= count2;
                    len2 -= count2;
                    Array.Copy(tmp, cursor2 + 1, a, dest + 1, count2);
                    if (len2 <= 1)
                    {
                        goto OuterBreak;
                    }
                }
                a[dest--] = a[cursor1--];
                if (--len1 == 0)
                {
                    goto OuterBreak;
                }
                minGallop--;
            } while (count1 >= MIN_GALLOP | count2 >= MIN_GALLOP);

            if (minGallop < 0)
            {
                minGallop = 0;
            }
            minGallop += 2;
        }

    OuterBreak:
        this.minGallop = minGallop < 1 ? 1 : minGallop;

        if (len2 == 1)
        {
            dest -= len1;
            cursor1 -= len1;
            Array.Copy(a, cursor1 + 1, a, dest + 1, len1);
            a[dest] = tmp[cursor2];
        }
        else if (len2 == 0)
        {
            throw new InvalidOperationException("Comparison method violates its general contract!");
        }
        else
        {
            Array.Copy(tmp, 0, a, dest - (len2 - 1), len2);
        }
    }

    private T[] EnsureCapacity(int minCapacity)
    {
        if (tmp == null || tmp.Length < minCapacity)
        {
            int newSize = minCapacity;
            newSize |= newSize >> 1;
            newSize |= newSize >> 2;
            newSize |= newSize >> 4;
            newSize |= newSize >> 8;
            newSize |= newSize >> 16;
            newSize++;

            if (newSize < 0)
            {
                newSize = minCapacity;
            }
            else
            {
                newSize = Math.Min(newSize, a.Length >>> 1);
            }

            tmp = new T[Math.Max(newSize, minCapacity)];
        }
        return tmp;
    }
}