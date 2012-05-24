using Mechanect.Common;
using Microsoft.Xna.Framework;

namespace Mechanect.Screens
{
    /// <summary>
    /// This class Represents the ITWorx screen.
    /// </summary>
    /// <remarks><para>AUTHOR: Ahmed Badr.</para></remarks>
    class ITworxScreen : FadingScreen
    {
        /// <summary>
        /// Creates a new instance of ITWorxScreen.
        /// </summary>
        public ITworxScreen()
            : base("Resources/Images/ITWorx", 0.6f, 0, 0, -0.06f)
        {
            
        }
        /// <summary>
        /// Updates this screen
        /// </summary>
        /// <param name="gameTime">Represents the time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Done)
            {
                base.Remove();
                ScreenManager.AddScreen(new AllExperiments(Game1.User));
            }
        }
        /// <summary>
        /// Draws the content of this screen.
        /// </summary>
        /// <param name="gameTime">Represents the time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);
        }
    }
}
