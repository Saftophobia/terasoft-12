using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using UI.Cameras;
using UI.Animation;


namespace Mechanect.Experimemnt3
{
    public class Experiment3 : Mechanect.Common.GameScreen
    {
        private Ball ball;
        private Vector3 intialPosition;
        private float arriveVelocity;
        private Vector3 shootPosition;
        private float friction;

        private bool pauseScreenShowed;
        private bool ballStoot;
        private TargetCamera camera;
        private ModelLinearAnimation animation;

        public Experiment3()
        {
            shootPosition = new Vector3(0, 0, 0);
            friction = -2;
            arriveVelocity = 15;

            intialPosition = new Vector3(-100, 0, -100);
        }

        public override void LoadContent()
        {
            camera = new TargetCamera(new Vector3(0, 60, 60), Vector3.Zero, ScreenManager.GraphicsDevice);
            ball = new Ball(intialPosition, 10, ScreenManager.GraphicsDevice, ScreenManager.Game.Content);
            animation = new ModelLinearAnimation(ball, shootPosition, arriveVelocity, friction, true);
        }

        public void animateBall(Vector3 velocity)
        {
            ball.Rotation = Vector3.Zero;
            animation = new ModelLinearAnimation(ball, velocity, friction, TimeSpan.FromSeconds(10), true);
        }

        public override void Update(GameTime gameTime, bool covered)
        {
            if (pauseScreenShowed)
            {
                if (animation.AnimationStoped && !ballStoot)
                {
                    ballStoot = true;
                    animateBall(new Vector3(10, 0, -10));
                }
            }
            else
            {
                float distance = (ball.Position - intialPosition).Length();
                float totalDistance = (shootPosition - intialPosition).Length();
                if (distance / totalDistance > 0.5 && !pauseScreenShowed)
                {
                    pauseScreenShowed = true;
                    //add pause screen
                }
            }
            camera.Update();
            animation.Update(gameTime.ElapsedGameTime);
            base.Update(gameTime, covered);
        }

        public override void Draw(GameTime gameTime)
        {
            ball.Draw(camera);
        }

        public override void UnloadContent()
        {

        }

    }

}
