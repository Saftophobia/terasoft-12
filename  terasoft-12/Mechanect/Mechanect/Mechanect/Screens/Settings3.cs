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
        Button OKbutton;
        Slider velocity;
        Slider angle;

        levelSelect level;

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Initializing the OK button and the two sliders of the velocity and angle
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
          level.Initialize();
          this.ScreenManager.Game.Components.Add(level);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// updating the button and the two sliders. It's requiered to make them run correctly
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="covered"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            if (OKbutton.isClicked())
            {
                //setToleranceOfAngle(angle.getValue());
                //setToleranceOfVelocity(velcoity.getValue());
                this.Remove();
            }
            OKbutton.update(gameTime);
            velocity.update();
            angle.update();
            base.Update(gameTime, covered);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// drwing the OK button and the two slider on the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            velocity.draw(this.ScreenManager.SpriteBatch);
            angle.draw(this.ScreenManager.SpriteBatch);
            OKbutton.draw(this.ScreenManager.SpriteBatch);
        }

    }
}
