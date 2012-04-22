 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    public class Predator
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
        
        int size;

        Vector2 velocity;
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        
        public double Angle
        {
            get
            {
                if (velocity.X == 0) return 0;
                return Math.Atan(velocity.Y / velocity.X) * (180 / Math.PI);
            }
        }

        public Predator(Vector2 location)
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


        public Vector2 getLocation()
        {
            return location;
        }



        /// <summary>
        /// UpdatePosition is called in each frame when the predator is moving
        /// to update the location of the predator in each frame
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim </para>   
        /// <para>DATE WRITTEN: April, 21 </para>
        /// </remarks>
        public void UpdatePosition(GameTime gameTime)
        {
            location.X += (float)(velocity.X * gameTime.ElapsedGameTime.TotalSeconds);
            location.Y += (float)(velocity.Y * gameTime.ElapsedGameTime.TotalSeconds);
            velocity.Y -= (float)(9.8 * gameTime.ElapsedGameTime.TotalSeconds);
        }
    
    }
}
