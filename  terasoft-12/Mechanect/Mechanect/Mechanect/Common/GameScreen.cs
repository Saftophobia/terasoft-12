using Microsoft.Xna.Framework;
using Mechanect.Screens;
using System;
namespace Mechanect.Common
{
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
    /// <summary>
    /// This class represents the screen of the game
    /// </summary>
    /// <remarks><para>AUTHOR: Ahmed Badr</para></remarks>
    public abstract class GameScreen
    {
        #region Fields and Properties

        public static int frameNumber;
        public UserAvatar userAvatar;

        private bool isFrozen;
        public bool IsFrozen
        {
            get { return isFrozen; }
            set { isFrozen = value; }
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
            internal set { screenManager = value; }
        }
        
        public bool IsActive
        {
            get
            {
                return screenState == ScreenState.Active;
            }
        }

        #endregion

        #region Initialization

        public virtual void LoadContent() {
            //userAvatar = new UserAvatar(Game1.User, Game1.User.Kinect, ScreenManager.Game.Content, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch);
            userAvatar.LoadContent();
  
        }

        public virtual void Initialize() {

           
        }

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
        /// khaled salah
        public virtual void Update(GameTime gameTime) {
            try
            {
                userAvatar.Update(gameTime);
            }
            catch (Exception)
            {
                throw new ArgumentException("please call base.Draw() , base.Update(), base.LoadContent() in your methods.... ya noob");
            }
        }
        //to be changed to an abstract method when Update(GameTime gametime, bool covered) is removed
        //{
        //    if (IsFrozen)
        //        return;
        //}
        
        /// <summary>
        /// Removes the current screen.
        /// </summary>
        public virtual void Remove()
        {
            screenManager.RemoveScreen(this);
        }
        /// <summary>
        /// Draws the screen.
        /// </summary>
        /// <param name="gameTime">represents the time of the game.</param>
        public virtual void Draw(GameTime gameTime)
        {
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
            //Screen will be drawn but not updated
            screenState = ScreenState.Frozen;
            IsFrozen = true;
        }

        /// <summary>
        /// Unfreezes the screen. The screen will be normally updated.
        /// </summary>
        public void UnfreezeScreen()
        {
            screenState = ScreenState.Active;
            IsFrozen = false;
        }
        #endregion
    }
}
