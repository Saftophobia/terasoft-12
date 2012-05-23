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
using ButtonsAndSliders;

namespace Mechanect.Screens
{
    class PauseScreen : Mechanect.Common.GameScreen
    {
        #region Cena'sMethods
        #region InstanceVariables
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
        string missed;
        Vector2 missedPosition;
        SpriteFont font2;
        Vector3 holePosition;
        #endregion
        #region Constructor
        public PauseScreen(User3 user, double ballVelocity, double ballMass, double legMass, Vector3 holePosition)
        {
            this.user = user;
            this.kinect = user.Kinect;
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
            missed = "";
            missedPosition = Vector2.Zero;
            this.holePosition = holePosition;


        }
        #endregion
        #region Load
        ///<summary>
        ///This method loads the textres used in the pause screen.
        ///</summary>
        ///<remarks>
        ///<para>AUTHOR: Cena </para>   
        ///</remarks>
        public override void LoadContent()
        {
            viewPort = ScreenManager.GraphicsDevice.Viewport;
            content = ScreenManager.Game.Content;
            spriteBatch = ScreenManager.SpriteBatch;



            font = content.Load<SpriteFont>("SpriteFont1");
            countFont = content.Load<SpriteFont>("SpriteFont2");
            font2 = content.Load<SpriteFont>("SpriteFont3");
            velocityBar = content.Load<Texture2D>("Textures/VBar");
            vBarPosition = new Vector2((velocityBar.Width / 2) + 20, viewPort.Height - (velocityBar.Height / 2));
            fillPosition = new Vector2(velocityBar.Width / 2 + 20, viewPort.Height - (7 / 2));

            givens = content.Load<Texture2D>("Textures/screen");
            givensPosition = new Vector2(viewPort.Width / 2, givens.Height / 4);


            arrow = content.Load<Texture2D>("Textures/arrow");
            arrowScale = 0.7f;
            arrowPosition = new Vector2(viewPort.Width - (float)( Math.Sqrt(arrowScale) * arrow.Width), viewPort.Height - (float)(Math.Sqrt(arrowScale) * arrow.Height / 2));
            arrowAngle = 0;

            countPosition = new Vector2(3 * (viewPort.Width / 7), viewPort.Height / 2);
            missedPosition = new Vector2(viewPort.Width / 3, viewPort.Height / 3);
           
            button = Tools3.OKButton(content, new Vector2(viewPort.Width - 245, 0), viewPort.Width, viewPort.Height, user);

            base.LoadContent();


        }
        #endregion
        #region Update
        ///<summary>
        ///This method updates the positions of the textures each frame.
        ///</summary>
        ///<remarks>
        ///<para>AUTHOR: Cena </para>   
        ///</remarks>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            button.Update(gameTime);
            if (!button.IsClicked())
            {
                if (!user.hasShot&&!user.hasMissed)
                {

                    user.UpdateMeasuringVelocityAndAngle(gameTime);

                    displayedGivens = "Ball Mass: " + Math.Truncate(ballMass * 1000) / 1000 +'\n' + "Ball Velocity: " + ballVelocity + '\n' + "Leg Mass: "
                     + (Math.Truncate(legMass * 1000) / 1000) + '\n' + "Ball Position: " + "[X:0,Y:0,Z:62]" + '\n' + "Hole Position: " + "[" +"X:"+ Math.Truncate(holePosition.X) + "," + "Y:0," +"Z:"+ Math.Truncate(holePosition.Z) + "]";
                }
                else
                {
                    #region GivensString
                    displayedGivens = "Ball Mass: " + Math.Truncate(ballMass * 1000) / 1000 +'\n' + "Ball Velocity: " + ballVelocity + '\n' + "Leg Mass: "
                        + Math.Truncate(legMass * 1000) / 1000 + '\n' + "Ball Position: " + "[X:0,Y:0,Z:62]" + '\n' + "Hole Position: " + "["+"X:"+Math.Truncate(holePosition.X)+","+"Y:0,"+"Z:"+Math.Truncate(holePosition.Z)+"]";
                    string shootingValues ="Shooting velocity: " + Math.Truncate(velocity.Length() * 1000) / 1000 + " m/s "
                        + '\n' + "Shooting angle: " + Math.Truncate((user.angle * 180 / Math.PI) * 1000) / 1000 + " deg";
                    #endregion

                    #region DrawingBarAndArrowInfo
                    if (!user.hasMissed)
                    {
                        velocity = user.SetVelocityRelativeToGivenMass() * Constants3.velocityScale;
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

                        user.velocity = velocity;
                        arrowAngle = (float)user.angle;
                        displayedGivens += ('\n' + shootingValues);
                    }
                    else
                        missed = "Missed!";
                    #endregion

                    #region CountDown And Clear
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
                                count = "GO!";
                                countColor = Color.BlueViolet;
                                countScale = 0.8f;
                            }
                            framesToWait++;

                        }
                    #endregion
                }
            }
            else
            {

                user.ResetUserForShootingOrTryingAgain();

                ExitScreen();
            }



            base.Update(gameTime);
        }
        #endregion
        #region Draw
        ///<summary>
        ///This method Draws the textures.
        ///</summary>
        ///<remarks>
        ///<para>AUTHOR: Cena </para>   

        ///</remarks>

        ///<returns>A Vector3: velocity of the user relative to it's mass</returns>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {


           

            spriteBatch.Begin();
            spriteBatch.Draw(givens, givensPosition, null, Color.White, 0, new Vector2(givens.Width / 2, givens.Height / 2), 0.9f, SpriteEffects.None, 0);
            spriteBatch.Draw(velocityBar, vBarPosition, null, Color.White, 0, new Vector2(velocityBar.Width / 2, velocityBar.Height / 2), 1f, SpriteEffects.None, 0);
            for (int i = 0; i < fills.Count; i++)
                spriteBatch.Draw(fills.ElementAt<Texture2D>(i), fillsPositions.ElementAt<Vector2>(i), null,
                    Color.White, 0, new Vector2(fills.ElementAt<Texture2D>(i).Width / 2,
                        fills.ElementAt<Texture2D>(i).Height / 2), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(arrow, arrowPosition, null, Color.White, arrowAngle, new Vector2((arrow.Width) / 2, (arrow.Height) / 2), arrowScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, displayedGivens, new Vector2(viewPort.Width / 6, givens.Height / 30), Color.Black);
            spriteBatch.DrawString(countFont, count, countPosition, countColor, 0, Vector2.Zero, countScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font2, missed, missedPosition, Color.Red);
            button.Draw(spriteBatch, ((float)viewPort.Width / 1024f) * 0.85f);
            button.DrawHand(spriteBatch);

            spriteBatch.End();


            base.Draw(gameTime);

        }
        #endregion
        #region Clear
        ///<summary>
        ///This method clears the velocity bar and the user's variables.
        ///</summary>
        ///<remarks>
        ///<para>AUTHOR: Cena </para>   
        ///</remarks>
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
            countPosition = new Vector2(3 * (viewPort.Width / 7), viewPort.Height / 2);
            countScale = 1;
            missed = "";
        }
        #endregion
        #endregion

        public override void Remove()
        {
            base.Remove();
        }
    }

}
