using System;
using NUnit.Framework;
using Mechanect;
using Mechanect.Exp3;
using Mechanect.Common;

namespace Tests
{
    [TestFixture]
    public class HoleTests
    {
        Game1 game1;
        Hole hole;
        ScreenManager screenManager;
        Environment3 environment3;
        int angleTolerance;
        [SetUp]
        public void Init()
        {
            game1 = new Game1();
            game1.Content.RootDirectory = @"F:\SE\ terasoft-12\Mechanect\TestsLib\bin\Debug\Content";
            game1.Run();
            environment3 = Constants3.environment3;
            screenManager = new ScreenManager(game1);
            angleTolerance = 4;
        }

        [Test]
        public void Test()
        {
            hole = new Hole(screenManager.Game.Content, screenManager.GraphicsDevice, environment3.terrainWidth, environment3.terrainHeight, 10, environment3.user.shootingPosition);
            Assert.LessOrEqual(environment3.HoleProperty.Position.Z, environment3.terrainHeight);
            Assert.GreaterOrEqual(environment3.HoleProperty.Position.Z, -(environment3.terrainHeight));
            Assert.LessOrEqual(environment3.HoleProperty.Position.X, environment3.terrainWidth);
            Assert.GreaterOrEqual(environment3.HoleProperty.Position.X, -environment3.terrainWidth);
        }
        [Test]
        public void RadiusTest()
        {
            Assert.LessOrEqual(environment3.GenerateRadius(angleTolerance), 40);
            Assert.GreaterOrEqual(environment3.GenerateRadius(angleTolerance), 5);
        }
    }
}
