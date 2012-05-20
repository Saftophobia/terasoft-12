using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using UI.Cameras;
using Mechanect.Exp3;
using Microsoft.Kinect;
using Mechanect.Common;
using Mechanect.Exp3;
using ButtonsAndSliders;

namespace Mechanect.Screens
{
    class PauseScreen : Mechanect.Common.GameScreen
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

        User3 user;
        MKinect kinect;
        VoiceCommands voiceCommands;
        double ballVelocity;
        double ballMass;
        double legMass;
        String displayedGivens;


        SpriteFont font;
        int framesToWait;
        Vector3 velocity;


        string count;
        Vector2 countPosition;
        Color countColor;
        SpriteFont countFont;
        float countScale;

        Button button;
        Texture2D ok;

        public PauseScreen(User3 user, MKinect kinect, double ballVelocity, double ballMass, double legMass)
        {
            this.user = user;
            this.kinect = kinect;
            this.ballVelocity = ballVelocity;
            this.legMass = legMass;
            this.ballMass = ballMass;
            framesToWait = 0;
            velocity = Vector3.Zero;
            voiceCommands = new VoiceCommands(kinect._KinectDevice, "ready,go");
            fillsPositions = new List<Vector2>();
            fills = new List<Texture2D>();
            displayedGivens = "";
            frameNumber = 0;
            count = "";
            countScale = 1;
            countColor = Color.Red;
            


        }


        public override void LoadContent()
        {
            viewPort = ScreenManager.GraphicsDevice.Viewport;
            content = ScreenManager.Game.Content;
            spriteBatch = ScreenManager.SpriteBatch;



            font = content.Load<SpriteFont>("SpriteFont1");
            countFont = content.Load<SpriteFont>("SpriteFont2");
            velocityBar = content.Load<Texture2D>("Textures/VBar");
            vBarPosition = new Vector2((velocityBar.Width / 2) + 20, viewPort.Height - (velocityBar.Height / 2));
            fillPosition = new Vector2(velocityBar.Width / 2 + 20, viewPort.Height - (7 / 2));

            givens = content.Load<Texture2D>("Textures/screen");
            givensPosition = new Vector2(viewPort.Width / 2, givens.Height / 4);


            arrow = content.Load<Texture2D>("Textures/arrow");
            arrowScale = 0.2f;
            arrowPosition = new Vector2(viewPort.Width - (float)((Math.Sqrt(arrowScale) * arrow.Width)), viewPort.Height / 2 + (float)((Math.Sqrt(arrowScale) * arrow.Height / 2)));
            arrowAngle = 0;

            countPosition = new Vector2(viewPort.Width / 2, viewPort.Height / 2);


            button = Tools3.OKButton(content, new Vector2(viewPort.Width - 255, 0), viewPort.Width, viewPort.Height, user);
            



        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {

            button.Update(gameTime);
            if (!voiceCommands.getHeared("go") && !button.IsClicked())
            {
                if (!user.HasShot()&&!user.hasMissed)
                {

                    user.UpdateMeasuringVelocityAndAngle(gameTime);
                    // truncate max velocity
                    velocity = user.SetVelocityRelativeToGivenMass();

                    int draw;
                    if (velocity.Length() > 31)
                        draw = 31;
                    else
                        draw = (int)velocity.Length();

                    for (int i = fills.Count() - 1; i < draw; i++)
                    {
                        fillsPositions.Add(fillPosition);
                        fills.Add(content.Load<Texture2D>("Textures/Vfill"));
                        fillPosition.Y -= 8;
                    }
                    arrowAngle = (float)user.Angle;
                    displayedGivens = "Ball Mass: " + ballMass + '\n' + "Ball Velocity: " + ballVelocity + '\n' + "Leg Mass: "
                     + (Math.Truncate(legMass * 1000) / 1000);
                }
                else
                {

                    user.velocity = velocity;
                    displayedGivens = "Ball Mass: " + ballMass + '\n' + "Ball Velocity: " + ballVelocity + '\n' + "Leg Mass: "
                        + Math.Truncate(legMass * 1000) / 1000 +'\n' + "Shooting velocity: " + Math.Truncate(velocity.Length() * 1000) / 1000 + " m/s "
                        + '\n' + "Shooting angle: " + Math.Truncate((user.Angle * 180 / Math.PI) * 1000) / 1000 + " deg";
                    if (user.hasMissed)
                        Clear();
                    else
                    {
                        if (framesToWait > 240) // after 4 seconds
                            Clear();

                        else
                        {
                            if (framesToWait >= 0 && framesToWait <= 60)
                                count = "3";
                            if (framesToWait > 60 && framesToWait <= 120)
                                count = "2";
                            if (framesToWait > 120 && framesToWait <= 180)
                                count = "1";
                            if (framesToWait > 180 && framesToWait <= 240)
                            {
                                count = "Try Again";
                                countColor = Color.DarkGreen;
                                countPosition = new Vector2(viewPort.Width / 8, (1 * viewPort.Height) / 3);
                                countScale = 0.8f;
                            }
                            framesToWait++;
                        }
                    }
                }
            }
            else
            {

                user.ResetUserForShootingOrTryingAgain();
                user.Trying = false;
                ExitScreen();
            }



            base.Update(gameTime, covered);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {


           

            spriteBatch.Begin();
            button.Draw(spriteBatch);
            spriteBatch.Draw(givens, givensPosition, null, Color.White, 0, new Vector2(givens.Width / 2, givens.Height / 2), 0.7f, SpriteEffects.None, 0);
            spriteBatch.Draw(velocityBar, vBarPosition, null, Color.White, 0, new Vector2(velocityBar.Width / 2, velocityBar.Height / 2), 1f, SpriteEffects.None, 0);
            for (int i = 0; i < fills.Count; i++)
                spriteBatch.Draw(fills.ElementAt<Texture2D>(i), fillsPositions.ElementAt<Vector2>(i), null,
                    Color.White, 0, new Vector2(fills.ElementAt<Texture2D>(i).Width / 2,
                        fills.ElementAt<Texture2D>(i).Height / 2), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(arrow, arrowPosition, null, Color.White, arrowAngle, new Vector2((arrow.Width) / 2, (arrow.Height) / 2), arrowScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, displayedGivens, new Vector2(viewPort.Width / 6, givens.Height / 30), Color.Black);
            spriteBatch.DrawString(countFont, count, countPosition, countColor, 0, Vector2.Zero, countScale, SpriteEffects.None, 0);
            button.DrawHand(spriteBatch);

            spriteBatch.End();

            


        }

        public void Clear()
        {
            fillsPositions.Clear();
            fills.Clear();
            fillPosition = new Vector2(velocityBar.Width / 2 + 20, viewPort.Height - (7 / 2));
            arrowAngle = 0;
            framesToWait = 0;
            user.ResetUserForShootingOrTryingAgain();
            count = "";
            countColor = Color.Red;
            countPosition = new Vector2(viewPort.Width / 2, viewPort.Height / 2);
            countScale = 1;
        }


        public override void Remove()
        {
            base.Remove();
        }
    }

}
