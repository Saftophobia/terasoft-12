using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Exp1;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Mechanect.Screens.Exp1Screens
{
    class Winnerscreen:Mechanect.Common.GameScreen
    {
        float timer;
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


        public Winnerscreen(User1 user1,User1 user2)
        {
            this.user1 = user1;
            this.user2 = user2;

        }

        public override void Initialize()
        {
            spritefont1 = Content.Load<SpriteFont>("SpriteFont1");
            base.Initialize();
        }
        public override void LoadContent()
        {
            //load the background
            //load the soundeffect

            base.LoadContent();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds - 4;
            //who is winner and who is disqualified

            if (user1.Winner)
            {
                winningstring = " Winner1 is the winner!";
            }
            if (user2.Winner)
            {
                winningstring = "Winner2 is the winner!";
            }
            if(user1.Disqualified && user1.Disqualified)
            {
                winningstring = " No one made it to the finish line :-/ ";
            }

            if (timer > 5)
            {
                ScreenManager.AddScreen(new GraphScreen(user1,user2));
                Remove();
            }
            base.Update(gameTime);
        }
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
           //Draw the Background
            //draw the Font
            SpriteBatch.Begin();
            SpriteBatch.DrawString(spritefont1, winningstring, new Vector2(500, 500), Microsoft.Xna.Framework.Color.Black);
            SpriteBatch.End();
        }
    }

}
