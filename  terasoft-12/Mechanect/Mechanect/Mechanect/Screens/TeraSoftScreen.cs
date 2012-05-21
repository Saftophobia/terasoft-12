using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Screens
{
    /// <summary>
    /// This class represents the Terasoft screen.
    /// </summary>
    class TeraSoftScreen : FadingScreen
    {
        private float scale;
        private Texture2D gucLogo;
        /// <summary>
        /// Creates a new instance of the TeraSoftScreen.
        /// </summary>
        public TeraSoftScreen()
            : base("Resources/Images/Terasoft", 0.5f,0,0,-0.06f)
        {
            scale = 0.3f;
        }

        /// <summary>
        /// Loads the content of this screen.
        /// </summary>
        public override void LoadContent()
        {
            gucLogo = ScreenManager.Game.Content.Load<Texture2D>(@"Resources/Images/GUC");
            base.LoadContent();
        }
        /// <summary>
        /// Updates the content of this screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        /// <param name="covered">specifies wether the screen is covered.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Done)
            {
                base.Remove();
                ScreenManager.AddScreen(new ITworxScreen());
            }
        }

        /// <summary>
        /// Draws the content of this screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(gucLogo, new Vector2(((ScreenManager.GraphicsDevice.Viewport.Width - gucLogo.Width * scale) / 1.1f),
                ((ScreenManager.GraphicsDevice.Viewport.Height - (gucLogo.Height) * scale) / 1.1f)),
                null, Color.White, 0, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
          
        }
    }
}
