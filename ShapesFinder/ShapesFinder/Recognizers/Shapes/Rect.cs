namespace ShapesFinder.Recognizers.Shapes
{
    public class Rect : IShape
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public double Scope { get; private set; }

        public Rect(int x, int y, int width, int height, double scope)
        {
            X = x;
            Y = y;

            Width = width;
            Height = height;

            Scope = scope;
        }

        public override string ToString() => "Rectangle";
    }
}
