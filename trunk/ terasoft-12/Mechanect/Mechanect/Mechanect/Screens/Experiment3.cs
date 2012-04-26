using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Mechanect.Classes;
using Mechanect.Cameras;


namespace Mechanect.Screens
{
    class Experiment3 : Mechanect.Common.GameScreen

    {
        Environment3 environment;
        GraphicsDevice graphics;
        Camera c;

           public Experiment3()
        {
          
        }
           public override void LoadContent()
        {

            graphics = this.ScreenManager.GraphicsDevice;

            c = new TargetCamera(new Vector3(0, 100, 100), Vector3.Zero, graphics);
            environment = new Environment3(ScreenManager.SpriteBatch);
            environment.InitializeEnvironment(ScreenManager.GraphicsDevice);
            environment.LoadEnvironmentContent(ScreenManager.Game.Content);
           


        }

        public override void UnloadContent()
        {
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            c.Update();   
            base.Update(gameTime, covered);
            //environment.UpdateEnvironment(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            environment.DrawEnvironment(c, gameTime);
        }


       

        public override void Remove()
        {
            
            base.Remove();
        }
    }
        
    
}
