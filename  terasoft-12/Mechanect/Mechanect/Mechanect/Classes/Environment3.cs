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


        /// <summary>
        /// This method verifies wether a method is solvable or not
        /// </summary>
        /// <returns>Retuns an int that represents the type of the problem with the experiment</returns>
        public int IsSolvable()
        {

            if (ball.Radius <= 0)
                return Constants3.negativeBRradius;
            if (ball.Mass <= 0)
                return Constants3.negativeBMass;
            if (hole.Radius <= 0)
                return Constants3.negativeHRadius;
            if (user.AssumedLegMass <= 0)
                return Constants3.negativeLMass;
            //hole position not before the leg position
            if (hole.Position.Z - user.ShootingPosition.Z > 0)
                return Constants3.negativeHPosZ;
            if (friction <= 0)
                return Constants3.negativeFriction;
            if (ball.Radius > hole.Radius)
                return Constants3.negativeRDifference;

            /*
             * Enviornment e;
             * e.getFinalPosition(e.getVelocityAfterCollision(new Vector2(Constants3.MAX_VELOCITYX, Constants3.MAX_VELOCITYY,
             * ballMass,AssumedLegMass,wind));//implement with hasScored();
            */

            Vector3 finalPos = Vector3.Zero;//to be changed with the position returned from getFinalPostion();
            if (Vector3.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector3.Subtract(hole.Position, user.ShootingPosition).LengthSquared())
                return Constants3.holeOutOfFarRange;
            /*
             * Enviornment e;
             * e.getFinalPosition(e.getVelocityAfterCollision(new Vector2(Constants3.MIN_VELOCITYX, Constants3.MIN_VELOCITYY,
             * ballMass, AssumedLegMass, wind, friction));
            */

            if (Vector3.Subtract(finalPos, user.ShootingPosition).LengthSquared() > Vector3.Subtract(hole.Position, user.ShootingPosition).LengthSquared()) //length squared used for better performance than length
                return Constants3.holeOutOfNearRange;

            return Constants3.solvableExperiment;//means solvable
        }

        /// <summary>
        /// Generates a solvable experiment
        /// </summary>
        public void GenerateSolvable()
        {
            int x = Constants3.solvableExperiment;
            do
            {
                x = IsSolvable();
                switch (x)
                {
                    case Constants3.holeOutOfNearRange: if (hole.Position.X == Constants3.maxHolePosX && hole.Position.Z != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Add(hole.Position, new Vector3(0, 0, -1)));
                        else if (hole.Position.Z == Constants3.maxHolePosX && hole.Position.X != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Add(hole.Position, new Vector3(1, 0, 0)));
                        else friction++; break;
                    case Constants3.holeOutOfFarRange: if (hole.Position.X == Constants3.maxHolePosX && hole.Position.Z != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Subtract(hole.Position, new Vector3(0, 0, -1)));
                        else if (hole.Position.Y == Constants3.maxHolePosX && hole.Position.X != Constants3.maxHolePosZ)
                            hole.Position = (Vector3.Subtract(hole.Position, new Vector3(1, 0, 0)));
                        else if (wind != 0)
                            wind--;
                        else if (friction != 1)
                            friction--;
                        else if (friction <= 1)
                            friction -= 0.1f;
                        break;
                    case Constants3.negativeRDifference: int tmp = ball.Radius; ball.Radius = (hole.Radius); hole.Radius = (tmp); break;
                    case Constants3.negativeLMass: user.AssumedLegMass *= 1; break;
                    case Constants3.negativeBMass: ball.Mass *= -1; break;
                    case Constants3.negativeBRradius: ball.Radius *= -1; break;
                    case Constants3.negativeHRadius: hole.Radius *= -1; break;
                    case Constants3.negativeFriction: friction *= -1; break;
                }
            } while ((x = IsSolvable()) != Constants3.solvableExperiment);
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
