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

namespace Race
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int player1DisqualificationTime;
        int player2DisqualificationTime;
        CountDown One;
        CountDown two;
        CountDown Three;
        CountDown go;
        CountDown background;
        SoundEffect effect1;
        SoundEffect effect2;
        Boolean play1 = true;
        Boolean play2 = true;
        Boolean play3 = true;
        Boolean play4 = true;


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The constructor Game() is used to create an 
        /// instance of the Game that will be runned in
        /// Program.cs
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is public
        /// </permission>
        /// <returns></returns>
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The Initialize function is used to create instances
        /// of the Game's fields
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is protected
        /// </permission>
        /// <returns></returns>

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Three = new CountDown(Content.Load<Texture2D>("MechanectContent/3"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 300, 150, 200, 200);
            two = new CountDown(Content.Load<Texture2D>("MechanectContent/2"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 300, 150, 200, 200);
            One = new CountDown(Content.Load<Texture2D>("MechanectContent/1"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 300, 150, 200, 200);
            go = new CountDown(Content.Load<Texture2D>("MechanectContent/go"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 330, 150, 150, 150);
            background = new CountDown(Content.Load<Texture2D>("MechanectContent/track2"), graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight, 0,0,800,490);
            effect1 = Content.Load<SoundEffect>("MechanectContent/BEEP1B");
            effect2 = Content.Load<SoundEffect>("MechanectContent/StartBeep");
            base.Initialize();
        }
        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function LoadContent instantiates the spriteBatch field
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is protected
        /// </permission>
        /// <returns></returns>

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// 
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is protected
        /// </permission>
        /// <returns></returns>

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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
        /// <returns>int: returns the time where player 1 was
        /// disqualified</returns>
        public int GetPlayer1Disq()
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
        /// <returns>int: returns the time where player 2 was
        /// disqualified</returns>
        public int GetPlayer2Disq()
        {
            return player2DisqualificationTime;
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The Update function is used to call the Update function
        /// for every instance of Countdown in order to decrement their
        /// counter to allow the next instance to appear on the screen
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is protected
        /// </permission>
        /// <returns></returns>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (Three.GetCounter() > 0)
            {
                Three.Update();
                if (play1)
                {
                    effect1.Play();
                    play1 = false;
                }
            }
            if (Three.GetCounter() == 0 && two.GetCounter() > 0)
            {
                two.Update();
                if (play2)
                {
                    effect1.Play();
                    play2 = false;
                }
            }
            if (two.GetCounter() == 0 && Three.GetCounter() == 0 && One.GetCounter() > 0)
            {
                One.Update();
                if (play3)
                {
                    effect1.Play();
                    play3 = false;
                }
            }
            if (One.GetCounter() == 0 && two.GetCounter() == 0 && Three.GetCounter() == 0 && go.GetCounter() > 0)
            {
                if (play4)
                {
                    effect2.Play();
                    play4 = false;
                }
                
            }
            base.Update(gameTime);
        }


        /// <remarks>
        /// <para>Author: Ahmed Shirin</para>
        /// <para>Date Written 20/4/2012</para>
        /// <para>Date Modified 20/4/2012</para>
        /// </remarks>
        /// <summary>
        /// The function Draw is used to draw the instances of
        /// Countdown when the counter of the previous instance
        /// reaches 0
        /// </summary>
        /// <param></param>        
        /// <permission cref="System.Security.PermissionSet">
        /// This function is protected
        /// </permission>
        /// <returns></returns>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            background.Draw(spriteBatch);
            if (Three.GetCounter() > 0)
            {
                Three.Draw(spriteBatch);
            }
            if (Three.GetCounter() == 0 && two.GetCounter() > 0)
            {
                two.Draw(spriteBatch);
            }
            if (two.GetCounter() == 0 && Three.GetCounter() == 0 && One.GetCounter() > 0)
            {
                One.Draw(spriteBatch);
            }
            if (One.GetCounter() == 0 && two.GetCounter() == 0 && Three.GetCounter() == 0 && go.GetCounter() > 0)
            {
                go.Draw(spriteBatch);
            }

            base.Draw(gameTime);
        }
    }
}
