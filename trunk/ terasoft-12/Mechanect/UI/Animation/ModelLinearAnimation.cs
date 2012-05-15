using System;
using Microsoft.Xna.Framework;
using UI.Components;

namespace UI.Animation
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
        private bool enableRotation;
        public bool AnimationStoped { get; private set; }

        /// <summary>
        /// constructs a ModelLinearAnimation instance
        /// </summary>
        /// <param name="model">the 3d custom model</param>
        /// <param name="velocity">the object's velocity vector</param>
        /// <param name="acceleration">the object's acceleration vector</param>
        /// <param name="enableRotation">enables object rotation</param>
        /// <param name="duration">animation duration</param>
        public ModelLinearAnimation(CustomModel model, Vector3 velocity, float acceleration, TimeSpan duration, bool enableRotation)
        {
            this.model = model;
            startPosition = model.Position;
            startAngle = model.Rotation;

            this.velocity = velocity;
            this.acceleration = Physics.Functions.GetVectorInDirectionOf(acceleration, velocity);
            this.duration = duration;
            this.enableRotation = enableRotation;

            if (acceleration < 0)
            {
                this.duration = TimeSpan.FromSeconds(Math.Abs(velocity.Length() / acceleration));
            }
        }

        public ModelLinearAnimation(CustomModel model, Vector3 endPosition, float finalVelocity, float acceleration, bool enableRotation)
        {
            this.model = model;
            startPosition = model.Position;
            startAngle = model.Rotation;

            Vector3 displacement = endPosition - startPosition;
            float intialVelocity = Physics.Functions.CalculateIntialVelocity(finalVelocity, acceleration, displacement.Length());

            this.velocity = Physics.Functions.GetVectorInDirectionOf(intialVelocity, displacement); ;
            this.acceleration = Physics.Functions.GetVectorInDirectionOf(acceleration, displacement);
            this.duration = TimeSpan.FromSeconds(2 * displacement.Length() / (intialVelocity + finalVelocity));
            this.enableRotation = enableRotation;
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
                Vector3 displacement = Physics.Functions.CalculateDisplacement(velocity, acceleration, elapsedTime);
                model.Position = startPosition + displacement;
                if (enableRotation)
                {
                    model.Rotation = startAngle + new Vector3(displacement.Length() / 3, (float) Math.Atan2(displacement.X, displacement.Z), 0);
                }
            }
            else
            {
                AnimationStoped = true;
            }

        }
    }
}

