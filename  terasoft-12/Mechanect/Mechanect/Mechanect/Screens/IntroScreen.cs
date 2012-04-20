using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Screens
{
    class IntroScreen:Mechanect.Common.GameScreen
    {
             public IntroScreen()
        {
        }

          public override void LoadContent()
        {
          
        }

        public override void UnloadContent()
        {
         
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            base.Update(gameTime, covered);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }



        public override void Remove()
        {
            base.Remove();
        }
    
    }
}
