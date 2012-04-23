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
        CountDown countdown;
        CountDown background;
        int timer = 0;
        List<int> timeslice;
        List<String> gCommands = new List<string> { "constantDisplacement", "constantAcceleration", "increasingAcceleration", "decreasingAcceleration", "constantVelocity" };
        List<String> racecommands;
        List<String> racecommandsforDRAW;
        

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


            //------------------------------------------------------------------------
           






        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {




            base.Update(gameTime, covered);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}
