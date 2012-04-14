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
    class Ball 
    {        
        Vector2 Position;
        Texture2D Texture;
        int StageWidth, StageHeight;
        int velX = 5;
        int velY = 5;
        Boolean reverseHeight;
        Boolean hitBarrier;
        Boolean upperCollision;

        public Ball(Texture2D texture, Vector2 P, int w, int h)
        {
            reverseHeight = false;
            upperCollision = false;
            hitBarrier = false;
            Position = P;
            Texture = texture;
            StageWidth = w;
            StageHeight = h;
        }

        public void setUpper(Boolean x)
        {
            this.upperCollision = x;
        }

        public void setBarrier(Boolean x)
        {
            hitBarrier = x;
        }

        public void setReverseY(Boolean x)
        {
            reverseHeight = x;
        }

        public Vector2 getPosition()
        {
            return Position;
        }

        public void setPosition(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }



        public void setVector(Vector2 P)
        {
            this.Position = P;
        }








        public void Update(GameTime gameTime, Controller controller)
        {


            if (upperCollision&&Position.Y>0)
            {
                Position.Y -= 2*velY;
            }

            if (hitBarrier)
            {

                Position.Y += 2 * velY;
            }

            if ((int)Position.X >= (int)controller.getPosition().X-50 &&
                (int)Position.X < (int)controller.getPosition().X +
                controller.getTextureWidth() && (int)Position.Y == 370)
            {
                upperCollision = false;
                hitBarrier = false;
                reverseHeight = true;
            }




            Position.X += velX;
            if (Position.X + Texture.Width >= StageWidth)
            {
                velX--;
            }
            else if (Position.X <= 0)
            {
                velX++;
            }



            
            if (!reverseHeight)
            {
                Position.Y += velY;
            }
            if (reverseHeight)
            {
                Position.Y -= velY;
            }

            if (Position.Y <= 0)
            {
                reverseHeight = false;
                upperCollision = false;
                hitBarrier = false;
            }

        }        


        public void draw(SpriteBatch spriteBatch)
        {            
                spriteBatch.Begin();
                spriteBatch.Draw(Texture, Position, Color.White);
                spriteBatch.End();            
        }
    }
}
