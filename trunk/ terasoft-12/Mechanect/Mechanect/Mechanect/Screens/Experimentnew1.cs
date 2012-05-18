using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
//using Mechanect.Experiment1;
using Microsoft.Kinect;
using Mechanect.Common;
using Microsoft.Xna.Framework.Audio;
using Mechanect.Exp1;

namespace Mechanect.Screens
{
    class Experimentnew1 : Mechanect.Common.GameScreen
    {
        #region kneevariables

        float speed = 0;
        float speed2 = 0;
        float speedr = 0;
        float speedr2 = 0;
        float[] speedlist = new float[2];
        float[] speedlistr = new float[2];
        float[] speedlist2 = new float[2];
        float[] speedlistr2 = new float[2];
       
        bool calculatespeedbool = false;
        bool calculatespeedboolr = false;
        bool calculatespeedbool2 = false;
        bool calculatespeedboolr2 = false;

        float[] max, min = new float[2];
        float[] maxr, minr = new float[2];
        float[] max2, min2 = new float[2];
        float[] maxr2, minr2 = new float[2];

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
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
            Environment1.update(gameTime);
            if (user1.skeleton == null || user2.skeleton == null)
            {
                Environment1.ChaseCam();
                kinect.requestSkeleton();
                user1.skeleton = kinect.globalSkeleton;
                kinect.request2ndSkeleton();
                user2.skeleton = kinect.globalSkeleton2;
                user1.Kneepos.Clear();
                user2.Kneepos.Clear();
                user1.Kneeposr.Clear();
                user2.Kneeposr.Clear();
                user1.Velocitylist.Clear();
                user2.Velocitylist.Clear();
                user1.Disqualified = user2.Disqualified = user1.Winner = user2.Winner = false;
            }
            else
            {

                timer += (float)gameTime.ElapsedGameTime.TotalSeconds - 4;
                countdown.Update();
                Environment1.TargetCam();
                if (timer > 0)
                {
                    fill_Knee_pos();
                    Tools1.getspeedl(user1, speed, speedlist, calculatespeedbool, max, min, timer,this.Environment1);
                    Tools1.getspeedr(user1, speedr, speedlistr, calculatespeedboolr, maxr, minr, timer, this.Environment1);
                    Tools1.getspeedl(user2, speed2, speedlist2, calculatespeedbool2, max2, min2, timer, this.Environment1);
                    Tools1.getspeedr(user2, speedr2, speedlistr2, calculatespeedboolr2, maxr2, minr2, timer, this.Environment1);
                    
                    
                    //display commands on screen
                    //check micho's method
                    //check micho's winning method
                }

            }

            if (user1.Winner || user2.Winner || (user1.Disqualified && user2.Disqualified))
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
            if (user1.skeleton == null || user2.skeleton == null)
            {
            }
            else
            {
                countdown.DrawCountdown(SpriteBatch);
                countdown.PlaySoundEffects();
                if (timer > 0)
                {
                    
                }
            }

           
        
        
        
        
        
        
        
        }
        /// <summary>
        /// Gets Data from kinect( Y position of knee) and adds it to the kneepos array at User1
        /// </summary>
        /// <para>Author: Safty</para>
        /// <para>Date Written 18/5/2012</para>
        /// <para>Date Modified 18/5/2012</para>
        /// </remarks>
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
    
    }
}
