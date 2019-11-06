namespace ShapesFinder.Recognizers
{
    public interface IShapeRecognizer
    {
        IShape RecognizeShape(IRawData shape);
    }
}
