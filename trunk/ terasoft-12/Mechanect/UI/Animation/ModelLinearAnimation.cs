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
    public class ModelLinearAnimation : Animation
    {

        private Vector3 startPosition, startAngle, velocity;
        private float acceleration;
        private TimeSpan duration;
        public Vector3 Displacement
        {
            get
            {
                return model.Position - startPosition;
            }
        }

        /// <summary>
        /// constructs a ModelLinearAnimation instance
        /// </summary>
        /// <param name="model">the 3d custom model</param>
        /// <param name="velocity">the object's velocity vector</param>
        /// <param name="acceleration">the object's acceleration vector</param>
        /// <param name="duration">animation duration</param>
        public ModelLinearAnimation(CustomModel model, Vector3 velocity, float acceleration, TimeSpan duration)
            : base(model)
        {
            startPosition = model.Position;
            startAngle = model.Rotation;

            this.velocity = velocity;
            this.acceleration = acceleration;
            this.duration = duration;

            if (acceleration < 0)
            {
                this.duration = TimeSpan.FromSeconds(Math.Abs(velocity.Length() / acceleration));
            }
        }

        /// <summary>
        /// updates the model position according to the time elapsed
        /// </summary>
        /// <param name="elapsed">time offset from the last update</param>
        public override void Update(TimeSpan elapsed)
        {
            this.elapsedTime += elapsed;
            if (elapsedTime < duration)
            {
                model.Position = startPosition + Physics.Functions.CalculateDisplacement(velocity, acceleration, elapsedTime);
            }
        }

        public override bool Finished()
        {
            return elapsedTime > duration;
        }
    }
}

