using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Common
{
   /// <summary>
   /// This class represents a screen manager.
   /// </summary>
   /// <remarks><para>AUTHOR: Ahmed Badr</para></remarks>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        private List<GameScreen> screens = new List<GameScreen>();
        private List<GameScreen> screensToUpdate = new List<GameScreen>();
        private SpriteBatch spriteBatch;
        #endregion

        #region Properties
        /// <summary>
        /// Return the sprite batch object.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        #endregion

        #region Initialization
        /// <summary>
        /// Constructor, initializes the manager
        /// </summary>
        public ScreenManager(Game game)
            : base(game)
        {
            base.Initialize();
            
        }

        /// <summary>
        /// Initialize the spriteBatch and screen dedicated content.
        /// </summary>
        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (GameScreen screen in screens)
                screen.LoadContent();
        }

        /// <summary>
        /// Unload screen dedicated content
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (GameScreen screen in screens)
                screen.UnloadContent();
        }
        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates all the screens that should be drawn in a first in first updated manner.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            //clear out the screensToUpdate list to copy the screens list
            //this allows us to add or remove screens without complaining.
            screensToUpdate.Clear();
          
            
            foreach (GameScreen screen in screens)
            {
                if(!screen.IsFrozen)
                screensToUpdate.Add(screen);
            }

            if (screensToUpdate.Count == 0)
                foreach (GameScreen screen in screens)
                {
                    screen.UnfreezeScreen();
                    screensToUpdate.Add(screen);
                }
            else
            {
                while (screensToUpdate.Count > 0)
                {
                    GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                    screensToUpdate.RemoveAt(screensToUpdate.Count - 1);
                    screen.Update(gameTime, false);
                    screen.Update(gameTime);

                }
            }
        }
        /// <summary>
        /// Draws all the screens that should be drawn in a first in first drawn manner.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a screen to the list of screens that are managed by the screenManager.
        /// </summary>
        /// <param name="screen">Represents the screen that should be managed by the screenManager</param>
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            screen.Initialize();
            screen.LoadContent();
            screens.Add(screen);
        }

        /// <summary>
        /// Removes a screen from the list of screens that are managed by the screenManager.
        /// </summary>
        /// <param name="screen">Represents the screen that should be removed from the list
        /// of managed screens by the screenManager</param>
        public void RemoveScreen(GameScreen screen)
        {
            screen.UnloadContent();
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }
        #endregion
    }
}
