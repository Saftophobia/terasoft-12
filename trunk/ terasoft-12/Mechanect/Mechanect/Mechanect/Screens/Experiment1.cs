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
using Mechanect.Classes;

namespace Mechanect.Screens
{
    class Experiment1:Mechanect.Common.GameScreen
    {
        Boolean positionmutex = false;
        Boolean graphmutex = false;
        GraphicsDevice graphics;
        List<double> cumulativetime;
        List<int> cumulativetimeint;
        MKinect kinect;
        Texture2D avatarBall;
        User1 player1, player2;
        CountDown countdown;
        CountDown background1,background2;
        float timer = 0;
        SpriteFont spritefont;
        int timecounter;
        PerformanceGraph Graph;
        List<int> timeslice;
        List<String> gCommands = new List<string> { "constantDisplacement", "constantAcceleration", "increasingAcceleration", "decreasingAcceleration", "constantVelocity" };
        List<String> racecommands;
        List<String> racecommandsforDRAW;
        AvatarprogUI avatarprogUI;
        int player1disqualification;
        int player2disqualification;
        SpriteFont font1, font2;
        Texture2D P1Tex, P2Tex;
        drawstring drawString = new drawstring(new Vector2(400, 400));
        drawstring player1disqstring = new drawstring(new Vector2(400, 550));
        drawstring player2disqstring = new drawstring(new Vector2(400,220));
        int NUMBEROFFRAMES = 0;
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





        public Experiment1(User1 player1,User1 player2,MKinect kinect)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.kinect = kinect;
        }

