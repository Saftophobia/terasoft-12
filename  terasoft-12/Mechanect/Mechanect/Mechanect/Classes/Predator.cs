 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Mechanect.Classes
{
    class Predator
    {
    
        Point location;
        int size;

        public Predator(Point location)
        {
            this.location = location;

        }

        /// <summary>
        /// returns the location of the predator.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns></returns>
       

        public Point getLocation()
        {
            return location;
        }
    
    }
}
