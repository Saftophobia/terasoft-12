using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    class NoobAdder
    {
        public int number;

        public NoobAdder()
        {
            this.number = 0;
        }

        public void noobAdder()
        {
            for (int i = 0; i < 5; i++)
            {
                number += i;
            }
        }
    }
}
