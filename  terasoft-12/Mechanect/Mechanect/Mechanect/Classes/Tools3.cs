using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Mechanect.Common;

namespace Mechanect.Classes
{
    class Tools3
    {
        public static int frameNumber = 0;
       
        
        
        
        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>>
        /// <summary>
        ///  Updates the velocity, angle variables of the user after each captured skeleton frame.
        /// </summary>
        /// <param name="user">takes instance of class User as input, to update their velocity and angle </param>

        public static void update_MeasuringVelocityAndAngle(User user)
        {
            user.setSkeleton();
            Skeleton skeleton = user.USER;

            if (skeleton != null)
            {
                if (frameNumber != -1)
                {
                    if (frameNumber % 2 == 0) //30 fps kinect, 60fps XNA 
                    {
                        storePosition(user);
                        if (frameNumber == 0)
                        {
                            initalizePlayerPosition(user);

                        }
                        else
                        {
                            setCurrentPosition(user);

                            if (hasPlayerMovedHisAnkle(skeleton,user))
                            {
                                if (isMovingForward(skeleton, user))
                                {
                                    updateSpeed(user);
                                    updateAngle(user);
                                    updatePosition(user);
                                   

                                }
                                else
                                    if (user.MovedForward)   
                                        frameNumber = -1;
                                    
                            }
                        }
                    }
                    if (frameNumber != -1)
                        frameNumber++;
                }
            }
        }
        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        ///  resolves the user's velocity into 2 components VelocityX, VelocityZ
        /// </summary>
        /// <param name="user">takes instance of class User to resolve their velocity</param>
        /// <returns>returns Vector2: holding the velocity components of the user</returns>

        public static Vector2 resolveUserVelocity(User user)
        {
            Vector2 velocity;
            velocity.X = (float)(user.Velocity * Math.Cos(user.Angle));
            velocity.Y = (float)(user.Velocity * Math.Sign(user.Angle));
            return velocity;
        }




        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        /// Checks if the user is currently moving their leg forward or not
        /// </summary>
        /// <param name="skeleton">takes instance of class User to check if they are moving their leg forward</param>
        /// <returns>returns true iff the user moved his leg forward</returns>

        public static bool isMovingForward(Skeleton skeleton,User user)
        {
            if (skeleton != null)
            {
                double currentZ = user.CurrentLeftLegPositionZ;
                double initialZ = user.InitialLeftLegPositionZ;
                if (user.RightLeg)
                {
                    currentZ = user.CurrentRightLegPositionZ;
                    initialZ = user.InitialRightLegPositionZ;
                }

                if (currentZ - initialZ < (-1*Constants3.legMovementTolerance))
                {
                    user.MovedForward = true;
                    return true;

                }
                if (!user.MovedForward)
                    if (user.RightLeg) user.InitialRightLegPositionZ = user.CurrentRightLegPositionZ;
                    else user.InitialLeftLegPositionZ = user.CurrentLeftLegPositionZ;


            }
            return false;
        }


        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
       /// <summary>
       /// Calculates the new value of the velocity and sets it the variable velocity of the user to it
      /// </summary>
      /// <param name="user">takes and instance of the class User to calculate their new velocity</param>

        public static void updateSpeed(User user)
        {
            double currentZ = user.CurrentLeftLegPositionZ;
            double initialZ = user.InitialLeftLegPositionZ;
            if (user.RightLeg)
            {
                currentZ = user.CurrentRightLegPositionZ;
                initialZ = user.InitialRightLegPositionZ;
            }
            double velocityFinal = 0;
            double deltaPosition = Math.Abs(currentZ - initialZ);
            double acceleration = getAcceleration(deltaPosition, Constants3.unitTime, user.Velocity);
            velocityFinal = getVelocity(acceleration, user.Velocity, Constants3.unitTime);
            if (user.Trying)
                user.Velocity = Math.Max(user.Velocity, velocityFinal);
            else
                user.Velocity = velocityFinal;
        }



        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
       /// <summary>
       /// updates the current and previous positions of the user's moving leg
       /// </summary>
       /// <param name="user">takes instance of the user to update their position</param>
        public static void updatePosition(User user)
        {
            if (user.RightLeg)
            {
                user.InitialRightLegPositionX = user.CurrentRightLegPositionX;
                user.InitialRightLegPositionZ = user.CurrentRightLegPositionZ;
            }
            else
            {
                user.InitialLeftLegPositionX = user.CurrentLeftLegPositionX;
                user.InitialLeftLegPositionZ = user.CurrentLeftLegPositionZ;
            }
        }

        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
    /// <summary>
    /// Calculates the value of new the angle that the user is moving their leg with and sets the variable angle to it
    /// </summary>
    /// <param name="user">takes instance of class user to calculate their angle </param>

        public static void updateAngle(User user)
        {
            if (user.storeZ2 != user.StoreZ1)
                user.Angle = (Math.Atan((user.StoreX2 - user.StoreX1) / (Math.Abs(user.StoreZ2 - user.StoreZ1))));
            else
                user.Angle = (Math.PI / 2);
        }


        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        /// stores the position where the user started moving their leg and the final position where they stopped moving their leg forward
        /// </summary>
      /// <param name="user">takes an instance of class user to store their position</param>

