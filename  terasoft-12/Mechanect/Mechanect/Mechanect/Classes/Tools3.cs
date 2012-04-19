using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Mechanect.Classes
{
    class Tools3
    {
        public static int frameNumber = 0;//  call the method resetUser to try again 
        //has to be updated in the pause screen after the measuring process finishes

        // try method which resets the boolean values

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
                        storePosition(frameNumber % 2,user);
                        if (frameNumber == 0)
                        {
                            initalizePlayerPosition(skeleton,user);

                        }
                        else
                        {
                            setCurrentPosition(skeleton,user);

                            if (hasPlayerMovedHisAnkle(skeleton,user))
                            {
                                if (isMovingForward(skeleton, user))
                                {
                                    updateSpeed(user);
                                    updatePosition(user);
                                    updateAngle(user);

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="skeleton"></param>
        /// <returns></returns>

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

                if (currentZ - initialZ < 0)
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

        /// <summary>
        /// 
        /// </summary>

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




        /// <summary>
        /// updates the initial x and z positions
        /// </summary>

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

        /// <summary>
        /// updates the angle that player is moving his leg with
        /// </summary>

        public static void updateAngle(User user)
        {
            if (user.StoreZ2 < user.StoreZ1)

                user.Angle = (Math.Atan((user.StoreX1 - user.StoreX2) / (Math.Abs(user.StoreZ2 - user.StoreZ1))));
            else
                if (user.StoreZ2 > user.StoreZ1)
                    user.Angle = Math.Atan((user.StoreX2 - user.StoreX1) / (Math.Abs(user.StoreZ2 - user.StoreZ1)));
                else
                    user.Angle = Math.PI / 2;


        }


        /// <summary>
        /// stores the last two x,z positions of either the left or the right leg to be used in measuring the angle,
        /// the variables are stored alternativley in store1 and store2 depending on the value of i.
        /// </summary>
        /// <param name="i">if i is 0 the x,z positions are stored in store1
        /// if i is 1 the x,z positions are stored in store2
        /// </param>

        public static void storePosition(int i, User user)
        {
            if (i == 0)
            {
                if (user.RightLeg)
                {
                    user.StoreX1 = user.InitialRightLegPositionX;
                    user.StoreZ1 = user.InitialRightLegPositionZ;
                }
                else
                {
                    user.StoreX1 = user.InitialLeftLegPositionX;
                    user.StoreZ1 = user.InitialLeftLegPositionZ;
                }
            }
            else
            {
                if (user.RightLeg)
                {
                    user.StoreX2 = user.InitialRightLegPositionX;
                    user.StoreZ2 = user.InitialRightLegPositionZ;
                }
                else
                {
                    user.StoreX2 = user.InitialLeftLegPositionX;
                    user.StoreZ2 = user.InitialLeftLegPositionZ;
                }

            }
        }


        public static double getVelocity(double acceleration, double velocityInitial, double totalTime)
        {
            return velocityInitial + (acceleration * totalTime);
        }

        public static double getAcceleration(double totalDistance, double totalTime, double velocityInitial)
        {
            return (((totalDistance - (velocityInitial * totalTime)) * 2) / Math.Pow(totalTime, 2));
        }

        public static void setCurrentPosition(Skeleton player, User user)
        {
            user.CurrentLeftLegPositionX = player.Joints[JointType.AnkleLeft].Position.X;
            user.CurrentLeftLegPositionZ = player.Joints[JointType.AnkleLeft].Position.Z;
            user.CurrentRightLegPositionX = player.Joints[JointType.AnkleRight].Position.X;
            user.CurrentRightLegPositionZ = player.Joints[JointType.AnkleRight].Position.Z;
        }

        public static void initalizePlayerPosition(Skeleton player, User user)
        {

            user.InitialLeftLegPositionZ = player.Joints[JointType.AnkleLeft].Position.Z;
            user.InitialLeftLegPositionX = player.Joints[JointType.AnkleLeft].Position.X;
            user.InitialRightLegPositionZ = player.Joints[JointType.AnkleRight].Position.Z;
           user.InitialRightLegPositionX = player.Joints[JointType.AnkleRight].Position.X;

        }


        public static bool hasPlayerMovedHisAnkle(Skeleton player, User user)
        {
            int movementState = 0;  // 0 has not moved, 1 moved one leg, 2 moved both legs


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


        public static void setVelocityRelativeToGivenMass(float velocity,User user)
        {
            user.Velocity = (float)(Constants3.NORMAL_LEG_MASS / user.AssumedLegMass) * user.Velocity;
        }

        public static bool hasAnkleCollidedWithTheBall()
        {

            return false;
        }



        public void resetUserForShootingOrTryingAgain(User user)
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

            frameNumber = 0;
        }
    }
}
