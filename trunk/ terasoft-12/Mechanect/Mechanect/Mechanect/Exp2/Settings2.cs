using ButtonsAndSliders;
using Mechanect.Common;
using Mechanect.Exp3;
using Microsoft.Xna.Framework;



namespace Mechanect.Exp2
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
              new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/1.5f, 
                          ScreenManager.GraphicsDevice.Viewport.Height/1.5f),
             ScreenManager.GraphicsDevice.Viewport.Width,
             ScreenManager.GraphicsDevice.Viewport.Height, user);
            levelAndTheme = new ThemeAndLevel(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/30f, ScreenManager.GraphicsDevice.Viewport.Height/25f), user);
            levelAndTheme.LoadContent(ScreenManager.GraphicsDevice.Viewport.Width,
            ScreenManager.GraphicsDevice.Viewport.Height,ScreenManager.Game.Content);

            base.LoadContent();

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
                if (levelAndTheme.levelNo == 1)
                    Tools2.tolerance = 30;
                else if (levelAndTheme.levelNo == 2)
                    Tools2.tolerance = 20;
                else if (levelAndTheme.levelNo == 3)
                    Tools2.tolerance = 10;
                
                ScreenManager.AddScreen(new InstructionsScreen2(user));
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
            oKbutton.Draw(ScreenManager.SpriteBatch,0.6f);
            ScreenManager.SpriteBatch.End();
            ScreenManager.SpriteBatch.Begin();
            oKbutton.DrawHand(ScreenManager.SpriteBatch);
            
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
            
        }

    }
}
