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
        public bool willFall;
        private int fallFactor;
        private Environment3 environment;

        /// <summary>
        /// constructs a new BallAnimation instance 
        /// </summary>
        /// <param name="ball">ball</param>
        /// <param name="hole">hole</param>
        /// <param name="velocity">intial velocity vector</param>
        /// <param name="friction">friction magnitude</param>
        /// <remarks>
        /// Author : Bishoy Bassem, Omar Abdulaal
        /// </remarks>
        public BallAnimation(Ball ball, Environment3 environment, Vector3 velocity)
            : base(ball, velocity, environment.Friction, LinearMotion.CalculateTime(velocity.Length(), 0, environment.Friction))
        {
            this.ball = ball;
            this.environment = environment;
            Vector3 totalDisplacement = LinearMotion.CalculateDisplacement(velocity, environment.Friction, Duration);
            Vector3 stopPosition = StartPosition + totalDisplacement;

            if (Vector3.Distance(stopPosition, environment.HoleProperty.Position) < (ball.Radius + environment.HoleProperty.Radius))
            {
                willFall = true;
            }

        }
        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);
            ball.Rotate(Displacement);

            ball.SetHeight(environment.GetHeight(ball.Position) - fallFactor*0.15f);
            if (!base.Finished)
            {
                if (fallFactor < 50)
                {
                    if (willFall)
                    {
                        if (ElapsedTime > Duration - TimeSpan.FromSeconds(4))
                        {
                            fallFactor++;
                            ball.SetHeight(ball.Position.Y - ball.Radius - (fallFactor * 0.15f));
                        }
                    }
                }
                else
                {
                    ball.SetHeight(ball.Position.Y - ball.Radius - fallFactor * 0.15f);
                    Finished = true;
                }
            }
            else
            {
                ball.SetHeight(ball.Position.Y - ball.Radius - fallFactor * 0.15f);
            }
        }

    }
}
