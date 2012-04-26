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
        private ContentManager content;
        private Vector2 position;
        private Texture2D winningPicture;
        private Texture2D losingPicture;

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// constructor takes ContentManager to be able to load the pictures and the position of the word
        /// to display in the screen
        /// </summary>
        /// <param name="c">content manager</param>
        /// <param name="p">desired poistion of the word</param>
        public WorL(ContentManager c, Vector2 p)
        {
            content = c;
            position = p;
            winningPicture = content.Load<Texture2D>("Textures/WorL/winner");
            losingPicture = content.Load<Texture2D>("Textures/WorL/looser");
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// displaying the wining word on the screen
        /// </summary>
        /// <param name="spriteBatch">used to draw the picture</param>
        public void DislayIsWin(SpriteBatch spriteBatch, bool status)
        {
            spriteBatch.Begin();
            if (status)
                spriteBatch.Draw(winningPicture, position, Color.White);
            else
                spriteBatch.Draw(winningPicture, position, Color.White);
            spriteBatch.End();
        }
    }
}
