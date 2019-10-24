using System;

using WavParser2.Files;
using WavParser2.FileFactories;

using Tao.OpenGl;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Drawing;
using Tao.FreeGlut;
using System.IO;

namespace WavParser2
{
    class Program
    {
        static void Main(string[] args)
        { 

            List<Color> colors = new List<Color>() { Color.Red, Color.Green, Color.Blue, Color.Violet, Color.Turquoise };

            WavAudioFileFactory wavFactory = new WavAudioFileFactory();

            AudioFile file = wavFactory.ParseAudioFile("D:\\!!!OLEG\\!!!WORK\\Studying\\АДОШ\\Lab 1-2\\А_У_И.wav");

            AudioFile file1 = wavFactory.ParseAudioFile("D:\\!!!OLEG\\!!!WORK\\Studying\\АДОШ\\Lab 1-2\\sound_sum_test_2.wav");
            AudioFile file2 = wavFactory.ParseAudioFile("D:\\!!!OLEG\\!!!WORK\\Studying\\АДОШ\\Lab 1-2\\sound_sum_test_3.wav");
            IAudioFile fsum = file1.Sum(file2);
            fsum.ConstructFile("D:\\!!!OLEG\\!!!WORK\\Studying\\АДОШ\\Lab 1-2\\sum.wav");
            return;

            Console.WriteLine(file);
            Console.WriteLine();

            if (Directory.Exists(Environment.CurrentDirectory + "/Total"))
                Directory.Delete(Environment.CurrentDirectory + "/Total", true);

            for (double i = Constants.NOISE_MIN_AMPLITUDE; i <= Constants.NOISE_MAX_AMPLITUDE; i += Constants.NOISE_DELTA_AMPLITUDE)
            {
                if (Directory.Exists(Environment.CurrentDirectory + "/" + i.ToString()))
                    Directory.Delete(Environment.CurrentDirectory + "/" + i.ToString(), true);

                List<TimedAmplitude> TimedAmplitudes = WaveAnalyzer.GetTimedAmplitudes(file);

                List<Point> TimedAmplitudesCoords = TimedAmplitudeListToPointList(TimedAmplitudes);
                Dictionary<List<Point>, Color> NoConversion = new Dictionary<List<Point>, Color>() { { TimedAmplitudesCoords, Color.Red } };

                SaveGraphics("NoConversion.png", i.ToString() + "/NoConversion", 500, 500, false, 1, 1, NoConversion, (x, y) => (x * 2 - 1, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("NoConversionX10.png", i.ToString() + "/NoConversion", 500, 500, false, 1, 1, NoConversion, (x, y) => (x * 2 * 10 - 1 - 10, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("NoConversionX100.png", i.ToString() + "/NoConversion", 500, 500, false, 1, 1, NoConversion, (x, y) => (x * 2 * 100 - 1 - 100, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("NoConversionX1000.png", i.ToString() + "/NoConversion", 500, 500, false, 1, 1, NoConversion, (x, y) => (x * 2 * 1000 - 1 - 1000, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));

                Console.WriteLine("NoConversion_" + i + " saved");



                List<TimedAmplitude> TimedExtremums = WaveAnalyzer.GetExtremums(TimedAmplitudes);

                List<Point> TimedExtremumsCoords = TimedAmplitudeListToPointList(TimedExtremums);
                Dictionary<List<Point>, Color> Extremums = new Dictionary<List<Point>, Color>() { { TimedExtremumsCoords, Color.Red } };

                SaveGraphics("Extremums.png", i.ToString() + "/Extremums", 500, 500, false, 1, 1, Extremums, (x, y) => (x * 2 - 1, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("ExtremumsX10.png", i.ToString() + "/Extremums", 500, 500, false, 1, 1, Extremums, (x, y) => (x * 2 * 10 - 1 - 10, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("ExtremumsX100.png", i.ToString() + "/Extremums", 500, 500, false, 1, 1, Extremums, (x, y) => (x * 2 * 100 - 1 - 100, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("ExtremumsX1000.png", i.ToString() + "/Extremums", 500, 500, false, 1, 1, Extremums, (x, y) => (x * 2 * 1000 - 1 - 1000, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));

                Console.WriteLine("Extremums_" + i + " saved");



                List<TimedAmplitude> SmoothedTimedExtremums = TimedExtremums;

                for (int j = 0; j < Constants.SMOOTH_POINT_GROUP_COUNTS.Length; j++)
                {
                    SmoothedTimedExtremums = WaveAnalyzer.RemoveNoiseSmoothing(SmoothedTimedExtremums, Constants.SMOOTH_POINT_GROUP_COUNTS[j]);

                    List<Point> SmoothedTimedExtremumsCoords = TimedAmplitudeListToPointList(SmoothedTimedExtremums);
                    Dictionary<List<Point>, Color> SmoothedTimedExtremumsData = new Dictionary<List<Point>, Color>() { { SmoothedTimedExtremumsCoords, Color.Red } };

                    SaveGraphics($"Smoothed{j}.png", i.ToString() + "/Smoothed", 500, 500, false, 1, 1, SmoothedTimedExtremumsData, (x, y) => (x * 2 - 1, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                    SaveGraphics($"Smoothed{j}X10.png", i.ToString() + "/Smoothed", 500, 500, false, 1, 1, SmoothedTimedExtremumsData, (x, y) => (x * 2 * 10 - 1 - 10, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                    SaveGraphics($"Smoothed{j}X100.png", i.ToString() + "/Smoothed", 500, 500, false, 1, 1, SmoothedTimedExtremumsData, (x, y) => (x * 2 * 100 - 1 - 100, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                    SaveGraphics($"Smoothed{j}X1000.png", i.ToString() + "/Smoothed", 500, 500, false, 1, 1, SmoothedTimedExtremumsData, (x, y) => (x * 2 * 1000 - 1 - 1000, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));

                    Console.WriteLine("Smoothed{j}_" + i + " saved");
                }



                List<TimedAmplitude> NoNoiseTimedExtremums = WaveAnalyzer.RemoveNoise(SmoothedTimedExtremums, i);

                List<Point> NoNoiseTimedExtremumsCoords = TimedAmplitudeListToPointList(NoNoiseTimedExtremums);
                Dictionary<List<Point>, Color> NoNoise = new Dictionary<List<Point>, Color>() { { NoNoiseTimedExtremumsCoords, Color.Red } };

                SaveGraphics("NoNoise.png", i.ToString() + "/NoNoise", 500, 500, false, 1, 1, NoNoise, (x, y) => (x * 2 - 1, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("NoNoiseX10.png", i.ToString() + "/NoNoise", 500, 500, false, 1, 1, NoNoise, (x, y) => (x * 2 * 10 - 1 - 10, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("NoNoiseX100.png", i.ToString() + "/NoNoise", 500, 500, false, 1, 1, NoNoise, (x, y) => (x * 2 * 100 - 1 - 100, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("NoNoiseX1000.png", i.ToString() + "/NoNoise", 500, 500, false, 1, 1, NoNoise, (x, y) => (x * 2 * 1000 - 1 - 1000, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));

                Console.WriteLine("NoNoise_" + i + " saved");



                List<List<TimedAmplitude>> SplittedSounds = WaveAnalyzer.SplitSounds(NoNoiseTimedExtremums, Constants.PAUSE_DURATION);
                Dictionary<List<Point>, Color> SplittedSoundsData = new Dictionary<List<Point>, Color>() { };

                for (int j = 0; j < SplittedSounds.Count; j++)
                    SplittedSoundsData.Add(TimedAmplitudeListToPointList(SplittedSounds[j]), colors[j]);

                SaveGraphics("Sounds.png", i.ToString() + "/SplittedSounds", 500, 500, false, 1, 1, SplittedSoundsData, (x, y) => (x * 2 - 1, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("SoundsX10.png", i.ToString() + "/SplittedSounds", 500, 500, false, 1, 1, SplittedSoundsData, (x, y) => (x * 2 * 10 - 1 - 10, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("SoundsX100.png", i.ToString() + "/SplittedSounds", 500, 500, false, 1, 1, SplittedSoundsData, (x, y) => (x * 2 * 100 - 1 - 100, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));
                SaveGraphics("SoundsX1000.png", i.ToString() + "/SplittedSounds", 500, 500, false, 1, 1, SplittedSoundsData, (x, y) => (x * 2 * 1000 - 1 - 1000, y / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5));

                Console.WriteLine("Sounds_" + i + " saved");


                List<List<Point>> Coordinates = new List<List<Point>>();
                foreach (List<TimedAmplitude> sound in SplittedSounds)
                    Coordinates.Add(WaveAnalyzer.GetSoundCoordinates(sound, (min1, max, min2) => (max.Time - min1.Time, min2.Time - max.Time)));

                Dictionary<List<Point>, Color> CoordinatesData = new Dictionary<List<Point>, Color>() { };

                for (int j = 0; j < Coordinates.Count; j++)
                    CoordinatesData.Add(Coordinates[j], colors[j]);

                if (Coordinates.Count == 0)
                    continue;

                double scaleX = 2 / (Coordinates.Select(CS => CS.Max(C => C.X)).Max() - Coordinates.Select(CS => CS.Min(C => C.X)).Min());
                double scaleY = 2 / (Coordinates.Select(CS => CS.Max(C => C.Y)).Max() - Coordinates.Select(CS => CS.Min(C => C.Y)).Min());

                SaveGraphics("CoordsLength.png", i.ToString(), 500, 500, true, 1, 2, CoordinatesData, (x, y) => (x * scaleX * Constants.RESULT_SCALE_SIMPLE - 1, y * scaleY * Constants.RESULT_SCALE_SIMPLE - 1));
                SaveGraphics($"CoordsLength_{i}.png", "Total/Length", 500, 500, true, 1, 2, CoordinatesData, (x, y) => (x * scaleX * Constants.RESULT_SCALE_SIMPLE - 1, y * scaleY * Constants.RESULT_SCALE_SIMPLE - 1));

                Console.WriteLine("CoordsLength_" + i + " saved");



                //List<List<Point>> CoordinatesSquare = new List<List<Point>>();
                //foreach (List<TimedAmplitude> sound in SplittedSounds)
                //    CoordinatesSquare.Add(WaveAnalyzer.GetSoundCoordinates(TimedAmplitudes, sound));

                //Dictionary<List<Point>, Color> CoordinatesSquareData = new Dictionary<List<Point>, Color>();

                //for (int j = 0; j < CoordinatesSquare.Count; j++)
                //    CoordinatesSquareData.Add(CoordinatesSquare[j], colors[j]);

                //if (CoordinatesSquare.Count == 0)
                //    continue;

                //scaleX = 2 / (CoordinatesSquare.Select(CS => CS.Max(C => C.X)).Max() - CoordinatesSquare.Select(CS => CS.Min(C => C.X)).Min());
                //scaleY = 2 / (CoordinatesSquare.Select(CS => CS.Max(C => C.Y)).Max() - CoordinatesSquare.Select(CS => CS.Min(C => C.Y)).Min());

                //SaveGraphics("CoordsSquare.png", i.ToString(), 500, 500, true, 1, 2, CoordinatesSquareData, (x, y) => (x * scaleX * Constants.RESULT_SCALE_SQUARE, y * scaleY * Constants.RESULT_SCALE_SQUARE));
                //SaveGraphics($"CoordsSquare_{i}.png", "Total/Square", 500, 500, true, 1, 2, CoordinatesSquareData, (x, y) => (x * scaleX * Constants.RESULT_SCALE_SQUARE, y * scaleY * Constants.RESULT_SCALE_SQUARE));

                //Console.WriteLine("CoordsSquare_" + i + " saved");


                Console.WriteLine(i + " processed");

                Console.WriteLine();
                Console.WriteLine("----------------------");
                Console.WriteLine();

                Console.Beep();
            }

            Console.Beep();
            Console.Beep();
            Console.Beep();

            Console.ReadLine();
        }

        private static List<Point> TimedAmplitudeListToPointList(List<TimedAmplitude> TAS)
        {
            List<Point> PS = new List<Point>();
            for (int i = 0; i < TAS.Count; i++)
                PS.Add(TAS[i]);
            return PS;
        }

        private delegate (double x, double y) VertexConversion(double t, double a);
        private static void SaveGraphics(string filename, string dirname, int windowWidth, int windowHeight, bool isPointMode, float lineWidth, float pointSize, 
                                     Dictionary<List<Point>, Color> dataGroups, VertexConversion conversion)
        {
            Bitmap bmp = new Bitmap(windowWidth, windowHeight);

            Graphics gr = Graphics.FromImage(bmp);
            gr.Clear(Color.Black);

            foreach (KeyValuePair<List<Point>, Color> group in dataGroups)
            {
                Brush brush = new SolidBrush(group.Value);
                Pen pen = new Pen(group.Value, lineWidth);

                if (isPointMode)
                    foreach (Point Coordinate in group.Key)
                    {
                        (double x, double y) = conversion(Coordinate.X, Coordinate.Y);

                        float xl = (float)((x + 1) * windowWidth / 2 - pointSize / 2);
                        float yt = (float)((1 - y) * windowHeight / 2 - pointSize / 2);

                        gr.FillEllipse(brush, xl, yt, pointSize, pointSize);
                    }
                else
                    for (int i = 0; i < group.Key.Count - 1; i++)
                    {
                        (double x1, double y1) = conversion(group.Key[i].X, group.Key[i].Y);
                        (double x2, double y2) = conversion(group.Key[i + 1].X, group.Key[i + 1].Y);

                        gr.DrawLine(pen, (float)((x1 + 1) * windowWidth / 2), (float)((1 - y1) * windowHeight / 2),
                                         (float)((x2 + 1) * windowWidth / 2), (float)((1 - y2) * windowHeight / 2));
                    }
            }

            if (!Directory.Exists(Environment.CurrentDirectory + "/" + dirname))
                Directory.CreateDirectory(Environment.CurrentDirectory + "/" + dirname);

            bmp.Save(Environment.CurrentDirectory + "/" + dirname + "/" + filename);
        }

        private static Random R = new Random();
        private static Color RandColor()
        {
            return Color.FromArgb(R.Next(0, 256), R.Next(0, 256), R.Next(0, 256));
        }
    }
}


//GlWindow NoConversionWindow = new GlWindow(500, 500, 0, 0, "No conversion", () => { }, () =>
//    {
//        Gl.glBegin(Gl.GL_LINE_STRIP);

//        Gl.glColor3f(1, 0, 0);

//        foreach (KeyValuePair<double, double> TimedAmplitude in TimedAmplitudes)
//            Gl.glVertex2d(TimedAmplitude.Key * 2 - 1, TimedAmplitude.Value / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5);

//        Gl.glEnd();
//    },
//(x, y) => { }, 30);
//NoConversionWindow.Start();

//GlWindow ExtremumsWindow = new GlWindow(500, 500, 500, 0, "Extremums", () => { }, () =>
//{
//    Gl.glBegin(Gl.GL_LINE_STRIP);

//    Gl.glColor3f(1, 0, 0);

//    foreach (KeyValuePair<double, double> TimedExtremum in TimedExtremums)
//        Gl.glVertex2d(TimedExtremum.Key * 2 - 1, TimedExtremum.Value / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5);

//    Gl.glEnd();
//},
//(x, y) => { }, 30);
//ExtremumsWindow.Start();

//GlWindow NoNoiseWindow = new GlWindow(500, 500, 1000, 0, "No Noise", () => { }, () =>
//{
//    Gl.glBegin(Gl.GL_LINE_STRIP);

//    Gl.glColor3f(1, 0, 0);

//    foreach (KeyValuePair<double, double> TimedExtremum in NoNoiseTimedExtremums)
//        Gl.glVertex2d(TimedExtremum.Key * 2 - 1, TimedExtremum.Value / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5);

//    Gl.glEnd();
//},
//(x, y) => { }, 30);
//NoNoiseWindow.Start();

//GlWindow SplittedSoundsWindow = new GlWindow(500, 500, 1000, 0, "Splitted Sounds", () => { }, () =>
//{
//    foreach (Dictionary<double, double> sound in SplittedSounds)
//    {
//        Gl.glBegin(Gl.GL_LINE_STRIP);

//        double min = sound.Min(S => S.Value);
//        double max = sound.Max(S => S.Value);

//        Gl.glColor3d((min + max) / 128, (max - min) / 128, 0);

//        foreach (KeyValuePair<double, double> wave in sound)
//            Gl.glVertex2d(wave.Key * 2 - 1, wave.Value / Math.Pow(2, (file.BitsPerSample <= 16 ? file.BitsPerSample : 0)) - 0.5);

//        Gl.glEnd();
//    }
//},
//(x, y) => { }, 30);
//SplittedSoundsWindow.Start();

//GlWindow SoundCoordssWindow = new GlWindow(500, 500, 1000, 0, "Coordinates", () => { }, () =>
//{
//    Gl.glPointSize(5);
//    foreach (Dictionary<double, double> coords in Coordinates)
//    {
//        Gl.glBegin(Gl.GL_POINTS);

//        Gl.glColor3d(colors[c].R / 255.0, colors[c].G / 255.0, colors[c].B / 255.0);
//        c = (c + 1) % Coordinates.Count;

//        foreach (KeyValuePair<double, double> coord in coords)
//            Gl.glVertex2d(coord.Key * scaleX * 10 - 1, coord.Value * scaleY * 10 - 1);

//        Gl.glEnd();
//    }
//},
//(x, y) => { }, 30);
//SoundCoordssWindow.Start();