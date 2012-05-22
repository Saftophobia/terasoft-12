using System;
using NUnit.Framework;
using Physics;
using Microsoft.Xna.Framework;

namespace Tests
{
    [TestFixture]
    public class LinearMotionTests
    {

        [Test]
        public void TestVectorDirection()
        {
            Assert.AreEqual(LinearMotion.GetVectorInDirectionOf(2, new Vector3(3, 0, 0)), new Vector3(2, 0, 0));
        }

        [Test]
        public void TestTime()
        {
            Assert.AreEqual(LinearMotion.CalculateTime(0, 10, 1), TimeSpan.FromSeconds(10));
            Assert.AreEqual(LinearMotion.CalculateTime(10, 0, -1), TimeSpan.FromSeconds(10));
        }

        [Test]
        public void TestDisplacement()
        {
            Assert.AreEqual(LinearMotion.CalculateDisplacement(new Vector3(10, 0, 0), -1, TimeSpan.FromSeconds(3)), new Vector3(25.5f, 0, 0));
        }

        [Test]
        public void TestVelocity()
        {
            Assert.AreEqual(LinearMotion.CalculateIntialVelocity(new Vector3(6, 0, 0), 4, 1), new Vector3(2f, 0, 0));
        }
    }
}
