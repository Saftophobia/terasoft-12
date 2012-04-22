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

namespace Mechanect.Common
{
    class WorL
    {
        ContentManager content;
        Vector2 position;


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// constructor takes ContentManager to be able to load the pics and the position
        /// </summary>
        /// <param name="c">content manager</param>
        /// <param name="p">poistion</param>
        public WorL(ContentManager c, Vector2 p)
        {
            content = c;
            position = p;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// displaying the wining word on the screen
        /// </summary>
        /// <param name="spriteBatch">to be able to draw</param>
        public void winningWord(SpriteBatch spriteBatch)
        {
            Texture2D pic = content.Load<Texture2D>("WorL/winner");
            spriteBatch.Begin();
            spriteBatch.Draw(pic, position, Color.White);
            spriteBatch.End();
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// displaying the losing word on the screen
        /// </summary>
        /// <param name="spriteBatch">to be able to draw</param>
        public void losingWord(SpriteBatch spriteBatch)
        {
            Texture2D pic = content.Load<Texture2D>("WorL/looser");
            spriteBatch.Begin();
            spriteBatch.Draw(pic, position, Color.White);
            spriteBatch.End();
        }
    }
}
