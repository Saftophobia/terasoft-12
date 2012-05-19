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
        /// constructs a ModelLinearAnimation instance
        /// </summary>
        /// <param name="model">the 3d custom model</param>
        /// <param name="velocity">the object's velocity vector</param>
        /// <param name="acceleration">the object's acceleration vector</param>
        /// <param name="duration">animation duration</param>
        public ModelLinearAnimation(CustomModel model, Vector3 velocity, float acceleration, TimeSpan duration)
            : base(model)
        {
            StartPosition = model.Position;
            Duration = duration;

            this.velocity = velocity;
            this.acceleration = acceleration;
        }

        /// <summary>
        /// updates the model position according to the time elapsed
        /// </summary>
        /// <param name="elapsed">time offset from the last update</param>
        public override void Update(TimeSpan elapsed)
        {
            this.elapsedTime += elapsed;
            if (!Finished())
            {
                model.Position = StartPosition + Physics.Functions.CalculateDisplacement(velocity, acceleration, elapsedTime);
            }
        }

        /// <summary>
        /// returns true if the animation finished
        /// </summary>
        public override bool Finished()
        {
            return elapsedTime > Duration;
        }
    }
}

