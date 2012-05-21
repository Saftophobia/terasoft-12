using Mechanect.Common;
using Microsoft.Xna.Framework;
using Mechanect.Exp2;
using ButtonsAndSliders;
using Mechanect.Exp3;



namespace Mechanect.Screens
{
    class Settings2 : GameScreen
    {
        private Button oKbutton;
        User2 user;
        private ThemeAndLevel levelAndTheme;

        public Settings2(User2 user)
        {
            this.user = user;
        }
        /// <summary>
        /// Load the ok button and showing level and theme screen
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        public override void LoadContent()
        {
            oKbutton = Tools3.OKButton(ScreenManager.Game.Content,
              new Vector2(ScreenManager.GraphicsDevice.Viewport.Width + 100, 
                          ScreenManager.GraphicsDevice.Viewport.Height + 50),
             ScreenManager.GraphicsDevice.Viewport.Width,
             ScreenManager.GraphicsDevice.Viewport.Height, user);
            levelAndTheme = new ThemeAndLevel(new Vector2(150, 150), user);
            levelAndTheme.LoadContent(ScreenManager.GraphicsDevice.Viewport.Width,
            ScreenManager.GraphicsDevice.Viewport.Height,ScreenManager.Game.Content);

        }

        /// <summary>
        /// Update method that  updae at gameTime and check for button clicking to exit
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="gameTime">gameTime</param>
        public override void Update(GameTime gameTime)
        {
            if (oKbutton.IsClicked())
            {

                //  ScreenManager.AddScreen(new InstructionsScreen2(user));
                Remove();
            }
            oKbutton.Update(gameTime);
            levelAndTheme.Update(gameTime);
            base.Update(gameTime);
        }
        /// <summary>
        /// drwing the OK button and the Theme And level selection bar
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="gameTime">gameTime</param>
        public override void Draw(GameTime gameTime)
        {

            levelAndTheme.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.Begin();
            oKbutton.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
            ScreenManager.SpriteBatch.Begin();
            oKbutton.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();


        }

    }
}
