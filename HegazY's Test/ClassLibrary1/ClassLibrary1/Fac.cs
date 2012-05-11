using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    class Fac
    {
        public static int Factorial(int number)
        {
            if (number < 0)
                return -1;
            if (number == 0)
                return 1;
            return (Factorial(number - 1) * number);
        }
    }
}
