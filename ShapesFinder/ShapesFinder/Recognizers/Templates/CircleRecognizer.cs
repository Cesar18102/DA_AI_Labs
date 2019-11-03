using System.Collections.Generic;

namespace ShapesFinder.Recognizers.Templates
{
    public class CircleRecognizer : IRecognizer
    {
        public IEnumerable<IShape> RecognizeAll(IEnumerable<IRawData> rawShapes)
        {
            return new List<IShape>();
        }
    }
}
