using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mechanect.Classes;

namespace Mechanect.Screens
{
    class Experimentnew1 : Mechanect.Common.GameScreen
    {

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
            Environment1 = new Environment1(ScreenManager.Game.Content, ScreenManager.Game.GraphicsDevice,this.SpriteBatch);
            Environment1.LoadContent();
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
            Environment1.update(gameTime);

            base.Update(gameTime, covered);
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
        }

    }
}
