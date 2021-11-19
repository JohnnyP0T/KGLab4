using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using KGLab4.Helpers;
//using Color = System.Drawing.Color;
using Point = System.Windows.Point;
using Color = System.Windows.Media.Color;

namespace KGLab4
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private Canvas _canvasView;

        public Canvas CanvasView
        {
            get => _canvasView;
            set
            {
                _canvasView = value;
                OnPropertyChanged(nameof(CanvasView));
            }
        }

        public int CanvasWidth => 500;
        public int CanvasHeight => 500;

        private int _axesXTransform;

        public int AxesXTransform
        {
            get => _axesXTransform;
            set
            {
                if (value > 500)
                {
                    _axesXTransform = 0;
                }
                else if (value < 0)
                {
                    _axesXTransform = 500;
                }
                else
                {
                    _axesXTransform = value;
                }
                OnPropertyChanged(nameof(AxesXTransform));
            }
        }
        private int _axesYTransform;

        public int AxesYTransform
        {
            get => _axesYTransform;
            set
            {
                if (value > 500)
                {
                    _axesYTransform = 0;
                }
                else if (value < 0)
                {
                    _axesYTransform = 500;
                }
                else
                {
                    _axesYTransform = value;
                }
                OnPropertyChanged(nameof(AxesYTransform));
            }
        }

        private int _speed;

        public int Speed
        {
            get => _speed;
            set
            {
                _speed = value;
            }
        }

        private int _scale;

        public int Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnPropertyChanged(nameof(Scale));
                if (FigureLines.Count != 0)
                {
                    foreach (var figureLine in FigureLines)
                    {
                        CanvasView.Children.Remove(figureLine);
                    }
                    DrawSquare();
                }
            }
        }

        private double _rotate;

        public double Rotate
        {
            get => _rotate;
            set
            {
                _rotate = value;
                OnPropertyChanged(nameof(Rotate));
                if (FigureLines.Count != 0)
                {
                    foreach (var figureLine in FigureLines)
                    {
                        CanvasView.Children.Remove(figureLine);
                    }
                    DrawSquare();
                }
            }
        }

        #region Buttons

        private bool _leftButtonIsChecked;

        public bool LeftButtonIsChecked
        {
            get => _leftButtonIsChecked;
            set
            {
                _leftButtonIsChecked = value;
                OnPropertyChanged(nameof(LeftButtonIsChecked));
            }
        }

        private bool _rightButtonIsChecked;

        public bool RightButtonIsChecked
        {
            get => _rightButtonIsChecked;
            set
            {
                _rightButtonIsChecked = value;
                OnPropertyChanged(nameof(_rightButtonIsChecked));
            }
        }

        private bool _upButtonIsChecked;

        public bool UpButtonIsChecked
        {
            get => _upButtonIsChecked;
            set
            {
                _upButtonIsChecked = value;
                OnPropertyChanged(nameof(UpButtonIsChecked));
            }
        }

        private bool _downButtonIsChecked;

        public bool DownButtonIsChecked
        {
            get => _downButtonIsChecked;
            set
            {
                _downButtonIsChecked = value;
                OnPropertyChanged(nameof(DownButtonIsChecked));
            }
        }

        #endregion

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

        private Point _pointDown;

        public Point PointDown
        {
            get => _pointDown;
            set
            {
                _pointDown = value;
                OnPropertyChanged(nameof(PointDown));
                RotationFigure((int)value.X, (int)value.Y);
            }
        }

        private async void RotationFigure(int m, int n)
        {
            var degrees = 0;
            while (!LeftButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                degrees += Speed;
                if (FigureLines.Count != 0)
                {
                    foreach (var figureLine in FigureLines)
                    {
                        CanvasView.Children.Remove(figureLine);
                    }
                    FigureLines.Clear();
                }
                var matrix = new double[3, 3];
                var cos = Math.Cos(Math.PI * degrees / 180.0);
                var sin = Math.Sin(Math.PI * degrees / 180.0);
                matrix[0, 0] = cos; matrix[0, 1] = sin; matrix[0, 2] = 0;
                matrix[1, 0] = -sin; matrix[1, 1] = cos; matrix[1, 2] = 0;
                matrix[2, 0] = -m*(cos - 1) + n*sin; matrix[2, 1] = -n*(cos - 1) - m*sin; matrix[2, 2] = 1;
                DrawSquare(matrix);
            }

            AxesXTransform = m;
            AxesYTransform = n;
        }

        public ViewModel()
        {
            CanvasView = new Canvas();
            BoldValue = 3;
            ColorLine = Colors.DeepSkyBlue;
            FigureLines = new List<Path>();
            Speed = 1;
            Scale = 1;
            Rotate = 0;
            DrawAxes();
            DrawFigureCommand.Execute(0);
        }

        #region Commands

        public RelayCommand ClearCommand => new RelayCommand(obj =>
        {
            CanvasView.Children.Clear();
        });

        public RelayCommand DrawAxesCommand => new RelayCommand(obj =>
        {
            DrawAxes();
        });
        public RelayCommand DrawFigureCommand => new RelayCommand(obj =>
        {
            AxesXTransform = CanvasWidth / 2;
            AxesYTransform = CanvasHeight / 2;
            DrawSquare();
        });

        public RelayCommand LeftTransformCommand => new RelayCommand(async obj =>
        {
            while (LeftButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                AxesXTransform -= Speed;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                FigureLines.Clear();
                DrawSquare();
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand RightTransformCommand => new RelayCommand(async obj =>
        {
            while (RightButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                AxesXTransform += Speed;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                FigureLines.Clear();
                DrawSquare();
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand UpTransformCommand => new RelayCommand(async obj =>
        {
            while (UpButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                AxesYTransform -= Speed;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                FigureLines.Clear();
                DrawSquare();
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand DownTransformCommand => new RelayCommand(async obj =>
        {
            while (DownButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(1));
                AxesYTransform += Speed;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                FigureLines.Clear();
                DrawSquare();
            }
        }, obj => FigureLines.Count != 0);
        #endregion

        private double[,] InitSquare()
        {
            var square = new double[5, 3];
            square[0, 0] = -50; square[0, 1] = -10; square[0, 2] = 1; // однородные координаты.
            square[1, 0] = -60; square[1, 1] = -40; square[1, 2] = 1;
            square[2, 0] = 10; square[2, 1] = -10; square[2, 2] = 1;
            square[3, 0] = 40; square[3, 1] = -60; square[3, 2] = 1;
            square[4, 0] = 0; square[4, 1] = 50; square[4, 2] = 1;

            return square;
        }

        private double[,] InitFigureCircle()
        {
            var square = new double[5, 3];
            square[0, 0] = -10; square[0, 1] = 0; square[0, 2] = 1; // однородные координаты.
            square[1, 0] = +10; square[1, 1] = 0; square[1, 2] = 1;
            square[2, 0] = 0; square[2, 1] = -10; square[2, 2] = 1;
            square[3, 0] = 0; square[3, 1] = +10; square[3, 2] = 1;
            square[4, 0] = 10; square[4, 1] = 10; square[4, 2] = 1;

            return square;
        }

        //инициализация матрицы сдвига
        private double[,] InitMatrixTransform(int k1, int l1)
        {
            var matrixShift = new double[3, 3];
            matrixShift[0, 0] = Scale; matrixShift[0, 1] = Rotate; matrixShift[0, 2] = 0;
            matrixShift[1, 0] = -Rotate; matrixShift[1, 1] = Scale; matrixShift[1, 2] = 0;
            matrixShift[2, 0] = k1; matrixShift[2, 1] = l1; matrixShift[2, 2] = Scale;
            return matrixShift;
        }

        private double[,] InitAxes()
        {
            var axes = new double[4, 3];
            axes[0, 0] = -CanvasHeight / 2; axes[0, 1] = 0; axes[0, 2] = 1;
            axes[1, 0] = CanvasHeight / 2; axes[1, 1] = 0; axes[1, 2] = 1;
            axes[2, 0] = 0; axes[2, 1] = CanvasWidth / 2; axes[2, 2] = 1;
            axes[3, 0] = 0; axes[3, 1] = -CanvasWidth / 2; axes[3, 2] = 1;
            return axes;
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

        private void DrawSquare(double[,]? matrix = null)
        {
            var square = InitSquare();
            var axes = InitMatrixTransform(AxesXTransform, AxesYTransform);
            if (matrix != null)
            {
                axes = matrix;
            }
            var square1 = MultiplyMatrix(square, axes);
            DrawLine(new Point(square1[0, 0], square1[0, 1]), new Point(square1[1, 0], square1[1, 1]), true);
            DrawLine(new Point(square1[1, 0], square1[1, 1]), new Point(square1[2, 0], square1[2, 1]), true);
            DrawLine(new Point(square1[2, 0], square1[2, 1]), new Point(square1[3, 0], square1[3, 1]), true);
            DrawLine(new Point(square1[3, 0], square1[3, 1]), new Point(square1[4, 0], square1[4, 1]), true);
            DrawLine(new Point(square1[4, 0], square1[4, 1]), new Point(square1[0, 0], square1[0, 1]), true);
        }

        private void DrawFigureCircle(double[,]? matrix = null)
        {
            var circle = InitFigureCircle();
            var axes = InitMatrixTransform(AxesXTransform, AxesYTransform);
        }

        private void DrawAxes()
        {
            var axes = InitAxes();
            var matrixTransform = InitMatrixTransform(CanvasWidth / 2, CanvasHeight / 2);
            var axis1 = MultiplyMatrix(axes, matrixTransform);
            var tempBold = BoldValue;
            var tempColor = ColorLine;
            ColorLine = Colors.Blue;
            BoldValue = 1;
            DrawLine(new Point(axis1[0, 0], axis1[0, 1]), new Point(axis1[1, 0], axis1[1, 1]));
            DrawLine(new Point(axis1[2, 0], axis1[2, 1]), new Point(axis1[3, 0], axis1[3, 1]));
            BoldValue = tempBold;
            ColorLine = tempColor;
        }

        private void DrawLine(Point startPoint, Point endPoint, bool IsFigure = false)
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
            CanvasView.Children.Add(myPath1);
            if (IsFigure)
            {
                FigureLines.Add(myPath1);
            }
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
