using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using UI.Cameras;
using UI.Animation;

namespace Mechanect.Exp3
{
    public class Experiment3 : Mechanect.Common.GameScreen
    {
        private Ball ball;
        private Vector3 intialPosition;
        private Vector3 shootPosition;
        private float arriveVelocity;
        private float friction;

        private TargetCamera targetCamera;
        private BallAnimation animation;
        private Simulation simulation;

        private bool pauseScreenShowed;
        private bool firstAnimation;

        public Experiment3()
        {
            intialPosition = new Vector3(-100, 0, -100);
            shootPosition = new Vector3(0, 0, 0);
            arriveVelocity = 10;
            friction = -2;
            firstAnimation = true;
        }

        public override void LoadContent()
        {
            targetCamera = new TargetCamera(new Vector3(0, 60, 60), Vector3.Zero, ScreenManager.GraphicsDevice);
            ball = new Ball(intialPosition, 10, ScreenManager.GraphicsDevice, ScreenManager.Game.Content);
            animation = new BallAnimation(ball, Physics.Functions.CalculateIntialVelocity(shootPosition - intialPosition, arriveVelocity, friction), friction, Vector3.Zero);
        }

        public void ShootBall(Vector3 velocity)
        {
            animation = new BallAnimation(ball, velocity, friction, Vector3.Zero);
        }

        public override void Update(GameTime gameTime, bool covered)
        {
            if (firstAnimation)
            {
                float distance = (ball.Position - intialPosition).Length();
                float totalDistance = (shootPosition - intialPosition).Length();
                if (distance / totalDistance > 0.5 && !pauseScreenShowed)
                {
                    pauseScreenShowed = true;
                    //add pause screen
                }
                //update distance bar
                if (distance / totalDistance > 1)
                {
                    //firstAnimation = false;
                    //ShootBall(new Vector3(10, 0, -10));
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

            /*if (!ballFallingIntoHole && animation.AnimationStoped && (Vector3.Distance(new Vector3(ball.Position.X, 0, ball.Position.Z), hole.Position) < (ball.Radius + hole.Radius)))
            {
                ballFallingIntoHole = true;
                AnimateBallFalling();
            }
            

            if (ballFallAnimation != null)
                ballFallAnimation.Update(gameTime.ElapsedGameTime);
            */
            targetCamera.Update();
            animation.Update(gameTime.ElapsedGameTime);
            ball.Rotate(animation.Displacement);
            //update ball height
            //ball.setHeight(environment.GetHeight(ball.Position));
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
            /*ballFallAnimation = new ModelFramedAnimation(ball);

            if (shootVelocity.X > 0)
                ballFallAnimation.AddFrame(new Vector3(ball.Position.X + 0.5f, -0.5f, ball.Position.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
            else
                ballFallAnimation.AddFrame(new Vector3(ball.Position.X - 0.5f, -0.5f, ball.Position.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
            ballFallAnimation.AddFrame(hole.Position, Vector3.Zero, TimeSpan.FromSeconds(0.75));*/
        }

        public override void Draw(GameTime gameTime)
        {
            Camera camera = targetCamera;
            if (simulation != null)
            {
                camera = simulation.Camera;
            }
            // draw environment
            ball.Draw(camera);
            if (firstAnimation)
            {
                //draw distance bar
            }
            if (simulation != null)
            {
                simulation.Draw();
            }
        }

        public override void UnloadContent()
        {

        }

    }

}
