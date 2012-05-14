using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mechanect.Common;
using Microsoft.Kinect;

namespace Mechanect.Screens
{
    public class UserStream : GameScreen
    {
        GraphicsDevice graphics;
        SpriteBatch spriteBatch;
        KinectSensor kinect;
        Texture2D colorVideo, depthVideo;
        int screenWidth;
        int screenHeight;
        Boolean debugging = true;
        ContentManager content;
        Texture2D screen;
        Vector2 screenPosition;
        SpriteFont font;

        public UserStream()
        {
        }

        public override void Initialize()
        {
            //Initialise Kinect

            graphics = ScreenManager.GraphicsDevice;
            screenWidth = graphics.Viewport.Width;
            screenHeight = graphics.Viewport.Height;
            spriteBatch = ScreenManager.SpriteBatch;
            content = ScreenManager.Game.Content;
            screenPosition = new Vector2(screenWidth / 2, screenHeight / 4);
            font = content.Load<SpriteFont>("spriteFont1");
            screen = content.Load<Texture2D>("Textures/screen");
            try
            {
                kinect = KinectSensor.KinectSensors[0];
                kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                kinect.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);
                kinect.Start();
                colorVideo = new Texture2D(graphics, kinect.ColorStream.FrameWidth, kinect.ColorStream.FrameHeight);
                depthVideo = new Texture2D(graphics, kinect.DepthStream.FrameWidth, kinect.DepthStream.FrameHeight);
                Debug.WriteLineIf(debugging, kinect.Status);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            base.Initialize();
        }

        public override void LoadContent()
        {
            try
            {
                kinect.ElevationAngle = 0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime, bool covered)
        {
            base.Update(gameTime, false);
        }

        public override void Draw(GameTime gameTime)
        {
            try
            {
                //graphics.Clear(Color.Transparent);
                //spriteBatch.Begin();
                //spriteBatch.Draw(screen, screenPosition, null, Color.Transparent, 0, new Vector2(screen.Width / 2, screen.Height / 2), 1f, SpriteEffects.None, 0);
                //spriteBatch.End();
                spriteBatch.Begin();
                spriteBatch.Draw(colorVideo, new Rectangle(0, 0, colorVideo.Width, colorVideo.Height), Color.White);
                spriteBatch.Draw(depthVideo, new Rectangle(640, 0, depthVideo.Width, depthVideo.Height), Color.White);
                spriteBatch.End();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        public override void Remove()
        {
            base.Remove();
        }

        void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs imageFrames)
        {
            //
            // Color Frame 
            //

            //Get raw image
            ColorImageFrame colorVideoFrame = imageFrames.OpenColorImageFrame();

            if (colorVideoFrame != null)
            {
                //Create array for pixel data and copy it from the image frame
                Byte[] pixelData = new Byte[colorVideoFrame.PixelDataLength];
                colorVideoFrame.CopyPixelDataTo(pixelData);

                //Convert RGBA to BGRA
                Byte[] bgraPixelData = new Byte[colorVideoFrame.PixelDataLength];
                for (int i = 0; i < pixelData.Length; i += 4)
                {
                    bgraPixelData[i] = pixelData[i + 2];
                    bgraPixelData[i + 1] = pixelData[i + 1];
                    bgraPixelData[i + 2] = pixelData[i];
                    bgraPixelData[i + 3] = (Byte)255; //The video comes with 0 alpha so it is transparent
                }

                // Create a texture and assign the realigned pixels
                colorVideo = new Texture2D(graphics, colorVideoFrame.Width, colorVideoFrame.Height);
                colorVideo.SetData(bgraPixelData);
            }
            else return;

            //
            // Depth Frame
            //
            DepthImageFrame depthVideoFrame = imageFrames.OpenDepthImageFrame();

            if (depthVideoFrame != null)
            {
                Debug.WriteLineIf(debugging, "Frame");
                //Create array for pixel data and copy it from the image frame
                short[] pixelData = new short[depthVideoFrame.PixelDataLength];
                depthVideoFrame.CopyPixelDataTo(pixelData);

                for (int i = 0; i < 10; i++)
                { Debug.WriteLineIf(debugging, pixelData[i]); }

                // Convert the Depth Frame
                // Create a texture and assign the realigned pixels
                //
                depthVideo = new Texture2D(graphics, depthVideoFrame.Width, depthVideoFrame.Height);
                depthVideo.SetData(ConvertDepthFrame(pixelData, kinect.DepthStream));
            }
            else return;
        }

        private byte[] ConvertDepthFrame(short[] depthFrame, DepthImageStream depthStream)
        {
            int RedIndex = 0, GreenIndex = 1, BlueIndex = 2, AlphaIndex = 3;

            byte[] depthFrame32 = new byte[depthStream.FrameWidth * depthStream.FrameHeight * 4];

            for (int i16 = 0, i32 = 0; i16 < depthFrame.Length && i32 < depthFrame32.Length; i16++, i32 += 4)
            {
                int player = depthFrame[i16] & DepthImageFrame.PlayerIndexBitmask;
                int realDepth = depthFrame[i16] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // transform 13-bit depth information into an 8-bit intensity appropriate
                // for display (we disregard information in most significant bit)
                byte intensity = (byte)(~(realDepth >> 4));

                depthFrame32[i32 + RedIndex] = (byte)(intensity);
                depthFrame32[i32 + GreenIndex] = (byte)(intensity);
                depthFrame32[i32 + BlueIndex] = (byte)(intensity);
                depthFrame32[i32 + AlphaIndex] = 255;
            }

            return depthFrame32;
        }
    }
}
