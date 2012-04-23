using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Common
{
    /// <summary>
    /// represents the objects' position and orientation after certain amount of time 
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class AnimationFrame
    {
        public Vector3 position;
        public Vector3 rotation;
        public TimeSpan time;

        /// <summary>
        /// constructs an AnimationFrame instance
        /// </summary>
        /// <param name="position">object's position</param>
        /// <param name="rotation">object's orientation</param>
        /// <param name="time">frame's time offset from the start of the animation</param>
        public AnimationFrame(Vector3 position, Vector3 rotation, TimeSpan time)
        {
            this.position = position;
            this.rotation = rotation;
            this.time = time;
        }
    }
}
