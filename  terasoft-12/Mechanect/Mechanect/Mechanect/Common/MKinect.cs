using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;

namespace Mechanect.Common
{
    class MKinect
    {
        public KinectSensor _KinectDevice;
        private Skeleton[] _FrameSkeletons;

        public Skeleton globalSkeleton;
        private int dropFrameRate;
        private int frameCounter;

        public MKinect()
        {
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this.KinectDevice = KinectSensor.KinectSensors
            .FirstOrDefault(x => x.Status == KinectStatus.Connected);
            dropFrameRate = -1;
            frameCounter = 0;
        }
        /// <summary>
        /// This method should be used to set the frame capturing rate for the kinect sensor.
        /// Note that this method will set any input bigger than 30 to 30, because the maximum kinect framerate is 30
        /// </summary>
        /// <param name="dropFrameRate"></param>
        public void SetDropFrameRate(int dropFrameRate)
        {
            this.dropFrameRate = dropFrameRate-1;
            frameCounter = 0;
        }

        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Initializing:
                case KinectStatus.Connected:
                    this.KinectDevice = e.Sensor;
                    break;
                case KinectStatus.Disconnected:
                    //TODO: Give the user feedback to plug-in a Kinect device.
                    this.KinectDevice = null;
                    break;
                default:
                    //TODO: Show an error state
                    break;
            }
        }

        public KinectSensor KinectDevice
        {
            get { return this._KinectDevice; }
            set
            {
                if (this._KinectDevice != value)
                {
                    //Uninitialize
                    if (this._KinectDevice != null)
                    {
                        this._KinectDevice.Stop();
                        this._KinectDevice.SkeletonFrameReady -= KinectDevice_SkeletonFrameReady;
                        this._KinectDevice.SkeletonStream.Disable();
                        this._FrameSkeletons = null;
                    }
                    this._KinectDevice = value;
                    //Initialize
                    if (this._KinectDevice != null)
                    {
                        if (this._KinectDevice.Status == KinectStatus.Connected)
                        {
                            var parameters = new TransformSmoothParameters
                            {
                                Smoothing = 0.0f,
                                Correction = 0.0f,
                                Prediction = 0.0f,
                                JitterRadius = 0.0f,
                                MaxDeviationRadius = 0.0f
                            };

                            this._KinectDevice.SkeletonStream.Enable(parameters);
                            this._FrameSkeletons = new
                            Skeleton[this._KinectDevice.SkeletonStream.FrameSkeletonArrayLength];
                            this.KinectDevice.SkeletonFrameReady +=
                            KinectDevice_SkeletonFrameReady;
                            this._KinectDevice.Start();
                        }
                    }
                }
            }
        }

        private void KinectDevice_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            //to be tested if it works correctly on the kinect tomorrow
            if (frameCounter == dropFrameRate)
            {
                frameCounter = 0;
                return;
            }
            else
                frameCounter++;
            
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    Skeleton skeleton;
                    frame.CopySkeletonDataTo(this._FrameSkeletons);
                    for (int i = 0; i < this._FrameSkeletons.Length; i++)
                    {
                        skeleton = this._FrameSkeletons[i];
                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.globalSkeleton = skeleton;
                        }
                    }
                    //skeleton = _FrameSkeletons.OrderBy(s => s.Position.Z)
                    //    .FirstOrDefault(s => s.TrackingState == SkeletonTrackingState.Tracked);
                }
            }
        }



        public Skeleton requestSkeleton()
        {
            return globalSkeleton;
        }


        public Point GetJointPoint(Joint joint, int sw, int sh)
        {
            DepthImagePoint point = this.KinectDevice.MapSkeletonPointToDepth(joint.Position, 
                DepthImageFormat.Resolution640x480Fps30);
            point.X = (int)(point.X * (sw*2) / this.KinectDevice.DepthStream.FrameWidth);
            point.Y = (int)(point.Y * sh / this.KinectDevice.DepthStream.FrameHeight);
            return new Point(point.X, point.Y);
        }

        public Point mapMetersToPixels(SkeletonPoint p, int sw, int sh)
        {
            DepthImagePoint point = this.KinectDevice.MapSkeletonPointToDepth(p,
                DepthImageFormat.Resolution640x480Fps30);
            point.X = (int)(point.X * (sw * 2) / this.KinectDevice.DepthStream.FrameWidth);
            point.Y = (int)(point.Y * sh / this.KinectDevice.DepthStream.FrameHeight);
            return new Point(point.X, point.Y);
        }

    }
}
