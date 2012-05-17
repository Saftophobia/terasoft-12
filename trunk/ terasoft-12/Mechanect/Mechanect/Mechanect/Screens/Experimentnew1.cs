using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mechanect.Classes;
using Microsoft.Kinect;
using Mechanect.Common;
using Microsoft.Xna.Framework.Audio;

namespace Mechanect.Screens
{
    class Experimentnew1 : Mechanect.Common.GameScreen
    {
        #region kneevariables
        
        float speed;
        float speedr;
        float[] speedlist = new float[2];
        float[] speedlistr = new float[2];

       
        bool calculatespeedbool = false;
        bool calculatespeedboolr = false;
        float[] max, min = new float[2];
        float[] maxr, minr = new float[2];
        Joint left, right;
        float timer = 0;
        int timecounter;
        #endregion
        MKinect kinect;
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
        CountDown countdown;
        /// <summary>
        /// Initializes Experiment1 with 2 different Users
        /// </summary>
        /// <param name="user1">User1 to be used in the Race</param>
        /// <param name="user2">User2 to be used in the Race</param>
        /// <remarks>
        /// <para>Author: Safty</para>
        /// <para>Date Written 15/5/2012</para>
        /// <para>Date Modified 15/5/2012</para>
        /// </remarks>
        public Experimentnew1(User1 user1, User1 user2)
        {
            this.user1 = user1;
            this.user2 = user2;
        }

        public override void Initialize()
        {
            kinect = new MKinect();
            base.Initialize();

        }
        /// <summary>
        /// Load Content from ContentManager
        /// </summary>
        /// <remarks>
        /// <para>Author: Safty</para>
        /// <para>Date Written 15/5/2012</para>
        /// <para>Date Modified 15/5/2012</para>
        /// </remarks>
        public override void LoadContent()
        {


            graphics = this.ScreenManager.GraphicsDevice;
            Environment1 = new Environment1(ScreenManager.Game.Content, ScreenManager.Game.GraphicsDevice, this.SpriteBatch);
            Environment1.LoadContent();
            loadcountdown();
        }



        /// <summary>
        /// Update Game's logic 60 times per second
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="covered"></param>
        /// <para>Author: Safty</para>
        /// <para>Date Written 15/5/2012</para>
        /// <para>Date Modified 15/5/2012</para>
        /// </remarks>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            countdown.Update();
            Environment1.update(gameTime);
            if (user1.skeleton == null || user2.skeleton == null)
            {
                Environment1.ChaseCam();
                kinect.requestSkeleton();
                user1.skeleton = kinect.globalSkeleton;
                kinect.request2ndSkeleton();
                user2.skeleton = kinect.globalSkeleton2;
            }
            else
            {
                Environment1.TargetCam();
                fill_Knee_pos();
                getspeedleft(user1);
                getspeedleft(user2);
                getspeedright(user1);
                getspeedright(user2);


            }

            if (user1.Winner || user2.Winner || (!user1.Disqualified && !user2.Disqualified))
            {
                //display Graphs
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw's Game component 60 times per second
        /// </summary>
        /// <param name="gameTime"></param>
        /// <para>Author: Safty</para>
        /// <para>Date Written 15/5/2012</para>
        /// <para>Date Modified 15/5/2012</para>
        /// </remarks>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Environment1.Draw(gameTime);
            countdown.DrawCountdown(SpriteBatch);
            countdown.PlaySoundEffects();
        }