        public static void storePosition(User user)
        {
            
                if (user.RightLeg)
                {
                    if(user.MovedForward&&!user.HasSetInitialPositionForAngle)
                    {
                    user.StoreX1 = user.InitialRightLegPositionX;
                    user.StoreZ1 = user.InitialRightLegPositionZ;
                    user.HasSetInitialPositionForAngle = true;
                    }
                    user.StoreX2 = user.InitialRightLegPositionX;
                    user.StoreZ2 = user.InitialRightLegPositionZ;

                }
                else
                {
                     if(user.MovedForward&&!user.HasSetInitialPositionForAngle)
                    {
                    user.StoreX1 = user.InitialLeftLegPositionX;
                    user.StoreZ1 = user.InitialLeftLegPositionZ;
                    user.HasSetInitialPositionForAngle = true;
                    }
                    user.StoreX2 = user.InitialLeftLegPositionX;
                    user.StoreZ2 = user.InitialLeftLegPositionZ;
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
        /// sets the current leg positions of the user from the detected skeleton
        /// </summary>
        /// <param name="user">takes instance of class user to store their current leg positions</param>

        public static void setCurrentPosition(User user)
        {
            user.CurrentLeftLegPositionX = user.USER.Joints[JointType.AnkleLeft].Position.X;
            user.CurrentLeftLegPositionZ = user.USER.Joints[JointType.AnkleLeft].Position.Z;
            user.CurrentRightLegPositionX = user.USER.Joints[JointType.AnkleRight].Position.X;
            user.CurrentRightLegPositionZ = user.USER.Joints[JointType.AnkleRight].Position.Z;
        }

        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        /// stores the initial position of the user
        /// </summary>
        /// <param name="user">takes an instance of the class user to store their initial position</param>
        public static void initalizePlayerPosition(User user)
        {

            user.InitialLeftLegPositionZ = user.USER.Joints[JointType.AnkleLeft].Position.Z;
            user.InitialLeftLegPositionX = user.USER.Joints[JointType.AnkleLeft].Position.X;
            user.InitialRightLegPositionZ = user.USER.Joints[JointType.AnkleRight].Position.Z;
           user.InitialRightLegPositionX = user.USER.Joints[JointType.AnkleRight].Position.X;

        }


        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        /// checks if the user moved their leg from the position they were standing initially
        /// </summary>
        /// <param name="user">takes a instance of user class to check if they moved their leg</param>
        /// <returns>returns true iff the user moved their leg</returns>
        public static bool hasPlayerMovedHisAnkle(User user)
        {
            int movementState = 0;  // 0 has not moved, 1 moved one leg, 2 moved both legs
            Skeleton player = user.USER;

            if (Math.Abs(user.CurrentLeftLegPositionZ - user.InitialLeftLegPositionZ) > Constants3.legMovementTolerance)
            {
                user.RightLeg = false;
                user.TrackedJoint = player.Joints[JointType.AnkleLeft];
                movementState++;

            }

            if (Math.Abs(user.CurrentRightLegPositionZ - user.InitialRightLegPositionZ) > Constants3.legMovementTolerance)
            {
                user.RightLeg = true;
                user.TrackedJoint = player.Joints[JointType.AnkleRight];
                movementState++;
            }
            if (movementState == 1) return true;
            if (movementState == 2) // player has changed their position
            {
                movementState = 0;
                user.InitialLeftLegPositionX = player.Joints[JointType.AnkleLeft].Position.X;
                user.InitialLeftLegPositionZ = player.Joints[JointType.AnkleLeft].Position.Z;
                user.InitialRightLegPositionX = player.Joints[JointType.AnkleRight].Position.X;
                user.InitialRightLegPositionZ = player.Joints[JointType.AnkleRight].Position.Z;

            }


            return false;
        }


        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        /// sets the velocity of the leg's user relative to the assumed mass of the user's leg
        /// </summary>
        /// <param name="user">takes an instance of class user to set their velocity relative to the assumed mass of their leg</param>
        public static void setVelocityRelativeToGivenMass(User user)
        {
            user.Velocity = (float)(Constants3.normalLegMass / user.AssumedLegMass) * user.Velocity;
        }


        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        /// initializes all the variables that stores the user's movement inorder to try shooting again
        /// </summary>
        /// <param name="user">takes an instance of class user to initialze the variables that store that user's movement</param>

        public static void resetUserForShootingOrTryingAgain(User user)
        {

            user.InitialLeftLegPositionX = 0;
            user.CurrentLeftLegPositionX = 0;
            user.InitialLeftLegPositionZ = 0;
            user.CurrentLeftLegPositionZ = 0;

            user.InitialRightLegPositionX = 0;
            user.CurrentRightLegPositionX = 0;
            user.InitialRightLegPositionZ = 0;
            user.CurrentRightLegPositionZ = 0;

            user.StoreX1 = 0;
            user.StoreX2 = 0;
            user.StoreZ1 = 0;
            user.StoreZ2 = 0;

            user.Velocity = 0;
            user.Angle = 0;

            user.MovedForward = false;
            user.Trying = true;
            user.HasSetInitialPositionForAngle = false;

            frameNumber = 0;
        }
    }
}
