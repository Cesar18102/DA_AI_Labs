using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavParser2
{
    public static class Constants
    {
        public static readonly int[] SMOOTH_POINT_GROUP_COUNTS = { };

        public const double RESULT_SCALE_SIMPLE = 5;
        public const double RESULT_SCALE_SQUARE = 10;

        public const double NOISE_MIN_AMPLITUDE = 0.228;
        public const double NOISE_MAX_AMPLITUDE = 0.231;
        public const double NOISE_DELTA_AMPLITUDE = 0.001;

        public const double PAUSE_DURATION = 0.05;

        public static Random R = new Random();
    }
}
