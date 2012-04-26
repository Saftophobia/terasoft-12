using System;
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
        Environment3 environment;
        public Hole()
        {
            position = Vector3.Zero;
            radius = 2;
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Generates a random float value between two float numbers.
        /// </summary>
        /// <param name="min">
        /// The minimum value. 
        /// </param>
        /// /// <param name="max">
        /// The maximum value.
        /// </param>

        public float GenerateRandomValue(float min, float max)
        {
            if (max > min)
            {
                var random = new Random();
                var value = ((float)(random.NextDouble() * (max - min))) + min;
                return value;
            }
            else throw new ArgumentException("max value has to be greater than min value");
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Sets the X,Y,Z values for the hole position which are related to the enviroment's terrain width and height.
        /// </summary>

        public void SetHoleValues()
        {   
            position.X = GenerateRandomValue(0, environment.terrainWidth);
            position.Y = 0;
            position.Z = GenerateRandomValue(0, environment.terrainHeight);
        }
    }
}
