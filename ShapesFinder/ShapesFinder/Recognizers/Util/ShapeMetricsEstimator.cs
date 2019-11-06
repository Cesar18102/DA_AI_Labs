namespace ShapesFinder.Recognizers.Util
{
    public static class ShapeMetricsEstimator
    {
        public static int Perimeter(this IRawData rawShape)
        {
            int count = 0;
            for (int i = 0; i < rawShape.Height; i++)
                for (int j = 0; j < rawShape.Width; j++)
                    if (!rawShape.IsEmpty(j, i) && (j == 0 || i == 0 || j == rawShape.Width - 1 || i == rawShape.Height - 1 ||
                        rawShape.IsEmpty(j - 1, i) || rawShape.IsEmpty(j + 1, i) || rawShape.IsEmpty(j, i - 1) || rawShape.IsEmpty(j, i + 1)))
                        count++;
            return count;
        }

        public static int Scope(this IRawData rawShape)
        {
            int count = 0;
            for (int i = 0; i < rawShape.Height; i++)
                for (int j = 0; j < rawShape.Width; j++)
                    if (!rawShape.IsEmpty(j, i))
                        count++;
            return count;
        }
    }
}
