using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ButtonsAndSliders;
using Mechanect.Exp1;
using Mechanect.Exp2;
using Mechanect.Exp3;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Screens
{
    class LastScreen : GameScreen
    {
        private Button mainMenu;
        private Button newGame;
        private int experemintNumber;
        private User user;


        /// <summary>
        /// construcor to initioalize some variables
        /// </summary>
        /// <param name="user">instance of class User</param>
        /// <param name="experemintNumber">the number of the experement which should be called when
        /// newGame button is clicked</param>
        public LastScreen(User user, int experemintNumber)
        {
            this.user = user;
            this.experemintNumber = experemintNumber;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// Initializing the mainmenu and newgame buttons
        /// </summary>
        public override void LoadContent()
        {
            int screenWidth = this.ScreenManager.GraphicsDevice.Viewport.Width;
            int screenHeight = this.ScreenManager.GraphicsDevice.Viewport.Height;
            ContentManager contentManager = this.ScreenManager.Game.Content;

            mainMenu = Tools3.MainMenuButton(contentManager, new Vector2(screenWidth - 200, 
                screenHeight - 250), screenWidth, screenHeight, user);

            newGame = Tools3.NewGameButton(contentManager, new Vector2(screenWidth - 500, 
                screenHeight - 250), screenWidth, screenHeight, user);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// updating the buttons. It's requiered to make them run correctly
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="covered"></param>
        public override void Update(GameTime gameTime)
        {
            if (mainMenu.IsClicked())
            {
                ScreenManager.AddScreen(new AllExperiments(user));
                Remove();
            }
            if (newGame.IsClicked())
            {
                switch(experemintNumber)
                {
                    case 1: ScreenManager.AddScreen(new Experiment1(new User1(), new User1(), new MKinect())); break;
                    case 2: ScreenManager.AddScreen(new InstructionsScreen2(new User2())); break;
                    case 3: ScreenManager.AddScreen(new InstructionsScreen3(new User3())); break;
                    default: break;
                }
                Remove();
            }
            mainMenu.Update(gameTime);
            newGame.Update(gameTime);

            base.Update(gameTime);
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// drwing the buttons
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
            this.ScreenManager.SpriteBatch.Begin();
            newGame.Draw(this.ScreenManager.SpriteBatch);
            mainMenu.Draw(this.ScreenManager.SpriteBatch);
            mainMenu.DrawHand(this.ScreenManager.SpriteBatch);
            this.ScreenManager.SpriteBatch.End();
        }
    }
}
