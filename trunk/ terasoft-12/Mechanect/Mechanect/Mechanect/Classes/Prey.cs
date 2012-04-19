 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Mechanect.Classes
{
    class Prey
    {
        Point location;
        int length;
        int width;


        public Prey(Point location, int width, int length)
        {
            this.location = location;
            this.length = length;
            this.width = width;
        }
        /// <summary>
        /// returns the location of the prey.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns></returns>
        public Point getLocation()
        {
            return location;
        }

        /// <summary>
        /// return the Height of the Prey
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns></returns>
        public int getHeight()
        {
            return length;
        }
        /// <summary>
        /// return the Width of the Prey
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns></returns>
        public int getWidth()
        {
            return width;
        }
    
    
    }
}