        public override void Initialize()
        {
            graphics = ScreenManager.GraphicsDevice;
            //-----------------------initializetimecountand commands--------------------------
            racecommands = gCommands;
            Mechanect.Classes.Tools1.commandshuffler<string>(racecommands);
            //racecommands = racecommands.Concat<string>(racecommands).ToList<string>(); // copy the list for more Commands
            // foreach (string s in racecommands)
            //   System.Console.WriteLine(s);
            timeslice = Mechanect.Classes.Tools1.generaterandomnumbers(racecommands.Count); //sets a time slice for each command
            // foreach (int s in timeslice)
            //  System.Console.WriteLine(s);
            racecommandsforDRAW = new List<string>();

            for (int time = 0; time < timeslice.Count; time++)
            {
                for (int timeslot = 1; timeslot <= timeslice[time]; timeslot++)
                {
                    racecommandsforDRAW.Add(racecommands[time]);
                    //draw the command on screen for each second
                }
            }

           cumulativetime = new List<double>();
            for (int i = 0; i < timeslice.Count; i++)
            {
                int cumutime = 0;
                for (int z = 0; z <= i; z++)
                {
                    cumutime = cumutime +  timeslice[z];
                }
                cumulativetime.Add(cumutime);
            }

            cumulativetimeint = new List<int>();
            for (int i = 0; i < timeslice.Count; i++)
            {
                int cumutime = 0;
                for (int z = 0; z <= i; z++)
                {
                    cumutime = cumutime + timeslice[z];
                }
                cumulativetimeint.Add(cumutime);
            }
            
            
            //-----------============-----------------------==========================================-


            //------------------------------Avatarprog----------------------------
            avatarprogUI = new AvatarprogUI();
             //SpriteBatch = new SpriteBatch(GraphicsDevice);
            // drawString.Font1 = Content.Load<SpriteFont>("SpriteFont1");

            //----------------------------------------------------------------------

            // initialize the graph
           

            Graph = new PerformanceGraph();
            //----================



         

            base.Initialize();
        }
        public override void LoadContent()
        {
            // graphics = new GraphicsDeviceManager(this);
          //  Content.RootDirectory = "Content";
            // IntPtr ptr = this.Window.Handle;
            //    System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(ptr);
            //  form.Size = new System.Drawing.Size(1024, 768);
            // graphics.PreferredBackBufferWidth = 1024;
            // graphics.PreferredBackBufferHeight = 650;
            //  graphics.ApplyChanges();
            spritefont = Content.Load<SpriteFont>("SpriteFont1");
            drawString.Font1 = Content.Load<SpriteFont>("SpriteFont1");
            avatarBall = Content.Load<Texture2D>("ball");
            Texture2D Texthree = Content.Load<Texture2D>("3");
            Texture2D Textwo = Content.Load<Texture2D>("2");
            Texture2D Texone = Content.Load<Texture2D>("1");
            Texture2D Texgo = Content.Load<Texture2D>("go");
            Texture2D Texback = Content.Load<Texture2D>("track2");
            SoundEffect Seffect1 = Content.Load<SoundEffect>("BEEP1B");
            SoundEffect Seffect2 = Content.Load<SoundEffect>("StartBeep");
            countdown = new CountDown(Texthree, Textwo, Texone, Texgo, Texback, Seffect1, Seffect2,ScreenManager.GraphicsDevice.Viewport.Width,ScreenManager.GraphicsDevice.Viewport.Height); //initializes the Countdown 
            background1 = new CountDown(Content.Load<Texture2D>("track2"), ScreenManager.GraphicsDevice.Viewport.Width,
            ScreenManager.GraphicsDevice.Viewport.Height, 0, 0, ScreenManager.GraphicsDevice.Viewport.Width/*1024*/, ScreenManager.GraphicsDevice.Viewport.Height/* 768*/); //initializes the background
            background2 = new CountDown(Content.Load<Texture2D>("Background2"), ScreenManager.GraphicsDevice.Viewport.Width,
            ScreenManager.GraphicsDevice.Viewport.Height, 0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height); //initializes the background
            player1disqstring.Font1 = Content.Load<SpriteFont>("SpriteFont1");
            player2disqstring.Font1= Content.Load<SpriteFont>("SpriteFont1");
            font1 = Content.Load<SpriteFont>("SpriteFont1");
            font2 = Content.Load<SpriteFont>("SpriteFont1");
            P1Tex = Content.Load<Texture2D>("xRed");
            P2Tex = Content.Load<Texture2D>("xBlue");



            //------------------------graphs

            Graph = new PerformanceGraph();
            List<string> Commands = new List<string>();
            List<double> CommandsTime = new List<double>();
            List<int> Player1Displacement = new List<int>();
            List<int> Player2Displacement = new List<int>();
        }
            //main initializing method
            //Graph.fs(Player1Displacement, Player2Displacement, Commands, CommandsTime, this);

           // -----------------------------------
          /*  while (true)
            {
                player1.skeleton = kinect.requestSkeleton();
                player2.skeleton = kinect.request2ndSkeleton();
                if (player1.skeleton != null && player2.skeleton != null)
                {
                    break;
                }
            }

        }*/

