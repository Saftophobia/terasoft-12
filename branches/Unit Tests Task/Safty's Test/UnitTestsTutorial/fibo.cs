using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestsTutorial
{
    class fibo
    {
        
        public fibo()
        {
            
        }

        public int getFibonacci(int n)
        {
            if (n == 0 || n == 1)
                return n;
            else
                return getFibonacci(n - 1) + getFibonacci(n - 2);
        }
        
    }
}
