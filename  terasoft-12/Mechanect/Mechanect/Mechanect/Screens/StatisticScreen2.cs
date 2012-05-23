using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mechanect.Common;
using ButtonsAndSliders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Mechanect.Exp3;
using System.Windows;

namespace Mechanect.Exp2
{
    class StatisticsScreen : GameScreen
    {
        User2 user;
        private Simulation userSimulation;
        private Simulation optimalSimulation;
        Vector2 mainMenuButtonPosition;
        Vector2 retryButtonPosition;
        Vector2 newGameButtonPosition;
        Vector2 seeResultsButtonPosition;
        private bool correctAnswer;
        private Button mainMenu;
        private Button retry;
        private Button solution;
        private Button newGame;
        private Texture2D hand;



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
            mainMenuButtonPosition = new Vector2((float)0.1665 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height);

            retryButtonPosition = new Vector2((float)0.4995 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height);

            newGameButtonPosition = new Vector2((float)0.8327 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height);

            seeResultsButtonPosition = new Vector2((float)0.8327 * ScreenManager.GraphicsDevice.Viewport.Width,
               (float)0.8325 * ScreenManager.GraphicsDevice.Viewport.Height);
        }
        //Still waiting for Hegazy to do the buttons for me.
        public override void LoadContent()
        {
            userSimulation.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice.Viewport, ScreenManager.GraphicsDevice);
            if (!correctAnswer)
                optimalSimulation.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice.Viewport, ScreenManager.GraphicsDevice);

            mainMenu = Tools3.MainMenuButton(ScreenManager.Game.Content, mainMenuButtonPosition, ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);

            retry = Tools3.MainMenuButton(ScreenManager.Game.Content, mainMenuButtonPosition, ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);

            newGame = Tools3.NewGameButton(ScreenManager.Game.Content, mainMenuButtonPosition, ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);

            if (!correctAnswer)
                solution = Tools3.MainMenuButton(ScreenManager.Game.Content, mainMenuButtonPosition, ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            userSimulation.Draw(new Rectangle(5, 5, 250, 100), ScreenManager.Game.Content, ScreenManager.SpriteBatch,
                ScreenManager.GraphicsDevice.Viewport);
            mainMenu.Draw(ScreenManager.SpriteBatch);
            mainMenu.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();

        }

        public override void Update(GameTime gametime)
        {
            userSimulation.Update(gametime);
        }

    }


}
