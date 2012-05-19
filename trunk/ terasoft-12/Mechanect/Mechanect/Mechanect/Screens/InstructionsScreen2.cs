 using Microsoft.Xna.Framework;
using Mechanect.Common;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Classes;
using Mechanect.Exp2;
namespace Mechanect.Screens
{
    class InstructionsScreen2 : GameScreen
    {
        string instructions = " Hey there instructions : \n \nThe Goal of the game is to throw the fish in the aquarium passing by the prey. You should move your arm from back to front to a certain angle with some velocity to determine the angle and velocity of projectile.";
        Instruction instruction;
        User2 user2;

        public InstructionsScreen2(User2 user2)
        {
            this.user2 = user2;
        }
        public InstructionsScreen2(string instructions, User2 user)
        {
            this.instructions = instructions;
            user2 = user;
        }

        public override void LoadContent()
        {   //I commented this line to have a compilation-error free repo
           // instruction = new Instruction(instructions, ScreenManager.Game.Content, ScreenManager.SpriteBatch, ScreenManager.GraphicsDevice, user2);
            instruction.SpriteFont = ScreenManager.Game.Content.Load<SpriteFont>("SpriteFont1");
            instruction.MyTexture = ScreenManager.Game.Content.Load<Texture2D>(@"Textures/screen");

        }

        public override void Update(GameTime gameTime, bool covered)
        {
            user2.setSkeleton();
            instruction.Button.Update(gameTime);
            
            if (instruction.Button.IsClicked())
            {
                ExitScreen();
                ScreenManager.AddScreen(new AdjustPosition(user2, 150, 350, 10, 50,2));
            }
            base.Update(gameTime, false);
        }

        public override void Draw(GameTime gameTime)
        {

            instruction.Draw(gameTime);
        }
        public override void Remove()
        {
            base.Remove();

        }


    }
}