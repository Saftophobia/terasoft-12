using System;
using NUnit.Framework;
using Mechanect;
using Mechanect.Exp3;

namespace Tests
{
    [TestFixture]
    public class SolvabilityTests
    {
        Experiment3 experiment3;
        Environment3 environment3;
        
        [SetUp]
        public void Init()
        {
            experiment3 = new Experiment3(new User3());
            environment3 = experiment3.EnvironmentProperty;
        }

        [Test]
        public void TestsHoleXPosition()
        {
            Assert.LessOrEqual(Math.Abs(environment3.HoleProperty.Position.X), Constants3.maxHolePosX - environment3.HoleProperty.Radius);
        }

        [Test]
        public void TestsHoleZPosition()
        {
            Assert.LessOrEqual(Math.Abs(environment3.HoleProperty.Position.Z), Constants3.maxHolePosZ - environment3.HoleProperty.Radius);

        }

        [Test]
        public void TestsHoleAfterShootingPosition()
        {
            Assert.LessOrEqual(environment3.HoleProperty.Position.Z, environment3.user.shootingPosition.Z);
        }

        [Test]
        public void TestsBallRadiusBiggerThanHoleRadius()
        {
            Assert.LessOrEqual(experiment3.BallPorperty.Radius, environment3.HoleProperty.Radius);
        }

        
        [Test]
        public void TestsNegativeFriction()
        {
            Assert.LessOrEqual(Environment3.Friction, 0);
        }

        [Test]
        public void TestsBigFriction()
        {
            Environment3.Friction = -9999999999;
            experiment3.GenerateSolvable();
            Assert.AreNotEqual(-9999999999, Environment3.Friction);
        }

        [Test]
        public void TestsNegativeBallRadius()
        {
            experiment3.BallPorperty.Radius = -30;
            experiment3.GenerateSolvable();
            Assert.GreaterOrEqual(experiment3.BallPorperty.Radius, 0);
        }

        [Test]
        public void TestsNegativeHoleRadius()
        {
            environment3.HoleProperty.Radius = -30;
            experiment3.GenerateSolvable();
            Assert.GreaterOrEqual(environment3.HoleProperty.Radius, 0);
        }

        [Test]
        public void TestsNegativeBallMass()
        {
            experiment3.BallPorperty.Mass = -30;
            experiment3.GenerateSolvable();
            Assert.GreaterOrEqual(experiment3.BallPorperty.Mass, 0);
        }

        [Test]
        public void TestsNegativeLegMass()
        {
            environment3.user.assumedLegMass = -1;
            experiment3.GenerateSolvable();
            Assert.Greater(environment3.user.assumedLegMass, 0);
        }
    }
}
