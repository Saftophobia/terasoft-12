using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Mechanect.Screens;

namespace Mechanect.Classes
{
    class User3:User
    {
        private bool hasSetInitialPositionForAngle;
        public bool HasSetInitialPositionForAngle
        {
            get
            {
                return hasSetInitialPositionForAngle;
            }
            set
            {
                hasSetInitialPositionForAngle = value;
            }
        }


        private Joint trackedJoint;
        public Joint TrackedJoint  //  the user's tracked joint
        {
            get
            {
                return trackedJoint;
            }
            set
            {
                trackedJoint = value;
            }
        }

        private bool rightLeg;  // is the user using his right leg or left
        public bool RightLeg
        {
            get
            {
                return rightLeg;
            }
            set
            {
                rightLeg = value;
            }
        }

        private bool movedForward; // has the user moved his leg forward
        public bool MovedForward
        {
            get
            {
                return movedForward;
            }
            set
            {
                movedForward = value;
            }
        }

        //x position
        private double initialRightLegPositionX; //meters // stores the initial position of the right leg
        public double InitialRightLegPositionX
        {
            get
            {
                return initialRightLegPositionX;
            }
            set
            {
                initialRightLegPositionX = value;
            }
        }
        private double currentRightLegPositionX; //meters // stores the current position of the right leg 
        public double CurrentRightLegPositionX
        {
            get
            {
                return currentRightLegPositionX;
            }
            set
            {
                currentRightLegPositionX = value;
            }
        }

        //z position
        private double initialRightLegPositionZ; //meters
        public double InitialRightLegPositionZ
        {
            get
            {
                return initialRightLegPositionZ;
            }
            set
            {
                initialRightLegPositionZ = value;
            }
        }


        private double currentRightLegPositionZ; //meters
        public double CurrentRightLegPositionZ
        {
            get
            {
                return currentRightLegPositionZ;
            }
            set
            {
                currentRightLegPositionZ = value;
            }
        }

        // store 

        private double storeX1;
        public double StoreX1
        {
            get
            {
                return storeX1;
            }
            set
            {
                storeX1 = value;

            }
        }
        private double storeX2;
        public double StoreX2
        {
            get
            {
                return storeX2;
            }
            set
            {
                storeX2 = value;

            }
        }
        private double storeZ1;
        public double StoreZ1
        {
            get
            {
                return storeZ1;
            }
            set
            {
                storeZ1 = value;

            }
        }
        public double storeZ2;
        public double StoreZ2
        {
            get
            {
                return storeZ2;
            }
            set
            {
                storeZ2 = value;

            }
        }


        //left leg

        //x position
        private double initialLeftLegPositionX; //meters // stores the initial position of the left leg
        public double InitialLeftLegPositionX
        {
            get
            {
                return initialLeftLegPositionX;
            }
            set
            {
                initialLeftLegPositionX = value;
            }
        }

        private double currentLeftLegPositionX; //meters // stores the current position of the left leg 
        public double CurrentLeftLegPositionX
        {
            get
            {
                return currentLeftLegPositionX;
            }
            set
            {
                currentLeftLegPositionX = value;
            }
        }

        //z position
        private double initialLeftLegPositionZ; //meters
        public double InitialLeftLegPositionZ
        {
            get
            {
                return initialLeftLegPositionZ;
            }
            set
            {
                initialLeftLegPositionZ = value;
            }
        }


        private double currentLeftLegPositionZ; //meters
        public double CurrentLeftLegPositionZ
        {
            get
            {
                return currentLeftLegPositionZ;
            }
            set
            {
                currentLeftLegPositionZ = value;
            }
        }



        private double velocity;
        public double Velocity // the velocity of the users leg
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }
        private bool trying; // is the user in the trying mode or shooting
        public bool Trying
        {
            get
            {
                return trying;
            }
            set
            {
                trying = value;
            }
        }

