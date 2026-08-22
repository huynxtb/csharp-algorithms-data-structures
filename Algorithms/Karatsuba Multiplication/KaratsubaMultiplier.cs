using System;
using System.Text;

public class KaratsubaMultiplier
{
    public static string Multiply(string num1, string num2)
    {
        if (string.IsNullOrEmpty(num1) || string.IsNullOrEmpty(num2))
            return "0";

        num1 = RemoveLeadingZeros(num1);
        num2 = RemoveLeadingZeros(num2);

        if (num1 == "0" || num2 == "0")
            return "0";

        int len1 = num1.Length;
        int len2 = num2.Length;

        if (len1 <= 2 && len2 <= 2)
        {
            long val1 = long.Parse(num1);
            long val2 = long.Parse(num2);
            return (val1 * val2).ToString();
        }

        int n = Math.Max(len1, len2);
        if (n % 2 != 0) n++;

        string s1 = num1.PadLeft(n, '0');
        string s2 = num2.PadLeft(n, '0');

        int mid = n / 2;

        string a = s1.Substring(0, mid);
        string b = s1.Substring(mid);
        string c = s2.Substring(0, mid);
        string d = s2.Substring(mid);

        string z0 = Multiply(b, d);
        string z2 = Multiply(a, c);
        string z1 = Multiply(Add(a, b), Add(c, d));

        string middle = Subtract(Subtract(z1, z2), z0);

        string r1 = ShiftLeft(z2, n);
        string r2 = ShiftLeft(middle, mid);

        return RemoveLeadingZeros(Add(Add(r1, r2), z0));
    }

    private static string Add(string num1, string num2)
    {
        StringBuilder sb = new StringBuilder();
        int i = num1.Length - 1;
        int j = num2.Length - 1;
        int carry = 0;

        while (i >= 0 || j >= 0 || carry > 0)
        {
            int sum = carry;
            if (i >= 0) sum += num1[i--] - '0';
            if (j >= 0) sum += num2[j--] - '0';
            sb.Append(sum % 10);
            carry = sum / 10;
        }

        char[] arr = sb.ToString().ToCharArray();
        Array.Reverse(arr);
        return new string(arr);
    }

    private static string Subtract(string num1, string num2)
    {
        StringBuilder sb = new StringBuilder();
        int i = num1.Length - 1;
        int j = num2.Length - 1;
        int borrow = 0;

        while (i >= 0)
        {
            int digit1 = num1[i--] - '0';
            int digit2 = j >= 0 ? num2[j--] - '0' : 0;
            int diff = digit1 - digit2 - borrow;

            if (diff < 0)
            {
                diff += 10;
                borrow = 1;
            }
            else
            {
                borrow = 0;
            }
            sb.Append(diff);
        }

        char[] arr = sb.ToString().ToCharArray();
        Array.Reverse(arr);
        return RemoveLeadingZeros(new string(arr));
    }

    private static string ShiftLeft(string s, int shift)
    {
        if (s == "0") return "0";
        return s + new string('0', shift);
    }

    private static string RemoveLeadingZeros(string s)
    {
        if (string.IsNullOrEmpty(s)) return "0";
        int start = 0;
        while (start < s.Length - 1 && s[start] == '0')
        {
            start++;
        }
        return s.Substring(start);
    }
}