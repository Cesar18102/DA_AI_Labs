namespace ShapesFinder.Recognizers
{
    public interface IShape
    {
        int X { get; }
        int Y { get; }

        int Width { get; }
        int Height { get; }

        double Scope { get; }
    }
}
