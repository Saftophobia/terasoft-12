using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mechanect.Classes;

namespace Mechanect.Screens
{
    class Experimentnew1 : Mechanect.Common.GameScreen
    {

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


        Environment1 Environment1;
        GraphicsDevice graphics;
        User1 user1, user2;

        public Experimentnew1(User1 user1, User1 user2)
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
            graphics = this.ScreenManager.GraphicsDevice;
            Environment1 = new Environment1(ScreenManager.Game.Content, ScreenManager.Game.GraphicsDevice);
            Environment1.LoadContent();
        }
        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            Environment1.update(gameTime);

            base.Update(gameTime, covered);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Environment1.Draw(gameTime);
        }

    }
}
