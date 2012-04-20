using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Common
{
    public class AnimationFrame
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public TimeSpan Time;

        public AnimationFrame(Vector3 Position, Vector3 Rotation, TimeSpan Time)
        {
            this.Position = Position;
            this.Rotation = Rotation;
            this.Time = Time;
        }
    }
}
