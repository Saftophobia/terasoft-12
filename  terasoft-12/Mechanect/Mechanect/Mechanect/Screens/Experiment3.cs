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
    class Experiment3 : Mechanect.Common.GameScreen

    {
        Environment3 environment;
        GraphicsDevice graphics;
        Camera c;

        ResultSimulation sim;

           public Experiment3()
        {
          
        }
           public override void LoadContent()
        {


            graphics = this.ScreenManager.GraphicsDevice;

            c = new TargetCamera(new Vector3(0, 100, 100), Vector3.Zero, graphics);
            
            environment = new Environment3(ScreenManager.SpriteBatch,ScreenManager.Game.Content,ScreenManager.GraphicsDevice);

            environment.InitializeEnvironment(ScreenManager.GraphicsDevice);
            environment.LoadEnvironmentContent();

            sim = new ResultSimulation(graphics, ScreenManager.SpriteBatch, ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1"), Color.Black, environment.ball.ballModel, new Vector3(5, 0 ,0 ), new Vector3(3, 0, 0), new Vector3(-1, 0, 0));

        }

        public override void UnloadContent()
        {
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            c.Update();
            //environment.ball.Update(environment.friction);
            sim.Update(gameTime);
            base.Update(gameTime, covered);
            //environment.UpdateEnvironment(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            environment.DrawEnvironment(sim.camera, gameTime);
            environment.ball.Draw(gameTime, sim.camera);
        }


       

        public override void Remove()
        {
            
            base.Remove();
        }
    }
        
    
}
