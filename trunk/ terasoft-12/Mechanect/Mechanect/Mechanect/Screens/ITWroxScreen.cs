using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Screens
{
    class ITworxScreen : FadingScreen
    {

        public ITworxScreen()
            : base("Resources/Images/ITWorx", 1f, 0, 0, -0.06f)
        { }

        public override void Update(GameTime gameTime, bool covered)
        {
            base.Update(gameTime, false);
            if (Done)
            {
                base.Remove();
                ScreenManager.AddScreen(new AllExperiments());
            }
        }
        /// <summary>
        /// Draw the content of this screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
        }
    }
}
