using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace UI
{
    public static class UILib
    {
        private static SpeechSynthesizer speechSynthesizer;
        private static SpriteFont spriteFont;
        /// <summary>
        /// This method takes a string of text and reads it to the user.
        /// </summary>
        ///  <remarks>
        /// <para>AUTHOR: Mohamed Alzayat </para>   
        /// <para>DATE WRITTEN: May, 13 </para>
        /// <para>DATE MODIFIED: May, 13  </para>
        /// </remarks>
        /// <param name="text">The string of text to be read out loud.</param>
        /// <returns> true if the string was said and false if it wasn't.</returns>
        public static bool SayText(string text)
        {
            if (speechSynthesizer == null)
            {
                speechSynthesizer = new SpeechSynthesizer();
            }
            try
            {
                speechSynthesizer.Volume = 100;
                speechSynthesizer.SpeakAsync(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public static void Write(string text, Rectangle position, SpriteBatch spriteBatch, ContentManager contentManager)
        {
            if (spriteFont == null)
            {
                spriteFont = contentManager.Load<SpriteFont>("Ariel");
            }
            spriteBatch.DrawString(spriteFont, WrapText(text, position), new Vector2(position.X, position.Y), Color.Black);

        }
        private static string WrapText(string text, Rectangle position)
        {
            string line = string.Empty;
            string returnString = string.Empty;
            string[] wordArray = text.Split(' ');

            foreach (string word in wordArray)
            {
                if (spriteFont.MeasureString(line + word).Length() > position.Width)
                {
                    returnString += line + '\n';
                    line = string.Empty;
                }

                line += word + ' ';
            }

            returnString += line;
            return returnString;
        }

        
    }
}
