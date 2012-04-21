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

namespace Common.Classes
{
    class Kinect
    {
        private KinectSensor _KinectDevice;
        private Skeleton[] _FrameSkeletons;

        public Skeleton globalSkeleton;

        public Kinect()
        {
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this.KinectDevice = KinectSensor.KinectSensors
            .FirstOrDefault(x => x.Status == KinectStatus.Connected);
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
                                Smoothing = 0.1f,
                                Correction = 0.1f,
                                Prediction = 0.1f,
                                JitterRadius = 0.5f,
                                MaxDeviationRadius = 0.1f
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
            point.X = (int)(point.X * (sw) / this.KinectDevice.DepthStream.FrameWidth);
            point.Y = (int)(point.Y * sh / this.KinectDevice.DepthStream.FrameHeight);
            return new Point(point.X, point.Y);
        }

    }
}
