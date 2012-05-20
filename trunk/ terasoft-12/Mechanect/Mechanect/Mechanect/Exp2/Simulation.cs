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

        public Simulation(Vector2 predatorPosition, Rectangle preyPosition, Rectangle aquariumPosition, float velocity, float angle)
        {
            environment = new Environment2(predatorPosition, preyPosition, aquariumPosition);
            initialPredatorVelocity = velocity * 
                new Vector2((float)Math.Cos(MathHelper.ToRadians(angle)), 
                    (float)Math.Sin(MathHelper.ToRadians(angle)));
            initialPredatorPosition = predatorPosition;
            simulationRunning = false;
            predatorInitialized = false;
        }

        public void LoadContent(ContentManager contentManager, Viewport viewport)
        {
            font = contentManager.Load<SpriteFont>("Ariel");
            //environment.LoadContent(contentManager, viewport);
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
                    predatorInitialized = true;
                }
            }
            else
            {
                //environment.Update(gameTime);
            }
        }

        public void Draw(Rectangle rectangle, ContentManager contentManager, SpriteBatch spriteBatch, Viewport viewport)
        {
            //environment.Draw(new Rectangle(rectangle.X, rectangle.Y + 50, rectangle.Width, rectangle.Height), contentManager, spriteBatch, viewport);
            string data = "Velocity = " + Math.Round(initialPredatorVelocity.Length(), 2) + ", Angle = " 
                + Math.Round(MathHelper.ToDegrees((float)Math.Tan(initialPredatorVelocity.Y / initialPredatorVelocity.X)), 2);
            spriteBatch.DrawString(font, data, new Vector2(rectangle.X, rectangle.Y), Color.Black);
        }
    }
}
