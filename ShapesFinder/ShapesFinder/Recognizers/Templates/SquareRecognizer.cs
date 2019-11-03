using System.Collections.Generic;

namespace ShapesFinder.Recognizers.Templates
{
    public class SquareRecognizer : IRecognizer
    {
        public IEnumerable<IShape> RecognizeAll(IEnumerable<IRawData> rawShapes)
        {
            return new List<IShape>();
        }
    }
}
