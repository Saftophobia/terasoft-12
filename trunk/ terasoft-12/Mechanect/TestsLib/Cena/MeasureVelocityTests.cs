using System;
using NUnit.Framework;
using Mechanect;
using Mechanect.Exp3;

namespace TestsLib
{
    [TestFixture]
    public class MeasureVelocityTests
    {
        #region InstanceVariables
        User3 user;
        int frameNumber;
        const double step = 0.5;
        #endregion
        #region Initialization
        [SetUp]
        public void Init()
        {
            user = new User3();
            frameNumber = 0;
           
        }
        #endregion
        #region IsMovingForwardTests
        [Test]
        public void IsMovingForwardTrueLeft()
        {
            bool test = true;
            Reset();
            while(frameNumber < 100){
                GenerateFrameForward();
                test &= user.IsMovingForward();
                frameNumber++;
            }
            Assert.IsTrue(test);
        }
        [Test]
        public void IsMovingForwardFalseLeft()
        {
            bool test = true;
            Reset();
            while (frameNumber < 100)
            {
                GenerateFrameBackward();
                test &= user.IsMovingForward();
                frameNumber++;
            }
            Assert.IsFalse(test);
        }
        [Test]
        public void IsMovingForwardForwardBackwardLeft()
        {
            bool test = true;
            Reset();
            while (frameNumber < 100)
            {

                if (frameNumber < RandomFrame())
                    GenerateFrameForward();
                else
                    GenerateFrameBackward();
                test &= user.IsMovingForward();
                frameNumber++;
            }
            Assert.IsFalse(test);
        }
        [Test]
        public void IsMovingForwardTrueRight()
        {
            bool test = true;
            Reset();
            user.RightLeg = true;
            while (frameNumber < 100)
            {
                GenerateFrameForward();
                test &= user.IsMovingForward();
                frameNumber++;
            }
            Assert.IsTrue(test);
        }
        [Test]
        public void IsMovingForwardFalseRight()
        {
            bool test = true;
            user.RightLeg = true;
            Reset();
            while (frameNumber < 100)
            {
                GenerateFrameBackward();
                test &= user.IsMovingForward();
                frameNumber++;
            }
            Assert.IsFalse(test);
        }
        [Test]
        public void IsMovingForwardForwardBackwardRight()
        {
            bool test = true;
            Reset();
            user.RightLeg = true;
            while (frameNumber < 100)
            {

                if (frameNumber < RandomFrame())
                    GenerateFrameForward();
                else
                    GenerateFrameBackward();
                test &= user.IsMovingForward();
                frameNumber++;
            }
            Assert.IsFalse(test);
        }
        #endregion
        #region HasMovedMinimumDistanceTests
        [Test]
        public void HasMovedMinimumDistanceTrueRight()
        {
            Reset();
            user.RightLeg = true;
            bool test = false;
            while (frameNumber < 100)
            {
                if (frameNumber < RandomFrame())
                    GenerateFrameForward();
                test |= user.HasMovedMinimumDistance();
                frameNumber++;
            }
            Assert.IsTrue(test);
        }
        [Test]
        public void HasMovedMinimumDistanceFalseRight()
        {
            Reset();
            user.RightLeg = true;
            bool test = false;
            while (frameNumber < 100)
            {
                test |= user.HasMovedMinimumDistance();
                frameNumber++;
            }
            
          
            Assert.IsFalse(test);
        }
        [Test]
        public void HasMovedMinimumDistanceTrueLeft()
        {
            Reset();
            bool test = false;
            GenerateFrameForward();
            user.RightLeg = false;
            while (frameNumber < 100)
            {
                if (frameNumber == RandomFrame())
                    GenerateFrameForward();
                test |= user.HasMovedMinimumDistance();
                frameNumber++;
            }
            Assert.IsTrue(test);

        }
        [Test]
        public void HasMovedMinimumDistanceFalseLeft()
        {
            Reset();
            bool test = false;
            user.RightLeg = false;
            while (frameNumber < 100)
            {
                test |= user.HasMovedMinimumDistance();
                frameNumber++;
            }
            Assert.IsFalse(test);
        }
        #endregion
        #region HasPlayerMovedTests
        [Test]
        public void HasPlayerMovedTrueForward()
        {
            bool test = false;
            while (frameNumber < 100)
            {

                if (frameNumber == RandomFrame())
                    GenerateFrameForward();
                frameNumber++;
                user.HasPlayerMoved();
                test |= user.HasPlayerMovedProperty;
            }
            Assert.IsTrue(test);
        }
        [Test]
        public void HasPlayerMovedTrueBackward()
        {
            bool test = false;
            while (frameNumber < 100)
            {
                if (frameNumber == RandomFrame())
                    GenerateFrameBackward();
                frameNumber++;
                user.HasPlayerMoved();
                test |= user.HasPlayerMovedProperty;
            }
            Assert.IsTrue(test);
        }
        [Test]
        public void HasPlayerMovedFalse()
        {
            bool test = false;
            while (frameNumber < 100)
            {
                frameNumber++;
                user.HasPlayerMoved();
                test |= user.HasPlayerMovedProperty;
            }
            Assert.IsFalse(test);
        }
        #endregion
        #region Helper Methods
        private void Reset()
        {
            user.PreviousLeftLegPositionX = 0;
            user.PreviousLeftLegPositionZ = 0;
            user.PreviousRightLegPositionX = 0;
            user.PreviousRightLegPositionZ = 0;
            user.CurrentLeftLegPositionX = 0;
            user.CurrentLeftLegPositionZ = 0;
            user.CurrentRightLegPositionX = 0;
            user.CurrentRightLegPositionZ = 0;
            user.RightLeg = false;
            frameNumber = 0;
        }
        private void GenerateFrameBackward()
        {
            if (!user.RightLeg)
            {
                user.PreviousLeftLegPositionX = user.CurrentLeftLegPositionX;
                user.PreviousLeftLegPositionZ = user.CurrentLeftLegPositionZ;
                user.CurrentLeftLegPositionX += step;
                user.CurrentLeftLegPositionZ += step;
            }
            else
            {
                user.PreviousRightLegPositionX = user.CurrentRightLegPositionX;
                user.PreviousRightLegPositionZ = user.CurrentRightLegPositionZ;
                user.CurrentRightLegPositionX += step;
                user.CurrentRightLegPositionZ += step;
            }
        }
        private void GenerateFrameForward()
        {
            if (!user.RightLeg)
            {
                user.PreviousLeftLegPositionX = user.CurrentLeftLegPositionX;
                user.PreviousLeftLegPositionZ = user.CurrentLeftLegPositionZ;
                user.CurrentLeftLegPositionX -= step;
                user.CurrentLeftLegPositionZ -= step;
                
            }
            else
            {

                user.PreviousRightLegPositionX = user.CurrentRightLegPositionX;
                user.PreviousRightLegPositionZ = user.CurrentRightLegPositionZ;
                user.CurrentRightLegPositionX -= step;
                user.CurrentRightLegPositionZ -= step;
            }
        }
        private int RandomFrame()
        {
            return new Random().Next(100);
        }
        #endregion
    }
}
