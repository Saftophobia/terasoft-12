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
        float[] depth;
        float[] angle;
        Boolean[] accepted;
        String[] command;

        float minDepth;
        float maxDepth;
        float minAngle;
        float maxAngle;

        String title;
        String rule1;
        String rule2;

        Button button;

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
            rule1 = "Stand at a distance of " + (minDepth + maxDepth) / 2 + " meters from the kinect sensor.";
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
                rule2 = "Turn to your left at an angle " + (-1 * avgAngle) + "degrees fwithrom the kinect sensor.";
            }
            int sw = ScreenManager.GraphicsDevice.Viewport.Width;
            int sh = ScreenManager.GraphicsDevice.Viewport.Height;
            button = new OKButton(ContentManager, new Vector2(sw -  100, sh - 100), sw, sh);
        }

        public override void UnloadContent()
        {

        }
        public float getDepth(int ID)
        {
            return users[ID].USER.Joints[JointType.HipCenter].Position.Z;
        }

        public float getAngle(int ID)
        {
            Vector2 rightHip = new Vector2(users[ID].USER.Joints[JointType.HipRight].Position.X, users[ID].USER.Joints[JointType.HipRight].Position.Z);
            Vector2 leftHip = new Vector2(users[ID].USER.Joints[JointType.HipLeft].Position.X, users[ID].USER.Joints[JointType.HipLeft].Position.Z);
            Vector2 point = new Vector2(rightHip.X - leftHip.X, rightHip.Y - leftHip.Y);
            double angle = Math.Atan(point.Y / point.X);
            angle *= (180 / Math.PI);
            return (float)angle;
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
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }



        public override void Remove()
        {
            base.Remove();
        }
    }
}
