using System.Drawing;

namespace ShapesFinder.BorderBuilders
{
    public class OvalBorderBuilder : BorderBuilder
    {
        public OvalBorderBuilder(float width, Color color, bool randColor = false) : base(width, color, randColor) { }

        public override void Draw(Graphics canvas, int ltx, int lty, int width, int height, string sign = "")
        {
            canvas.DrawEllipse(pen, ltx - pen.Width / 2, lty - pen.Width / 2, width + pen.Width, height + pen.Width);
            canvas.DrawString(sign, DEFAULT_SIGN_FONT, signBrush, ltx, lty - pen.Width - DEFAULT_SIGN_FONT.Size * (sign.Split('\n').Length + 1));

            canvas.Save();
        }
    }
}
