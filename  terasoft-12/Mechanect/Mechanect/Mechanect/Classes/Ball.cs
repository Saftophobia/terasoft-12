using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Mechanect.Classes
{
    class Ball
    {
        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set {position = value;}
        }

        private Vector3 velocity;
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        private Vector3 initialVelocity;
        public Vector3 InitialVelocity
        {
            get { return initialVelocity; }
            set { initialVelocity = value; }
        }
        private Vector3 initialBallPosition;
        public Vector3 InitialBallPosition
        {
            get { return initialBallPosition; }
            set { initialBallPosition = value; }
        }
        private int radius;
        public int Radius {
            get 
            {
                return radius;
            }
            set
            {
                radius = value;
            }
        }
        private double mass;
        public double Mass {
            get
            {
                return mass;
            }
            set
            {
                mass = value;
            }
        }

        public float maxMass, minMass;

        public Ball(float minMass, float maxMass)
        {
            this.maxMass = maxMass;
            this.minMass = minMass;
            mass = generateBallMass(minMass,maxMass);
        }

       ///<remarks>
       ///<para>
       ///Author: Cena
       ///</para> 
       /// </remarks>
        /// <summary>
        /// generates a random mass for the ball within a certain given range
        /// </summary>
        /// <param name="min">minimum mass of the ball</param>
        /// <param name="max">maximum mass of the ball</param>
        /// <returns> returns the generated mass</returns>
        public float generateBallMass(float min, float max)
        {
            Random random = new Random();
            float generatedMass = ((float)(random.NextDouble() * (max - min))) + min;
            return generatedMass;
        }
        /// <summary>
        /// Updates the ball position.
        /// </summary>
        /// <param name="friction"> the environment's friction</param>
        public void Update(float friction)
        {
            Position = Vector3.Add(Position,Velocity);
            Velocity = new Vector3(Velocity.X, Velocity.Y, Velocity.Z - friction);
        }



    }
}
