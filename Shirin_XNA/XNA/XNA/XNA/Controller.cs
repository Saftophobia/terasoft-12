using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
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
    class Controller 
    {
        Vector2 Position;
        Texture2D Texture;
        int StageWidth, StageHeight;

        public Controller(Texture2D a, Vector2 b, int c, int d)
        {
             
            Position = b;
            Texture = a;
            StageWidth = c;
            StageHeight = d;
        }




        public Vector2 getPosition()
        {
            return Position;
        }

        public int getTextureWidth()
        {
            return Texture.Width;
        }

        public void setPosition(int x,int y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public void setVector(Vector2 P)
        {
            this.Position = P;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, Position, Microsoft.Xna.Framework.Color.White);
            spriteBatch.End();
        }
    }
}
