using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Mechanect.Exp3;
using Microsoft.Xna.Framework;

namespace TestsLib.Bisho
{
    public class BallTests
    {
        private Ball ball;

        [SetUp]
        public void Initialize()
        {
            ball = new Ball(3f);
        }

        [Test]
        public void TestBallRotation()
        {
            ball.Rotate(new Vector3(10, 0, 10));
            Assert.AreEqual(ball.Rotation.Y, (float)(Math.PI / 4));
        }

        [Test]
        public void TestSetHeight()
        {
            ball.SetHeight(1);
            Assert.AreEqual(ball.Position.Y, 4);
        }

        [Test]
        public void TestInsideTerrain()
        {
            for (int i = 0; i < 10; i++)
            {
                ball.GenerateInitialPosition(300, 300);
                Assert.True(ball.InsideTerrain(300, 300));
            }      
        }

    }
}
