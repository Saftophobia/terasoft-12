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
        User3 user;
        bool freezeLock;
    
        //ResultSimulation sim;

           public Experiment3(User user)
        {
            this.user = (User3)user;
          
            freezeLock = false;
          
        }
           public override void LoadContent()
        {


            graphics = this.ScreenManager.GraphicsDevice;

            c = new TargetCamera(new Vector3(0, 80, 120), Vector3.Zero, graphics);
            
            environment = new Environment3(ScreenManager.SpriteBatch,ScreenManager.Game.Content,ScreenManager.GraphicsDevice,(User3)user);
            environment.GenerateSolvable();
            environment.InitializeEnvironment(ScreenManager.GraphicsDevice);
            environment.LoadEnvironmentContent();

            //sim = new ResultSimulation(graphics, ScreenManager.SpriteBatch, ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1"), Color.Black, environment.ball.ballModel, new Vector3(5, 0 ,0 ), new Vector3(3, 0, 0), new Vector3(-1, 0, 0));

        }

        public override void UnloadContent()
        {
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            Vector3 position = environment.ball.Position;
            c.Update();
            if (environment.ball.Position.X >= environment.user.ShootingPosition.X && environment.ball.Position.Z >= environment.user.ShootingPosition.Z)
            {
                environment.Update();

            }
            environment.ball.Update(environment.Friction);
            
            environment.DistanceBar.Update(new Vector2(position.X,position.Z));
            //sim.Update(gameTime);
            Bar bar = environment.DistanceBar;
            if ((Vector2.Distance(bar.InitialPos, bar.CurrentPos) / Vector2.Distance(bar.InitialPos, bar.ShootingPos)) > 0.5 && !freezeLock)
            {
                freezeLock = true;
                this.FreezeScreen();
                ScreenManager.AddScreen(new PauseScreen(user, user.Kinect, environment.ball.Velocity.Length(), environment.ball.Mass, ((User3)user).AssumedLegMass));
            }

            //if (environment.ball.Position.X >= user.ShootingPosition.X && environment.ball.Position.Z >= user.ShootingPosition.Z)
            //{
            //    environment.Update();

            //}
            
            base.Update(gameTime, covered);
            //environment.UpdateEnvironment(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            environment.DrawEnvironment(c, gameTime);
            environment.distanceBar.Draw();
            environment.ball.Draw(gameTime, c);
            ScreenManager.SpriteBatch.End();
        }

        private void StartSimulation()
        {
            ScreenManager.AddScreen(new SimulationScreen(environment));
            ExitScreen();
        }
       

        public override void Remove()
        {
            
            base.Remove();
        }
    }
        
    
}
