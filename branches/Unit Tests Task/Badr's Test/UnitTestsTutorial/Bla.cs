using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTestsTutorial
{
    class Bla
    {

        public int num;
        
        //sets num to a value between 5,6
        public Bla()
        {
            Random r = new Random();
            num = r.Next(5, 6);
        }

        public int Factorial()
        {
            if (num <= 1)
                return 1;
            return (num--) * Factorial();
        }

        
    }
}