        public override void UnloadContent()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="covered"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {

            player1.skeleton = kinect.requestSkeleton();
            player2.skeleton = kinect.request2ndSkeleton();

            if (!positionmutex)
            {   
                if (Tools1.SetPositions(player1.skeleton, player2.skeleton, SpriteBatch, spritefont, 500))
                {
                    positionmutex = true;
                }

            }


            if (positionmutex)
            {


                NUMBEROFFRAMES++;
                //----------------------TIME----------------------------
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                timecounter += (int)timer;
                if (timer >= 1.0F)
                    timer = 0F;





                //--------------------keepsgettingskeletons----------------









                //=======================================================
                if (timecounter < 4)
                {
                    countdown.UpdateCountdownScreen();

                }

                //after countdown, Update the Race 
                if ((timecounter >= 5 & (timecounter < racecommandsforDRAW.Count + 4)) & !(player2.Disqualified & player1.Disqualified) & !player1.Winner & !player2.Winner)
                {


                    drawString.Update(racecommandsforDRAW[timecounter - 4] + "");
                    avatarprogUI.Update(kinect, player1.skeleton, player2.skeleton); //update position of avatars on screen


                    // float rem = timer*10000000 / 12;
                    // if (((timer * 10000000) % 12) == 0)

                    //drawString.Update((NUMBEROFFRAMES / (timecounter + timer))+"");


                    //drawString.Update(NUMBEROFFRAMES + "               " + timecounter + "           " + timer); 
                    //10000000

                    if (NUMBEROFFRAMES % 5 == 0)
                    {

                        if (player1.skeleton != null & player2.skeleton != null)
                        {
                            if (player1.skeleton.Position.X > player2.skeleton.Position.X)
                            {
                                player1.Positions.Add(player1.skeleton.Position.Z);
                                player2.Positions.Add(player2.skeleton.Position.Z);
                            }
                            else
                            {
                                player2.Positions.Add(player1.skeleton.Position.Z);
                                player1.Positions.Add(player2.skeleton.Position.Z);
                            }
                        }







                        /*if (player1.skeleton != null)
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
                        
                        }*/
                    }

                    //-----------------which command i m in-----------------------
                    string s = racecommandsforDRAW[timecounter - 5];
                    for (int i = 0; i < racecommands.Count; i++)
                        if (racecommands[i] == s)
                        {
                            player1.ActiveCommand = i;
                            player2.ActiveCommand = i;
                        }

                    //-----------------------------------------------------------



                    // if (timer % 10 == 0 /*& timecounter >=5*/)
                    if (NUMBEROFFRAMES % 60 == 0 && NUMBEROFFRAMES > 300)
                    {
                        for (int z = 0; z < cumulativetime.Count; z++)
                        {
                            if (cumulativetimeint[z] != timecounter)
                            {
                                Tools1.CheckEachSecond(timecounter - 5, player1, player2, timeslice, racecommands, 100, SpriteBatch, spritefont);
                                break;
                            }
                        }

                        // Tools1.CheckEachSecond(timecounter - 5, player1, player2,timeslice , racecommands, 100, SpriteBatch, spritefont);


                        //  Tools1.CheckEachSecond(timecounter - 5, player1, player2, timeslice, racecommands, 10000, SpriteBatch, spritefont);


                        player1.DisqualificationTime = player1disqualification;
                        player2.DisqualificationTime = player2disqualification;
                        if (player1.Disqualified)
                        {
                            player1disqstring.Update("player 1 is disqualified");
                        }
                        if (player2.Disqualified)
                        {
                            player2disqstring.Update("player 2 is disqualified");
                        }



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
                    /*
                    if (player1.skeleton == null & player2.skeleton != null)
                    {
                        if (player2.skeleton.Position.Z <= 0.9 & !player2.Disqualified)
                        {
                            player2.Winner = false;
                        }

                    }

                    if (player1.skeleton != null & player2.skeleton == null)
                    {
                        if (player1.skeleton.Position.Z <= 0.9 & !player1.Disqualified)
                        {
                            player1.Winner = false;
                        }

                    }
                    */





                }
                if (timecounter >= racecommandsforDRAW.Count + 4 || player2.Disqualified || player1.Disqualified || player2.Winner || player1.Winner/* || player1.skeleton == null || player2.skeleton == null*/)
                {

                    List<double> timeofrace = new List<double>(); // time of the race
                    timeofrace.Add(timeslice[0]);

                    for (int i = 1; i < cumulativetime.Count; i++)
                    {
                        if (cumulativetime[i] <= timecounter)
                        {
                            timeofrace.Add(timeslice[i]);
                        }
                    }



                    if (!graphmutex)
                    {
                        List<double> timeslicedouble = new List<double>();
                        foreach (int s in timeslice)
                        {
                            timeslicedouble.Add((double)s);
                        }



                        /* List<string> Commands = new List<string>();
                         List<double> CommandsTime = new List<double>();
                         List<float> Player1Displacement = new List<float>();
                         List<float> Player2Displacement = new List<float>();
                         //initiating testing values 
                         Commands.Add("constantDisplacement");
                         Commands.Add("constantDisplacement");
                         Commands.Add("increasingAcceleration");
                         Commands.Add("constantDisplacement");
                         CommandsTime.Add(1);
                         CommandsTime.Add(1);
                         CommandsTime.Add(1);
                         CommandsTime.Add(1);
                         int intitial = 4000;
                         int stepping = 1;
                         for (int i = 0; i <= 47; i++)
                         {
                             if (intitial > 0)
                             {
                                 Player1Displacement.Add(intitial);
                                 stepping = stepping + 5;
                                 intitial = intitial - stepping;
                             }
                             else
                             {
                                 Player1Displacement.Add(0);
                             }
                         }
                         intitial = 4000;
                         stepping = 1;
                         for (int i = 0; i <= 47; i++)
                         {
                             if (intitial > 0)
                             {
                                 Player2Displacement.Add(intitial);
                                 stepping = stepping + 54;
                                 intitial = intitial - stepping;
                             }
                             else
                             {
                                 Player2Displacement.Add(0);
                             }
                         }
                         //main initializing method*/

                        //  Graph.drawGraphs(Player1Displacement, Player2Displacement, Commands, CommandsTime, player1disqualification, player2disqualification, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

                        /* List<float> positions1times1k = new List<float>();
                         List<float> positions2times1k = new List<float>();
                         foreach (float s in player1.Positions)
                         {
                             positions1times1k.Add(s * 1000);
                         }

                         foreach (float o in player2.Positions)
                         {
                             positions2times1k.Add(o * 1000);
                         }*/




                        // Graph.drawGraphs(player1.Positions, player2.Positions, racecommands,timeslicedouble,player1disqualification, player2disqualification, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
                        Graph.DrawGraphs(player1.Positions, player2.Positions, racecommands, timeofrace, player1disqualification, player2disqualification, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);


                        graphmutex = true;
                    }







                }
                base.Update(gameTime, covered);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {

            if(!positionmutex){
                if (Tools1.SetPositions(player1.skeleton, player2.skeleton, SpriteBatch, spritefont, 500))
                {
                    positionmutex = true;
                }
            }else{

            
            if (timecounter < 4)
            {
               // SpriteBatch.Begin();
                background1.Draw(SpriteBatch);
                countdown.DrawCountdownScreen(SpriteBatch);
              //  SpriteBatch.End();
            }

            //After countdown,Draw the Avatar
            if ((timecounter >= 4 & (timecounter < racecommandsforDRAW.Count + 4)) & !(player2.Disqualified & player1.Disqualified) & !player1.Winner & !player2.Winner)
            {
                
                background2.Draw(SpriteBatch);
                SpriteBatch.Begin();
                drawString.Draw(SpriteBatch);
            avatarprogUI.Draw(SpriteBatch,avatarBall);

            if (player1.Disqualified)
            {
                player1disqstring.Draw(SpriteBatch);
            }
            if (player2.Disqualified)
            {
                player2disqstring.Draw(SpriteBatch);
            }



            SpriteBatch.End();
           
            }


            // after Race, Draw the Graphs
            if (timecounter >= racecommandsforDRAW.Count + 4 ||(player2.Disqualified & player1.Disqualified) || player2.Winner || player1.Winner)
            {
               // SpriteBatch sprite2 = SpriteBatch;
                SpriteBatch.Begin();
               Graph.DrawRange(SpriteBatch, graphics);
                Graph.DrawEnvironment(SpriteBatch, graphics, font1, font2);
                Graph.DrawDisqualification(SpriteBatch,ScreenManager.GraphicsDevice.Viewport.Width,ScreenManager.GraphicsDevice.Viewport.Height, P1Tex, P2Tex,(double)player1disqualification,(double)player2disqualification);
               SpriteBatch.End();
                
            }


           
        }}

        public override void Remove()
        {
            base.Remove();
        }
    }
}
