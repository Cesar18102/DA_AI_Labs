using System;
using System.Drawing;

namespace ShapesFinder.Util
{
    public class ColorRandom
    {
        private Random R = new Random();
        public Color NextColor() => Color.FromArgb(R.Next(256), R.Next(256), R.Next(256));
    }
}
