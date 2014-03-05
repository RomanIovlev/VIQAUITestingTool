using System;
using System.Diagnostics;

namespace VIQA.Common
{
    public class Timer
    {
        private readonly Stopwatch _watch = new Stopwatch();
        private TimeSpan _timeout;
        public double Timeout {
            get { return _timeout.TotalMilliseconds; }
            set { _timeout = TimeSpan.FromMilliseconds((value)); }
        }

        public Timer()
        { 
            _watch = Stopwatch.StartNew();
        }

        public TimeSpan TimePassed()
        {
            return _watch.Elapsed;
        }
        
        public bool TimeoutPassed(double timeout = 10000)
        {
            _timeout = TimeSpan.FromMilliseconds(timeout);
            return _watch.Elapsed > _timeout;
        }
    }
}
