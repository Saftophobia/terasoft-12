using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project2
{
    class Class1
    {
        public int fib(int a)
        {
            if (a < 0)
                return -1;
            if (a < 2)
                return a;
            else
                return fib(a - 1) + fib(a - 2);
        }
        public static void Main(String[] args)
        {
             Class1 x = new Class1();
             int a = x.fib(12);
             Console.Out.WriteLine(a);
             Console.ReadLine();
            
        }
    }
}
