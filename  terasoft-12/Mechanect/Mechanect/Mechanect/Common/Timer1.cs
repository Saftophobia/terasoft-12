using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Common.Classes
{
    class Timer1
    {
        DateTime startTime;
        bool running;

        ///<remarks>
        ///<para>
        ///Author: HegazY
        ///</para>
        ///</remarks>
        /// <summary>
        /// used to start the timer by marking the time and changing the status of the timer
        /// to runnig
        /// </summary>
        public void start()
        {
            running = true;
            startTime = DateTime.Now;
        }


        /// <summary>
        /// used to get the time since the timer has started, to the time this method is called
        /// </summary>
        /// <returns>the duration that the timer has spent since it's started</returns>
        public double getDuration()
        {
            if (running)
            {
                return (DateTime.Now - startTime).TotalMilliseconds;
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
        public void stop()
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
        public bool isRunning()
        {
            return running;
        }
    }
}
