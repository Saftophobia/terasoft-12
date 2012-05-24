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

        ///<remarks>
        ///<para>AUTHOR:Bishoy Bassem,  Omar Abdulaal</para>
        ///</remarks>
        /// <summary>
        /// Creates a new BallAnimation instance.
        /// </summary>
        /// <param name="ball">Ball instance.</param>
        /// <param name="environment">Environment3 instance.</param>
        /// <param name="velocity">Intial velocity vector.</param>
        public BallAnimation(Ball ball, Environment3 environment, Vector3 velocity)
            : base(ball, velocity, Environment3.Friction, LinearMotion.CalculateTime(velocity.Length(), 0, Environment3.Friction))
        {
            this.ball = ball;
            this.environment = environment;
            Vector3 totalDisplacement = LinearMotion.CalculateDisplacement(velocity, Environment3.Friction, Duration);
            Vector3 stopPosition = StartPosition + totalDisplacement;

            if (Vector3.Distance(stopPosition, environment.HoleProperty.Position) < (ball.Radius + environment.HoleProperty.Radius))
            {
                willFall = true;
            }

        }

        ///<remarks>
        ///<para>AUTHOR: Omar Abdulaal </para>
        ///</remarks>
        /// <summary>
        /// Updates the animation 
        /// </summary>
        /// <param name="elapsed">Elapsed Time</param>
        public override void Update(TimeSpan elapsed)
        {
            base.Update(elapsed);
            ball.Rotate(Displacement);

            if (!Finished)
            {
                if (willFall)
                {
                    if (ElapsedTime > Duration - TimeSpan.FromSeconds(4))
                    {
                        if (fallFactor < 100)
                        {
                            ball.SetHeight(ball.Position.Y - ball.Radius - (fallFactor * 0.15f));
                            fallFactor++;
                        }
                        else
                        {
                            ball.SetHeight(ball.Position.Y - ball.Radius - (fallFactor * 0.15f));
                        }
                    }
                    else
                        ball.SetHeight(environment.GetHeight(ball.Position) + ball.Radius);
                }
                else
                {
                    ball.SetHeight(environment.GetHeight(ball.Position) + ball.Radius);
                    if (ball.Position.Y < environment.HoleProperty.Position.Y)
                        ball.SetHeight(environment.GetHeight(StartPosition) + ball.Radius);
                }
            }
        }
    }
}
