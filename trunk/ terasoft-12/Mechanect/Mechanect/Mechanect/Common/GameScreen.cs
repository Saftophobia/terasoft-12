using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
using Mechanect.Screens;
namespace Mechanect.Common
{
    /// <summary>
    /// What is our screen doing currently?
    /// </summary>
    public enum ScreenState
    {
        Active,
        Frozen,
        Hidden
    }
    public abstract class GameScreen
    {
        #region Fields and Properties

        public static int frameNumber { get; set; }

        /// <summary>
        /// Is the screen a popup (ie a message dialog)
        /// </summary>

        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }
        User user;
        bool pausescreenappeared = false;
        public bool IsFrozen
        {
            get { return isFrozen; }
            set { isFrozen = value; }
        }
        private bool isFrozen = false;
        private bool isPopup = false;


        /// <summary>
        /// What is the screen doing currently?
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            set { screenState = value; }
        }
        ScreenState screenState;

        /// <summary>
        /// The screen manager that controls the screen.
        /// </summary>
        /// <summary>
        /// Updates the Screen
        /// </summary>
        /// <example>This sample shows how use Update method in a class extending GameScreen if you want to have your screen
        /// not covered by another screen
        /// <code>
        /// public override void Update(GameTime gameTime, bool covered)
        /// {
        ///     base.Update(gameTime, false);
        /// }
        /// </code>
        /// </example>
        /// <param name="gameTime"></param>
        /// <param name="covered"></param>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        ScreenManager screenManager;

        
        public bool IsActive
        {
            get
            {
                return screenState == ScreenState.Active;
            }
        }

        /// <summary>
        /// Is the screen currently being covered by another?
        /// </summary>
        #endregion

        #region Initialization
        public virtual void LoadContent() {
           user = new User();
        }

        public virtual void UnloadContent() { }
        #endregion

        #region Update and Draw
        public virtual void Initialize() {
            user = new User();
        }

        [System.Obsolete("will be replaced by Update(gameTime)", false)]
        public virtual void Update(GameTime gameTime, bool covered)
        {
         /*   user.setSkeleton();
            if (user.USER == null)
            {
                if (pausescreenappeared == false)
                {
                    FreezeScreen();
                    screenManager.AddScreen(new CommonPauseScreen(user));
                    pausescreenappeared = true;
                }
            }
            else if (pausescreenappeared == true)
            {
                Remove();
                pausescreenappeared = false;
                UnfreezeScreen();
            }*/
             
            if (IsFrozen)
                return;
        }

        public virtual void Update(GameTime gameTime)
        {
            //if (IsFrozen)
            //    return;
        }

        //change protection level to private
        [System.Obsolete("use method ExitScreen() instead", false)]
        public virtual void Remove()
        {
            screenManager.RemoveScreen(this);
        }

        public abstract void Draw(GameTime gameTime);
        #endregion

        #region Methods
        public virtual void ExitScreen()
        {
                this.Remove();
        }



        /// <summary>
        /// Should be called when the screen should be frozen, note that this method does not automatically freeze the screen.
        /// It only changes the ScreenState to Frozen, which should be checked as a condition later.
        /// </summary>
        public void FreezeScreen()
        {
            //Screen will be drawn but not updated
            screenState = ScreenState.Frozen;
            ScreenManager.screensToUpdate.Remove(this);
            IsFrozen = true;
        }

        public void UnfreezeScreen()
        {
            //Screen will be drawn but not updated
            screenState = ScreenState.Active;
            IsFrozen = false;
            screenManager.screensToUpdate.Add(this);
        }
        #endregion
    }
}
