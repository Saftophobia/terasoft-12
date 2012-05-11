using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace unittest
{
    class Program
    {
       

        public int  fibo(int n)
        {
            if (n < 2)
                return n;
            else
                return fibo(n - 1) + fibo(n - 2);
        }
        public static void Main()
        {

        }

   
    }
}
