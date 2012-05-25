using NUnit.Framework;
using Mechanect.Exp3;

namespace Tests
{
    [TestFixture]
    public class HoleTests
    {
        Experiment3 experiment3;
        Environment3 environment3;
        int angleTolerance;

        [SetUp]
        public void Init()
        {

            experiment3 = new Experiment3(new User3());
            environment3 = experiment3.EnvironmentProperty; 
            angleTolerance = 4;
        }

        [Test]
        public void Test()
        {
            Assert.LessOrEqual(environment3.HoleProperty.Position.Z, environment3.TerrainHeight);
            Assert.GreaterOrEqual(environment3.HoleProperty.Position.Z, -(environment3.TerrainHeight));
            Assert.LessOrEqual(environment3.HoleProperty.Position.X, environment3.TerrainWidth);
            Assert.GreaterOrEqual(environment3.HoleProperty.Position.X, -environment3.TerrainWidth);
        }
        [Test]
        public void RadiusTest()
        {
            Assert.LessOrEqual(Environment3.GenerateRadius(angleTolerance), 40);
            Assert.GreaterOrEqual(Environment3.GenerateRadius(angleTolerance), 5);
        }
    }
}
