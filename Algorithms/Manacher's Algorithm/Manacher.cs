using System;
using System.Text;

public static class Manacher
{
    public static string FindLongestPalindromicSubstring(string s)
    {
        if (s == null)
        {
            throw new ArgumentNullException(nameof(s));
        }
        if (s.Length <= 1)
        {
            return s;
        }

        int[] p = GetPalindromeTable(s);
        int maxLen = 0;
        int centerIndex = 0;

        for (int i = 1; i < p.Length - 1; i++)
        {
            if (p[i] > maxLen)
            {
                maxLen = p[i];
                centerIndex = i;
            }
        }

        int startIndex = (centerIndex - 1 - maxLen) / 2;
        return s.Substring(startIndex, maxLen);
    }

    public static int[] GetPalindromeTable(string s)
    {
        if (s == null)
        {
            throw new ArgumentNullException(nameof(s));
        }

        string t = Preprocess(s);
        int[] p = new int[t.Length];
        int c = 0;
        int r = 0;

        for (int i = 1; i < t.Length - 1; i++)
        {
            int iMirror = 2 * c - i;
            if (r > i)
            {
                p[i] = Math.Min(r - i, p[iMirror]);
            }
            else
            {
                p[i] = 0;
            }

            while (t[i + 1 + p[i]] == t[i - 1 - p[i]])
            {
                p[i]++;
            }

            if (i + p[i] > r)
            {
                c = i;
                r = i + p[i];
            }
        }

        return p;
    }

    private static string Preprocess(string s)
    {
        if (s.Length == 0)
        {
            return "^$";
        }
        StringBuilder sb = new StringBuilder(s.Length * 2 + 3);
        sb.Append('^');
        for (int i = 0; i < s.Length; i++)
        {
            sb.Append('#');
            sb.Append(s[i]);
        }
        sb.Append("#$");
        return sb.ToString();
    }
}