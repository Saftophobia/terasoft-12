using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;

namespace Mechanect.Classes
{
     /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;  
        CountDown countdown;            
        Boolean displayCountdown = true;
        CountDown background;
        
        int timer = 0;


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
                countdown = new CountDown(Texthree, Textwo, Texone, Texgo, Texback, Seffect1, Seffect2, graphics);
                background = new CountDown(Texback, graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 0, 0, 1024, 768);
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
       
        public GraphicsDeviceManager getGraphicsDeviceManager()
        {
            return graphics;
        }
    }
}
