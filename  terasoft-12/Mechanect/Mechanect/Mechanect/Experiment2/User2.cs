using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    /// <summary>
    /// This Class responsible for functionalities that User will do when testing either Velocity or Angle
    /// <remarks>
    /// <para>Author: Mohamed Raafat</para>
    /// </remarks>
    /// </summary>
    class User2 : Mechanect.Classes.User
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

        private double measuredVelocity;
        private double measuredAngle;
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

                Vector2 hand = new Vector2(USER.Joints[JointType.HandLeft].Position.X - USER.Joints[JointType.ShoulderLeft].Position.X
                    , USER.Joints[JointType.HandLeft].Position.Y - USER.Joints[JointType.ShoulderLeft].Position.Y);

                double angle = (float)Math.Atan(hand.Y / hand.X);
                angle = angle * 180 / Math.PI;
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
        private void MeasureVelocity()
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
