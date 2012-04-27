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

        public Aquarium(Vector2 location, float width, float length)
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
        /// <returns>returns Vector2 postion of the Aquarium</returns>
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
        /// <returns>returns the height of of Aquarium</returns>
        public float getHeight()
        {
            return length;
        }
        /// <summary>
        /// returns the width of the Aquarium.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <returns>returns the width of the Aquarium</returns>
        public float getWidth()
        {
            return width;
        }
    }
}
