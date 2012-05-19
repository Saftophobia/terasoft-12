using System;
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
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>

        private int currentTime;
        private int startTime;
        private int counter;
        private int listCounter;
        private bool shooting;
        private bool beforeHip;
        private double previousAngle;
        private double angleBeingMeasured;
        private double measuredVelocity;
        private double measuredFinalAngle;
        private List<Vector2> angleAndTime = new List<Vector2>();

        /// <summary>
        /// Setter and Getter for Instance variable "angleBeingMeasured"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>double, The value of the angleBeingMeasured</returns>

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

        /// <summary>
        /// Setter and Getter for Instance variable "measuredFinalAngle"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>double, The value of the measuredFinalAngle</returns>

        public double MeasuredFinalAngle
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
        /// Setter and Getter for Instance variable "measuredVelocity"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>double, The value of the measuredVelocity</returns>
        /// 
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
        /// Setter and Getter for Instance variable "previousAngle"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>double, The value of the previousAngle</returns>
        /// 
        public double PreviousAngle
        {
            get
            {
                return previousAngle;
            }
            set
            {
                previousAngle = value;
            }
        }

        /// <summary>
        /// Setter and Getter for Instance variable "shooting"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>bool, The value of the shooting</returns>

        public bool Shooting
        {
            get
            {
                return shooting;
            }
            set
            {
                shooting = value;
            }

        }

        /// <summary>
        /// Setter and Getter for Instance variable "beforeHip"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>bool, The value of the beforeHip</returns>

        public bool BeforeHip
        {
            get
            {
                return beforeHip;
            }
            set
            {
                beforeHip = value;
            }
        }

        /// <summary>
        /// Setter and Getter for Instance variable "currentTime"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>int, The value of the currentTime</returns>

        public int CurrentTime
        {
            get
            {
                return currentTime;
            }
            set
            {
                currentTime = value;
            }
        }

        /// <summary>
        /// Setter and Getter for Instance variable "startTime"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>int, The value of the startTime</returns>

        public int StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
            }
        }

        /// <summary>
        /// Setter and Getter for Instance variable "counter"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>int, The value of the counter</returns>

        public int Counter
        {
            get
            {
                return counter;
            }
            set
            {
                counter = value;
            }
        }

        /// <summary>
        /// Setter and Getter for Instance variable "listCounter"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns> int,The value of the listCounter</returns>

        public int ListCounter
        {
            get
            {
                return listCounter;
            }
            set
            {
                listCounter = value;
            }
        }
        /// <summary>
        /// Setter and Getter for Instance variable "angleAndTime"
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>List'Vector2',The value of the angleAndTime</returns>

        public List<Vector2> AngleTime
        {
            get
            {
                return angleAndTime;
            }
            set
            {
                angleAndTime = value;
            }
        }

        /// <summary>
        /// Constructor for User2 class, that sets the value of the counter and listCounter
        /// </summary>
        /// <remarks>AUTHOR: Mohamed Raafat</remarks>
        public User2()
        {
            this.counter = 0;
            this.listCounter = 0;

        }


        /* 
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
 */
        /// <summary>
        /// Resets all instance variables to their intitial values
        /// </summary>
        /// <remarks>AUTHOR: Mohamed Raafat</remarks>

        public void Reset()
        {
            shooting = false;
            beforeHip = false;
            measuredFinalAngle = 0;
            previousAngle = 0;
        }






        /// <summary>
        /// Calculate the angle between two planes, one is a vertical plane containing: left shoulder, right shoulder,
        /// and center hip, and another plane that is inclined containing: left shoulder, right shoulder, and left hand
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
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

                if (shooting)
                {
                    if (listCounter == 5)
                        listCounter = 0;
                    if (angleAndTime.Count >= 0 && angleAndTime.Count < angleAndTime.Capacity)
                    {
                        angleAndTime.Add(new Vector2((float)angleBeingMeasured, (float)currentTime));
                    }
                    else
                    {
                        if (angleAndTime.Count == angleAndTime.Capacity)
                        {
                            angleAndTime.RemoveAt(this.listCounter);
                            angleAndTime.Insert(this.listCounter, new Vector2((float)angleBeingMeasured, (float)currentTime));
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
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <returns>Returns the linear velocity of the arm</returns>
        private void MeasureVelocity()
        {

            if (USER == null || currentTime == 0)
            {
                MeasuredVelocity = 0;
                return;
            }

            measuredVelocity = (this.angleAndTime[this.listCounter].X - this.angleAndTime[0].X) /
                (this.angleAndTime[this.listCounter].Y - this.angleAndTime[0].Y);

        }



    }

}