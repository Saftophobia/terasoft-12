using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Mechanect.Classes;



namespace Mechanect.Screens
{
    class Experiment3 : Mechanect.Common.GameScreen

    {
        Environment3 environment;
        GraphicsDevice graphics;

           public Experiment3(GraphicsDevice g)
        {
            graphics = g;
          environment.InitializeEnvironment(g);
        }
           public override void LoadContent()
        {
            environment = new Environment3(ScreenManager.SpriteBatch);
            environment.LoadEnvironmentContent(ScreenManager.Game.Content);
           


        }

        public override void UnloadContent()
        {
           
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
         
            base.Update(gameTime, covered);
            environment.UpdateEnvironment(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            environment.DrawEnvironment(gameTime);
        }


       

        public override void Remove()
        {
            
            base.Remove();
        }
    }
        
    
}
