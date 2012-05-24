using System.Timers;
using Microsoft.Xna.Framework;

namespace ButtonsAndSliders
{
    class Timer1
    {
        private double startTime;
        private bool running;


        /// <summary>
        /// Starts the timer by marking the time and changing the status of the timer
        /// to runnig.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Start(GameTime gameTime)
        {
            running = true;
            startTime = gameTime.TotalGameTime.TotalMilliseconds;
        }


        /// <summary>
        /// Gets the time since the timer has started to the time this method is called.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The duration the timer has spent since it's started.</returns>
        public double GetDuration(GameTime gameTime)
        {
            if (running)
            {
                double currentTime = gameTime.TotalGameTime.TotalMilliseconds;
                return (currentTime - startTime);
            }
            return 0;
        }


        /// <summary>
        /// Changes the status of the timer to not running.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        public void Stop()
        {
            running = false;
        }


        /// <summary>
        /// Checks if the timer is running or not.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <returns>Returns true if the timer is running.</returns>
        public bool IsRunning()
        {
            return running;
        }
    }
}
