using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Mechanect.Exp1;
using ButtonsAndSliders;
using Microsoft.Xna.Framework.Content;

namespace Mechanect.Screens
{
    class InstructionsScreen1 : GameScreen
    {
       
        
        string header = "\n\n\n\n                  .Welcome To Mechanect Racing Game." + '\n'
             + "                           .By TeraSoft Team." + '\n'
               + "                                 .GUC.";
        private string title1 = "\n\n\n\n\n\n\n\n\n      Game Instructions:-";
        private string title2 = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n      General Instructions:-";
        private string text1 = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n          1-Goal: Finishing the race first while applying the given commands." +
            "\n\n          2-Gameplay: You will be notified when you should apply a new command, failing to apply the command" +
            "\n            will lead to your disqualification.\n "
            + "\n          3-Tip: Run as fast as possible when you are allowed to.\n";
        private string text2 = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n          * The avatar on the top right \n            represents your distance from\n            the screen.\n";
        private string green = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n                                                                             Green: Good.";
        private string white = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n                                                                             White: Too Far.";
        private string red = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n                                                                             Red: Too Near.";
        private string crossed = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n                                                                             Crossed: Not Detected.";
        //private Instruction instruction;
        private User1 user1;
        private User1 user2;
        private Texture2D myTexture;
        private Rectangle rect;
        private Button button;
        private float scale;
        SpriteFont font1;
        SpriteFont font2;
        SpriteFont font3;

        public InstructionsScreen1(User1 user1, User1 user2)
        {
            this.user1 = user1;
            this.user2 = user2;
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


            
            isTwoPlayers = true;
            myTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/Screens/instructions");
            scale = ((float)(ScreenManager.GraphicsDevice.Viewport.Width) / (float)myTexture.Width);
            rect = new Rectangle(0, 0, (int)(scale * myTexture.Width), (int)(scale * myTexture.Height));
            font1 = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont4");
            font2 = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont5");
            font3 = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont6");
            button = Mechanect.Exp3.Tools3.OKButton(ScreenManager.Game.Content,
            new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 496, ScreenManager.GraphicsDevice.Viewport.Height - 196), ScreenManager.GraphicsDevice.Viewport.Width,
            ScreenManager.GraphicsDevice.Viewport.Height, user1);
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

                //Un-comment the line below when the screen is finally committed
                
                
                ScreenManager.AddScreen(new Mechanect.Screens.Exp1Screens.Exp1(user1,user2));
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
            float sw = ScreenManager.GraphicsDevice.Viewport.Width;
            float sh = ScreenManager.GraphicsDevice.Viewport.Height;
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(myTexture, rect, Color.White);
            button.Draw(ScreenManager.SpriteBatch, scale);
            button.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.DrawString(font1, header, Vector2.Zero, Color.DarkViolet, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font2, title1, Vector2.Zero, Color.DarkRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font2, title2, Vector2.Zero, Color.DarkRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, text1, Vector2.Zero, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, text2, Vector2.Zero, Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, green, Vector2.Zero, Color.DarkGreen, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, white, Vector2.Zero, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, red, Vector2.Zero, Color.DarkRed, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.DrawString(font3, crossed, Vector2.Zero, Color.DarkGray, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
        /// <summary>
        /// This is called when you want to exit the screen.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>  
        public override void Remove()
        {
            base.Remove();
        }

    }
}
