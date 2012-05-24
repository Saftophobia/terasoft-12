using System;
using Microsoft.Xna.Framework;

namespace Physics
{
    /// <summary>
    /// Contains linear motion functions.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem.
    /// </remarks>
    public class LinearMotion
    {
        /// <summary>
        /// Calculates the vector in direction of another one given its magnitude.
        /// </summary>
        /// <param name="magnitude">Vector magnitude.</param>
        /// <param name="anotherVector">The other vector.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public static Vector3 GetVectorInDirectionOf(float magnitude, Vector3 anotherVector)
        {
            return magnitude * Vector3.Normalize(anotherVector);
        }


        /// <summary>
        /// Calculates the time using v = v0 + at equation.
        /// </summary>
        /// <param name="intialVelocity">Initial velocity magnitude.</param>
        /// <param name="finalVelocity">Final velocity magnitude.</param>
        /// <param name="acceleration">Acceleration magnitude.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public static TimeSpan CalculateTime(float initialVelocity, float finalVelocity, float acceleration)
        {
            return TimeSpan.FromSeconds((finalVelocity - initialVelocity) / acceleration);
        }


        /// <summary>
        /// Calculates the displacement vector using r - r0 = v0t + 0.5a(t^2) equation.
        /// </summary>
        /// <param name="intialVelocity">Initial velocity vector.</param>
        /// <param name="acceleration">Acceleration magnitude.</param>
        /// <param name="time">Time passed.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public static Vector3 CalculateDisplacement(Vector3 initialVelocity, float acceleration, TimeSpan time)
        {
            float seconds = (float)time.TotalSeconds;
            Vector3 accelerationVector = GetVectorInDirectionOf(acceleration, initialVelocity);
            return (initialVelocity * seconds) + (0.5f * accelerationVector * seconds * seconds);
        }


        /// <summary>
        /// Calculates the initial velocity magnitude using vf^2 = v0^2 + 2as equation.
        /// </summary>
        /// <param name="displacement">Displacement vector.</param>
        /// <param name="finalVelocity">Final velocity magnitude.</param>
        /// <param name="acceleration">Acceleration magnitude.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public static Vector3 CalculateInitialVelocity(Vector3 displacement, float finalVelocity, float acceleration)
        {
            float initialVelocity = (float)Math.Sqrt((finalVelocity * finalVelocity) - 
                (2 * acceleration * displacement.Length()));

            return GetVectorInDirectionOf(initialVelocity, displacement);
        }
    }
}
