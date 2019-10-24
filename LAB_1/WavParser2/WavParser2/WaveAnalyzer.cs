using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WavParser2.Files;

namespace WavParser2
{
    public class WaveAnalyzer
    {
        public static List<TimedAmplitude> GetTimedAmplitudes(AudioFile file)
        {
            List<TimedAmplitude> TimedAmplitudes = new List<TimedAmplitude>();

            for (int i = 0; i < file.Data.Length; i++)
                TimedAmplitudes.Add(new TimedAmplitude((double)i / file.Data.Length, file.Data[i]));

            return TimedAmplitudes;
        }

        public static List<TimedAmplitude> GetExtremums(List<TimedAmplitude> timedAmplitudes)
        {
            List<TimedAmplitude> TimedAmplitudeExtremums = new List<TimedAmplitude>();
            TimedAmplitudeExtremums.Add(timedAmplitudes.First());

            bool isIncreasing = timedAmplitudes[1].Amplitude >= timedAmplitudes[0].Amplitude;
            for (int i = 1; i < timedAmplitudes.Count - 1; i++)
            {
                bool currentIsIncreasing = timedAmplitudes[i + 1].Amplitude >= timedAmplitudes[i].Amplitude;
                if (isIncreasing != currentIsIncreasing)
                    TimedAmplitudeExtremums.Add(timedAmplitudes[i]);

                isIncreasing = currentIsIncreasing;
            }

            TimedAmplitudeExtremums.Add(timedAmplitudes.Last());

            return TimedAmplitudeExtremums;
        }

        public static List<TimedAmplitude> RemoveNoise(List<TimedAmplitude> timedExtremums, double maxNoise) //AVG chrono
        {
            if (maxNoise == 0)
                return timedExtremums;

            TimedAmplitude FirstWave = timedExtremums.First();
            List<TimedAmplitude> NoNoiseExtremums = new List<TimedAmplitude>();

            NoNoiseExtremums.Add(FirstWave);
            double LastWaveSize = FirstWave.Amplitude;

            for (int i = 1; i < timedExtremums.Count - 1; i++)
            {
                TimedAmplitude wave = timedExtremums[i];
                if (Math.Abs(wave.Amplitude - LastWaveSize) / Math.Max(wave.Amplitude, LastWaveSize) >= maxNoise)
                {
                    NoNoiseExtremums.Add(wave);
                    LastWaveSize = wave.Amplitude;
                }
            }

            NoNoiseExtremums.Add(timedExtremums.Last());

            return NoNoiseExtremums;
        }

        public static List<TimedAmplitude> RemoveNoiseSmoothing(List<TimedAmplitude> timedExtremums, int pointGroupCount)
        {
            List<TimedAmplitude> SmoothedAmplitudes = new List<TimedAmplitude>();

            for(int i = 0; i < timedExtremums.Count; i += pointGroupCount)
            {
                double sumAmplitude = timedExtremums[i].Amplitude / 2;

                double startTime = timedExtremums[i].Time;
                double sumTime = 0;

                int count = 1;
                for ( ; count < pointGroupCount - 1 && count + i < timedExtremums.Count - 1; count++)
                {
                    sumAmplitude += timedExtremums[count + i].Amplitude;
                    sumTime += timedExtremums[count + i].Time - startTime;
                }

                if (count + i < timedExtremums.Count)
                {
                    sumAmplitude += timedExtremums[count + i].Amplitude / 2;
                    sumTime += timedExtremums[count + i].Time - startTime;
                }

                double time = startTime + sumTime / count;
                double avgAmplitude = sumAmplitude / count;

                SmoothedAmplitudes.Add(new TimedAmplitude(time, avgAmplitude));
            }

            return SmoothedAmplitudes;
        }

        public static List<List<TimedAmplitude>> SplitSounds(List<TimedAmplitude> noNoiseExtremums, double pauseDuration)
        {
            List<List<TimedAmplitude>> sounds = new List<List<TimedAmplitude>>();

            double prevWaveTime = -Constants.PAUSE_DURATION;
            foreach (TimedAmplitude wave in noNoiseExtremums)
            {
                if (wave.Time - prevWaveTime >= Constants.PAUSE_DURATION)
                    sounds.Add(new List<TimedAmplitude>());

                sounds.Last().Add(wave);
                prevWaveTime = wave.Time;
            }

            foreach(List<TimedAmplitude> sound in sounds)
            {
                if (sound.Count >= 2 && sound.First().Amplitude > sound[1].Amplitude)
                    sound.RemoveAt(0);

                if (sound.Count >= 2 && sound.Last().Amplitude > sound[sound.Count - 2].Amplitude)
                    sound.RemoveAt(sound.Count - 1);
            }

            sounds.RemoveAll(S => S.Count <= 1);
            return sounds;
        }

        public delegate (double x, double y) WaveToCoordinatesTranformer(TimedAmplitude min1, TimedAmplitude max, TimedAmplitude min2);
        public static List<Point> GetSoundCoordinates(List<TimedAmplitude> sound, WaveToCoordinatesTranformer waveToCoordinatesTranformer)
        {
            List<Point> coords = new List<Point>();

            TimedAmplitude prev1 = sound.First();
            TimedAmplitude prev2 = sound[1];

            for (int i = 2; i < sound.Count; i++)
            {
                (double x, double y) = waveToCoordinatesTranformer(prev1, prev2, sound[i]);
                Point P = new Point(x, y);

                if(!coords.Contains(P))
                    coords.Add(P);

                prev1 = prev2;
                prev2 = sound[i];
            }

            return coords;
        }

        public static List<Point> GetSoundCoordinates(List<TimedAmplitude> timedAmplitudesSource, List<TimedAmplitude> sounds)
        {
            List<Point> Coordinates = new List<Point>();

            for(int i = 0; i < sounds.Count - 2; i++)
            {
                double start = sounds[i].Time;
                double end = sounds[i + 1].Time;
                double end2 = sounds[i + 2].Time;

                List<TimedAmplitude> FirstHalfWave = timedAmplitudesSource.Where(TA => TA.Time >= start && TA.Time <= end).ToList();
                List<TimedAmplitude> SecondHalfWave = timedAmplitudesSource.Where(TA => TA.Time >= end && TA.Time <= end2).ToList();

                double x = GetSquare(FirstHalfWave, sounds[i].Amplitude);
                double y = GetSquare(FirstHalfWave, sounds[i + 2].Amplitude);

                Point P = new Point(x, y);

                if (!Coordinates.Contains(P))
                    Coordinates.Add(P);
            }
            return Coordinates;
        }

        public static double GetSquare(List<TimedAmplitude> data, double zeroY)
        {
            double sum = 0;
            foreach (TimedAmplitude amplitude in data)
                sum += amplitude.Amplitude - zeroY;
            return sum * (data.Last().Time - data.First().Time) / data.Count;
        }
    }
}
