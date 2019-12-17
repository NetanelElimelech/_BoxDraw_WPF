using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BoxDraw
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private Box box = new Box();
        private GlassSize glassSize = new GlassSize();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = box;
        }

        private void RegularLine(int length)
        {
            BoxLine boxLine = new BoxLine();

            if (boxLine is IDrawLine)
            {
                IDrawLine drawLine = boxLine;
                drawLine.SetZero((int)DrawCanvas.Width, (int)DrawCanvas.Height);
            }
        }

        public void DrawBox_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();

            //If width is greater than width, swap them
            box.SwapLengthWidth();

            //Calc diagonal shift for drawing the top and the both sides
            box._widthShift = box.CalcWidthShift(box._width);

            // Calc the total size of the box, incl. the top and the side
            double totalHeight = box.CalcTotalHeight(box._height, box._widthShift);
            double totalWidth = box.CalcTotalHeight(box._length, box._widthShift);

            //Resize the box to fit it into the canvas
            double resizeFactor = box.CalcResizeFactor(DrawCanvas.Height, DrawCanvas.Width, totalHeight, totalWidth);
            double heightResized = box.ResizeHeightForDraw(resizeFactor, box._height);
            double widthResized = box.ResizeLengthForDraw(resizeFactor, box._length);
            double widthShiftResized = box.ResizeWidthShiftForDraw(resizeFactor, box._width);

            //Calc corner points (first - front and back, then - the sides: shifted)
            Dictionary<string, CornerPoint> CornerPoints = CalcCornerPoints(heightResized, widthResized, widthShiftResized, DrawCanvas.Height);
            Dictionary<string, CornerPoint> ShiftedCornerPoints = ShiftCornerPoints(heightResized, widthResized, widthShiftResized, DrawCanvas.Height, DrawCanvas.Width, CornerPoints);

            //Calc and draw the outer contour of the box
            Dictionary<string, Line> BoxLines = CalcLines(ShiftedCornerPoints);
            DrawLines(DashLines(BoxLines));

            //Calc the sizes of the glass pieces for cutting
            box.GlassSizeString = glassSize.GetGlassSize(box._length, box._height, box._width, box._thickness);
        }

        public Dictionary<string, Line> CalcLines(Dictionary<string, CornerPoint> CornerPoints)
        {
            Dictionary<string, Line> Lines = new Dictionary<string, Line>();

            if (CornerPoints.ContainsKey("A")
                && CornerPoints.ContainsKey("B")
                && CornerPoints.ContainsKey("C")
                && CornerPoints.ContainsKey("D")
                && CornerPoints.ContainsKey("E")
                && CornerPoints.ContainsKey("F")
                && CornerPoints.ContainsKey("G")
                && CornerPoints.ContainsKey("H"))
            {
                Line front_hor_lineAB = new Line();
                front_hor_lineAB.X1 = CornerPoints["A"].X;            //              G___________________H
                front_hor_lineAB.Y1 = CornerPoints["A"].Y;            //              /|                 /|
                front_hor_lineAB.X2 = CornerPoints["B"].X;            //             / |                / |
                front_hor_lineAB.Y2 = CornerPoints["B"].Y;            //            /  |               /  |
                                                                      //          C/___|______________/D  |
                Line front_hor_lineCD = new Line();                   //           |   |              |   |
                front_hor_lineCD.X1 = CornerPoints["C"].X;            //           |   |              |   |
                front_hor_lineCD.Y1 = CornerPoints["C"].Y;            //           |   |              |   |
                front_hor_lineCD.X2 = CornerPoints["D"].X;            //           |   |              |   |
                front_hor_lineCD.Y2 = CornerPoints["D"].Y;            //           |   |              |   |
                                                                      //           |   |E_ _ _ _ _ _ _|_ _|F
                Line front_ver_lineAC = new Line();                   //           |   /              |   /
                front_ver_lineAC.X1 = CornerPoints["A"].X;            //           |  /               |  /
                front_ver_lineAC.Y1 = CornerPoints["A"].Y;            //           | /                | /
                front_ver_lineAC.X2 = CornerPoints["C"].X;            //           |/_________________|/
                front_ver_lineAC.Y2 = CornerPoints["C"].Y;            //           A                  B
                                                                      //
                Line front_ver_lineBD = new Line();                   //
                front_ver_lineBD.X1 = CornerPoints["B"].X;
                front_ver_lineBD.Y1 = CornerPoints["B"].Y;
                front_ver_lineBD.X2 = CornerPoints["D"].X;
                front_ver_lineBD.Y2 = CornerPoints["D"].Y;

                Line back_hor_lineEF = new Line();
                back_hor_lineEF.X1 = CornerPoints["E"].X;
                back_hor_lineEF.Y1 = CornerPoints["E"].Y;
                back_hor_lineEF.X2 = CornerPoints["F"].X;
                back_hor_lineEF.Y2 = CornerPoints["F"].Y;

                Line back_hor_lineGH = new Line();
                back_hor_lineGH.X1 = CornerPoints["G"].X;
                back_hor_lineGH.Y1 = CornerPoints["G"].Y;
                back_hor_lineGH.X2 = CornerPoints["H"].X;
                back_hor_lineGH.Y2 = CornerPoints["H"].Y;

                Line back_ver_lineEG = new Line();
                back_ver_lineEG.X1 = CornerPoints["E"].X;
                back_ver_lineEG.Y1 = CornerPoints["E"].Y;
                back_ver_lineEG.X2 = CornerPoints["G"].X;
                back_ver_lineEG.Y2 = CornerPoints["G"].Y;

                Line back_ver_lineFH = new Line();
                back_ver_lineFH.X1 = CornerPoints["F"].X;
                back_ver_lineFH.Y1 = CornerPoints["F"].Y;
                back_ver_lineFH.X2 = CornerPoints["H"].X;
                back_ver_lineFH.Y2 = CornerPoints["H"].Y;

                Line dia_lineAE = new Line();
                dia_lineAE.X1 = CornerPoints["A"].X;
                dia_lineAE.Y1 = CornerPoints["A"].Y;
                dia_lineAE.X2 = CornerPoints["E"].X;
                dia_lineAE.Y2 = CornerPoints["E"].Y;

                Line dia_lineBF = new Line();
                dia_lineBF.X1 = CornerPoints["B"].X;
                dia_lineBF.Y1 = CornerPoints["B"].Y;
                dia_lineBF.X2 = CornerPoints["F"].X;
                dia_lineBF.Y2 = CornerPoints["F"].Y;

                Line dia_lineCG = new Line();
                dia_lineCG.X1 = CornerPoints["C"].X;
                dia_lineCG.Y1 = CornerPoints["C"].Y;
                dia_lineCG.X2 = CornerPoints["G"].X;
                dia_lineCG.Y2 = CornerPoints["G"].Y;

                Line dia_lineDH = new Line();
                dia_lineDH.X1 = CornerPoints["D"].X;
                dia_lineDH.Y1 = CornerPoints["D"].Y;
                dia_lineDH.X2 = CornerPoints["H"].X;
                dia_lineDH.Y2 = CornerPoints["H"].Y;

                Lines = new Dictionary<string, Line>(){
                { "front_AB", front_hor_lineAB },
                { "front_CD", front_hor_lineCD },
                { "front_AC", front_ver_lineAC },
                { "front_BD", front_ver_lineBD },
                { "back_EF_dashed", back_hor_lineEF },
                { "back_GH", back_hor_lineGH },
                { "back_EG_dashed", back_ver_lineEG },
                { "back_FH", back_ver_lineFH },
                { "dia_AE_dashed", dia_lineAE },
                { "dia_BF", dia_lineBF },
                { "dia_CG", dia_lineCG },
                { "dia_DH", dia_lineDH } };
            }
            return Lines;
        }

        public void ClearCanvas()
        {
            DrawCanvas.Children.Clear();
        }

        public void DrawLines(Dictionary<string, Line> BoxLines)
        {
            SolidColorBrush black = new SolidColorBrush(Colors.Black);

            foreach (KeyValuePair<string, Line> pair in BoxLines)
            {
                if (pair.Value is Line)
                {
                    pair.Value.Stroke = black;
                    DrawCanvas.Children.Add(pair.Value);
                }
            }
        }

        public Dictionary<string, Line> DashLines(Dictionary<string, Line> BoxLines)
        {
            foreach (KeyValuePair<string, Line> pair in BoxLines)
            {
                if (pair.Key.Contains("dashed"))
                {
                    pair.Value.StrokeDashArray = new DoubleCollection() { 3, 3 };
                }

                else
                {
                    pair.Value.StrokeThickness = 3;
                }
            }

            return BoxLines;
        }

        private Dictionary<string, CornerPoint> CalcCornerPoints(double height, double length, double widthShift, double canvasHeight)
        {
            CornerPoint pointA = new CornerPoint(0, 0, canvasHeight, 0); //front bottom left
            CornerPoint pointB = new CornerPoint(length, 0, canvasHeight, 0); //front bottom right
            CornerPoint pointC = new CornerPoint(0, 0, canvasHeight, height); //front top left
            CornerPoint pointD = new CornerPoint(length, 0, canvasHeight, height); //front top right
            CornerPoint pointE = new CornerPoint(widthShift, 0, canvasHeight, widthShift); //back bottom left
            CornerPoint pointF = new CornerPoint(widthShift, -length, canvasHeight, widthShift); //back bottom right
            CornerPoint pointG = new CornerPoint(widthShift, 0, pointE.Y, height); //back top left
            CornerPoint pointH = new CornerPoint(widthShift, -length, pointF.Y, height); //back top right

            Dictionary<string, CornerPoint> CornerPoints = new Dictionary<string, CornerPoint>(){
                { "A", pointA },
                { "B", pointB },
                { "C", pointC },
                { "D", pointD },
                { "E", pointE },
                { "F", pointF },
                { "G", pointG },
                { "H", pointH } };
            
            return CornerPoints;
        }

        public Dictionary<string, CornerPoint> ShiftCornerPoints(double heightResized, double lenthResized, double widthShiftResized, double canvasHeight, double canvasWdth, Dictionary<string, CornerPoint> CornerPoints)
        {
            //Shift corner point to the center of the canvas
            Dictionary<string, CornerPoint> ShiftedCornerPoints = CornerPoints;

            double totalLengthResized = lenthResized + widthShiftResized;
            double horShift = (canvasWdth - totalLengthResized) / 2;

            double totalHeightResized = heightResized + widthShiftResized;
            double verShift = (canvasHeight - totalHeightResized) / 2;

            foreach (KeyValuePair<string, CornerPoint> pair in CornerPoints)
            {
                pair.Value.X += horShift;
                pair.Value.Y -= verShift;
            }

            return ShiftedCornerPoints;
        }
    }
}
