 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Classes
{
     class Instructions : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Font1;
        Vector2 origin = new Vector2(0f, 0f);
        Vector2 positionInScreen;
        String text;




        public Instructions(String text, Vector2 positionInScreen)
        {
            
            Window.AllowUserResizing = true;
            this.text = text;
            this.positionInScreen = new Vector2(positionInScreen.X, positionInScreen.Y);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            int screenWidth = 800;
            int screenHeight = 400;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            this.IsMouseVisible = true;

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
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// </para>Author: Mohamed Raafat </para>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font1 = Content.Load<SpriteFont>("TimesNewRoman");

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <para>Author: Mohamed Raafat</para>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.YellowGreen);
            spriteBatch.Begin();
            string output = WrapText(Font1, this.text, this.GraphicsDevice.Viewport.Width);
            spriteBatch.DrawString(Font1, output, positionInScreen, Color.Black, 0, origin, 1f, SpriteEffects.None, 0.0f);
            spriteBatch.End();

        }

    }


}
