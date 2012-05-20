using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Mechanect.Exp1;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Kinect;

namespace Mechanect.Screens.Exp1Screens
{
    class Exp1 : Mechanect.Common.GameScreen
    {
        #region KeeVariables
        float firstframe = 5000;
        float speed = 5000;
        float speed2 = 50000;
        float speedr = 50000;
        float speedr2 = 50000;
        float[] speedlist = new float[2];
        float[] speedlistr = new float[2];
        float[] speedlist2 = new float[2];
        float[] speedlistr2 = new float[2];

        bool calculatespeedbool = false;
        bool calculatespeedbool2 = false;
        bool calculatespeedboolr = false;
        bool calculatespeedboolr2 = false;

        float[] min = new float[2];
        float[] minr = new float[2];
        float[] min2 = new float[2];
        float[] minr2 = new float[2];
        float[] max = new float[2];
        float[] maxr = new float[2];
        float[] max2 = new float[2];
        float[] maxr2 = new float[2];

        Joint left, right;
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
        float timer = 0;
        CountDown countdown;
        public Exp1(User1 user1, User1 user2)
        {
            this.user1 = user1;
            this.user2 = user2;
        }
        public override void Initialize()
        {
            kinect = new MKinect();
            base.Initialize();
        }
        public override void LoadContent()
        {
            graphics = this.ScreenManager.GraphicsDevice;
            Environment1 = new Environment1(ScreenManager.Game.Content, ScreenManager.Game.GraphicsDevice, this.SpriteBatch);
            Environment1.LoadContent();
            loadcountdown();
        }
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
                    //I commented these to have a compilation-error free repo
                    //Tools1.getspeedl(user1, speed, speedlist, calculatespeedbool, max, min, timer,this.Environment1);
                    //Tools1.getspeedr(user1, speedr, speedlistr, calculatespeedboolr, maxr, minr, timer, this.Environment1);
                    //Tools1.getspeedl(user2, speed2, speedlist2, calculatespeedbool2, max2, min2, timer, this.Environment1);
                    //Tools1.getspeedr(user2, speedr2, speedlistr2, calculatespeedboolr2, maxr2, minr2, timer, this.Environment1);
                    if (firstframe != (user1.skeleton.Joints[JointType.KneeLeft].Position.Y))
                    {
                        fill_Knee_pos();
                        getspeedleft();
                        getspeedleft2();
                        //   getspeedright();
                        //  getspeedright2();
                        firstframe = (user1.skeleton.Joints[JointType.KneeLeft].Position.Y);
                    }
                    if (timer > 29)
                    {
                        int b = 0;
                    }

