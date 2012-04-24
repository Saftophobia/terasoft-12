using Mechanect.Common;
using Microsoft.Xna.Framework;
using MechanectXNA;
namespace Mechanect.Screens
{
    class InstructionsScreen3 : Instruction
    {
        Vector2 position = new Vector2(100, 20);

        string instructions = " The point of this game is to shoot the ball that it reaches the hole with zero velocity";
        public InstructionsScreen3()
            : base()
        {
        }
        public InstructionsScreen3(string instructions)
            : base(instructions)
        {
            this.instructions = instructions;
        }

        public override void LoadContent()
        {
            base.LoadContent();
           
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {


            base.Update(gameTime, covered);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Draws the how to play instruction screen related to experiment3.
        /// </summary>
        /// <param name="gameTime">
        /// An instance variable of class GameTime in Microsoft.Xna.Framework.GameTime package.
        /// </param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Remove()
        {
            base.Remove();
        }
    }
}
