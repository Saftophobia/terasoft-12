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
            : base("Resources/Images/ITWorx", 1f, 0, 0, -70)
        { }

        public override void Update(GameTime gameTime, bool covered)
        {
            base.Update(gameTime, false);
            if (Done)
            {
                base.Remove();
                ScreenManager.AddScreen(new GUCScreen());
            }
        }
    }
}
