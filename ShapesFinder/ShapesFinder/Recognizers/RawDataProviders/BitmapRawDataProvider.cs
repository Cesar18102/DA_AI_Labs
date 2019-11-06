using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace ShapesFinder.Recognizers.RawDataProviders
{
    public class BitmapRawDataProvider : IRawData
    {
        public const string SAVE_FILE_FORMAT = ".jpg";

        public static readonly Color EMPTY_COLOR_BORDER = Color.FromArgb(251, 251, 251);
        public static readonly Color EMPTY_COLOR = Color.FromArgb(255, 255, 255);

        public static readonly Brush EMPTY_COLOR_BRUSH = new SolidBrush(EMPTY_COLOR);
        public static readonly Pen EMPTY_COLOR_PEN = new Pen(EMPTY_COLOR);
        public static readonly Pen MEANING_COLOR_PEN = new Pen(Color.Black);

        public Bitmap BMP { get; private set; }
        public LogicalRawDataProvider Map { get; private set; }

        public BitmapRawDataProvider(Bitmap bmp, int x = 0, int y = 0)
        {
            BMP = bmp;

            X = x;
            Y = y;

            bool[,] map = new bool[bmp.Height, bmp.Width];
            for (int i = 0; i < bmp.Height; i++)
                for (int j = 0; j < bmp.Width; j++)
                    map[i, j] = !MapIsEmpty(j, i);

            Map = new LogicalRawDataProvider(bmp.Width, bmp.Height, x, y, map);
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public int Width { get => BMP == null ? -1 : BMP.Width; }
        public int Height { get => BMP == null ? -1 : BMP.Height; }

        private bool MapIsEmpty(int x, int y)
        {
            Color pixel = BMP.GetPixel(x, y);
            return pixel.R >= EMPTY_COLOR_BORDER.R && pixel.G >= EMPTY_COLOR_BORDER.G && pixel.B >= EMPTY_COLOR_BORDER.B;
        }

        public bool IsEmpty(int x, int y) => Map.IsEmpty(y, x);

        public void Erase(IEnumerable<(int x, int y)> pts)
        {
            using (Graphics canvas = Graphics.FromImage(BMP))
            {
                foreach ((int x, int y) pnt in pts)
                    canvas.DrawRectangle(EMPTY_COLOR_PEN, pnt.x, pnt.y, 1, 1);

                canvas.Save();
            }
        }

        public IRawData GetCropped(IEnumerable<(int x, int y)> pts)
        {
            int left = pts.Min(P => P.x);
            int top = pts.Min(P => P.y);

            int right = pts.Max(P => P.x);
            int bot = pts.Max(P => P.y);

            int w = right - left + 1, h = bot - top + 1;
            Bitmap crop = new Bitmap(w, h);

            using (Graphics canvas = Graphics.FromImage(crop))
            {
                foreach ((int x, int y) pnt in pts)
                    canvas.DrawRectangle(MEANING_COLOR_PEN, pnt.x, pnt.y, 1, 1);
                canvas.Save();
            }

            return new BitmapRawDataProvider(crop);
        }

        public void Dispose() => BMP.Dispose();

        public void Save(string filename) => BMP.Save(filename + SAVE_FILE_FORMAT);
    }
}
