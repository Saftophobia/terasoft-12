using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UI.Components;

namespace UI.Animation
{
    /// <summary>
    /// Represents the 3D model's framed animation.
    /// </summary>
    /// <remarks>
    /// AUTHOR : Bishoy Bassem.
    /// </remarks>
    public class ModelFramedAnimation : Animation
    {

        private List<AnimationFrame> frames = new List<AnimationFrame>();

        /// <summary>
        /// Creates a new ModelFramedAnimation instance.
        /// </summary>
        /// <param name="model">CustomModdel instance.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public ModelFramedAnimation(CustomModel model)
            : base(model)
        {
           frames = new List<AnimationFrame>();
        }

        /// <summary>
        /// Updates the model's position and orientation according to the time elapsed.
        /// </summary>
        /// <param name="elapsed">Time offset from the last update.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void Update(TimeSpan elapsed)
        {
            if (ElapsedTime > frames[frames.Count - 1].Time)
            {
                Finished = true;
                return;
            }
            ElapsedTime += elapsed;
           
            int i = 0;
            while (frames[i + 1].Time < ElapsedTime)
            {
                i++;
            }

            TimeSpan frameElapsedTime = ElapsedTime - frames[i].Time;
            float amt = (float)((frameElapsedTime.TotalSeconds) 
                / (frames[i + 1].Time - frames[i].Time).TotalSeconds);

            model.Position = Vector3.CatmullRom(
               frames[Wrap(i - 1, frames.Count - 1)].Position,
               frames[Wrap(i, frames.Count - 1)].Position,
               frames[Wrap(i + 1, frames.Count - 1)].Position,
               frames[Wrap(i + 2, frames.Count - 1)].Position,
               amt);

            model.Rotation = Vector3.Lerp(frames[i].Rotation, frames[i + 1].Rotation, amt);
        }

        /// <summary>
        /// Wraps the value between 0 and max.
        /// </summary>
        /// <param name="max">The maximum number the value can reach.</param>
        /// <param name="value">Number to be wraped.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        private int Wrap(int value, int max)
        {
            while (value > max)
            {
                value -= max;
            }
            while (value < 0)
            {
                value += max;
            }
            return value;
        }

        /// <summary>
        /// Adds a new AnimationFrame to the sequence of frames.
        /// </summary>
        /// <param name="position">Object's position.</param>
        /// <param name="rotation">Object's orientation.</param>
        /// <param name="time">Frame's time offset from the start of the animation.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void AddFrame(Vector3 position, Vector3 rotation, TimeSpan time)
        {
            frames.Add(new AnimationFrame(position, rotation, time));
        }

    }
}
