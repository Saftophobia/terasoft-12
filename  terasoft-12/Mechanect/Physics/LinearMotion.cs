using System;
using Microsoft.Xna.Framework;

namespace Physics
{
    public class LinearMotion
    {
        /// <summary>
        /// calculates the vector in direction of another one given its magnitude
        /// </summary>
        /// <param name="anotherVector">the other vector</param>
        /// <param name="magnitude">the magnitude of the vector</param>
        /// <remarks>Author : Bishoy Bassem</remarks>
        public static Vector3 GetVectorInDirectionOf(float magnitude, Vector3 anotherVector)
        {
            return magnitude * Vector3.Normalize(anotherVector);
        }


        /// <summary>
        /// calculates the time the equation v = v0 + at
        /// </summary>
        /// <param name="intialVelocity">start velocity magnitude</param>
        /// <param name="finalVelocity">final velocity magnitude</param>
        /// <param name="acceleration">acceleration magnitude</param>
        /// <remarks>Author : Bishoy Bassem</remarks>
        public static TimeSpan CalculateTime(float intialVelocity, float finalVelocity, float acceleration)
        {
            return TimeSpan.FromSeconds((finalVelocity - intialVelocity) / acceleration);
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
            float seconds = (float)time.TotalSeconds;
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
            float intialVelocity = (float)Math.Sqrt((finalVelocity * finalVelocity) - 
                (2 * acceleration * displacement.Length()));

            return GetVectorInDirectionOf(intialVelocity, displacement);
        }
    }
}
