using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using UI.Cameras;
using UI.Animation;
using UI.Components;
using Mechanect.Classes;
using Physics;
using Mechanect.Screens;
using ButtonsAndSliders;


namespace Mechanect.Exp3
{
    public class Experiment3 : Mechanect.Common.GameScreen
    {
        private Ball ball;
        public Ball BallPorperty
        {
            get
            {
                return ball;
            }
        }
        private Bar bar;
        private Environment3 environment;
        public Environment3 EnvironmentProperty
        {
            get
            {
                return environment;
            }
        }
        private User3 user;

        private TargetCamera targetCamera;
        private BallAnimation animation;
        private Simulation simulation;

        private bool pauseScreenShowed;
        private bool firstAnimation;
        private bool hasWhistled;

        private float arriveVelocity;
        private Vector3 shootVelocity;

        private Button mainMenu;
        private Button newGame;

        private SoundEffect whistle, crowd;

        /// <summary>
        /// Creates a new Experiment3 game screen.
        /// </summary>
        /// <param name="user">User3 instance.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public Experiment3(User3 user)
        {
            ball = new Ball(2.5f);
            ball.GenerateBallMass(0.004f, 0.006f);

            environment = new Environment3(user);

            arriveVelocity = 10;
            firstAnimation = true;
            user.ShootingPosition = new Vector3(0, 3, 45);
            this.user = user;
            GenerateSolvable();
        }

        /// <summary>
        /// Loads the experiment's environment and buttons.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void LoadContent()
        {
            targetCamera = new TargetCamera(new Vector3(0, 30, 95), new Vector3(0,20,0), 
                ScreenManager.GraphicsDevice);

            environment.InitializeUI(ScreenManager.Game.Content, ScreenManager.GraphicsDevice);
            environment.LoadContent();
           
            ball.LoadContent(ScreenManager.Game.Content.Load<Model>(@"Models/ball"));
            ball.GenerateInitialPosition(environment.TerrainWidth, environment.TerrainWidth);
           
            environment.ball = ball;

            Vector3 initialVelocity = LinearMotion.CalculateInitialVelocity(user.ShootingPosition - ball.Position, 
                arriveVelocity, Environment3.Friction);

            animation = new BallAnimation(ball, environment, initialVelocity);

            bar = new Bar(new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 10, 
                ScreenManager.GraphicsDevice.Viewport.Height - 225), ScreenManager.SpriteBatch, 
                new Vector2(ball.Position.X, ball.Position.Z), new Vector2(ball.Position.X, ball.Position.Z), 
                new Vector2(user.ShootingPosition.X, user.ShootingPosition.Z), ScreenManager.Game.Content);

            //whistle = ScreenManager.Game.Content.Load<SoundEffect>("whistle");

            //crowd = ScreenManager.Game.Content.Load<SoundEffect>("crowd_cheer");
            //crowd.Play();

            InitializeButtons();

            base.LoadContent();
        }

        /// <summary>
        /// Updates the experiment's screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            environment.PlayerModel.Update();
            environment.PlayerAnimation.Update();
            UpdateFirstAnimation(gameTime);
            UpdateSecondAnimation();
            if (simulation != null)
            {
                UpdateButtons(gameTime);
                simulation.Update(gameTime);
            }
            else
                animation.Update(gameTime.ElapsedGameTime);
            
            targetCamera.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the first animation.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void UpdateFirstAnimation(GameTime gameTime)
        {
            if (!firstAnimation)
                return;
            
            float distance = animation.Displacement.Length();
            float totalDistance = (user.ShootingPosition - animation.StartPosition).Length();
            if (distance / totalDistance > 0.5 && !pauseScreenShowed)
            {
                pauseScreenShowed = true;
                FreezeScreen();
                ScreenManager.AddScreen(new PauseScreen(user, arriveVelocity, ball.Mass, user.AssumedLegMass, 
                    environment.HoleProperty.Position));
            }
            bar.Update(new Vector2(ball.Position.X, ball.Position.Z));
            if (ball.HasBallEnteredShootRegion())
            {
                environment.arriveVelocity = arriveVelocity;
                /*if (!hasWhistled)
                {
                    whistle.Play();
                    hasWhistled = true;
                }*/
                user.UpdateMeasuringVelocityAndAngle(gameTime);
                Vector3 shootVelocity = user.Velocity;
                if (user.HasShot && shootVelocity.Length() != 0)
                {
                    firstAnimation = false;
                    this.shootVelocity = Functions.GetVelocityAfterCollision(shootVelocity, ball.Mass, user.AssumedLegMass, arriveVelocity, Constants3.velocityScale);
                    animation = new BallAnimation(ball, environment, this.shootVelocity);
                }
            }

            if (animation.Finished)
                UpdateButtons(gameTime);
        }

