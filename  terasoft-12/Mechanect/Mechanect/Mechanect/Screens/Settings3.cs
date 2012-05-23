using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ButtonsAndSliders;
using Mechanect.Exp3;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace Mechanect.Screens
{
    class Settings3 : GameScreen
    {
        private Button OKbutton;
        private Slider velocity;
        private Slider angle;
        private User user;
        private levelSelect level;

        private Texture2D backGround;
        private float scaleW;
        private float scaleH;

        public Settings3(User user)
        {
            this.user = user;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Initializing the OK button and the two sliders of the velocity and angle and the levels slider
        /// </summary>
        public override void LoadContent()
        {
            int screenWidth = this.ScreenManager.GraphicsDevice.Viewport.Width;
            int screenHeight = this.ScreenManager.GraphicsDevice.Viewport.Height;
            ContentManager contentManager = this.ScreenManager.Game.Content;

            OKbutton = Tools3.OKButton(contentManager,
            new Vector2((int)(screenWidth * 0.38), (int)(screenHeight * 0.68)), screenWidth,
            screenHeight, user);

            velocity = new Slider(new Vector2((int)(screenWidth * 0.35), (int)(screenHeight * 0.5)), screenWidth, screenHeight,
            contentManager, user);

            angle = new Slider(new Vector2((int)(screenWidth * 0.35), (int)(screenHeight * 0.6)), screenWidth, screenHeight,
            contentManager, user);


            level = new levelSelect(this.ScreenManager.Game, new Vector2(20, 50), 
              this.ScreenManager.SpriteBatch, user);

            level.Initialize(screenWidth, screenHeight);

            velocity.LoadContent();
            angle.LoadContent();

            backGround = contentManager.Load<Texture2D>("Textures/Screens/settings");

            scaleW = ((float) screenWidth / (float) backGround.Width);
            scaleH = ((float)screenHeight / (float)backGround.Height);

            base.LoadContent();
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// updating the button, the two sliders and the levels slider. It's requiered to make them run correctly
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="covered"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (OKbutton.IsClicked())
            {
                ScreenManager.AddScreen(new Experiment3((User3)user));
                Environment3.angleTolerance = angle.Value;
                Environment3.velocityTolerance = velocity.Value;
                Remove();
            }
            OKbutton.Update(gameTime);
            velocity.Update(gameTime);
            angle.Update(gameTime);
            level.Update(gameTime);
            base.Update(gameTime);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// drwing the OK button, the two slider on the screen and the levels slider
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
            //level.Draw(gameTime);
            Rectangle rect = new Rectangle(0, 0, (int)(scaleW * backGround.Width), (int)(scaleH * backGround.Height));
            spriteBatch.Begin();
            spriteBatch.Draw(backGround, rect, Color.White);
            OKbutton.Draw(spriteBatch, scaleW, scaleH);
            velocity.Draw(spriteBatch, scaleW, scaleH);
            angle.Draw(spriteBatch, scaleW, scaleH);
            OKbutton.DrawHand(spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

    }
}
