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
            Assert.LessOrEqual(environment3.HoleProperty.Position.Z, Constants3.maxHolePosZ);
            Assert.GreaterOrEqual(environment3.HoleProperty.Position.Z, -(Constants3.maxHolePosZ));
            Assert.LessOrEqual(environment3.HoleProperty.Position.X, Constants3.maxHolePosX);
        }
        [Test]
        public void RadiusTest()
        {
            Assert.LessOrEqual(Environment3.GenerateRadius(angleTolerance), 40);
            Assert.GreaterOrEqual(Environment3.GenerateRadius(angleTolerance), 5);
        }
    }
}
