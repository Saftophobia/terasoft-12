using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using UI.Animation;
using Physics;

namespace Mechanect.Exp3
{
    public class BallAnimation : ModelLinearAnimation
    {

        private Ball ball;
        private ModelFramedAnimation fallAnimation;
        private Vector3 totalDisplacement;

        public BallAnimation(Ball ball, Vector3 velocity, float friction, Vector3 holePosition, float holeRadius)
            : base(ball, velocity, friction, LinearMotion.CalculateTime(velocity.Length(), 0, friction))
        {
            this.ball = ball;
            Vector3 totalDisplacement = LinearMotion.CalculateDisplacement(velocity, friction, Duration);
            Vector3 stopPosition = StartPosition + totalDisplacement;

            if (totalDisplacement.Length() < (ball.Radius + holeRadius))
            {
                fallAnimation = new ModelFramedAnimation(ball);
                if (stopPosition.X < holePosition.X)
                    fallAnimation.AddFrame(new Vector3(stopPosition.X + 0.5f, -0.5f, stopPosition.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
                else
                {
                    if (stopPosition.X > holePosition.X) fallAnimation.AddFrame(new Vector3(stopPosition.X - 0.5f, -0.5f, stopPosition.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
                    else fallAnimation.AddFrame(stopPosition, Vector3.Zero, TimeSpan.FromSeconds(0.2));
                }
                fallAnimation.AddFrame(holePosition, Vector3.Zero, TimeSpan.FromSeconds(0.75));
            }

        }

        public override void Update(TimeSpan elapsed)
        {
            if (fallAnimation == null || Displacement.Length() / totalDisplacement.Length() < 0.95)
            {
                base.Update(elapsed);
                ball.Rotate(Displacement);
            }
            else
            {
                fallAnimation.Update(elapsed);
            }
        }

        public override bool Finished()
        {
            if (fallAnimation == null)
            {
                return base.Finished();
            }
            else
            {
                return fallAnimation.Finished();
            }
        }

    }
}
