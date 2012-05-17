using Mechanect.Common;
using Microsoft.Xna.Framework;
using Mechanect.Classes;
using Mechanect.Experiment2;
using ButtonsAndSliders;
using Mechanect.Exp3;

namespace Mechanect.Screens
{
     class Settings2 : GameScreen
    {
        private Button _oKbutton;
        User2 user;
        private ThemeAndLevel _level;

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
            _oKbutton = Tools3.OKButton(ScreenManager.Game.Content,
              new Vector2(ScreenManager.GraphicsDevice.Viewport.Width +100, ScreenManager.GraphicsDevice.Viewport.Height+50),
             ScreenManager.GraphicsDevice.Viewport.Width,
             ScreenManager.GraphicsDevice.Viewport.Height, user);
            _level = new ThemeAndLevel(ScreenManager.Game, new Vector2(150, 150), ScreenManager.SpriteBatch, user);
            _level.Initialize(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

        }

        /// <summary>
        /// Update method that  updae at gameTime and check for button clicking to exit
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="gameTime">gameTime</param>
        /// <param name="covered">covered</param>
        public override void Update(GameTime gameTime, bool covered)
        {
            if (_oKbutton.IsClicked())
            {
                ExitScreen();
                ScreenManager.AddScreen(new InstructionsScreen2(user));
               // Remove();
            }
            _oKbutton.Update(gameTime);
            _level.Update(gameTime);
            base.Update(gameTime, covered);
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
           
            _level.Draw(gameTime);
            _oKbutton.Draw(ScreenManager.SpriteBatch);
            
            ScreenManager.SpriteBatch.Begin();
            _oKbutton.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
           
        }

    }
}
