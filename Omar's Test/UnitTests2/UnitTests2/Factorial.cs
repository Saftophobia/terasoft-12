using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTests2
{
    class Factorial
    {
        public Factorial()
        {
        }

        public int factorial(int num)
        {
            if (num <= 1) return 1;
            else return num * factorial(num - 1);
        }
    }
}
