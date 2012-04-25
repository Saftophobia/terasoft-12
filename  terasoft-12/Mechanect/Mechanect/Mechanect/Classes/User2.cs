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

        private double currentAngle;
        private double CurrentAngle
        {
            get
            {
                return currentAngle;
            }
            set
            {
                currentAngle = value;
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
                startPosition = USER.Joints[JointType.HandLeft].Position.Z;
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

        private bool shooting;
        public bool Shooting
        {
            get
            {
                return shooting;
            }
            set
            {
                shooting = false;
            }
        }
        private bool beforeHip;
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

        private double previousAngle;
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

        private double measuredVelocity;
        public double MeasuredVelocity
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
       

        internal void MeasureVelocityAndAngle()
        {
            //throw new NotImplementedException();
        }

     

        /// <summary>
        /// Gets the angle between two Vectors, from left hip to left shoulder and from left shoulder to left hand
        /// and convert it to degrees.
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed Raafat</para>
        /// </remarks>
        /// <param name ="gametime">Takes the gametime to make time calculations </param>
        int counter = 0;
        public void measureAngle(GameTime gametime)
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

                AngleBeingMeasured = radianToDegree;

                // to be able to set the beforeHip instance, i need to check if he was standing before with his hand
                // in the right position or not.
                if (USER.Joints[JointType.HandLeft].Position.X < USER.Joints[JointType.HipLeft].Position.X)
                    beforeHip = true;


                if (beforeHip)
                {
                    if (USER.Joints[JointType.HandLeft].Position.X > USER.Joints[JointType.HipLeft].Position.X)
                    {
                        shooting = true;
                        beforeHip = false;
                        startTime = gametime.TotalGameTime - gametime.ElapsedGameTime;
                    }
                    else
                    currentAngle = 0;
                }
                else
                {
                    if (shooting)
                    {
                        currentAngle = radianToDegree;
                    }

                }
                if ((previousAngle != 0 && currentAngle - previousAngle > 3) || previousAngle == 0)
                {
                    previousAngle = radianToDegree;
                    currentTime = gametime.TotalGameTime - startTime;
                }
                else
                {
                    if (previousAngle != 0 && currentAngle - previousAngle <= 3)
                    {
                        currentTime = gametime.TotalGameTime - startTime;
                        currentAngle = radianToDegree;
                        shooting = false;
                    }
                }

              

            }

        }

        /// <summary>
        /// Calculate the angular velocity and then the linear velocity
        /// </summary>
        /// <para>Author: Mohamed Raafat</para>
        /// <returns>Returns the linear velocity of the arm</returns>
        public void measureVelocity()
        {

            double angularVelocity = this.previousAngle / currentTime.Seconds;
            double lengthHipToHand = Math.Sqrt((Math.Pow((finalHandPosition.X - USER.Joints[JointType.HipLeft].Position.X), 2))
                + Math.Pow((finalHandPosition.Y - USER.Joints[JointType.HipLeft].Position.Y), 2));
            measuredVelocity = angularVelocity * lengthHipToHand;
        
        }


        
    }
}
