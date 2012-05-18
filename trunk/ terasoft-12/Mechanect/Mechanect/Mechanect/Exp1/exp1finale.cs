/*using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using Mechanect.Classes;
using Mechanect.Common;
using Mechanect.Tools;

namespace ElLeela
{

    public class Game1c : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        CountDown One, Two, Three, GO, BackGround;
        SoundEffect effect1, effect2;

        MKinect kinect;
        AvatarprogUI avatarprogUI;
        User player1;
        User player2;

        List<int> timeslice;
        List<String> gCommands = new List<string> { "constantDisplacement", "constantAcceleration", "increasingAcceleration", "decreasingAcceleration", "constantVelocity" };
        List<String> racecommands;
        List<String> racecommandsforDRAW;
        drawstring drawString, player1drawstring, player2drawstring;
        Boolean play1, play2, play3, play4 = true;
        float timer;
        int timecounter;




        public Game1c()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            kinect = new MKinect();
            player1 = new User("first", kinect);
            player2 = new User("second", kinect);
            drawString = new drawstring(new Vector2(400, 400));
            player1drawstring = new drawstring(new Vector2(300, 450));
            player2drawstring = new drawstring(new Vector2(600, 450));
            player1drawstring.Update("player1disQualified");
            player2drawstring.Update("player2disQualified");

            //---------------initialize the timeslices and game sequence ------------------------------------
            racecommands = gCommands;
            Mechanect.Tools.Tools1.commandshuffler<string>(racecommands);
            racecommands = racecommands.Concat<string>(racecommands).ToList<string>();
            // foreach (string s in racecommands)
            //   System.Console.WriteLine(s);
            timeslice = Mechanect.Tools.Tools1.generaterandomnumbers(racecommands.Count);
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


            //---------------done with timeslces and game sequence -------------------------------------------








            //-------------------------------------------------------------------------------------------
            Three = new CountDown(Content.Load<Texture2D>("3"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 300, 150, 200, 200);
            Two = new CountDown(Content.Load<Texture2D>("2"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 300, 150, 200, 200);
            One = new CountDown(Content.Load<Texture2D>("1"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 300, 150, 200, 200);
            GO = new CountDown(Content.Load<Texture2D>("go"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 330, 150, 150, 150);
            BackGround = new CountDown(Content.Load<Texture2D>("track2"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 0, 0, 800, 490);
            effect1 = Content.Load<SoundEffect>("BEEP1B");
            effect2 = Content.Load<SoundEffect>("StartBeep");
            //-------------------------------------------------------------------------------------------


            avatarprogUI = new AvatarprogUI();



            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            drawString.Font1 = Content.Load<SpriteFont>("SpriteFont1");
            player1drawstring.Font1 = Content.Load<SpriteFont>("SpriteFont1");
            player2drawstring.Font1 = Content.Load<SpriteFont>("SpriteFont1");
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }



        // TTTHEEE UPPPPPDAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAATE
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            //------------------------------the TIME ----------------------------------------------

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            timecounter += (int)timer;
            if (timer >= 1.0F)
                timer = 0F;




            //-----------------------------time done ----------------------------------------------

            //----------------------------keeps getting skeletons---------------------------------
            if (player1.skeleton == null)
                player1.skeleton = kinect.requestSkeleton();

            if (player2.skeleton == null)
                player2.skeleton = kinect.request2ndSkeleton();
            //--------------------------skeletonsdone----------------------------------



            if (timecounter < 4)
            {
                if (Three.GetCounter() > 0)
                {
                    Three.Update();
                    if (play1)
                    {
                        effect1.Play();
                        play1 = false;
                    }
                }
                if (Three.GetCounter() == 0 && Two.GetCounter() > 0)
                {
                    Two.Update();
                    if (play2)
                    {
                        effect1.Play();
                        play2 = false;
                    }
                }
                if (Two.GetCounter() == 0 && Three.GetCounter() == 0 && One.GetCounter() > 0)
                {
                    One.Update();
                    if (play3)
                    {
                        effect1.Play();
                        play3 = false;
                    }
                }
                if (One.GetCounter() == 0 && Two.GetCounter() == 0 && Three.GetCounter() == 0 && GO.GetCounter() > 0)
                {
                    if (play4)
                    {
                        effect2.Play();
                        play4 = false;
                    }

                }
            }
            //-------------------------------------------------------------------------------------------



            //race should start after countdown


            if ((timecounter >= 4 & (timecounter < racecommandsforDRAW.Count + 4)) & !(player2.Disqualified & player1.Disqualified) & !player1.Winner & !player2.Winner)
            {








                //if(player1.skeleton != null & player2.skeleton != null)
                avatarprogUI.Update(kinect, player1.skeleton, player2.skeleton); //update position of avatars on screen
                drawString.Update(racecommandsforDRAW[timecounter - 4] + "");   //update STRING DRAW on screen
                //----------------------------------------------------------------------------------
                if (player1.skeleton != null)
                {
                    player1.Depthfromkinect.Add(player1.skeleton.Position.Z);
                }
                else
                {
                    //  player1.Disqualified = true;
                }
                if (player2.skeleton != null)
                {
                    player2.Depthfromkinect.Add(player2.skeleton.Position.Z);
                }
                else
                {
                    //player2.Disqualified = true;
                }
                //-------------------------------------------------------------------------------------



                if (timer % 10 == 0)
                {
                    //
                    //    Mechanect.Tools.Tools1.CheckEachSecond(timer,player1,player2,timeslice,racecommands ,/*tolerance 1,spriteBatch,/* some font );

             //   } 
                /*
                if (player1.skeleton.Position.Z <= 0.9 & player2.skeleton.Position.Z > 0.9 & !player1.Disqualified)
                {
                    player1.Winner = true;
                }
                if (player1.skeleton.Position.Z <= 0.9 & player2.skeleton.Position.Z <= 0.9 &!player2.Disqualified)
                {
                    player2.Winner = true;
                }
                if (player1.skeleton.Position.Z <= 0.9 & player2.skeleton.Position.Z <= 0.9)
                {
                    player1.Winner= true;
                    player2.Winner = true;
                }
                */



