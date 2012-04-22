 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    public class Aquarium
    {
    
        Vector2 location;
        public Vector2 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }
        int length;
        public int Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }
        int width;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public Aquarium(Vector2 location, int width, int length)
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
        public Vector2 getLocation()
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
