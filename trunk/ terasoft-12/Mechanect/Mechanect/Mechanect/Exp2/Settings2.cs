using ButtonsAndSliders;
using Mechanect.Common;
using Mechanect.Exp3;
using Mechanect.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Mechanect.Exp2
{
    /// <summary>
    /// Settings2 Class is Screen that allows you to change theme and level.
    /// </summary>
    class Settings2 : GameScreen
    {
        private Button oKbutton;
        User2 user;
        private ThemeAndLevel levelAndTheme;
        Texture2D background;

        /// <summary>
        /// Constructor for Settings2 taking User2 as input
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="user">Represents User2</param>

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
            background = ScreenManager.Game.Content.Load<Texture2D>("Textures/Screens/SettingsScreen");
            oKbutton = Tools3.OKButton(ScreenManager.Game.Content,
              new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2.55f,
                          ScreenManager.GraphicsDevice.Viewport.Height / 1.45f),
             ScreenManager.GraphicsDevice.Viewport.Width,
             ScreenManager.GraphicsDevice.Viewport.Height, user);
            levelAndTheme = new ThemeAndLevel(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 17f, ScreenManager.GraphicsDevice.Viewport.Height / 7f), user);
            levelAndTheme.LoadContent(ScreenManager.GraphicsDevice.Viewport.Width,
            ScreenManager.GraphicsDevice.Viewport.Height, ScreenManager.Game.Content);

            base.LoadContent();

        }

        /// <summary>
        /// Update is method that get updated at gameTime and checks for button clicking to exit
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

                if (levelAndTheme.themeNo == 1)
                    Tools2.themeNumber = 1;

                ScreenManager.AddScreen(new InstructionsScreen2(user));
                Remove();
            }
            oKbutton.Update(gameTime);
            levelAndTheme.Update(gameTime);
            base.Update(gameTime);
        }
        /// <summary>
        /// Drawing the OK button and the Theme And level selection bar
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Tamer Nabil </para>
        /// </remarks>
        /// <param name="gameTime">gameTime</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(background, ScreenManager.GraphicsDevice.Viewport.Bounds, Color.White);
            ScreenManager.SpriteBatch.End();
            levelAndTheme.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.Begin();
            oKbutton.Draw(ScreenManager.SpriteBatch, 0.7f);
            ScreenManager.SpriteBatch.End();
            ScreenManager.SpriteBatch.Begin();
            oKbutton.DrawHand(ScreenManager.SpriteBatch);

            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);

        }

    }
}
