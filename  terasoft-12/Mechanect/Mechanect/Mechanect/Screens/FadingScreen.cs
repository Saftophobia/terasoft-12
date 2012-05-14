using System.Threading;
using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Screens
{
    class FadingScreen : GameScreen
    {
        private bool first;
        private float fading;
        private Texture2D black;
        private Texture2D logo;
        private string path;
        private float scale;
        public FadingScreen(string path, float logoScale)
        {
            this.path = path;
            first = true;
            fading = 1f;
            scale = logoScale;
        }

        public override void LoadContent()
        {
            black = ScreenManager.Game.Content.Load<Texture2D>(@"Resources/Images/black");
            logo = ScreenManager.Game.Content.Load<Texture2D>(@""+path);
        }

        public override void Update(GameTime gameTime, bool covered)
        {
            if (fading <= 0.1f || !first)
            {
                if (first)
                {
                    first = false;
                    Thread.Sleep(2000);
                }

                fading /= 0.96f;
            }
            else
                fading *= 0.99f;
        }
 
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.Black);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(logo, new Vector2((ScreenManager.GraphicsDevice.Viewport.Width - logo.Width * scale) / 2,
                (ScreenManager.GraphicsDevice.Viewport.Height - (logo.Height - 100) * scale) / 2),
                null, Color.White, -0.1f, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 0);

            ScreenManager.SpriteBatch.Draw(black, Vector2.Zero, null, Color.White * fading, -0.1f, new Vector2(0, 0),
                new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.End();
        }        

    }
}
