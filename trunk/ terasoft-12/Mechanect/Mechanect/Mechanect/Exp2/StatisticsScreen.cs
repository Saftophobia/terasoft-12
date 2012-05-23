using System.Windows;
using ButtonsAndSliders;
using Mechanect.Common;
using Mechanect.Exp2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private bool solutionVisible;

        private Vector2 predatorPosition;
        private Rect preyPosition;
        private Rect aquariumPosition;

        private Button mainMenu;
        private Button retry;
        private Button solution;
        private Button newGame;

        private Viewport ViewPort
        {
            get { return ScreenManager.GraphicsDevice.Viewport; }
        }

        /// <summary>
        /// Constructor to initialize the instance variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="predatorPosition"></param>
        /// <param name="preyPosition"></param>
        /// <param name="aquariumPosition"></param>
        /// <param name="userVelocity"></param>
        /// <param name="userAngle"></param>
        /// <param name="user"></param>
        public StatisticsScreen(Vector2 predatorPosition, Rect preyPosition, Rect aquariumPosition, float userVelocity,
            float userAngle, User2 user)
        {

            this.userSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, userVelocity, userAngle);
            this.user = user;
            this.predatorPosition = predatorPosition;
            this.preyPosition = preyPosition;
            this.aquariumPosition = aquariumPosition;
            correctAnswer = true;
        }
        /// <summary>
        /// Constructor to initialize the instance variables
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="predatorPosition"></param>
        /// <param name="preyPosition"></param>
        /// <param name="aquariumPosition"></param>
        /// <param name="userVelocity"></param>
        /// <param name="userAngle"></param>
        /// <param name="optimalVelocity"></param>
        /// <param name="optimalAngle"></param>
        /// <param name="user"></param>
        public StatisticsScreen(Vector2 predatorPosition, Rect preyPosition, Rect aquariumPosition, float userVelocity,
            float userAngle, float optimalVelocity, float optimalAngle, User2 user)
        {
            this.userSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, userVelocity, userAngle);
            this.optimalSimulation = new Simulation(predatorPosition, preyPosition, aquariumPosition, optimalVelocity, optimalAngle);
            this.user = user;
            this.predatorPosition = predatorPosition;
            this.preyPosition = preyPosition;
            this.aquariumPosition = aquariumPosition;
            correctAnswer = false;
        }
        /// <summary>
        /// Initialize the positions of the buttons and simulation graphs
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// </summary>
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

            rightSimulationPosition = new Rectangle(7*ViewPort.Width / 12, ViewPort.Height / 10, ViewPort.Width / 3,
                4 * ViewPort.Height / 10); ///
            centerSimulationPosition = new Rectangle(ViewPort.Width / 3, ViewPort.Height / 10, ViewPort.Width / 3,
                4 * ViewPort.Height / 10);
            leftSimulationPosition = new Rectangle(ViewPort.Width / 12, ViewPort.Height / 10, ViewPort.Width / 3,
                4 * ViewPort.Height / 10); ///
            currentUserSimulationPosition = centerSimulationPosition; ///
        }
        /// <summary>
        /// Load the simulation and the buttons
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// </summary>
        public override void LoadContent()
        {
            userSimulation.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice);
            if (!correctAnswer)
                optimalSimulation.LoadContent(ScreenManager.Game.Content, ScreenManager.GraphicsDevice);

            mainMenu = Mechanect.Exp3.Tools3.MainMenuButton(ScreenManager.Game.Content, mainMenuButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);

            retry = Mechanect.Exp3.Tools3.RetryButton(ScreenManager.Game.Content, retryButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);

            newGame = Mechanect.Exp3.Tools3.NewGameButton(ScreenManager.Game.Content, newGameButtonPosition,
                ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);
            

            if (!correctAnswer)
                solution = Mechanect.Exp3.Tools3.SolutionButton(ScreenManager.Game.Content, seeResultsButtonPosition,
                    ScreenManager.GraphicsDevice.Viewport.Width,
                ScreenManager.GraphicsDevice.Viewport.Width, user);
        }

        /// <summary>
        /// Draw the Simulation and Buttons
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            mainMenu.Draw(ScreenManager.SpriteBatch,0.5f);
            retry.Draw(ScreenManager.SpriteBatch, 0.5f);
            userSimulation.Draw(currentUserSimulationPosition, ScreenManager.SpriteBatch);
            mainMenu.DrawHand(ScreenManager.SpriteBatch);
            if (correctAnswer || solutionVisible)
                newGame.Draw(ScreenManager.SpriteBatch, 0.5f);
            else
                solution.Draw(ScreenManager.SpriteBatch, 0.5f);
            if (currentUserSimulationPosition.X <= leftSimulationPosition.X)
                optimalSimulation.Draw(rightSimulationPosition, ScreenManager.SpriteBatch);

            ScreenManager.SpriteBatch.End();

        }
        /// <summary>
        /// Update the objects drawns on the screen
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed Raafat</para>
        /// </remarks>
        /// <param name="gametime"></param>
        public override void Update(GameTime gametime)
        {
            
            mainMenu.Update(gametime);
            retry.Update(gametime);
            userSimulation.Update(gametime);
            if (mainMenu.IsClicked())
            {
                this.Remove();
                ScreenManager.AddScreen(new AllExperiments(user));
            }
            if (retry.IsClicked())
            {
                this.Remove();
                ScreenManager.AddScreen(new Experiment2(user, predatorPosition, preyPosition, aquariumPosition));
            }

            if (correctAnswer || solutionVisible)
            {
                newGame.Update(gametime);
                if (newGame.IsClicked())
                {

                    this.Remove();
                    ScreenManager.AddScreen(new Experiment2(user));
                }
                if (!correctAnswer)
                {
                    if (currentUserSimulationPosition.X > leftSimulationPosition.X)
                    {
                        currentUserSimulationPosition.Offset(-2, 0);
                        if (currentUserSimulationPosition.X < leftSimulationPosition.X)
                            currentUserSimulationPosition = leftSimulationPosition;
                    }
                    else
                    {
                        optimalSimulation.Update(gametime);
                    }
                }
            }
            else
            {
                solution.Update(gametime);
                if (solution.IsClicked())
                {
                    solutionVisible = true;
                }
            }
        }

    }


}