                    //display commands on screen
                    //check micho's method
                    //check micho's winning method
                }

            }

            if (user1.Winner || user2.Winner || (user1.Disqualified && user2.Disqualified))
            {
                ScreenManager.AddScreen(new Winnerscreen(user1,user2));
                Remove();
            }
            base.Update(gameTime);
        }
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
        public void fill_Knee_pos()
        {

            if (user1.skeleton.Position.X > user2.skeleton.Position.X)
            {
                user1.Kneepos.Add((float)Math.Round((user1.skeleton.Joints[JointType.KneeLeft].Position.Y), 2));
                user2.Kneepos.Add((float)Math.Round((user2.skeleton.Joints[JointType.KneeLeft].Position.Y), 2));

                user1.Kneeposr.Add((float)Math.Round((user1.skeleton.Joints[JointType.KneeRight].Position.Y), 2));
                user2.Kneeposr.Add((float)Math.Round((user2.skeleton.Joints[JointType.KneeRight].Position.Y), 2));


            }
            else
            {
                user2.Kneepos.Add((float)Math.Round((user1.skeleton.Joints[JointType.KneeLeft].Position.Y), 2));
                user1.Kneepos.Add((float)Math.Round((user2.skeleton.Joints[JointType.KneeLeft].Position.Y), 2));

                user2.Kneeposr.Add((float)Math.Round((user1.skeleton.Joints[JointType.KneeRight].Position.Y), 2));
                user1.Kneeposr.Add((float)Math.Round((user2.skeleton.Joints[JointType.KneeRight].Position.Y), 2));
            }
        }
        public void getspeedleft2()
        {

            if (user2.Kneepos.Count() != 0)
            {
                if (user2.Kneepos.Count() == 1)
                {
                    min2[0] = user2.Kneepos[0]; //set the first input to be minimum
                    min2[1] = timer;
                }
                else
                {


                    if (user2.Kneepos[user2.Kneepos.Count() - 1] == user2.Kneepos[user2.Kneepos.Count() - 2]) // if the next input inlist equal the one b4 .. discard the one b4
                    {

                        if (user2.Kneepos[user2.Kneepos.Count() - 1] == max2[0])
                        {
                            max2[0] = user2.Kneepos[user2.Kneepos.Count() - 1];
                            max2[1] = timer;
                        }
                        if (user2.Kneepos[user2.Kneepos.Count() - 1] == min2[0])
                        {
                            min2[0] = user2.Kneepos[user2.Kneepos.Count() - 1];
                            min2[1] = timer;
                        }
                        user2.Kneepos.RemoveAt(user2.Kneepos.Count() - 2);
                        // and set one b4 to be the min
                    }
                    else
                    {
                        if (user2.Kneepos[user2.Kneepos.Count() - 1] > user2.Kneepos[user2.Kneepos.Count() - 2]) // if the next input greater than the one b4 .. set it to max
                        {

                            calculatespeedbool2 = true;
                            max2[0] = user2.Kneepos[user2.Kneepos.Count() - 1];
                            max2[1] = timer;
                            user2.Kneepos.RemoveAt(user2.Kneepos.Count() - 2);

                        }
                        else // the next input is smaller than the one b4 .. 
                        {


                            if (calculatespeedbool2)
                            {
                                if ((max2[0] - min2[0]) >= 0.3)
                                {

                                    speed2 = Math.Abs(((max2[0] - min2[0]) / (max2[1] - min2[1])));
                                    float[] speedlist2 = new float[2];
                                    speedlist2[0] = speed2;
                                    speedlist2[1] = timer;//calculate the speed of the oscillation from min to max
                                    user2.Velocitylist.Add(speedlist2);


                                    this.Environment1.MoveAvatar(2, (int)speed2 * 15);


                                    calculatespeedbool2 = false;
                                }
                                else
                                {
                                    calculatespeedbool2 = false;
                                }
                            }
                            min2[0] = user2.Kneepos[user2.Kneepos.Count() - 1];
                            min2[1] = timer;
                            user2.Kneepos.RemoveAt(user2.Kneepos.Count() - 2);


                        }
                    }


                }
            }
        }
        public void getspeedright2()
        {

            if (user2.Kneeposr.Count() != 0)
            {
                if (user2.Kneeposr.Count() == 1)
                {
                    minr2[0] = user2.Kneeposr[0]; //set the first input to be minimum
                    minr2[1] = timer;
                }
                else
                {


                    if (user2.Kneeposr[user2.Kneeposr.Count() - 1] == user2.Kneeposr[user2.Kneeposr.Count() - 2]) // if the next input inlist equal the one b4 .. discard the one b4
                    {

                        if (user2.Kneeposr[user2.Kneeposr.Count() - 1] == maxr2[0])
                        {
                            maxr2[0] = user2.Kneeposr[user2.Kneeposr.Count() - 1];
                            maxr2[1] = timer;
                        }
                        if (user2.Kneeposr[user2.Kneeposr.Count() - 1] == minr2[0])
                        {
                            minr2[0] = user2.Kneeposr[user2.Kneeposr.Count() - 1];
                            minr2[1] = timer;
                        }
                        user2.Kneeposr.RemoveAt(user2.Kneeposr.Count() - 2);
                        // and set one b4 to be the min
                    }
                    else
                    {
                        if (user2.Kneeposr[user2.Kneeposr.Count() - 1] > user2.Kneeposr[user2.Kneeposr.Count() - 2]) // if the next input greater than the one b4 .. set it to max
                        {

                            calculatespeedboolr2 = true;
                            maxr2[0] = user2.Kneeposr[user2.Kneeposr.Count() - 1];
                            maxr2[1] = timer;
                            user2.Kneeposr.RemoveAt(user2.Kneeposr.Count() - 2);

                        }
                        else // the next input is smaller than the one b4 .. 
                        {


                            if (calculatespeedboolr2)
                            {
                                if ((maxr2[0] - minr2[0]) >= 0.3)
                                {
                                    speedr2 = Math.Abs(((maxr2[0] - minr2[0]) / (maxr2[1] - minr2[1])));
                                    float[] speedlistr2 = new float[2];
                                    speedlistr2[0] = speedr2;
                                    speedlistr2[1] = timer;//calculate the speed of the oscillation from min to max
                                    user2.Velocitylist.Add(speedlistr2);


                                    this.Environment1.MoveAvatar(2, (int)speedr2 * 15);


                                    calculatespeedboolr2 = false;
                                }
                                else
                                {
                                    calculatespeedboolr2 = false;
                                }
                            }
                            minr2[0] = user2.Kneeposr[user2.Kneeposr.Count() - 1];
                            minr2[1] = timer;
                            user2.Kneeposr.RemoveAt(user2.Kneeposr.Count() - 2);






                        }
                    }
                }
            }
        }
        public void getspeedleft()
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
                    else
                    {
                        if (user1.Kneepos[user1.Kneepos.Count() - 1] > user1.Kneepos[user1.Kneepos.Count() - 2]) // if the next input greater than the one b4 .. set it to max
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
                                if ((max[0] - min[0]) >= 0.3)
                                {
                                    speed = Math.Abs(((max[0] - min[0]) / (max[1] - min[1])));
                                    float[] speedlist = new float[2];
                                    speedlist[0] = speed;
                                    speedlist[1] = timer;//calculate the speed of the oscillation from min to max
                                    user1.Velocitylist.Add(speedlist);



                                    this.Environment1.MoveAvatar(1, (int)speed * 15);



                                    calculatespeedbool = false;
                                }
                                else
                                {
                                    calculatespeedbool = false;
                                }
                            }
                            min[0] = user1.Kneepos[user1.Kneepos.Count() - 1];
                            min[1] = timer;
                            user1.Kneepos.RemoveAt(user1.Kneepos.Count() - 2);


                        }
                    }


                }
            }
        }
        public void getspeedright()
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
                    else
                    {
                        if (user1.Kneeposr[user1.Kneeposr.Count() - 1] > user1.Kneeposr[user1.Kneeposr.Count() - 2]) // if the next input greater than the one b4 .. set it to max
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
                                if ((maxr[0] - minr[0]) >= 0.3)
                                {
                                    speedr = Math.Abs(((maxr[0] - minr[0]) / (maxr[1] - minr[1])));
                                    float[] speedlistr = new float[2];
                                    speedlistr[0] = speedr;
                                    speedlistr[1] = timer;//calculate the speed of the oscillation from min to max
                                    user1.Velocitylist.Add(speedlistr);

                                    if (timer > 20)
                                    {
                                        string x = "";
                                    }
                                    this.Environment1.MoveAvatar(1, (int)speedr * 15);



                                    calculatespeedboolr = false;

                                }
                                else
                                {
                                    calculatespeedboolr = false;
                                }
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
}
