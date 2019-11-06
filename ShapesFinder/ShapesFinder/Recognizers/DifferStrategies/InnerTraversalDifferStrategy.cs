using System.Linq;
using System.Collections.Generic;
using System;

namespace ShapesFinder.Recognizers.DifferStrategies
{
    public class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().Equals(typeof(Point)))
                return false;

            Point P = (obj as Point);
            return P.X == X && P.Y == Y;
        }
    }

    public class InnerTraversalDifferStrategy : IDifferStrategy
    {
        private static readonly Point INVALID_POINT = new Point(-1, -1);

        public IEnumerable<IRawData> DifferShape(IRawData data)
        {
            List<IRawData> rawShapes = new List<IRawData>();
            IRawData currentData = data;

            data.Save(Environment.CurrentDirectory + "/Output/source");

            while (true)
            {
                Point firstPoint = FindFirstShape(currentData);

                if (firstPoint.Equals(INVALID_POINT))
                    break;

                IEnumerable<(int x, int y)> points = InnerBFSTraversalInit(firstPoint, data).Select(P => (P.X, P.Y));

                //int left = points.Min(P => P.x);
                //int top = points.Min(P => P.y);

                //int right = points.Max(P => P.x);
                //int bot = points.Max(P => P.y);

                rawShapes.Add(currentData.GetCropped(points));
                rawShapes.Last().Save(Environment.CurrentDirectory + $"/Output/{firstPoint.X}_{firstPoint.Y}");

                currentData.Erase(points);
            }

            return rawShapes;
        }

        private Point FindFirstShape(IRawData data)
        {
            for (int i = 0; i < data.Height; i++)
                for (int j = 0; j < data.Width; j++)
                    if (!data.IsEmpty(j, i))
                        return new Point(j, i);
            return new Point(-1, -1);
        }

        private List<Point> InnerBFSTraversalInit(Point startPoint, IRawData data)
        {
            List<Point> result = new List<Point>();
            Queue<Point> toVisit = new Queue<Point>();
            Point currentPoint = startPoint;

            while(true)
            {
                result.Add(currentPoint);

                foreach (Point neighbourPoint in GetNotEmptyNotVisitedNeighbours(currentPoint, data, result, toVisit))
                    toVisit.Enqueue(neighbourPoint);

                if (toVisit.Count == 0)
                    break;

                currentPoint = toVisit.Dequeue();
            }

            return result;
        }

        private List<Point> GetNotEmptyNotVisitedNeighbours(Point point, IRawData data, List<Point> visited, Queue<Point> toVisit)
        {
            List<Point> notEmptyNotVisitedNeighbours = new List<Point>();

            for (int i = -1; i <= 1; i++)
                for(int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    Point pointShifted = new Point(point.X + j, point.Y + i);

                    if (pointShifted.X < 0 || pointShifted.X >= data.Width || pointShifted.Y < 0 || pointShifted.Y >= data.Height)
                        continue;

                    if (data.IsEmpty(pointShifted.X, pointShifted.Y) || visited.Contains(pointShifted) || toVisit.Contains(pointShifted))
                    {
                        bool x = data.IsEmpty(pointShifted.X, pointShifted.Y);
                        continue;
                    }

                    

                    notEmptyNotVisitedNeighbours.Add(pointShifted);
                }

            return notEmptyNotVisitedNeighbours;
        }
    }
}
