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

namespace Mechanect.Experiment3
{
    public class Experiment3 : Mechanect.Common.GameScreen
    {
        private Environment3 environment;
        private Ball ball;
        private Hole hole;
        private Vector3 intialPosition;
        private Vector3 shootPosition, shootVelocity;
        private float arriveVelocity;
        private float friction;

        private TargetCamera targetCamera;
        private ModelLinearAnimation animation;
        private ModelFramedAnimation ballFallAnimation;
        private Simulation simulation;

        private bool pauseScreenShowed;
        private bool ballStoot;
        private bool simulationStarted, ballFallingIntoHole;


        public Experiment3()
        {
            intialPosition = new Vector3(-100, 0, -100);
            shootPosition = new Vector3(0, 0, 0);
            arriveVelocity = 15;
            friction = -2;
        }

        public override void LoadContent()
        {
            targetCamera = new TargetCamera(new Vector3(0, 60, 60), Vector3.Zero, ScreenManager.GraphicsDevice);
            ball = new Ball(intialPosition, 10, ScreenManager.GraphicsDevice, ScreenManager.Game.Content);
            animation = new ModelLinearAnimation(ball, shootPosition, arriveVelocity, friction, true);
        }

        public void ShootBall(Vector3 velocity)
        {
            ball.Rotation = Vector3.Zero;
            animation = new ModelLinearAnimation(ball, velocity, friction, TimeSpan.FromSeconds(10), true);
        }

        public override void Update(GameTime gameTime, bool covered)
        {
            if (!pauseScreenShowed)
            {
                float distance = (ball.Position - intialPosition).Length();
                float totalDistance = (shootPosition - intialPosition).Length();
                if (distance / totalDistance > 0.5 && !pauseScreenShowed)
                {
                    pauseScreenShowed = true;
                    //add pause screen
                }
            }
            else if (animation.AnimationStoped && !ballStoot)
            {
                ballStoot = true;
                ShootBall(new Vector3(10, 0, -10));
            }
            else if (animation.AnimationStoped && !simulationStarted)
            {
                simulationStarted = true;
                simulation = new Simulation(ball, shootPosition, new Vector3(50, 0, -100), new Vector3(10, 0, -10), friction, ScreenManager.Game.Content, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch);
            }
            else if (simulationStarted)
            {
                simulation.Update(gameTime);
                if (simulation.SimulationFinished)
                {
                    //add final screen
                }
            }
            if (!ballStoot)
            {
                //update distance bar
            }
            /*if (!ballFallingIntoHole && animation.AnimationStoped && (Vector3.Distance(new Vector3(ball.Position.X, 0, ball.Position.Z), hole.Position) < (ball.Radius + hole.Radius)))
            {
                ballFallingIntoHole = true;
                AnimateBallFalling();
            }
            targetCamera.Update();
            animation.Update(gameTime.ElapsedGameTime);

            if (ballFallAnimation != null)
                ballFallAnimation.Update(gameTime.ElapsedGameTime);

            //update ball height
            ball.setHeight(environment.GetHeight(ball.Position));*/

            base.Update(gameTime, covered);
        }

        /// <summary>
        /// Creates the animation of the ball falling into the hole.
        /// </summar
        /// <remarks>
        /// <para>AUTHOR: Omar Abdulaal.</para>
        /// </remarks>
        private void AnimateBallFalling()
        {
            ballFallAnimation = new ModelFramedAnimation(ball);

            if (shootVelocity.X > 0)
                ballFallAnimation.AddFrame(new Vector3(ball.Position.X + 0.5f, -0.5f, ball.Position.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
            else
                ballFallAnimation.AddFrame(new Vector3(ball.Position.X - 0.5f, -0.5f, ball.Position.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
            ballFallAnimation.AddFrame(hole.Position, Vector3.Zero, TimeSpan.FromSeconds(0.75));
        }

        public override void Draw(GameTime gameTime)
        {
            Camera camera = targetCamera;
            if (simulationStarted)
            {
                camera = simulation.Camera;
            }
            // draw E]environment
            ball.Draw(camera);
            if (!ballStoot)
            {
                //draw distance bar
            }
            if (simulationStarted)
            {
                simulation.Draw();
            }
        }

        public override void UnloadContent()
        {

        }

    }

}
