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
        /// <summary>
        /// This is the default construtor for TeraSoftScreen class.
        /// </summary>
        public TeraSoftScreen()
            : base("Resources/Images/Terasoft", 0.5f,0,0,-50)
        { }

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
    }
}
