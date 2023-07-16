using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Score
{
    public class StopwatchManager
    {
        private Stopwatch stopwatch;
        private TimeSpan pausedTime;
        private bool isRunning;

        public StopwatchManager()
        {
            stopwatch = new Stopwatch();
            isRunning = false;
        }

        public TimeSpan ElapsedTime
        {
            get { return stopwatch.Elapsed + pausedTime; }
        }

        public void Start()
        {
            if (!isRunning)
            {
                stopwatch.Start();
                isRunning = true;
            }
        }

        public void Pause()
        {
            if (isRunning)
            {
                stopwatch.Stop();
                isRunning = false;
            }
        }

        public void Reset()
        {
            if (isRunning)
            {
                return;
            }
            else
            {
                stopwatch.Reset();
                isRunning = false;
            }
        }
    }
}
