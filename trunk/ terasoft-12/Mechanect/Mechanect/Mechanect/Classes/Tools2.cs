 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    class Tools2
    {
    
      
        
        /// <summary>
        /// Generate 2D vector point randomly through given range.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <returns>returns Vector2 with the random x and y</returns>
        
        public Vector2 Generate2Dpoint(float minX, float maxX, float minY, float maxY)
        {
            Vector2 vec = new Vector2();
            Random rand = new Random();
           
             vec.X = rand.Next((int)minX,(int)maxX);
            vec.Y = rand.Next((int)minY,(int)maxY);
            return vec;

        }
    }
}

