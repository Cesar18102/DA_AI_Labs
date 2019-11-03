using System.Collections.Generic;

namespace ShapesFinder.Recognizers.Templates
{
    public class TriangleRecognizer : IRecognizer
    {
        public IEnumerable<IShape> RecognizeAll(IEnumerable<IRawData> rawShapes)
        {
            return new List<IShape>();
        }
    }
}
