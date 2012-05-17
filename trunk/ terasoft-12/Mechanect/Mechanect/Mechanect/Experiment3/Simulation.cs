using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using UI.Animation;
using UI.Cameras;

namespace Mechanect.Experiment3
{
    /// <summary>
    /// represents the simulation of the result
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class Simulation
    {
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;
        private Ball ball;
        private Vector3 shootPosition;
        private Vector3 optimalVelocity;

        private SpriteFont font;
        private Color fontColor;

        private ModelLinearAnimation animation1;
        private ModelLinearAnimation animation2;
        private bool secondAnimationStarted;
        /// <summary>
        /// the chase camera instance
        /// </summary>
        public ChaseCamera Camera { get; private set; }
        public Boolean SimulationFinished
        {
            get
            {
                return animation2.AnimationStoped;
            }

        }
        
        private String velocity1;
        private String velocity2;

        /// <summary>
        /// constructs a new Simulation instance and intializes it
        /// </summary>
        /// <param name="ball">ball instance</param>
        /// <param name="shootPosition">shoot position vector</param>
        /// <param name="holePosition">hole position vector</param>
        /// <param name="shootVelocity">shoot velocity vector</param>
        /// <param name="friction">friction magnitude</param>
        /// <param name="content">content manager</param>
        /// <param name="device">draphics device</param>
        /// <param name="spriteBatch">sprite batch</param>
        public Simulation(Ball ball, Vector3 shootPosition, Vector3 holePosition, Vector3 shootVelocity, float friction, ContentManager content, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            this.device = device;
            this.spriteBatch = spriteBatch;
            this.ball = ball;
            this.shootPosition = shootPosition;
            ball.Rotation = Vector3.Zero;
            ball.Position = shootPosition;

            font = content.Load<SpriteFont>("SpriteFont1");
            fontColor = Color.Black;

            optimalVelocity = Physics.Functions.CalculateIntialVelocity(holePosition - shootPosition, 0, friction);

            animation1 = new ModelLinearAnimation(ball, shootVelocity, friction, TimeSpan.FromSeconds(10), true);
            animation2 = new ModelLinearAnimation(ball, optimalVelocity, friction, TimeSpan.FromSeconds(10), true);
            Camera = new ChaseCamera(new Vector3(0, 40, 80), Vector3.Zero, Vector3.Zero, device);

            velocity1 = shootVelocity.ToString();
            velocity2 = optimalVelocity.ToString();
        }

        /// <summary>
        /// updates the chase camera and model's position and orientation according to the time elapsed
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            ModelLinearAnimation current = animation1;
            if (animation1.AnimationStoped)
            {
                if (!secondAnimationStarted)
                {
                    secondAnimationStarted = true;
                    ball.Rotation = Vector3.Zero;
                    ball.Position = shootPosition;
                    Camera = new ChaseCamera(new Vector3(0, 40, 80), Vector3.Zero, Vector3.Zero, device);
                }
                current = animation2;
            }
            current.Update(gameTime.ElapsedGameTime);
            Camera.Move(ball.Position);
            Camera.Rotate(new Vector3(0, 0.005f, 0));
            Camera.Update();
        }

        /// <summary>
        /// draws the velocity values
        /// </summary>
        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            spriteBatch.DrawString(font, velocity1, new Vector2(5, 0), (!secondAnimationStarted) ? Color.Red : fontColor, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(font, velocity2, new Vector2(5, font.MeasureString(velocity2).Y * 0.5f), (secondAnimationStarted) ? Color.Red : fontColor, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
            spriteBatch.End();
        }

    }
}
