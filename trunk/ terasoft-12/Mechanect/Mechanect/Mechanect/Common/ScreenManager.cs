using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mechanect.Screens;

namespace Mechanect.Common
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        List<GameScreen> screens = new List<GameScreen>();
        public List<GameScreen> screensToUpdate = new List<GameScreen>();

        SpriteBatch spriteBatch;

        bool isInitialized;
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

            isInitialized = true;
        }

        /// <summary>
        /// Initialize the spriteBatch and screen dedicated content.
        /// </summary>
        protected override void LoadContent()
        {
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load screen dedicated content
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
        /// Update manager and screens
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

            if (!Game.IsActive)
            {
                //Pause
            }
            else
            {
                while (screensToUpdate.Count > 0)
                {
                    GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                    screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                    //Update the screen
                    screen.Update(gameTime, false);

                }
            }
        }

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
        public void AddScreen(GameScreen screen)
        {
            screen.ScreenManager = this;
            if (this.isInitialized)
            {
                screen.LoadContent();
                screen.Initialize();
            }
            screens.Add(screen);
        }

        public void RemoveScreen(GameScreen screen)
        {
            if (this.isInitialized)
            {
                screen.UnloadContent();
            }
            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }
        #endregion
    }
}
