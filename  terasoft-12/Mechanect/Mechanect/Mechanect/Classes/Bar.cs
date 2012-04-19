using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Mechanect.Classes
{
    class Bar
    {
        private SpriteBatch spriteBatch;
        private Texture2D bar;
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
        /// <example>This sample shows how to call the <see cref="GetZero"/> method.
        /// <code>
        ///  protected override void LoadContent()
        ///  {
        ///     Bar bar = new Bar(new Vector2(50, 50), spriteBatch, initialPos, currentPos, shootingPos, Content);
        ///  }
        /// </code>
        /// </example>
        /// <param name="drawingPosition"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="initialPos"></param>
        /// <param name="currentPos"></param>
        /// <param name="shootingPos"></param>
        /// <param name="content"></param>
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
        /// Draws the bar.
        /// </summary>
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