        private double angle; // angle of the leg
        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }
        private double assumedLegMass;
        public double AssumedLegMass
        {
            get
            {
                return assumedLegMass;
            }
            set
            {
                assumedLegMass = value;
            }
        }
        private Vector3 shootingPosition;
        public Vector3 ShootingPosition
        {
            get
            {
                return shootingPosition;
            }
            set
            {
                shootingPosition = value;
            }
        }
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

            storeX1 = 0;
            storeX2 = 0;
            storeZ1 = 0;
            storeZ2 = 0;
            
            velocity = 0;
            angle = 0;
        
            movedForward = false;
            trying = true;
            hasSetInitialPositionForAngle = false;
            assumedLegMass = GenerateFootMass(Constants3.normalLegMass, 0.04f);
         

        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Takes the minimum and maximum possible values for the mass of the foot and sets it to a random float number between these two numbers.
        /// </summary>
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
         /// <remarks>
         ///<para>AUTHOR: Khaled Salah </para>
         ///</remarks>
         /// <summary>
         /// Takes the minimum and maximum possible values for the mass of the foot and generates a random float between these two numbers.
         /// </summary>
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

         public void Update_MeasuringVelocityAndAngle()
         {
             setSkeleton();
             Skeleton skeleton = USER;

             if (skeleton != null)
             {
                 if (PauseScreen.frameNumber != -1)
                 {
                     if (PauseScreen.frameNumber % 2 == 0) //30 fps kinect, 60fps XNA 
                     {
                         StorePosition();
                         if (PauseScreen.frameNumber == 0)
                         {
                             InitalizePlayerPosition();

                         }
                         else
                         {
                             SetCurrentPosition();

                             if (HasPlayerMovedHisAnkle())
                             {
                                 if (IsMovingForward())
                                 {
                                     UpdateSpeed();
                                     UpdateAngle();
                                     UpdatePosition();


                                 }
                                 else
                                     if (MovedForward)
                                         PauseScreen.frameNumber = -1;

                             }
                         }
                     }
                     if (PauseScreen.frameNumber != -1)
                         PauseScreen.frameNumber++;
                 }
             }
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

         public  bool IsMovingForward()
         {
             if (this.USER != null)
             {
                 double currentZ = CurrentLeftLegPositionZ;
                 double initialZ = InitialLeftLegPositionZ;
                 if (RightLeg)
                 {
                     currentZ = CurrentRightLegPositionZ;
                     initialZ = InitialRightLegPositionZ;
                 }

                 if (currentZ - initialZ < (-1 * Constants3.legMovementTolerance))
                 {
                     MovedForward = true;
                     return true;

                 }
                 if (!MovedForward)
                     if (RightLeg) InitialRightLegPositionZ = CurrentRightLegPositionZ;
                     else InitialLeftLegPositionZ = CurrentLeftLegPositionZ;


             }
             return false;
         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// Calculates the new value of the velocity and sets it the variable velocity of the User3 to it
         /// </summary>

         public void UpdateSpeed()
         {
             double currentZ = CurrentLeftLegPositionZ;
             double initialZ = InitialLeftLegPositionZ;
             if (RightLeg)
             {
                 currentZ = CurrentRightLegPositionZ;
                 initialZ = InitialRightLegPositionZ;
             }
             double velocityFinal = 0;
             double deltaPosition = Math.Abs(currentZ - initialZ);
             double acceleration = Tools3.GetAcceleration(deltaPosition, Constants3.unitTime, Velocity);
             velocityFinal = Tools3.GetVelocity(acceleration, Velocity, Constants3.unitTime);
             if (Trying)
                 Velocity = Math.Max(Velocity, velocityFinal);
             else
                 Velocity = velocityFinal;
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// updates the current and previous positions of the User3's moving leg
         /// </summary>
         public void UpdatePosition()
         {
             if (RightLeg)
             {
                 InitialRightLegPositionX = CurrentRightLegPositionX;
                 InitialRightLegPositionZ = CurrentRightLegPositionZ;
             }
             else
             {
                 InitialLeftLegPositionX = CurrentLeftLegPositionX;
                 InitialLeftLegPositionZ = CurrentLeftLegPositionZ;
             }
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// Calculates the value of new the angle that the User3 is moving their leg with and sets the variable angle to it
         /// </summary>

         public  void UpdateAngle()
         {
             if (storeZ2 != StoreZ1)
                 Angle = (Math.Atan((StoreX2 - StoreX1) / (Math.Abs(StoreZ2 - StoreZ1))));
             else
                 Angle = (Math.PI / 2);
         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// stores the position where the User3 started moving their leg and the final position where they stopped moving their leg forward
         /// </summary>
       
         public  void StorePosition()
         {

             if (RightLeg)
             {
                 if (MovedForward && !HasSetInitialPositionForAngle)
                 {
                     StoreX1 = InitialRightLegPositionX;
                     StoreZ1 = InitialRightLegPositionZ;
                     HasSetInitialPositionForAngle = true;
                 }
                 StoreX2 = InitialRightLegPositionX;
                 StoreZ2 = InitialRightLegPositionZ;

             }
             else
             {
                 if (MovedForward && !HasSetInitialPositionForAngle)
                 {
                     StoreX1 = InitialLeftLegPositionX;
                     StoreZ1 = InitialLeftLegPositionZ;
                     HasSetInitialPositionForAngle = true;
                 }
                 StoreX2 = InitialLeftLegPositionX;
                 StoreZ2 = InitialLeftLegPositionZ;
             }
         }

      
      


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// sets the current leg positions of the User3 from the detected skeleton
         /// </summary>
        

         public void SetCurrentPosition()
         {
             CurrentLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
             CurrentLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
             CurrentRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;
             CurrentRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// stores the initial position of the User3
         /// </summary>
         public  void InitalizePlayerPosition()
         {

             InitialLeftLegPositionZ = USER.Joints[JointType.AnkleLeft].Position.Z;
             InitialLeftLegPositionX = USER.Joints[JointType.AnkleLeft].Position.X;
             InitialRightLegPositionZ = USER.Joints[JointType.AnkleRight].Position.Z;
             InitialRightLegPositionX = USER.Joints[JointType.AnkleRight].Position.X;

         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// checks if the User3 moved their leg from the position they were standing initially
         /// </summary>
         public  bool HasPlayerMovedHisAnkle()
         {
             int movementState = 0;  // 0 has not moved, 1 moved one leg, 2 moved both legs
             Skeleton player = USER;

             if (Math.Abs(CurrentLeftLegPositionZ - InitialLeftLegPositionZ) > Constants3.legMovementTolerance)
             {
                 RightLeg = false;
                 TrackedJoint = player.Joints[JointType.AnkleLeft];
                 movementState++;

             }

             if (Math.Abs(CurrentRightLegPositionZ - InitialRightLegPositionZ) > Constants3.legMovementTolerance)
             {
                 RightLeg = true;
                 TrackedJoint = player.Joints[JointType.AnkleRight];
                 movementState++;
             }
             if (movementState == 1) return true;
             if (movementState == 2) // player has changed their position
             {
                 movementState = 0;
                 InitialLeftLegPositionX = player.Joints[JointType.AnkleLeft].Position.X;
                 InitialLeftLegPositionZ = player.Joints[JointType.AnkleLeft].Position.Z;
                 InitialRightLegPositionX = player.Joints[JointType.AnkleRight].Position.X;
                 InitialRightLegPositionZ = player.Joints[JointType.AnkleRight].Position.Z;

             }


             return false;
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
         public float SetVelocityRelativeToGivenMass()
         {
             return (float)((Constants3.normalLegMass / AssumedLegMass) * Velocity);
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

             InitialLeftLegPositionX = 0;
             CurrentLeftLegPositionX = 0;
             InitialLeftLegPositionZ = 0;
             CurrentLeftLegPositionZ = 0;

             InitialRightLegPositionX = 0;
             CurrentRightLegPositionX = 0;
             InitialRightLegPositionZ = 0;
             CurrentRightLegPositionZ = 0;

             StoreX1 = 0;
             StoreX2 = 0;
             StoreZ1 = 0;
             StoreZ2 = 0;

             Velocity = 0;
             Angle = 0;

             MovedForward = false;
             Trying = true;
             HasSetInitialPositionForAngle = false;

             PauseScreen.frameNumber = 0;
         }
       
    }
}
