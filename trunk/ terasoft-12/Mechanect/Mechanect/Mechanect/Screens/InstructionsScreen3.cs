using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Mechanect.Screens
{
    class InstructionsScreen3 : GameScreen
    {
        public InstructionsScreen3()
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
        /// Draws the how to play instruction screen related to experiment3
        /// </summary>
        /// <param name="gameTime">
        /// an instance variable of class GameTime in Microsoft.Xna.Framework.GameTime package
        /// </param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Vector2 position = new Vector2(100, 20);
            Instruction I = new Instruction("The goal of this game is to shoot the ball such that it reaches the hole with 0 velocity", position);
            I.Draw();
        }
    
        public override void Remove()
        {
            base.Remove();
        }
    }
}
