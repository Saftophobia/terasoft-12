using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UI.Components;

namespace UI.Animation
{
    /// <summary>
    /// represents 3d model framed animation
    /// </summary>
    /// <remarks>
    /// Auther : Bishoy Bassem
    /// </remarks>
    public class ModelFramedAnimation : Animation
    {

        private List<AnimationFrame> frames = new List<AnimationFrame>();

        /// <summary>
        /// constructs a ModelFramedAnimation instance
        /// </summary>
        /// <param name="model">the 3d custom model</param>
        public ModelFramedAnimation(CustomModel model)
            : base(model)
        {
           frames = new List<AnimationFrame>();
        }

        /// <summary>
        /// updates the model position and orientation according to the time elapsed
        /// </summary>
        /// <param name="elapsed">time offset from the last update</param>
        public override void Update(TimeSpan elapsed)
        {
            this.elapsedTime += elapsed;
            if (Finished())
            {
                return;
            }

            int i = 0;
            while (frames[i + 1].Time < elapsedTime)
            {
                i++;
            }

            TimeSpan frameElapsedTime = elapsedTime - frames[i].Time;
            float amt = (float)((frameElapsedTime.TotalSeconds) / (frames[i + 1].Time - frames[i].Time).TotalSeconds);

            model.Position = Vector3.CatmullRom(
               frames[Wrap(i - 1, frames.Count - 1)].Position,
               frames[Wrap(i, frames.Count - 1)].Position,
               frames[Wrap(i + 1, frames.Count - 1)].Position,
               frames[Wrap(i + 2, frames.Count - 1)].Position,
               amt);

            model.Rotation = Vector3.Lerp(frames[i].Rotation, frames[i + 1].Rotation, amt);
        }

        /// <summary>
        /// wraps the value to be between 0 and max
        /// </summary>
        /// <param name="max">the maximum number the value can reach </param>
        /// <param name="value">number to be wraped</param>
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
        /// adds an AnimationFrame to sequence of frames
        /// </summary>
        /// <param name="position">object's position</param>
        /// <param name="rotation">object's orientation</param>
        /// <param name="time">frame's time offset from the start of the animation</param>
        public void AddFrame(Vector3 position, Vector3 rotation, TimeSpan time)
        {
            frames.Add(new AnimationFrame(position, rotation, time));
        }

        /// <summary>
        /// returns true if the animation finished
        /// </summary>
        public override bool Finished()
        {
            if (frames.Count == 0)
            {
                return true;
            }
            else
            {
                return elapsedTime > frames[frames.Count - 1].Time;
            }
        }
    }
}
