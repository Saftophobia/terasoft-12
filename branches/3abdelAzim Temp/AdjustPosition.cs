using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Mechanect.Classes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using Mechanect.Common;

namespace Mechanect.Screens
{
    class AdjustPosition : Mechanect.Common.GameScreen
    {

        #region Variables

        int gameID;
        Button button;
        SpriteFont font;

        String[] commands;
        DepthBar[] depthBars;
        AngleBar[] angleBars;

        /*
        User[] users;
        Color[] userColors;
        float[] angle;
        Boolean[] accepted;
        String[] command;

        int gameID;
        
        float minAngle;
        float maxAngle;

        String title;
        String rule2;

        Button button;
        
        DepthBar[] depthBars;
        Color accept;
        Color reject;

        Texture2D angleBar;
        int angleBarHeight = 200;
        int angleBarWidth = 400;
        int curveWidth = 30;

        Texture2D arrow;
        Vector2 arrowOrigin;

        SpriteFont font;
        */

        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <summary>
        /// gets the content manager from the screen manager
        /// and returns it.
        /// </summary>
        ContentManager ContentManager
        {
            get
            {
                return ScreenManager.Game.Content;
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
            depthBars = new DepthBar[1];
            depthBars[0] = new DepthBar(user, minDepth, maxDepth, 400, 200, Color.GreenYellow, Color.OrangeRed, Color.Blue, ScreenManager.GraphicsDevice);
            angleBars = new AngleBar[1];
            angleBars[0] = new AngleBar(user, minDepth, maxDepth, 400, 200, Color.GreenYellow, Color.OrangeRed, Color.Blue, ScreenManager.GraphicsDevice);
            commands = new String[1];
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
        public AdjustPosition(User user1,User user2 ,int minDepth, int maxDepth, float minAngle, float maxAngle, int gameID)
        {
            depthBars = new DepthBar[2];
            depthBars[0] = new DepthBar(user1, minDepth, maxDepth, 400, 200, Color.GreenYellow, Color.OrangeRed, Color.Blue, ScreenManager.GraphicsDevice);
            depthBars[1] = new DepthBar(user2, minDepth, maxDepth, 400, 200, Color.GreenYellow, Color.OrangeRed, Color.Blue, ScreenManager.GraphicsDevice);
            angleBars = new AngleBar[2];
            angleBars[0] = new AngleBar(user1, minDepth, maxDepth, 400, 200, Color.GreenYellow, Color.OrangeRed, Color.Blue, ScreenManager.GraphicsDevice);
            angleBars[1] = new AngleBar(user2, minDepth, maxDepth, 400, 200, Color.GreenYellow, Color.OrangeRed, Color.Blue, ScreenManager.GraphicsDevice);
            commands = new String[2];
            this.gameID = gameID;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        
        public override void LoadContent()
        {
            font = ContentManager.Load<SpriteFont>("Ariel");
            button = Tools3.OKButton(ContentManager, new Vector2(800, 450), ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height, new User());
        }
        #endregion

        #region update

        public override void Update(GameTime gameTime)
        {
            foreach (DepthBar depthBar in depthBars)
            {
                depthBar.Update();
            }
            foreach (AngleBar angleBar in angleBars)
            {
                angleBar.Update();
            }
            for (int i = 0; i < commands.Length; i++)
            {
                commands[i] = Command(i);
            }
        }

        private string Command(int id)
        {
            if (!depthBars[id].Accepted())
            {
                return depthBars[id].Command();
            }
            if (!angleBars[id].Accepted())
            {
                return angleBars[id].Command();
            }
            return "Ready";
        }

        /* Old Update
        /// <summary>
        /// Updates the User's depth and angle, and updates the command the user shall see
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="ID"> The ID of the user in users[]</param>
        public void updateUser(int ID)
        {
            if (users[ID].USER == null || users[ID].USER.Position.Z == 0)
            {
                command[ID] = "No player detected";
                angle[ID] = 0;
                accepted[ID] = false;
                return;
            }
            angle[ID] = GetAngle(ID);

            accepted[ID] = (depthBars[ID].Accepted() && angle[ID] <= maxAngle && angle[ID] >= minAngle);

            if (accepted[ID])
            {
                command[ID] = "You are ready to start the game";
            }
            else if (!depthBars[ID].Accepted())
            {
                command[ID] = depthBars[ID].Command();
            }
            else if (angle[ID] > maxAngle)
            {
                command[ID] = "Turn a little to your left";
            }
            else if (angle[ID] < minAngle)
            {
                command[ID] = "Turn a little to your right";
            }
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
        public override void Update(GameTime gameTime, Boolean covered)
        {
            for (int i = 0; i < users.Length; i++)
                updateUser(i);
            button.Update(gameTime);
            if (button.IsClicked())
            {
                Boolean allAccepted = true;
                for (int i = 0; allAccepted && i < accepted.Length; i++)
                    if (!accepted[i])
                        allAccepted = false;
                if (allAccepted)
                {
                    switch (gameID)
                    {
                        case 2:
                            ScreenManager.AddScreen(new Experiment2((User2)users[0]));
                            ExitScreen();
                            break;
                    }
                }
            }
            depthBars[0].Update();
        }
         * */
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

            button.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.DrawString(font, "Adjust Position", new Vector2(500, 20), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, depthBars[0].Rule, new Vector2(100, 120), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, angleBars[0].Rule, new Vector2(100, 220), Color.OrangeRed);
            depthBars[0].Draw(ScreenManager.SpriteBatch,Vector2.Zero);
            angleBars[0].Draw(ScreenManager.SpriteBatch, Vector2.Zero);
            ScreenManager.SpriteBatch.End();
            /*
             * 
            for (int i = 0; i < commands.Length; i++)
            {
                ScreenManager.SpriteBatch.DrawString(font, "Player " + i+1 +" : " + commands[i], new Vector2(100, 320 + 100 * i), Color.OrangeRed);
            }
            depthBars[0].Draw(ScreenManager.SpriteBatch, new Vector2(100, 450));
            ScreenManager.SpriteBatch.Draw(angleBar, new Vector2(300, 450), Color.White);
            for (int i = 0; i < users.Length; i++)
            {
                ScreenManager.SpriteBatch.Draw(arrow, new Rectangle(angleBarWidth / 2 + 300, angleBarHeight + 450, 200, 20), null, userColors[i], (float)((angle[i] - 90) * Math.PI / 180), arrowOrigin, SpriteEffects.None, 0f);
            }
             * */
        }
        #endregion

    }
}
