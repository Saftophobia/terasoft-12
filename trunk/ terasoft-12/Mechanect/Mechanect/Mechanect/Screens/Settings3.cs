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
        private float scaleH;
        private float scaleW;


        /// <summary>
        /// The constructor of the settings screen for experiment 3.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        /// <param name="user">The instance of the user.</param>
        public Settings3(User user)
        {
            this.user = user;
        }

        
        /// <summary>
        /// Loading the OK button and the two sliders of the velocity and angle and the levels slider.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        public override void LoadContent()
        {
            int screenWidth = this.ScreenManager.GraphicsDevice.Viewport.Width;
            int screenHeight = this.ScreenManager.GraphicsDevice.Viewport.Height;
            ContentManager contentManager = this.ScreenManager.Game.Content;

            OKbutton = Tools3.OKButton(contentManager,
            new Vector2((int)(screenWidth * 0.38), (int)(screenHeight * 0.68)), screenWidth,
            screenHeight, user);

            velocity = new Slider(new Vector2((int)(screenWidth * 0.35), (int)(screenHeight * 0.5)), 
                screenWidth, screenHeight, user);

            angle = new Slider(new Vector2((int)(screenWidth * 0.35), (int)(screenHeight * 0.6)), 
                screenWidth, screenHeight, user);


            level = new levelSelect(this.ScreenManager.Game, new Vector2(110, 125), user);


            velocity.LoadContent(contentManager);
            angle.LoadContent(contentManager);

            backGround = contentManager.Load<Texture2D>("Textures/Screens/settings");

            scaleH = ((float) screenHeight / (float) backGround.Height);
            scaleW = ((float)screenWidth / (float)backGround.Width);

            level.Initialize(screenWidth, screenHeight, scaleH);

            base.LoadContent();
        }



        /// <summary>
        /// Updates the button, the two sliders and the levels slider. It's required to make them run correctly.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (OKbutton.IsClicked())
            {
                Environment3.angleTolerance = angle.Value;
                Environment3.velocityTolerance = velocity.Value;
                ScreenManager.AddScreen(new Experiment3((User3)user));
                Remove();
            }
            OKbutton.Update(gameTime);
            velocity.Update(gameTime);
            angle.Update(gameTime);
            level.Update(gameTime);
            base.Update(gameTime);
        }


        /// <summary>
        /// Draws the OK button, the two slider and the levels slider on the screen.
        /// </summary>
        /// <remarks>
        /// <para>Author: HegazY</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
                        
            Rectangle rect = new Rectangle(0, 0, (int)(scaleW * backGround.Width), (int)(scaleH * backGround.Height));
            spriteBatch.Begin();
            spriteBatch.Draw(backGround, rect, Color.White);
            level.Draw(spriteBatch, scaleW);
            velocity.Draw(spriteBatch, scaleW, scaleH);
            angle.Draw(spriteBatch, scaleW, scaleH);
            OKbutton.Draw(spriteBatch, scaleW, scaleH);
            OKbutton.DrawHand(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
