using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Common;
using Microsoft.Xna.Framework;

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
            var position = new Vector2(100, 20);
            string instructions = "The goal of this game is to shoot the ball such that it reaches the hole with 0 velocity";
            var I = new Instruction(instructions, position);
            I.Draw();
        }
    
        public override void Remove()
        {
            base.Remove();
        }
    }
}
