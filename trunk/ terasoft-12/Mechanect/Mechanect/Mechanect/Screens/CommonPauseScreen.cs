﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Classes;
using Microsoft.Xna.Framework;


namespace Mechanect.Screens
{
    class CommonPauseScreen : GameScreen
    {
        public CommonPauseScreen()
        {
        }


        public override void LoadContent()
        {
           
        }

        public override void UnloadContent()
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            base.Update(gameTime, covered);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Draws the pause screen which should be shown if the user is not detected by the kinect device
        /// </summary>     
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var position = new Vector2(100, 20);
            var I = new Instruction("No Player Detected, Please stand in the correct position then press ok ", position);
            I.Draw();
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}
