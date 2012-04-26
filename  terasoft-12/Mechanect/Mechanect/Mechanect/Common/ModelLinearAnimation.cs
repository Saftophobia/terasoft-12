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
        public Boolean animationStoped { get; private set; }

        /// <summary>
        /// constructs a ModelLinearAnimation instance
        /// </summary>
        /// <param name="model">the 3d custom model</param>
        /// <param name="velocity">the object's velocity vector</param>
        /// <param name="acceleration">the object's acceleration vector</param>
        /// <param name="enableRotation">enables object rotation</param>
        /// <param name="duration">animation duration</param>
        public ModelLinearAnimation(CustomModel model, Vector3 velocity, float acceleration, TimeSpan duration, Boolean enableRotation)
        {
            this.model = model;
            this.velocity = velocity;
            this.acceleration = Vector3.Zero;
            
            if (velocity.X > 0)
            {
                this.acceleration.X = acceleration;
            }
            if (velocity.Y != 0)
            {
                this.acceleration.Y = acceleration;
            }
            if (velocity.Z != 0)
            {
                this.acceleration.Z = acceleration;
            }
            
            this.enableRotation = enableRotation;
            elapsedTime = TimeSpan.FromSeconds(0);
            if (Vector3.Add(velocity, this.acceleration).Length() < velocity.Length())
            {
                this.duration = TimeSpan.FromSeconds(velocity.Length() / this.acceleration.Length() / 2f);
            }
            else
            {
                this.duration = duration;
            }
            startPosition = model.position;
            startAngle = model.rotation;
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
                Vector3 v = (velocity * ms / 1000f) + (acceleration * ms * ms / 1000000f);
                if (v.Z < 0)
                {
                    model.position = new Vector3(model.position.X,model.position.Y,startPosition.Z + v.Z);
                    if (enableRotation)
                    {
                        Vector3 delta = (velocity * ms / 1000f + acceleration * ms * ms / 1000000f) / 180;
                        model.rotation = startAngle + new Vector3(delta.Z, 0, 0) * 100;
                    }
                }
                if ((velocity.X > 0 && v.X > 0))
                {

                    model.position = new Vector3(startPosition.X - v.X, model.position.Y, model.position.Z);
                   
                    if (enableRotation)
                    {
                        Vector3 delta = (velocity * ms / 1000f + acceleration * ms * ms / 1000000f) / 180;
                        model.rotation = startAngle + new Vector3(0, 0, -1 * delta.X) * 100;
                    }
                }
                if ((velocity.X < 0 && v.X < 0))
                {
                    model.position = new Vector3(startPosition.X + v.X, model.position.Y, model.position.Z);

                    if (enableRotation)
                    {
                        Vector3 delta = (velocity * ms / 1000f + acceleration * ms * ms / 1000000f) / 180;
                        model.rotation = startAngle + new Vector3(0, 0, -1 * delta.X) * 100;
                    }

                }
                
            }
            else
            {
                animationStoped = true;
            }

        }
    }
}

