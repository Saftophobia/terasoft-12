using System.Threading;
using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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
        private float rotation;
        private float xPositionOffset;
        private float yPositionOffset;
        private bool done;
        public bool Done
        {
            set
            {
                done = value;
            }
            get
            {
                return done;
            }
        }

        public FadingScreen()
        {
        }
        
        public FadingScreen(string path, float logoScale,float rotation,float xPositionOffset, float yPositionOffset)
        {
            this.path = path;
            this.rotation = rotation;
            this.xPositionOffset = xPositionOffset;
            this.yPositionOffset = yPositionOffset;
            first = true;
            fading = 1f;
            scale = logoScale;
            done = false;
            
        }

        public override void LoadContent()
        {
            black = ScreenManager.Game.Content.Load<Texture2D>(@"Resources/Images/black");
            logo = ScreenManager.Game.Content.Load<Texture2D>(@""+path);
        }

        public override void Update(GameTime gameTime, bool covered)
        {
            if (fading <= 0.01f || !first)
            {
                if (first)
                {
                    first = false;
                    Thread.Sleep(1500);
                }

                fading /= 0.96f;
                if (fading > 0.999)
                    Done = true;
            }
            else
                fading *= 0.97f;
        }
 
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(logo, new Vector2(((ScreenManager.GraphicsDevice.Viewport.Width - logo.Width * scale) / 2) + xPositionOffset,
                ((ScreenManager.GraphicsDevice.Viewport.Height - (logo.Height - 100) * scale) / 2) + yPositionOffset),
                null, Color.White, rotation, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 0);

            ScreenManager.SpriteBatch.Draw(black, Vector2.Zero, null, Color.White * fading, 0, new Vector2(0, 0),
                new Vector2(ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height), SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.End();
        }        

    }
}
