using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mechanect.Screens
{
    class AngleBar
    {
        private Classes.User user;
        private int minDepth;
        private int maxDepth;
        private int p;
        private int p_2;
        private Microsoft.Xna.Framework.Color color;
        private Microsoft.Xna.Framework.Color color_2;
        private Microsoft.Xna.Framework.Color color_3;
        private Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice;
        public StringBuilder Rule;

        public AngleBar(Classes.User user, int minDepth, int maxDepth, int p, int p_2, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color color_2, Microsoft.Xna.Framework.Color color_3, Microsoft.Xna.Framework.Graphics.GraphicsDevice graphicsDevice)
        {
            // TODO: Complete member initialization
            this.user = user;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
            this.p = p;
            this.p_2 = p_2;
            this.color = color;
            this.color_2 = color_2;
            this.color_3 = color_3;
            this.graphicsDevice = graphicsDevice;
        }
        internal void Update()
        {
            throw new NotImplementedException();
        }

        internal string Command()
        {
            throw new NotImplementedException();
        }

        internal bool Accepted()
        {
            throw new NotImplementedException();
        }

        /*
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
         */


        /*
        #region anglebar

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


        #endregion
*/

        internal void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 vector2)
        {
            throw new NotImplementedException();
        }
    }
}
