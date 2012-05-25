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
                speechSynthesizer.Volume = 100;
            }
            try
            {
                speechSynthesizer.SpeakAsync(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        /// Public, static, and main method to handle the the text wrapping.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="text">String containing instructions</param>
        /// <param name="position">Position on the screen for the text to be dsiplayed</param>
        /// <param name="spriteBatch">Sprite batch to draw the string</param>
        /// <param name="contentManager">Content</param>
         [System.Obsolete("will be replaced by  public Instruction(string text, Rectangle position, SpriteBatch spriteBatch, SpriteFont spriteFont, Color color)", false)]
        public static void Write(string text, Rectangle position, SpriteBatch spriteBatch, ContentManager contentManager,
            SpriteFont spriteFont, Color color)
        {

            spriteBatch.DrawString(spriteFont, WrapText(text, position, spriteFont), new Vector2(position.X, position.Y), color);

        }

         /// <summary>
         /// Public, static, and main method to handle the the text wrapping.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Mohamed Raafat</para>
         /// </remarks>
         /// <param name="text">String containing instructions</param>
         /// <param name="position">Position on the screen for the text to be dsiplayed</param>
         /// <param name="spriteBatch">Sprite batch to draw the string</param>
         public static void Write(string text, Rectangle position, SpriteBatch spriteBatch, SpriteFont spriteFont, Color color)
         {

             spriteBatch.DrawString(spriteFont, WrapText(text, position, spriteFont), new Vector2(position.X, position.Y), color);

         }

        /// <summary>
        /// Helper method that contains the algorithm for handeling the text wraping
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private static string WrapText(string text, Rectangle position,SpriteFont spriteFont)
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
