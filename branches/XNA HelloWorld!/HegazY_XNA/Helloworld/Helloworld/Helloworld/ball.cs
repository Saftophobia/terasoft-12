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

namespace Helloworld
{
    class ball
    {
        Vector2 position, velocity;
        Texture2D texture;
        int stageWidth, stageHeight;

        public ball(Texture2D t, Vector2 p, Vector2 v, int sh, int sw)
        {
            position = p;
            velocity = v;
            texture = t;
            stageHeight = sh;
            stageWidth = sw;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();
        }

        public void update(GameTime gameTime)
        {
            position.X += velocity.X;
            position.Y += velocity.Y;
            if (position.X >= (stageWidth - texture.Width) || position.X <= 0)
            {
                velocity.X *= -1;
            }
            if (position.Y >= (stageHeight - texture.Height) || position.Y <= 0)
            {
                velocity.Y *= -1;
            }
        }
    }
}
