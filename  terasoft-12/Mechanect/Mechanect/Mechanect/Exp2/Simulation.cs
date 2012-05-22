using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Exp2
{
    class Simulation
    {
        private Environment2 environment;
        private Vector2 initialPredatorVelocity;
        private Vector2 initialPredatorPosition;
        private bool predatorInitialized;
        private bool simulationRunning;
        private float milliseconds;
        private SpriteFont font;
        private float velocity;
        private float angle;

        public Simulation(Vector2 predatorPosition, Rectangle preyPosition, Rectangle aquariumPosition, float velocity, float angle)
        {
            environment = new Environment2(predatorPosition, preyPosition, aquariumPosition);
            initialPredatorVelocity = velocity * 
                new Vector2((float)Math.Cos(MathHelper.ToRadians(angle)), 
                    (float)Math.Sin(MathHelper.ToRadians(angle)));
            initialPredatorPosition = predatorPosition;
            simulationRunning = false;
            predatorInitialized = false;
            this.velocity = velocity;
            this.angle = angle;
            
        }

        public void LoadContent(ContentManager contentManager, Viewport viewport, GraphicsDevice graphicsDevice)
        {
            font = contentManager.Load<SpriteFont>("Ariel");
            environment.LoadContent(contentManager, graphicsDevice, viewport);
            
        }

        public void Update(GameTime gameTime)
        {
            if (!simulationRunning)
            {
                milliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (milliseconds > 2000)
                {
                    milliseconds = 0;
                    simulationRunning = true;
                }
                else if (milliseconds > 1000)
                {
                    environment.Predator.Location = initialPredatorPosition;
                    environment.Predator.Velocity = initialPredatorVelocity;
                    environment.Predator.Movable = true;
                    environment.Prey.Eaten = false;
                    predatorInitialized = true;
                }
            }
            else
            {
                simulationRunning = environment.Update(gameTime);
            }
           
        }

        public void Draw(Rectangle rectangle, ContentManager contentManager, SpriteBatch spriteBatch, Viewport viewport)
        {
            environment.Draw(rectangle,spriteBatch);
            string data = "Velocity = " + velocity +  ", Angle = " + angle;
            spriteBatch.DrawString(font, data, new Vector2(rectangle.X, rectangle.Y), Color.Black);
        }
    }
}
