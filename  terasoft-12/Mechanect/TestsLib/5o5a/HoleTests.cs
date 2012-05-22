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
        [SetUp]
        public void Init()
        {
            game1 = new Game1();
            game1.Content.RootDirectory = @"F:\SE\ terasoft-12\Mechanect\TestsLib\bin\Debug\Content";
            game1.Run();
            environment3 = Constants3.environment3;
            screenManager = new ScreenManager(game1);
        }

        [Test]
        public void Test()
        {
            hole = new Hole(screenManager.Game.Content, screenManager.GraphicsDevice, environment3.terrainWidth, environment3.terrainHeight, 10, environment3.user.shootingPosition);
            Assert.LessOrEqual(environment3.HoleProperty.Position.Z, (environment3.user.shootingPosition.Z - hole.Radius));
            Assert.GreaterOrEqual(environment3.HoleProperty.Position.Z, -(environment3.terrainHeight - hole.Radius) / 2);
            Assert.LessOrEqual(environment3.HoleProperty.Position.X, environment3.terrainWidth / 4);
            Assert.GreaterOrEqual(environment3.HoleProperty.Position.X, -environment3.terrainWidth / 4);
            Assert.AreEqual(environment3.HoleProperty.Position.Y, 0);
        }
    }
}
