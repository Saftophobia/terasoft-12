using Microsoft.Xna.Framework;

namespace Physics
{
    public class Functions
    {
        /// <summary>
        /// calculates the friction vector that will cause the velocity to stop without changing the velocity direction
        /// </summary>
        /// <param name="velocity">the velocity vector</param>
        /// <param name="frictionValue">the value of the friction</param>
        /// <remarks>Auther : Bishoy Bassem</remarks>
        public static Vector3 GetVectorInDirectionOf(float magnitude, Vector3 anotherVector)
        {
            anotherVector.Normalize();
            return magnitude * anotherVector;
        }
    }
}
