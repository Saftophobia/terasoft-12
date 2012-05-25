using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Mechanect.Screens;
using Mechanect.Common;
using Physics;

namespace Mechanect.Exp3
{
    /// <summary>
    /// Holds the information about the user for exp3.
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Cena </para>
    /// </remarks>
    public class User3:User
    {

        #region InstanceVariables
        #region Leg
        public bool RightLeg { get; set; }
        #endregion
        #region rightLegPositions
        public double InitialRightLegPositionX { get; set; }
        public double CurrentRightLegPositionX { get; set; }
        public double InitialRightLegPositionZ { get; set; }
        public double CurrentRightLegPositionZ { get; set; }
        public double PreviousRightLegPositionX { get; set; }
        public double PreviousRightLegPositionZ { get; set; }
        public double StartRightLegPositionX { get; set; }
        public double StartRightLegPositionZ { get; set; }
        #endregion
        #region leftLegPositions
        public double InitialLeftLegPositionX { get; set; }
        public double CurrentLeftLegPositionX { get; set; }
        public double InitialLeftLegPositionZ { get; set; }
        public double CurrentLeftLegPositionZ { get; set; }
        public double PreviousLeftLegPositionX { get; set; }
        public double PreviousLeftLegPositionZ { get; set; }
        public double StartLeftLegPositionX { get; set; }
        public double StartLeftLegPositionZ { get; set; }
        #endregion
        #region others
        public Vector3 Velocity { get; set; }
        public double Angle { get; set; }
        public double AssumedLegMass { get; set; }
        public Vector3 ShootingPosition { get; set; }
        #endregion
        #region time
        private double currentTime;
        private double initialTime;
        #endregion
        #region states
        public bool HasShot { get; set; }
        public bool HasMissed { get; set; }
        public bool HasPlayerMovedProperty { get; set; }
        private bool movedForward;
        private bool firstUpdate;
        private bool hasJustStarted;
        #endregion
        #endregion
        #region Constructor
        /// <summary>
        /// Creates an new instance of User3.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Cena </para>
        /// </remarks>
        public User3()
        {



            InitialLeftLegPositionX = 0;
            CurrentLeftLegPositionX = 0;
            InitialLeftLegPositionZ = 0;
            CurrentLeftLegPositionZ = 0;

            InitialRightLegPositionX = 0;
            CurrentRightLegPositionX = 0;
            InitialRightLegPositionZ = 0;
            CurrentRightLegPositionZ = 0;

            Velocity = Vector3.Zero;
            Angle = 0;

            movedForward = false;
            AssumedLegMass = 0.01f;
            currentTime = 0;
            initialTime = 0;
            hasJustStarted = true;
            HasShot = false;
            firstUpdate = true;
            HasMissed = false;
            HasPlayerMovedProperty = false;




        }
        #endregion
        #region Khaled'sMethods
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
             AssumedLegMass = GenerateFootMass(minMass, maxMass);
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

        #endregion
        #region Cena'sMethods

         #region UpdateMethods

         /// <summary>
         /// This method updates the velocity vector of the user's leg each three XNA frames.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>  
         /// </remarks>
         /// <param name="gameTime">The time used in calculating the velocity</param> 

