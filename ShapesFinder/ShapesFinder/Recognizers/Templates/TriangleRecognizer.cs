using System;
using System.Linq;
using System.Collections.Generic;

using ShapesFinder.Recognizers.Util;
using ShapesFinder.Recognizers.Shapes;

namespace ShapesFinder.Recognizers.Templates
{
    public class TriangleRecognizer : ShapeRecognizer
    {
        private const double MAX_DEVIATION = 0.05;
        private const int PIN_MAX = 10;

        public override IShape RecognizeShape(IRawData shape)
        {
            double triangleScope = shape.Scope();
            double wrapperScope = shape.Width * shape.Height;

            Triangle result = new Triangle(shape.X, shape.Y, shape.Width, shape.Height, triangleScope);

            if (Math.Abs(0.5 - triangleScope / wrapperScope) < MAX_DEVIATION)
                return result;

            List<List<(int x, int y)>> ways = GetAllCountures(shape);

            bool isRectTriangle = ways.Count == 1 && Math.Abs(1 - (double)ways.First().Count / (shape.Width + shape.Height)) <= MAX_DEVIATION;
            bool twoSidesTouchTriangle = ways.Count == 2 && (ways.First().Count <= PIN_MAX || ways.Last().Count <= PIN_MAX);
            bool threePinsTriangle = ways.Count == 3 && ways.All(W => W.Count <= PIN_MAX);

            return isRectTriangle || twoSidesTouchTriangle || threePinsTriangle ? result : null;
        }
    }
}
