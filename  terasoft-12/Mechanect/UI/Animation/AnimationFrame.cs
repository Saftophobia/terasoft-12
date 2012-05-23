using System;
using Microsoft.Xna.Framework;

namespace UI.Animation
{
    /// <summary>
    /// Represents the object's position and orientation after certain amount of time.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem
    /// </remarks>
    public class AnimationFrame
    {
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public TimeSpan Time { get; private set; }

        /// <summary>
        /// Creates an AnimationFrame instance.
        /// </summary>
        /// <param name="position">Object's position</param>
        /// <param name="rotation">Object's orientation</param>
        /// <param name="time">Frame's time offset from the start of the animation</param>
        public AnimationFrame(Vector3 position, Vector3 rotation, TimeSpan time)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Time = time;
        }
    }
}
