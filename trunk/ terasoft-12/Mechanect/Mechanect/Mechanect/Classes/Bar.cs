using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Mechanect.Classes
{
    /// <summary>
    ///  This class represents the Distance Bar Drawable Object
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Ahmed Badr </para>
    /// This class uses Vector2 to do calculations for a 3D environment in order to speed up the calculations
    /// </remarks>
    class Bar
    {
        private SpriteBatch spriteBatch;
        private Texture2D bar;
        public Texture2D Bar
        {
            get
            {
                return bar;
            }
            set
            {
                bar = value;
            }
        }
        private Texture2D ball;
        private Vector2 shootingPos;
        public Vector2 ShootingPos
        {
            get
            {
                return shootingPos;
            }
            set
            {
                shootingPos = value;
            }
        }
        private Vector2 currentPos;
        public Vector2 CurrentPos
        {
            get
            {
                return CurrentPos;
            }
            set
            {
                CurrentPos = value;
            }
        }
        private Vector2 initialPos;
        public Vector2 InitialPos
        {
            get
            {
                return InitialPos;
            }
            set
            {
                InitialPos = value;
            }
        }
        private float offset;
        private Vector2 drawingPosition;

        /// <summary>
        /// Initializes the Bar
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Ahmed Badr </para>
        /// </remarks>
        /// <example>This sample shows how to instantiate the Bar class.
        /// <code>
        ///  protected override void LoadContent()
        ///  {
        ///     Bar bar = new Bar(new Vector2(50, 50), spriteBatch, initialPos, currentPos, shootingPos, Content);
        ///  }
        /// </code>
        /// </example>
        /// <param name="drawingPosition">
        /// Specifies where will the bar be on the screen</param>
        /// <param name="spriteBatch">
        /// The spriteBatch object that will be used to draw the bar</param>
        /// <param name="initialPos">
        /// The initial position of the ball</param>
        /// <param name="currentPos">
        /// The current position of the ball</param>
        /// <param name="shootingPos">
        /// The final position of the ball</param>
        /// <param name="content">
        /// Specifies the content manager responsible for this object</param>
        public Bar(Vector2 drawingPosition, SpriteBatch spriteBatch, Vector2 initialPos, Vector2 currentPos, Vector2 shootingPos, ContentManager content)
        {
            this.drawingPosition = drawingPosition;
            this.initialPos = initialPos;
            this.currentPos = currentPos;
            this.shootingPos = shootingPos;
            this.spriteBatch = spriteBatch;
            bar = content.Load<Texture2D>(@"Resources/Images/Bar");
            ball = content.Load<Texture2D>(@"Resources/Images/Ball");
        }
        /// <summary>
        /// Updates the current position of the ball with the new position
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Ahmed Badr </para>
        /// </remarks>
        /// <param name="currentPos">
        /// Specifies the current position of the ball</param>
        public void Update(Vector2 currentPos)
        {
            this.currentPos = currentPos;
        }
        /// <summary>
        /// Draws the bar.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Ahmed Badr </para>
        /// </remarks>
        /// <example>This sample shows how to use the Draw() method for this class
        /// <code>
        ///  protected override void Draw()
        ///  {
        ///     spriteBatch.Begin();
        ///     bar.Draw();
        ///     spriteBatch.End();
        ///  }
        /// </code>
        /// </example>
        public void Draw()
        {
            offset = Vector2.Distance(initialPos, currentPos) / Vector2.Distance(initialPos, shootingPos);
            spriteBatch.Draw(bar,
            drawingPosition,
            null,
            Color.White,
            0,
            Vector2.Zero,
            1,
            SpriteEffects.None,
            0);
            spriteBatch.Draw(ball,
             Vector2.Add(new Vector2((bar.Width / 2) - (ball.Width / 2), (offset * (bar.Height - ball.Height))), drawingPosition),
             null,
             Color.White,
             0,
             Vector2.Zero,
             1,
             SpriteEffects.None,
             0);
        }

    }
}
