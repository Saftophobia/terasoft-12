using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mechanect.Exp2;
namespace Mechanect.Exp2
{
    public class Prey
    {
        Vector2 location;
        Texture2D preyTexture;
        float angle;
        public float Angle
        {
            set
            {
                angle = value;
            }
            get
            {
                return angle;
            }
        }

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
        /// <summary>
        /// Sets the texture for the Prey
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 17 </para>
        /// <para>DATE MODIFIED: May, 17  </para>
        /// </remarks>
        /// <param name="contentManager">A content Manager to get the texture from the directories</param>       
        public void setTexture(ContentManager contentManager)
        {
            preyTexture = contentManager.Load<Texture2D>("Textures/Experiment2/ImageSet1/worm");
        }

        /// <summary>
        /// Draws The scaled sprite batch
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 17 </para>
        /// <para>DATE MODIFIED: May, 17  </para>
        /// </remarks>
        /// <param name="mySpriteBatch"> The MySpriteBatch that will be used in drawing</param>
        /// <param name="location">The location of the drawing origin</param>
        /// <param name="scale"> The scaling of the texture</param>
        public void Draw(MySpriteBatch mySpriteBatch, Vector2 location, float scale)
        {
            mySpriteBatch.DrawTexture(preyTexture, location, angle, scale);
        }
    }
}
