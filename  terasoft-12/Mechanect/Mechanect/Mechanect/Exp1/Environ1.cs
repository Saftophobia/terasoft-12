using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using UI.Experiment1;

namespace Mechanect.Exp1
{
    class Environ1
    {
        ContentManager Content;
        GraphicsDevice device;
        SpriteBatch spriteBatch;
        CountDown background;
        CountDown view;
        CountDown finish;
        CountDown one;
        CountDown two;
        CountDown start;
        CountDown flag1;
        CountDown flag2;
        public Moving2DAvatar bike1;
        public Moving2DAvatar bike2;
        SoundEffect Chants;
        SoundEffectInstance ChantsInstance;
        
        
        Boolean t = false;
        /// <summary>
        /// Initializes the Environment, and Calling the Content, Graphicsdevice and the SpriteBatch from the ScreenManager.
        /// </summary>
        /// <param name="content">Initializes content from screenManager</param>
        /// <param name="device">Initializes graphics device from screenManager</param>
        /// <param name="spriteBatch">Initializes sprite batch from screenManager</param>
        /// <para>AUTHOR: Safty </para>
        /// <para>DATE WRITTEN: 24/5/12 </para>
        /// <para>DATE MODIFIED: 24/5/12 </para>
        /// </remarks>
        public Environ1(ContentManager content, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            this.Content = content;
            this.device = device;
            this.spriteBatch = spriteBatch;
        }
        /// <summary>
        /// Loads the Textures from the Content Manager.
        /// </summary>
        /// <para>AUTHOR: Safty </para>
        /// <para>DATE WRITTEN: 24/5/12 </para>
        /// <para>DATE MODIFIED: 24/5/12 </para>
        /// </remarks>
        public void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.device);
            background = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/back"), (device.DisplayMode.Width / 2) - 70, 0, 150, device.DisplayMode.Height);
            view = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/track"), 0, 0, device.DisplayMode.Width, device.DisplayMode.Height);
            finish = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/finish"), (device.DisplayMode.Width / 2) - 70, 0, 150, 40);
            one = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/2"), (device.DisplayMode.Width / 2) + 30, device.DisplayMode.Height - 35, 28, 30);
            two = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/1"), (device.DisplayMode.Width / 2) - 45, device.DisplayMode.Height - 35, 20, 28);
            start = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/start"), (device.DisplayMode.Width / 2) - 82, device.DisplayMode.Height - 65, 185, 50);
            flag1 = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/flag1"), (device.DisplayMode.Width / 2) + 61, 0, 50, 50);
            flag2 = new CountDown(Content.Load<Texture2D>("Exp1/2Dcontent/flag2"), (device.DisplayMode.Width / 2) - 100, 0, 50, 50);
            bike1 = new Moving2DAvatar(Content.Load<Texture2D>("Exp1/2Dcontent/bike"), new Vector2((device.DisplayMode.Width / 2) - 45, (int)(device.DisplayMode.Height  * 0.92)));
            bike2 = new Moving2DAvatar(Content.Load<Texture2D>("Exp1/2Dcontent/bike"), new Vector2((device.DisplayMode.Width / 2) + 30, (int)(device.DisplayMode.Height * 0.92)));
            Chants = Content.Load<SoundEffect>("Exp1/2Dcontent/Crowd1");            
            ChantsInstance = Chants.CreateInstance();
            ChantsInstance.Volume = 0.3f;
        }
        /// <summary>
        /// Updates the Logic of the Environment, including the Cheers.
        /// </summary>
        /// <para>AUTHOR: Safty </para>
        /// <para>DATE WRITTEN: 24/5/12 </para>
        /// <para>DATE MODIFIED: 24/5/12 </para>
        /// </remarks>
        public void Update()
        {
            if (!t)//Condition to prevent replaying of the soundtrack
            {
                ChantsInstance.IsLooped = true;
                ChantsInstance.Play();
                t = true;
            }
           
        }
        /// <summary>
        /// Draws the Environment, Flag, background and Bikes.
        /// </summary>
        /// <para>AUTHOR: Safty </para>
        /// <para>DATE WRITTEN: 24/5/12 </para>
        /// <para>DATE MODIFIED: 24/5/12 </para>
        /// </remarks>
        public void Draw()
        {           
            view.Draw(spriteBatch);
            background.Draw(spriteBatch);
            finish.Draw(spriteBatch);
            one.Draw(spriteBatch);
            two.Draw(spriteBatch);
            flag1.Draw(spriteBatch);
            flag2.Draw(spriteBatch);
            start.Draw(spriteBatch);
            bike1.Draw(spriteBatch);
            bike2.Draw(spriteBatch);
            
        }

    }
}
