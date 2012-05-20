using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Exp1;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Mechanect.Screens.Exp1Screens
{
    class GraphScreen:Mechanect.Common.GameScreen
    {
        User1 user1;
        User1 user2;
        Viewport ViewPort
        {
            get
            {
                return ScreenManager.GraphicsDevice.Viewport;
            }
        }
        ContentManager Content
        {
            get
            {
                return ScreenManager.Game.Content;
            }
        }
        SpriteBatch SpriteBatch
        {
            get
            {
                return ScreenManager.SpriteBatch;
            }
        }
        public GraphScreen(User1 user1, User1 user2)
        {
            this.user1 = user1;
            this.user2 = user2;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }


    }
}
