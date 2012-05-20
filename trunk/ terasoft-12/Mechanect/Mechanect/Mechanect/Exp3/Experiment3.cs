using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using UI.Cameras;
using UI.Animation;
using Mechanect.Classes;
using Physics;

namespace Mechanect.Exp3
{
    public class Experiment3 : Mechanect.Common.GameScreen
    {
        private Ball ball;
        private Bar bar;
        private Vector3 intialPosition;
        private Vector3 shootPosition;
        private float arriveVelocity;
        private float friction;

        private Environment3 environment;
        private User3 user;

        private TargetCamera targetCamera;
        private BallAnimation animation;
        private Simulation simulation;

        private bool pauseScreenShowed;
        private bool firstAnimation;

        public Experiment3(User3 user)
        {
            intialPosition = new Vector3(-100, 0, -100);
            shootPosition = new Vector3(0, 3, 62);
            arriveVelocity = 10;
            friction = -2;
            firstAnimation = true;
            this.user = user;
        }

        public override void LoadContent()
        {
            targetCamera = new TargetCamera(new Vector3(0, 140, 250), new Vector3(0,80,0), ScreenManager.GraphicsDevice);

            ball = new Ball(intialPosition, 4, ScreenManager.GraphicsDevice, ScreenManager.Game.Content);

            Vector3 intialVelocity = LinearMotion.CalculateIntialVelocity(shootPosition - ball.Position, arriveVelocity, friction);

            animation = new BallAnimation(ball, intialVelocity, friction, Vector3.Zero, 0);

            environment = new Environment3(intialPosition, intialVelocity,
                ScreenManager.SpriteBatch, ScreenManager.Game.Content, ScreenManager.GraphicsDevice, user, ball);
            bar = new Bar(new Vector2(500,200), ScreenManager.SpriteBatch, new Vector2(ball.Position.X, ball.Position.Z), new Vector2(ball.Position.X, ball.Position.Z), new Vector2(shootPosition.X, shootPosition.Z), ScreenManager.Game.Content);
            environment.arriveVelocity = arriveVelocity;
            
            //environment.LoadContent();

           // environment.GenerateSolvable();
            //Constants3.environment3 = environment;
            //Constants3.game1.Exit();
        }

        public void ShootBall(Vector3 velocity)
        {
            animation = new BallAnimation(ball, velocity, friction, Vector3.Zero, 0);
        }

        public override void Update(GameTime gameTime)
        {
            environment.PlayerModel.Update();
            if (firstAnimation)
            {
                float distance = animation.Displacement.Length();
                float totalDistance = (shootPosition - animation.StartPosition).Length();
                if (distance / totalDistance > 0.5 && !pauseScreenShowed)
                {
                    pauseScreenShowed = true;
                    //add pause screen
                }
                bar.Update(new Vector2(ball.Position.X,ball.Position.Z));
                if (distance / totalDistance > 1)
                {
                    firstAnimation = false;
                    ShootBall(new Vector3(10, 0, -10));
                }
                if (animation.Finished())
                {
                    //add final screen
                }
            }
            else if (animation.Finished() && simulation == null)
            {
                simulation = new Simulation(ball, shootPosition, new Vector3(50, 0, -100), new Vector3(10, 0, -10), friction, ScreenManager.Game.Content, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch);
            }

            if (simulation != null)
            {
                simulation.Update(gameTime);
                if (simulation.Finished())
                {
                    //add final screen
                }
            }

            targetCamera.Update();
            animation.Update(gameTime.ElapsedGameTime);
            //update ball height
            ball.SetHeight(environment.GetHeight(ball.Position));

            if (environment.hasBallEnteredShootRegion())
            {
                Vector3 shootVelocity = 8 * environment.Shoot(gameTime);
                if (user.hasShot)
                {
                    if (shootVelocity.Length() != 0)
                    {
                        this.ShootBall(environment.GetVelocityAfterCollision(shootVelocity));
                        firstAnimation = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Creates the animation of the ball falling into the hole.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Omar Abdulaal.</para>
        /// </remarks>
        private void AnimateBallFalling()
        {
            /*ballFallAnimation = new ModelFramedAnimation(ball);

            if (shootVelocity.X > 0)
                ballFallAnimation.AddFrame(new Vector3(ball.Position.X + 0.5f, -0.5f, ball.Position.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
            else
                ballFallAnimation.AddFrame(new Vector3(ball.Position.X - 0.5f, -0.5f, ball.Position.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
            ballFallAnimation.AddFrame(hole.Position, Vector3.Zero, TimeSpan.FromSeconds(0.75));*/
        }

        /*
        /// <summary>
        /// Calculates Velocity after collision using conservation of momentum laws.
        /// </summary>
        /// <param name="initialVelocity">Initial velocity before collision.</param>
        /// <returns>Vector3 velocity after collision</returns>
        private Vector3 GetVelocityAfterCollision(Vector3 initialVelocity)
        {
            double ballMass, legMass, initialLegVelocity;

            initialLegVelocity = initialVelocity.Length();
            ballMass = ball.Mass;
            legMass = user.AssumedLegMass;

            float finalVelocity = (float) (((legMass * initialLegVelocity) + (ballMass * arriveVelocity) - (0)) / ballMass);
            Vector3 normalizedVector = Vector3.Normalize(initialVelocity);

            return normalizedVector * finalVelocity;
        }
        */

        public override void Draw(GameTime gameTime)
        {
            Camera camera = targetCamera;
            if (simulation != null)
            {
                camera = simulation.Camera;
            }
            //environment.Draw(camera, gameTime);
            ball.Draw(camera);
            if (firstAnimation)
            {
                bar.Draw();
            }
            if (simulation != null)
            {
                simulation.Draw();
            }
            environment.PlayerModel.Draw(gameTime, camera.View, camera.Projection);
        }

        public override void UnloadContent()
        {

        }

    }

}
