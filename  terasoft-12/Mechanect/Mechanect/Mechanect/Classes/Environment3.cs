using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Mechanect.Classes
{
    class Environment3 : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Hole hole;
        private Ball ball;
        private User user;
        private float wind;
        private float friction;
        private bool hasCollidedWithBall, ballShot;
        private double ballMass, assumedLegMass;

        public Environment3(Microsoft.Xna.Framework.Game game, User user, float minBallMass, float maxBallMass) : base(game)
        {

            this.user = user;
            ball = new Ball(minBallMass,maxBallMass);
           
        }
        


        public int IsSolvable()
        {

            if (ball.Radius <= 0)
                return Constants3.NEGATIVE_BRADIUS;
            if (ball.Mass <= 0)
                return Constants3.NEGATIVE_BMASS;
            if (hole.Radius <= 0)
                return Constants3.NEGATIVE_HRADIUS;
            if (user.AssumedLegMass <= 0)
                return Constants3.NEGATIVE_LMASS;
            //hole position not before the leg position
            if (hole.Position.Y - user.ShootingPosition.Y <= 0)
                return Constants3.NEGATIVE_HPOSY;
            if (friction < 0)
                return Constants3.NEGATIVE_FRICTION;
            if (ball.Radius > hole.Radius)
                return Constants3.NEGATIVE_RDIFFERENCE;

            /*
             * Enviornment e;
             * e.getFinalPosition(e.getVelocityAfterCollision(new Vector2(Constants3.MAX_VELOCITYX, Constants3.MAX_VELOCITYY,
             * ballMass,AssumedLegMass,wind));//implement with hasScored();
            */

            Vector2 finalPos = Vector2.Zero;//to be changed with the position returned from getFinalPostion();
            if (Vector2.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector2.Subtract(hole.Position, user.ShootingPosition).LengthSquared())
                return Constants3.HOLE_OUT_OF_FAR_RANGE;
            /*
             * Enviornment e;
             * e.getFinalPosition(e.getVelocityAfterCollision(new Vector2(Constants3.MIN_VELOCITYX, Constants3.MIN_VELOCITYY,
             * ballMass, AssumedLegMass, wind, friction));
            */

            if (Vector2.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector2.Subtract(hole.Position, user.ShootingPosition).LengthSquared()) //length squared used for better performance than length
                return Constants3.HOLE_OUT_OF_NEAR_RANGE;

            return Constants3.SOLVABLE_EXPERIMENT;//means solvable
        }


        public void GenerateSolvable()
        {
            int x = Constants3.SOLVABLE_EXPERIMENT;
            do
            {
                x = IsSolvable();
                switch (x)
                {
                    case Constants3.HOLE_OUT_OF_NEAR_RANGE: if (hole.Position.X == Constants3.MAX_HOLEPOSX && hole.Position.Y != Constants3.MAX_HOLEPOSY)
                            hole.Position = (Vector2.Add(hole.Position, new Vector2(0, -1)));
                        else if (hole.Position.Y == Constants3.MAX_HOLEPOSX && hole.Position.X != Constants3.MAX_HOLEPOSY)
                            hole.Position = (Vector2.Add(hole.Position, new Vector2(1, 0)));
                        else friction++; break;
                    case Constants3.HOLE_OUT_OF_FAR_RANGE: if (hole.Position.X == Constants3.MAX_HOLEPOSX && hole.Position.Y != Constants3.MAX_HOLEPOSY)
                            hole.Position = (Vector2.Subtract(hole.Position, new Vector2(0, -1)));
                        else if (hole.Position.Y == Constants3.MAX_HOLEPOSX && hole.Position.X != Constants3.MAX_HOLEPOSY)
                            hole.Position = (Vector2.Subtract(hole.Position, new Vector2(1, 0)));
                        else if (wind != 0)
                            wind--;
                        else if (friction != 1)
                            friction--;
                        else if (friction <= 1)
                            friction -= 0.1f;
                        break;
                    case Constants3.NEGATIVE_RDIFFERENCE: int tmp = ball.Radius; ball.Radius = (hole.Radius); hole.Radius = (tmp); break;
                    case Constants3.NEGATIVE_LMASS: user.AssumedLegMass *= 1; break;
                    case Constants3.NEGATIVE_BMASS: ball.Mass *= -1; break;
                    case Constants3.NEGATIVE_BRADIUS: ball.Radius *= -1; break;
                    case Constants3.NEGATIVE_HRADIUS: hole.Radius *= -1; break;
                    case Constants3.NEGATIVE_FRICTION: friction *= -1; break;
                }
            } while ((x = IsSolvable()) != Constants3.SOLVABLE_EXPERIMENT);
        }


        #region Omar's Methods


        public override void Update(GameTime gameTime)
        {
            Tools3.update_MeasuringVelocityAndAngle(user);
            checkCollision();
            shoot();
            base.Update(gameTime);
        }

        public override void Initialize()
        {
            hasCollidedWithBall = false;
            ballShot = false;
            assumedLegMass = user.AssumedLegMass;
            base.Initialize();
        }


        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public void shoot()
        {
            Vector3 initialLegVelocity; //This variable represents the velocity of the leg with which the user has shot the ball.
            initialLegVelocity = new Vector3((float)(user.Velocity * Math.Cos(user.Angle)), 0, -(float)(user.Velocity * Math.Sin(user.Angle)));
            if (hasCollidedWithBall && !ballShot)
            {
                ballMass = ball.Mass; //get the mass of the ball
                Vector3 velocityAfterCollision = getVelocityAfterCollision(initialLegVelocity); //calculate the velocity of the ball right after the collision
                ball.Velocity = velocityAfterCollision; // update the velocity of the ball
                ballShot = true;
            }
        }

        public void checkCollision()
        {

            Vector3 legPosition; //Current position of leg.
            legPosition = new Vector3((float)user.CurrentRightLegPositionX, 0, (float)user.CurrentRightLegPositionZ);

            if (Math.Abs(Vector3.Subtract(ball.Position, legPosition).Length()) < 150f)
            {
                hasCollidedWithBall = true;
                user.ShootingPosition = legPosition;
            }
            else
                hasCollidedWithBall = false;
        }

        public Vector3 getVelocityAfterCollision(Vector3 initialVelocity)
        {
            double initialVelocityLeg, initialVelocityBall, finalVelocityBall, angle;

            double acceleration = -(friction + wind); //Deceleration of the ball due to resistance.

            //Get the velocity of the ball right before the collision.
            initialVelocityBall = Math.Sqrt((ball.Velocity.LengthSquared() + (2 * acceleration * Math.Abs(Vector3.Distance(ball.Position, user.ShootingPosition)))));
            initialVelocityLeg = initialVelocity.Length();

            //Calculate the angle with which the user has shot the ball.
            angle = Math.Atan2(-initialVelocity.Z, initialVelocity.X);

            //Calculate what will the ball's speed be after collision using conservation of momentum equation.
            finalVelocityBall = ((assumedLegMass * initialVelocityLeg) + (ballMass * initialVelocityBall) - (assumedLegMass * initialVelocityLeg)) / ballMass;

            //Return a vector containing the ball's speed and direction.
            return new Vector3((float)(finalVelocityBall * Math.Cos(angle)), 0, -(float)(finalVelocityBall * Math.Sin(angle)));
        }

        #endregion
    }
}
