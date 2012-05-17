using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using UI.Animation;

namespace Mechanect.Exp3
{
    public class BallAnimation : ModelLinearAnimation
    {
        
        private ModelFramedAnimation fallAnimation;
        private Ball ball;
        private Vector3 holePosition;
        private Vector3 stopPosition;
        private bool ballFell;

        public BallAnimation(Ball ball, Vector3 shootVelocity, float friction, Vector3 holePosition, float holeRadius)
            : base(ball, shootVelocity, friction, TimeSpan.FromSeconds(10))
        {
            this.ball = ball;
            ball.Rotation = Vector3.Zero;
            this.holePosition = holePosition;
            fallAnimation = new ModelFramedAnimation(ball);
            stopPosition = Physics.Functions.CalculateDisplacement(shootVelocity, friction, TimeSpan.FromSeconds(Math.Abs(shootVelocity.Length() / friction)));

            if (Vector3.Distance(stopPosition, holePosition) < (ball.Radius + holeRadius))
            {
                if (shootVelocity.X > 0)
                    fallAnimation.AddFrame(new Vector3(stopPosition.X + 0.5f, -0.5f, stopPosition.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));
                else
                    fallAnimation.AddFrame(new Vector3(stopPosition.X - 0.5f, -0.5f, stopPosition.Z - 0.5f), Vector3.Zero, TimeSpan.FromSeconds(0.2));

                fallAnimation.AddFrame(holePosition, Vector3.Zero, TimeSpan.FromSeconds(0.75));

                ballFell = true;
            }

        }

        public override void Update(TimeSpan elapsed)
        {
            Vector3 displacement = base.Displacement;


            if (!base.Finished())
            {
                base.Update(elapsed);
                ball.Rotate(Displacement);
                return;
            }
            // check if ball fell and animte
            /*if ((displacement.Length() / Vector3.Distance(initialPosition, stopPosition) < 0.95) || !ballFell)
            {
                base.Update(elapsed);
                ball.Rotate(Displacement);
                return;
            }
            else
                fallAnimation.Update(elapsed);
             */
        }

        public override bool Finished()
        {
            if (ballFell)
            {
                return base.Finished() && fallAnimation.Finished();
            }
            else
            {
                return base.Finished();
            }
        }

    }
}
