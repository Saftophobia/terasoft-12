using System;
using Mechanect.Screens;
using Microsoft.Xna.Framework;
namespace Mechanect.Common
{
    #region ScreenState
    /// <summary>
    /// Represents the screen states.
    /// </summary>
    /// <remarks><para>AUTHOR: Ahmed Badr</para></remarks>
    public enum ScreenState
    {
        Active,
        Frozen,
        Hidden
    }
    #endregion

    /// <summary>
    /// This class represents a screen.
    /// </summary>
    /// <remarks><para>AUTHOR: Ahmed Badr.</para></remarks>
    public abstract class GameScreen
    {
        #region Fields and Properties

        public static int frameNumber;
        public UserAvatar userAvatar;
        public bool IsFrozen
        {
            get { 
                return screenState == ScreenState.Frozen;
            }
        }

        private ScreenState screenState;
        public ScreenState ScreenState
        {
            get { return screenState; }
            set { screenState = value; }
        }
        private ScreenManager screenManager;
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            set { screenManager = value; }
        }
        
        public bool IsActive
        {
            get
            {
                return screenState == ScreenState.Active;
            }
        }
        public bool showAvatar=true;
        public bool isTwoPlayers = false;
        #endregion

        #region Initialization
        /// <summary>
        /// 5o5a
        /// </summary>
        public virtual void LoadContent() {
            if (showAvatar)
            {
                if(isTwoPlayers)
                    userAvatar = new UserAvatar(Game1.User, Game1.User2,ScreenManager.Game.Content, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch);
                else
                userAvatar = new UserAvatar(Game1.User, ScreenManager.Game.Content, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch);
                userAvatar.LoadContent();
            }
  
        }

        /// <summary>
        /// Initializes the GameScreen.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmed Badr.</para></remarks>
        public virtual void Initialize() {
        }
        /// <summary>
        /// Unloads the content of GameScreen.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmed Badr.</para></remarks>
        public virtual void UnloadContent() { }
        #endregion

        #region Update and Draw

        [System.Obsolete("will be replaced by Update(gameTime)", false)]
        public virtual void Update(GameTime gameTime, bool covered)
        {
       
                    
             
            if (IsFrozen)
                return;
        }
        /// <summary>
        /// Updates the screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        /// <remarks><para>AUTHOR: Ahmed Badr.</para></remarks>
        /// 5o5a
        public virtual void Update(GameTime gameTime) {
            if (showAvatar)
            {
                    userAvatar.Update(gameTime);  
            }
        }
        
        /// <summary>
        /// Removes the current screen.
        /// </summary>
        /// <remarks><para>AUTHOR: Ahmed Badr.</para></remarks>
        public virtual void Remove()
        {
            screenManager.RemoveScreen(this);
        }
        /// <summary>
        /// Draws the screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        /// 5o5a
        public virtual void Draw(GameTime gameTime)
        {
            if(showAvatar)
            userAvatar.Draw(gameTime);
        }
        #endregion

        #region Methods
        [System.Obsolete("use method Remove() instead", false)]
        public virtual void ExitScreen()
        {
                this.Remove();
        }

        /// <summary>
        /// Freezes the screen. The screen will not be updated.
        /// </summary>
        public void FreezeScreen()
        {
            screenState = ScreenState.Frozen;
        }

        /// <summary>
        /// Unfreezes the screen. The screen will be normally updated.
        /// </summary>
        public void UnfreezeScreen()
        {
            screenState = ScreenState.Active;
        }
        #endregion
    }
}
