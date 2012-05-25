using System;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Exp2
{

    /// <summary>
    /// Displays a Simulation of an environment with given locations and velocity
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: Mohamed AbdelAzim</para>
    /// </remarks>
    class Simulation
    {
        private Environment2 environment;
        private Vector2 initialPredatorVelocity;
        private Vector2 initialPredatorPosition;
        private bool simulationRunning;
        private float milliseconds;
        private SpriteFont font;
        private float velocity;
        private float angle;
        private ContentManager contentManager;

        /// <summary>
        /// Generates the Simulation that displays the environment during the projectile motion
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="predatorPosition">The initial position of the predator</param>
        /// <param name ="preyPosition">The position of the prey</param>
        /// <param name ="aquariumPosition">The position of the aquarium</param>
        /// <param name ="velocity">The magnitude of velocity of the projectile</param>
        /// <param name ="angle">The degree angle of the projectile</param>
        public Simulation(Vector2 predatorPosition, Rect preyPosition, Rect aquariumPosition, float velocity, float angle)
        {
            environment = new Environment2(predatorPosition, preyPosition, aquariumPosition);
            initialPredatorVelocity = velocity *
                new Vector2((float)Math.Cos(MathHelper.ToRadians(angle)),(float)Math.Sin(MathHelper.ToRadians(angle)));
            initialPredatorPosition = predatorPosition;
            simulationRunning = false;
            this.velocity = velocity;
            this.angle = angle;

        }

        /// <summary>
        /// Loads the textures of the simulation
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="contentManager">The content manager of the screen manager</param>
        /// <param name ="graphicsDevice">The graphics device of the screen manager</param>
        public void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            font = contentManager.Load<SpriteFont>("Ariel");
            environment.LoadContent(contentManager, graphicsDevice,graphicsDevice.Viewport);
            
        }


        /// <summary>
        /// Updates the simulation to generate different positions of predator with time
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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
                }
            }
            else
            {
                simulationRunning = environment.Update(gameTime);
            }
           
        }

        /// <summary>
        /// Draws the simulation in the specified Rectangle.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name ="rectangle">The rectangle that the simulation will be drawn in.</param>
        /// <param name ="spriteBatch">The sprite batch of the screen manager.</param>
        public void Draw(Rectangle rectangle, SpriteBatch spriteBatch)
        {
            string data = "     Velocity = " + velocity + ", Angle = " + angle;
            UI.UILib.Write(data, rectangle, spriteBatch, font, Color.DarkRed);
            if (rectangle.Height > 25)
                environment.Draw(new Rectangle(rectangle.X, rectangle.Y + 25, rectangle.Width, rectangle.Height - 25), spriteBatch);
        }
    }
}
