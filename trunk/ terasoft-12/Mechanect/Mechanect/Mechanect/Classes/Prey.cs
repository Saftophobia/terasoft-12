 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    public class Prey
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

        float length;
        public float Length
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
        float width;
        public float Width
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

        public Prey(Vector2 location, float width, float length)
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
        /// <returns>returns Vector2 postion of the Prey</returns>
        public Vector2 getLocation()
        {
            return location;
        }

        /// <summary>
        /// return the Height of the Prey
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns Height  of the prey</returns>
        public float getHeight()
        {
            return length;
        }
        /// <summary>
        /// return the Width of the Prey
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns Width of the prey</returns>
        public float getWidth()
        {
            return width;
        }
    
    
    }
}
