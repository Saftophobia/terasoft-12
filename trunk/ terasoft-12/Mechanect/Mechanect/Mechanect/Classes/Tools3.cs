using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Classes
{
    class Tools3
    {
        public static int frameNumber = 0;

        /// <summary>
        /// calculates the friction vector that will cause the velocity to stop without changing the velocity direction
        /// </summary>
        /// <param name="velocity">the velocity vector</param>
        /// <param name="frictionValue">the value of the friction</param>
        /// <remarks>Auther : Bishoy Bassem</remarks>
        public static Vector3 calculateFriction(Vector3 velocity, float frictionValue)
        {
            float sx = -MathHelper.Clamp(velocity.X, -1, 1);
            float sy = -MathHelper.Clamp(velocity.Y, -1, 1);
            float sz = -MathHelper.Clamp(velocity.Z, -1, 1);
            bool xmax = Math.Abs(velocity.X) > Math.Abs(velocity.Y) && Math.Abs(velocity.X) > Math.Abs(velocity.Z);
            bool ymax = Math.Abs(velocity.Y) > Math.Abs(velocity.X) && Math.Abs(velocity.Y) > Math.Abs(velocity.Z);
            if (xmax)
            {
                return new Vector3(sx, sy * velocity.Y / velocity.X, sz * velocity.Z / velocity.X) * frictionValue;
            }
            if (ymax)
            {
                return new Vector3(sx * velocity.X / velocity.Y, sy, sz * velocity.Z / velocity.Y) * frictionValue;
            }
            return new Vector3(sx * velocity.X / velocity.Z, sy * velocity.Y / velocity.Z, sz) * frictionValue;
            
        }
        
        
        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>>
        /// <summary>
        ///  Updates the velocity, angle variables of the User3 after each captured skeleton frame.
        /// </summary>
        /// <param name="User3">takes instance of class User3 as input, to update their velocity and angle </param>

        public static void update_MeasuringVelocityAndAngle(User3 user)
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

                            if (hasPlayerMovedHisAnkle(user))
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

        public static bool isMovingForward(Skeleton skeleton,User3 User3)
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

                if (currentZ - initialZ < (-1*Constants3.legMovementTolerance))
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
                    if(User3.MovedForward&&!User3.HasSetInitialPositionForAngle)
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
                     if(User3.MovedForward&&!User3.HasSetInitialPositionForAngle)
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
        /// <param name="User3">takes an instance of class User3 to scale their velocity relative to the assumed mass
        /// of their leg and maps the velocity in meters to pixels</param>
        /// <returns>returns the scales velocity</returns>
        public static float setVelocityRelativeToGivenMass(User3 User3)
        {
            return  (float)((Constants3.normalLegMass / User3.AssumedLegMass) * User3.Velocity);
        }


        ///<remarks>
        ///<para>
        ///Author: Cena
        ///</para>
        ///</remarks>
        /// <summary>
        /// initializes all the variables that stores the User3's movement inorder to try shooting again
        /// </summary>
        /// <param name="User3">takes an instance of class User3 to initialze the variables that store that User3's movement</param>

        public static void resetUserForShootingOrTryingAgain(User3 User3)
        {

            User3.InitialLeftLegPositionX = 0;
            User3.CurrentLeftLegPositionX = 0;
            User3.InitialLeftLegPositionZ = 0;
            User3.CurrentLeftLegPositionZ = 0;

            User3.InitialRightLegPositionX = 0;
            User3.CurrentRightLegPositionX = 0;
            User3.InitialRightLegPositionZ = 0;
            User3.CurrentRightLegPositionZ = 0;

            User3.StoreX1 = 0;
            User3.StoreX2 = 0;
            User3.StoreZ1 = 0;
            User3.StoreZ2 = 0;

            User3.Velocity = 0;
            User3.Angle = 0;

            User3.MovedForward = false;
            User3.Trying = true;
            User3.HasSetInitialPositionForAngle = false;

            frameNumber = 0;
        }



        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to get the customized button with OKButton
        /// </summary>
        /// <param name="c">content managaer to load pictures</param>
        /// <param name="p">position of the button</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="u">instance of user<param>
        /// <returns>returns OK button</returns>
        public static Button OKButton(ContentManager c, Vector2 p, int sw, int sh, User u)
        {
            return new Button(c.Load<GifAnimation.GifAnimation>("Textures/Buttons/ok-s"),
           c.Load<GifAnimation.GifAnimation>("Textures/Buttons/ok-m"), p, sw, sh,
           c.Load<Texture2D>("Textures/Buttons/hand"), u);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to get the customized button with MainMenu
        /// </summary>
        /// <param name="c">content managaer to load pictures</param>
        /// <param name="p">position of the button</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="u">instance of user<param>
        /// <returns>returns MainMenu button</returns>
        public static Button MainMenuButton(ContentManager c, Vector2 p, int sw, int sh, User u)
        {
            return new Button(c.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-s"),
           c.Load<GifAnimation.GifAnimation>("Textures/Buttons/menu-m"), p, sw, sh,
           c.Load<Texture2D>("Textures/Buttons/hand"), u);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to get the customized button with NewGame
        /// </summary>
        /// <param name="c">content managaer to load pictures</param>
        /// <param name="p">position of the button</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="u">instance of user<param>
        /// <returns>returns NewGame button</returns>
        public static Button NewGameButton(ContentManager c, Vector2 p, int sw, int sh, User u)
        {
            return new Button(c.Load<GifAnimation.GifAnimation>("Textures/Buttons/newgame-s"),
           c.Load<GifAnimation.GifAnimation>("Textures/Buttons/newgame-m"), p, sw, sh,
           c.Load<Texture2D>("Textures/Buttons/hand"), u);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Used to display the hand on the screen. It should be called inside a Draw() method
        /// </summary>
        /// <param name="c">content managaer to load pictures</param>
        /// <param name="p">position of the button</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="user">instance of user<param>
        /// <param name="spriteBatch">used to draw the picture</param>
        public static void DisplayHand(ContentManager c, int sw, int sh, SpriteBatch spriteBatch, User user)
        {
            if (user.USER != null)
            {
                Texture2D pic = c.Load<Texture2D>("Textures/Buttons/hand");
                Vector2 handPosition = new Vector2(user.Kinect.GetJointPoint(user.USER.Joints[JointType.HandRight], sw, sh).X,
                    user.Kinect.GetJointPoint(user.USER.Joints[JointType.HandRight], sw, sh).Y);
                spriteBatch.Begin();
                spriteBatch.Draw(pic, handPosition, Color.White);
                spriteBatch.End();
            }
        }
    }
}
