using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using Mechanect.Common;
using Common.Classes;

namespace Mechanect
{
    public class Button
    {

        protected Vector2 position;
        protected GifAnimation.GifAnimation texture, animation, stopped;
        protected int screenW, ScreenH;

        public Vector2 Position { get { return position; } set { position = value; } }
        public bool isActive;
        MKinect kinect;
        Texture2D hand;
        Vector2 handPosition;

        Timer1 timer;

        bool status;

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// The constructor used ti initialize the button
        /// </summary>
        /// <param name="t">the gif texture of the button not moving</param>
        /// <param name="tt">the gif texture of the button not moving</param>
        /// <param name="p">the position of the slider</param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="h">the picture of the hand</param>
        public Button(GifAnimation.GifAnimation t, GifAnimation.GifAnimation tt, Vector2 p, int sw, int sh, Texture2D h)
        {
            position = p;
            texture = t;
            stopped = t;
            animation = tt;
            screenW = sw;
            ScreenH = sh;

            hand = h;
            kinect = new MKinect();
            timer = new Timer1();
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// drawing the bar and the pointer
        /// </summary>
        /// <param name="spriteBatch">used to draw the texture</param>
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture.GetTexture(), position, Color.White);
            spriteBatch.Draw(hand, handPosition, Color.White);
            spriteBatch.End();
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// checking if the hand of the user has been hovered on the button and starts
        /// the timer and activating the pointer to be moved.
        /// </summary>
        /// <param name="gameTime">takes the object gametime which tracks the time of the game</param>
        public void update(GameTime gameTime)
        {
            moveHand();
            checkColission();
            texture.Update(gameTime.ElapsedGameTime.Ticks);

            if (isActive && checkColission())
            {
                if (!timer.isRunning())
                    timer.start();
                else
                {
                    if (timer.getDuration() >= (1 * 1000))
                    {
                        animate();
                        status = true;
                    }

                }
            }
            else
            {
                timer.stop();
                stop();
            }


        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// used to track the user's hand
        /// </summary>
        void moveHand()
        {
            Skeleton skeleton = kinect.requestSkeleton();
            if (skeleton != null)
            {
                handPosition.X = kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, ScreenH).X;
                handPosition.Y = kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, ScreenH).Y;
            }
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// changing the button to the animated picture
        /// </summary>
        public void animate()
        {
            texture = animation;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// changing the button to the stopped picture
        /// </summary>
        public void stop()
        {
            texture = stopped;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// checks if the hand of the user is over the pointer or not
        /// </summary>
        /// <returns>returns true if the user is hovering the button</returns>
        public bool checkColission()
        {
            Skeleton skeleton = kinect.requestSkeleton();
            if (skeleton != null)
            {
                Point hand = kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, ScreenH);
                Rectangle r1 = new Rectangle(hand.X, hand.Y, 50, 50);
                Rectangle r2 = new Rectangle((int)position.X, (int)position.Y, texture.GetTexture().Width, texture.GetTexture().Height);

                if (r1.Intersects(r2))
                    return true;
                else
                    return false;
            }
            return false;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// used to check if the button has been clicked or not
        /// </summary>
        /// <returns>returns true if the user clicked the button</returns>
        public bool isClicked()
        {
            return status && isActive;
        }
    }
}
