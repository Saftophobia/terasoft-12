﻿using Microsoft.Xna.Framework;


namespace Mechanect.Screens
{
    class CommonPauseScreen : Instruction
    {
        string instructions = "User is not detected by kinect device please stand in correct position then press ok";
        public CommonPauseScreen()
        {
            new Instruction(instructions);
        }

        public CommonPauseScreen(string instructions)
            : base(instructions)
        {
            this.instructions = instructions;
        }
        
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// LoadContent will be called only once before drawing and its the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
        }
        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// Allows the game screen to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">Determines whether you want this screen to be covered by another screen or not.</param>
        public override void Update(GameTime gameTime, bool covered)
        {
            base.Update(gameTime, covered);
        }

        /// <remarks>
        ///<para>AUTHOR: Khaled Salah </para>
        ///</remarks>
        /// <summary>
        /// This is called when the game screen should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>    
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        
    }
}
