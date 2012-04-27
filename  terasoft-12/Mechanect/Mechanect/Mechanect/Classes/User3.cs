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
                         storePosition(this);
                         if (PauseScreen.frameNumber == 0)
                         {
                             initalizePlayerPosition(this);

                         }
                         else
                         {
                             setCurrentPosition(this);

                             if (hasPlayerMovedHisAnkle(this))
                             {
                                 if (isMovingForward(skeleton, this))
                                 {
                                     updateSpeed(this);
                                     updateAngle(this);
                                     updatePosition(this);


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
         ///  resolves the User3's velocity into 2 components VelocityX, VelocityZ
         /// </summary>
         /// <param name="User3">takes instance of class User3 to resolve their velocity</param>
         /// <returns>returns Vector2: holding the velocity components of the User3</returns>

         public static Vector2 resolveUser3Velocity(User3 User3)
         {
             Vector2 velocity;
             velocity.X = (float)(User3.Velocity * Math.Cos(User3.Angle));
             velocity.Y = (float)(User3.Velocity * Math.Sign(User3.Angle));
             return velocity;
         }




         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// Checks if the User3 is currently moving their leg forward or not
         /// </summary>
         /// <param name="skeleton">takes instance of class User3 to check if they are moving their leg forward</param>
         /// <returns>returns true iff the User3 moved his leg forward</returns>

         public static bool isMovingForward(Skeleton skeleton, User3 User3)
         {
             if (skeleton != null)
             {
                 double currentZ = User3.CurrentLeftLegPositionZ;
                 double initialZ = User3.InitialLeftLegPositionZ;
                 if (User3.RightLeg)
                 {
                     currentZ = User3.CurrentRightLegPositionZ;
                     initialZ = User3.InitialRightLegPositionZ;
                 }

                 if (currentZ - initialZ < (-1 * Constants3.legMovementTolerance))
                 {
                     User3.MovedForward = true;
                     return true;

                 }
                 if (!User3.MovedForward)
                     if (User3.RightLeg) User3.InitialRightLegPositionZ = User3.CurrentRightLegPositionZ;
                     else User3.InitialLeftLegPositionZ = User3.CurrentLeftLegPositionZ;


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
         /// <param name="User3">takes and instance of the class User3 to calculate their new velocity</param>

         public static void updateSpeed(User3 User3)
         {
             double currentZ = User3.CurrentLeftLegPositionZ;
             double initialZ = User3.InitialLeftLegPositionZ;
             if (User3.RightLeg)
             {
                 currentZ = User3.CurrentRightLegPositionZ;
                 initialZ = User3.InitialRightLegPositionZ;
             }
             double velocityFinal = 0;
             double deltaPosition = Math.Abs(currentZ - initialZ);
             double acceleration = getAcceleration(deltaPosition, Constants3.unitTime, User3.Velocity);
             velocityFinal = getVelocity(acceleration, User3.Velocity, Constants3.unitTime);
             if (User3.Trying)
                 User3.Velocity = Math.Max(User3.Velocity, velocityFinal);
             else
                 User3.Velocity = velocityFinal;
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// updates the current and previous positions of the User3's moving leg
         /// </summary>
         /// <param name="User3">takes instance of the User3 to update their position</param>
         public static void updatePosition(User3 User3)
         {
             if (User3.RightLeg)
             {
                 User3.InitialRightLegPositionX = User3.CurrentRightLegPositionX;
                 User3.InitialRightLegPositionZ = User3.CurrentRightLegPositionZ;
             }
             else
             {
                 User3.InitialLeftLegPositionX = User3.CurrentLeftLegPositionX;
                 User3.InitialLeftLegPositionZ = User3.CurrentLeftLegPositionZ;
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
         /// <param name="User3">takes instance of class User3 to calculate their angle </param>

         public static void updateAngle(User3 User3)
         {
             if (User3.storeZ2 != User3.StoreZ1)
                 User3.Angle = (Math.Atan((User3.StoreX2 - User3.StoreX1) / (Math.Abs(User3.StoreZ2 - User3.StoreZ1))));
             else
                 User3.Angle = (Math.PI / 2);
         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// stores the position where the User3 started moving their leg and the final position where they stopped moving their leg forward
         /// </summary>
         /// <param name="User3">takes an instance of class User3 to store their position</param>

         public static void storePosition(User3 User3)
         {

             if (User3.RightLeg)
             {
                 if (User3.MovedForward && !User3.HasSetInitialPositionForAngle)
                 {
                     User3.StoreX1 = User3.InitialRightLegPositionX;
                     User3.StoreZ1 = User3.InitialRightLegPositionZ;
                     User3.HasSetInitialPositionForAngle = true;
                 }
                 User3.StoreX2 = User3.InitialRightLegPositionX;
                 User3.StoreZ2 = User3.InitialRightLegPositionZ;

             }
             else
             {
                 if (User3.MovedForward && !User3.HasSetInitialPositionForAngle)
                 {
                     User3.StoreX1 = User3.InitialLeftLegPositionX;
                     User3.StoreZ1 = User3.InitialLeftLegPositionZ;
                     User3.HasSetInitialPositionForAngle = true;
                 }
                 User3.StoreX2 = User3.InitialLeftLegPositionX;
                 User3.StoreZ2 = User3.InitialLeftLegPositionZ;
             }
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// calculates the final velocity using Vf = Vi + a*t
         /// </summary>
         /// <param name="acceleration">acceleration of the body</param>
         /// <param name="velocityInitial">initial veloctiy of the body</param>
         /// <param name="totalTime">the total time the body moved</param>
         /// <returns></returns>

         public static double getVelocity(double acceleration, double velocityInitial, double totalTime)
         {
             return velocityInitial + (acceleration * totalTime);
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// calculate the acceleration using S = Vi*t + 0.5*a*t^2
         /// </summary>
         /// <param name="totalDistance"></param>
         /// <param name="totalTime"></param>
         /// <param name="velocityInitial"></param>
         /// <returns></returns>

         public static double getAcceleration(double totalDistance, double totalTime, double velocityInitial)
         {
             return (((totalDistance - (velocityInitial * totalTime)) * 2) / Math.Pow(totalTime, 2));
         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// sets the current leg positions of the User3 from the detected skeleton
         /// </summary>
         /// <param name="User3">takes instance of class User3 to store their current leg positions</param>

         public static void setCurrentPosition(User3 User3)
         {
             User3.CurrentLeftLegPositionX = User3.USER.Joints[JointType.AnkleLeft].Position.X;
             User3.CurrentLeftLegPositionZ = User3.USER.Joints[JointType.AnkleLeft].Position.Z;
             User3.CurrentRightLegPositionX = User3.USER.Joints[JointType.AnkleRight].Position.X;
             User3.CurrentRightLegPositionZ = User3.USER.Joints[JointType.AnkleRight].Position.Z;
         }

         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// stores the initial position of the User3
         /// </summary>
         /// <param name="User3">takes an instance of the class User3 to store their initial position</param>
         public static void initalizePlayerPosition(User3 User3)
         {

             User3.InitialLeftLegPositionZ = User3.USER.Joints[JointType.AnkleLeft].Position.Z;
             User3.InitialLeftLegPositionX = User3.USER.Joints[JointType.AnkleLeft].Position.X;
             User3.InitialRightLegPositionZ = User3.USER.Joints[JointType.AnkleRight].Position.Z;
             User3.InitialRightLegPositionX = User3.USER.Joints[JointType.AnkleRight].Position.X;

         }


         ///<remarks>
         ///<para>
         ///Author: Cena
         ///</para>
         ///</remarks>
         /// <summary>
         /// checks if the User3 moved their leg from the position they were standing initially
         /// </summary>
         /// <param name="User3">takes a instance of User3 class to check if they moved their leg</param>
         /// <returns>returns true iff the User3 moved their leg</returns>
         public static bool hasPlayerMovedHisAnkle(User3 User3)
         {
             int movementState = 0;  // 0 has not moved, 1 moved one leg, 2 moved both legs
             Skeleton player = User3.USER;

             if (Math.Abs(User3.CurrentLeftLegPositionZ - User3.InitialLeftLegPositionZ) > Constants3.legMovementTolerance)
             {
                 User3.RightLeg = false;
                 User3.TrackedJoint = player.Joints[JointType.AnkleLeft];
                 movementState++;

             }

             if (Math.Abs(User3.CurrentRightLegPositionZ - User3.InitialRightLegPositionZ) > Constants3.legMovementTolerance)
             {
                 User3.RightLeg = true;
                 User3.TrackedJoint = player.Joints[JointType.AnkleRight];
                 movementState++;
             }
             if (movementState == 1) return true;
             if (movementState == 2) // player has changed their position
             {
                 movementState = 0;
                 User3.InitialLeftLegPositionX = player.Joints[JointType.AnkleLeft].Position.X;
                 User3.InitialLeftLegPositionZ = player.Joints[JointType.AnkleLeft].Position.Z;
                 User3.InitialRightLegPositionX = player.Joints[JointType.AnkleRight].Position.X;
                 User3.InitialRightLegPositionZ = player.Joints[JointType.AnkleRight].Position.Z;

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
