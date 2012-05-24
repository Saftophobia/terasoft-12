using Mechanect.Exp2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Mechanect.Exp3;
using UI;
using ButtonsAndSliders;
using Microsoft.Xna.Framework.Content;
namespace Mechanect.Screens
{
    class InstructionsScreen2 : GameScreen
    {
        SpriteBatch spriteBatch;
        GraphicsDevice graphics;
        int screenWidth;
        int screenHeight;
        ContentManager content;
        string header = "\n\n\n                  .Welcome To Mechanect Projectile Game" + '\n'
             + "                           .By TeraSoft Team." + '\n'
               + "                                 .GUC.";
        private string title1 = "\n Game Instructions:-";
        private string title2 = "\n General Instructions:-";
        private string text1 = "1-Goal: Use projectile equations to calculate  angle and velocity "+
            "that the fish can be thrown by to eat the prey and fall in the aquarium.\n \n \n"
            +"2-Givens: Predator Point,Prey Point,Aquarium point. \n\n \n" + 
            "3-Settings: The next screen will tell you how to adjust your position for the game.\n\n\n";
        private string text2 = "* The avatar on the top right \n  represents your distance from \n  the screen.";
        private string green = "Green: Good.";
        private string white = "White: Too Far.";
        private string red = "Red: Too Near.";
        private string crossed = "Crossed: Not Detected.";
        private User2 user2;
        private Texture2D myTexture;
        private Rectangle rect;
        private Button button;
        private float scale;
        SpriteFont font1;
        SpriteFont font2;
        SpriteFont font3;

        public InstructionsScreen2(User2 user2)
        {
            this.user2 = user2;
        }

        public InstructionsScreen2(string instructions, User2 user2)
            : this(user2)
        {
        }
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks
        public override void LoadContent()
        {
            content = ScreenManager.Game.Content;
            graphics = ScreenManager.GraphicsDevice;
            spriteBatch = ScreenManager.SpriteBatch;
            screenHeight = graphics.Viewport.Height;
            screenWidth = graphics.Viewport.Width;
            myTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Screens/instructions");
            scale = ((float)(ScreenManager.GraphicsDevice.Viewport.Width) / (float)myTexture.Width);
            rect = new Rectangle(0, 0, (int)(scale * myTexture.Width), (int)(scale * myTexture.Height));
            font1 = content.Load<SpriteFont>("SpriteFont4");
            font2 = content.Load<SpriteFont>("SpriteFont5");
            font3 = content.Load<SpriteFont>("SpriteFont6");
            button = Tools3.OKButton(ScreenManager.Game.Content,
            new Vector2(screenWidth - 496, screenHeight - 196), screenWidth, screenHeight, user2);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game screen to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio
        /// and detects if the user clicked the button to skip this screen.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah, Bishoy Bassem </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">Determines whether you want this screen to be covered by another screen or not.</param>

        public override void Update(GameTime gameTime)
        {
            if (button.IsClicked())
            {
            ScreenManager.AddScreen(new AdjustPosition(user2, 150, 350, -40, 40, 2));
                Remove();
            }
            button.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game screen should draw itself.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(myTexture, rect, Color.White);
            button.Draw(spriteBatch, scale);
            button.DrawHand(spriteBatch);
            spriteBatch.DrawString(font1, header, Vector2.Zero, Color.DarkViolet, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            UILib.Write(title1, new Rectangle(screenWidth / 10 - 40, screenHeight / 5, screenWidth, screenHeight), spriteBatch, content, font2, Color.DarkRed);
            UILib.Write(text1, new Rectangle(screenWidth / 10 - 22, screenHeight / 4 + 18, screenWidth - 200, screenHeight), spriteBatch, content, font3, Color.Black);
            UILib.Write(title2, new Rectangle(screenWidth / 10 - 40, screenHeight / 2 + 90, screenWidth, screenHeight), spriteBatch, content, font2, Color.DarkRed);
            UILib.Write(text2, new Rectangle(screenWidth / 10 - 22, screenHeight / 2 + 140, screenWidth, screenHeight), spriteBatch, content, font3, Color.Black);
            UILib.Write(green, new Rectangle(screenWidth / 2 + 130, screenHeight / 2 + 155, screenWidth, screenHeight), spriteBatch, content, font3, Color.Green);
            UILib.Write(white, new Rectangle(screenWidth / 2 + 130, screenHeight / 2 + 170, screenWidth, screenHeight), spriteBatch, content, font3, Color.White);
            UILib.Write(red, new Rectangle(screenWidth / 2 + 130, screenHeight / 2 + 185, screenWidth, screenHeight), spriteBatch, content, font3, Color.DarkRed);
            UILib.Write(crossed, new Rectangle(screenWidth / 2 + 130, screenHeight / 2 + 200, screenWidth, screenHeight), spriteBatch, content, font3, Color.DarkGray);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
    }
}
