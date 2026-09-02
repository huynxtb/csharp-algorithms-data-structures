using System;
using System.Numerics;

public static class InterpolationUtils
{
    public static T Linear<T>(T val1, T val2, double weight) where T : INumber<T>
    {
        T w = T.CreateChecked(weight);
        return val1 + (val2 - val1) * w;
    }
}