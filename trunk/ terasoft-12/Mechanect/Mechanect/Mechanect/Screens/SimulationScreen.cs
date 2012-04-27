using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Mechanect.Classes;
using Mechanect.Common;
using Mechanect.Cameras;



namespace Mechanect.Screens
{
     class SimulationScreen : Mechanect.Common.GameScreen

    {
        Environment3 environment;
        GraphicsDevice graphics;

        ResultSimulation sim;

        Vector3 shootVelocity = new Vector3(-20, 0, -10);
        Vector3 optimalVelocity = new Vector3(-15, 0, -20);
        float friction = 1f;

        public SimulationScreen(Environment3 environment)
        {

        }
           public override void LoadContent()
        {

            graphics = this.ScreenManager.GraphicsDevice;
            environment = new Environment3(ScreenManager.SpriteBatch, ScreenManager.Game.Content, ScreenManager.GraphicsDevice, new User3());

            environment.InitializeEnvironment(ScreenManager.GraphicsDevice);
            environment.LoadEnvironmentContent();

           
            environment.ball.ballModel.Position = new Vector3(50, 10, 50);
            sim = new ResultSimulation(graphics, ScreenManager.SpriteBatch, ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1"), Color.Black, environment.ball.ballModel, shootVelocity, optimalVelocity, friction);
        }

        public override void UnloadContent()
        {
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            
            sim.Update(gameTime);
            if (sim.SimulationFinished)
            {
                ScreenManager.AddScreen(new LastScreen(environment.user, 3));
                ExitScreen();
            }
            base.Update(gameTime, covered);

        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            environment.DrawEnvironment(sim.Camera, gameTime);
            environment.ball.Draw(gameTime, sim.Camera);
        }


       

        public override void Remove()
        {
            
            base.Remove();
        }
    }
        
    
}
