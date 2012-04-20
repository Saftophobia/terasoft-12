using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MechanectXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        SpriteFont Font1;
        Vector2 positionInScreen;
        String instructions;

        public Game1(String instructions, Vector2 positionInScreen)
        {

            setInsruction(instructions);
            this.positionInScreen = new Vector2(positionInScreen.X, positionInScreen.Y);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Setter for the instruction
        /// </summary>
        /// <author>Mohamed Raafat </author>
        /// <param name="instruction"></param>

        void setInsruction(String instruction)
        {
            this.instructions = instruction;
        }

        /// <summary>
        /// Makes sure that text displayed will not exceeds screen boundries
        /// </summary>
        /// <para>AUTHOR: Mohamed Raafat </para>
        /// <param name="spriteFont">contains properties of the font</param>
        /// <param name="text">text to be displayed on screen</param>
        /// <param name="maxLineWidth">max line width to be displayed</param>
        /// <returns>builder</returns>
        public String WrapText(SpriteFont spriteFont, String text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder builder = new StringBuilder();
            float lineWidth = positionInScreen.X;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (String word in words)
            {
                Vector2 width = spriteFont.MeasureString(word);


                if (lineWidth + width.X < maxLineWidth)
                {
                    builder.Append(word + " ");
                    lineWidth += width.X + spaceWidth;

                }
                else
                {
                    builder.Append("\n" + word + " ");
                    lineWidth = 0;

                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// This Method gets the Instructions that will be drawn later
        /// </summary>
        /// <para>Author: Mohamed Raafat</para>
        /// <returns> Instructions </returns>
        String getInsructions()
        {
            String output = WrapText(Font1, this.instructions, this.GraphicsDevice.Viewport.Width);
            return output;
        }

    }

}
