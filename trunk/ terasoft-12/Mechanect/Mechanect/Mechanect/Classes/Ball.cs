using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Mechanect.Common;
using Mechanect.Cameras;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Classes
{
    class Ball
    {
        public CustomModel ballModel;

        ContentManager content;
        GraphicsDevice device;
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

        private float maxMass;
           public float MaxMass
           {
               get { 
                   return maxMass; 
               }
               set
               {
                   maxMass = value;
               }
           }

           private float minMass;
           public float MinMass
           {
               get
               {
                   return minMass;
               }
               set
               {
                   minMass = value;
               }
           }

        public Ball(float minMass, float maxMass, GraphicsDevice device, ContentManager content)
        {
            this.maxMass = maxMass;
            this.minMass = minMass;
            mass = GenerateBallMass();
            this.device = device;
           this.content = content;
        
            
       

        }

        public void LoadContent()
        {
            ballModel = new CustomModel(content.Load<Model>(@"Models/ball"), initialBallPosition, Vector3.Zero, new Vector3(0.02f), device);
        }

       ///<remarks>
       ///<para>
       ///Author: Cena
       ///</para> 
       /// </remarks>
        /// <summary>
        /// generates a random mass for the ball within a certain given range
        /// </summary>
        /// <returns> returns the generated mass</returns>
        public float GenerateBallMass()
        {
            Random random = new Random();
            float generatedMass = ((float)(random.NextDouble() * (MaxMass - MinMass))) + MinMass;
            return generatedMass;
        }


        /// <summary>
        /// Updates the ball position.
        /// </summary>
        /// <param name="friction"> the environment's friction</param>
        public void Update(float friction)
        {
            
            
            //if (Velocity.Z >= 0)
            //    Velocity = new Vector3(Velocity.X, Velocity.Y, 0);
            if((InitialVelocity.X>0) && (Velocity.X<=0))
                Velocity = new Vector3(0, Velocity.Y, Velocity.Z);
            if ((InitialVelocity.X < 0) && (Velocity.X >= 0))
                Velocity = new Vector3(0, Velocity.Y, Velocity.Z);


            Position = Vector3.Add(Position,Velocity);
            Velocity = new Vector3(Velocity.X , Velocity.Y, Velocity.Z/* + friction*/);
            if(InitialVelocity.X>0)
                Velocity = new Vector3(Velocity.X/* - friction*/, Velocity.Y, Velocity.Z);
            else if(InitialVelocity.Z <0)
                Velocity = new Vector3(Velocity.X/* + friction*/, Velocity.Y, Velocity.Z);


            ballModel.Position = Position;



        }

        
    
        public void Draw(GameTime gameTime, Camera camera)
        {
            ballModel.Draw(camera);
        }


    }
}