/*















            }
            if (timecounter >= racecommandsforDRAW.Count + 4 || (player2.Disqualified & player1.Disqualified) || player2.Winner || player1.Winner)
            {
                //GRAPhS!




            }


            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (timecounter < 4)
            {
                //-------------------------------------------------------------------------------------------

                BackGround.Draw(spriteBatch);
                if (Three.GetCounter() > 0)
                {
                    Three.Draw(spriteBatch);
                }
                if (Three.GetCounter() == 0 && Two.GetCounter() > 0)
                {
                    Two.Draw(spriteBatch);
                }
                if (Two.GetCounter() == 0 && Three.GetCounter() == 0 && One.GetCounter() > 0)
                {
                    One.Draw(spriteBatch);
                }
                if (One.GetCounter() == 0 && Two.GetCounter() == 0 && Three.GetCounter() == 0 && GO.GetCounter() > 0)
                {
                    GO.Draw(spriteBatch);
                }
            }
            //-------------------------------------------------------------------------------------------



            if (timecounter >= 4 & timecounter < racecommandsforDRAW.Count + 4)
            {
                //race should start after countdown
                BackGround.Draw(spriteBatch);
                spriteBatch.Begin();
                avatarprogUI.Draw(spriteBatch, Content.Load<Texture2D>("ball"));
                if (player1.Disqualified)
                {
                    player1drawstring.Draw(spriteBatch);
                }
                if (player2.Disqualified)
                {
                    player2drawstring.Draw(spriteBatch);
                }
                drawString.Draw(spriteBatch);
                spriteBatch.End();
            }

            if (timecounter >= racecommandsforDRAW.Count + 4)
            {
                //do smth GRAPHS

            }



            base.Draw(gameTime);
        }
    }
}
*/