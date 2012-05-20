using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Mechanect.Screens;
using Mechanect.Common;

namespace Mechanect.Exp3
{
    public class User3:User
    {


        #region joint
        public bool rightLeg;
        #endregion
        #region rightLegPositions
        public double initialRightLegPositionX { get; set; }
        public double currentRightLegPositionX { get; set; }
        public double initialRightLegPositionZ { get; set; }
        public double currentRightLegPositionZ { get; set; }
        public double previousRightLegPositionX { get; set; }
        public double previousRightLegPositionZ { get; set; }
        public double startRightLegPositionX { get; set; }
        public double startRightLegPositionZ { get; set; }
        #endregion
        #region leftLegPositions
        public double initialLeftLegPositionX { get; set; }
        public double currentLeftLegPositionX { get; set; }
        public double initialLeftLegPositionZ { get; set; }
        public double currentLeftLegPositionZ { get; set; }
        public double previousLeftLegPositionX { get; set; }
        public double previousLeftLegPositionZ { get; set; }
        public double startLeftLegPositionX { get; set; }
        public double startLeftLegPositionZ { get; set; }
        #endregion
        #region others
        public Vector3 velocity { get; set; }
        public double angle { get; set; }
        public double assumedLegMass { get; set; }
        public Vector3 shootingPosition { get; set; }
        #endregion
        #region time
        private double currentTime;
        private double initialTime;
        #endregion
        #region states
        public bool hasShot { get; set; }
        public bool hasMissed { get; set; }
        private bool movedForward;
        private bool firstUpdate;
        public bool hasPlayerMoved;
        private bool hasJustStarted;
        #endregion


        public User3()
        {



            initialLeftLegPositionX = 0;
            currentLeftLegPositionX = 0;
            initialLeftLegPositionZ = 0;
            currentLeftLegPositionZ = 0;

            initialRightLegPositionX = 0;
            currentRightLegPositionX = 0;
            initialRightLegPositionZ = 0;
            currentRightLegPositionZ = 0;

            velocity = Vector3.Zero;
            angle = 0;

            movedForward = false;
            assumedLegMass = 0.01f;
            currentTime = 0;
            initialTime = 0;
            hasJustStarted = true;
            hasShot = false;
            firstUpdate = true;
            hasMissed = false;
            hasPlayerMoved = false;




        }
        /// <summary>
        /// Takes the minimum and maximum possible values for the mass of the foot and sets it to a random float number between these two numbers.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="minMass">
        /// The minimum possible value for the foot mass.
        /// </param>
        /// /// <param name="maxMass">
        /// The maximum possible value for the foot mass.
        /// </param>
     
