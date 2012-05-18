using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Exp1
{
    class Moving2DAvatar
    {
        Vector2 Position;
        Texture2D Texture;

        public Moving2DAvatar(Texture2D a, Vector2 b)
        {
            Position = b;
            Texture = a;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 17/5/2012</para>
        /// <para>Date Modified 17/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Move is used to move the avatar in along the Y-axis.
        /// </summary>
        /// <param name="value">The value to be decremented from the Y position of the avatar.</param>
        /// <returns>void.</returns>
        public void Move(int value)
        {
            Position.Y -= value;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 17/5/2012</para>
        /// <para>Date Modified 17/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Draw is used to draw the 2D avatar.
        /// </summary>
        /// <param name="value">An instance of the spriteBatch.</param>
        /// <returns>void.</returns>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();
        }
    }
}
