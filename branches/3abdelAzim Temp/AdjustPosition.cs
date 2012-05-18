using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using Mechanect.Common;
using Mechanect.Experiment2;
using ButtonsAndSliders;
using Mechanect.Exp3;
using UI.Components;
using Mechanect.ButtonsAndSliders;

namespace Mechanect.Screens
{
    class AdjustPosition : Mechanect.Common.GameScreen
    {

        #region Variables

        User[] users;
        int gameID;
        Button button;
        SpriteFont font;

        DepthBar depthBar;
        AngleBar angleBar;

        bool Accepted 
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
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="user"> the object User which tracks the skeleton of the player</param>
        /// <param name="minDepth"> an integer representing the minimum distance in centimeters the player should stand at.</param>
        /// <param name="maxDepth"> an integer representing the maximum distance in centimeters the player should stand at.</param>
        /// <param name="minAngle"> a float representing the minimum angle the player should make with the kinect sensor.</param>
        /// <param name="maxAngle"> a float representing the minimum angle the player should make with the kinect sensor.</param>
        public AdjustPosition(User user, int minDepth, int maxDepth, float minAngle, float maxAngle, int gameID)
        {
            this.users = new User[1];
            this.users[0] = user;
            Color[] playerColors = new Color[1];
            playerColors[0] = Color.Blue;
            depthBar = new DepthBar(users, minDepth, maxDepth, 50, 200, Color.GreenYellow, Color.OrangeRed, playerColors, ScreenManager.GraphicsDevice);
            angleBar = new AngleBar(users, minDepth, maxDepth, 200, Color.GreenYellow, Color.OrangeRed, playerColors, ScreenManager.GraphicsDevice, ScreenManager.Game.Content);
            this.gameID = gameID;
        }


        /// <summary>
        /// Creates object "AdjustPosition" that makes sure that users are standing correctly. works for 2 users.
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="user1"> the object User which tracks the skeleton of the first player</param>
        /// <param name="user2"> the object User which tracks the skeleton of the second player</param>
        /// <param name="minDepth"> an integer representing the minimum distance in centimeters players should stand at.</param>
        /// <param name="maxDepth"> an integer representing the maximum distance in centimeters players should stand at.</param>
        /// <param name="minAngle"> a float representing the minimum angle players should make with the kinect sensor.</param>
        /// <param name="maxAngle"> a float representing the minimum angle players should make with the kinect sensor.</param>
        public AdjustPosition(User user1, User user2, int minDepth, int maxDepth, float minAngle, float maxAngle, int gameID)
        {
            this.users = new User[2];
            users[0] = user1;
            users[1] = user2;
            Color[] playerColors = new Color[2];
            playerColors[0] = Color.Blue;
            playerColors[1] = Color.Orange;
            depthBar = new DepthBar(users, minDepth, maxDepth, 50, 200, Color.GreenYellow, Color.OrangeRed, playerColors, ScreenManager.GraphicsDevice);
            angleBar = new AngleBar(users, minDepth, maxDepth, 200, Color.GreenYellow, Color.OrangeRed, playerColors, ScreenManager.GraphicsDevice, ScreenManager.Game.Content);
            this.gameID = gameID;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

        public override void LoadContent()
        {
            font = ScreenManager.Game.Content.Load<SpriteFont>("Ariel");
            //button = Tools3.OKButton(ScreenManager.Game.Content, new Vector2(800, 450), ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height, new User());
        }
        #endregion

        #region update

        

        private string Command(int id)
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
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="covered">determines whether the screen is covered by another screen or not.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < users.Length; i++)
            {
                users[i].setSkeleton(i);
            }
            /*
            if (Accepted)
            {
                button.Update(gameTime);
                if (button.IsClicked())
                {
                    switch (gameID)
                    {
                            case 2:
                                ScreenManager.AddScreen(new Experiment2((User2)user));
                                ExitScreen();
                                break;
                    }
                
                 }
            }
             */
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
            //if (Accepted)
            //    button.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.DrawString(font, "Adjust Position", new Vector2(500, 20), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, depthBar.Rule, new Vector2(100, 120), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, angleBar.Rule, new Vector2(100, 220), Color.OrangeRed);
            depthBar.Draw(ScreenManager.SpriteBatch, Vector2.Zero);
            angleBar.Draw(ScreenManager.SpriteBatch, Vector2.Zero);
            ScreenManager.SpriteBatch.End();
        }
        #endregion

    }
}
