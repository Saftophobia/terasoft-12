using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Common
{
    /// <summary>
    /// What is our screen doing currently?
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
        Frozen,
        Inactive,
    }
    public abstract class GameScreen
    {
        #region Fields and Properties

        /// <summary>
        /// Is the screen a popup (ie a message dialog)
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            set { isPopup = value; }
        }
        private bool isPopup = false;

        /// <summary>
        /// The amount of time it takes for the screen to transition on
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }
        TimeSpan transitionOnTime = TimeSpan.Zero;

        /// <summary>
        /// The amount of time it takes for the screen to transition off
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }
        TimeSpan transitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// How far are we along the transition?
        /// </summary>
        public float TransitionPercent
        {
            get { return transitionPercent; }
        }
        float transitionPercent = 0.00f;

        /// <summary>
        /// Controls how fast the screen transitions
        /// </summary>
        public float TransitionSpeed
        {
            get { return transitionSpeed; }
        }
        float transitionSpeed = 1.5f;

        /// <summary>
        /// Controls the direction the screen transitions
        /// </summary>
        public int TransitionDirection
        {
            get { return transitionDirection; }
        }
        int transitionDirection = 1;

        /// <summary>
        /// Holds the alpha value of the screen
        /// </summary>
        public byte ScreenAlpha
        {
            get { return (byte)(transitionPercent * 255); }
        }

        /// <summary>
        /// What is the screen doing currently?
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            set { screenState = value; }
        }
        ScreenState screenState = ScreenState.TransitionOn;

        /// <summary>
        /// The screen manager that controls the screen.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        ScreenManager screenManager;

        /// <summary>
        /// Is the screen currently exiting?
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected set
            {
                isExiting = value;
                if (isExiting && (Exiting != null))
                {
                    Exiting(this, EventArgs.Empty);
                }
            }
        }
        bool isExiting = false;

        public bool IsActive
        {
            get
            {
                return (screenState == ScreenState.TransitionOn
                    || screenState == ScreenState.Active);
            }
        }

        /// <summary>
        /// Event Handlers for the screen entering and exiting
        /// </summary>
        public event EventHandler Entering;
        public event EventHandler Exiting;

        /// <summary>
        /// Is the screen currently being covered by another?
        /// </summary>
        #endregion

        #region Initialization
        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }
        #endregion

        #region Update and Draw
        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime, bool covered)
        {
            if (IsExiting)
            {
                screenState = ScreenState.TransitionOff;
                if (!ScreenTransition(gameTime, transitionOffTime, -1))
                {
                    this.Remove();
                }
            }
            else if (covered)
            {
                if (ScreenTransition(gameTime, transitionOffTime, 1))
                {
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    screenState = ScreenState.Hidden;
                }
            }
            else if(screenState != ScreenState.Active)
            {
                if (ScreenTransition(gameTime, transitionOffTime, 1))
                {
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    screenState = ScreenState.Active;
                }
            }
        }

        public virtual void Remove()
        {
            screenManager.RemoveScreen(this);
        }

        private bool ScreenTransition(GameTime gameTime, TimeSpan transitionTime, int direction)
        {
            float transitionDelta;

            if (transitionTime == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / transitionTime.TotalMilliseconds);

            transitionPercent += transitionDelta * direction * transitionSpeed;

            if ((transitionPercent <= 0) || (transitionPercent >= 1))
            {
                transitionPercent = MathHelper.Clamp(transitionPercent, 0, 1);
                return false;
            }
            return true;
        }
        public virtual void HandleInput()
        {
            if (screenState != ScreenState.Active)
                return;
        }
        public abstract void Draw(GameTime gameTime);
        #endregion

        #region Methods
        public virtual void ExitScreen()
        {
            IsExiting = true;
            if (transitionOffTime == TimeSpan.Zero)
                this.Remove();
        }
        public void FreezeScreen()
        {
            //Screen will be drawn but not updated
            screenState = ScreenState.Frozen;
        }
        #endregion
    }
}
