using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Mechanect;

namespace Mechanect
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class Instruction : GameScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont Font1;
        Vector2 origin;
        Vector2 positionInScreen;
        String instructions;
        //Texture2D mytexture;
        Vector2 sPos = Vector2.Zero;
        Vector2 ButtonPosition = new Vector2(300, 300);
        Button b;
        ContentManager cmanager;
        Game1 game = new Game1();
        int screenWidth = 800;
        int screenHeight = 400;

        public Vector2 PositionInScreen
        {
            get
            {
                return positionInScreen;
            }
            set
            {
                positionInScreen = value;

            }
        }

        public String Instructions
        {
            get
            {
                return instructions;
            }
            set
            {
                instructions = value;
            }
        }
        
        public Instruction()
        {
            game.Window.AllowUserResizing = true;
            origin = new Vector2(0f, 0f);
            game.IsMouseVisible = true;
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            spriteBatch = ScreenManager.SpriteBatch;
            cmanager = ScreenManager.Game.Content;
        }
        public Instruction(String instructions)
        {
            game.Window.AllowUserResizing = true;
            this.instructions = instructions;
            origin = new Vector2(0f, 0f);
            game.IsMouseVisible = true;
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

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Initializes the screen variables needed
        /// </summary>

        public override void Initialize()
        {
            screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;
            screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            spriteBatch = ScreenManager.SpriteBatch;
            cmanager = ScreenManager.Game.Content;
            b = new OKButton(cmanager, ButtonPosition, 960, 600);
            Font1 = cmanager.Load<SpriteFont>("spriteFont1");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// 

        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            //mytexture = cmanager.Load<Texture2D>(@"Textures/screen");
            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <para>Author: Mohamed Raafat</para>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        String getInsructions()
        {
            String output = WrapText(Font1, this.instructions, screenWidth);
            return output;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {

            if (b.isClicked())
            {
                this.Remove();
            }
            base.Update(gameTime, covered);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Draws the instruction screen and the text with an ok button down the screen.
        /// </summary>
        /// <param name="gameTime">
        /// An instance variable of class GameTime in Microsoft.Xna.Framework.GameTime package.
        /// </param>
        public override void Draw(GameTime gameTime)
        {
            //graphicsdevice.Clear(Color.YellowGreen);
            string output = WrapText(Font1, this.instructions, screenWidth);

            b.draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(Font1, output, positionInScreen, Color.Black, 0, origin, 1f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
        }

        public override void Remove()
        {
            base.Remove();
        }
        //static void Main(String[] args)
        //{
        //    String text = "mohamed raafah ahmed aboelhassieb";
        //    Vector2 position = new Vector2(12f, 43f);
        //    Game1 game = new Game1(text, position);
        //    game.Run();

        //}


    }

}
