using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Classes
{
    class Ball
    {
        private int radius;
        public int Radius {
            get 
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
        private double mass;
        public double Mass {
            get
            {
                return mass;
            }
            set
            {
                mass = value;
            }
        }

    }
}
