using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        private Kinect kinect;
        public Kinect Kinect
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
            kinect = new Kinect();
            user = kinect.requestSkeleton();
            this.assumedLegMass = assumedLegMass;

        }
    }
}
