using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Exp1;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Mechanect.Screens.Exp1Screens
{
    class Winnerscreen:Mechanect.Common.GameScreen
    {
        float timer;
        SoundEffectInstance CheersInstance;
        SoundEffect Cheers;
        PerformanceGraph Graph;
        CountDown view;
        List<string> racecommands;
        List<int> timeslice;
        GraphicsDevice device;
        User1 user1, user2;
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
        SpriteFont spritefont1;
        string winningstring;


        public Winnerscreen(User1 user1,User1 user2,GraphicsDevice device,List<string> racecommands,List<int> timeslice)
        {
            this.user1 = user1;
            this.user2 = user2;
            this.device = device;
            this.racecommands = racecommands;
            this.timeslice = timeslice;
        }

        public override void Initialize()
        {
            spritefont1 = Content.Load<SpriteFont>("SpriteFont1");
            isTwoPlayers = true;
            Graph = new PerformanceGraph();
            List<float> vel1 = new List<float>();
            List<double> time1 = new List<double>();
            List<float> vel2 = new List<float>();
            List<double> time2 = new List<double>();
            vel1.Add(0);
            vel2.Add(0);
            time1.Add(0);
            time2.Add(0);

            for (int i = 0; i < user1.Velocitylist.Count(); i++)
            {
                vel1.Add(user1.Velocitylist[i][0]);
                time1.Add(user1.Velocitylist[i][1]);
            }
            for (int i = 0; i < user2.Velocitylist.Count(); i++)
            {

                vel2.Add(user2.Velocitylist[i][0]);
                time2.Add(user2.Velocitylist[i][1]);
            }

            List<double> timeslicedouble = new List<double>();
            foreach (float time in this.timeslice)
            {
                timeslicedouble.Add((double)time);
            } // change this typecasting
           
            GraphEngine.DrawGraphs(Graph, vel1, time1, vel2, time2, this.racecommands, timeslicedouble, user1.DisqualificationTime, user2.DisqualificationTime, device.DisplayMode.Width, device.DisplayMode.Height, (int)(device.DisplayMode.Height * 1.1));
            

            base.Initialize();

        }
        public override void LoadContent()
        {
            winningstring = "";
            view = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/chearingbackground"), 0, 0, device.DisplayMode.Width, device.DisplayMode.Height);
            Cheers = Content.Load<SoundEffect>("Exp1/2Dcontent/Crowd2");
            CheersInstance = Cheers.CreateInstance();
            CheersInstance.Play();
            //load the background
            //load the soundeffect
           
            base.LoadContent();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //who is winner and who is disqualified
            
            
              
            if (user1.Winner && !user1.Disqualified)
            {
                winningstring = " Player 1 is the winner!";
            }
            if (user2.Winner && !user2.Disqualified)
            {
                winningstring = "Player 2 is the winner!";
            }
            if(user1.Disqualified && user1.Disqualified)
            {
                winningstring = " Damn, you guys sucks!!";
            }

            if (timer > 6)
            {
                
                
            }
            base.Update(gameTime);
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
           //Draw the Background
            //draw the Font
            view.Draw(SpriteBatch);
            if (timer < 6)
            {
                SpriteBatch.Begin();
                SpriteBatch.DrawString(spritefont1, winningstring, new Vector2((int)(device.DisplayMode.Width * 0.35), (int)(device.DisplayMode.Height * 0.3)), Microsoft.Xna.Framework.Color.Black);
                SpriteBatch.End();
            }
            else
            {
                SpriteFont font = Content.Load<SpriteFont>("SpriteFont1");
                SpriteFont font2 = Content.Load<SpriteFont>("SpriteFont1");
                //Texture2D P1Tex = Content.Load<Texture2D>("MechanectContent/xRed");
                //Texture2D P2Tex = Content.Load<Texture2D>("MechanectContent/xBlue");
                SpriteBatch.Begin();
                //GraphUI.DrawRange(SpriteBatch, device, Graph);
                GraphUI.DrawEnvironment(Graph, SpriteBatch, device, font, font2);
                //  GraphUI.DrawDisqualification(Graph, spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, P1Tex, P2Tex, player1DisqualificationTime, player2DisqualificationTime);
                SpriteBatch.End();
                
            }
            base.Draw(gameTime);
        }
    }

}
