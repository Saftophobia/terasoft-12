using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Mechanect.Classes
{
    class CountDown
    {        
        Texture2D Texture;
        int StageWidth, StageHeight;
        Rectangle r;
        int v1;
        int v2;
        int v3;
        int v4;
        int counter;

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The constructor CountDown takes 8 parameters to create an instance 
        /// of the Countdown object
        /// </summary>
        /// <param name="tex">Represents the Texture2D of object</param>       
        /// <param name="a">Represents the back buffer width</param>
        /// <param name="b">Represents the back buffer height</param>
        /// <param name="v1">Represents the x co-ordinate of the object</param>
        /// <param name="v2">Represents the y co-ordinate of the object</param>
        /// <param name="v3">Represents width of the object</param>
        /// <param name="v4">Represents height of the object</param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        
        public CountDown(Texture2D tex, int a, int b, int v1, int v2, int v3, int v4)
        {
            
            Texture = tex;
            StageWidth = a;
            StageHeight = b;
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            r = new Rectangle(v1, v2, v3, v4);
            counter = r.Height;
            
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Update is used to decrement the counter in
        /// order to allow the next number to appearon the screen 
        /// when the counter reaches 0
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>

        public void Update()
        {


            counter=counter-4;

               
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetCounter is used to get the counter
        /// in order to decide whether the next number should
        /// appear on the screen or not yet
        /// </summary>
        /// <param name></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>int: returns the counter</returns>

        public int GetCounter()
        {
            return counter;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Draw is used to draw to Texture given
        /// the co-ordinates of the rectangle in which the texture
        /// will be drawn.
        /// </summary>
        /// <param name="spriteBatch"> An instance of the spriteBatch
        /// responsible for drawing the Texture</param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public void Draw(SpriteBatch spriteBatch)
        {            
                spriteBatch.Begin();
                spriteBatch.Draw(Texture, r, Color.White);
                spriteBatch.End();            
        }

    }
}
