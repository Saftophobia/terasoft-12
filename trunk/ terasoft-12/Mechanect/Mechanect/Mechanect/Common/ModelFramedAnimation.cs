using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Common
{
    public class ModelFramedAnimation
    {
        List<AnimationFrame> frames = new List<AnimationFrame>();
        TimeSpan elapsedTime = TimeSpan.FromSeconds(0);

        CustomModel model;

        public ModelFramedAnimation(CustomModel model)
        {
            this.model = model;
            this.frames = new List<AnimationFrame>(); ;
            AddFrame(model.position, model.rotation, TimeSpan.FromSeconds(0));
        }

        public void Update(TimeSpan Elapsed)
        {

            this.elapsedTime += Elapsed;
            TimeSpan endTime = frames[frames.Count - 1].Time;

            if (elapsedTime > endTime)
                return;

            int i = 0;

            while (frames[i + 1].Time < elapsedTime)
                i++;

            TimeSpan frameElapsedTime = elapsedTime - frames[i].Time;
            float amt = (float)((frameElapsedTime.TotalSeconds) / (frames[i + 1].Time - frames[i].Time).TotalSeconds);

            model.position = Vector3.CatmullRom(
               frames[wrap(i - 1, frames.Count - 1)].Position,
               frames[wrap(i, frames.Count - 1)].Position,
               frames[wrap(i + 1, frames.Count - 1)].Position,
               frames[wrap(i + 2, frames.Count - 1)].Position,
               amt);

            model.rotation = Vector3.Lerp(frames[i].Rotation, frames[i + 1].Rotation, amt);
        }

        private int wrap(int value, int max)
        {
            while (value > max)
                value -= max;

            while (value < 0)
                value += max;

            return value;
        }

        public void AddFrame(Vector3 Position, Vector3 Rotation, TimeSpan Time)
        {
            frames.Add(new AnimationFrame(Position, Rotation, Time));
        }
    }
}
