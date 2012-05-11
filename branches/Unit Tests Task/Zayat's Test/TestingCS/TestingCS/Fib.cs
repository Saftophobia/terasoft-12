using System;
using System.Collections.Generic;
using System.Linq;


namespace TestingCS
{
    public  static class Fib
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Int64 x = fibDP(5);
            Console.WriteLine(x);
            
        }

        public static Int64 fib(int n)
        {
            if (n < 0)
            {
                return -1;
            }
            if (n < 2)
            {
                return n;
            }
            return fib(n - 1) + fib(n - 2);
        }
        public static Int64 fibDP(int n)
        {
            if (n < 0)
            {
                return -1;
            }
            List<Int64> fib = new List<Int64>();
            fib.Add(0);
            fib.Add(1);
            for (int i = 2; i <= n; i++)
            {
                //Console.WriteLine(i);
                fib.Add(fib[i - 1] + fib[i - 2]);
            }
            return fib[n];

        }
        
    }
}
