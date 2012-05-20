using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using ButtonsAndSliders;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;

namespace ButtonsAndSliders
{

    public class Slider
    {
        private Vector2 positionBar;
        private Vector2 positionPointer;
        private Texture2D texture, onPic, offPic, barPic;
        private int screenW, ScreenH;
        private int value;

        private User user;
        private Timer1 timer;

        private ContentManager Content;

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// The constructor used to initialize the slider
        /// </summary>
        /// <param name="p">the position of the slider, where the center is the top left corner </param>
        /// <param name="sw">screen width</param>
        /// <param name="sh">screen height</param>
        /// <param name="c">the content manager to draw the textures</param>
        public Slider(Vector2 p, int sw, int sh, ContentManager c, User u)
        {
            screenW = sw;
            ScreenH = sh;
            value = 1;

            user = u;
            Content = c;
            timer = new Timer1();

            texture = Content.Load<Texture2D>("Textures/Slider/off");
            onPic = Content.Load<Texture2D>("Textures/Slider/on");
            offPic = Content.Load<Texture2D>("Textures/Slider/off");
            barPic = Content.Load<Texture2D>("Textures/Slider/bar");

            positionBar = p;
            positionPointer.X = p.X;
            positionPointer.Y = p.Y - ((texture.Height - barPic.Height) / 2);
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
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(barPic, positionBar, Color.White);
            spriteBatch.Draw(texture, positionPointer, Color.White);
            spriteBatch.End();
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// checking if the hand of the user has been hovered on the pointer and starts
        /// the timer and activating the pointer to be moved.
        /// </summary>
        public void Update(GameTime gameTime) 
        {
            if (CheckCollision())
            {
                if (!timer.IsRunning())
                    timer.Start(gameTime);
                else
                {
                    if (timer.GetDuration(gameTime) >= (2000))
                    {
                        On();
                        Move();
                    }
                    
                }
            }
            else
            {
                timer.Stop();
                Off();
            }
            
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// tracking the movement of the hand to move the pointer to the right or the left. Incrementing
        /// or decrementing the value according to the movement.
        /// </summary>
        private void Move()
        {
            Skeleton skeleton = user.Kinect.requestSkeleton();
            if (skeleton != null)
            {
                Point hand = user.Kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, ScreenH);
                
                if ((hand.X - positionPointer.X) >= 30 && !(value == 5))
                {
                    positionPointer.X += (barPic.Width / 5);
                    value++;
                }
                if ((hand.X - positionPointer.X) <= -30 && !(value == 1))
                {
                    positionPointer.X -= (barPic.Width / 5);
                    value--;
                }
            }
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// changing the pointer to the activated pointer picture
        /// </summary>
        private void On()
        {
            texture = onPic;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// changing the pointer to the disactivated pointer picture
        /// </summary>
        private void Off()
        {
            texture = offPic;
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
        private bool CheckCollision()
        {

            Skeleton skeleton = user.USER;
            if (skeleton != null)
            {
                Point hand = user.Kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, ScreenH);
                Rectangle r1 = new Rectangle(hand.X, hand.Y, 50, 50);
                Rectangle r2 = new Rectangle((int)positionPointer.X, (int)positionPointer.Y, offPic.Width, offPic.Height);

                return r1.Intersects(r2);
            }
            return false;
        }


        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// used to get the value that the user selected
        /// </summary>
        /// <returns>the value that the user selected</returns>
        public int GetValue()
        {
            return value;
        }

    }
}
