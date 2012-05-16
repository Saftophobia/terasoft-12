using System;
using Microsoft.Xna.Framework;

namespace Physics
{
    public class Functions
    {

        /// <summary>
        /// calculates the vector in direction of another one given its magnitude
        /// </summary>
        /// <param name="anotherVector">the other vector</param>
        /// <param name="magnitude">the magnitude of the vector</param>
        /// <remarks>Author : Bishoy Bassem</remarks>
        public static Vector3 GetVectorInDirectionOf(float magnitude, Vector3 anotherVector)
        {
            anotherVector.Normalize();
            return magnitude * anotherVector;
        }


        /// <summary>
        /// calculates the displacement vector using the equation r - r0 = v0t + 0.5a(t^2) 
        /// </summary>
        /// <param name="intialVelocity">start velocity vector</param>
        /// <param name="acceleration">acceleration vector</param>
        /// <param name="time">time passed</param>
        /// <remarks>Author : Bishoy Bassem</remarks>
        public static Vector3 CalculateDisplacement(Vector3 intialVelocity, float acceleration, TimeSpan time)
        {
            float seconds = (float) time.TotalSeconds;
            Vector3 accelerationVector = GetVectorInDirectionOf(acceleration, intialVelocity);
            return (intialVelocity * seconds) + (0.5f * accelerationVector * seconds * seconds);
        }


        /// <summary>
        /// calculates the intial velocity magnitude using the equation vf^2 = v0^2 + 2as
        /// </summary>
        /// <param name="displacement">displacement vector</param>
        /// <param name="finalVelocity">final velocity magnitude</param>
        /// <param name="acceleration">acceleration magnitude</param>
        /// <remarks>Author : Bishoy Bassem</remarks>
        public static Vector3 CalculateIntialVelocity(Vector3 displacement, float finalVelocity, float acceleration)
        {
            float intialVelocity = (float)Math.Sqrt(finalVelocity * finalVelocity - 2 * acceleration * displacement.Length());
            return Physics.Functions.GetVectorInDirectionOf(intialVelocity, displacement); 
        }


        /// <summary>
        /// calculates the time using the equation s = (v0 + vf)t / 2
        /// </summary>
        /// <param name="distance">distance magnitude</param>
        /// <param name="intialVelocity">intial velocity magnitude</param>
        /// <param name="finalVelocity">final velocity magnitude</param>
        /// <returns></returns>
        public static TimeSpan CalculateTime(float distance, float intialVelocity, float finalVelocity)
        {
            return TimeSpan.FromSeconds(2 * distance / (intialVelocity + finalVelocity));
        }


        /// <summary>
        /// Generates a random float value between two float numbers.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <param name="min">
        /// The minimum value. 
        /// </param>
        /// /// <param name="max">
        /// The maximum value.
        /// </param>
        /// <returns>
        /// Float number which is the generated random value.
        /// </returns>

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

    }
}
