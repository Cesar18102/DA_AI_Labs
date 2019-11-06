using System;

using ShapesFinder.Recognizers.Util;
using ShapesFinder.Recognizers.Shapes;

namespace ShapesFinder.Recognizers.Templates
{
    public class CircleRecognizer : ShapeRecognizer
    {
        private const double MAX_DEVIATION = 0.03;

        public override IShape RecognizeShape(IRawData shape) =>
            Math.Abs(1 - (Math.PI / 4.0) / ((double)shape.Scope() / (shape.Width * shape.Height))) <= MAX_DEVIATION ? 
                new Circle(shape.X, shape.Y, shape.Width / 2) : null;
    }
}
