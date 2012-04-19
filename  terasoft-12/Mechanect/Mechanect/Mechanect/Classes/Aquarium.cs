 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Mechanect.Classes
{
    class Aquarium
    {
    
        Point location;
        int length;
        int width;


        public Aquarium(Point location, int width, int length)
        {
            this.location = location;
            this.length = length;
            this.width = width;
        }
        /// <summary>
        /// returns the location of the Aquarium.
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
        /// returns the height of the Aquarium.
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
        /// returns the width of the Aquarium.
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