         public void UpdateMeasuringVelocityAndAngle(GameTime gameTime)
         {
            
             setSkeleton();
             Skeleton skeleton = USER;
             if (skeleton != null)
             {

                 if (!HasShot && !HasMissed)
                 {
                     StoreTime(gameTime);
                     if (GameScreen.frameNumber % 3 == 0)
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
                             if (!HasPlayerMovedProperty)
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
                                     if (movedForward && HasMovedMinimumDistance())
                                         HasShot = true;
                                     if (movedForward && !HasMovedMinimumDistance())
                                     {
                                         HasMissed = true;
                                         Velocity = Vector3.Zero;
                                         Angle = 0;
                                     }
                                     else
                                         StoreInitialTime(gameTime);

                                 }
                             }
                         }
                     }

                 }
                 GameScreen.frameNumber++;

             }
            
             
         }

         /// <summary>
         /// This method updates the value of the velocity vector.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>  
         /// </remarks>

         public void UpdateSpeed()
         {
             double currentZ, initialZ, currentX, initialX;
             if (RightLeg)
             {
                 currentZ = CurrentRightLegPositionZ;
                 initialZ = InitialRightLegPositionZ;
                 currentX = CurrentRightLegPositionX;
                 initialX = InitialRightLegPositionX;
             }
             else
             {
                 currentZ = CurrentLeftLegPositionZ;
                 initialZ = InitialLeftLegPositionZ;
                 currentX = CurrentLeftLegPositionX;
                 initialX = InitialLeftLegPositionX;
             }
             double deltaTime = Math.Abs(currentTime - initialTime);
             Vector3 deltaPosition = new Vector3((float)(currentX - initialX), 0, (float)(currentZ - initialZ));
             Vector3 finalVelocity = Functions.GetVelocity(deltaPosition, deltaTime);
             Velocity = finalVelocity;
         }

         ///<summary>
         /// This method updates the value of the shooting angle.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>  
         /// </remarks>

         public void UpdateAngle()
         {
             double positionX1, positionX2, positionZ1, positionZ2;
            
             if (RightLeg)
             {
                 positionX1 = InitialRightLegPositionX;
                 positionX2 = CurrentRightLegPositionX;
                 positionZ1 = InitialRightLegPositionZ;
                 positionZ2 = CurrentRightLegPositionZ;
             }
             else
             {
                 positionX1 = InitialLeftLegPositionX;
                 positionX2 = CurrentLeftLegPositionX;
                 positionZ1 = InitialLeftLegPositionZ;
                 positionZ2 = CurrentLeftLegPositionZ;
             }
             if (positionZ2 != positionZ1)
                 Angle = Math.Atan((positionX2 - positionX1) / Math.Abs((positionZ2 - positionZ1)));
             else
                 if (positionX1 < positionX2)
                     Angle = (Math.PI / 2);
                 else
                     Angle = -(Math.PI / 2);

         }

            #endregion

         #region StoreMethods
         /// <summary>
         /// This method stores the original position of the user's leg.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para> 
         /// </remarks>
        

         private void StoreStartingPosition()
         {
            StartLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
            StartLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
            StartRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
            StartRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
         }

         /// <summary>
         /// This method stores the initial position of the user's leg when they start moving forward.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>    
         /// </remarks>

         public void StoreInitialPosition()
         {
             InitialLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
             InitialLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
             InitialRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
             InitialRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
             PreviousLeftLegPositionX = InitialLeftLegPositionX;
             PreviousLeftLegPositionZ = InitialLeftLegPositionZ;
             PreviousRightLegPositionX = InitialRightLegPositionX;
             PreviousRightLegPositionZ = InitialRightLegPositionZ;
         }


         /// <summary>
         /// This method stores the current position of the user's leg.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>    
         /// </remarks>
        
         public void StoreCurrentPosition()
         {
             CurrentLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
             CurrentLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
             CurrentRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
             CurrentRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
         }

         /// <summary>
         /// This method stores the previous position of the user's leg.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para> 
         /// </remarks>
         
         public void StorePreviousPosition()
         {
             if (CurrentRightLegPositionZ != 0)
             {
                 PreviousLeftLegPositionX = CurrentLeftLegPositionX;
                 PreviousLeftLegPositionZ = CurrentLeftLegPositionZ;
                 PreviousRightLegPositionX = CurrentRightLegPositionX;
                 PreviousRightLegPositionZ = CurrentRightLegPositionZ;
             }
         }

         /// <summary>
         /// This method stores the initial time that the user started moving his leg forward at.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>    
         /// </remarks>
         
         public void StoreInitialTime(GameTime gameTime)
         {
             initialTime = gameTime.TotalGameTime.TotalSeconds;
         }

         /// <summary>
         /// This method stores the current time.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>    
         /// </remarks>
        
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

         
         #endregion
         
         #region CheckMethods
         /// <summary>
         /// This method checks if the user moved his leg forward a minimum distance.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>   
         /// </remarks>
         /// <returns>A bool which is true if the user moved his leg forward a certain distance.</returns>
         public bool HasMovedMinimumDistance()
         {
             if (RightLeg)
                 return (InitialRightLegPositionZ - CurrentRightLegPositionZ) > Constants3.minimumShootingDistance;
             return (InitialLeftLegPositionZ - CurrentLeftLegPositionZ) > Constants3.minimumShootingDistance;
         }
         /// <summary>
         /// This method checks if the user moved their leg forward
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>   
         /// </remarks>
         /// <returns>A bool that is true if the user moved their leg forward.</returns>


         public bool IsMovingForward()
         {
             double currentZ, previousZ, startZ; 
             if (RightLeg)
             {
                 currentZ = CurrentRightLegPositionZ;
                 previousZ = PreviousRightLegPositionZ;
                 startZ = StartRightLegPositionZ;
             }
             else
             {
                  currentZ = CurrentLeftLegPositionZ;
                  previousZ = PreviousLeftLegPositionZ;
                  startZ = StartLeftLegPositionZ;
             }
             if (startZ >= currentZ)
             {

                 if ((currentZ - previousZ) < (-1 * Constants3.movingForwardTolerance))
                 {
                     movedForward = true;
                     return true;

                 }
             }
             else
             {
                 if (!movedForward)
                 {
                     if (RightLeg)
                     {
                         InitialRightLegPositionZ = CurrentRightLegPositionZ;
                         InitialRightLegPositionX = CurrentRightLegPositionX;
                     }
                     else
                     {
                         InitialLeftLegPositionZ = CurrentLeftLegPositionZ;
                         InitialLeftLegPositionX = CurrentLeftLegPositionX;
                     }
                 }

             }
             return false;
         }
         /// <summary>
         /// This method checks if the user moved their leg.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>   
         /// </remarks>
         /// <returns>A bool that is true if the user moved their leg.</returns>
         public void HasPlayerMoved()
         {


             if (Math.Abs(CurrentLeftLegPositionZ - StartLeftLegPositionZ) > Constants3.legMovementTolerance)
             {
                 RightLeg = false;

                 HasPlayerMovedProperty = true;
                 return;
             }
             if (Math.Abs(CurrentRightLegPositionZ - StartRightLegPositionZ) > Constants3.legMovementTolerance)
             {
                 RightLeg = true;

                 HasPlayerMovedProperty = true;
                 return;
             }

         }
        #endregion

         #region OtherMethods
         /// <summary>
         /// This method initializes all the stored variables.
         /// </summary>
         /// <remarks>
         /// <para>AUTHOR: Cena </para>   
         /// </remarks>

         public void ResetUserForShootingOrTryingAgain()
         {

             InitialLeftLegPositionX = 0;
             CurrentLeftLegPositionX = 0;
             InitialLeftLegPositionZ = 0;
             CurrentLeftLegPositionZ = 0;

             InitialRightLegPositionX = 0;
             CurrentRightLegPositionX = 0;
             InitialRightLegPositionZ = 0;
             CurrentRightLegPositionZ = 0;

             currentTime = 0;
             initialTime = 0;
        
             hasJustStarted = true;
             HasShot = false;
             firstUpdate = true;

             Velocity = Vector3.Zero;
             Angle = 0;

             movedForward = false;
             GameScreen.frameNumber = 0;
             HasMissed = false;
             HasPlayerMovedProperty = false;




         }
     #endregion
         #endregion

    }
}
