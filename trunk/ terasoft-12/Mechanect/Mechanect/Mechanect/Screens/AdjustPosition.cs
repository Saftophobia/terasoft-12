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
        User[] users;
        Color[] userColors;
        int[] depth;
        float[] angle;
        Boolean[] accepted;
        String[] command;

        int gameID;

        int minDepth;
        int maxDepth;
        float minAngle;
        float maxAngle;

        String title;
        String rule1;
        String rule2;

        Button button;

        Texture2D depthBar;
        int depthBarWidth;
        int depthBarHeight;
        Color accept;
        Color reject;

        Texture2D angleBar;
        int angleBarHeight = 200;
        int angleBarWidth = 400;
        int curveWidth = 30;

        Texture2D arrow;
        Vector2 arrowOrigin;

        SpriteFont font;


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
            users = new User[1];
            userColors = new Color[1];
            command = new String[1];
            depth = new int[1];
            angle = new float[1];
            accepted = new Boolean[1];
            this.users[0] = user;
            this.userColors[0] = Color.Blue;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
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
            users = new User[2];
            userColors = new Color[2];
            this.users[0] = user1;
            this.users[1] = user2;
            this.userColors[0] = Color.Blue;
            this.userColors[1] = Color.Brown;
            command = new String[2];
            depth = new int[2];
            angle = new float[2];
            accepted = new Boolean[2];
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
            this.gameID = gameID;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            title = "Adjust Position";
            rule1 = "Stand at a distance of " + ((float)(minDepth + maxDepth) / 200) + " meters from the kinect sensor.";
            float avgAngle = (minAngle + maxAngle) / 2;
            if (avgAngle == 0)
            {
                rule2 = "Stand facing the kinect sensor";
            }
            else if (avgAngle > 0)
            {
                rule2 = "Turn to your right at an angle " + avgAngle + "degrees with the kinect sensor.";
            }
            else
            {
                rule2 = "Turn to your left at an angle " + (-1 * avgAngle) + "degrees with the kinect sensor.";
            }
            accept = Color.GreenYellow;
            reject = Color.OrangeRed;
            depthBarHeight = 200;
            depthBarWidth = 30;
            depthBar = new Texture2D(ScreenManager.GraphicsDevice, depthBarWidth, depthBarHeight);
            angleBarHeight = 200;
            angleBarWidth = 400;
            angleBar = semiCircle();
            //to be updated
            arrow = ContentManager.Load<Texture2D>("ball");
            font = ContentManager.Load<SpriteFont>("Ariel");
            int screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            int screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;


            button = Tools3.OKButton(ContentManager, new Vector2(800, 450), screenWidth, screenHeight, users[0]);
        }


        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <summary>
        /// <returns>returns the distance of users[ID] from the kinect sensor</returns>
        /// </summary>
        /// <param name="ID"> the index of the User in the users array</param>
        public int getDepth(int ID)
        {
            if (ID < users.Length)
                return (int) (100 * users[ID].USER.Joints[JointType.HipCenter].Position.Z);
            else return 0;
        }


        /// <summary>
        /// measures the orientation of the user with respect to the kinect sensor
        /// <example>a player standing facing the kinect sensor will have zero angle, </example>
        /// <example>a player turned to his right with respect to the kinect sensor will a positive angle, </example>
        /// <example>a player turned to his left with respect to the kinect sensor will a negative angle. </example>
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="ID"> the index of the User in the users array</param>
        /// <returns>returns the angle users[ID] makes with the kinect sensor. </returns>
        public float getAngle(int ID)
        {
            if (ID < users.Length)
            {
                Vector2 rightHip = new Vector2(users[ID].USER.Joints[JointType.HipRight].Position.X, users[ID].USER.Joints[JointType.HipRight].Position.Z);
                Vector2 leftHip = new Vector2(users[ID].USER.Joints[JointType.HipLeft].Position.X, users[ID].USER.Joints[JointType.HipLeft].Position.Z);
                Vector2 point = new Vector2(rightHip.X - leftHip.X, rightHip.Y - leftHip.Y);
                double angle = Math.Atan(point.Y / point.X);
                angle *= (180 / Math.PI);
                return (float)angle;
            }
            else return 0;
        }


        /// <summary>
        /// gets the suitable color that fits in the gradient in the semicircle
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="startAngle"> the start angle of the gradient</param>
        /// <param name="endAngle"> the end angle of the gradient</param>
        /// <param name="currentAngle"> the pixel's angle</param>
        /// <param name="left"> the color at the start (left side) of the gradient</param>
        /// <param name="right"> the color at the end (right side) of the gradient</param>
        /// <returns>returns the color corresponding to the gradient respect to pixel's position within the angle ranges</returns>
        public Color curveColor(int startAngle, int endAngle, int currentAngle, Color left, Color right)
        {
            int R = (right.R * (currentAngle - startAngle) + left.R * (endAngle - currentAngle)) / (endAngle - startAngle);
            int G = (right.G * (currentAngle - startAngle) + left.G * (endAngle - currentAngle)) / (endAngle - startAngle);
            int B = (right.B * (currentAngle - startAngle) + left.B * (endAngle - currentAngle)) / (endAngle - startAngle);
            return new Color(R, G, B);
        }

        /// <summary>
        /// creates the texture2D representing the angle bar
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        ///<returns>returns the semicircle with gradient indicating the accepted ranges for user's angle</returns>
        public Texture2D semiCircle()
        {
            float avgAngle = (minAngle + maxAngle) / 2;
            Texture2D grad = new Texture2D(ScreenManager.GraphicsDevice, angleBarWidth, angleBarHeight);
            Color[] data = new Color[angleBarHeight * angleBarWidth];
            int x = 0;
            int y = 0;
            double r = 0;
            double theta;
            for (int i = 0; i < data.Length; i++)
            {
                x = (int)(i % angleBarWidth - angleBarWidth / 2);
                y = angleBarHeight - i / angleBarWidth;
                r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                if (r <= angleBarWidth / 2 && r >= angleBarWidth / 2 - curveWidth)
                {
                    if (x == 0) theta = 0;
                    else
                    {
                        theta = Math.Atan((double)y / x) * 180 / Math.PI;
                        if (theta > 0) theta = 90 - theta;
                        else theta = -90 - theta;
                    }
                    if (theta <= minAngle || theta >= maxAngle)
                        data[i] = reject;
                    else if (theta >= (minAngle + avgAngle) / 2 && theta <= (maxAngle + avgAngle) / 2)
                        data[i] = accept;
                    else if (theta < avgAngle)
                        data[i] = curveColor((int)minAngle, (int)(minAngle + avgAngle) / 2, (int)theta, reject, accept);
                    else if (theta > avgAngle)
                        data[i] = curveColor((int)(maxAngle + avgAngle) / 2, (int)maxAngle, (int)theta, accept, reject);
                }
            }
            grad.SetData(data);
            return grad;
        }


        /// <summary>
        /// The method gets the suitable color to fit in the gradient.
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="start"> the start position of the gradient</param>
        /// <param name="end"> the end position of the gradient</param>
        /// <param name="index"> the pixel's position</param>
        /// <param name="top"> the color at the top of the gradient</param>
        /// <param name="bot"> the color at the bottom of the gradient</param>
        /// <returns>returns a color according to the location with respect to the start and end points of the gradient. </returns>
        public Color color(int start, int end, int index, Color top, Color bot)
        {
            int R = (bot.R * (index - start) + top.R * (end - index)) / (end - start);
            int G = (bot.G * (index - start) + top.G * (end - index)) / (end - start);
            int B = (bot.B * (index - start) + top.B * (end - index)) / (end - start);
            return new Color(R, G, B);
        }


        /// <summary>
        /// updates the gradient in the Depth bar to represent the players
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        public void updateDepthBar()
        {
            Color[] data = new Color[depthBarHeight];
            int avgDepth = (minDepth + maxDepth) / 2;
            for (int i = 0; i < depthBarHeight; i++)
            {
                if (2 * i + 50 <= minDepth || 2 * i + 50 >= maxDepth)
                    data[i] = reject;
                else if (2 * i + 50 > (avgDepth + minDepth) / 2 && 2 * i + 50 < (avgDepth + maxDepth) / 2)
                    data[i] = accept;
                else if (2 * i + 50 < avgDepth)
                    data[i] = color(minDepth, (avgDepth + minDepth) / 2, 2 * i + 50, reject, accept);
                else if (2 * i + 50 > avgDepth)
                    data[i] = color((avgDepth + maxDepth) / 2, maxDepth, 2 * i + 50, accept, reject);
                for (int j = 0; j < users.Length; j++)
                    if (2 * i + 50 <= depth[j] + 5 && 2 * i + 50 >= depth[j] - 5)
                        data[i] = userColors[j];
            }
            Color[] finalData = new Color[depthBarHeight * depthBarWidth];
            for (int j = 0; j < finalData.Length; j++)
            {
                finalData[j] = data[j / depthBarWidth];
            }
            depthBar.SetData(finalData);
        }


        /// <summary>
        /// Updates the User's depth and angle, and updates the command the user shall see
        /// </summary>
        ///<remarks>
        ///<para>
        ///Author: Mohamed AbdelAzim
        ///</para>
        ///</remarks>
        /// <param name="ID"> The ID of the user in users[]</param>
        public void UpdateUser(int ID)
        {
            if (users[ID].USER == null || users[ID].USER.Position.Z == 0)
            {
                command[ID] = "No player detected";
                depth[ID] = 0;
                angle[ID] = 0;
                accepted[ID] = false;
                return;
            }
            depth[ID] = getDepth(ID);
            angle[ID] = getAngle(ID);

            accepted[ID] = (depth[ID] <= maxDepth && depth[ID] >= minDepth && angle[ID] <= maxAngle && angle[ID] >= minAngle);

            if (accepted[ID])
            {
                command[ID] = "You are ready to start the game";
            }
            else if (depth[ID] < minDepth)
            {
                command[ID] = "Move backwards away from the kinect sensor";
            }
            else if (depth[ID] > maxDepth)
            {
                command[ID] = "Move forward towards the kinect sensor";
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
                UpdateUser(i);
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
            updateDepthBar();
        }

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
            button.Draw(ScreenManager.SpriteBatch);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(font, title, new Vector2(500, 20), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, rule1, new Vector2(100, 120), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, rule2, new Vector2(100, 220), Color.OrangeRed);
            for (int i = 0; i < users.Length; i++)
            {
                ScreenManager.SpriteBatch.DrawString(font, "Player " + i +" : " + command[i], new Vector2(100, 320 + 100 * i), Color.OrangeRed);
            }
            ScreenManager.SpriteBatch.Draw(depthBar, new Vector2(100, 450), Color.White);
            ScreenManager.SpriteBatch.Draw(angleBar, new Vector2(300, 450), Color.White);
            for (int i = 0; i < users.Length; i++)
            {
                ScreenManager.SpriteBatch.Draw(arrow, new Rectangle(angleBarWidth / 2 + 300, angleBarHeight + 450, 200, 20), null, userColors[i], (float)((angle[i] - 90) * Math.PI / 180), arrowOrigin, SpriteEffects.None, 0f);
            }
            ScreenManager.SpriteBatch.End();
        }
    }
}
