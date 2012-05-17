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

        public BallAnimation(Ball ball, Vector3 shootVelocity, float friction, Vector3 holePosition)
            : base(ball, shootVelocity, friction, TimeSpan.FromSeconds(10))
        {
            this.ball = ball;
            ball.Rotation = Vector3.Zero;
            this.holePosition = holePosition;
            fallAnimation = new ModelFramedAnimation(ball);
            stopPosition = Physics.Functions.CalculateDisplacement(shootVelocity, friction, TimeSpan.FromSeconds(Math.Abs(shootVelocity.Length() / friction)));
        }

        public override void Update(TimeSpan elapsed)
        {
            if (!base.Finished())
            {
                base.Update(elapsed);
                ball.Rotate(Displacement);
                return;
            }
            // check if ball fell and animte
        }

        public override bool Finished()
        {
            bool ballFell = true;
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
