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

namespace XNA
{
    class Win
    {
        Boolean Exists;
        Vector2 Position;
        Texture2D Texture;
        int StageWidth, StageHeight;

        public Win(Texture2D a, Vector2 b, int c, int d)
        {
            Exists = true;
            Position = b;
            Texture = a;
            StageWidth = c;
            StageHeight = d;
        }

        public int getXPosition()
        {
            return (int)Position.X;
        }

        public int getYPosition()
        {
            return (int)Position.Y;
        }

        public int getTextureWidth()
        {
            return Texture.Width;
        }

        public Texture getTexture()
        {
            return Texture;
        }

        public void setVector(Vector2 P)
        {
            this.Position = P;
        }

        public void setExists(Boolean b)
        {
            Exists = b;
        }

        public Boolean getExists()
        {
            return Exists;
        }

        public void Update(GameTime gameTime)
        {
            if (Position.X > 110)
            {
                Position.X--;
            }
        }


        public void draw(SpriteBatch spriteBatch)
        {
            if (Exists)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Texture, Position, Color.White);
                spriteBatch.End();
            }
        }
    }
}
