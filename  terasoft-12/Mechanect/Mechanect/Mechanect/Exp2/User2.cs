using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;


namespace Mechanect.Exp2
{
    /// <summary>
    /// This Class responsible for functionalities that User will do when testing either Velocity or Angle
    /// <remarks>
    /// <para>Author: Mohamed Raafat</para>
    /// </remarks>
    /// </summary>
    public class User2 : Mechanect.Common.User
    {

        /// <summary>
        /// Instance Variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>

        private int counter;
        private bool shooting;
        private bool beforeHip;
        private float measuredVelocity;
        private float measuredAngle;
        private List<float> angleList = new List<float>();
        private float startTime;
        /// <summary>
        /// Getters for the Vectors
        /// </summary>
        #region Vector getters
        private Vector3 CenterHip
        {
            get
            {
                return new Vector3(USER.Joints[JointType.HipCenter].Position.X,
                    USER.Joints[JointType.HipCenter].Position.Y, USER.Joints[JointType.HipCenter].Position.Z);
            }
        }
        private Vector3 LeftShoulder
        {
            get
            {
                return new Vector3(USER.Joints[JointType.ShoulderLeft].Position.X,
                    USER.Joints[JointType.ShoulderLeft].Position.Y, USER.Joints[JointType.ShoulderLeft].Position.Z);
            }
        }
        private Vector3 RightShoulder
        {
            get
            {
                return new Vector3(USER.Joints[JointType.ShoulderRight].Position.X,
                    USER.Joints[JointType.ShoulderRight].Position.Y, USER.Joints[JointType.ShoulderRight].Position.Z);
            }
        }
        private Vector3 LeftHand
        {
            get
            {
                return new Vector3(USER.Joints[JointType.HandLeft].Position.X,
                    USER.Joints[JointType.HandLeft].Position.Y, USER.Joints[JointType.HandLeft].Position.Z);
            }
        }
        #endregion

        /// <summary>
        /// Setter and Getter for Instance variable "measuredFinalAngle"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>Float, The value of the measuredFinalAngle</returns>

        public float MeasuredAngle
        {
            get
            {
                return measuredAngle;
            }
        }

        /// <summary>
        /// Setter and Getter for Instance variable "measuredVelocity"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>Double, The value of the measuredVelocity</returns>
        /// 
        public float MeasuredVelocity
        {
            get
            {
                return measuredVelocity;
            }
        }


        /// <summary>
        /// Constructor for User2 class, that sets the value of the counter and listCounter
        /// </summary>
        /// <remarks>AUTHOR: Mohamed Raafat</remarks>
        public User2()
        {
            Reset();
        }



        /// <summary>
        /// Resets all instance variables to their intitial values
        /// </summary>
        /// <remarks>AUTHOR: Mohamed Raafat</remarks>

        public void Reset()
        {
            shooting = false;
            beforeHip = false;
            angleList = new List<float>();
            measuredAngle = 0;
            measuredVelocity = 0;
        }
        /// <summary>
        /// Helper method to calculate current angle being meausred
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="leftShoulder">Left shoulder vector</param>
        /// <param name="rightShoulder">Right Shoulder vector</param>
        /// <param name="centerHip">Center Hip Vector</param>
        /// <param name="leftHand">Left hand vector</param>
        /// <returns>float, current calculated angle</returns>

        private float CurrentAngle(Vector3 leftShoulder, Vector3 rightShoulder, Vector3 centerHip, Vector3 leftHand)
        {
            Vector3 centerHipToLeftShoulder = leftShoulder - centerHip;
            Vector3 centerHipToRightShoulder = rightShoulder - centerHip;
            Vector3 leftHandToLeftShoulder = leftShoulder - leftHand;
            Vector3 leftHandToRightShoulder = rightShoulder - leftHand;
            Vector3 normalToHipPlane = Vector3.Cross(centerHipToLeftShoulder, centerHipToRightShoulder);
            Vector3 normalToHandPlane = Vector3.Cross(leftHandToLeftShoulder, leftHandToRightShoulder);
            float angle = (float)Math.Acos(Vector3.Dot(normalToHandPlane, normalToHipPlane)
                / (normalToHipPlane.Length() * normalToHandPlane.Length()));

            if (Vector3.Cross(normalToHipPlane, normalToHandPlane).X < 0)
            {
                angle *= -1;
            }
            angle = MathHelper.ToDegrees(angle) / 2;
            return angle;
        }

        /// <summary>
        /// Helper method to calculate the measured final velocity
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="list">list containing set of measured angles</param>
        /// <param name="gameTime">An instance of the game time</param>
        /// <returns>float: meausred velocity</returns>


        private float MeasureVelocity(List<float> list, GameTime gameTime)
        {
            if (list.Count <= 3)
                return 0;
            return (float)((166) * (list[list.Count - 3] / (gameTime.TotalGameTime.TotalMilliseconds - startTime)));
        }

        /// <summary>
        /// Helper method to detect when will the hand stop and the user stopped shooting
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="angleList">List containing set of calculated angles</param>
        /// <returns>bool, True if hand has stopped, false otherwise</returns>
        private bool HandStopped(List<float> angleList)
        {
            if (angleList.Count <= 3)
                return false;
            else
                return angleList[angleList.Count - 1] - angleList[angleList.Count - 3] < 0.5;
        }


        /// <summary>
        /// Main method to that sets the final angle and velocity being measured
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name ="gametime">Takes the gametime to make time calculations </param>
        public void MeasureVelocityAndAngle(GameTime gametime)
        {
            if (++counter % 3 == 0)
            {
                counter = 0;
                if (USER == null || USER.Position.Z == 0)
                {
                    return;
                }

                float currentAngle = CurrentAngle(LeftShoulder, RightShoulder, CenterHip, LeftHand);

                if (currentAngle < 0)
                {
                    shooting = false;
                    beforeHip = true;
                    angleList.Clear();
                    return;
                }

                if (!shooting && beforeHip && currentAngle > 0)
                {

                    shooting = true;
                    beforeHip = false;
                    startTime = (float)gametime.TotalGameTime.TotalMilliseconds;
                    angleList.Add(currentAngle);
                    return;
                }

                if (shooting)
                {
                    angleList.Add(currentAngle);


                    if (HandStopped(angleList))
                    {
                        if (currentAngle > 10)
                        {
                            measuredAngle = currentAngle;
                            measuredVelocity = MeasureVelocity(angleList, gametime);
                        }
                        shooting = false;
                        beforeHip = false;
                        angleList.Clear();
                    }
                }
            }
        }
    }
}