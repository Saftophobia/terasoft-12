﻿using System;
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


namespace Common.Classes
{

    class Slider
    {
        Vector2 positionBar;
        Vector2 positionPointer;
        Texture2D texture, onPic, offPic, barPic;
        int screenW, ScreenH;
        int value;

        Kinect kinect;
        Timer1 timer;

        ContentManager Content;

        /// <summary>
        /// The constructor used ti initialize the slider
        /// </summary>
        /// <param name="p">the position of the slider </param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="c">the content manager to draw the textures</param>

        public Slider(Vector2 p, int sw, int sh, ContentManager c)
        {
            screenW = sw;
            ScreenH = sh;
            value = 1;

            kinect = new Kinect();
            Content = c;
            timer = new Timer1();

            texture = Content.Load<Texture2D>("off");
            onPic = Content.Load<Texture2D>("on");
            offPic = Content.Load<Texture2D>("off");
            barPic = Content.Load<Texture2D>("bar");

            positionBar = p;
            positionPointer.X = p.X;
            positionPointer.Y = p.Y - ((texture.Height - barPic.Height) / 2);
        }


        /// <summary>
        /// drawing the bar and the pointer
        /// </summary>
        /// <param name="spriteBatch">used to draw the texture</param>
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(barPic, positionBar, Color.White);
            spriteBatch.Draw(texture, positionPointer, Color.White);
            spriteBatch.End();
        }

        /// <summary>
        /// checking if the hand of the user has been hovered on the pointer and starts
        /// the timer and activating the pointer to be moved.
        /// </summary>
        public void update() 
        {
            if (checkColission())
            {
                if (!timer.isRunning())
                    timer.start();
                else
                {
                    if (timer.getDuration() >= (1 * 1000))
                    {
                        on();
                        move();
                    }
                    
                }
            }
            else
            {
                timer.stop();
                off();
            }
            
        }

        /// <summary>
        /// tracking the movement of the hand to move the pointer to the right or the left. Incrementing
        /// or decrementing the value according to the movement.
        /// </summary>
        void move()
        {
            Skeleton skeleton = kinect.requestSkeleton();
            if (skeleton != null)
            {
                Point hand = kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, ScreenH);
                
                if ((hand.X - positionPointer.X) >= 40 && !(value == 5))
                {
                    positionPointer.X += (barPic.Width/5);
                    value++;
                }
                if ((hand.X - positionPointer.X) <= -40 && !(value == 1))
                {
                    positionPointer.X -= (barPic.Width / 5);
                    value--;
                }
            }
        }

        /// <summary>
        /// changing the pointer to the activated pointer picture
        /// </summary>
        void on()
        {
            texture = onPic;
        }

        /// <summary>
        /// changing the pointer to the disactivated pointer picture
        /// </summary>
        void off()
        {
            texture = offPic;
        }

        /// <summary>
        /// checks if the hand of the user is over the pointer or not
        /// </summary>
        /// <returns></returns>
        public bool checkColission()
        {

            Skeleton skeleton = kinect.requestSkeleton();
            if (skeleton != null)
            {
                Point hand = kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, ScreenH);
                Rectangle r1 = new Rectangle(hand.X, hand.Y, 50, 50);
                Rectangle r2 = new Rectangle((int)positionPointer.X, (int)positionPointer.Y, offPic.Width, offPic.Height);

                if (r1.Intersects(r2))
                    return true;
                else
                    return false;
            }
            return false;
        }


    }
}
