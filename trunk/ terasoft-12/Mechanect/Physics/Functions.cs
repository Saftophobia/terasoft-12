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
        /// <remarks>Auther : Bishoy Bassem</remarks>
        public static Vector3 GetVectorInDirectionOf(float magnitude, Vector3 anotherVector)
        {
            anotherVector.Normalize();
            return magnitude * anotherVector;
        }

        /// <summary>
        /// calculates the displacement vector using the equation r - r0 = v0 * t + 0.5 * a * (t^2) 
        /// </summary>
        /// <param name="intialVelocity">start velocity vector</param>
        /// <param name="acceleration">acceleration vector</param>
        /// <param name="time">time passed</param>
        /// <remarks>Auther : Bishoy Bassem</remarks>
        public static Vector3 CalculateDisplacement(Vector3 intialVelocity, Vector3 acceleration, TimeSpan time)
        {
            float seconds = (float) time.TotalSeconds;
            return (intialVelocity * seconds) + (0.5f * acceleration * seconds * seconds);
        }

        /// <summary>
        /// calculates the intial velocity magnitude using the equation vf^2 = v0^2 + 2as
        /// </summary>
        /// <param name="finalVelocity">final velocity magnitude</param>
        /// <param name="acceleration">acceleration magnitude</param>
        /// <param name="distance">distance</param>
        /// <remarks>Auther : Bishoy Bassem</remarks>
        public static float CalculateIntialVelocity(float finalVelocity, float acceleration, float distance)
        {
            return (float)Math.Sqrt(finalVelocity * finalVelocity - 2 * acceleration * distance);
        }

    }
}
