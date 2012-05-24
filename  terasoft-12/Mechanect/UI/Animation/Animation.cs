using System;
using UI.Components;

namespace UI.Animation
{
    /// <summary>
    /// Represents a generic type for all animation types.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem.
    /// </remarks>
    public abstract class Animation
    {
        public TimeSpan ElapsedTime { get; protected set; }
        protected CustomModel model;
        public bool Finished { get; protected set; }

        /// <summary>
        /// Intializes the animation's attributes.
        /// </summary>
        /// <param name="model">CustomModel instance.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        protected Animation(CustomModel model)
        {
            this.model = model;
            ElapsedTime = TimeSpan.FromSeconds(0);
        }

        /// <summary>
        /// Updates the model's position and orientation according to the time elapsed.
        /// </summary>
        /// <param name="elapsed">Time offset from the last update.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public abstract void Update(TimeSpan elapsed);

        /// <summary>
        /// Stops the animation.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public virtual void Stop()
        {
            Finished = true;
        }

    }
}
