using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Mechanect.Common;
namespace Mechanect.Exp2
{
    /// <summary>
    /// This class was created just to eliminate the duplication of the customized Draw method.
    /// </summary>
    public class MySpriteBatch
    {
        SpriteBatch SpriteBatch;
        /// <summary>
        /// A constructor that initializes the customized spriteBatch 
        /// </summary>
        /// <param name="SpriteBatch">The original sprite Batch used </param>
        public MySpriteBatch(SpriteBatch SpriteBatch)
        {
            this.SpriteBatch = SpriteBatch;
        }
        /// <summary>
        /// Draws the texture specified from it's origin
        /// </summary>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 17 </para>
        /// <para>DATE MODIFIED: May, 17  </para>
        /// </remarks>
        /// <param name="texture2D">The texture to be drawn</param>
        /// <param name="position">The position of drawing on screen (origin of drawing)</param>
        /// <param name="angle">The rotation angle of your texture</param>
        /// <param name="scale">The scaling of the texture</param>
        public void DrawTexture(Texture2D texture2D, Vector2 position, float angle, Vector2 scale)
        {
            SpriteBatch.Draw(texture2D, position, null, Color.White, angle, new Vector2(texture2D.Width, texture2D.Height)/2, scale, SpriteEffects.None, 0);
        }


    }
}
