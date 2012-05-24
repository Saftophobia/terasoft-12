using System;
using NUnit.Framework;
using Physics;
using Microsoft.Xna.Framework;
using UI.Animation;
using UI.Components;
using Mechanect.Exp3;

namespace Tests
{
    [TestFixture]
    public class AnimationTests
    {
        private CustomModel model;
        private ModelLinearAnimation animation;

        [SetUp]
        public void Initialize()
        {
            model = new CustomModel(Vector3.Zero, Vector3.Zero, Vector3.One);
            animation = new ModelLinearAnimation(model, new Vector3(10, 0, 0), -1, TimeSpan.FromSeconds(20));
        }

        [Test]
        public void TestAnimation()
        {
            Assert.False(animation.Finished);
            animation.Update(TimeSpan.FromSeconds(3));

            Assert.AreEqual(model.Position, new Vector3(25.5f, 0, 0));
            animation.Update(TimeSpan.FromSeconds(5));

            Assert.False(animation.Finished);
            animation.Update(TimeSpan.FromSeconds(13));

            animation.Stop();
            Assert.True(animation.Finished);
        }

    }
}
