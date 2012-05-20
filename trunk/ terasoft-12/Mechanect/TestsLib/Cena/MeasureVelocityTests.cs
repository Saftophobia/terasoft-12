using System;
using NUnit.Framework;
using Mechanect;
using Mechanect.Exp3;
using Microsoft.Kinect;
using System.Threading;

namespace TestsLib
{
    [TestFixture]
    public class Cena
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
            user.rightLeg = true;
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
            user.rightLeg = true;
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
            user.rightLeg = true;
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
            user.rightLeg = true;
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
            user.rightLeg = true;
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
            user.rightLeg = false;
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
            user.rightLeg = false;
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
                test |= user.hasPlayerMoved;
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
                test |= user.hasPlayerMoved;
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
                test |= user.hasPlayerMoved;
            }
            Assert.IsFalse(test);
        }
        #endregion
        #region Helper Methods
        private void Reset()
        {
            user.previousLeftLegPositionX = 0;
            user.previousLeftLegPositionZ = 0;
            user.previousRightLegPositionX = 0;
            user.previousRightLegPositionZ = 0;
            user.currentLeftLegPositionX = 0;
            user.currentLeftLegPositionZ = 0;
            user.currentRightLegPositionX = 0;
            user.currentRightLegPositionZ = 0;
            user.rightLeg = false;
            frameNumber = 0;
        }
        private void GenerateFrameBackward()
        {
            if (!user.rightLeg)
            {
                user.previousLeftLegPositionX = user.currentLeftLegPositionX;
                user.previousLeftLegPositionZ = user.currentLeftLegPositionZ;
                user.currentLeftLegPositionX += step;
                user.currentLeftLegPositionZ += step;
            }
            else
            {
                user.previousRightLegPositionX = user.currentRightLegPositionX;
                user.previousRightLegPositionZ = user.currentRightLegPositionZ;
                user.currentRightLegPositionX += step;
                user.currentRightLegPositionZ += step;
            }
        }
        private void GenerateFrameForward()
        {
            if (!user.rightLeg)
            {
                user.previousLeftLegPositionX = user.currentLeftLegPositionX;
                user.previousLeftLegPositionZ = user.currentLeftLegPositionZ;
                user.currentLeftLegPositionX -= step;
                user.currentLeftLegPositionZ -= step;
            }
            else
            {

                user.previousRightLegPositionX = user.currentRightLegPositionX;
                user.previousRightLegPositionZ = user.currentRightLegPositionZ;
                user.currentRightLegPositionX -= step;
                user.currentRightLegPositionZ -= step;
            }
        }
        private int RandomFrame()
        {
            return new Random().Next(100);
        }
        #endregion
    }
}
