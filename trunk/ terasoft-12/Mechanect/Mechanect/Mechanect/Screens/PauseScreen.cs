using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Mechanect.Cameras;
using Mechanect.Classes;
using Microsoft.Kinect;

namespace Mechanect.Screens
{
    class PauseScreen:Mechanect.Common.GameScreen
    {
        ContentManager content;
        Viewport viewPort;
        SpriteBatch spriteBatch;
        
        Texture2D givens;
        Vector2 givensPosition;


        Texture2D velocityBar;
        Vector2 vBarPosition;

      
        Vector2 fillPosition;
        List<Vector2> fillsPositions;
        List<Texture2D> fills;


        Texture2D arrow;
        Vector2 arrowPosition;
        float arrowAngle;
        float arrowScale;
    
        User user;
        MKinect kinect;
        Boolean hasNotWaited;
        VoiceCommands voiceCommands;
           public PauseScreen(User user,MKinect kinect)
        {

            this.user = user;
            this.kinect = kinect;
            hasNotWaited = true;
            voiceCommands = new VoiceCommands(kinect._KinectDevice);
            
           

        }

         
          public override void LoadContent()
        {
            viewPort = ScreenManager.GraphicsDevice.Viewport;
            content = ScreenManager.Game.Content;
            spriteBatch = ScreenManager.SpriteBatch;
          

           
            velocityBar = content.Load<Texture2D>("Textures/VBar");
            vBarPosition = new Vector2((velocityBar.Width / 2) + 20, viewPort.Height - (velocityBar.Height / 2));
            fillPosition = new Vector2(velocityBar.Width / 2 + 20, viewPort.Height - (7 / 2));

            givens = content.Load<Texture2D>("Textures/screen");
            givensPosition = new Vector2(viewPort.Width/2 , givens.Height/4);


            arrow = content.Load<Texture2D>("Textures/arrow");
            arrowScale = 0.3f;
            arrowPosition = new Vector2(viewPort.Width - (float)((Math.Sqrt(arrowScale) * arrow.Width)), viewPort.Height / 2 + (float)((Math.Sqrt(arrowScale) * arrow.Height / 2)));
            arrowAngle = 0;
            
        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
             TimeSpan elapsedTime = TimeSpan.Zero;
            if (!voiceCommands.getHeared("go"))
            {
                if (Tools3.frameNumber != -1)
                {
                   
                    Tools3.update_MeasuringVelocityAndAngle(user);
                    Tools3.setVelocityRelativeToGivenMass(user);
                    Vector2 velocityInMeters = Tools3.resolveUserVelocity(user);
                    SkeletonPoint p = new SkeletonPoint();
                    p.X = velocityInMeters.X;
                    p.Z = velocityInMeters.Y;
                    Vector2 velocityInPixels = Tools3.getPointsOnScreen(kinect, p, viewPort.Width, viewPort.Height);
                    double velocity = Math.Sqrt(Math.Pow(velocityInPixels.X, 2) + Math.Pow(velocityInPixels.Y, 2));
                    for (int i = fills.Count-1; i < velocity; i++)
                    {
                        fillsPositions.Add(fillPosition);
                        fills.Add(content.Load<Texture2D>("Vfill"));
                        fillPosition.Y -= 8;
                    }
                    arrowAngle = (float)user.Angle;
                }
                else
                {
                    if (hasNotWaited)
                    {
                        elapsedTime = gameTime.TotalGameTime;
                        hasNotWaited = false;
                    }
                    if (elapsedTime.TotalSeconds - gameTime.TotalGameTime.TotalSeconds > 10)
                    {
                        Tools3.frameNumber = 0;
                        hasNotWaited = true;
                    }
                }
            }
            else
            {
                Tools3.resetUserForShootingOrTryingAgain(user);
                ExitScreen();
            }


       
            base.Update(gameTime, covered);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(givens,givensPosition,null,Color.White,0,new Vector2(givens.Width/2,givens.Height/2),0.5f,SpriteEffects.None,0);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.Draw(velocityBar, vBarPosition, null, Color.White, 0, new Vector2(velocityBar.Width / 2, velocityBar.Height / 2), 1f, SpriteEffects.None, 0);
            spriteBatch.End();


            for (int i = 0; i < fills.Count; i++)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(fills.ElementAt<Texture2D>(i), fillsPositions.ElementAt<Vector2>(i), null,
                    Color.White, 0, new Vector2(fills.ElementAt<Texture2D>(i).Width / 2,
                        fills.ElementAt<Texture2D>(i).Height / 2), 1, SpriteEffects.None, 0);
                spriteBatch.End();
            }

            spriteBatch.Begin();
            spriteBatch.Draw(arrow, arrowPosition, null, Color.White, arrowAngle, new Vector2((arrow.Width) / 2, (arrow.Height) / 2), arrowScale, SpriteEffects.None, 0);
            spriteBatch.End();
        
            
        }



        public override void Remove()
        {
            base.Remove();
        }
    }
    
}
