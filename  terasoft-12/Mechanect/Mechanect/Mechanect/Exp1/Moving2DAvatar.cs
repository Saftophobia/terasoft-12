using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Exp1
{
    public class Moving2DAvatar
    {
        private Vector2 Position;
        private Texture2D Texture;

        public Moving2DAvatar(Texture2D texture, Vector2 position)
        {
            Position = position;
            Texture = texture;
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
        public void Move(Vector2 value)
        {
            Position.Y -= value.Y;
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
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.End();
        }
    }
}
