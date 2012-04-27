using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Common
{
    /// <summary>
    /// represents 3d model linear animation with acceleration
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class ModelLinearAnimation
    {

        private Vector3 startPosition, startAngle, velocity, acceleration;
        private TimeSpan elapsedTime;
        private TimeSpan duration;
        private CustomModel model;
        private Boolean enableRotation;
        public Boolean AnimationStoped { get; private set; }

        /// <summary>
        /// constructs a ModelLinearAnimation instance
        /// </summary>
        /// <param name="model">the 3d custom model</param>
        /// <param name="velocity">the object's velocity vector</param>
        /// <param name="acceleration">the object's acceleration vector</param>
        /// <param name="enableRotation">enables object rotation</param>
        /// <param name="duration">animation duration</param>
        public ModelLinearAnimation(CustomModel model, Vector3 velocity, Vector3 acceleration, TimeSpan duration, Boolean enableRotation)
        {
            this.model = model;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.enableRotation = enableRotation;
            elapsedTime = TimeSpan.FromSeconds(0);
            if (Vector3.Add(velocity, acceleration).Length() < velocity.Length())
            {
                this.duration = TimeSpan.FromSeconds(velocity.Length() / acceleration.Length() / 2f);
            }
            else
            {
                this.duration = duration;
            }
            startPosition = model.Position;
            startAngle = model.Rotation;
        }

        /// <summary>
        /// updates the model position and orientation according to the time elapsed
        /// </summary>
        /// <param name="elapsed">time offset from the last update</param>
        public void Update(TimeSpan elapsed)
        {
            this.elapsedTime += elapsed;
            if (elapsedTime.TotalSeconds < duration.TotalSeconds)
            {
                float ms = ((float)elapsedTime.TotalMilliseconds);
                model.Position = startPosition + (velocity * ms / 1000f) + (acceleration * ms * ms / 1000000f);
                if (enableRotation)
                {
                    Vector3 delta = (velocity * ms / 1000f + acceleration * ms * ms / 1000000f) / (float) Math.PI;
                    model.Rotation = startAngle + new Vector3(delta.Z, delta.Y, -delta.X);
                }
            }
            else
            {
                AnimationStoped = true;
            }

        }
    }
}

