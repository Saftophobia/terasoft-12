using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Exp3
{
    public static class Constants3
    {
        //Used for UnitTests
        public static Game1 game1;
        public static Environment3 environment3;
        
        
        public const float minVelocityZ = 0.0f;
        public const float maxVelocityZ = 30.0f;
        public const float maxHolePosX = 60f;
        public const float maxHolePosZ = 60f;
        public const int negativeBRradius = 1;
        public const int negativeBMass = 2;
        public const int negativeHRadius = 3;
        public const int negativeLMass = 4;
        public const int negativeHPosZ = 6;
        public const int negativeFriction = 7;
        public const int negativeRDifference = 8;
        public const double scaleRatio = 0.014;
        //Difference between the radii of the ball and hole
        public const int holeOutOfFarRange = 9;
        public const int holeOutOfNearRange = 10;
        public const int solvableExperiment = 0;
        public const float normalLegMass = 0.01f;
        public const float legMovementTolerance = 0.15f; //meters
        public const float minimumShootingDistance = 0.25f;//meters
        public const float movingForwardTolerance = 0.09f;
        public const float velocityScale = 2;

        public const int minShootingX = -10;
        public const int maxShootingX = 10;
        public const int minShootingZ = 25;
        public const int maxShootingZ = 45;
    }
}
