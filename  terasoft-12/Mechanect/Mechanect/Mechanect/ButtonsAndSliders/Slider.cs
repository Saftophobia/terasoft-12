using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using ButtonsAndSliders;
using Mechanect.Common;
using Microsoft.Xna.Framework.Content;

namespace ButtonsAndSliders
{
    /// <summary>
    /// Slider used to select a cerain value according the motion of the user's hand.
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: AhmeD HegazY</para>
    /// </remarks>
    public class Slider
    {
        private Vector2 positionBar;
        private Vector2 positionPointer;
        private Texture2D texture, onPic, offPic, barPic;
        private int screenW, screenH;
        public int Value { get; private set;}

        private User user;
        private Timer1 timer;

        /// <summary>
        /// The constructor used to initialize the slider.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="position">The position of the slider, where the center is the top left corner.</param>
        /// <param name="screenW">The width of the screen.</param>
        /// <param name="screenH">The height of the screen.</param>
        /// <param name="user">The instance of the user.</param>
        public Slider(Vector2 position, int screenW, int screenH, User user)
        {
            this.screenW = screenW;
            this.screenH = screenH;
            Value = 1;

            this.user = user;
            timer = new Timer1();

            positionBar = position;
            positionPointer.X = position.X;
        }


        /// <summary>
        /// Loads the images of the slider and the pointer.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="content">The content manager to load the textures.</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Textures/Slider/off");
            onPic = content.Load<Texture2D>("Textures/Slider/on");
            offPic = content.Load<Texture2D>("Textures/Slider/off");
            barPic = content.Load<Texture2D>("Textures/Slider/bar");

            positionPointer.Y = positionBar.Y - ((texture.Height - barPic.Height) / 2);
        }

        ///<remarks>
        /// <summary>
        /// Draws the slider.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">The spritebatch used to draw the textures.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(barPic, positionBar, Color.White);
            spriteBatch.Draw(texture, positionPointer, Color.White);
        }


        /// <summary>
        /// Draws the slider.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">The spritebatch used to draw the textures.</param>
        /// <param name="scale">The ratio to scale the width and the height of the slider.</param>
        public void Draw(SpriteBatch spriteBatch, float scale)
        {
            Rectangle rectangleBar = new Rectangle((int)positionBar.X, (int)positionBar.Y,
                (int)(scale * barPic.Width), (int)(scale * barPic.Height));

            Rectangle rectanglePointer = new Rectangle((int)positionPointer.X, (int)positionPointer.Y,
                (int)(scale * texture.Width), (int)(scale * texture.Height));

            spriteBatch.Draw(barPic, rectangleBar, Color.White);
            spriteBatch.Draw(texture, rectanglePointer, Color.White);
        }


        /// <summary>
        /// Draws the slider.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">The spritebatch used to draw the textures.</param>
        /// <param name="scaleW">The ratio to scale the width of the slider.</param>
        /// <param name="scaleW">The ratio to scale the height of the slider.</param>
        public void Draw(SpriteBatch spriteBatch, float scaleW, float scaleH)
        {
            Rectangle rectangleBar = new Rectangle((int)positionBar.X, (int)positionBar.Y,
                (int)(scaleW * barPic.Width), (int)(scaleH * barPic.Height));

            Rectangle rectanglePointer = new Rectangle((int)positionPointer.X, (int)positionPointer.Y,
                (int)(scaleW * texture.Width), (int)(scaleH * texture.Height));

            spriteBatch.Draw(barPic, rectangleBar, Color.White);
            spriteBatch.Draw(texture, rectanglePointer, Color.White);
        }


        /// <summary>
        /// Allows the slider to run correctly.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
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


        /// <summary>
        /// Moves the pointer according to the movement of the hand. Incrementing
        /// or decrementing the value according to the movement.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        private void Move()
        {
            Skeleton skeleton = user.Kinect.requestSkeleton();
            if (skeleton != null)
            {
                Point hand = user.Kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, screenH);

                if ((hand.X - positionPointer.X) >= 30 && !(Value == 4))
                {
                    positionPointer.X += (barPic.Width / 5);
                    Value++;
                }
                if ((hand.X - positionPointer.X) <= -30 && !(Value == 1))
                {
                    positionPointer.X -= (barPic.Width / 5);
                    Value--;
                }
            }
        }


        /// <summary>
        /// Changes the pointer to the activated pointer picture.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        private void On()
        {
            texture = onPic;
        }


        /// <summary>
        /// Changes the pointer to the disactivated pointer picture.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        private void Off()
        {
            texture = offPic;
        }


        /// <summary>
        /// Checks if the user's hand is over the pointer or not.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <returns>Returns true if the user's hand is hovering over the pointer.</returns>
        private bool CheckCollision()
        {
            Skeleton skeleton = user.USER;
            if (skeleton != null)
            {
                Point hand = user.Kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, screenH);
                Rectangle r1 = new Rectangle(hand.X, hand.Y, 50, 50);
                Rectangle r2 = new Rectangle((int)positionPointer.X, (int)positionPointer.Y, 
                    offPic.Width, offPic.Height);

                return r1.Intersects(r2);
            }
            return false;
        }

    }
}
