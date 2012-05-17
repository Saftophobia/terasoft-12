using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;

namespace ButtonsAndSliders
{
    class Timer1
    {
        private double startTime;
        private bool running;

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// used to start the timer by marking the time and changing the status of the timer
        /// to runnig
        /// </summary>
        public void Start(GameTime gameTime)
        {
            running = true;
            startTime = gameTime.TotalGameTime.TotalMilliseconds;
        }


        /// <summary>
        /// used to get the time since the timer has started, to the time this method is called
        /// </summary>
        /// <returns>the duration that the timer has spent since it's started</returns>
        public double GetDuration(GameTime gameTime)
        {
            if (running)
            {
                double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
                return (currentTime - startTime);
            }
            return 0;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// changing the status of the timer to not running
        /// </summary>
        public void Stop()
        {
            running = false;
        }

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// used to checks if the timer is running or not
        /// </summary>
        /// <returns>true if the timer is running and tracking the time</returns>
        public bool IsRunning()
        {
            return running;
        }
    }
}
