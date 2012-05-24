﻿using System;
using Microsoft.Xna.Framework;

namespace Physics
{
    public class Functions
    {

        ///<summary>
        ///This method gets the velocity using v = x/t
        ///</summary>
        ///<remarks>
        ///<para>AUTHOR: Cena </para>  
        ///</remarks>
        ///<param name="Vector3">Takes the displacement</param> 
        ///<param name="Vector3">Takes the totaltime</param> 
        ///<returns>A Vector3: the velocity</returns>


        public static Vector3 GetVelocity(Vector3 position, double totalTime)
        {
            return new Vector3(position.X / (float)totalTime, 0, position.Z / (float)totalTime);
        }

        ///<summary>
        ///This method corrects the velocity
        ///</summary>
        ///<remarks>
        ///<para>AUTHOR: Cena </para>  
        ///</remarks>
        ///<param name="Vector3">Takes the velocity</param> 


        public static void CorrectVelocity(Vector3 velocity)
        {
            if (velocity.Length() <= 1 && velocity.Length() > 0)
                velocity *= 10;
            if (velocity.Length() <= 10 && velocity.Length() > 0)
                velocity *= 2;
            else
                velocity *= 1.5f;

        }

        ///<summary>
        ///This method calculates the veclocity vector relative to the user's assumed leg mass.
        ///</summary>
        ///<remarks>
        ///<para>AUTHOR: Cena </para>   

        ///</remarks>

        ///<returns>A Vector3: velocity of the user relative to it's mass</returns>

        public static Vector3 SetVelocityRelativeToGivenMass(float n1, float n2,Vector3 velocity)
        {

            float ratio = (float)(n1 / n2);
            return new Vector3(velocity.X * ratio, 0, velocity.Z * ratio);

        }

        /// <summary>
        /// Generates a random float value between two float numbers.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// </remarks>
        /// <param name="min">
        /// The minimum value. 
        /// </param>
        /// /// <param name="max">
        /// The maximum value.
        /// </param>
        /// <returns>
        /// Float number which is the generated random value.
        /// </returns>

        public static float GenerateRandomValue(float min, float max)
        {
            if (max > min)
            {
                var random = new Random();
                var value = ((float)(random.NextDouble() * (max - min))) + min;
                return value;
            }
            else throw new ArgumentException("max value has to be greater than min value");
        }


        /// <summary>
        /// Generates a position for the hole according to certain constraints, given the terrain width, terrain height, shooting position and the hole radius.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Khaled Salah </para>
        /// <param name="radius">The radius of the hole.</param>
        /// <param name="terrainWidth">The width of the terrain where the hole should be generated within.</param>
        /// <param name="terrainHeight">The height of the terrain where the hole should be generated within.</param>
        /// <param name="shootingPosition">The shooting position of the user.</param>
        /// </remarks>
        /// <returns>
        /// Vector which is the randomly generated position of the hole.
        /// </returns>

        public static Vector3 GeneratePosition(int radius, int terrainWidth, int terrainHeight, Vector3 shootingPosition)
        {
            float X = GenerateRandomValue(-(terrainWidth) / 4 + 3 * radius, (terrainWidth) / 4 - 3 * radius);
            float Y = -1;
            float Z = GenerateRandomValue(-(terrainHeight / 2) + (3 * radius), -(terrainHeight / 4) + 3 * radius);
            return new Vector3(X, Y, Z);
        }
    }
}
