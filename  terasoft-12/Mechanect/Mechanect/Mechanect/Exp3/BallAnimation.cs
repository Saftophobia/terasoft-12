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
        private float holeRadius;

        public BallAnimation(Ball ball, Vector3 velocity, float friction, Vector3 holePosition, float holeRadius)
            : base(ball, velocity, friction, LinearMotion.CalculateTime(velocity.Length(), 0, friction))
        {
            this.ball = ball;
            Vector3 totalDisplacement = LinearMotion.CalculateDisplacement(velocity, friction, Duration);
            Vector3 stopPosition = StartPosition + totalDisplacement;
            this.holeRadius = holeRadius;

            //if (Vector3.Distance(stopPosition, holePosition) < (ball.Radius + holeRadius))
            //{
                //fallAnimation = new ModelFramedAnimation(ball);
                /*if (stopPosition.X < holePosition.X)
                    fallAnimation.AddFrame(new Vector3(holePosition.X + holeRadius, holePosition.Y, holePosition.Z), Vector3.Zero, TimeSpan.FromSeconds(0.5));
                else
                {
                    if (stopPosition.X > holePosition.X)
                        fallAnimation.AddFrame(new Vector3(holePosition.X - holeRadius, holePosition.Y, holePosition.Z), Vector3.Zero, TimeSpan.FromSeconds(0.5));
                    else
                        fallAnimation.AddFrame(new Vector3(holePosition.X, holePosition.Y, holePosition.Z + 10), Vector3.Zero, TimeSpan.FromSeconds(0.5));
                }*/
                //fallAnimation.AddFrame((totalDisplacement.Length() - holeRadius) * Vector3.Normalize(totalDisplacement), Vector3.Zero, TimeSpan.FromSeconds(0));
                //fallAnimation.AddFrame(holePosition, Vector3.Zero, TimeSpan.FromSeconds(2));
                //fallAnimation.AddFrame(new Vector3(holePosition.X, holePosition.Y - 20, holePosition.Z), Vector3.Zero, TimeSpan.FromSeconds(2));
            //}

        }

        public override void Update(TimeSpan elapsed)
        {
            if (fallAnimation == null || Displacement.Length() < totalDisplacement.Length())
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
