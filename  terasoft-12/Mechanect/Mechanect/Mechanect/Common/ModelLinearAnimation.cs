using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Common
{
    public class ModelLinearAnimation
    {

        private Vector3 startPosition, startAngle, velocity, acceleration;
        private TimeSpan elapsedTime;
        private TimeSpan duration;
        private CustomModel model;
        private Boolean enableRotation;
        private Boolean objectWillStop;

        public ModelLinearAnimation(CustomModel model, Vector3 velocity, Vector3 acceleration, Boolean enableRotation)
        {
            this.model = model;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.enableRotation = enableRotation;
            elapsedTime = TimeSpan.FromSeconds(0);
            if (Vector3.Add(velocity, acceleration).Length() < velocity.Length())
            {
                objectWillStop = true;
                duration = TimeSpan.FromSeconds(velocity.Length() / acceleration.Length() / 2);
            }
            startPosition = model.position;
            startAngle = model.rotation;
        }

        public void Update(TimeSpan elapsed)
        {
            this.elapsedTime += elapsed;
            if (!objectWillStop || elapsedTime.TotalSeconds < duration.TotalSeconds)
            {
                float ms = ((float)elapsedTime.TotalMilliseconds);
                model.position = startPosition + (velocity * ms / 1000f) + (acceleration * ms * ms / 1000000f);
                if (enableRotation)
                {
                    model.rotation = startAngle + (velocity * ms / 1000f + acceleration * ms * ms / 1000000f) / 180;
                }
            }

        }
    }
}
