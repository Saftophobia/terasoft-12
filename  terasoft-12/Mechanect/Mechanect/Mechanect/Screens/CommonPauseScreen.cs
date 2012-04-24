using Microsoft.Xna.Framework;
using MechanectXNA;


namespace Mechanect.Screens
{
    class CommonPauseScreen : Instruction
    {
        Vector2 position = new Vector2(100, 20);
        string instructions = "User is not detected by kinect device please stand in correct position then press ok";
        public CommonPauseScreen()
            : base()
        {
        }

        public CommonPauseScreen(string instructions)
            : base(instructions)
        {
            this.instructions = instructions;
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool covered)
        {
            base.Update(gameTime, covered);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Draws the pause screen which should be shown if the user is not detected by the kinect device.
        /// </summary>     
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
