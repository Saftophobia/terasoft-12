using System;
using NUnit.Framework;
using Mechanect;
using Mechanect.Exp3;

namespace Tests
{
    [TestFixture]
    public class SolvabilityTests
    {
        Game1 game1;
        Environment3 environment3;

        [SetUp]
        public void Init()
        {
            game1 = new Game1();
            game1.Content.RootDirectory = @"F:\SE\ terasoft-12\Mechanect\TestsLib\bin\Debug\Content";
            game1.Run();
            environment3 = Constants3.environment3;
        }

        [Test]
        public void TestsHoleXPosition()
        {
            Assert.LessOrEqual(Math.Abs(environment3.HoleProperty.Position.X), Constants3.maxHolePosX - environment3.HoleProperty.Radius);
        }

        [Test]
        public void TestsHoleZPosition()
        {
            Assert.LessOrEqual(Math.Abs(environment3.HoleProperty.Position.Z), Constants3.maxHolePosZ - environment3.HoleProperty.Radius,
                "pos: " + environment3.HoleProperty.Position.Z);

        }

        [Test]
        public void TestsHoleAfterShootingPosition()
        {
            Assert.LessOrEqual(environment3.HoleProperty.Position.Z, environment3.user.shootingPosition.Z);
        }
    }
}
