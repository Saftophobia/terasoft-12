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

namespace Mechanect.Experimemnt3
{
    public class Experiment3 : Mechanect.Common.GameScreen
    {
        private Ball ball;
        private Vector3 intialPosition;
        private Vector3 shootPosition;
        private float arriveVelocity;
        private float friction;

        private TargetCamera targetCamera;
        private ModelLinearAnimation animation;
        private Simulation simulation;

        private bool pauseScreenShowed;
        private bool ballStoot;
        private bool simulationStarted;


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
            targetCamera.Update();
            animation.Update(gameTime.ElapsedGameTime);
            //update ball height
            base.Update(gameTime, covered);
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
