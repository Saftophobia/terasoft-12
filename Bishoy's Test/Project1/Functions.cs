using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1
{
    class Functions
    {

        public static int Sum(int[] array)
        {
            if (array == null)
            {
                throw new ArgumentException();
            }
            int sum = 0;
            foreach (int i in array)
            {
                sum += i;
            }
            return sum;
        }
    }
}
