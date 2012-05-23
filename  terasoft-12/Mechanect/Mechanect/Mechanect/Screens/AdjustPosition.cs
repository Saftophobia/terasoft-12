using ButtonsAndSliders;
using Mechanect.ButtonsAndSliders;
using Mechanect.Common;
using Mechanect.Exp2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mechanect.Screens
{
    class AdjustPosition : Mechanect.Common.GameScreen
    {

        #region Variables

        User[] user;
        int gameID;
        Button button;
        SpriteFont font;

        DepthBar depthBar;
        AngleBar angleBar;

        /// <summary>
        /// Getter for the Users' State
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed AbdelAzim</para>
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
        /// Author: Mohamed AbdelAzim
        /// </para>
        /// </remarks>
        /// <param name="user">the object User which tracks the skeleton of the player</param>
        /// <param name="minDepth">an integer representing the minimum distance in centimeters the player should stand at.</param>
        /// <param name="maxDepth">an integer representing the maximum distance in centimeters the player should stand at.</param>
        /// <param name="minAngle">an integer representing the minimum angle the player should make with the kinect sensor.</param>
        /// <param name="maxAngle">an integer representing the minimum angle the player should make with the kinect sensor.</param>
        public AdjustPosition(User user, int minDepth, int maxDepth, int minAngle, int maxAngle, int gameID)
        {
            this.user = new User[1];
            this.user[0] = user;
            Color[] playerColor = new Color[1];
            playerColor[0] = Color.Blue;
            depthBar = new DepthBar(this.user, minDepth, maxDepth, 50, 200, Color.GreenYellow, Color.OrangeRed, playerColor);
            angleBar = new AngleBar(this.user, minAngle, maxAngle, 200, Color.GreenYellow, Color.OrangeRed, playerColor);
            this.gameID = gameID;
        }


        /// <summary>
        /// Creates object "AdjustPosition" that makes sure that users are standing correctly. works for 2 users.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Author: Mohamed AbdelAzim
        /// </para>
        /// </remarks>
        /// <param name="user1">the object User which tracks the skeleton of the first player</param>
        /// <param name="user2">the object User which tracks the skeleton of the second player</param>
        /// <param name="minDepth">an integer representing the minimum distance in centimeters players should stand at.</param>
        /// <param name="maxDepth">an integer representing the maximum distance in centimeters players should stand at.</param>
        /// <param name="minAngle">an integer representing the minimum angle players should make with the kinect sensor.</param>
        /// <param name="maxAngle">an integer representing the minimum angle players should make with the kinect sensor.</param>
        public AdjustPosition(User user1, User user2, int minDepth, int maxDepth, int minAngle, int maxAngle, int gameID)
        {
            this.user = new User[2];
            user[0] = user1;
            user[1] = user2;
            Color[] playerColor = new Color[2];
            playerColor[0] = Color.Blue;
            playerColor[1] = Color.Orange;
            depthBar = new DepthBar(user, minDepth, maxDepth, 50, 200, Color.GreenYellow, Color.OrangeRed, playerColor);
            angleBar = new AngleBar(user, minAngle, maxAngle, 200, Color.GreenYellow, Color.OrangeRed, playerColor);
            this.gameID = gameID;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            font = ScreenManager.Game.Content.Load<SpriteFont>("Ariel");
            button = Exp3.Tools3.OKButton(ScreenManager.Game.Content, new Vector2(800, 450), ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height, new User());
            depthBar.LoadContent(ScreenManager.GraphicsDevice);
            angleBar.LoadContent(ScreenManager.GraphicsDevice, ScreenManager.Game.Content);
        }
        #endregion

        #region update

        /// <summary>
        /// A getter to the command that should be visible to the user
        /// </summary>
        /// <remarks>
        /// <para>Author: Mohamed AbdelAzim</para>
        /// </remarks>
        /// <param name="id">an int representing the ID of the user</param>
        /// <returns>string, returns the command that should be applied by the user to be standing correctly</returns>
        public string Command(int id)
        {
            if (!depthBar.Accepted)
            {
                return depthBar.Command(id);
            }
            if (!angleBar.Accepted)
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
        /// Author: Mohamed AbdelAzim
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
                            case 2:
                                ScreenManager.AddScreen(new Experiment2((User2)user[0]));
                                Remove();
                                break;
                    }
                
                 }
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the screen should draw itself. displays depth bar and user's rules and commands that allow him to stand correctly
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            ScreenManager.SpriteBatch.Begin();
            if (Accepted)
                button.Draw(ScreenManager.SpriteBatch, 1);
            ScreenManager.SpriteBatch.DrawString(font, "Adjust Position", new Vector2(500, 20), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, depthBar.Rule, new Vector2(100, 120), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, angleBar.Rule, new Vector2(100, 220), Color.OrangeRed);
            for (int i = 0; i < user.Length; i++)
            {
                ScreenManager.SpriteBatch.DrawString(font, Command(i), new Vector2(100, 320 + 100 * i), Color.OrangeRed);
            }
            depthBar.Draw(ScreenManager.SpriteBatch, new Vector2(100,520));
            angleBar.Draw(ScreenManager.SpriteBatch, new Vector2(200,520));
            ScreenManager.SpriteBatch.End();
        }
        #endregion

    }
}
