﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// </summary>
        private int currentTime;
        private int startTime;
        private bool shooting;
        private bool beforeHip;
        private float previousAngle;
        int counter = 0;

        private float measuredVelocity;
        private float measuredAngle;
        /// <summary>
        /// Getter and setter for the MeasuredAgnle Value
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// <returns> the measured angle</returns>

        public float MeasuredAngle
        {
            get
            {
                return measuredAngle;
            }
            set
            {
                measuredAngle = value;
            }
        }

        /// <summary>
        /// Getter and setter for the MeasuredVelocity Value
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// <returns> the measured velocity</returns>


        public float MeasuredVelocity
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
            measuredAngle = 0;
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
        public void MeasureAngle(GameTime gametime)
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

                Vector2 hand = new Vector2(USER.Joints[JointType.HandLeft].Position.X - USER.Joints[JointType.ShoulderLeft].Position.X
                    , USER.Joints[JointType.HandLeft].Position.Y - USER.Joints[JointType.ShoulderLeft].Position.Y);

                float angle = (float)Math.Atan(hand.Y / hand.X);
                angle = (float)(angle * 180 / Math.PI);
                angle += 90;
                angle /= 2;

                if (angle - previousAngle > 0.5)
                {
                    previousAngle = angle;
                    return;
                }
                currentTime = (int)gametime.TotalGameTime.TotalMilliseconds - startTime;
                measuredAngle = (int)(10 * angle) / 10f;
                shooting = false;
            }

        }


        /// <summary>
        /// Calculate the angular velocity and then the linear velocity
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>Returns the linear velocity of the arm</returns>
        public void MeasureVelocity()
        {

            if (USER == null || currentTime == 0)
            {
                MeasuredVelocity = 0;
                return;
            }

            measuredVelocity = ((int)(500 * measuredAngle / currentTime)) / 10f;
        }



    }

}
