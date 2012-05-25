using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace Mechanect.Exp2
{
    /// <summary>
    /// Represents one of the targets that the predator has to pass by in order to win the game
    /// </summary>
    public class Prey
    {
        Vector2 location;
        float length;
        float width;
        public bool Eaten {get; set;}

        Texture2D preyTexture;
       
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
        /// <para>DATE MODIFIED: May, 22  </para>
        /// </remarks>
        /// <param name="mySpriteBatch"> The MySpriteBatch that will be used in drawing</param>
        /// <param name="location">The location of the drawing origin</param>
        /// <param name="scale"> The scaling of the texture</param>
        public void Draw(MySpriteBatch mySpriteBatch, Vector2 location, Vector2 scale)
        {
            mySpriteBatch.DrawTexture(preyTexture, location, 0, scale/preyTexture.Width);
        }
    }
}
