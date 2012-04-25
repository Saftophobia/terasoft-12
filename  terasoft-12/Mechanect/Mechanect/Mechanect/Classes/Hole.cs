using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    class Hole
    {
        private int radius;
        public int Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
        private Vector3 position;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public Hole()
        {
            position = Vector3.Zero;
            radius = 2;
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// generates a random float value between two float numbers.
        /// </summary>
        /// <param name="min">
        /// The minimum value. 
        /// </param>
        /// /// <param name="max">
        /// The maximum value.
        /// </param>

        public float GenerateRandomValue(float min, float max)
        {
            if ((min >= 0) && (max >= 0) && (max > min))
            {
                var random = new Random();
                var value = ((float)(random.NextDouble() * (max - min))) + min;
                return value;
            }
            else throw new ArgumentException("parameters have to be non negative numbers and max value has to be greater than min value");
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Takes the minimum and maximum possible values for the x,y,z positions of the hole and it's radius then initializes the hole with random values between these two numbers
        /// </summary>
        /// <param name="min">
        /// The minimum possible value for the hole variables.
        /// </param>
        /// /// <param name="max">
        /// The maximum possible value for the hole variables.
        /// </param>

        public void SetHoleValues(float min, float max)
        {
            position.X = GenerateRandomValue(min, max);
            position.Y = GenerateRandomValue(min, max);
            position.Z = GenerateRandomValue(min, max);
            radius = (int)GenerateRandomValue(min, max);
        }
    }
}
