using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxDraw
{
    public class CornerPoint
    {
        public enum LineType { hor, ver, dia}

        public double X { get; set; }
        public double Y { get; set; }
        public int zeroX { get; set; }
        public int zeroY { get; set; }

        public CornerPoint(double xValue1, double xValue2, double yValue1, double yValue2)
        {
            X = CalcX(xValue1, xValue2);
            Y = CalcY(yValue1, yValue2);
        }

        double CalcX(double value1, double value2)
        {
            if (value1 != value2)
            {
                if (value1 >= value2)
                {
                    X = value1 - value2;
                }

                else
                {
                    X = value2 - value1;
                }
            }

            return X;
        }

        double CalcY(double value1, double value2)
        {
            if (value1 != value2)
            {
                if (value1 >= value2)
                {
                    Y = value1 - value2;
                }

                else
                {
                    Y = value2 - value1;
                }
            }

            return Y;
        }
    }
}
