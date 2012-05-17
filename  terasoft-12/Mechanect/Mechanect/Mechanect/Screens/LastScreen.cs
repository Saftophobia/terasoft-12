using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Common;
using Microsoft.Xna.Framework;
using Mechanect.Classes;
using Mechanect.Experiment2;
using ButtonsAndSliders;
using Mechanect.Exp3;


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
            mainMenu = Tools3.MainMenuButton(this.ScreenManager.Game.Content,
            new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Width-200 , this.ScreenManager.GraphicsDevice.Viewport.Height-250),
            this.ScreenManager.GraphicsDevice.Viewport.Width,
            this.ScreenManager.GraphicsDevice.Viewport.Height,user);

            newGame = Tools3.NewGameButton(this.ScreenManager.Game.Content,
              new Vector2(this.ScreenManager.GraphicsDevice.Viewport.Width - 500, this.ScreenManager.GraphicsDevice.Viewport.Height - 250),
              this.ScreenManager.GraphicsDevice.Viewport.Width,
              this.ScreenManager.GraphicsDevice.Viewport.Height, user);
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
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool covered)
        {
            if (mainMenu.IsClicked())
            {

                ExitScreen();
                ScreenManager.AddScreen(new AllExperiments());
                this.Remove();
            }
            if (newGame.IsClicked())
            {
                ExitScreen();
                switch(experemintNumber)
                {
                    case 1: ScreenManager.AddScreen(new Experiment1(new User1(), new User1(), new MKinect())); break;
                    case 2: ScreenManager.AddScreen(new InstructionsScreen2(new User2())); break;
                    case 3: ScreenManager.AddScreen(new InstructionsScreen3(new User3())); break;
                    default: break;
                }
                this.Remove();
            }
            mainMenu.Update(gameTime);
            newGame.Update(gameTime);
            base.Update(gameTime, covered);
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
            mainMenu.Draw(this.ScreenManager.SpriteBatch);
            newGame.Draw(this.ScreenManager.SpriteBatch);
        }
    }
}
