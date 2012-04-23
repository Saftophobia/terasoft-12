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
       
        Vector2 vectorp1;
        Vector2 vectorp2;
        

        /// <summary>
        /// it Initialize the avatarpicture and position on screen
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Safty </para>
        ///<para>DATE WRITTEN: 20/4/12 </para>
        ///<para>DATE MODIFIED: 20/4/12 </para>
        ///</remarks>
        public AvatarprogUI()
        {
            //kinect = new MKinect();
            //base.Initialize();
            // spriteBatch = new SpriteBatch(GraphicsDevice);
            //phototexture = Content.Load<Texture2D>("ball");
            vectorp1 = new Vector2(700, 25);
            vectorp2 = new Vector2(700, 200);
        }

        public void Update(MKinect kinect,Skeleton p1,Skeleton p2)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //  this.Exit();
            p1 = kinect.requestSkeleton();
            p2 = kinect.request2ndSkeleton();
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

        public void Draw(SpriteBatch spriteBatch, Texture2D phototexture)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            //spriteBatch.Begin();
            spriteBatch.Draw(phototexture, vectorp1, new Rectangle(0, 0, 100, 100), Color.White);
            spriteBatch.Draw(phototexture, vectorp2, new Rectangle(0, 0, 100, 100), Color.White);
           // spriteBatch.End();
            //base.Draw(gameTime);
        }
    }
}
