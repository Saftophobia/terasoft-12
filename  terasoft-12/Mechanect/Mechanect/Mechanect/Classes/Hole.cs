using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    class Hole
    {
        private int radius;
        public int Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
    }
}
