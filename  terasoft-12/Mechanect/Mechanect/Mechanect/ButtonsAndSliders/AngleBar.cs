using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;
using Microsoft.Kinect;

namespace Mechanect.ButtonsAndSliders {
    class AngleBar
    {

        #region Variables And Fields

        User[] users;
        int minAngle;
        int maxAngle;
        int curveWidth;
        int radius;
        Color accept;
        Color reject;
        Color[] playerColors;

        Texture2D semiCircle;
        Texture2D playerIndicator;

        public bool Accepted
        {
            get
            {
                for (int i = 0; i < users.Length; i++)
                    if (Angle(i) >= maxAngle || Angle(i) <= minAngle)
                        return false;
                return true;
            }
        }

        public string Rule
        {
            get
            {
                float avgAngle = (minAngle + maxAngle) / 2;
                if (avgAngle == 0)
                {
                    return "Stand facing the kinect sensor";
                }
                else if (avgAngle > 0)
                {
                    return "Turn to your right at an angle " + avgAngle + "degrees with the kinect sensor.";
                }
                else
                {
                    return "Turn to your left at an angle " + (-1 * avgAngle) + "degrees with the kinect sensor.";
                }
            }
        }

        #endregion

        #region LoadTextures

        public AngleBar(User[] users, int minAngle, int maxAngle, int radius, Color accept, Color reject, Color[] playerColors, GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            this.users = users;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
            this.radius = radius;
            this.curveWidth = radius / 4;
            this.accept = accept;
            this.reject = reject;
            this.playerColors = playerColors;
            semiCircle = CreateSemiCircle(graphicsDevice);
            playerIndicator = contentManager.Load<Texture2D>("ball");
        }

        #region Create Semicircle
        

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
        public Texture2D CreateSemiCircle(GraphicsDevice graphicsDevice)
        {
            int width = 2 * radius;
            float avgAngle = (minAngle + maxAngle) / 2;
            Texture2D grad = new Texture2D(graphicsDevice, width, radius);
            Color[] data = new Color[radius * width];
            int x = 0;
            int y = 0;
            double r = 0;
            double theta;
            for (int i = 0; i < data.Length; i++)
            {
                x = (int)(i % width - width / 2);
                y = radius - i / width;
                r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                if (r <= width / 2 && r >= width / 2 - curveWidth)
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


        #endregion

        #endregion

        #region Functions

        public string Command(int ID)
        {
            if (Angle(ID) == 0)
                return "No player detected";
            if (Angle(ID) < minAngle)
                return "Turn a little to your right";
            if (Angle(ID) > maxAngle)
                return "Turn a little to your left";
            return "OK!";
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
        public int Angle(int ID)
        {
            try
            {
                Vector2 rightHip = new Vector2(users[ID].USER.Joints[JointType.HipRight].Position.X, users[ID].USER.Joints[JointType.HipRight].Position.Z);
                Vector2 leftHip = new Vector2(users[ID].USER.Joints[JointType.HipLeft].Position.X, users[ID].USER.Joints[JointType.HipLeft].Position.Z);
                Vector2 point = rightHip - leftHip;
                double angle = Math.Atan(point.Y / point.X);
                angle *= (180 / Math.PI);
                return (int)angle;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(semiCircle, position, Color.White);
            for (int i = 0; i < users.Length; i++)
                spriteBatch.Draw(playerIndicator,
                               new Rectangle(radius + (int)position.X, radius + (int)position.Y, radius, radius / 8),
                               null, playerColors[i],
                               (float)((Angle(i) - 90) * Math.PI / 180),
                               new Vector2(0, playerIndicator.Height / 2),
                               SpriteEffects.None,
                               0f);

        }

        #endregion
    }
}
