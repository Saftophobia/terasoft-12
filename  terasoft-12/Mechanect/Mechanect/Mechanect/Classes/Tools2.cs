 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Classes
{
    class Tools2
    {
    
       /// <summary>
        /// Generate 2D point randomly through given range.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point generate2Dpoint(int x,int y)
        {
            Random rand = new Random();
           int xpoint = rand.Next(x,y);
           int ypoint = rand.Next(x,y);
            Point point = new Point(xpoint,ypoint);

            return point;

        }
    }
}
