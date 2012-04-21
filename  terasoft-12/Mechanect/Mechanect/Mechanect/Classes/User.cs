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

        private Joint trackedJoint;
        public Joint TrackedJoint  //  the user's tracked joint
        {
            get
            {
                return trackedJoint;
            }
            set
            {
                trackedJoint = value;
            }
        }
         
        private bool rightLeg;  // is the user using his right leg or left
        public bool RightLeg
        {
            get
            {
                return rightLeg;
            }
            set
            {
                rightLeg = value;
            }
        }

        private bool movedForward; // has the user moved his leg forward
        public bool MovedForward
        {
            get
            {
                return movedForward;
            }
            set
            {
                movedForward = value;
            }
        }

        //x position
        private double initialRightLegPositionX; //meters // stores the initial position of the right leg
        public double InitialRightLegPositionX
        {
            get
            {
                return initialRightLegPositionX;
            }
            set
            {
                initialRightLegPositionX = value;
            }
        }
        
        private double currentRightLegPositionX; //meters // stores the current position of the right leg 
        public double CurrentRightLegPositionX
        {
            get
            {
                return CurrentRightLegPositionX;
            }
            set
            {
                currentRightLegPositionX = value;
            }
        }

        //z position
        private double initialRightLegPositionZ; //meters
        public double InitialRightLegPositionZ
        {
            get
            {
                return initialRightLegPositionZ;
            }
            set
            {
                initialRightLegPositionZ = value;
            }
        }


        private double currentRightLegPositionZ; //meters
        public double CurrentRightLegPositionZ
        {
            get
            {
                return CurrentRightLegPositionZ;
            }
            set
            {
                currentRightLegPositionZ = value;
            }
        }

        // store 

        private double storeX1;
        public double StoreX1
        {
            get
            {
                return storeX1;
            }
            set
            {
                storeX1 = value;

            }
        }
        private double storeX2;
        public double StoreX2
        {
            get
            {
                return storeX2;
            }
            set
            {
                storeX2 = value;

            }
        }
        private double storeZ1;
        public double StoreZ1
        {
            get
            {
                return storeZ1;
            }
            set
            {
                storeZ1 = value;

            }
        }
        public double storeZ2;
        public double StoreZ2
        {
            get
            {
                return storeZ2;
            }
            set
            {
                storeZ2 = value;

            }
        }


        //left leg

        //x position
        private double initialLeftLegPositionX; //meters // stores the initial position of the left leg
        public double InitialLeftLegPositionX
        {
            get
            {
                return initialLeftLegPositionX;
            }
            set
            {
                initialLeftLegPositionX = value;
            }
        }

        private double currentLeftLegPositionX; //meters // stores the current position of the left leg 
        public double CurrentLeftLegPositionX
        {
            get
            {
                return CurrentLeftLegPositionX;
            }
            set
            {
                currentLeftLegPositionX = value;
            }
        }

        //z position
        private double initialLeftLegPositionZ; //meters
        public double InitialLeftLegPositionZ
        {
            get
            {
                return initialLeftLegPositionZ;
            }
            set
            {
                initialLeftLegPositionZ = value;
            }
        }


        private double currentLeftLegPositionZ; //meters
        public double CurrentLeftLegPositionZ
        {
            get
            {
                return CurrentLeftLegPositionZ;
            }
            set
            {
                currentLeftLegPositionZ = value;
            }
        }



        private double velocity;
        public double Velocity // the velocity of the users leg
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        private bool trying; // is the user in the trying mode or shooting
        public bool Trying
        {
            get
            {
                return trying;
            }
            set
            {
                trying = value;
            }
        }

        private double angle; // angle of the leg
        public double Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }


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
        private Vector3 shootingPosition;
        public Vector3 ShootingPosition
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

        /// <summary>
        /// this is the index of the array of currentCommands in the class Game which defines what is the command active
        /// </summary>
        private int activeCommand;
        public int ActiveCommand
        {
            get
            {
                return activeCommand;
            }
            set
            {
                activeCommand = value;
            }
        }
        private List<float> positions;
        public List<float> Positions
        {
            get
            {
                return positions;
            }
            set
            {
                positions = value;
            }
        }
        private bool disqualified;
        public bool Disqualified
        {
            get
            {
                return disqualified;
            }
            set
            {
                disqualified = value;
            }
        }
        private int disqualificationTime;
        public int DisqualificationTime
        {
            get
            {
                return disqualificationTime;
            }
            set
            {
                disqualificationTime = value;
            }
        }
        

        public User(float assumedLegMass)
        {
            kinect = new MKinect();
            user = kinect.requestSkeleton();
            this.assumedLegMass = assumedLegMass;

            initialLeftLegPositionX = 0;
            currentLeftLegPositionX = 0;
            initialLeftLegPositionZ = 0;
            currentLeftLegPositionZ = 0;

            initialRightLegPositionX = 0;
            currentRightLegPositionX = 0;
            initialRightLegPositionZ = 0;
            currentRightLegPositionZ = 0;

            storeX1 = 0;
            storeX2 = 0;
            storeZ1 = 0;
            storeZ2 = 0;
            
            velocity = 0;
            angle = 0;
        
            movedForward = false;
            trying = true;
         

        }
    }
}
