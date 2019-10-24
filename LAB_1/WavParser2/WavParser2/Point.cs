using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WavParser2
{
    public class Point
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType().Equals(typeof(Point)))
                return false;

            Point P = obj as Point;
            return P.X == X && P.Y == Y;
        }
    }
}
