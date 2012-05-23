using System;
using UI.Components;

namespace UI.Animation
{
    /// <summary>
    /// Represents a generic type for all animation types.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem
    /// </remarks>
    public abstract class Animation
    {
        public TimeSpan ElapsedTime { get; protected set; }
        protected CustomModel model;
        public bool Finished { get; set; }

        /// <summary>
        /// Intializes the animation's attributes.
        /// </summary>
        /// <param name="model">model</param>
        protected Animation(CustomModel model)
        {
            this.model = model;
            ElapsedTime = TimeSpan.FromSeconds(0);
        }

        /// <summary>
        /// Updates the model position and orientation according to the time elapsed.
        /// </summary>
        /// <param name="elapsed">Time offset from the last update</param>
        public abstract void Update(TimeSpan elapsed);

    }
}
