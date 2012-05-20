using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Mechanect.Exp2
{
    public class Aquarium
    {

        Vector2 location;
        Texture2D aquariumTexture;
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
        /// <summary>
        /// Sets the texture for the Aquarium
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 17 </para>
        /// <para>DATE MODIFIED: May, 17  </para>
        /// </remarks>
        /// <param name="contentManager">A content Manager to get the texture from the directories</param>
        public void setTexture(ContentManager contentManager)
        {
            aquariumTexture = contentManager.Load<Texture2D>("Textures/Experiment2/ImageSet1/Fishbowl");
        }


        // <summary>
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
            mySpriteBatch.DrawTexture(aquariumTexture, location, angle, scale);
        }
    }
}
