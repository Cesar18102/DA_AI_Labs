using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavParser2
{
    public class TimedAmplitude
    {
        public double Time { get; private set; }
        public double Amplitude { get; private set; }

        public TimedAmplitude(double time, double amplitude)
        {
            Time = time;
            Amplitude = amplitude;
        }

        public static implicit operator Point(TimedAmplitude TA) => new Point(TA.Time, TA.Amplitude);
    }
}
