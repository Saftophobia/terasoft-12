using System;
using NUnit.Framework;
using Physics;
using Microsoft.Xna.Framework;
using Mechanect.Exp3;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TestsLib.Omar
{
    class BallTests
    {
        Ball ball;
        Hole hole;
        User3 user;
        BallAnimation animation;
        Experiment3 exp;
        Environment3 environment;
        GraphicsDevice graphics;
        ContentManager Content;
        float friction;

        [SetUp]
        public void Init()
        {
            user = new User3();
            user.shootingPosition = new Vector3(0, 3, 62);
            exp = new Experiment3(user);
            friction = -2;
            graphics = exp.ScreenManager.GraphicsDevice;
            Content = exp.ScreenManager.Game.Content;
            environment = Constants3.environment3;
            ball = environment.ball;
            hole = environment.HoleProperty;
            
        }


        [Test]
        public void CheckIfFall()
        {
            animation = new BallAnimation(ball, environment, LinearMotion.CalculateInitialVelocity(hole.Position - user.shootingPosition, 0, friction));
            Assert.IsTrue(animation.willFall);
        }

        [Test]
        public void CheckIfWontFall()
        {
            animation = new BallAnimation(ball, environment, LinearMotion.CalculateInitialVelocity(hole.Position - user.shootingPosition, 10, friction));
            Assert.IsFalse(animation.willFall);
        }
    }
}
