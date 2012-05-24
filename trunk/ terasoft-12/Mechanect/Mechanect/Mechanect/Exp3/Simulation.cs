using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using UI.Animation;
using UI.Cameras;
using Physics;

namespace Mechanect.Exp3
{
    /// <summary>
    /// Represents the simulation of the result.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem.
    /// </remarks>
    public class Simulation
    {
        private GraphicsDevice device;
        private SpriteBatch spriteBatch;
        private Ball ball;
        private Environment3 environment;
        private Vector3 shootPosition;

        private SpriteFont font;
        private Texture2D box;

        private BallAnimation animation1;
        private BallAnimation animation2;
        private bool secondAnimationStarted;

        public ChaseCamera Camera { get; private set; }

        private String velocity1;
        private String velocity2;

        /// <summary>
        /// Creates a new Simulation instance and intializes it.
        /// </summary>
        /// <param name="ball">Ball instance.</param>
        /// <param name="shootPosition">Shoot position.</param>
        /// <param name="holePosition">Hole position.</param>
        /// <param name="shootVelocity">Shoot velocity vector.</param>
        /// <param name="friction">Friction magnitude.</param>
        /// <param name="content">Loads objects from files.</param>
        /// <param name="device">Displays graphics on the screen.</param>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public Simulation(Ball ball, Environment3 environment, Vector3 shootPosition, Vector3 shootVelocity,
            ContentManager content, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            this.device = device;
            this.spriteBatch = spriteBatch;
            this.ball = ball;
            this.environment = environment;
            this.shootPosition = shootPosition;

            ball.Position = shootPosition;

            font = content.Load<SpriteFont>("SpriteFont1");
            box = content.Load<Texture2D>("Textures/screen");

            Vector3 optimalVelocity = LinearMotion.CalculateIntialVelocity(environment.HoleProperty.Position -
                shootPosition, 0, Environment3.Friction);

            animation1 = new BallAnimation(ball, environment, shootVelocity);
            animation2 = new BallAnimation(ball, environment, optimalVelocity);
            Camera = new ChaseCamera(new Vector3(0, 40, 80), Vector3.Zero, Vector3.Zero, device);

            velocity1 = String.Format("<{0,4:0.0}, 0.0,{1,4:0.0}>", shootVelocity.X, shootVelocity.Z);
            velocity2 = String.Format("<{0,4:0.0}, 0.0,{1,4:0.0}>", optimalVelocity.X, optimalVelocity.Z);
        }

        /// <summary>
        /// Updates the chase camera and model positions and orientations according to the time elapsed.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void Update(GameTime gameTime)
        {
            ModelLinearAnimation current = animation1;
            if (animation1.Finished)
            {
                if (!secondAnimationStarted)
                {
                    secondAnimationStarted = true;
                    ball.Position = shootPosition;
                    Camera = new ChaseCamera(new Vector3(0, 40, 80), Vector3.Zero, Vector3.Zero, device);
                }
                current = animation2;
            }
            else
            {
                if (!ball.InsideTerrain(environment.terrainWidth, environment.terrainHeight))
                {
                    animation1.Stop();
                }
            }
            current.Update(gameTime.ElapsedGameTime);
            Camera.Move(ball.Position);
            Camera.Rotate(new Vector3(0, 0.005f, 0));
            Camera.Update();
        }

        /// <summary>
        /// Displays the velocity values.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, 
                SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            spriteBatch.Draw(box, new Rectangle(-20 ,-130, 360, 205), Color.White);

            if (secondAnimationStarted)
            {
                spriteBatch.DrawString(font, "Replay  ", new Vector2(5, 0), Color.White, 0, 
                    Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                if ((int)(animation2.ElapsedTime.TotalSeconds / 0.4) % 2 == 0)
                {
                    spriteBatch.DrawString(font, "Optimal ", new Vector2(5, font.MeasureString(velocity2).Y), 
                        Color.Red, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                }
            }
            else
            {
                if ((int)(animation1.ElapsedTime.TotalSeconds / 0.4) % 2 == 0)
                {
                    spriteBatch.DrawString(font, "Replay  ", new Vector2(5, 0), Color.Red, 0, 
                        Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
                }
                spriteBatch.DrawString(font, "Optimal ", new Vector2(5, font.MeasureString(velocity2).Y),
                    Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
            }

            spriteBatch.DrawString(font, velocity1, new Vector2(font.MeasureString("Optimal ").X + 5, 0),
                (!secondAnimationStarted) ? Color.Red : Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(font, velocity2, new Vector2(font.MeasureString("Optimal ").X + 5,
                font.MeasureString(velocity2).Y), (secondAnimationStarted) ? Color.Red : Color.White, 0, 
                Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

            spriteBatch.End();
        }

    }
}
