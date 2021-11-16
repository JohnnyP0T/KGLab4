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
                _axesXTransform = value;
                OnPropertyChanged(nameof(AxesXTransform));
            }
        }
        private int _axesYTransform;

        public int AxesYTransform
        {
            get => _axesYTransform;
            set
            {
                _axesYTransform = value;
                OnPropertyChanged(nameof(AxesYTransform));
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


        public ViewModel()
        {
            CanvasView = new Canvas();
            BoldValue = 3;
            ColorLine = Colors.Red;
            FigureLines = new List<Path>();

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
                await Task.Run(() => Thread.Sleep(10));
                AxesXTransform -= 1;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                DrawSquare();
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand RightTransformCommand => new RelayCommand(async obj =>
        {
            while (RightButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(10));
                AxesXTransform += 1;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                DrawSquare();
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand UpTransformCommand => new RelayCommand(async obj =>
        {
            while (UpButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(10));
                AxesYTransform -= 1;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                DrawSquare();
            }
        }, obj => FigureLines.Count != 0);

        public RelayCommand DownTransformCommand => new RelayCommand(async obj =>
        {
            while (DownButtonIsChecked)
            {
                await Task.Run(() => Thread.Sleep(10));
                AxesYTransform += 1;
                foreach (var figureLine in FigureLines)
                {
                    CanvasView.Children.Remove(figureLine);
                }
                DrawSquare();
            }
        }, obj => FigureLines.Count != 0);
        #endregion

        private int[,] InitSquare()
        {
            var square = new int[4,3];
            square[0, 0] = -50; square[0, 1] = 0; square[0, 2] = 1; // однородные координаты.
            square[1, 0] = 0; square[1, 1] = 50; square[1, 2] = 1;
            square[2, 0] = 50; square[2, 1] = 0; square[2, 2] = 1;
            square[3, 0] = 0; square[3, 1] = -50; square[3, 2] = 1;
            return square;
        }
        //инициализация матрицы сдвига
        private int[,] InitMatrixTransform(int k1, int l1)
        {
            var matrixShift = new int[3,3];
            matrixShift[0, 0] = 1; matrixShift[0, 1] = 0; matrixShift[0, 2] = 0;
            matrixShift[1, 0] = 0; matrixShift[1, 1] = 1; matrixShift[1, 2] = 0;
            matrixShift[2, 0] = k1; matrixShift[2, 1] = l1; matrixShift[2, 2] = 1;
            return matrixShift;
        }

        private int[,] InitAxes()
        {
            var axes = new int[4,3];
            axes[0, 0] = -250; axes[0, 1] = 0; axes[0, 2] = 1;
            axes[1, 0] = 250; axes[1, 1] = 0; axes[1, 2] = 1;
            axes[2, 0] = 0; axes[2, 1] = 250; axes[2, 2] = 1;
            axes[3, 0] = 0; axes[3, 1] = -250; axes[3, 2] = 1;
            return axes;
        }

        private int[,] MultiplyMatrix(int[,] a, int[,] b)
        {
            var n = a.GetLength(0);
            var m = a.GetLength(1);

            var r = new int[n, m];
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

        private void DrawSquare()
        {
            var square = InitSquare();
            var axes = InitMatrixTransform(AxesXTransform, AxesYTransform);
            var square1 = MultiplyMatrix(square, axes);
            DrawLine(new Point(square1[0, 0], square1[0, 1]), new Point(square1[1, 0], square1[1, 1]), true);
            DrawLine(new Point(square1[1, 0], square1[1, 1]), new Point(square1[2, 0], square1[2, 1]), true);
            DrawLine(new Point(square1[2, 0], square1[2, 1]), new Point(square1[3, 0], square1[3, 1]), true);
            DrawLine(new Point(square1[3, 0], square1[3, 1]), new Point(square1[0, 0], square1[0, 1]), true);
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
            DrawLine(new Point(axis1[0,0], axis1[0, 1]), new Point(axis1[1,0], axis1[1,1]));
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
