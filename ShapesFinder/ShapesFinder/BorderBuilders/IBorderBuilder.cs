using System.Drawing;

namespace ShapesFinder.BorderBuilders
{
    public interface IBorderBuilder
    {
        void Draw(Graphics canvas, int ltx, int lty, int width, int height, string sign = "");
    }
}
