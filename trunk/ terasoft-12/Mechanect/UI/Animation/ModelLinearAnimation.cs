using System;
using Microsoft.Xna.Framework;
using UI.Components;
using Physics;

namespace UI.Animation
{
    /// <summary>
    /// Represents 3d model linear animation with acceleration.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem
    /// </remarks>
    public class ModelLinearAnimation : Animation
    {

        public Vector3 StartPosition { get; private set; }
        public TimeSpan Duration { get; private set; }
        public Vector3 Displacement
        {
            get
            {
                return model.Position - StartPosition;
            }
        }

        private Vector3 velocity;
        private float acceleration;

        /// <summary>
        /// Creates a ModelLinearAnimation instance.
        /// </summary>
        /// <param name="model">3D custom model</param>
        /// <param name="velocity">Object's velocity vector</param>
        /// <param name="acceleration">Object's acceleration vector</param>
        /// <param name="duration">Animation duration</param>
        public ModelLinearAnimation(CustomModel model, Vector3 velocity, float acceleration, TimeSpan duration)
            : base(model)
        {
            StartPosition = model.Position;
            Duration = duration;

            this.velocity = velocity;
            this.acceleration = acceleration;
        }

        /// <summary>
        /// Updates the model position according to the time elapsed.
        /// </summary>
        /// <param name="elapsed">Time offset from the last update</param>
        public override void Update(TimeSpan elapsed)
        {
            if (ElapsedTime > Duration)
            {
                Finished = true;
                return;
            }
            ElapsedTime += elapsed;
            model.Position = StartPosition + LinearMotion.CalculateDisplacement(velocity, acceleration, ElapsedTime);
        }

    }
}

