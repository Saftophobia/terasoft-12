using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Kinect;
using Mechanect.Common;

namespace Mechanect.Classes
{
    class drawstring
    {
        Vector2 Fontpos;
        public SpriteFont Font1;
        string output = "default";

        public drawstring(Vector2 Fontpos)
        {
            this.Fontpos = Fontpos;
        }


        public void Update(string typetoprint)
        {
            this.output = typetoprint;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            {

               
                Vector2 FontOrigin = Font1.MeasureString(output) / 2;
                // Draw the string
                
                spriteBatch.DrawString(Font1, output, Fontpos, Color.White,
                    0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
               
               
               
            }
        }


    }
}
