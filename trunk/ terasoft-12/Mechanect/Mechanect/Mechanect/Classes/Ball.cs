using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Classes
{
    class Ball
    {
        private int radius;
        public int Radius {
            get 
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
        private double mass;
        public double Mass {
            get
            {
                return mass;
            }
            set
            {
                mass = value;
            }
        }

        public Ball(float minMass, float maxMass)
        {
            mass = generateBallMass(minMass,maxMass);
        }

        /// <summary>
        /// generates a random mass for the ball within a certain range
        /// </summary>
        /// <param name="min">minimum mass of the ball</param>
        /// <param name="max">maximum mass of the ball</param>
        /// <returns></returns>
        public float generateBallMass(float min, float max)
        {
            Random random = new Random();
            float generatedMass = ((float)(random.NextDouble() * (max - min))) + min;
            return generatedMass;
        }



    }
}
