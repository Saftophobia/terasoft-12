using System.Timers;
using Microsoft.Xna.Framework;

namespace ButtonsAndSliders
{
    /// <summary>
    /// Timer
    /// </summary>
    /// <remarks>
    /// <para>AUTHOR: AhmeD HegazY</para>
    /// </remarks>
    class Timer1
    {
        private double startTime;
        private bool running;


        /// <summary>
        /// Starts the timer by marking the time and changing the status of the timer
        /// to running.
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
        /// Gets the difference between the start time and the current time.
        /// </summary>
        /// <remarks>
        /// <para>AUTHOR: AhmeD HegazY</para>
        /// </remarks>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>The duration the timer has spent since it started.</returns>
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
