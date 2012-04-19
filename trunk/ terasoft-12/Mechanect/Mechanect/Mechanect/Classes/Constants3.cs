using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Classes
{
    class Constants3
    {
	    public static const float MIN_VELOCITYY = 1.0f;
        public static const float MIN_VELOCITYX = 1.0f;
        public static const float MAX_VELOCITYY = 1.0f;
        public static const float MAX_VELOCITYX = 1.0f;
        public static const float MAX_HOLEPOSX = 1.0f;
        public static const float MAX_HOLEPOSY = 1.0f;
        public static const int NEGATIVE_BRADIUS = 1;
        public static const int NEGATIVE_BMASS = 2;
        public static const int NEGATIVE_HRADIUS = 3;
        public static const int NEGATIVE_LMASS = 4;
        public static const int NEGATIVE_HPOSY = 6;
        public static const int NEGATIVE_FRICTION = 7;
        public static const int NEGATIVE_RDIFFERENCE = 8;
        //Difference between the radii of the ball and hole
        public static const int HOLE_OUT_OF_FAR_RANGE = 9;
        public static const int HOLE_OUT_OF_NEAR_RANGE = 10;
        public static const int SOLVABLE_EXPERIMENT = 0;
        public static const float NORMAL_LEG_MASS = 0.01f;
        public static const double unitTime = 0.0333333333334; //seconds
        public static const float legMovementTolerance = 0.09f; //meters
    }
}
