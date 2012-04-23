using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mechanect.Cameras;
using Mechanect.Common;

namespace Mechanect.Screens
{
    class Experiment1:Mechanect.Common.GameScreen
    {
        GraphicsDeviceManager graphics;
        MKinect kinect;
        User1 player1, player2;
        CountDown countdown;
        CountDown background;
        float timer = 0;
        int timecounter;
        List<int> timeslice;
        List<String> gCommands = new List<string> { "constantDisplacement", "constantAcceleration", "increasingAcceleration", "decreasingAcceleration", "constantVelocity" };
        List<String> racecommands;
        List<String> racecommandsforDRAW;
        AvatarprogUI avatarprogUI;
        int player1disqualification;
        int player2disqualification;
        //drawstring drawString
        

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





        public Experiment1(User1 User,MKinect kinect)
        {
            
        }


        public override void LoadContent()
        {
           // graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           // IntPtr ptr = this.Window.Handle;
        //    System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(ptr);
          //  form.Size = new System.Drawing.Size(1024, 768);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 650;
            graphics.ApplyChanges();


            Texture2D Texthree = Content.Load<Texture2D>("MechanectContent/3");
            Texture2D Textwo = Content.Load<Texture2D>("MechanectContent/2");
            Texture2D Texone = Content.Load<Texture2D>("MechanectContent/1");
            Texture2D Texgo = Content.Load<Texture2D>("MechanectContent/go");
            Texture2D Texback = Content.Load<Texture2D>("MechanectContent/track2");
            SoundEffect Seffect1 = Content.Load<SoundEffect>("MechanectContent/BEEP1B");
            SoundEffect Seffect2 = Content.Load<SoundEffect>("MechanectContent/StartBeep");
            countdown = new CountDown(Texthree, Textwo, Texone, Texgo, Texback, Seffect1, Seffect2, graphics);
            background = new CountDown(Texback, graphics.PreferredBackBufferWidth,
            graphics.PreferredBackBufferHeight, 0, 0, 1024, 768);

            //-----------------------initializetimecountand commands--------------------------
            racecommands = gCommands;
            Mechanect.Classes.Tools1.commandshuffler<string>(racecommands);
            racecommands = racecommands.Concat<string>(racecommands).ToList<string>();
            // foreach (string s in racecommands)
            //   System.Console.WriteLine(s);
            timeslice = Mechanect.Classes.Tools1.generaterandomnumbers(racecommands.Count);
            // foreach (int s in timeslice)
            //  System.Console.WriteLine(s);
            racecommandsforDRAW = new List<string>();

            for (int time = 0; time < timeslice.Count; time++)
            {
                for (int timeslot = 1; timeslot <= timeslice[time]; timeslot++)
                {
                    racecommandsforDRAW.Add(racecommands[time]);
                }
            }


            //------------------------------Avatarprog----------------------------
            avatarprogUI = new AvatarprogUI();
            //SpriteBatch = new SpriteBatch(GraphicsDevice);
           // drawString.Font1 = Content.Load<SpriteFont>("SpriteFont1");

            //----------------------------------------------------------------------





        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {

            //----------------------TIME----------------------------
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timecounter += (int)timer;
            if (timer >= 1.0F)
                timer = 0F;
            
            //--------------------keepsgettingskeletons----------------


            player1.skeleton = kinect.requestSkeleton();
            player2.skeleton = kinect.request2ndSkeleton();
            if (timecounter < 4)
            {
                countdown.UpdateCountdownScreen();

            }
            if ((timecounter >= 4 & (timecounter < racecommandsforDRAW.Count + 4)) & !(player2.Disqualified & player1.Disqualified) /*& /*!player1.Winner & !player2.Winner*/)
            {
                avatarprogUI.Update(kinect, player1.skeleton, player2.skeleton);
                //drawString.Update(racecommandsforDRAW[timecounter - 4] + "")
                
                if (player1.skeleton != null)
                {
                    player1.Positions.Add(player1.skeleton.Position.Z);
                }
                else
                {
                      player1.Disqualified = true;
                }
                if (player2.skeleton != null)
                {
                    player2.Positions.Add(player2.skeleton.Position.Z);
                }
                else
                {
                    player2.Disqualified = true;
                }
                

                if (timer % 10 == 0)
                 {
                    //
                    //Mechanect.Tools.Tools1.CheckEachSecond(timer, player1, player2, timeslice, racecommands, 5, spriteBatch,spfont);


                    player1.DisqualificationTime = player1disqualification;
                    player2.DisqualificationTime = player2disqualification;

                 }

                if (player1.skeleton != null & player2.skeleton != null)
                   {
                
                if (player1.skeleton.Position.Z <= 0.9 & player2.skeleton.Position.Z > 0.9 & !player1.Disqualified)
                {
                    player1.Winner = true;
                }


                if (player1.skeleton.Position.Z > 0.9 & player2.skeleton.Position.Z <= 0.9 & !player2.Disqualified)
                {
                    player2.Winner = true;
                }

                 /*if (player1.skeleton.Position.Z <= 0.9 & player2.skeleton.Position.Z <= 0.9 & !player1.Disqualified &!player2.Disqualified)
                     {
                         player1.Winner = true;
                         player2.Winner = true;
                     }
                   */
                }



            }

            if (timecounter >= racecommandsforDRAW.Count + 4 || (player2.Disqualified & player1.Disqualified) || player2.Winner || player1.Winner)
            {



                //sprite2.End();







            }
            base.Update(gameTime, covered);
        }
            

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (timecounter < 4)
            {
                background.Draw(SpriteBatch);
                countdown.DrawCountdownScreen(SpriteBatch);
            }


            if ((timecounter >= 4 & (timecounter < racecommandsforDRAW.Count + 4)) & !(player2.Disqualified & player1.Disqualified) & !player1.Winner & !player2.Winner)
            {
            avatarprogUI.Draw(SpriteBatch, Content.Load<Texture2D>("ball"));
            }


            if (timecounter >= racecommandsforDRAW.Count + 4 || (player2.Disqualified & player1.Disqualified) || player2.Winner || player1.Winner)
            {

            }


           
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}