        public void fill_Knee_pos()
        {

            if (user1.skeleton.Position.X > user2.skeleton.Position.X)
            {
                user1.Kneepos.Add((float)Math.Round((user1.skeleton.Joints[JointType.KneeLeft].Position.Y),2));
                user2.Kneepos.Add((float)Math.Round((user2.skeleton.Joints[JointType.KneeLeft].Position.Y), 2));

                user1.Kneeposr.Add((float)Math.Round((user1.skeleton.Joints[JointType.KneeRight].Position.Y),2));
                user2.Kneeposr.Add((float)Math.Round((user2.skeleton.Joints[JointType.KneeRight].Position.Y),2));


            }
            else
            {
                user2.Kneepos.Add((float)Math.Round((user1.skeleton.Joints[JointType.KneeLeft].Position.Y),2));
                user1.Kneepos.Add((float)Math.Round((user2.skeleton.Joints[JointType.KneeLeft].Position.Y),2));

                user2.Kneeposr.Add((float)Math.Round((user1.skeleton.Joints[JointType.KneeRight].Position.Y),2));
                user1.Kneeposr.Add((float)Math.Round((user2.skeleton.Joints[JointType.KneeRight].Position.Y), 2));
            }
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 16/5/2012</para>
        /// <para>Date Modified 16/5/2012</para>
        /// </remarks>
        /// <summary>
        /// The function loadcountdown() loads the textures and soundeffects required for the countdown.
        /// </summary>
        /// <returns>void</returns>
        public void loadcountdown()
        {
            Texture2D Texthree = Content.Load<Texture2D>("3");
            Texture2D Textwo = Content.Load<Texture2D>("2");
            Texture2D Texone = Content.Load<Texture2D>("1");
            Texture2D Texgo = Content.Load<Texture2D>("go");
            Texture2D Texback = Content.Load<Texture2D>("track2");
            SoundEffect Seffect1 = Content.Load<SoundEffect>("BEEP1B");
            SoundEffect Seffect2 = Content.Load<SoundEffect>("StartBeep");
            countdown = new CountDown();
            countdown.InitializeCountDown(Texthree, Textwo, Texone, Texgo, Seffect1, Seffect2);//initializes the Countdown 
        }

        public void getspeedleft(User1 user1)
        {

            if (user1.Kneepos.Count() != 0)
            {
                if (user1.Kneepos.Count() == 1)
                {
                    min[0] = user1.Kneepos[0]; //set the first input to be minimum
                    min[1] = timer;
                }
                else
                {


                    if (user1.Kneepos[user1.Kneepos.Count() - 1] == user1.Kneepos[user1.Kneepos.Count() - 2]) // if the next input inlist equal the one b4 .. discard the one b4
                    {

                        if (user1.Kneepos[user1.Kneepos.Count() - 1] == max[0])
                        {
                            max[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                            max[1] = timer;
                        }
                        if (user1.Kneepos[user1.Kneepos.Count() - 1] == min[0])
                        {
                            min[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                            min[1] = timer;
                        }
                        user1.Kneepos.RemoveAt(user1.Kneepos.Count() - 2);
                        // and set one b4 to be the min
                    }
                    if (user1.Kneepos.Count() - 1 > user1.Kneepos.Count() - 2) // if the next input greater than the one b4 .. set it to max
                    {

                        calculatespeedbool = true;
                        max[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                        max[1] = timer;
                        user1.Kneepos.RemoveAt(user1.Kneepos.Count() - 2);

                    }
                    else // the next input is smaller than the one b4 .. 
                    {


                        if (calculatespeedbool)
                        {
                            speed = ((max[0] - min[0]) / (max[1] - min[1]));
                            speedlist[0] = speed;
                            speedlist[1] = timer;//calculate the speed of the oscillation from min to max
                            user1.Velocitylist.Add(speedlist);
                            calculatespeedbool = false;
                        }
                        min[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                        min[1] = timer;
                        user1.Kneepos.RemoveAt(user1.Kneepos.Count() - 2);


                    }
                }



            }
        }
        public void getspeedright(User1 user1)
        {
            if (user1.Kneeposr.Count() != 0)
            {
                if (user1.Kneeposr.Count() == 1)
                {
                    minr[0] = user1.Kneeposr[0]; //set the first input to be minimum
                    minr[1] = timer;
                }
                else
                {


                    if (user1.Kneeposr[user1.Kneeposr.Count() - 1] == user1.Kneeposr[user1.Kneeposr.Count() - 2]) // if the next input inlist equal the one b4 .. discard the one b4
                    {

                        if (user1.Kneeposr[user1.Kneeposr.Count() - 1] == maxr[0])
                        {
                            maxr[0] = user1.Kneeposr[user1.Kneeposr.Count() - 1];
                            maxr[1] = timer;
                        }
                        if (user1.Kneeposr[user1.Kneeposr.Count() - 1] == minr[0])
                        {
                            minr[0] = user1.Kneeposr[user1.Kneeposr.Count() - 1];
                            minr[1] = timer;
                        }
                        user1.Kneeposr.RemoveAt(user1.Kneeposr.Count() - 2);
                        // and set one b4 to be the min
                    }
                    if (user1.Kneeposr.Count() - 1 > user1.Kneeposr.Count() - 2) // if the next input greater than the one b4 .. set it to max
                    {

                        calculatespeedboolr = true;
                        maxr[0] = user1.Kneeposr[user1.Kneeposr.Count() - 1];
                        maxr[1] = timer;
                        user1.Kneeposr.RemoveAt(user1.Kneeposr.Count() - 2);

                    }
                    else // the next input is smaller than the one b4 .. 
                    {


                        if (calculatespeedboolr)
                        {
                            speedr = ((maxr[0] - minr[0]) / (maxr[1] - minr[1]));
                            speedlistr[0] = speedr;
                            speedlistr[1] = timer;//calculate the speed of the oscillation from min to max
                            user1.Velocitylist.Add(speedlistr);
                            calculatespeedboolr = false;
                        }
                        minr[0] = user1.Kneeposr[user1.Kneeposr.Count() - 1];
                        minr[1] = timer;
                        user1.Kneeposr.RemoveAt(user1.Kneeposr.Count() - 2);


                    }
                }



            }
        }
    
    
    }
}
