using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace WindowsGame1
{
    class TrollGuy
    {
        Texture2D texture;
        Vector2 pos1;
        Vector2 pos2;
        float speed1;
        public TrollGuy(ContentManager Content)
        {
            speed1 = 3.0f;
            pos1 = Vector2.Zero;
            pos2 = Vector2.Zero;
            texture = Content.Load<Texture2D>(@"Images/epic_trollface");
        }
        public void update(GameWindow Window)
        {
            pos1.X += speed1;
            if (pos1.X > Window.ClientBounds.Width - texture.Width ||
            pos1.X < 0)
                speed1 *= -1;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture,
            pos1,
            null,
            Color.White,
            0,
            Vector2.Zero,
            1,
            SpriteEffects.None,
            0);
            spriteBatch.End();
        }
    }
}
