using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Michel_s_Task
{
    class Fibonacci
    {
        public Fibonacci()
        {
        }

        public int GetFibonnacci(int i)
        {
            if (i < 0)
                return -1;
            else
            {
                if (i <= 1)
                    return 1;
                else
                {
                    return (GetFibonnacci(i - 1) + GetFibonnacci(i - 2));
                }
            }
        }
    }
}
