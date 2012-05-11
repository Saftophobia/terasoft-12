using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest
{
    public class power
    {
        int num;
        int result;
        public power(int n)
        {
            num = n;
            result = 1;
        }

        public int twoPower()
        {
            for (int i = 0; i < num; i++)
            {
                result *= 2;
            }
            return result;
            
        }


    }
}
