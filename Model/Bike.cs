using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KGLab4.Model
{
    public class Bike : INotifyPropertyChanged
    {
        private int _countCircle;

        public int CountCircle
        {
            get => _countCircle;
            set
            {
                _countCircle = value;
                OnPropertyChanged(nameof(CountCircle));
            }
        }

        private List<Path> _figureLines;

        public List<Path> FigureLines
        {
            get => _figureLines;
            set
            {
                _figureLines = value;
                OnPropertyChanged(nameof(FigureLines));
            }
        }

        private Color _colorLine;

        public Color ColorLine
        {
            get => _colorLine;
            set
            {
                _colorLine = value;
                OnPropertyChanged(nameof(ColorLine));
            }
        }

        private int _boldValue;

        public int BoldValue
        {
            get => _boldValue;
            set
            {
                _boldValue = value;
                OnPropertyChanged(nameof(BoldValue));
            }
        }

        private Point _pointCenter;

        public Point PointCenter
        {
            get => _pointCenter;
            set
            {
                _pointCenter = value;
                OnPropertyChanged(nameof(PointCenter));
            }
        }

        public void DrawBike()
        {
            if (FigureLines.Count != 0)
            {
                FigureLines.Clear();
            }
            for (var i = 0; i < CountCircle; i++)
            {
                DrawLine(new Point(Pedals[0, 0], Pedals[0, 1]), new Point(Pedals[1, 0], Pedals[1, 1]));
                DrawLine(new Point(Pedals[2, 0], Pedals[2, 1]), new Point(Pedals[3, 0], Pedals[3, 1]));

                DrawLine(new Point(Carcass[0, 0], Carcass[0, 1]), new Point(Carcass[1, 0], Carcass[1, 1]));
                DrawLine(new Point(Carcass[1, 0], Carcass[1, 1]), new Point(Carcass[2, 0], Carcass[2, 1]));

            }
        }

        public void Animate(int speed)
        {
            var cos = Math.Cos(Math.PI * speed / 180.0);
            var sin = Math.Sin(Math.PI * speed / 180.0);
            var matrixTransform = new double[3, 3];
            InitPedals();
            matrixTransform[0, 0] = cos; matrixTransform[0, 1] = sin; matrixTransform[0, 2] = 0;
            matrixTransform[1, 0] = -sin; matrixTransform[1, 1] = cos; matrixTransform[1, 2] = 0;
            matrixTransform[2, 0] = PointCenter.X; matrixTransform[2, 1] = PointCenter.Y; matrixTransform[2, 2] = 1;
            Pedals = MultiplyMatrix(Pedals, matrixTransform);
            DrawBike();
        }

        private double[,] Pedals;
        private double[,] Carcass;
        private double[,] MatrixShift;


        public Bike(Color colorLine, int boldValue, int countCircle, Point pointCenter)
        {
            _colorLine = colorLine;
            _boldValue = boldValue;
            _countCircle = countCircle;
            _pointCenter = pointCenter;
            FigureLines = new List<Path>();
            InitPedals();
            InitCarcass();
            Carcass = MultiplyMatrix(Carcass, InitMatrixShift());
        }

        private void InitPedals()
        {
            Pedals = new double[4, 3];
            Pedals[0, 0] = -10; Pedals[0, 1] = -10; Pedals[0, 2] = 1; // однородные координаты.
            Pedals[1, 0] = 10; Pedals[1, 1] = 10; Pedals[1, 2] = 1;
            Pedals[2, 0] = -10; Pedals[2, 1] = 10; Pedals[2, 2] = 1;
            Pedals[3, 0] = 10; Pedals[3, 1] = -10; Pedals[3, 2] = 1;
        }

        public void InitCarcass()
        {
            Carcass = new double[5, 3];
            Carcass[0, 0] = 0; Carcass[0, 1] = 0; Carcass[0, 2] = 1; // однородные координаты.
            Carcass[1, 0] = -20; Carcass[1, 1] = -20; Carcass[1, 2] = 1;
            Carcass[2, 0] = -50; Carcass[2, 1] = 10; Carcass[2, 2] = 1;
        }

        private double[,] InitMatrixShift()
        {
            var matrixShift = new double[3, 3];
            matrixShift[0, 0] = 1; matrixShift[0, 1] = 0; matrixShift[0, 2] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = 1; matrixShift[1, 2] = 0;
            matrixShift[2, 0] = PointCenter.X; matrixShift[2, 1] = PointCenter.Y; matrixShift[2, 2] = 1;
            return matrixShift;
        }

        private void DrawLine(Point startPoint, Point endPoint)
        {
            var line = new LineGeometry(startPoint, endPoint);
            var myPath1 = new Path
            {
                Stroke = new SolidColorBrush(ColorLine),
                StrokeThickness = BoldValue
            };
            var gp = new GeometryGroup();
            gp.Children.Add(line);
            myPath1.Data = gp;
            FigureLines.Add(myPath1);
        }

        private double[,] MultiplyMatrix(double[,] a, double[,] b)
        {
            var n = a.GetLength(0);
            var m = a.GetLength(1);

            var r = new double[n, m];
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    r[i, j] = 0;
                    for (var ii = 0; ii < m; ii++)
                    {
                        r[i, j] += a[i, ii] * b[ii, j];
                    }
                }
            }
            return r;
        }

        #region Notify

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
