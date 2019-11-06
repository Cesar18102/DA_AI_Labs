using System.Collections.Generic;

namespace ShapesFinder.Recognizers.Templates
{
    public abstract class ShapeRecognizer : IShapeRecognizer
    {
        private const int SPACE_ACCEPTABLE = 3;

        public abstract IShape RecognizeShape(IRawData shape);

        protected List<List<(int x, int y)>> GetAllCountures(IRawData data)
        {
            bool[,] visited = new bool[data.Height, data.Width];
            (int x, int y) startPoint = FindNext(visited, data);
            List<List<(int x, int y)>> ways = new List<List<(int x, int y)>>();

            while (!startPoint.Equals((-1, -1)))
            {
                List<(int x, int y)> way = new List<(int x, int y)>();
                ProcessPerimeter(startPoint.x, startPoint.y, data, visited, way, new Queue<(int x, int y)>());
                way.RemoveAll(P => P.x != 0 && P.x != data.Width - 1 && P.y != 0 && P.y != data.Height - 1);
                ways.Add(way);

                startPoint = FindNext(visited, data);
            }

            return ways;
        }

        protected (int x, int y) FindNext(bool[,] visited, IRawData data)
        {
            for (int i = 0; i < data.Height; i++)
                if (!visited[i, 0] && !data.IsEmpty(0, i))
                    return (0, i);
                else if (!visited[i, data.Width - 1] && !data.IsEmpty(data.Width - 1, i))
                    return (data.Width - 1, i);

            for (int i = 0; i < data.Width; i++)
                if (!visited[0, i] && !data.IsEmpty(i, 0))
                    return (i, 0);
                else if (!visited[data.Height - 1, i] && !data.IsEmpty(i, data.Height - 1))
                    return (i, data.Height - 1);

            return (-1, -1);
        }

        protected void ProcessPerimeter(int x, int y, IRawData data, bool[,] visited, List<(int x, int y)> way, Queue<(int x, int y)> toVisit)
        {
            way.Add((x, y));
            visited[y, x] = true;

            if (x <= SPACE_ACCEPTABLE || x >= data.Width - SPACE_ACCEPTABLE - 1)
                for (int i = -SPACE_ACCEPTABLE; i <= SPACE_ACCEPTABLE; i++)
                    if (y + i >= 0 && y + i < data.Height && !data.IsEmpty(x, y + i) && !visited[y + i, x] && !toVisit.Contains((x, y + i)))
                        toVisit.Enqueue((x, y + i));

            if (y <= SPACE_ACCEPTABLE || y >= data.Height - SPACE_ACCEPTABLE - 1)
                for (int i = -SPACE_ACCEPTABLE; i <= SPACE_ACCEPTABLE; i++)
                    if (x + i >= 0 && x + i < data.Width && !data.IsEmpty(x + i, y) && !visited[y, x + i] && !toVisit.Contains((x + i, y)))
                        toVisit.Enqueue((x + i, y));

            if (toVisit.Count == 0)
                return;

            (int x, int y) point = toVisit.Dequeue();
            ProcessPerimeter(point.x, point.y, data, visited, way, toVisit);
        }
    }
}
