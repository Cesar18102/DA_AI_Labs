using System;
using System.Linq;
using System.Collections.Generic;

using ShapesFinder.Recognizers.Util;
using ShapesFinder.Recognizers.Shapes;

namespace ShapesFinder.Recognizers.Templates
{
    public class RectRecognizer : ShapeRecognizer
    {
        private const double MAX_DEVIATION = 0.01;
        private const int PIN_MAX = 10;

        public override IShape RecognizeShape(IRawData shape)
        {
            double shapeScope = shape.Scope();
            Rect result = new Rect(shape.X, shape.Y, shape.Width, shape.Height, shapeScope);

            double wrapperScope = shape.Width * shape.Height;

            if (Math.Abs(1 - shapeScope / wrapperScope) <= MAX_DEVIATION)
                return result;

            List<List<(int x, int y)>> ways = GetAllCountures(shape);

            if (ways.Count == 4 && ways.All(W => W.Count < PIN_MAX))
                return result;

            return null;
        }
    }
}
