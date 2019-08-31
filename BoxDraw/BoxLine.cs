using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace BoxDraw
{
    class BoxLine : IDrawLine
    {
        private int _zeroX_Line { get; set; }
        private int _zeroY_Line { get; set; }
        private int _length { get; set; }

        private Line line = null;

        void IDrawLine.SetZero(int xCoord, int yCoord)
        {
            _zeroX_Line = xCoord;
            _zeroY_Line = yCoord;
        }

        public void DrawLine(Canvas canvas, int top, int left, int Y2, int X2)
        {
            if (line != null)
            {
                canvas.Children.Remove(line);
            }

            line = new Line();
            Canvas.SetTop(line, top);
            Canvas.SetLeft(line, left);
            line.Y2 = Y2;
            line.X2 = X2;
            canvas.Children.Add(line);
        }
    }
}
