using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Mechanect;

namespace MechanectXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
     class Instruction :GameScreen
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice graphicsdevice;
        SpriteBatch spriteBatch;
        SpriteFont Font1;
        Vector2 origin;
        Vector2 positionInScreen;
        String instructions;
        Texture2D mytexture;
        Vector2 sPos = Vector2.Zero;
        Vector2 ButtonPosition = Vector2.Zero;
        Button b;
        ContentManager cmanager;
        Game game;


        public Instruction(String instructions, Vector2 positionInScreen)
        {
            game.Window.AllowUserResizing = true;
            this.instructions = instructions;
            this.positionInScreen = new Vector2(positionInScreen.X, positionInScreen.Y);
            this.graphics = new GraphicsDeviceManager(game);
            cmanager.RootDirectory = "Content";
            int screenWidth = 800;
            int screenHeight = 400;
            origin = new Vector2(0f, 0f);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
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

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphicsdevice);
            Font1 = cmanager.Load<SpriteFont>("TimesNewRoman");
            mytexture = cmanager.Load<Texture2D>(@"MechanectContent/Textures/Screen");
            b = new OKButton(cmanager, ButtonPosition, 960, 600);
            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <para>Author: Mohamed Raafat</para>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        String getInsructions()
        {
            String output = WrapText(Font1, this.instructions, graphicsdevice.Viewport.Width);
            return output;
        }
        
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Draws the instruction screen with an ok button beneathe the text.
        /// </summary>
        /// <param name="gameTime">
        /// An instance variable of class GameTime in Microsoft.Xna.Framework.GameTime package.
        /// </param>
        public override void Draw(GameTime gameTime)
        {
            graphicsdevice.Clear(Color.YellowGreen);
            spriteBatch.Begin();
            string output = WrapText(Font1, this.instructions, graphicsdevice.Viewport.Width);
            b.draw(spriteBatch);
            spriteBatch.DrawString(Font1, output, positionInScreen, Color.Black, 0, origin, 1f, SpriteEffects.None, 0.0f);
            spriteBatch.End();
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
