using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Common;
using Microsoft.Xna.Framework;
using Common.Classes;

namespace Mechanect.Screens
{
    class Settings3 : GameScreen
    {
        private Button OKbutton;
        private Slider velocity;
        private Slider angle;

        private levelSelect level;

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
          OKbutton = new OKButton(this.ScreenManager.Game.Content,
            new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Width / 2, this.ScreenManager.GraphicsDevice.Viewport.Height-400),
            this.ScreenManager.GraphicsDevice.Viewport.Width,
            this.ScreenManager.GraphicsDevice.Viewport.Height);

          velocity = new Slider(new Vector2(20, 200),
            this.ScreenManager.GraphicsDevice.Viewport.Width,
            this.ScreenManager.GraphicsDevice.Viewport.Height,
            this.ScreenManager.Game.Content);

          angle = new Slider(new Vector2(20, 400),
            this.ScreenManager.GraphicsDevice.Viewport.Width,
            this.ScreenManager.GraphicsDevice.Viewport.Height,
            this.ScreenManager.Game.Content);


          level = new levelSelect(this.ScreenManager.Game, new Vector2(20, 500), this.ScreenManager.SpriteBatch);

          level.Initialize(this.ScreenManager.GraphicsDevice.Viewport.Width, this.ScreenManager.GraphicsDevice.Viewport.Height);

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
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            if (OKbutton.IsClicked())
            {
                //setToleranceOfAngle(angle.getValue());
                //setToleranceOfVelocity(velcoity.getValue());
                this.Remove();
            }
            OKbutton.Update(gameTime);
            velocity.Update();
            angle.Update();
            level.Update(gameTime);
            base.Update(gameTime, covered);
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
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            level.Draw(gameTime);
            velocity.Draw(this.ScreenManager.SpriteBatch);
            angle.Draw(this.ScreenManager.SpriteBatch);
            OKbutton.Draw(this.ScreenManager.SpriteBatch);
        }

    }
}
