using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Common;
using ButtonsAndSliders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Windows;
//using Mechanect.Exp3;
using Mechanect.Exp2;

namespace Mechanect.Screens
{
    class StatisticsScreen : GameScreen
    {
        User2 user;
        private Simulation userSimulation;
        private Simulation optimalSimulation;

        private Rectangle leftSimulationPosition;
        private Rectangle rightSimulationPosition;
        private Rectangle centerSimulationPosition;
        private Rectangle currentUserSimulationPosition;

        Vector2 mainMenuButtonPosition;
        Vector2 retryButtonPosition;
        Vector2 newGameButtonPosition;
        Vector2 seeResultsButtonPosition;

        private bool correctAnswer;



        private Button mainMenu;
        private Button retry;
        private Button solution;
        private Button newGame;

        private Viewport ViewPort
        {
            get { return ScreenManager.GraphicsDevice.Viewport; }
        }


        public StatisticsScreen(Vector2 predatorPosition, Rect preyPosition, Rect aquariumPosition, float userVelocity,
            float userAngle, User2 user)
        {

            this.userSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, userVelocity, userAngle);
            this.user = user;
            correctAnswer = true;
        }

        public StatisticsScreen(Vector2 predatorPosition, Rect preyPosition, Rect aquariumPosition, float userVelocity,
            float userAngle, float optimalVelocity, float optimalAngle, User2 user)
        {
            this.userSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, userVelocity, userAngle);
            this.optimalSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, optimalVelocity, optimalAngle);
            this.user = user;
            correctAnswer = false;
        }

        public override void Initialize()
        {
            mainMenuButtonPosition = new Vector2((float)0.01*ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.8*ScreenManager.GraphicsDevice.Viewport.Height);

            retryButtonPosition = new Vector2((float)0.42 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.8 * ScreenManager.GraphicsDevice.Viewport.Height);

            newGameButtonPosition = new Vector2((float)0.84 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.8 * ScreenManager.GraphicsDevice.Viewport.Height);

            seeResultsButtonPosition = new Vector2((float)0.84 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.8 * ScreenManager.GraphicsDevice.Viewport.Height);

            rightSimulationPosition = new Rectangle(); ///
            centerSimulationPosition = new Rectangle(); ///
            leftSimulationPosition = new Rectangle(); ///
            currentUserSimulationPosition = new Rectangle(); ///
        }
        //Still waiting for Hegazy to do the buttons for me.
        public override void LoadContent()
        {
            userSimulation.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice);
            if (!correctAnswer)
                optimalSimulation.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice);

            mainMenu = Mechanect.Exp3.Tools3.MainMenuButton(ScreenManager.Game.Content, mainMenuButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);

            retry = Mechanect.Exp3.Tools3.MainMenuButton(ScreenManager.Game.Content, retryButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);

            newGame = Mechanect.Exp3.Tools3.NewGameButton(ScreenManager.Game.Content, newGameButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);
            

            if (!correctAnswer)
                solution = Mechanect.Exp3.Tools3.MainMenuButton(ScreenManager.Game.Content, seeResultsButtonPosition,
                    ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);
        }

        public override void Draw(GameTime gameTime)
        {
         
             
            ScreenManager.SpriteBatch.Begin();
            mainMenu.Draw(ScreenManager.SpriteBatch,0.5f);
            retry.Draw(ScreenManager.SpriteBatch, 0.5f);
            newGame.Draw(ScreenManager.SpriteBatch, 0.5f);
            

           // if (!correctAnswer)
             //   newGame.Draw(ScreenManager.SpriteBatch);
            //else

                newGame.DrawHand(ScreenManager.SpriteBatch);

            ScreenManager.SpriteBatch.End();

        }

        public override void Update(GameTime gametime)
        {
            mainMenu.Update(gametime);
            retry.Update(gametime);
            userSimulation.Update(gametime);
        }

    }


}
