using System;
using UI.Components;

namespace UI.Animation
{
    /// <summary>
    /// represents a generic type for all Animation types
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public abstract class Animation
    {
        public TimeSpan ElapsedTime { get; protected set; }
        protected CustomModel model;
        public bool Finished { get; set; }

        /// <summary>
        /// intializes the animation's model
        /// </summary>
        /// <param name="model">model</param>
        protected Animation(CustomModel model)
        {
            this.model = model;
            ElapsedTime = TimeSpan.FromSeconds(0);
        }

        /// <summary>
        /// updates the model position and orientation according to the time elapsed
        /// </summary>
        /// <param name="elapsed">time offset from the last update</param>
        public abstract void Update(TimeSpan elapsed);

    }
}
