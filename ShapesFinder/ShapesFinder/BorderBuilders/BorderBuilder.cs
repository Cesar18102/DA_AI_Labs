using System.Drawing;

using ShapesFinder.Util;

namespace ShapesFinder.BorderBuilders
{
    public abstract class BorderBuilder : IBorderBuilder
    {
        protected Pen pen { get; set; }
        protected Brush signBrush { get; private set; }

        protected static readonly Font DEFAULT_SIGN_FONT = new Font("Arial", 10, FontStyle.Bold);
        protected static readonly ColorRandom R = new ColorRandom();

        public BorderBuilder(float width, Color color, bool randColor = false)
        {
            if (randColor) color = R.NextColor();
            pen = new Pen(color, width);
            signBrush = new SolidBrush(color);
        }

        public abstract void Draw(Graphics canvas, int ltx, int lty, int width, int height, string sign = "");
    }
}
