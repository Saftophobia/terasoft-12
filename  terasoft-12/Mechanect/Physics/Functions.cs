using Microsoft.Xna.Framework;

namespace Physics
{
    public class Functions
    {
        /// <summary>
        /// calculates the vector in direction of another one given its magnitude
        /// </summary>
        /// <param name="velocity">the other vector</param>
        /// <param name="frictionValue">the magnitude of the vector</param>
        /// <remarks>Auther : Bishoy Bassem</remarks>
        public static Vector3 GetVectorInDirectionOf(float magnitude, Vector3 anotherVector)
        {
            anotherVector.Normalize();
            return magnitude * anotherVector;
        }
    }
}
