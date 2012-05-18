﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Mechanect.Common;
using Mechanect.Classes;
using MKinect;

namespace Mechanect.Experiment2
{
    /// <summary>
    /// This Class responsible for functionalities that User will do when testing either Velocity or Angle
    /// <remarks>
    /// <para>Author: Mohamed Raafat</para>
    /// </remarks>
    /// </summary>
    class User2 : Mechanect.Common.User
    {

        /// <summary>
        /// Instance Variables
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// </summary>
        private int currentTime;
        private int startTime;
        private bool shooting;
        private bool beforeHip;
        private double previousAngle;
        int counter = 0;
        int listCounter = 0;
        private double angleBeingMeasured;
        public double AngleBeingMeasured
        {
            get
            {
                return angleBeingMeasured;
            }
            set
            {
                angleBeingMeasured = value;
            }
        }
        List<Vector2> angleAndTime = new List<Vector2>;
        private double measuredVelocity;
        private double measuredFinalAngle;
        /// <summary>
        /// Getter and setter for the MeasuredAgnle Value
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// <returns> the measured angle</returns>

        public double MeasuredAngle
        {
            get
            {
                return measuredFinalAngle;
            }
            set
            {
                measuredFinalAngle = value;
            }
        }

        /// <summary>
        /// Getter and setter for the MeasuredVelocity Value
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// <returns> the measured velocity</returns>


        public double MeasuredVelocity
        {
            get
            {
                return measuredVelocity;
            }
            set
            {
                measuredVelocity = value;
            }
        }

        public User2()
        {
          
        }

        
        
        /// <summary>
        /// Getter and setter for the MeasuredAgnle Value
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>

        public void MeasureVelocityAndAngle(GameTime gameTime)
        {
            MeasureAngle(gameTime);
            MeasureVelocity();
        }

        /// <summary>
        /// Resets all instance variables to their intitial values
        /// </summary>

        public void Reset()
        {
            shooting = false;
            beforeHip = false;
            measuredFinalAngle = 0;
            previousAngle = 0;
        }






        /// <summary>
        /// Gets the angle between two Vectors, from left hip to left shoulder and from left shoulder to left hand
        /// and convert it to degrees.
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// <param name ="gametime">Takes the gametime to make time calculations </param>
        private void MeasureAngle(GameTime gametime)
        {
            if (USER == null)
            {
                return;
            }
            if (!shooting && !beforeHip)
            {
                beforeHip = USER.Joints[JointType.HandLeft].Position.X < USER.Joints[JointType.HipCenter].Position.X;
                return;
            }
            if (!shooting && beforeHip && USER.Joints[JointType.HandLeft].Position.X > USER.Joints[JointType.HipCenter].Position.X)
            {
                beforeHip = false;
                shooting = true;
                startTime = (int)gametime.TotalGameTime.TotalMilliseconds;
                previousAngle = 0;
                return;
            }
            if (shooting)
            {
                counter++;
                if (counter % 3 != 0)
                    return;
                counter = 0;

                Vector3 centerHipToLeftShoulder = new Vector3(USER.Joints[JointType.ShoulderLeft].Position.X - USER.Joints[JointType.HipCenter].Position.X,
                    USER.Joints[JointType.ShoulderLeft].Position.Y - USER.Joints[JointType.HipCenter].Position.Y,
                    USER.Joints[JointType.ShoulderLeft].Position.Z - USER.Joints[JointType.HipCenter].Position.Z);

                Vector3 centerHiptoRightShoulder = new Vector3(USER.Joints[JointType.ShoulderRight].Position.X - USER.Joints[JointType.HipCenter].Position.X,
                    USER.Joints[JointType.ShoulderRight].Position.Y - USER.Joints[JointType.HipCenter].Position.Y,
                    USER.Joints[JointType.ShoulderRight].Position.Z - USER.Joints[JointType.HipCenter].Position.Z);

                Vector3 leftShoulderToRightShoulder = new Vector3(USER.Joints[JointType.ShoulderLeft].Position.X - USER.Joints[JointType.ShoulderRight].Position.X,
                    USER.Joints[JointType.ShoulderLeft].Position.Y - USER.Joints[JointType.ShoulderRight].Position.Y,
                    USER.Joints[JointType.ShoulderLeft].Position.Z - USER.Joints[JointType.ShoulderRight].Position.Z);

                Vector3 leftShoulderToLeftHand = new Vector3(USER.Joints[JointType.ShoulderLeft].Position.X - USER.Joints[JointType.ShoulderRight].Position.X,
                    USER.Joints[JointType.ShoulderLeft].Position.Y - USER.Joints[JointType.ShoulderRight].Position.Y,
                    USER.Joints[JointType.ShoulderLeft].Position.Z - USER.Joints[JointType.ShoulderRight].Position.Z);

                Vector3 normalToShouldersHipPlane = Vector3.Cross(centerHipToLeftShoulder, centerHiptoRightShoulder);

                Vector3 normalToShoulderHandPlane = Vector3.Cross(leftShoulderToLeftHand, leftShoulderToRightShoulder);

                double angle = (double)Math.Acos(Vector3.Dot(normalToShoulderHandPlane, normalToShouldersHipPlane));
                angle = angle * 180 / Math.PI;
                angle += 90; angle /= 2;
                if (angle - previousAngle > 0.5)
                {
                    previousAngle = angle;
                    return;
                }
                currentTime = (int)gametime.TotalGameTime.TotalMilliseconds - startTime;

                measuredFinalAngle = (int)(10 * angle) / 10f;
                shooting = false;

                if (shooting == true)
                {
                    if (listCounter == 5)
                        listCounter = 0;
                    if (angleAndTime.Count >= 0 && angleAndTime.Count < angleAndTime.Capacity)
                    {
                        angleAndTime.Add(new Vector2((float)currentTime, (float)angleBeingMeasured));
                    }
                    else
                    {
                        if (angleAndTime.Count == angleAndTime.Capacity)
                        {
                            angleAndTime.RemoveAt(this.listCounter);
                            angleAndTime.Insert(this.listCounter, new Vector2((float)currentTime, (float)angleBeingMeasured));
                        }
                    }
                    listCounter++;

                }



            }

        }


        /// <summary>
        /// Calculate the angular velocity and then the linear velocity
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>Returns the linear velocity of the arm</returns>
        private void MeasureVelocity()
        {

            if (USER == null || currentTime == 0)
            {
                MeasuredVelocity = 0;
                return;
            }

            measuredVelocity = ((int)(500 * measuredFinalAngle / currentTime)) / 10f;
        }



    }

}