        /// <summary>
        /// Updates the second animation.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public void UpdateSecondAnimation()
        {
            if (firstAnimation)
                return;
            
            if (!ball.InsideTerrain(environment.TerrainWidth, environment.TerrainHeight))
                animation.Stop();
            
            if (animation.Finished && simulation == null)
                simulation = new Simulation(ball, environment, user.ShootingPosition, shootVelocity,
                    ScreenManager.Game.Content, ScreenManager.GraphicsDevice, ScreenManager.SpriteBatch);
        }

        /// <summary>
        /// Draws the experiment's screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        public override void Draw(GameTime gameTime)
        {
            Camera camera = targetCamera;
            if (simulation != null)
                camera = simulation.Camera;
            
            environment.Draw(camera, gameTime);
            ball.Draw(camera);
            if (firstAnimation)
            {
                float distance = animation.Displacement.Length();
                float totalDistance = (user.ShootingPosition - animation.StartPosition).Length();
                if (distance / totalDistance > 1)
                {
                    DrawStatus();
                    DrawButtons();
                }
                else
                    bar.Draw();
               
            }
            if (simulation != null)
            {
                simulation.Draw();
                DrawButtons();
                DrawStatus();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Displays winner or loser according to the winning conditions
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        private void DrawStatus()
        {
            int screenWidth = this.ScreenManager.GraphicsDevice.Viewport.Width;
            int screenHeight = this.ScreenManager.GraphicsDevice.Viewport.Height;
            Tools3.DisplayIsWin(ScreenManager.SpriteBatch, ScreenManager.Game.Content,
                new Vector2(35, screenHeight - 110), 0.5f, animation.willFall);
        }

        /// <summary>
        /// Initializes the experiment's buttons.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        private void InitializeButtons()
        {
            int screenWidth = this.ScreenManager.GraphicsDevice.Viewport.Width;
            int screenHeight = this.ScreenManager.GraphicsDevice.Viewport.Height;

            mainMenu = Tools3.MainMenuButton(ScreenManager.Game.Content, new Vector2(screenWidth - 105,
                screenHeight - 105), screenWidth, screenHeight, user);

            newGame = Tools3.NewGameButton(ScreenManager.Game.Content, new Vector2(screenWidth / 2 - 70,
                screenHeight - 150), screenWidth, screenHeight, user);
        }

        /// <summary>
        /// Updates the experiment's buttons.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        private void UpdateButtons(GameTime gameTime)
        {
            mainMenu.Update(gameTime);
            newGame.Update(gameTime);
            if (mainMenu.IsClicked())
            {
                Remove();
                ScreenManager.AddScreen(new AllExperiments((User3)Game1.user3));
            }
            if (newGame.IsClicked())
            {
                Remove();
                ScreenManager.AddScreen(new Experiment3((User3)Game1.user3));
            }
        }

        /// <summary>
        /// Draws the experiment's buttons.
        /// </summary>
        /// <remarks>
        /// AUTHOR : Bishoy Bassem.
        /// </remarks>
        private void DrawButtons()
        {
            ScreenManager.SpriteBatch.Begin();
            newGame.Draw(ScreenManager.SpriteBatch, 0.6f);
            mainMenu.Draw(ScreenManager.SpriteBatch, 0.4f);
            mainMenu.DrawHand(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.End();
        }

        /// <summary>
        /// This method verifies whether the experiment is solvable.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Ahmed Badr. </para>
        ///</remarks>
        /// <returns>Retuns an int that represents the type of the problem with the experiment.</returns>
        private int IsSolvable()
        {

            if (ball.Radius <= 0)
                return Constants3.negativeBRradius;
            if (ball.Mass <= 0)
                return Constants3.negativeBMass;
            if (environment.HoleProperty.Radius <= 0)
                return Constants3.negativeHRadius;
            if (user.AssumedLegMass <= 0)
                return Constants3.negativeLMass;
            if (environment.HoleProperty.Position.Z - user.ShootingPosition.Z > 0)
                return Constants3.negativeHPosZ;
            if (Environment3.Friction > 0)
                return Constants3.negativeFriction;
            if (ball.Radius > environment.HoleProperty.Radius)
                return Constants3.negativeRDifference;
            
            Vector3 finalPos = Functions.GetFinalPosition(Functions.GetVelocityAfterCollision(
                new Vector3(0, 0, Constants3.maxVelocityZ), ball.Mass, user.AssumedLegMass, arriveVelocity,
                (float)Constants3.velocityScale), Environment3.Friction, ball.Position);
 
            if (Vector3.DistanceSquared(finalPos, user.ShootingPosition) < Vector3.DistanceSquared(environment.HoleProperty.Position, user.ShootingPosition))
                return Constants3.holeOutOfFarRange;

            finalPos = Vector3.Zero; Functions.GetFinalPosition(Functions.GetVelocityAfterCollision(
                new Vector3(0, 0, Constants3.minVelocityZ), ball.Mass, user.AssumedLegMass, arriveVelocity,
                (float)Constants3.velocityScale), Environment3.Friction, ball.Position);
 
            if (Vector3.DistanceSquared(finalPos, user.ShootingPosition) > Vector3.DistanceSquared(environment.HoleProperty.Position, user.ShootingPosition)) //length squared used for better performance than length
                return Constants3.holeOutOfNearRange;

            return Constants3.solvableExperiment;
        }

        public Vector3 GetVelocityAfterCollision(Vector3 initialVelocity) { return Vector3.Zero; }

        //TO sanad, make this method static?
        private Vector3 BallFinalPosition(Vector3 velocity) { return Vector3.Zero; }

        /// <summary>
        /// Generates a solvable experiment.
        /// </summary>
        /// <remarks>
        ///<para>AUTHOR: Ahmed Badr. </para>
        ///</remarks>
        public void GenerateSolvable()
        {
            Hole hole = environment.HoleProperty;
            if (hole.Position.Z >= Constants3.maxHolePosZ - hole.Radius)
                hole.Position = new Vector3(hole.Position.X, hole.Position.Y, Constants3.maxHolePosZ - hole.Radius);
            if (Math.Abs(hole.Position.X) > Constants3.maxHolePosX - hole.Radius)
                hole.Position = new Vector3(Constants3.maxHolePosX - hole.Radius, hole.Position.Y, hole.Position.Z);
            if (hole.Position.Z >= user.ShootingPosition.Z)
                hole.Position = new Vector3(hole.Position.X, hole.Position.Y, Constants3.maxHolePosZ - hole.Radius);

            var x = Constants3.solvableExperiment;
            do
            {
                x = IsSolvable();
                switch (x)
                {
                    case Constants3.holeOutOfNearRange: Environment3.Friction++; break;
                    case Constants3.holeOutOfFarRange:
                        if (Environment3.Friction < -1)
                            Environment3.Friction /= 2;
                        else if (Environment3.Wind < 0)
                            Environment3.Wind++;
                        else hole.Position = new Vector3(hole.Position.X / 2, hole.Position.Y, hole.Position.Z + 1); break;
                    case Constants3.negativeRDifference: int tmp = (int)ball.Radius; ball.Radius = (hole.Radius); hole.Radius = (tmp); break;
                    case Constants3.negativeLMass: user.AssumedLegMass *= -1; break;
                    case Constants3.negativeBMass: ball.Mass *= -1; break;
                    case Constants3.negativeBRradius: ball.Radius *= -1; break;
                    case Constants3.negativeHRadius: hole.Radius *= -1; break;
                    case Constants3.negativeFriction: Environment3.Friction *= -1; break;
                    case Constants3.negativeHPosZ: hole.Position = Vector3.Add(hole.Position, new Vector3(0, 0, 1)); break;
                }
            } while (x != Constants3.solvableExperiment);
        }

    
    }



}
