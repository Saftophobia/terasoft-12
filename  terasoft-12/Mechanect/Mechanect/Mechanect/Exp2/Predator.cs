using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Mechanect.Exp2
{
    /// <summary>
    /// Represents a predator (in this case a fish) that will be used to represent the projectile motion passing by it's target points
    /// </summary>
    public class Predator
    {

        private Vector2 location;
        private Vector2 velocity;

        private Texture2D fishTexture;

        public bool Movable { get; set; }

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
            Movable = true;
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
        /// <summary>
        /// Sets the texture for the Predator
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 17 </para>
        /// <para>DATE MODIFIED: May, 17  </para>
        /// </remarks>
        /// <param name="contentManager">A content Manager to get the texture from the directories</param>
        public void setTexture(ContentManager contentManager)
        {   
            fishTexture = contentManager.Load<Texture2D>("Textures/Experiment2/ImageSet1/fish");
        }

        /// <summary>
        /// Draws The scaled sprite batch
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 17 </para>
        /// <para>DATE MODIFIED: May, 22  </para>
        /// </remarks>
        /// <param name="mySpriteBatch"> The MySpriteBatch that will be used in drawing</param>
        /// <param name="location">The location of the drawing origin</param>
        /// <param name="scale"> The scaling of the texture</param>
        public void Draw(MySpriteBatch mySpriteBatch, Vector2 location, Vector2 scale)
        {
            mySpriteBatch.DrawTexture(fishTexture, location, MathHelper.ToRadians(-1 * (float)Angle),
                scale/fishTexture.Width);
        }
    }
}
