using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    public class Maths
    {

        public static int Fibonacci(int n)
        {
            if (n < 2) return n;
            else return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

    }
}
