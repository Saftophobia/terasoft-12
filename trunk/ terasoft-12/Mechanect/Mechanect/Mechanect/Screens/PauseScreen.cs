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
        public static int frameNumber = 0;

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
    
        User3 user;
        MKinect kinect;
        VoiceCommands voiceCommands;
        double ballVelocity;
        double ballMass;
        double legMass;
        String displayedGivens;


        SpriteFont font;
        String st;
        int framesToWait;
        double velocity;

           public PauseScreen( User3 user,MKinect kinect,double ballVelocity, double ballMass, double legMass)
        {
            this.user = user;
            this.kinect = kinect;
            this.ballVelocity = ballVelocity;
            this.legMass = legMass;
            this.ballMass = ballMass;
            framesToWait = 0;
            velocity = 0;
            voiceCommands = new VoiceCommands(kinect._KinectDevice,"ready,go");
            fillsPositions = new List<Vector2>();
            fills = new List<Texture2D>();

            st = "";
            displayedGivens = "";
      
            
           

        }

         
          public override void LoadContent()
        {
            viewPort = ScreenManager.GraphicsDevice.Viewport;
            content = ScreenManager.Game.Content;
            spriteBatch = ScreenManager.SpriteBatch;
             

          
            font = content.Load<SpriteFont>("spriteFont1");
           
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
           
           
            if (!voiceCommands.getHeared("go"))
            {
                if (frameNumber != -1)
                {
                   
                    user.Update_MeasuringVelocityAndAngle();
                    // truncate max velocity
                    velocity = user.SetVelocityRelativeToGivenMass();

                    for (int i = fills.Count()-1; i < user.Velocity; i++)
                    {
                        fillsPositions.Add(fillPosition);
                        fills.Add(content.Load<Texture2D>("Textures/Vfill"));
                        fillPosition.Y -= 8;
                    }
                    arrowAngle = (float)user.Angle;
                }
                else
                {
                    st = "   " + velocity + " m/s " +'\n' + user.Angle + " degrees";
                    user.Velocity = velocity;

                    if (framesToWait > 300) // after 5 seconds
                    {

                        fillsPositions.Clear();
                        fills.Clear();
                        fillPosition = new Vector2(velocityBar.Width / 2 + 20, viewPort.Height - (7 / 2));
                        arrowAngle = 0;
                        framesToWait = 0;
                        user.ResetUserForShootingOrTryingAgain();
                        
                    }
                    else
                        framesToWait++;
                }
            }
            else
            {
             
                user.ResetUserForShootingOrTryingAgain();
                ExitScreen();
            }


       
            base.Update(gameTime, covered);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            spriteBatch.Begin();
            spriteBatch.Draw(givens,givensPosition,null,Color.White,0,new Vector2(givens.Width/2,givens.Height/2),1f,SpriteEffects.None,0);
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

            spriteBatch.Begin();
            spriteBatch.DrawString(font, st, new Vector2(0, 0), Color.Red);
            spriteBatch.End();

            spriteBatch.Begin();
            displayedGivens = "Ball Mass: " + ballMass + '\n' + "Ball Velocity: " + ballVelocity + '\n' + "Leg Mass: " + legMass;
            spriteBatch.DrawString(font, displayedGivens, new Vector2(viewPort.Width/2, givens.Height/4), Color.Salmon);
            spriteBatch.End();
        
            
        }



        public override void Remove()
        {
            base.Remove();
        }
    }
    
}
