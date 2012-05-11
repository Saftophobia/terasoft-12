using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shirin
{
    public class Fib
    {
         public Fib()
        {

        }

        public int GetFib (int n)
        {
            if (n == 0)
            {
                return 0;
            }
            if (n == 1)
            {
                return 1;
            }
            else
            {
                return GetFib(n - 1) + GetFib(n - 2);
            }
        }
    }
}
