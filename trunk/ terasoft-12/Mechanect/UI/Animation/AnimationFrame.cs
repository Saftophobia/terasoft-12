using System;
using Microsoft.Xna.Framework;

namespace UI.Animation
{
    /// <summary>
    /// represents the objects' position and orientation after certain amount of time 
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class AnimationFrame
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public TimeSpan Time;

        /// <summary>
        /// constructs an AnimationFrame instance
        /// </summary>
        /// <param name="position">object's position</param>
        /// <param name="rotation">object's orientation</param>
        /// <param name="time">frame's time offset from the start of the animation</param>
        public AnimationFrame(Vector3 position, Vector3 rotation, TimeSpan time)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Time = time;
        }
    }
}
