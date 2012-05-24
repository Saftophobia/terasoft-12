using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Mechanect.Common;
using Mechanect.Classes;


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
        private List<Vector2> angleAndTime = new List<Vector2>();


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


        /// <summary>
        /// Setter and Getter for Instance variable "measuredFinalAngle"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>float, The value of the measuredFinalAngle</returns>

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
        /// <returns>double, The value of the measuredVelocity</returns>
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
            angleAndTime = new List<Vector2>();
            measuredAngle = 0;
            measuredVelocity = 0;
        }
        /// <summary>
        /// Helper method to calculate current angle being meausred
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="leftShoulder"></param>
        /// <param name="rightShoulder"></param>
        /// <param name="centerHip"></param>
        /// <param name="leftHand"></param>
        /// <returns></returns>

        private float CurrentAngle(Vector3 leftShoulder, Vector3 rightShoulder, Vector3 centerHip, Vector3 leftHand)
        {
            Vector3 centerHipToLeftShoulder = leftShoulder - centerHip;
            Vector3 centerHipToRightShoulder = rightShoulder - centerHip;
            Vector3 leftHandToLeftShoulder = leftHand - leftShoulder;
            Vector3 leftHandToRightShoulder = leftHand - rightShoulder;
            Vector3 normalToHipPlane = Vector3.Cross(centerHipToLeftShoulder, centerHipToRightShoulder);
            Vector3 normalToHandPlane = Vector3.Cross(leftHandToLeftShoulder, leftHandToRightShoulder);
            float angle = (float)Math.Acos(Vector3.Dot(normalToHandPlane, normalToHipPlane)
                / (normalToHipPlane.Length() * normalToHandPlane.Length()));

            if (Vector3.Cross(normalToHipPlane, normalToHandPlane).X < 0)
            {
                angle *= -1;
            }
            return angle;
        }
        /// <summary>
        /// Helper method to calculate the measured final velocity
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="list"></param>
        /// <returns>float: meausred velocity</returns>


        private float MeasureVelocity(List<Vector2> list)
        {
            this.measuredVelocity = (list[list.Count-2].X - list[0].X) /
                (list[list.Count].Y - list[0].Y);

            return (float)measuredVelocity;
        }
        /// <summary>
        /// Helper method to update the velocity being measured according to the angle being measured
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="gametime"></param>
        /// <param name="angleTimeList"></param>
        /// <param name="angle"></param>
        private void UpdateVelocity(GameTime gametime, List<Vector2> angleTimeList, float angle)
        {
            if (angleTimeList.Count == 5)
                angleTimeList.RemoveAt(0);
            angleTimeList.Add(new Vector2((float)angle, (float)gametime.TotalGameTime.TotalSeconds));
        }
        /// <summary>
        /// Helper method to detect when will the hand stop and the user stopped shooting
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="angleTimeList"></param>
        /// <returns></returns>
        private bool HandStopped(List<Vector2> angleTimeList)
        {
            return angleTimeList[angleTimeList.Count - 1].X - angleTimeList[angleTimeList.Count - 2].X < 0.5;
        }


        /// <summary>
        /// Main method to that sets the final angle and velocity being measured
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name ="gametime">Takes the gametime to make time calculations </param>
        public void MeasureAngleAndVelocity(GameTime gametime)
        {
            if (++counter % 3 == 0)
            {
                counter = 0;
                if (USER == null || USER.Position.Z == 0)
                {
                    return;
                }

                float currentAngle = CurrentAngle(LeftShoulder, RightShoulder, CenterHip, LeftHand);
                if (!shooting)
                {
                    shooting = beforeHip && currentAngle > 0;
                    beforeHip = currentAngle < 0;
                    return;
                }

                UpdateVelocity(gametime, angleAndTime, currentAngle);
                if (HandStopped(angleAndTime))
                {
                    measuredAngle = currentAngle;
                    measuredVelocity = MeasureVelocity(angleAndTime);
                    shooting = false;
                    beforeHip = false;
                    angleAndTime = new List<Vector2>();
                }
            }
        }
    }
}