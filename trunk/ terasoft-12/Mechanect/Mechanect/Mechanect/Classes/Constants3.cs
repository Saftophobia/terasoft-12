using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Classes
{
    static class Constants3
    {
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
        //Difference between the radii of the ball and hole
        public const int holeOutOfFarRange = 9;
        public const int holeOutOfNearRange = 10;
        public const int solvableExperiment = 0;
        public const float normalLegMass = 0.01f;
        public const float legMovementTolerance = 0.15f; //meters
    }
}
