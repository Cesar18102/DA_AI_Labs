using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace ShapesFinder.Recognizers.RawDataProviders
{
    public class LogicalRawDataProvider : IRawData
    {
        public const string SAVE_FILE_FORMAT = ".txt";

        private const string EMPTY_SYMBOL = "0";
        private const string NOT_EMPTY_SYMBOL = "1";

        private bool[,] Map { get; set; }

        public LogicalRawDataProvider(int width, int height, int x = 0, int y = 0, bool[,] data = null)
        {
            Map = data == null ? new bool[height, width] : data;

            Width = width;
            Height = height;

            X = x;
            Y = y;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }

        public IRawData GetCropped(IEnumerable<(int x, int y)> pts)
        {
            int left = pts.Min(P => P.x);
            int top = pts.Min(P => P.y);

            int right = pts.Max(P => P.x);
            int bot = pts.Max(P => P.y);

            int w = right - left + 1, h = bot - top + 1;
            bool[,] crop = new bool[h, w];

            foreach((int x, int y) pnt in pts)
                crop[pnt.y - top, pnt.x - left] = Map[pnt.y, pnt.x];

            return new LogicalRawDataProvider(w, h, left, top, crop);
        }

        public void Erase(IEnumerable<(int x, int y)> pts)
        {
            foreach ((int x, int y) pnt in pts)
                Map[pnt.y, pnt.x] = false;
        }

        public bool IsEmpty(int x, int y) => !Map[y, x];

        public void Save(string filename)
        {
            using(StreamWriter str = File.CreateText(filename + SAVE_FILE_FORMAT))
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                        str.Write(Map[i, j] ? NOT_EMPTY_SYMBOL : EMPTY_SYMBOL);
                    str.WriteLine();
                }
        }

        public void Dispose() { }
    }
}
