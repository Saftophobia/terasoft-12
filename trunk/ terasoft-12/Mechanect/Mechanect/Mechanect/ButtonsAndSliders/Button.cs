using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using ButtonsAndSliders;
using Mechanect.Common;

namespace ButtonsAndSliders
{
    /// <summary>
    /// Button used to perform an action when the user's hand hovers over it.
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: AhmeD HegazY</para>
    /// </remarks>
    public class Button
    {

        private Vector2 position;
        private GifAnimation.GifAnimation texture, animation, stopped;
        private int screenW, screenH;

        private User user;
        private Texture2D hand;
        private Vector2 handPosition;

        private Timer1 timer;

        private bool status;


        /// <summary>
        /// The constructor used to initialize the button.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="texture">The picture of the button when it is not selected.</param>
        /// <param name="animation">The picture of the button when it is selected.</param>
        /// <param name="position">The position of the button, where the center is in the top left corner.</param>
        /// <param name="screenW">The width of the screen.</param>
        /// <param name="screenH">The height of the screen.</param>
        /// <param name="hand">The picture of the hand.</param>
        /// <param name="user">The instance of the user.</param>
        public Button(GifAnimation.GifAnimation texture, GifAnimation.GifAnimation animation,
            Vector2 position, int screenW, int screenH, Texture2D hand, User user)
        {
            this.position = position;
            this.texture = texture;
            this.stopped = texture;
            this.animation = animation;
            this.screenW = screenW;
            this.screenH = screenH;
            this.hand = hand;
            this.user = user;
            timer = new Timer1();

        }


        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">The spritebatch used to draw the texture.</param>

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture.GetTexture(), position, Color.White);
        }


        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">The spritebatch used to draw the texture.</param>
        /// <param name="scale">The ratio used to scale the width and the height of the button.</param>
        public void Draw(SpriteBatch spriteBatch, float scale)
        {

            Rectangle rectangle = new Rectangle((int) position.X, (int) position.Y, 
                (int)(scale * texture.Width), (int)(scale * texture.Height));
            spriteBatch.Draw(texture.GetTexture(), rectangle, Color.White);
        }


        /// <summary>
        /// Draws the button.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">The spritebatch used to draw the texture.</param>
        /// <param name="scaleW">The ratio used to scale the width of the button.</param>
        /// <param name="scaleW">The ratio used to scale the height of the button.</param>
        public void Draw(SpriteBatch spriteBatch, float scaleW, float scaleH)
        {
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y,
                (int)(scaleW * texture.Width), (int)(scaleH * texture.Height));
            spriteBatch.Draw(texture.GetTexture(), rectangle, Color.White);
        }


        /// <summary>
        /// Runs the logic of the button, such as changing the button's picture to the animated one.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            user.setSkeleton();
            MoveHand();
            texture.Update(gameTime.ElapsedGameTime.Ticks);

            if (CheckCollision())
            {
                if (!timer.IsRunning())
                    timer.Start(gameTime);
                else
                {
                    Animate();
                    if (timer.GetDuration(gameTime) >= (2000))
                    {
                        status = true;
                        timer.Stop();
                    }

                }
            }
            else
            {
                timer.Stop();
                Stop();
            }


        }


        /// <summary>
        /// Used to track the user's hand.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        private void MoveHand()
        {
            Skeleton skeleton = user.USER;
            if (skeleton != null)
            {
                handPosition.X = user.Kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], 
                    screenW, screenH).X;
                handPosition.Y = user.Kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], 
                    screenW, screenH).Y;
            }
        }


        /// <summary>
        /// Changes the button to the animated picture.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        private void Animate()
        {
            texture = animation;
        }


        /// <summary>
        /// Changes the button to the stopped picture.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        private void Stop()
        {
            texture = stopped;
        }


        /// <summary>
        /// Checks if the user's hand is over the button or not.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <returns>Returns true if the user's hand is over the button.</returns>
        private bool CheckCollision()
        {
            Skeleton skeleton = user.Kinect.requestSkeleton();
            if (skeleton != null)
            {
                Point hand = user.Kinect.GetJointPoint(skeleton.Joints[JointType.HandRight], screenW, screenH);
                Rectangle r1 = new Rectangle(hand.X, hand.Y, 20, 20);
                Rectangle r2 = new Rectangle((int)position.X, (int)position.Y, 
                    texture.GetTexture().Width, texture.GetTexture().Height);

                return r1.Intersects(r2);
            }
            return false;
        }


        /// <summary>
        /// Checks if the button was clicked or not.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <returns>Returns true if the user clicked on the button.</returns>
        public bool IsClicked()
        {
            return status;
        }


        /// <summary>
        /// Resets the status variable to false, which means the button is not clicked.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        public void Reset()
        {
            status = false;
        }


        /// <summary>
        /// Draws the hand on the screen. It must be called after beginning the SpriteBatch.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="spriteBatch">The spritebatch used to draw the texture.</param>
        public void DrawHand(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(hand, handPosition, Color.White);
        }

    }
}
