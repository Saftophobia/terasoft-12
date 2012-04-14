using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HelloWorld_
{
    class MyWizard
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;

        public MyWizard(Vector2 Position, Vector2 Velocity, Texture2D Texture)
        {
            this.Position = Position;
            this.Velocity = Velocity;
            this.Texture = Texture;
        }

        public void Update()
        {
            this.Position += this.Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Texture,new Rectangle((int)Position.X,(int)Position.Y,Texture.Width,Texture.Height),Color.White);
            spriteBatch.End();
        }
    }
}
