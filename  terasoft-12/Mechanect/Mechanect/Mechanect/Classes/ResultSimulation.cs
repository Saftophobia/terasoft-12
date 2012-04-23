using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Mechanect.Common.Cameras;

namespace Mechanect.Classes
{
    /// <summary>
    /// represents the simulation of the result
    /// </summary>
    public class ResultSimulation
    {
        private GraphicsDevice graphics;
        private SpriteBatch spriteBatch;
        private Vector3 shootVelocity;
        private Vector3 optimalVelocity;
        private Vector3 friction;
        private Vector3 initialPosition;
        private CustomModel ball;
        private SpriteFont font;
        private Color fontColor;

        private ModelLinearAnimation animation;
        /// <summary>
        /// the chase camera instance
        /// </summary>
        public ChaseCamera camera { get; private set; }
        private String velocity1;
        private String velocity2;
        private Boolean currentAnimation;

        /// <summary>
        /// constructs a new ResultSimulation instance and intializes it
        /// </summary>
        /// <param name="ball">3d ball model</param>
        /// <param name="shootVelocity">shooting velocity vector</param>
        /// <param name="optimalVelocity">correct velocity vector</param>
        /// <param name="friction">acceleration vector</param>
        /// <param name="fontColor"></param>
        /// <param name="graphics"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="font"></param>
        public ResultSimulation(GraphicsDevice graphics, SpriteBatch spriteBatch, SpriteFont font, Color fontColor, CustomModel ball, Vector3 shootVelocity, Vector3 optimalVelocity, Vector3 friction)
        {
            this.graphics = graphics;
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.fontColor = fontColor;
            this.ball = ball;
            this.shootVelocity = shootVelocity;
            this.optimalVelocity = optimalVelocity;
            this.friction = friction;
            initialPosition = ball.position;
            animation = new ModelLinearAnimation(ball, shootVelocity, friction, TimeSpan.FromSeconds(10), true);
            camera = new ChaseCamera(new Vector3(0, 400, 800), Vector3.Zero, Vector3.Zero, graphics);
            velocity1 = shootVelocity.ToString();
            velocity2 = optimalVelocity.ToString();
        }

        /// <summary>
        /// updates the chase camera and model's position and orientation according to the time elapsed
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (animation.animationStoped)
            {
                if (!currentAnimation)
                {
                    currentAnimation = true;
                    ball.position = initialPosition;
                    animation = new ModelLinearAnimation(ball, optimalVelocity, friction, TimeSpan.FromSeconds(10), true);
                    camera = new ChaseCamera(new Vector3(0, 400, 800), Vector3.Zero, Vector3.Zero, graphics);
                }
                return;

            }
            animation.Update(gameTime.ElapsedGameTime);
            camera.Move(ball.position);
            camera.Rotate(new Vector3(0, 0.005f, 0));
            camera.Update();
        }

        /// <summary>
        /// draws the velocity values and the model according to the view and position of the chase camera
        /// </summary>
        public void Draw()
        {
            graphics.BlendState = BlendState.Opaque;
            graphics.DepthStencilState = DepthStencilState.Default;
            graphics.SamplerStates[0] = SamplerState.LinearWrap;
            ball.Draw(camera);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            spriteBatch.DrawString(font, velocity1, new Vector2(5, 0), (!currentAnimation) ? Color.Red : fontColor, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(font, velocity2, new Vector2(5, font.MeasureString(velocity2).Y * 0.5f), (currentAnimation) ? Color.Red : fontColor, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0.5f);
            spriteBatch.End();
        }

    }
}
