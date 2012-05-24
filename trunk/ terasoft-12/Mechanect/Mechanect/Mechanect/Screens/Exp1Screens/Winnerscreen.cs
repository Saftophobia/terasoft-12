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
        CountDown view;
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


        public Winnerscreen(User1 user1,User1 user2,GraphicsDevice device)
        {
            this.user1 = user1;
            this.user2 = user2;
            this.device = device;
        }

        public override void Initialize()
        {
            spritefont1 = Content.Load<SpriteFont>("SpriteFont1");
            isTwoPlayers = true;
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
            
            
              
            if (user1.Winner)
            {
                winningstring = " Player 1 is the winner!";
            }
            if (user2.Winner)
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

            }
            base.Draw(gameTime);
        }
    }

}
