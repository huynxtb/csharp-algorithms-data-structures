using System;

public static class ExtendedEuclideanSolver
{ 
    public static (long Gcd, long X, long Y) Solve(long a, long b)
    { 
        if (a == 0 && b == 0)
        { 
            return (0, 0, 0);
        }

        long x0 = 1, y0 = 0;
        long x1 = 0, y1 = 1;
        long tempA = a;
        long tempB = b;

        while (tempB != 0)
        { 
            long q = tempA / tempB;
            long r = tempA % tempB;

            tempA = tempB;
            tempB = r;

            long nextX = x0 - q * x1;
            long nextY = y0 - q * y1;

            x0 = x1;
            y0 = y1;
            x1 = nextX;
            y1 = nextY;
        }

        if (tempA < 0)
        { 
            tempA = -tempA;
            x0 = -x0;
            y0 = -y0;
        }

        return (tempA, x0, y0);
    }
}