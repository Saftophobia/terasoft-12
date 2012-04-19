using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Mechanect.Common;

namespace Mechanect.Classes
{
    class User
    {

        private Skeleton user;
        public Skeleton User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }

        private MKinect kinect;
        public MKinect Kinect
        {
            get
            {
                return kinect;
            }
            set
            {
                kinect = value;
            }
        }
        private double assumedLegMass;
        public double AssumedLegMass
        {
            get
            {
                return assumedLegMass;
            }
            set
            {
                assumedLegMass = value;
            }
        }
        private Vector2 shootingPosition;
        public Vector2 ShootingPosition
        {
            get
            {
                return shootingPosition;
            }
            set
            {
                shootingPosition = value;
            }
        }

        

        public User(float assumedLegMass)
        {
            kinect = new MKinect();
            user = kinect.requestSkeleton();
            this.assumedLegMass = assumedLegMass;

        }
    }
}
