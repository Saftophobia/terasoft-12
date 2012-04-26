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

        public void setSkeleton(int id)
        {
            switch (id)
            {
                case 0: user = kinect.requestSkeleton(); break;
                case 1: user = kinect.request2ndSkeleton(); break;
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

        public User()
        {
            kinect = new MKinect();
            user = kinect.requestSkeleton();
        }
    }
}
