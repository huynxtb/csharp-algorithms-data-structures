using System;
using System.Collections.Generic;
using System.Linq;
public class LZWCompression
{
    public static List<int> Compress(string input)
    {
        var dictionary = new Dictionary<string, int>();
        for (int i = 0; i < 256; i++)
        {
            dictionary.Add(((char)i).ToString(), i);
        }
        var result = new List<int>();
        var w = "";
        foreach (var c in input)
        {
            var wc = w + c;
            if (dictionary.ContainsKey(wc))
            {
                w = wc;
            }
            else
            {
                result.Add(dictionary[w]);
                dictionary.Add(wc, dictionary.Count);
                w = c.ToString();
            }
        }
        if (!string.IsNullOrEmpty(w))
        {
            result.Add(dictionary[w]);
        }
        return result;
    }
    public static string Decompress(List<int> compressed)
    {
        var dictionary = new Dictionary<int, string>();
        for (int i = 0; i < 256; i++)
        {
            dictionary.Add(i, ((char)i).ToString());
        }
        var result = "";
        var w = ((char)compressed[0]).ToString();
        result += w;
        for (int i = 1; i < compressed.Count; i++)
        {
            var k = compressed[i];
            string entry;
            if (dictionary.ContainsKey(k))
            {
                entry = dictionary[k];
            }
            else if (k == dictionary.Count)
            {
                entry = w + w[0];
            }
            else
            {
                throw new Exception("Invalid compressed k: " + k);
            }
            result += entry;
            dictionary.Add(dictionary.Count, w + entry[0]);
            w = entry;
        }
        return result;
    }
}