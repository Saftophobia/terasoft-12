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
       /// <returns> the angle in degrees</returns>
        int counter = 0;
        public double calulateAngle()
        {

            counter += 1 ;
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

                Vector3 HipToShoulder = new Vector3(leftShoulder.X - leftHip.X, leftShoulder.Y - leftHip.Y, leftShoulder.Z - leftHip.Z);

                Vector3 ShoulderToHand = new Vector3(leftHand.X - leftShoulder.X, leftHand.Y - leftShoulder.Y, leftHand.Z - leftShoulder.Z);

                HipToShoulder.Normalize();
                ShoulderToHand.Normalize();

                //return value the angle, direction from the left hip to the hand.
                double angle = (float)Math.Acos(Vector3.Dot(HipToShoulder, ShoulderToHand));
                double radianToDegree = (double)angle * 180 / Math.PI;

                return radianToDegree;
            }
            else
                return 0;
        }
    }
}
