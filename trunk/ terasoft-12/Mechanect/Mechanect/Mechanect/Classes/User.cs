using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Mechanect.Common;

namespace Mechanect.Classes
{
    public class User
    {

        private Skeleton user;
        public Skeleton USER
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

        public void setSkeleton()
        {
            user = kinect.requestSkeleton();
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

        public User()
        {
            kinect = new MKinect();
            user = kinect.requestSkeleton();
        }
    }
}
