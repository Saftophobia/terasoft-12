using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Screens;
using System.Collections;
using Mechanect.Exp3;
using System.Threading;

namespace Mechanect.Common
{
   /// <summary>
   /// This class represents a screen manager.
   /// </summary>
   /// <remarks><para>AUTHOR: Ahmed Badr</para></remarks>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        Dictionary<string,GameScreen> screens = new Dictionary<string,GameScreen>();
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
            User user = new User();
            User3 user3 = new User3();
            AddScreen("terasoftscreen", new TeraSoftScreen());
            AddScreen("itworxscreen", new ITworxScreen());
            AddScreen("allexperiments", new AllExperiments(user));
            AddScreen("experiment3", new Experiment3(user3));
            AddScreen("pausescreen", new PauseScreen(user3, user3.Kinect, 7, 7, 0.01));
            //add your screens here
        }

        /// <summary>
        /// Unload screen dedicated content
        /// </summary>
        protected override void UnloadContent()
        {
            foreach(string key in screens.Keys) {
                screens[key].UnloadContent();
            }
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
            foreach (string key in screens.Keys)
            {
                if (screens[key].IsActive)
                    screensToUpdate.Add(screens[key]);   
            }

            if (screensToUpdate.Count == 0)
            foreach (string key in screens.Keys)
            {
                screens[key].UnfreezeScreen();
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
            foreach (string key in screens.Keys)
            {
                if (screens[key].ScreenState == ScreenState.Hidden || !screens[key].IsActive)
                    continue;

                screens[key].Draw(gameTime);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a screen to the list of screens that are managed by the screenManager.
        /// </summary>
        /// <param name="screen">Represents the screen that should be managed by the screenManager</param>
        public void AddScreen(string screenName, GameScreen screen)
        {
            screen.ScreenManager = this;
                screen.LoadContent();
                screen.Initialize();
                screen.IsActive = false;
            screens.Add(screenName, screen);
            Thread.Sleep(200);
        }

        public void LoadScreen(string screenName){
            screens[screenName].IsActive = true;
        }

        /// <summary>
        /// Removes a screen from the list of screens that are managed by the screenManager.
        /// </summary>
        /// <param name="screen">Represents the screen that should be removed from the list
        /// of managed screens by the screenManager</param>
        public void RemoveScreen(string screenName)
        {
            GameScreen screen = screens[screenName];
            if (this.isInitialized)
            {
                screen.UnloadContent();
            }
            screens.Remove(screenName);
            screensToUpdate.Remove(screen);
        }
        #endregion
    }
}
