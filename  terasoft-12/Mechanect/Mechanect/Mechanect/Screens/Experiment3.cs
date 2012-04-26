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

        ModelLinearAnimation animation;

        Camera c;


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
            animation = new ModelLinearAnimation(environment.ball.ballModel, environment.ball.InitialVelocity, environment.friction, TimeSpan.FromSeconds(10),  true);


        }

        public override void UnloadContent()
        {
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            c.Update();
            environment.ball.Update(environment.friction);
            animation.Update(gameTime.ElapsedGameTime);
            base.Update(gameTime, covered);
            //environment.UpdateEnvironment(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            environment.DrawEnvironment(c, gameTime);
            environment.ball.Draw(gameTime, c);
        }


       

        public override void Remove()
        {
            
            base.Remove();
        }
    }
        
    
}
