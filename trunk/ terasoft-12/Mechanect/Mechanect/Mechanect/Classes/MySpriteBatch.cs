using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Mechanect.Common;
namespace Mechanect.Experiment2.UI
{
    class MySpriteBatch 
    {
        SpriteBatch SpriteBatch;
        /// <summary>
        /// A constructor that initializes the customized spriteBatch 
        /// </summary>
        /// <param name="SpriteBatch">The original sprite Batch used </param>
        MySpriteBatch(SpriteBatch SpriteBatch)
        {
            this.SpriteBatch = SpriteBatch;
        }
        /// <summary>
        /// Draws The object specified from it's origin
        /// </summary>
        /// <param name="texture2D">The texture to be drawn</param>
        /// <param name="position">The position of drawing on screen</param>
        /// <param name="angle">The rotation angle of your texture</param>
        /// <param name="scale">The scaling of the texture</param>
        void Draw(Texture2D texture2D, Vector2 position, float angle,float scale)
        {
            SpriteBatch.Draw(texture2D, position, null, Color.White, angle, CenterOfTexture(texture2D,position,scale), scale, SpriteEffects.None, 0);
        }
        /// <summary>
        /// Enables you to draw the Texture2D from it's origin
        /// N.B: This Method doesn't draw, it just returns a position Vector2 that will be used as origin in drawing.
        /// </summary>
        /// <param name="texture2D">Texture that needs to be drawn with the position at it's center</param>
        /// <param name="position">The position of the texture</param>
        /// <param name="scale">Takes the scaling of the Texture2D</param>
        /// <returns></returns>
        private Vector2 CenterOfTexture(Texture2D texture2D, Vector2 position, float scale)
        {
            return new Vector2(position.X + texture2D.Width*scale/2, position.Y + texture2D.Height*scale/2);          
        }
       
    }
}
