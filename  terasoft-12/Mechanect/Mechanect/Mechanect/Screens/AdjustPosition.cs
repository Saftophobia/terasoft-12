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
        int[] depth;
        float[] angle;
        Boolean[] accepted;
        String[] command;

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

        SpriteFont font;

        ContentManager ContentManager
        {
            get
            {
                return ScreenManager.Game.Content;
            }
        }

        public AdjustPosition(User user, int minDepth, int maxDepth, float minAngle, float maxAngle)
        {
            users = new User[1];
            this.users[0] = user;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
        }

        public AdjustPosition(User user1,User user2 ,int minDepth, int maxDepth, float minAngle, float maxAngle)
        {
            users = new User[2];
            this.users[0] = user1;
            this.users[1] = user2;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
        }

        public override void LoadContent()
        {
            title = "Adjust Position";
            rule1 = "Stand at a distance of " + (minDepth + maxDepth) / 200 + " meters from the kinect sensor.";
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
            int sw = ScreenManager.GraphicsDevice.Viewport.Width;
            int sh = ScreenManager.GraphicsDevice.Viewport.Height;
            button = new OKButton(ContentManager, new Vector2(sw -  100, sh - 100), sw, sh);
            depthBarHeight = 200;
            depthBarWidth = 30;
            depthBar = new Texture2D(ScreenManager.GraphicsDevice, depthBarWidth, depthBarHeight);
            accept = Color.GreenYellow;
            reject = Color.OrangeRed;
            font = ContentManager.Load<SpriteFont>("Ariel");
        }

        public override void UnloadContent()
        {

        }
        public int getDepth(int ID)
        {
            if (ID < users.Length)
                return (int) (100 * users[ID].USER.Joints[JointType.HipCenter].Position.Z);
            else return 0;
        }

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

        public Color color(int start, int end, int index, Color top, Color bot)
        {
            return new Color((bot.R * (index - start) + top.R * (end - index)) / (end - start), (bot.G * (index - start) + top.G * (end - index)) / (end - start), (bot.B * (index - start) + top.B * (end - index)) / (end - start));
        }

        public void updateDepthBar()
        {
            Color[] data = new Color[depthBarHeight];
            int avgDepth = (minDepth + maxDepth) / 2;
            for (int i = 0; i < depthBarHeight; i++)
            {
                if (2 * i + 50 <= getDepth(0) + 5 && 2 * i + 50 >= getDepth(0) - 5)
                    data[i] = Color.Blue;
                else if (2 * i + 50 <= getDepth(0) + 5 && 2 * i + 50 >= getDepth(0) - 5)
                    data[i] = Color.DarkCyan;
                else if (2 * i + 50 <= minDepth || 2 * i + 50 >= maxDepth)
                    data[i] = reject;
                else if (2 * i + 50 > (avgDepth + minDepth) / 2 && 2 * i + 50 < (avgDepth + maxDepth) / 2)
                    data[i] = accept;
                else if (2 * i + 50 < avgDepth)
                    data[i] = color(minDepth, (avgDepth + minDepth) / 2, 2 * i + 50, reject, accept);
                else if (2 * i + 50 > avgDepth)
                    data[i] = color((avgDepth + maxDepth) / 2, maxDepth, 2 * i + 50, accept, reject);
            }
            Color[] finalData = new Color[depthBarHeight * depthBarWidth];
            for (int j = 0; j < finalData.Length; j++)
            {
                finalData[j] = data[j / depthBarWidth];
            }
            depthBar.SetData(finalData);
        }

        public void UpdateUser(int ID)
        {
            if (users[ID].USER == null)
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

        public override void Update(GameTime gameTime, Boolean covered)
        {
            for (int i = 0; i < users.Length; i++)
                UpdateUser(i);
            button.update(gameTime);
            updateDepthBar();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(font, title, new Vector2(500, 20), Color.OrangeRed);
            ScreenManager.SpriteBatch.End();
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(font, rule1, new Vector2(100, 120), Color.OrangeRed);
            ScreenManager.SpriteBatch.End();
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(font, rule2, new Vector2(100, 220), Color.OrangeRed);
            ScreenManager.SpriteBatch.End();
            for (int i = 0; i < users.Length; i++)
            {
                ScreenManager.SpriteBatch.Begin();
                ScreenManager.SpriteBatch.DrawString(font, command[i], new Vector2(100, 320 + 100 * i), Color.OrangeRed);
                ScreenManager.SpriteBatch.End();
            }
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(depthBar, new Vector2(100, 520), Color.White);
            ScreenManager.SpriteBatch.End();
        }



        public override void Remove()
        {
            base.Remove();
        }
    }
}
