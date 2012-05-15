using System.Threading;
using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mechanect.Screens
{
    class TeraSoftScreen : FadingScreen
    {
        public TeraSoftScreen()
            : base("Resources/Images/Terasoft", 0.5f,0,0,-50)
        { }

        public override void Update(GameTime gameTime, bool covered)
        {
            base.Update(gameTime,false);
            if (Done)
            {
                base.Remove();
                ScreenManager.AddScreen(new ITworxScreen());
            }
        }
    }
}
