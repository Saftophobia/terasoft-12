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
using Mechanect.Experiment2;
using ButtonsAndSliders;
using Mechanect.Exp3;

namespace Mechanect.Screens
{
    class AdjustPosition : Mechanect.Common.GameScreen
    {
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

        DepthBar[] depthBar;
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
            angle = new float[1];
            accepted = new Boolean[1];
            depthBar = new DepthBar[1];
            this.users[0] = user;
            this.userColors[0] = Color.Blue;

            accept = Color.GreenYellow;
            reject = Color.OrangeRed;
            depthBar[0] = new DepthBar(user, minDepth, maxDepth, 400, 200, accept, reject, Color.Blue, ScreenManager.GraphicsDevice);
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
            angle = new float[2];
            accepted = new Boolean[2];
            depthBar = new DepthBar[2];
            accept = Color.GreenYellow;
            reject = Color.OrangeRed;
            depthBar[0] = new DepthBar(user1, minDepth, maxDepth, 400, 200, accept, reject, Color.Blue, ScreenManager.GraphicsDevice);
            depthBar[1] = new DepthBar(user2, minDepth, maxDepth, 400, 200, accept, reject, Color.Beige, ScreenManager.GraphicsDevice);
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
            angleBarHeight = 200;
            angleBarWidth = 400;
            angleBar = SemiCircle();
            //to be updated
            arrow = ContentManager.Load<Texture2D>("ball");
            font = ContentManager.Load<SpriteFont>("Ariel");
            int screenHeight = ScreenManager.GraphicsDevice.Viewport.Height;
            int screenWidth = ScreenManager.GraphicsDevice.Viewport.Width;


            button = Tools3.OKButton(ContentManager, new Vector2(800, 450), screenWidth, screenHeight, users[0]);
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
        public float GetAngle(int ID)
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
        public Texture2D SemiCircle()
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
                angle[ID] = 0;
                accepted[ID] = false;
                return;
            }
            angle[ID] = GetAngle(ID);

            accepted[ID] = (depthBar[ID].Accepted() && angle[ID] <= maxAngle && angle[ID] >= minAngle);

            if (accepted[ID])
            {
                command[ID] = "You are ready to start the game";
            }
            else if (!depthBar[ID].Accepted())
            {
                command[ID] = depthBar[ID].Command();
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
            depthBar[0].Update();
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
            ScreenManager.SpriteBatch.DrawString(font, depthBar[0].Rule, new Vector2(100, 120), Color.OrangeRed);
            ScreenManager.SpriteBatch.DrawString(font, rule2, new Vector2(100, 220), Color.OrangeRed);
            for (int i = 0; i < users.Length; i++)
            {
                ScreenManager.SpriteBatch.DrawString(font, "Player " + i +" : " + command[i], new Vector2(100, 320 + 100 * i), Color.OrangeRed);
            }
            depthBar[0].Draw(ScreenManager.SpriteBatch, new Vector2(100, 450));
            ScreenManager.SpriteBatch.Draw(angleBar, new Vector2(300, 450), Color.White);
            for (int i = 0; i < users.Length; i++)
            {
                ScreenManager.SpriteBatch.Draw(arrow, new Rectangle(angleBarWidth / 2 + 300, angleBarHeight + 450, 200, 20), null, userColors[i], (float)((angle[i] - 90) * Math.PI / 180), arrowOrigin, SpriteEffects.None, 0f);
            }
            ScreenManager.SpriteBatch.End();
        }

    }
}
