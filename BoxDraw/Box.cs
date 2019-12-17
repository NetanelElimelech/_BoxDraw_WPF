using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxDraw
{
    public class Box : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // same as: if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public const double DOWNSIZE = 0.95; //This assures that the box will still be smaller than the canvas (after proportional fitting)

        public double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public double _length;
        public double Length
        {
            get { return _length; }
            set
            {
                _length = value;
                OnPropertyChanged(nameof(Length));
            }
        }

        public double _width;
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

        public double _thickness;
        public double Thickness
        {
            get { return _thickness; }
            set
            {
                _thickness = value;
                OnPropertyChanged(nameof(Thickness));
            }
        }

        public string _glassSizeString;
        public string GlassSizeString
        {
            get { return _glassSizeString; }
            set
            {
                _glassSizeString = value;
                OnPropertyChanged(nameof(GlassSizeString));
            }
        }

        public double _widthShift { get; set; }

        public void SwapLengthWidth()
        {
            if (_length < _width)
            {
                double tmp = _length;
                _length = _width;
                _width = tmp;
                Width = _width;
                Length = _length;
            }
        }

        public double CalcTotalWidth(double width, double widthShift)
        {
            return width + widthShift;
        }

        public double CalcTotalHeight(double height, double widthShift)
        {
            return height + widthShift;
        }

        public double CalcWidthShift(double length)
        {
            double diagonal = 0.6 * length; //Resize the diagonal lines ("width") for perspective compensation
            return Math.Sqrt((diagonal * diagonal) / 2); //Pythagorean theorem
        }

        public double CalcResizeFactor(double canvasHeight, double canvasWidth, double totalHeight, double totalLength)
        {
            double factor = 0;

            double factor1 = canvasHeight / totalHeight;
            double factor2 = canvasWidth / totalLength;

            if (factor1 <= factor2)
            {
                factor = factor1 * DOWNSIZE;
            }
            
            else
            {
                factor = factor2 * DOWNSIZE;
            }

            return factor;
        }

        public int ResizeHeightForDraw(double factor, double height)
        {
            double heightResized = height * factor;

            return (int)heightResized;
        }

        public int ResizeLengthForDraw(double factor, double length)
        {
            double lengthResized = length * factor;

            return (int)lengthResized;
        }

        public int ResizeWidthShiftForDraw(double factor, double width)
        {
            double widthResized = width * factor;

            double widthShiftResized = CalcWidthShift(widthResized);

            return (int)widthShiftResized;
        }
    }
}