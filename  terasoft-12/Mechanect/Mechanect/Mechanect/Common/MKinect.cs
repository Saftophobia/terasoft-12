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
    public class MKinect
    {
        public KinectSensor _KinectDevice;
        private Skeleton[] _FrameSkeletons;

        public Skeleton globalSkeleton;
        public Skeleton globalSkeleton2;
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
        /// Sets the drop rate of frames.For example, for 12/15 FPS, use dropFrameRate=2.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Ahmed Badr.</para>
        /// <para>NOTE: For maximum FPS, use dropFrameRate=-1.</para>
        /// </remarks>
        /// <param name="dropFrameRate">
        /// Specifies the drop rate for frames.</param>
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

        /// <summary>
        /// This method will be automatically called with the maximum possible frame rate. If the requested 
        /// frame rate is less than the maximum frame rate, then this method will just drop frames to
        /// achieve the requested frame rate.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: Ahmed Badr.</para>
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KinectDevice_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            if (frameCounter == dropFrameRate)
            {
                frameCounter = 0;
                return;
            }
            frameCounter++;
            
            using (SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    Skeleton skeleton;
                    Skeleton skeleton2;
                    frame.CopySkeletonDataTo(this._FrameSkeletons);
                    for (int i = 0; i < this._FrameSkeletons.Length; i++)
                    {
                        skeleton = this._FrameSkeletons[i];
                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.globalSkeleton = skeleton;
                        }
                    }
                    for (int y = 0; y < this._FrameSkeletons.Length; y++)
                    {
                        skeleton2 = this._FrameSkeletons[y];
                        if (skeleton2.TrackingState == SkeletonTrackingState.Tracked && skeleton2 != globalSkeleton)
                        {
                            this.globalSkeleton2 = skeleton2;
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

        public Skeleton request2ndSkeleton()
        {
            return globalSkeleton2;
        }


        public Point GetJointPoint(Joint joint, int sw, int sh)
        {
            Vector2 point = new Vector2(joint.Position.X, joint.Position.Y);
            point.X = (point.X) * (sw);
            point.Y = (sh) - ((point.Y + 1) * sh / 2);

            return new Point((int)(point.X), (int)point.Y * 2);
        }

    

    }
}
