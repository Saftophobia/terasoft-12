using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    class User2 : Mechanect.Classes.User
    {

        private Vector3 finalHandPosition;
        public Vector3 FinalHandPosition
        {
            get
            {
                return finalHandPosition;
            }
            set
            {
                finalHandPosition = value;
            }
        }

        private double finalAngle;
        private double FinalAngle
        {
            get
            {
                return finalAngle;
            }
            set
            {
                finalAngle = value;
            }
        }

        private double startPosition;
        public double StartPosition
        {
            get
            {
                return startPosition;
            }
            set
            {
                startPosition = USER.Joints[JointType.HandLeft].Position.Y;
            }
        }

        private TimeSpan currentTime;
        public TimeSpan CurrentTime
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

        private TimeSpan startTime;
        public TimeSpan StartTime
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
        private Skeleton skeleton;
        public Skeleton Skeleton
        {
            get
            {
                return skeleton;
            }
            set
            {
                skeleton = value;
            }
        }


        private double measuredAngle;
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

        private double measuredVelocity;
        public double MeasuredVelocity
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
        private double initialLeftHand;
        public double InitialLeftHand
        {
            get
            {
                return initialLeftHand;
            }
            set
            {
                initialLeftHand = value;
            }
        }
        // the constructor should initialize the initial left hand position to Zero

        internal void MeasureVelocityAndAngle()
        {
            //throw new NotImplementedException();
        }

       


        /// <summary>
        /// Gets the angle between two Vectors, from left hip to left shoulder and from left shoulder to left hand
        /// and convert it to degrees.
        /// <Author>Mohamed Raafat </Author>
        /// </summary>
        /// <remarks>The counter variable ensure to unify difference in Fps of XNA and Kinect</remarks>
        /// <returns> The angle in degrees</returns>
        int counter = 0;
        public double measureAngle(GameTime gametime)
        {

            if (hasStartedMoving(gametime))
            {
                counter += 1;

                if (counter % 3 == 0)
                {
                    counter = 0;
                    Vector3 leftHip = new Vector3(USER.Joints[JointType.HipLeft].Position.X
                        , USER.Joints[JointType.HipLeft].Position.Y
                        , USER.Joints[JointType.HipLeft].Position.Z);

                    Vector3 leftShoulder = new Vector3(USER.Joints[JointType.ShoulderLeft].Position.X
                        , USER.Joints[JointType.ShoulderLeft].Position.Y
                        , USER.Joints[JointType.ShoulderLeft].Position.Z);

                    Vector3 leftHand = new Vector3(USER.Joints[JointType.HandLeft].Position.X
                        , USER.Joints[JointType.HandLeft].Position.Y
                        , USER.Joints[JointType.HandLeft].Position.Z);

                    //sets the final coordinates of the left hand
                    finalHandPosition.X = leftHand.X;
                    finalHandPosition.Y = leftHand.Y;
                    finalHandPosition.Z = leftHand.Z;

                    Vector3 HipToShoulder = new Vector3(leftShoulder.X - leftHip.X, leftShoulder.Y - leftHip.Y, leftShoulder.Z - leftHip.Z);

                    Vector3 ShoulderToHand = new Vector3(leftHand.X - leftShoulder.X, leftHand.Y - leftShoulder.Y, leftHand.Z - leftShoulder.Z);

                    HipToShoulder.Normalize();
                    ShoulderToHand.Normalize();

                    //return value the angle, direction from the left hip to the hand.
                    double angle = (float)Math.Acos(Vector3.Dot(HipToShoulder, ShoulderToHand));
                    double radianToDegree = (double)angle * 180 / Math.PI;

                    this.MeasuredAngle = radianToDegree;

                    if (MeasuredAngle == finalAngle)
                        currentTime = gametime.TotalGameTime - startTime;

                    finalAngle = radianToDegree;



                    return radianToDegree;
                }
                else
                    return finalAngle;
            }
            else
                return finalAngle;
        }



        /// <summary>
        /// Calculate the angular velocity and then the linear velocity
        /// </summary>
        /// <Author>Mohamed Raafat</Author>
        /// <returns>Returns the linear velocity of the arm</returns>
        public void measureVelocity()
        {

            double angularVelocity = this.finalAngle / currentTime.Seconds;
            double lengthHipToHand = Math.Sqrt((Math.Pow((finalHandPosition.X - USER.Joints[JointType.HipLeft].Position.X), 2))
                + Math.Pow((finalHandPosition.Y - USER.Joints[JointType.HipLeft].Position.Y), 2));
            measuredVelocity = angularVelocity * lengthHipToHand;
        
        }

    }
}
