using System.Windows.Controls;

namespace BoxDraw
{
    interface IDrawLine
    {
        void SetZero(int xCoord, int yCoord);
        void DrawLine(Canvas canvas, int top, int left, int Y2, int X2);
    }
}
