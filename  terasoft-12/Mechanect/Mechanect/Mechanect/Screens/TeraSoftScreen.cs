using System.Threading;
using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mechanect.Screens
{
    /// <summary>
    /// This is a screen containg the GUC logo.
    /// </summary>
    class TeraSoftScreen : FadingScreen
    {
        private float scale;
        private Texture2D gucLogo;
        /// <summary>
        /// This is the default construtor for TeraSoftScreen class.
        /// </summary>
        public TeraSoftScreen()
            : base("Resources/Images/Terasoft", 0.5f,0,0,-0.06f)
        {
            scale = 0.3f;
        }

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
        public override void Update(GameTime gameTime, bool covered)
        {
            base.Update(gameTime,false);
            if (Done)
            {
                base.Remove();
                ScreenManager.AddScreen(new ITworxScreen());
            }
        }

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
