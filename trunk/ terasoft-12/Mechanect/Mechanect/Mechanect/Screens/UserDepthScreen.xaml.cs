using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;
using System.Xaml;
using Mechanect.Common;
using System.Windows.Markup;

namespace Mechanect.Screens
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     partial class UserDepthScreen
    {
        public UserDepthScreen()
        {
            InitializeComponent();
        }
        KinectSensor _sensor;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
         /*   kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
            if (KinectSensor.KinectSensors.Count > 0)
            {
                _sensor = KinectSensor.KinectSensors[0];
                if (_sensor.Status == KinectStatus.Connected)
                {
                    _sensor.ColorStream.Enable();
                    _sensor.DepthStream.Enable();
                    _sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(_newSensor_AllFramesReady);
                    _sensor.Start();
                }
            }*/
            _sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(_newSensor_AllFramesReady);
        }
        void kinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor oldSensor = (KinectSensor)e.OldValue;
            StopKinect(oldSensor);
            KinectSensor newSensor = (KinectSensor)e.NewValue;
            newSensor.ColorStream.Enable();
            newSensor.DepthStream.Enable();
            //newSensor.SkeletonStream.Enable();
            newSensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(_newSensor_AllFramesReady);
            try
            {
                newSensor.Start();
            }
            catch (System.IO.IOException)
            {
                kinectSensorChooser1.AppConflictOccurred();
            }
        }
        void _newSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = (ColorImageFrame)e.OpenColorImageFrame())
            {

                if (colorFrame != null)
                {
                    byte[] pixels = new byte[colorFrame.PixelDataLength];
                    colorFrame.CopyPixelDataTo(pixels);
                    int stride = colorFrame.Width * 4;
                    image1.Source = BitmapSource.Create(colorFrame.Width, colorFrame.Height, 96, 96, PixelFormats.Bgr32, null, pixels, stride);
                }
            }
            using (DepthImageFrame depthFrame = (DepthImageFrame)e.OpenDepthImageFrame())
            {
                if (depthFrame != null)
                {
                    short[] depthPixels = new short[depthFrame.PixelDataLength];
                    depthFrame.CopyPixelDataTo(depthPixels);
                    byte[] pixels = GeneratePixels(depthFrame);
                    int stride = depthFrame.Width * 2;
                    image2.Source = BitmapSource.Create(depthFrame.Width, depthFrame.Height, 96, 96, PixelFormats.Bgr32, null, pixels, stride);
                }
            }
        }
        byte[] GeneratePixels(DepthImageFrame frame)
        {
            short[] depths = new short[frame.PixelDataLength];
            byte[] pixels = new byte[frame.Width * frame.Height * 4];
            const int blue = 0;
            const int green = 1;
            const int red = 2;
            for (int depthi = 0, colori = 0; depthi < depths.Length & colori < pixels.Length; colori += 4, depthi++)
            {
                int depth = depths[depthi] >> DepthImageFrame.PlayerIndexBitmaskWidth;
                int player = depths[depthi] & DepthImageFrame.PlayerIndexBitmask;

                if (depth <= 900)
                {
                    pixels[colori + green] = 255;
                    pixels[colori + blue] = 0;
                    pixels[colori + red] = 0;

                }
                if (depth > 900 && depth <= 2000)
                {
                    pixels[colori + green] = 0;
                    pixels[colori + blue] = 255;
                    pixels[colori + red] = 0;

                }
                if (depth > 2000)
                {
                    pixels[colori + green] = 0;
                    pixels[colori + blue] = 0;
                    pixels[colori + red] = 255;
                }
                pixels[colori + blue] = 255;
                pixels[colori + green] = 0;
                pixels[colori + red] = 0;
            }
            return pixels;
        }

        void StopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                sensor.Stop();
                sensor.DepthStream.Disable();
                sensor.ColorStream.Disable();
                sensor.AudioSource.Stop();
                sensor.SkeletonStream.Disable();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

    }
}

