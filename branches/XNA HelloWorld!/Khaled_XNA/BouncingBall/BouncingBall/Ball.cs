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

namespace BouncingBall
{
    class Ball
    {
        Vector2 Position;
        Texture2D Texture;
        int StageWidth, StageHeight;
        int velX = 5;
        int velY = 5;

        public Ball(Texture2D texture, Vector2 position, int stageWidth, int stageHeight)
        {
            Position = position;
            Texture = texture;
            StageWidth = stageWidth;
            StageHeight = stageHeight;
        }
        public void Update(GameTime gameTime)
        {
            Position.X += velX;
            Position.Y += velY;

            if (Position.X + Texture.Width >= StageWidth)
            {
                velX = -velX;
            }
            else if (Position.X <= 0)
            {
                velX = 90;
            }
            if (Position.Y + Texture.Height >= StageHeight)
            {
                velY = -velY;
            }
            else if (Position.Y <= 0)
            {
                velY = 90;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.End();

        }
    }
}