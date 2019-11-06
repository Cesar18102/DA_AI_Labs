using System;

namespace ShapesFinder.Recognizers.Shapes
{
    public class Circle : IShape
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int R { get; private set; }

        public int Width => 2 * R;
        public int Height => 2 * R;

        public double Scope => Math.PI * R * R;

        public Circle(int x, int y, int r)
        {
            X = x;
            Y = y;

            R = r;
        }

        public override string ToString() => "Circle";
    }
}
