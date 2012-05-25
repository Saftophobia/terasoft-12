using ButtonsAndSliders;
using Mechanect.ButtonsAndSliders;
using Mechanect.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Screens
{
    class AdjustPosition : Mechanect.Common.GameScreen
    {

        #region Variables

        private User[] user;
        private Color[] userColor;
        private int gameID;
        private Button button;
        private SpriteFont font;

        private DepthBar depthBar;
        private AngleBar angleBar;

        private Texture2D background;
        /// <summary>
        /// Getter for the Users' State
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <returns>bool, Returns true if all users are standing correctly</returns>
        public bool Accepted
        {
            get
            {
                return depthBar.Accepted && angleBar.Accepted;
            }
        }

        #endregion

        #region Constructors and Load

        /// <summary>
        /// creates object "AdjustPosition" that makes sure that the user is standing correctly. Works for one user.
        /// </summary>
        /// <remarks>
        /// <para>
        /// AUTHOR: Mohamed AbdelAzim
        /// </para>
        /// </remarks>
        /// <param name="user">the object User which tracks the skeleton of the player</param>
        /// <param name="minDepth">an integer representing the minimum distance in centimeters the player should stand at.</param>
        /// <param name="maxDepth">an integer representing the maximum distance in centimeters the player should stand at.</param>
        /// <param name="minAngle">an integer representing the minimum angle the player should make with the kinect sensor.</param>
        /// <param name="maxAngle">an integer representing the minimum angle the player should make with the kinect sensor.</param>
        /// <param name="gameID">an integer representing the id of the game to determine which game will be opened after this screen.</param>
        public AdjustPosition(User user, int minDepth, int maxDepth, int minAngle, int maxAngle, int gameID)
        {
            isTwoPlayers = false;
            this.user = new User[1];
            this.user[0] = user;
            userColor = new Color[1];
            userColor[0] = Color.Blue;
            depthBar = new DepthBar(this.user, minDepth, maxDepth, 20, 300, Color.Violet, Color.DarkMagenta, userColor);
            angleBar = new AngleBar(this.user, minAngle, maxAngle, 120, Color.Violet, Color.DarkMagenta, userColor);
            this.gameID = gameID;
        }
        

        /// <summary>
        /// Creates object "AdjustPosition" that makes sure that users are standing correctly. works for 2 users.
        /// </summary>
        /// <remarks>
        /// <para>
        /// AUTHOR: Mohamed AbdelAzim
        /// </para>
        /// </remarks>
        /// <param name="user1">the object User which tracks the skeleton of the first player</param>
        /// <param name="user2">the object User which tracks the skeleton of the second player</param>
        /// <param name="minDepth">an integer representing the minimum distance in centimeters players should stand at.</param>
        /// <param name="maxDepth">an integer representing the maximum distance in centimeters players should stand at.</param>
        /// <param name="minAngle">an integer representing the minimum angle players should make with the kinect sensor.</param>
        /// <param name="maxAngle">an integer representing the minimum angle players should make with the kinect sensor.</param>
        /// <param name="gameID">an integer representing the id of the game to determine which game will be opened after this screen.</param>
        public AdjustPosition(User user1, User user2, int minDepth, int maxDepth, int minAngle, int maxAngle, int gameID)
        {
            isTwoPlayers = true;
            this.user = new User[2];
            user[0] = user1;
            user[1] = user2;
            userColor = new Color[2];
            userColor[0] = Color.Blue;
            userColor[1] = Color.Red;
            depthBar = new DepthBar(user, minDepth, maxDepth, 20, 300, Color.Violet, Color.DarkMagenta, userColor);
            angleBar = new AngleBar(user, minAngle, maxAngle, 120, Color.Violet, Color.DarkMagenta, userColor);
            this.gameID = gameID;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            background = ScreenManager.Game.Content.Load<Texture2D>("Textures/Screens/AdjustPosition");
            font = ScreenManager.Game.Content.Load<SpriteFont>("Ariel");
            button = Exp3.Tools3.OKButton(ScreenManager.Game.Content, new Vector2((int)(viewport.Width * 0.38), (int)(viewport.Height * 0.68)), ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height, new User());
            depthBar.LoadContent(ScreenManager.GraphicsDevice);
            angleBar.LoadContent(ScreenManager.GraphicsDevice, ScreenManager.Game.Content);
            base.LoadContent();
        }
        #endregion

        #region update

        /// <summary>
        /// A getter to the command that should be visible to the user
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="id">an int representing the ID of the user</param>
        /// <returns>string, returns the command that should be applied by the user to be standing correctly</returns>
        public string Command(int id)
        {
            if (!depthBar.UserAccepted(id))
            {
                return depthBar.Command(id);
            }
            if (!angleBar.UserAccepted(id))
            {
                return angleBar.Command(id);
            }
            return "Ready";
        }

        /// <summary>
        /// Runs every frame gathering players' data and updating screen parameters.
        /// </summary>
        /// <remarks>
        /// <para>
        /// AUTHOR: Mohamed AbdelAzim
        /// </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < user.Length; i++)
            {
                user[i].setSkeleton(i);
            }
            if (Accepted)
            {
                button.Update(gameTime);
                if (button.IsClicked())
                {
                    switch (gameID)
                    {
                        case 1:
                           //ScreenManager.AddScreen(new Experiment1(new Mechanect.Exp1.User1(), new Mechanect.Exp1.User1(), new MKinect())); //commented becoz as a User1 i dont need it anymore
                            Remove();
                            break;
                        case 2:
                            user[0] = new Mechanect.Exp2.User2();
                            ScreenManager.AddScreen(new Mechanect.Exp2.Experiment2((Mechanect.Exp2.User2)user[0]));
                            Remove();
                            break;
                        case 3:
                            user[0] = new Mechanect.Exp3.User3();
                            ScreenManager.AddScreen(new Mechanect.Exp3.Experiment3((Mechanect.Exp3.User3)user[0]));
                            Remove();
                            break;
                    }
                 }
            }
            base.Update(gameTime);
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the screen should draw itself. displays depth bar and user's rules and commands that allow him to stand correctly
        /// </summary>
        /// <remarks>
        /// <para>
        /// AUTHOR: Mohamed AbdelAzim
        /// </para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(background, ScreenManager.GraphicsDevice.Viewport.Bounds,Color.White);
            if (Accepted)
            {
                button.Draw(ScreenManager.SpriteBatch, viewport.Width / 1024f);
                button.DrawHand(ScreenManager.SpriteBatch);
            }
            UI.UILib.Write(depthBar.Rule, new Rectangle((int)(0.2 * viewport.Width), 
                (int)(0.25 * viewport.Height), (int)(0.5 * viewport.Width),
                (int)(0.25 * viewport.Height)), ScreenManager.SpriteBatch, font, Color.DarkViolet);
            UI.UILib.Write(angleBar.Rule, new Rectangle((int)(0.2 * viewport.Width), 
                (int)(0.25 * viewport.Height + 60), (int)(0.5 * viewport.Width),
                (int)(0.25 * viewport.Height)), ScreenManager.SpriteBatch, font, Color.DarkViolet);
            for (int i = 0; i < user.Length; i++)
            {
                UI.UILib.Write(Command(i), new Rectangle((int)(0.25 * viewport.Width), 
                    (int)(0.25 * viewport.Height + 110 + 35 * i), (int)(0.5 * viewport.Width), 
                    (int)(0.25 * viewport.Height)), ScreenManager.SpriteBatch, font, userColor[i]);
            }
            depthBar.Draw(ScreenManager.SpriteBatch, new Vector2((int)(0.1 * viewport.Width), (int)(0.25 * viewport.Height)));
            angleBar.Draw(ScreenManager.SpriteBatch, new Vector2((int)(0.6 * viewport.Width), (int)(0.4 * viewport.Height)));
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion

    }
}
