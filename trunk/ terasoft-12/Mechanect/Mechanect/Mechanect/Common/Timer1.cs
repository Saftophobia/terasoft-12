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

        public void start()
        {
            running = true;
            startTime = DateTime.Now;
        }

        public double getDuration()
        {
            if (running)
            {
                return (DateTime.Now - startTime).TotalMilliseconds;
            }
            return 0;
        }

        public void stop()
        {
            running = false;
        }

        public bool isRunning()
        {
            return running;
        }
    }
}
