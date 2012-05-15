using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Screens
{
    class GUCScreen : FadingScreen
    {
        public GUCScreen()
            : base("Resources/Images/GUC", 1f, 0, 0, -70)
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
    }
}
