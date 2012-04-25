using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Mechanect.Classes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        double player1DisqualificationTime = -1; //Obtained by user story 3.7
        double player2DisqualificationTime = 1; //Obtained by user story 3.7   
        PerformanceGraph Graph;
        CountDown countdown;
        Boolean displayCountdown = false;
        CountDown background;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IntPtr ptr = this.Window.Handle;
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(ptr);
            form.Size = new System.Drawing.Size(1024, 768);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 650;
            graphics.ApplyChanges();
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>        
        /// </summary>
        /// <param></param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        protected override void Initialize()
        {
            if (displayCountdown)
            {
                Texture2D Texthree = Content.Load<Texture2D>("MechanectContent/3");
                Texture2D Textwo = Content.Load<Texture2D>("MechanectContent/2");
                Texture2D Texone = Content.Load<Texture2D>("MechanectContent/1");
                Texture2D Texgo = Content.Load<Texture2D>("MechanectContent/go");
                Texture2D Texback = Content.Load<Texture2D>("MechanectContent/track2");
                SoundEffect Seffect1 = Content.Load<SoundEffect>("MechanectContent/BEEP1B");
                SoundEffect Seffect2 = Content.Load<SoundEffect>("MechanectContent/StartBeep");
                //countdown = new CountDown(Texthree, Textwo, Texone, Texgo, Texback, Seffect1, Seffect2, graphics);
                background = new CountDown(Texback, graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 0, 0, 1024, 768);
            }

            if (!displayCountdown)
            {
                background = new CountDown(Content.Load<Texture2D>("MechanectContent/Background2"), graphics.PreferredBackBufferWidth,
                    graphics.PreferredBackBufferHeight, 0, 0, 1024, 768);

                Graph = new PerformanceGraph();
                List<string> Commands = new List<string>();
                List<double> CommandsTime = new List<double>();
                List<float> Player1Displacement = new List<float>();
                List<float> Player2Displacement = new List<float>();
                //initiating testing values 
                Commands.Add("increasingAcceleration");
                CommandsTime.Add(4);
                int intitial = 4000;
                int stepping = 2;
                for (int i = 0; i <= 95; i++)
                {
                    if (intitial > 0)
                    {
                        Player1Displacement.Add(intitial);
                        intitial = intitial - stepping;
                        stepping = stepping + 2;
                    }
                    else
                    {
                        Player1Displacement.Add(0);
                    }
                }
                intitial = 4000;
                stepping = 1;
                for (int i = 0; i <= 95; i++)
                {
                    if (intitial > 0)
                    {
                        Player2Displacement.Add(intitial);
                        stepping = stepping + 4;
                        intitial = intitial - stepping;
                    }
                    else
                    {
                        Player2Displacement.Add(0);
                    }
                }
                //main initializing method
                Graph.drawGraphs(Player1Displacement, Player2Displacement, Commands, CommandsTime, this);

            }
            base.Initialize();
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The Update function is used to call the Update function 
        /// in the PerformanceGraph class in order to allow the line
        /// to increment its x co-ordinates and increment/decrement its
        /// y co-ordinates till it reaches the specified range.
        /// 
        /// The Update function is also used to call the Update function
        /// in the Countdown class in order to shrink the displayed number
        /// till a counter reaches 0 allowing the next number to appear
        /// </summary> 
        /// <param></param>
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            SpriteBatch sprite2 = spriteBatch;
            sprite2.Begin();

            if (displayCountdown)
            {
                countdown.UpdateCountdownScreen();
            }
            sprite2.End();
            base.Update(gameTime);
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Created: 22-4-2012</para>
        /// <para>Date Modified: 22-4-2012</para>
        /// </remarks>
        /// <summary>
        /// The Draw function is used to draw the x-axis, the y-axis of the
        /// displacement, velocity and acceleration graphs, as well as drawing
        /// the lines connecting the ranges on the graphs, the Draw function is
        /// also used to draw the current countdown's number on the screen when
        /// the previous number's counter reaches zero
        /// </summary>
        /// <param name="gameTime">An instance of the GameTime class</param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (!displayCountdown)
            {
                background.Draw(spriteBatch);
                SpriteFont font = Content.Load<SpriteFont>("MechanectContent/MyFont1");
                SpriteFont font2 = Content.Load<SpriteFont>("MechanectContent/MyFont2");
                Texture2D P1Tex = Content.Load<Texture2D>("MechanectContent/xBlue");
                Texture2D P2Tex = Content.Load<Texture2D>("MechanectContent/xBlack");
                SpriteBatch sprite2 = spriteBatch;
                sprite2.Begin();
                Graph.drawRange(spriteBatch, GraphicsDevice);
                Graph.drawAxis(spriteBatch, GraphicsDevice, font, font2);
                sprite2.End();
            }

            if (displayCountdown)
            {
                background.Draw(spriteBatch);
                countdown.DrawCountdownScreen(spriteBatch);
            }

            base.Draw(gameTime);
        }




        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

        }


        protected override void UnloadContent()
        {

        }
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetPlayer1Disq is used to get the time where
        /// player 1 was disqualified
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>double: returns the time where player 1 was
        /// disqualified</returns>
        public double GetPlayer1Disq()
        {
            return player1DisqualificationTime;
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function GetPlayer2Disq is used to get the time where
        /// player 2 was disqualified
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns>double: returns the time where player 2 was
        /// disqualified</returns>
        public double GetPlayer2Disq()
        {
            return player2DisqualificationTime;
        }

        public GraphicsDeviceManager getGraphicsDeviceManager()
        {
            return graphics;
        }
    }
}