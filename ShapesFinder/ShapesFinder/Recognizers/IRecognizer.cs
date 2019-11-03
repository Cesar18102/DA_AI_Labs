using System.Collections.Generic;

namespace ShapesFinder.Recognizers
{
    public interface IRecognizer
    {
        IEnumerable<IShape> RecognizeAll(IEnumerable<IRawData> rawShapes);
    }
}
