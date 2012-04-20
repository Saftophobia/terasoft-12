using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Kinect;
using Mechanect.Common;

namespace Mechanect.Classes
{
    class AvatarprogUI
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MKinect kinect;
        Vector2 vectorp1;
        Vector2 vectorp2;
        Texture2D phototexture;
        Skeleton p1;
        Skeleton p2;
        public AvatarprogUI()
        {
            //kinect = new MKinect();
            //base.Initialize();
            // spriteBatch = new SpriteBatch(GraphicsDevice);
            //phototexture = Content.Load<Texture2D>("ball");
            vectorp1 = new Vector2(700, 25);
            vectorp2 = new Vector2(700, 200);
        }

        protected void Update()
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //  this.Exit();
            p1 = kinect.requestSkeleton();
            p2 = kinect.requestSkeleton();
            if (p1 != null)
            {
                vectorp1.X = p1.Position.Z * 200;
            }
            if (p2 != null)
            {
                vectorp2.X = p2.Position.Z * 200;
            }
            // base.Update(gameTime);
        }

        protected void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(phototexture, vectorp1, new Rectangle(0, 0, 100, 100), Color.White);
            spriteBatch.Draw(phototexture, vectorp2, new Rectangle(0, 0, 100, 100), Color.White);
            spriteBatch.End();
            //base.Draw(gameTime);
        }
    }
}
