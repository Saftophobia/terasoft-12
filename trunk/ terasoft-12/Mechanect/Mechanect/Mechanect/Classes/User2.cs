using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Mechanect.Classes
{
    class User2
    {

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



        internal void MeasureVelocityAndAngle()
        {
            //throw new NotImplementedException();
        }
    }
}
