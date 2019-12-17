using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxDraw
{
    class GlassSize : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // same as: if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public const double GRINDING = 0.2;

        double _height { get; set; }
        double _length { get; set; }
        double _width { get; set; }

        

        private string CalcTopSize(double length, double width)
        {
            string topSize = null;

            _length = length + GRINDING;
            _width = width + GRINDING;

            return topSize = $"{_length} x {_width} cm";
        }

        private string CalcFrontSize(double length, double height, double thickness)
        {
            string frontSize = null;

            _length = length + GRINDING;
            _height = (height - thickness) + GRINDING;

            return frontSize = $"{_length} x {_height} cm";
        }

        private string CalcSideSize(double width, double height, double thickness)
        {
            string sideSize = null;

            _width = (width - thickness * 2) + GRINDING;
            _height = (height - thickness) + GRINDING;

            return sideSize = $"{_width} x {_height} cm";
        }

        public string GetGlassSize(double length, double height, double width, double thickness)
        {
            string _glassSizeString = null;

            string topSize = CalcTopSize(length, width);
            string frontSize = CalcFrontSize(length, height, thickness);
            string sideSize = CalcSideSize(width, height, thickness);

            _glassSizeString = string.Concat(topSize, "\n", frontSize, " x 2\n", sideSize, " x 2");

            return _glassSizeString;
        }
    }
}