         public void SetFootMassInThisRange(float minMass, float maxMass)
         {
             assumedLegMass = GenerateFootMass(minMass, maxMass);
         } 
         /// <summary>
         /// Takes the minimum and maximum possible values for the mass of the foot and generates a random float between these two numbers.
         /// </summary>
         /// <remarks>
         ///<para>AUTHOR: Khaled Salah </para>
         ///</remarks>
         /// <param name="minMass">
         /// The minimum possible value for the foot mass.
         /// </param>
         /// /// <param name="maxMass">
         /// The maximum possible value for the foot mass.
         /// </param>
         /// <returns>
         /// Float number which is the generated random value.
         /// </returns>
         public float GenerateFootMass(float min, float max)
         {
             if ((min >= 0) && (max >= 0) && (max > min))
             {
                 var random = new Random();
                 var generatedMass = ((float)(random.NextDouble() * (max - min))) + min;
                 return generatedMass;
             }
             else throw new ArgumentException("parameters have to be non negative numbers and max value has to be greater than min value");
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>>
         /// <summary>
         ///  Updates the velocity, angle variables of the User3 after each captured skeleton frame.
         /// </summary>

         public void UpdateMeasuringVelocityAndAngle(GameTime gameTime)
         {
             setSkeleton();
             Skeleton skeleton = USER;
             if (skeleton != null)
             {
                 if (GameScreen.frameNumber % 3 == 0)
                 {
                     StoreTime(gameTime);
                     if (!hasShot && !hasMissed) 
                     {
                         if (hasJustStarted)
                         {
                             StoreStartingPosition();
                             StoreInitialPosition();
                             hasJustStarted = false;
                         }
                         else
                         {
                             StoreCurrentPosition();
                             if (!hasPlayerMoved)
                                 HasPlayerMoved();
                             else
                             {
                                 if (IsMovingForward())
                                 {
                                    
                                     UpdateSpeed();
                                     UpdateAngle();
                                     StorePreviousPosition();
                                 }
                                 else
                                 {
                                     if (HasAlreadyMovedForward() && HasMovedMinimumDistance())
                                         hasShot = true;
                                     else
                                         if (HasAlreadyMovedForward() && !HasMovedMinimumDistance())
                                             hasMissed = true;
                                         else
                                             StoreInitialTime(gameTime);

                                 }
                             }
                         }
                     }

                 }

             }
             GameScreen.frameNumber++;
         }


         private void StoreStartingPosition()
         {
            startLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
            startLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
            startRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
            startRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
         }

         public bool HasMovedMinimumDistance()
         {
             if (rightLeg)
                 return initialRightLegPositionZ - currentRightLegPositionZ > Constants3.minimumShootingDistance;
             return initialLeftLegPositionZ - currentLeftLegPositionZ > Constants3.minimumShootingDistance;
         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// Checks if the User3 is currently moving their leg forward or not
         /// </summary>
         /// <returns>returns true iff the User3 moved his leg forward</returns>

         public bool IsMovingForward()
         {
                 double currentZ = currentLeftLegPositionZ;
                 double previousZ = previousLeftLegPositionZ;
                 if (rightLeg)
                 {
                     currentZ = currentRightLegPositionZ;
                     previousZ = previousRightLegPositionZ;
                 }

                 if (currentZ - previousZ < (-1 * Constants3.movingForwardTolerance))
                 {
                     movedForward = true;
                     return true;

                 }
                 if (!movedForward)
                 {
                     if (rightLeg)
                     {
                         initialRightLegPositionZ = currentRightLegPositionZ;
                         initialRightLegPositionX = currentRightLegPositionX;
                     }
                     else
                     {
                         initialLeftLegPositionZ = currentLeftLegPositionZ;
                         initialLeftLegPositionX = currentLeftLegPositionX;
                     }
                 

             }
             return false;
         }

         public void UpdateAngle()
         {
             double positionX1 = initialLeftLegPositionX;
             double positionX2 = currentLeftLegPositionX;
             double positionZ1 = initialLeftLegPositionZ;
             double positionZ2 = currentLeftLegPositionZ;
             if (rightLeg)
             {
                 positionX1 = initialRightLegPositionX;
                 positionX2 = currentRightLegPositionX;
                 positionZ1 = initialRightLegPositionZ;
                 positionZ2 = currentRightLegPositionZ;
             }
             if (positionZ2 != positionZ1)
                 angle = Math.Atan((positionX2 - positionX1) / Math.Abs((positionZ2 - positionZ1)));
             else
                 if (positionX1 < positionX2)
                     angle = (Math.PI / 2);
                 else
                     angle = -(Math.PI / 2);

         }




         public void HasPlayerMoved()
         {

          
             if (Math.Abs(currentLeftLegPositionZ - startLeftLegPositionZ) > Constants3.legMovementTolerance)
             {
                 rightLeg = false;
                 
                 hasPlayerMoved = true;
                 return;
             }
             if (Math.Abs(currentRightLegPositionZ - startRightLegPositionZ) > Constants3.legMovementTolerance)
             {
                 rightLeg = true;
                 
                 hasPlayerMoved = true;
                 return;
             }
            
         }




         public bool HasAlreadyMovedForward()
         {
             return movedForward;
         }
         public bool HasShot()
         {
             return hasShot;
         }
         public void UpdateSpeed()
         {
             double currentZ = currentLeftLegPositionZ;
             double initialZ = initialLeftLegPositionZ;
             double currentX = currentLeftLegPositionX;
             double initialX = initialLeftLegPositionX;
             if (rightLeg)
             {
                 currentZ = currentRightLegPositionZ;
                 initialZ = initialRightLegPositionZ;
                 currentX = currentRightLegPositionX;
                 initialX = initialRightLegPositionX;
             }
             double deltaTime = Math.Abs(currentTime - initialTime);
             Vector3 deltaPosition = new Vector3((float)(currentX - initialX), 0, (float)(currentZ - initialZ));
             Vector3 finalVelocity = Tools3.GetVelocity(deltaPosition, deltaTime);
             velocity = finalVelocity;
         }


         public void StoreInitialPosition()
         {
             initialLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
             initialLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
             initialRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
             initialRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
             previousLeftLegPositionX = initialLeftLegPositionX;
             previousLeftLegPositionZ = initialLeftLegPositionZ;
             previousRightLegPositionX = initialRightLegPositionX;
             previousRightLegPositionZ = initialRightLegPositionZ;
         }


         public void StoreCurrentPosition()
         {
             currentLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
             currentLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
             currentRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
             currentRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
         }


         public void StoreInitialTime(GameTime gameTime)
         {
             initialTime = gameTime.TotalGameTime.TotalSeconds;
         }
         public void StoreTime(GameTime gameTime)
         {
             if (firstUpdate)
             {
                 StoreInitialTime(gameTime);
                 firstUpdate = false;
             }
             else
                 currentTime = gameTime.TotalGameTime.TotalSeconds;
         }

         public void StorePreviousPosition()
         {
             if (currentRightLegPositionZ != 0)
             {
                 previousLeftLegPositionX = currentLeftLegPositionX;
                 previousLeftLegPositionZ = currentLeftLegPositionZ;
                 previousRightLegPositionX = currentRightLegPositionX;
                 previousRightLegPositionZ = currentRightLegPositionZ;
             }
         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// scales the velocity of the leg's User3 relative to the assumed mass of the User3's leg and maps the velocity in meters to pixels
         /// </summary>
         /// <returns>returns the scales velocity</returns>
         public Vector3 SetVelocityRelativeToGivenMass()
         {
             float ratio = (float)(Constants3.normalLegMass / assumedLegMass);
             return new Vector3(velocity.X * ratio, 0, velocity.Z * ratio);

         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// initializes all the variables that stores the User3's movement inorder to try shooting again
         /// </summary>

         public void ResetUserForShootingOrTryingAgain()
         {

             initialLeftLegPositionX = 0;
             currentLeftLegPositionX = 0;
             initialLeftLegPositionZ = 0;
             currentLeftLegPositionZ = 0;

             initialRightLegPositionX = 0;
             currentRightLegPositionX = 0;
             initialRightLegPositionZ = 0;
             currentRightLegPositionZ = 0;

             currentTime = 0;
             initialTime = 0;
        
             hasJustStarted = true;
             hasShot = false;
             firstUpdate = true;

             velocity = Vector3.Zero;
             angle = 0;

             movedForward = false;
             GameScreen.frameNumber = 0;
             hasMissed = false;
             hasPlayerMoved = false;




         }
       
    }
}
