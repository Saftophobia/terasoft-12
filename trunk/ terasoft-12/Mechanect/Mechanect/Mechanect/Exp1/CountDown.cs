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

namespace Mechanect.Exp1
{
    class CountDown
    {
        Texture2D Texture;
        Rectangle r;
        int counter;
        SoundEffect effect1;
        SoundEffect effect2;
        Texture2D three;
        Texture2D two;
        Texture2D one;
        Texture2D go;

        public CountDown(Texture2D tex, int v1, int v2, int v3, int v4)
        {
            Texture = tex;
            r = new Rectangle(v1, v2, v3, v4);
            counter = r.Height;
        }

        public CountDown()
        {

        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 16/5/2012</para>
        /// <para>Date Modified 16/2012</para>
        /// </remarks>
        /// <summary>
        /// The function InitializeCountdown accepts a set of textures and sound effects to be used during the countdown.
        /// </summary>
        /// <param name = "Texthree">Number Three.</param>
        /// <param name="Textwo">Number Two.</param>
        /// <param name="Texone">Number One.</param>
        /// <param name="Texgo">The word Go.</param>
        /// <param name="Seffect1">An instance of the SoundEffect.</param>
        /// <param name="Seffect2">An instance of the SoundEffect.</param>
        /// <returns>void</returns>
        public void InitializeCountDown(Texture2D Texthree, Texture2D Textwo, Texture2D Texone, Texture2D Texgo, SoundEffect Seffect1, SoundEffect Seffect2)
        {
            three = Texthree;
            two = Textwo;
            one = Texone;
            go = Texgo;
            effect1 = Seffect1;
            effect2 = Seffect2;
            counter = 0;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 16/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Update is used to increment the counter in order to allow the next number to appear on the screen.
        /// </summary>
        /// <param></param> 
        /// <returns>void</returns>
        public void Update()
        {
            counter++;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Draw is used to draw a Texture given the co-ordinates of the rectangle in which the texture will be drawn.
        /// </summary>
        /// <param name="spriteBatch"> An instance of the spriteBatch class.</param>       
        /// <returns>void</returns>
        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Texture, r, Color.White);
                spriteBatch.End();
            }
            catch (Exception e)
            {
                try
                {
                    spriteBatch.Draw(Texture, r, Color.White);
                }
                catch (Exception f)
                {
                }
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 16/5/2012</para>
        /// <para>Date Modified 16/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function PlaySoundEffects is used to play sound effects during the countdown.
        /// </summary>  
        /// <returns>void</returns>
        public void PlaySoundEffects()
        {
            switch (counter)
            {
                case 4: effect1.Play(); break;
                case 54: effect1.Play(); break;
                case 104: effect1.Play(); break;
                case 154: effect2.Play(); break;
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 16/5/2012</para>
        /// <para>Date Modified 16/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function DrawCountdown is used to draw the numbers on the screen.
        /// </summary>  
        /// <returns>void</returns>
        public void DrawCountdown(SpriteBatch spriteBatch,int x, int y)
        {
            
            Rectangle r = new Rectangle(x, y, 140, 140);
           //if (counter < 170)
            //{
                if (counter >= 0 && counter < 50)
                {
                    spriteBatch.Draw(three, r, Color.White);
                }
                if (counter >= 50 && counter < 100)
                {
                    spriteBatch.Draw(two, r, Color.White);
                }
                if (counter >= 100 && counter < 150)
                {
                    spriteBatch.Draw(one, r, Color.White);
                }
                if (counter >= 150)
                {
                    spriteBatch.Draw(go, r, Color.White);
                }
            //}
            
        }
    }
}
